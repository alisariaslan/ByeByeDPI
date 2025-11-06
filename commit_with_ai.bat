@echo off
REM -----------------------------------------------
REM create_commit_with_ai.bat (stable version)
REM -----------------------------------------------

git --version >nul 2>&1
IF %ERRORLEVEL% NEQ 0 (
    echo Git is not installed or not in PATH.
    pause
    exit /b
)

git status --porcelain > temp_status.txt
setlocal enabledelayedexpansion
set "unstaged_changes="

for /f "tokens=1*" %%a in (temp_status.txt) do (
    if "%%a"=="??" (
        set "unstaged_changes=!unstaged_changes!%%b "
    ) else (
        if not "%%a"=="A" (
            set "unstaged_changes=!unstaged_changes!%%b "
        )
    )
)

if defined unstaged_changes (
    echo Unstaged changes detected:
    echo ----------------------------
    echo !unstaged_changes!
    echo ----------------------------
    set /p stage_confirm=Do you want to stage all changes? (y/n): 
    if /I "!stage_confirm!"=="y" (
        git add .
        echo All changes staged.
    ) else (
        echo Cannot proceed without staging changes.
        del temp_status.txt
        pause
        exit /b
    )
)

git diff --cached --name-only > temp_changes.txt
set "changes="
for /f "usebackq delims=" %%f in ("temp_changes.txt") do (
    set "changes=!changes! %%f"
)

if "%changes%"=="" (
    echo No staged changes found.
    del temp_status.txt
    del temp_changes.txt
    pause
    exit /b
)

set "prompt=Generate a concise git commit message for the following changed files: %changes%"

echo Generating commit message with Ollama...
ollama generate codellama:7b --prompt "%prompt%" --max-tokens 100 > temp_commit.txt

REM Read only the first line (strip newlines safely)
set /p commit_message=<temp_commit.txt

del temp_status.txt
del temp_changes.txt
del temp_commit.txt

echo.
echo AI-generated commit message:
echo ----------------------------
echo %commit_message%
echo ----------------------------
echo.

set /p confirm=Do you want to commit with this message? (y/n): 
if /I "%confirm%"=="y" (
    git commit -m "%commit_message%"
    echo Commit done!
) else (
    echo Commit canceled.
)

pause
