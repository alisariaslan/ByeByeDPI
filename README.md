# ByeByeDPI

> **A simple Windows tool to manage [GoodbyeDPI](https://github.com/ValdikSS/GoodbyeDPI)** — a utility for bypassing Deep Packet Inspection (DPI) on certain networks.
> This graphical interface allows you to start, stop, and manage the DPI bypass process easily with update checking, tray support, and parameter customization.

## 🚀 Features

* ✅ Start and stop **GoodbyeDPI** with one click
* ⚙️ Edit parameters (`params.json`) and domain checklists (`checklist.json`) directly
* 🔔 Background update checks with GitHub integration
* 🪟 Option to start automatically with Windows
* 📥 Minimize to tray and run silently
* 🧹 Reset / clear saved configuration

## 🖼️ Screenshots

<img width="275" height="458" alt="image" src="https://github.com/user-attachments/assets/ff4dd794-8ba0-43c9-bfc3-be85db9f47b6" />

## ⚙️ Installation

```bash
# Download installer from the latest release
# Then run ByeByeDPI_Installer.exe
```

### Auto-start

The application can be configured to launch at Windows startup via its settings dialog.

## 📚 Developer Guide

| Role | Where to start |
|------|----------------|
| **Build** | `ByeByeDPI.sln` – use Visual Studio 2026 or newer. |
| **Code Structure** | - `Core/TrayApplicationContext.cs` – main entry point.- `Views/*` – UI forms and controls. |
| **Configuration** | `HelloClipboard/Constants/Constants.cs` – shared constants and paths.`latest_version_v2.json` – version metadata. |
| **Hotkeys & Privileges** | `TrayApplicationContext.ReloadGlobalHotkey()` handles hotkey registration; see `Utils/PriviligesHelper.cs`. |

## 📄 License

MIT – see the [LICENSE](LICENSE) file.

---

## Contact

Report bugs or request features via GitHub Issues:
[https://github.com/alisariaslan/ByeByeDPI/issues](https://github.com/alisariaslan/ByeByeDPI/issues)

For other inquiries:
**[dev@alisariaslan.com](mailto:dev@alisariaslan.com)**

---

**Contributing**
Feel free to open issues or pull requests. Please follow the PR template in `.github/pull_request_template.md` for consistency.
