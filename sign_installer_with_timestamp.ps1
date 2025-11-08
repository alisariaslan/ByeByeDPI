# save as sign_installer.ps1
param(
    [string] $installerPath = "Output\ByeByeDPI_Installer.exe",
    [string] $pfxPasswordFile = "pfx_password.txt",
    [string] $pfxOut = "ByeByeDPI_Installer_codesign.pfx",
    [string] $certSubject = "CN=Ali SARIASLAN",
    [string] $signtoolPath = "signtool",   # veya signtool.exe tam yolu
    [string] $timestampUrl = "http://timestamp.sectigo.com", 
    [int] $delaySeconds = 5
)

# 1) Read password from text file
if (-not (Test-Path $pfxPasswordFile)) {
    throw "Password file not found: $pfxPasswordFile"
}
$pfxPassword = (Get-Content -Path $pfxPasswordFile -Raw).Trim()
if ([string]::IsNullOrWhiteSpace($pfxPassword)) {
    throw "Password file is empty or invalid."
}

# 2) Create self-signed code signing certificate
Write-Host "Creating self-signed code signing certificate..."
$cert = New-SelfSignedCertificate -Type CodeSigningCert -Subject $certSubject -CertStoreLocation "Cert:\CurrentUser\My" -KeyExportPolicy Exportable -NotAfter (Get-Date).AddYears(2)
if (-not $cert) { throw "Certificate creation failed." }
Write-Host "Created cert with thumbprint: $($cert.Thumbprint)"

# 3) Export cert to PFX
Write-Host "Exporting PFX to $pfxOut ..."
$securePwd = ConvertTo-SecureString -String $pfxPassword -Force -AsPlainText
Export-PfxCertificate -Cert "Cert:\CurrentUser\My\$($cert.Thumbprint)" -FilePath $pfxOut -Password $securePwd | Out-Null
Write-Host "Exported PFX."

# 4) Check if installer exists
if (-not (Test-Path $installerPath)) {
    Write-Host "Installer not found: $installerPath"
    exit 1
}

# 5) Sign the installer if not already signed
$signature = Get-AuthenticodeSignature $installerPath
if ($signature.Status -eq 'Valid') {
    Write-Host "$installerPath already signed. Skipping..."
}
else {
    Write-Host "Signing: $installerPath"
    $args = @(
        "sign", "/f", $pfxOut, "/p", $pfxPassword,
        "/fd", "SHA256", "/td", "SHA256",
        "/tr", $timestampUrl, $installerPath
    )

    $proc = Start-Process -FilePath $signtoolPath -ArgumentList $args -NoNewWindow -Wait -PassThru -ErrorAction SilentlyContinue
    if ($proc.ExitCode -ne 0) {
        Write-Warning "signtool returned exit code $($proc.ExitCode) for $installerPath"
    }
    else {
        Write-Host "Signed OK."
    }

    Start-Sleep -Seconds $delaySeconds
}

# 6) Save results CSV
$outCsv = Join-Path -Path (Split-Path $installerPath) -ChildPath "signing_result_$(Get-Date -Format yyyyMMdd_HHmmss).csv"
$sha256 = (Get-FileHash -Algorithm SHA256 -Path $installerPath).Hash
[PSCustomObject]@{
    File             = $installerPath
    SHA256           = $sha256
    SigntoolExitCode = if ($proc) { $proc.ExitCode } else { 0 }
    TimestampServer  = $timestampUrl
    Time             = (Get-Date).ToString("o")
} | Export-Csv -Path $outCsv -NoTypeInformation -Encoding UTF8

Write-Host "Signing finished. Results written to $outCsv"
Write-Host "PFX exported at: $pfxOut"
Write-Host "Certificate thumbprint: $($cert.Thumbprint)"
Write-Host "To trust this cert locally, import it into Trusted Root Certification Authorities."
