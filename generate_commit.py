import os
import subprocess
import json
import sys
import re
import requests

# Simple class for colored output (may not work on all systems)
class Colors:
    """ANSI codes for coloring console output."""
    HEADER = '\033[95m'
    OKBLUE = '\033[94m'
    OKCYAN = '\033[96m'
    OKGREEN = '\033[92m'
    WARNING = '\033[93m'
    FAIL = '\033[91m'
    ENDC = '\033[0m'
    BOLD = '\033[1m'
    
    # Additional colors for potential PowerShell compatibility
    DARK_YELLOW = '\033[33m' 
    RED = FAIL
    YELLOW = WARNING
    GREEN = OKGREEN
    CYAN = OKCYAN


def print_header():
    """Prints the header message."""
    print("-" * 50)
    print("lm_commit_generator.py (LM Studio Supported)")
    print("Automatically generate a git commit message using LM Studio AI")
    print("-" * 50 + "\n")

from config import (
    LM_STUDIO_API_URL,
    MODEL,
    LM_STUDIO_BEARER_TOKEN
)
def run_git_command(command_parts, ignore_errors=False):
    """Runs git commands and returns the output."""
    try:
        result = subprocess.run(
            ['git'] + command_parts, 
            capture_output=True, 
            text=True, 
            check=not ignore_errors,
            # Using 'utf-8' instead of default to mitigate UTF-8 issues on Windows
            encoding='utf-8' 
        )
        return result.stdout.strip()
    except subprocess.CalledProcessError as e:
        if not ignore_errors:
            print(f"{Colors.RED}--- Git command failed: {' '.join(command_parts)}{Colors.ENDC}")
            print(f"Error: {e.stderr.strip()}")
        return None
    except FileNotFoundError:
        if not ignore_errors:
            print(f"{Colors.RED}--- Git is not installed or not found in PATH.{Colors.ENDC}")
            sys.exit(1)
        return None

def invoke_lm_studio_ai(prompt: str, model: str, max_tokens: int = 1024) -> str | None:
    """Sends a request to the LM Studio API."""
    
    body = {
        "model": model,
        "messages": [
            {"role": "system", "content": "You are a concise AI code analyst."},
            {"role": "user", "content": prompt}
        ],
        "temperature": 0.3,
        "max_tokens": max_tokens,
        "stream": False
    }

    headers = {
        "Content-Type": "application/json"
    }

    if LM_STUDIO_BEARER_TOKEN:
        headers["Authorization"] = f"Bearer {LM_STUDIO_BEARER_TOKEN}"

    try:
        response = requests.post(LM_STUDIO_API_URL, headers=headers, json=body, timeout=180)
        response.raise_for_status()

        data = response.json()

        if data.get("choices") and len(data["choices"]) > 0:
            message = data["choices"][0]["message"]["content"].strip()
            return message.strip('"').strip("'")
        else:
            return None
    except requests.exceptions.RequestException as e:
        print(f"{Colors.RED}--- API request failed: {e}{Colors.ENDC}")
        return None
    except Exception as e:
        print(f"{Colors.RED}--- An unexpected error occurred: {e}{Colors.ENDC}")
        return None


def filter_staged_files(file_list: str) -> list[str]:
    """Filters Git output and excludes unwanted file types."""
    if not file_list:
        return []
    
    filtered_files = []
    for file in file_list.splitlines():
        file = file.strip()
        if not file:
            continue
            
        # Unwanted extensions
        if any(file.endswith(ext) for ext in [".g.dart", ".freezed.dart", ".csproj", ".Designer.cs"]):
            continue
            
        filtered_files.append(file)
        
    return filtered_files


