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

<img width="490" height="740" alt="image" src="https://github.com/user-attachments/assets/2c44c829-ddba-43c5-8c1c-b175f1f8817f" />

## ⚙️ Installation Notes

- Extract all files from the .zip archive to a regular folder. Do not run the installer directly from inside the compressed archive.
- Run setup.exe
- If installation fails with an error about WinDivert64.sys being in use, stop the driver first with using cmd (as an Admin):
- sc stop WinDivert
- Then rerun the installer.

## Contact

Report bugs or request features via GitHub Issues: https://github.com/alisariaslan/ByeByeDPI/issues
For other inquiries: [dev@alisariaslan.com](mailto:dev@alisariaslan.com)

