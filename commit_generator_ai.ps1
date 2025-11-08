Clear-Host
Write-Host "-----------------------------------------------"
Write-Host "create_commit_with_ai.ps1"
Write-Host "Automatically generate a git commit message using Ollama AI"
Write-Host "-----------------------------------------------`n"

# --- CONFIGURATION ---
$model = "mistral:latest"
# ----------------------

# Check if git is installed
if (-not (Get-Command git -ErrorAction SilentlyContinue)) {
    Write-Host "❌ Git is not installed or not in PATH."
    exit
}

# Get unstaged and staged files
$unstaged = git diff --name-only
$staged_files = git diff --cached --name-only

# If nothing staged, ask user if they want to stage all
if (-not $staged_files) {
    if ($unstaged) {
        Write-Host "⚠️  No staged changes found, but there are unstaged files:`n"
        Write-Host $unstaged -ForegroundColor Yellow
        $stageAnswer = Read-Host "`nDo you want to stage ALL changes? (y/n)"
        if ($stageAnswer -eq "y") {
            git add -A
            Write-Host "`n✅ All changes staged."
            $staged_files = git diff --cached --name-only
        }
        else {
            Write-Host "`n❌ No files staged. Exiting."
            exit
        }
    }
    else {
        Write-Host "⚠️  No modified or staged files found. Nothing to commit."
        exit
    }
}

# Get staged diff content
$staged_diff = git diff --cached
$diff_string = ($staged_diff | Out-String).Trim()

# Prepare AI prompt
$prompt = "Generate a concise git commit message based on the following staged diff:`n$diff_string"

# Generate commit message using Ollama
$commit_message = & ollama run $model "$prompt" 2>$null

if (-not $commit_message) {
    Write-Host "`n❌ Failed to get response from Ollama."
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
    # Convert multi-line response to a single line and escape quotes
    $commit_message = $commit_message -replace "`r`n"," "
    $commit_message = $commit_message -replace '"', '\"'

    # Commit
    git commit -m "$commit_message"
    Write-Host "`n✅ Commit created successfully."

    # Ask to push
    $pushAnswer = Read-Host "`nDo you want to push the commit to remote? (y/n)"
    if ($pushAnswer -eq "y") {
        Write-Host "`n🚀 Pushing to remote..."
        git push
        if ($LASTEXITCODE -eq 0) {
            Write-Host "`n✅ Push completed successfully."
        }
        else {
            Write-Host "`n⚠️ Push failed. Please check your connection or credentials."
        }
    }
    else {
        Write-Host "`nPush skipped."
    }
}
else {
    Write-Host "`nCommit cancelled."
}
