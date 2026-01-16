import os
import subprocess
import json
import sys
import tempfile
import requests
from datetime import datetime, timedelta

from config import (
    LM_STUDIO_API_URL,
    MODEL,
    LM_STUDIO_BEARER_TOKEN
)
class Colors:
    HEADER = '\033[95m'
    OKBLUE = '\033[94m'
    OKCYAN = '\033[96m'
    OKGREEN = '\033[92m'
    WARNING = '\033[93m'
    FAIL = '\033[91m'
    ENDC = '\033[0m'
    BOLD = '\033[1m'

def run_git_command(command_parts):
    try:
        result = subprocess.run(
            ['git'] + command_parts,
            capture_output=True,
            text=True,
            check=True,
            encoding='utf-8'
        )
        return result.stdout.strip()
    except Exception:
        return None

def invoke_lm_studio_ai(prompt: str) -> str | None:
    body = {
        "model": MODEL,
        "messages": [
            {
                "role": "system",
                "content": "You are a professional technical writer. Create clear, categorized release notes in English."
            },
            {"role": "user", "content": prompt}
        ],
        "temperature": 0.5,
        "max_tokens": 2048,
        "stream": False
    }

    headers = {"Content-Type": "application/json"}
    if LM_STUDIO_BEARER_TOKEN:
        headers["Authorization"] = f"Bearer {LM_STUDIO_BEARER_TOKEN}"

    try:
        response = requests.post(
            LM_STUDIO_API_URL,
            headers=headers,
            json=body,
            timeout=600
        )
        response.raise_for_status()
        data = response.json()
        return data["choices"][0]["message"]["content"].strip()
    except Exception as e:
        print(f"{Colors.FAIL}API Error: {e}{Colors.ENDC}")
        return None

def select_day_range():
    print(f"""{Colors.OKBLUE}
Select commit time range:
1) Today
2) Last 7 days
3) Last 30 days
4) Custom (enter number of days)
{Colors.ENDC}""")

    choice = input("Choice (1-4): ").strip()
    now = datetime.now()

    if choice == "2":
        days = 7
    elif choice == "3":
        days = 30
    elif choice == "4":
        try:
            days = int(input("Enter number of days: ").strip())
            if days <= 0:
                raise ValueError
        except ValueError:
            print(f"{Colors.WARNING}Invalid number. Defaulting to 1 day.{Colors.ENDC}")
            days = 1
    else:
        days = 1

    return f"Last {days} day(s)", now - timedelta(days=days)

def select_branch_scope():
    print(f"""{Colors.OKBLUE}
Select branch scope:
1) Current branch
2) All branches
{Colors.ENDC}""")

    choice = input("Choice (1-2): ").strip()

    if choice == "1":
        return "CURRENT branch", []
    else:
        return "ALL branches", ["--all"]

def main():
    print(f"{Colors.HEADER}=== Release Notes Generator ==={Colors.ENDC}\n")

    range_title, since_date = select_day_range()
    branch_title, branch_args = select_branch_scope()

    since_str = since_date.strftime('%Y-%m-%d %H:%M:%S')

    print(
        f"{Colors.OKCYAN}Collecting commits ({range_title}) from {branch_title}...{Colors.ENDC}"
    )

    git_args = [
        "log",
        f"--since={since_str}",
        "--oneline"
    ] + branch_args

    commits = run_git_command(git_args)

    if not commits:
        print(f"{Colors.WARNING}No commits found for {range_title}.{Colors.ENDC}")
        return

    prompt = f"""
Generate professional Release Notes in English based on the following Git commits.

Time Range: {range_title}
Branch Scope: {branch_title}

Categorize into:
- 🚀 Features
- 🐞 Bug Fixes
- 🛠️ Maintenance & Refactoring

Commits:
{commits}

Rules:
- Do NOT show commit hashes
- Use bullet points
- Be concise and professional
"""

    release_notes = invoke_lm_studio_ai(prompt)

    if not release_notes:
        print(f"{Colors.FAIL}Failed to generate release notes.{Colors.ENDC}")
        return

    json_version = json.dumps(release_notes)

    file_content = f"""RELEASE NOTES
Time Range: {range_title}
Branch Scope: {branch_title}
Generated at: {datetime.now().strftime('%Y-%m-%d %H:%M')}

{"=" * 40}

{release_notes}

{"-" * 40}
JSON SINGLE-LINE VERSION:
{json_version}
"""

    try:
        with tempfile.NamedTemporaryFile(
            delete=False,
            suffix=".txt",
            mode="w",
            encoding="utf-8"
        ) as f:
            f.write(file_content)
            temp_path = f.name

        print(f"{Colors.OKGREEN}Done! Opening release notes...{Colors.ENDC}")

        if sys.platform == "win32":
            os.startfile(temp_path)
        elif sys.platform == "darwin":
            subprocess.run(["open", temp_path])
        else:
            subprocess.run(["xdg-open", temp_path])

    except Exception as e:
        print(f"{Colors.FAIL}File error: {e}{Colors.ENDC}")

if __name__ == "__main__":
    main()
