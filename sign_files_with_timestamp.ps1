# save as sign_with_timestamp.ps1
param(
    [string] $releasePath = "ByeByeDPI\bin\Release",
    [string] $pfxPasswordFile = "pfx_password.txt",
    [string] $pfxOut = "ByeByeDPI_files_codesign.pfx",
    [string] $certSubject = "CN=Ali SARIASLAN",
    [string] $signtoolPath = "signtool",   # or full path to signtool.exe
    [string] $timestampUrl = "http://timestamp.sectigo.com", # or http://timestamp.digicert.com
    [int] $delaySeconds = 20
)

# 1) Read password from text file
if (-not (Test-Path $pfxPasswordFile)) {
    throw "Password file not found: $pfxPasswordFile"
}
$pfxPassword = (Get-Content -Path $pfxPasswordFile -Raw).Trim()
if ([string]::IsNullOrWhiteSpace($pfxPassword)) {
    throw "Password file is empty or invalid."
}

# 2) Create a self-signed code signing certificate
Write-Host "Creating self-signed code signing certificate..."
$cert = New-SelfSignedCertificate -Type CodeSigningCert -Subject $certSubject -CertStoreLocation "Cert:\CurrentUser\My" -KeyExportPolicy Exportable -NotAfter (Get-Date).AddYears(2)
if (-not $cert) { throw "Certificate creation failed." }
Write-Host "Created cert with thumbprint: $($cert.Thumbprint)"

# 3) Export cert to PFX
Write-Host "Exporting PFX to $pfxOut ..."
$securePwd = ConvertTo-SecureString -String $pfxPassword -Force -AsPlainText
Export-PfxCertificate -Cert "Cert:\CurrentUser\My\$($cert.Thumbprint)" -FilePath $pfxOut -Password $securePwd | Out-Null
Write-Host "Exported PFX."

# 4) Build list of files to sign (only necessary executables, driver, DLL)
$files = @()
$files += Join-Path $releasePath "ByeByeDPI.exe"
$files += Join-Path $releasePath "goodbyedpi.exe"
$files += Join-Path $releasePath "WinDivert64.sys"
$files += Join-Path $releasePath "WinDivert.dll"

# Sadece var olan dosyaları filtrele
$files = $files | Where-Object { Test-Path $_ }

if ($files.Count -eq 0) {
    Write-Host "No matching files found in $releasePath"
    exit 0
}


# 5) Sign each file if not already signed
$results = @()
foreach ($filePath in $files) {
    $signature = Get-AuthenticodeSignature $filePath
    if ($signature.Status -eq 'Valid') {
        Write-Host "$filePath already signed. Skipping..."
        continue
    }

    Write-Host "Signing: $filePath"
    $args = @(
        "sign", "/f", $pfxOut, "/p", $pfxPassword,
        "/fd", "SHA256", "/td", "SHA256",
        "/tr", $timestampUrl, $filePath
    )

    $proc = Start-Process -FilePath $signtoolPath -ArgumentList $args -NoNewWindow -Wait -PassThru -ErrorAction SilentlyContinue
    if ($proc.ExitCode -ne 0) {
        Write-Warning "signtool returned exit code $($proc.ExitCode) for $filePath"
    }
    else {
        Write-Host "Signed OK."
    }

    $sha256 = (Get-FileHash -Algorithm SHA256 -Path $filePath).Hash
    $results += [PSCustomObject]@{
        File             = $filePath
        SHA256           = $sha256
        SigntoolExitCode = $proc.ExitCode
        TimestampServer  = $timestampUrl
        Time             = (Get-Date).ToString("o")
    }

    Write-Host "Waiting $delaySeconds seconds before next sign (to avoid TSA throttling)..."
    Start-Sleep -Seconds $delaySeconds
}

# 6) Save results CSV
$outCsv = Join-Path -Path $releasePath -ChildPath "signing_results_$(Get-Date -Format yyyyMMdd_HHmmss).csv"
$results | Export-Csv -Path $outCsv -NoTypeInformation -Encoding UTF8
Write-Host "Signing finished. Results written to $outCsv"
Write-Host "PFX exported at: $pfxOut"

# Optional: display certificate details
Write-Host "Certificate thumbprint: $($cert.Thumbprint)"
Write-Host "To trust this cert locally, import it into Trusted Root Certification Authorities."
