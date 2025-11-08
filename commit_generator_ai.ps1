Clear-Host
Write-Host "-----------------------------------------------"
Write-Host "create_commit_with_ai.ps1"
Write-Host "Automatically generate a git commit message using Ollama AI"
Write-Host "-----------------------------------------------`n"

# --- CONFIGURATION ---
# Model to use
$model = "codellama:7b"
# ----------------------

# Check if git is installed
if (-not (Get-Command git -ErrorAction SilentlyContinue)) {
    Write-Host "❌ Git is not installed or not in PATH."
    exit
}

# Get unstaged and staged files
$unstaged = git diff --name-only
$staged = git diff --cached

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

# Prepare prompt
$changes = ($staged) -join ", "
$prompt = "Generate git commit message for these: $changes"

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
    git commit -m "$($commit_message)"
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