def main():
    """Main program flow."""
    
    # Force output encoding to resolve output encoding errors (Unicode/Emoji issues) on Windows
    try:
        sys.stdout.reconfigure(encoding='utf-8')
        sys.stderr.reconfigure(encoding='utf-8')
    except Exception:
        # Continue even if there is an error (e.g., old Python)
        pass

    print_header()

    # Check if Git is installed
    if run_git_command(['--version'], ignore_errors=True) is None:
        sys.exit(1)

    # Get unstaged and staged files
    unstaged_output = run_git_command(['diff', '--name-only'])
    staged_output = run_git_command(['diff', '--cached', '--name-only'])
    
    unstaged_files = unstaged_output.splitlines() if unstaged_output else []
    staged_files = filter_staged_files(staged_output)
    
    # If no files are staged, ask the user to stage all
    if not staged_files:
        if unstaged_files:
            print(f"{Colors.WARNING}--- No changes are staged. Unstaged files exist:{Colors.ENDC}\n")
            print('\n'.join(unstaged_files) + f"{Colors.ENDC}")
            
            stage_answer = input("\nDo you want to stage all changes? (y/n): ").strip().lower()
            
            if stage_answer == "y":
                run_git_command(['add', '-A'])
                print(f"\n{Colors.OKGREEN}*** All changes staged. ***{Colors.ENDC}")
                
                # Get files again
                staged_output = run_git_command(['diff', '--cached', '--name-only'])
                staged_files = filter_staged_files(staged_output)
                
                if not staged_files:
                    print(f"\n{Colors.DARK_YELLOW}--- All staged files were auto-generated. No files left to analyze. Exiting.{Colors.ENDC}")
                    sys.exit(0)
            else:
                print("\n--- No files staged. Exiting.")
                sys.exit(0)
        else:
            print(f"{Colors.WARNING}--- No modified or staged files found. Nothing to commit.{Colors.ENDC}")
            sys.exit(0)


    print(f"\n{Colors.OKCYAN}+++ Analyzing staged files with {MODEL} (LM Studio)...{Colors.ENDC}\n")

    display_output = "" # Output to be shown on screen
    
    # Stage 1: Generate message for each file
    for file in staged_files:
        print(f"Processing file: {Colors.WARNING}{file}{Colors.ENDC}")

        file_diff = run_git_command(['diff', '--cached', '--', file])

        if not file_diff:
            print(f"   {Colors.DARK_YELLOW}--- Could not get staged diff for {file}. Skipping.{Colors.ENDC}")
            display_output += f"* **{file}**: Could not get diff. Skipped.\n"
            continue

        prompt = f"""
Analyze the git diff below for a single file.

Create a SINGLE concise summary following the Conventional Commits style:

<type>: <summary>

Allowed types:
- feat (new feature)
- fix (bug fix)
- refactor (code change that neither fixes a bug nor adds a feature)
- perf (performance improvement)
- docs (documentation)
- test (tests)
- chore (tooling, configuration, maintenance)

Rules:
- Use only a single line
- Type must be lowercase
- Do not mention file names
- Do not add a body/description
- Maximum 72 characters

Diff:
{file_diff}
"""

        ai_response = invoke_lm_studio_ai(prompt, MODEL)

        if not ai_response:
            message = "--- Response could not be obtained. (API or Model Error)"
            print(f"   {Colors.RED}{message}{Colors.ENDC}")
            display_output += f"* **{file}**: {message}\n"
        else:
            message = ai_response
            
            # Check and enforce compliance with the rule
            if not re.match(r'^(feat|fix|refactor|perf|docs|test|chore):\s+', message):
                message = f"chore: {message}"

            print(f"   {Colors.OKGREEN}*** Result: {message}{Colors.ENDC}")
            display_output += f"* **{file}**: {message}\n"


    # Stage 2: Create the final summary message
    print(f"\n{Colors.OKCYAN}+++ Generating final summary commit message...{Colors.ENDC}")

    summary_context = display_output
    if len(summary_context) > 2000:
        summary_context = summary_context[:2000]

    summary_prompt = f"""
Below is a list of change summaries per file.

Create a SINGLE git commit message in the Conventional Commits format:

<type>: <summary>

Rules:
- Total maximum 72 characters
- Lowercase type
- Do not mention file names
- Do not add a body/description
- RETURN ONLY the commit message

Changes:
{summary_context}
"""

    summary_message = invoke_lm_studio_ai(
        prompt=summary_prompt, 
        model=MODEL, 
        max_tokens=2048
    )

    # --- SHOW RESULTS ---
    print("\n" + "=" * 50)
    print(f" {Colors.OKCYAN}*** AI GENERATED COMMIT ANALYSIS ***{Colors.ENDC}")
    print("=" * 50)

    print(f"\n## Per-File Analysis Results\n")
    print(display_output)

    print("\n" + "-" * 50)
    print(f"## Suggested Main Commit Message (Summary){Colors.OKCYAN}")
    
    if not summary_message:
        print(f"{Colors.WARNING}--- Summary message could not be generated. Using default.{Colors.ENDC}")
        summary_message = "chore: update multiple files"
    
    # Final check and enforcement
    if not re.match(r'^(feat|fix|refactor|perf|docs|test|chore):\s+', summary_message):
        summary_message = f"chore: {summary_message}"

    print(f"{Colors.OKGREEN}{summary_message}{Colors.ENDC}")
    print("-" * 50)

    # Commit confirmation
    answer = input("\nDo you want to commit with this summary message? (y/n): ").strip().lower()
    if answer == "y":
        commit_message = summary_message.strip('"').strip("'").strip()
        
        # Commit
        run_git_command(['commit', '-m', commit_message])

        print(f"\n{Colors.OKGREEN}*** Commit successfully created. ***{Colors.ENDC}")

        # Push confirmation
        push_answer = input("\nDo you want to push the commit to the remote repository? (y/n): ").strip().lower()
        if push_answer == "y":
            print(f"\n{Colors.OKCYAN}+++ Pushing to remote...{Colors.ENDC}")
            
            push_result = run_git_command(['push'], ignore_errors=True)
            
            if push_result is not None: 
                print(f"\n{Colors.OKGREEN}*** Push completed successfully. ***{Colors.ENDC}")
            else:
                print(f"\n{Colors.WARNING}--- Push failed. Check your connection or credentials.{Colors.ENDC}")
        else:
            print("\nPush skipped.")
    else:
        print("\nCommit cancelled.")

if __name__ == "__main__":
    try:
        main()
    except KeyboardInterrupt:
        print("\nOperation cancelled by user.")
        sys.exit(0)