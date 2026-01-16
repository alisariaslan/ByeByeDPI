import os
import subprocess
import time
import hashlib
from datetime import datetime
from config import PFX_PASSWORD

# --- CONFIGURATION ---
release_path = r"HelloClipboard\bin\Release\net10.0-windows"
pfx_out = r"HelloClipboard_files_codesign.pfx"
cert_subject = "CN=Ali SARIASLAN"
signtool_path = "signtool"  # veya tam yolu: r"C:\Program Files (x86)\Windows Kits\10\bin\x64\signtool.exe"
timestamp_url = "http://timestamp.sectigo.com"
delay_seconds = 20

# 1) Create self-signed certificate (PowerShell üzerinden)
print("Creating self-signed code signing certificate...")
create_cert_cmd = [
    "powershell",
    "-Command",
    f"$cert = New-SelfSignedCertificate -Type CodeSigningCert -Subject '{cert_subject}' "
    f"-CertStoreLocation Cert:\\CurrentUser\\My -KeyExportPolicy Exportable -NotAfter (Get-Date).AddYears(2); "
    f"$cert.Thumbprint"
]
thumbprint = subprocess.check_output(create_cert_cmd, text=True).strip()
if not thumbprint:
    raise RuntimeError("Certificate creation failed.")
print(f"Created cert with thumbprint: {thumbprint}")

# 2) Export certificate to PFX
print(f"Exporting PFX to {pfx_out} ...")
export_pfx_cmd = [
    "powershell",
    "-Command",
    f'$securePwd = ConvertTo-SecureString -String "{PFX_PASSWORD}" -Force -AsPlainText; '
    f'Export-PfxCertificate -Cert "Cert:\\CurrentUser\\My\\{thumbprint}" -FilePath "{pfx_out}" -Password $securePwd'
]
subprocess.run(export_pfx_cmd, check=True)
print("Exported PFX.")

# 3) Build list of files to sign
files_to_sign = [
    os.path.join(release_path, "HelloClipboard.exe"),
    os.path.join(release_path, "goodbyedpi.exe"),
    os.path.join(release_path, "WinDivert64.sys"),
    os.path.join(release_path, "WinDivert.dll")
]
files_to_sign = [f for f in files_to_sign if os.path.exists(f)]

if not files_to_sign:
    print(f"No matching files found in {release_path}")
    exit(0)

# 4) Sign each file
results = []

for file_path in files_to_sign:
    # Check if already signed
    check_sig_cmd = ["powershell", "-Command", f"Get-AuthenticodeSignature '{file_path}' | Select-Object -ExpandProperty Status"]
    status = subprocess.check_output(check_sig_cmd, text=True).strip()
    if status == "Valid":
        print(f"{file_path} already signed. Skipping...")
        continue

    print(f"Signing: {file_path}")
    sign_cmd = [
        signtool_path,
        "sign",
        "/f", pfx_out,
        "/p", PFX_PASSWORD,
        "/fd", "SHA256",
        "/td", "SHA256",
        "/tr", timestamp_url,
        file_path
    ]
    proc = subprocess.run(sign_cmd, capture_output=True, text=True)
    if proc.returncode != 0:
        print(f"Warning: signtool returned exit code {proc.returncode} for {file_path}")
    else:
        print("Signed OK.")

    # Calculate SHA256
    with open(file_path, "rb") as f:
        sha256 = hashlib.sha256(f.read()).hexdigest()

    results.append({
        "File": file_path,
        "SHA256": sha256,
        "SigntoolExitCode": proc.returncode,
        "TimestampServer": timestamp_url,
        "Time": datetime.now().isoformat()
    })

    print(f"Waiting {delay_seconds} seconds before next sign...")
    time.sleep(delay_seconds)

# 5) Save results CSV
import csv
out_csv = os.path.join(release_path, f"signing_results_{datetime.now().strftime('%Y%m%d_%H%M%S')}.csv")
with open(out_csv, "w", newline="", encoding="utf-8") as csvfile:
    writer = csv.DictWriter(csvfile, fieldnames=results[0].keys() if results else [])
    writer.writeheader()
    writer.writerows(results)

print(f"Signing finished. Results written to {out_csv}")
print(f"PFX exported at: {pfx_out}")
print(f"Certificate thumbprint: {thumbprint}")
print("To trust this cert locally, import it into Trusted Root Certification Authorities.")
