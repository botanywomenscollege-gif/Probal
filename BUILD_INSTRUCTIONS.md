# Building Exam Seat Plan Generator as Windows .exe

This document explains how to convert `Exam_seat_plan (10).py` into a standalone Windows executable (.exe) that can be installed and run on any Windows computer without Python.

## Prerequisites

1. **Python 3.x installed** on your build machine (download from https://www.python.org/)
   - During installation, check "Add Python to PATH"
2. **Git** (optional, for cloning the repo)

## Step-by-Step Instructions

### 1. Prepare Your Environment

Open Command Prompt (cmd.exe) or PowerShell and run:

```bash
# Create a project folder
mkdir exam-seat-plan
cd exam-seat-plan

# Clone the repository (or copy the Python file manually)
git clone https://github.com/botanywomenscollege-gif/Probal.git
cd Probal
```

### 2. Install Required Dependencies

```bash
# Install the Python packages needed by the app
pip install openpyxl reportlab python-docx

# Install PyInstaller (for converting Python to .exe)
pip install pyinstaller
```

### 3. Build the Executable

Run this command from the directory containing `Exam_seat_plan (10).py`:

```bash
pyinstaller --onefile --windowed --name "ExamSeatPlan" --collect-all reportlab "Exam_seat_plan (10).py"
```

**Explanation of flags:**
- `--onefile` — Bundles everything into a single .exe file (easier distribution)
- `--windowed` — Removes the console window (GUI-only app)
- `--name "ExamSeatPlan"` — Names the output executable `ExamSeatPlan.exe`
- `--collect-all reportlab` — Includes all ReportLab data files (fonts, resources)

### 4. Find Your .exe

After the build completes, locate the executable:

```
exam-seat-plan\Probal\dist\ExamSeatPlan.exe
```

## Distribution

The single `ExamSeatPlan.exe` file can now be:

- **Copied directly** to any Windows 10/11 computer with no installation needed
- **Zipped and shared** via email or cloud storage
- **Run immediately** by double-clicking it (first run may take a few seconds as Windows scans it)

### Optional: Create an Installer

To create a professional installer (.msi), install NSIS and use:

```bash
pip install pyinstaller nuitka
# Or use tools like Inno Setup or Advanced Installer
```

## Troubleshooting

| Issue | Solution |
|-------|----------|
| "pyinstaller not found" | Run `pip install pyinstaller` |
| "Python not found" | Add Python to PATH: Settings → Environment Variables → add Python folder |
| Large .exe file (200+ MB) | This is normal for `--onefile`; use `--onedir` for a folder bundle instead |
| App won't start | Run `ExamSeatPlan.exe` from Command Prompt to see error messages |
| Missing fonts/ReportLab errors | Ensure `--collect-all reportlab` is in the command |

## Alternative: One-Directory Bundle

If the .exe is too large, create a folder bundle instead:

```bash
pyinstaller --onedir --windowed --name "ExamSeatPlan" --collect-all reportlab "Exam_seat_plan (10).py"
```

This creates `dist\ExamSeatPlan\` with the .exe and supporting files. You can zip this folder and distribute it.

## Notes

- First-time app startup may take 5-10 seconds (one-time initialization)
- Antivirus software may flag the .exe as suspicious (false positive); this is normal for unsigned executables
- To sign the .exe professionally, purchase a code-signing certificate

---

**Version:** 1.0  
**Last Updated:** 2026-07-20
