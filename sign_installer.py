# sign_installer.py
import subprocess
import time
from pathlib import Path
from datetime import datetime
import config  # burada config.py'de PFX_PASSWORD tanımlı olmalı
import sys

# ---------------- CONFIG ----------------
installer_path = Path("Output/HelloClipboard_Installer.exe")
pfx_out = Path("HelloClipboard_Installer_codesign.pfx")
cert_subject = "CN=Ali SARIASLAN"
signtool_path = "signtool"  # veya signtool.exe tam yolu
timestamp_url = "http://timestamp.sectigo.com"
delay_seconds = 5

# ---------------- HELPERS ----------------
def run(cmd):
    print("> " + " ".join(cmd))
    proc = subprocess.run(cmd, capture_output=True, text=True)
    if proc.returncode != 0:
        print(proc.stdout)
        print(proc.stderr)
        raise RuntimeError(f"Command failed with exit code {proc.returncode}")
    return proc.stdout.strip()

# ---------------- 1) PFX Şifresi ----------------
pfx_password = config.PFX_PASSWORD
if not pfx_password:
    raise ValueError("PFX_PASSWORD is empty in config.py")

# ---------------- 2) Self-signed sertifika oluştur ----------------
print("Creating self-signed code signing certificate...")
# PowerShell üzerinden sertifika oluşturuyoruz
create_cert_cmd = [
    "powershell",
    "-Command",
    f"""
    $cert = New-SelfSignedCertificate -Type CodeSigningCert -Subject '{cert_subject}' `
        -CertStoreLocation 'Cert:\\CurrentUser\\My' -KeyExportPolicy Exportable -NotAfter (Get-Date).AddYears(2)
    Write-Output $cert.Thumbprint
    """
]
thumbprint = run(create_cert_cmd)
print(f"Created cert with thumbprint: {thumbprint}")

# ---------------- 3) Sertifikayı PFX olarak dışa aktar ----------------
print(f"Exporting PFX to {pfx_out} ...")
export_pfx_cmd = [
    "powershell",
    "-Command",
    f"""
    $pwd = ConvertTo-SecureString -String '{pfx_password}' -Force -AsPlainText
    Export-PfxCertificate -Cert 'Cert:\\CurrentUser\\My\\{thumbprint}' -FilePath '{pfx_out}' -Password $pwd
    """
]
run(export_pfx_cmd)
print("Exported PFX.")

# ---------------- 4) Installer var mı kontrol et ----------------
if not installer_path.exists():
    print(f"Installer not found: {installer_path}")
    sys.exit(1)

# ---------------- 5) Installer imzala ----------------
# Önce imza durumu kontrol edelim
check_sig_cmd = [
    "powershell",
    "-Command",
    f"""
    (Get-AuthenticodeSignature '{installer_path}').Status
    """
]
signature_status = run(check_sig_cmd)
if signature_status == "Valid":
    print(f"{installer_path} already signed. Skipping...")
else:
    print(f"Signing: {installer_path}")
    signtool_args = [
        signtool_path, "sign",
        "/f", str(pfx_out),
        "/p", pfx_password,
        "/fd", "SHA256",
        "/td", "SHA256",
        "/tr", timestamp_url,
        str(installer_path)
    ]
    proc = subprocess.run(signtool_args)
    if proc.returncode != 0:
        print(f"signtool returned exit code {proc.returncode} for {installer_path}")
    else:
        print("Signed OK.")
    time.sleep(delay_seconds)

# ---------------- 6) Sonuçları CSV olarak kaydet ----------------
import csv
import hashlib

sha256 = hashlib.sha256(installer_path.read_bytes()).hexdigest()
out_csv = installer_path.parent / f"signing_result_{datetime.now():%Y%m%d_%H%M%S}.csv"

with open(out_csv, "w", newline="", encoding="utf-8") as f:
    writer = csv.DictWriter(f, fieldnames=["File", "SHA256", "SigntoolExitCode", "TimestampServer", "Time"])
    writer.writeheader()
    writer.writerow({
        "File": str(installer_path),
        "SHA256": sha256,
        "SigntoolExitCode": proc.returncode if 'proc' in locals() else 0,
        "TimestampServer": timestamp_url,
        "Time": datetime.now().isoformat()
    })

print(f"Signing finished. Results written to {out_csv}")
print(f"PFX exported at: {pfx_out}")
print(f"Certificate thumbprint: {thumbprint}")
print("To trust this cert locally, import it into Trusted Root Certification Authorities.")
