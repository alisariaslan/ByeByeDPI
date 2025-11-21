# ByeByeDPI

**A simple Windows tool to manage [GoodbyeDPI](https://github.com/ValdikSS/GoodbyeDPI)** — a utility for bypassing Deep Packet Inspection (DPI) on certain networks.
This graphical interface allows you to start, stop, and manage the DPI bypass process easily with update checking, tray support, and parameter customization.

---

## 🧩 Features

- ✅ Start and stop **GoodbyeDPI** with one click
- ⚙️ Edit parameters (`params.json`) and domain checklists (`checklist.json`) directly
- 🔔 Background update checks with GitHub integration
- 🪟 Option to start automatically with Windows
- 📥 Minimize to tray and run silently
- 🧹 Reset / clear saved configuration

---

## 🖼️ Screenshots
<img width="536" height="799" alt="image" src="https://github.com/user-attachments/assets/c263e152-6b36-4227-bfe5-6b2b4516950d" />

⚙️ Installation Notes

- Download “ByeByeDPI_Installer.exe” from the “latest” release on GitHub.
- Run the installer and follow the instructions.

🗑️ Uninstallation Notes

- Click “Stop Access” or “Reset” inside ByeByeDPI.
- If the app is running in the system tray, exit it from the tray icon.
- Open “Add or Remove Programs” in Windows.
- Select “ByeByeDPI” and click Uninstall.
- If the installation folder still remains after uninstalling, run the following command in CMD or PowerShell (as Administrator) to stop the background service: sc stop WinDivert

## Contact

Report bugs or request features via GitHub Issues: https://github.com/alisariaslan/ByeByeDPI/issues
For other inquiries: [dev@alisariaslan.com](mailto:dev@alisariaslan.com)

