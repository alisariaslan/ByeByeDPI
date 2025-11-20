Clear-Host
Write-Host "-----------------------------------------------"
Write-Host "create_commit_with_ai_v2.ps1 (Per-File AI Analysis)"
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
# Filter out empty lines AND exclude files ending with .g.dart or .freezed.dart
$staged_files = git diff --cached --name-only | 
    Where-Object { 
        $_ -ne "" -and 
        $_ -notlike "*.g.dart" -and 
        $_ -notlike "*.freezed.dart" 
    }

# If nothing staged, ask user if they want to stage all
if (-not $staged_files) {
    if ($unstaged) {
        Write-Host "⚠️  No staged changes found, but there are unstaged files:`n"
        Write-Host $unstaged -ForegroundColor Yellow
        $stageAnswer = Read-Host "`nDo you want to stage ALL changes? (y/n)"
        if ($stageAnswer -eq "y") {
            git add -A
            Write-Host "`n✅ All changes staged."
            # Re-fetch staged files, applying the same filter
            $staged_files = git diff --cached --name-only | 
                Where-Object { 
                    $_ -ne "" -and 
                    $_ -notlike "*.g.dart" -and 
                    $_ -notlike "*.freezed.dart" 
                }

            if (-not $staged_files) {
                Write-Host "`n⚠️ All files were staged, but only auto-generated files remained for processing. No files to analyze. Exiting." -ForegroundColor DarkYellow
                exit
            }
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

Write-Host "`n🔍 Analyzing staged files with $model (Ollama)...`n"

# Analyze staged changes file-by-file
$combined_commit_messages = @()
$display_output = "" # Holds the output to be displayed on screen

foreach ($file in $staged_files) {
    Write-Host "Processing file: **$file**" -ForegroundColor Yellow

    # Get diff content only for the current file
    $file_diff = git diff --cached -- "$file"

    if (-not $file_diff) {
        Write-Host "   ⚠️ Could not get staged diff for **$file**. Skipping." -ForegroundColor DarkYellow
        continue
    }

    # Prepare AI prompt
    $prompt = "Generate a concise, single-line description of the changes in this file diff. Do not include the file name in the output. Diff for '$file':`n$file_diff"

    # Generate commit message using Ollama
    # Suppress error output by redirecting to $null
    $ai_response = & ollama run $model "$prompt" 2>$null

    if (-not $ai_response) {
        $message = "❌ Failed to get response for **$file**."
        Write-Host $message -ForegroundColor Red
        # Fix: Delimit variable with curly braces
        $combined_commit_messages += "* ${file}: Failed to generate AI message."
        $display_output += "* **$file**: $message`n"
    }
    else {
        $message = ($ai_response | Out-String).Trim()
        
        # Clean up unnecessary quotes
        $message = $message.Trim('"').Trim("'")

        Write-Host "   ✅ Result: $message" -ForegroundColor Green
        
        # Fix: Delimit variable with curly braces
        # Format for the final combined message (as a bulleted list)
        $combined_commit_messages += "* ${file}: $message"
        
        # Format for screen display
        $display_output += "* **$file**: $message`n"
    }
}

# Create the final main commit message
$final_commit_message_list = $combined_commit_messages -join "`n"

# Optionally, send the list back to Ollama to generate a single summary message
Write-Host "`n🧠 Generating final summary commit message..."
$summary_prompt = "Generate a concise, high-level git commit message summarizing the following list of changes (Do not include the list itself in the final message):`n$final_commit_message_list"
$summary_message = & ollama run $model "$summary_prompt" 2>$null
$summary_message = ($summary_message | Out-String).Trim().Trim('"').Trim("'")

# --- DISPLAY RESULTS ---
Write-Host "`n============================================"
Write-Host "  ✅ AI-GENERATED COMMIT ANALYSIS" -ForegroundColor Cyan
Write-Host "============================================"

Write-Host "`n## Per-File Analysis Results (Data Sent and Result Received)`n"
Write-Host $display_output

Write-Host "`n---"
Write-Host "## 📝 Suggested Main Commit Message (Summary)" -ForegroundColor Cyan
Write-Host $summary_message -ForegroundColor Green
Write-Host "---`n"

if (-not $summary_message) {
    Write-Host "`n❌ Final summary message generation failed. Exiting."
    exit
}

# Confirm commit
$answer = Read-Host "Do you want to commit with this summary message? (y/n)"
if ($answer -eq "y") {
	# Trim and remove quotes
	$commit_message = $summary_message.Trim('"')

	# Escape any remaining double quotes just in case
	$commit_message = $commit_message -replace '"','\"'

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