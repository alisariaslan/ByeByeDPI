Clear-Host
Write-Host "-----------------------------------------------"
Write-Host "create_commit_with_ai.ps1"
Write-Host "Automatically generate a git commit message using Ollama CodeLlama:7b"
Write-Host "-----------------------------------------------`n"

# Check if git is installed
if (-not (Get-Command git -ErrorAction SilentlyContinue)) {
    Write-Host "❌ Git is not installed or not in PATH."
    Read-Host "Press Enter to exit"
    exit
}

# Get unstaged and staged files
$unstaged = git diff --name-only
$staged = git diff --cached --name-only

# If nothing staged, ask user if they want to stage all
if (-not $staged) {
    if ($unstaged) {
        Write-Host "⚠️  No staged changes found, but there are unstaged files:`n"
        Write-Host $unstaged -ForegroundColor Yellow
        $stageAnswer = Read-Host "`nDo you want to stage ALL changes? (y/n)"
        if ($stageAnswer -eq "y") {
            git add -A
            Write-Host "`n✅ All changes staged."
            $staged = git diff --cached --name-only
        } else {
            Write-Host "`n❌ No files staged. Exiting."
            Read-Host "Press Enter to exit"
            exit
        }
    } else {
        Write-Host "⚠️  No modified or staged files found. Nothing to commit."
        Read-Host "Press Enter to exit"
        exit
    }
}

# Prepare prompt
$changes = ($staged) -join ", "
$prompt = "Generate a concise git commit message for the following changed files: $changes"

# Try with 'generate', fallback to 'run' if needed
Write-Host "`n🤖 Generating commit message with CodeLlama..."
$commit_message = & ollama generate codellama:7b --no-stream --prompt $prompt --max-tokens 100 2>$null

if (-not $commit_message) {
    Write-Host "⚠️  No response from 'generate', trying fallback method..."
    $commit_message = & ollama run codellama:7b "$prompt" 2>$null
}

if (-not $commit_message) {
    Write-Host "`n❌ Failed to get response from Ollama."
    Read-Host "Press Enter to exit"
    exit
}

# Display result
$commit_message = ($commit_message | Out-String).Trim()
Write-Host "`nAI-generated commit message:"
Write-Host "--------------------------------------------"
Write-Host $commit_message -ForegroundColor Cyan
Write-Host "--------------------------------------------`n"

# Confirm commit
$answer = Read-Host "Do you want to commit with this message? (y/n)"
if ($answer -eq "y") {
    git commit -m "$($commit_message)"
    Write-Host "`n✅ Commit created successfully."

    # Ask to push
    $pushAnswer = Read-Host "`nDo you want to push the commit to remote? (y/n)"
    if ($pushAnswer -eq "y") {
        Write-Host "`n🚀 Pushing to remote..."
        git push
        if ($LASTEXITCODE -eq 0) {
            Write-Host "`n✅ Push completed successfully."
        } else {
            Write-Host "`n⚠️ Push failed. Please check your connection or credentials."
        }
    } else {
        Write-Host "`nPush skipped."
    }
} else {
    Write-Host "`nCommit cancelled."
}
