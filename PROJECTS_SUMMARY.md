# 📦 Complete Project Summary - All Applications

## Overview

This repository contains multiple Windows desktop applications built with C# .NET 8, compiled into standalone .exe files with no dependencies.

---

## 🎓 Project 1: Exam Seat Plan Generator

### Purpose
Automate exam seating arrangements for educational institutions. Generates professional exam hall sheets with PDF/Word export.

### Features
- 📊 Import student data from Excel worksheets
- 🎫 Automatic seating allocation to exam halls/rooms
- 📄 Generate PDF exam hall sheets
- 📝 Word document export
- 🏫 Support for 28+ hall/room configurations
- 🎯 Automatic expelled candidate handling
- 📋 Subject code recognition (20+ subjects)
- 🖨️ Professional formatting with Calibri fonts

### Files
```
ExamSeatPlanGenerator/
├── MainForm.cs              ← Application logic
├── ExamSeatPlan.csproj      ← Project configuration
└── .github/workflows/
    └── build-csharp-exe.yml ← Auto-build workflow
```

### Build Output
- **File**: `ExamSeatPlan.exe`
- **Size**: ~100-120 MB
- **Platform**: Windows 10+
- **Runtime**: .NET 8 (self-contained)

### Technologies Used
- C# Windows Forms
- EPPlus (Excel reading)
- iText7 (PDF generation)
- OpenXml (Word generation)

### Download
📥 Get the latest build from [GitHub Releases](https://github.com/botanywomenscollege-gif/Probal/releases)

---

## 📝 Project 2: Todo List Manager

### Purpose
Lightweight task management application with local JSON storage. Organize daily tasks with categories, due dates, and search functionality.

### Features
- ✅ Add, Edit, Delete tasks
- 📂 5 task categories (Work, Personal, Shopping, Health, Other)
- 📅 Due date management with date picker
- 🔍 Search and filter by category/status
- 💾 Local JSON file storage (`todos.json`)
- 📊 Task statistics (Total, Completed, Pending)
- 🎨 Color-coded categories for quick identification
- ⌨️ Keyboard shortcuts (Enter, Delete, Double-click)
- 🔐 100% offline, no cloud required

### Files
```
TodoListApp/
├── MainForm.cs              ← Application logic
├── TodoList.csproj          ← Project configuration
└── .github/workflows/
    └── build-todolist-exe.yml ← Auto-build workflow
```

### Build Output
- **File**: `TodoList.exe`
- **Size**: ~80-100 MB
- **Platform**: Windows 10+
- **Runtime**: .NET 8 (self-contained)

### Data Storage
- Format: JSON
- Location: `todos.json` (same folder as .exe)
- Structure: Array of TodoItem objects with Id, Title, Category, DueDate, etc.

### Technologies Used
- C# Windows Forms
- System.Text.Json (JSON serialization)
- .NET 8 Windows Forms

### Download
📥 Get the latest build from [GitHub Releases](https://github.com/botanywomenscollege-gif/Probal/releases)

### Documentation
- 📖 Full Guide: `TODO_LIST_README.md`
- 🚀 Quick Start: `TODOLIST_QUICKSTART.md`

---

## 🏗️ Project Architecture

### Repository Structure
```
Probal/
├── ExamSeatPlanGenerator/
│   ├── MainForm.cs
│   └── ExamSeatPlan.csproj
├── TodoListApp/
│   ├── MainForm.cs
│   └── TodoList.csproj
├── .github/workflows/
│   ├── build-csharp-exe.yml        (Exam Seat Plan)
│   └── build-todolist-exe.yml      (Todo List)
├── BUILD_INSTRUCTIONS.md           (General build guide)
├── TODO_LIST_README.md             (Todo List docs)
└── TODOLIST_QUICKSTART.md          (Todo List quick start)
```

### Technology Stack

| Component | Technology | Version |
|-----------|-----------|---------|
| **Framework** | .NET | 8.0 |
| **UI Framework** | Windows Forms | Built-in |
| **Language** | C# | Latest |
| **JSON** | System.Text.Json | 8.0.0 |
| **Excel** | EPPlus | 7.0.0 |
| **PDF** | iText7 | 8.0.0 |
| **Word** | DocumentFormat.OpenXml | 3.0.0 |

---

## 🚀 How to Download & Use

### Automatic Builds (Easiest)

Each project has automatic GitHub Actions workflows that:
1. **Trigger on**: Push to main branch or manual workflow dispatch
2. **Build**: Compiles C# code to Windows .exe
3. **Release**: Creates GitHub Release with downloadable .exe
4. **Artifact**: Stores .exe for 90 days

### Steps to Download
1. Visit: https://github.com/botanywomenscollege-gif/Probal/releases
2. Find the latest release for your application
3. Download the `.exe` file
4. Run directly (no installation needed!)

### Manual Build from Source

```bash
# Clone repository
git clone https://github.com/botanywomenscollege-gif/Probal.git
cd Probal

# For Exam Seat Plan Generator
cd ExamSeatPlanGenerator
dotnet publish -c Release -r win-x64 --self-contained

# For Todo List Manager
cd TodoListApp
dotnet publish -c Release -r win-x64 --self-contained

# .exe files created in dist/ folder
```

---

## 📋 System Requirements (Both Applications)

| Requirement | Specification |
|-----------|---------------|
| **Operating System** | Windows 10 or later (64-bit) |
| **Processor** | 1 GHz or faster |
| **RAM** | 256 MB minimum, 512 MB recommended |
| **Disk Space** | 100-120 MB per application |
| **.NET Runtime** | Pre-included (no separate installation) |
| **Internet** | Not required (100% offline) |

---

## 🔐 Security & Privacy

### Exam Seat Plan Generator
- ✅ No cloud storage
- ✅ Local file processing only
- ✅ No tracking or telemetry
- ✅ Excel files stay on your computer
- ✅ PDF/Word outputs created locally

### Todo List Manager
- ✅ Completely offline
- ✅ No internet connection required
- ✅ No account or login needed
- ✅ No telemetry or tracking
- ✅ All data in local JSON file
- ✅ You own your data

---

## 🐛 Troubleshooting

### Common Issues

**Issue: ".exe won't run"**
- Solution: Ensure Windows 10 or later installed
- Solution: Antivirus may flag it (whitelist the .exe)
- Solution: Try right-click → Run as Administrator

**Issue: "Missing dependencies"**
- Solution: All dependencies are bundled in the .exe
- Solution: No separate .NET installation needed

**Issue: "Data not saving" (Todo List)**
- Solution: Check folder write permissions
- Solution: Move app to Documents or Desktop

**Issue: "First run takes time"**
- Solution: Normal behavior (5-10 seconds for initialization)
- Solution: Subsequent runs will be instant

### Getting Help
1. Check application-specific README files
2. Review troubleshooting sections in docs
3. Open GitHub issue with detailed description
4. Provide error messages and system info

---

## 📈 Version History

### Version 1.0.0 (July 20, 2024)

#### Exam Seat Plan Generator
- ✅ Excel import functionality
- ✅ Automatic seating allocation
- ✅ PDF generation
- ✅ Multiple hall support
- ✅ Subject code recognition

#### Todo List Manager
- ✅ Task management (Add/Edit/Delete)
- ✅ 5 task categories
- ✅ Due date support
- ✅ Search and filter
- ✅ Local JSON storage
- ✅ Statistics display

---

## 🛠️ Development Guide

### Build Locally

```bash
# Prerequisites: .NET 8 SDK installed

# Restore dependencies
dotnet restore

# Build solution
dotnet build --configuration Release

# Publish as single .exe
dotnet publish -c Release -r win-x64 --self-contained

# Output: dist/[AppName].exe
```

### Project Structure
- Each application is independent
- Separate project folders (ExamSeatPlanGenerator, TodoListApp)
- Each has its own .csproj file
- Separate GitHub Actions workflows for each

### Adding New Features
1. Edit `MainForm.cs` in the application folder
2. Build and test locally
3. Push to GitHub
4. Automatic GitHub Actions build creates new .exe
5. Download from Releases page

---

## 📚 Documentation Files

| File | Purpose | Audience |
|------|---------|----------|
| `BUILD_INSTRUCTIONS.md` | General build guidelines | Developers |
| `TODO_LIST_README.md` | Complete Todo List docs | End Users |
| `TODOLIST_QUICKSTART.md` | Quick setup guide | End Users |
| `PROJECTS_SUMMARY.md` | This file | Everyone |

---

## 🎯 Future Enhancements

### Exam Seat Plan Generator
- [ ] Web-based interface
- [ ] Student photograph integration
- [ ] Barcode generation
- [ ] Email integration
- [ ] Multi-format export (CSV, Excel)

### Todo List Manager
- [ ] Task priorities
- [ ] Recurring tasks
- [ ] Task notes/descriptions
- [ ] Export to PDF/Excel
- [ ] Dark mode
- [ ] Cloud sync option

---

## 📞 Support & Feedback

### Getting Support
- 📖 Read the documentation files
- 🐛 Check GitHub Issues
- 💬 Open a new issue for bugs/features
- 📧 Email: botanywomenscollege@gmail.com

### Reporting Issues
Please include:
1. Application name and version
2. Operating system and version
3. Detailed description of the issue
4. Steps to reproduce
5. Error messages (if any)

### Feature Requests
1. Describe the feature
2. Explain why it's needed
3. Provide examples
4. Open GitHub issue with tag "enhancement"

---

## 📄 License

This project is open source and available under the MIT License.

You are free to:
- ✅ Use commercially
- ✅ Modify the source
- ✅ Distribute copies
- ✅ Use privately

---

## 🙏 Credits

### Built With
- **C#** - Programming language
- **.NET 8** - Framework
- **Windows Forms** - UI Framework
- **EPPlus** - Excel handling
- **iText7** - PDF generation
- **GitHub Actions** - CI/CD

### Contributors
- Botany Women's College

---

## 📊 Project Statistics

```
Total Applications:     2
Total Lines of Code:    2,500+
Build Files:            2
Documentation Files:    4
Workflows:              2
Supported Platforms:    Windows 10+
```

---

## 🎉 Getting Started

### For Exam Seat Plan Users
1. Download `ExamSeatPlan.exe` from Releases
2. Prepare Excel file with student data
3. Run the application
4. Generate exam hall sheets in PDF/Word

### For Todo List Users
1. Download `TodoList.exe` from Releases
2. Double-click to launch
3. Start adding tasks
4. Automatic local backup

---

## ✨ Key Highlights

✅ **No Installation Required** - Run .exe directly  
✅ **100% Offline** - No internet needed  
✅ **Fast Performance** - Compiled, not interpreted  
✅ **Professional UI** - Modern Windows Forms  
✅ **Small Footprint** - ~100 MB total  
✅ **Open Source** - GitHub available  
✅ **Automatic Builds** - Latest versions always available  
✅ **Complete Documentation** - Multiple guides included  

---

**Last Updated**: July 20, 2024  
**Current Version**: 1.0.0  
**Repository**: https://github.com/botanywomenscollege-gif/Probal  
**Status**: Production Ready ✅

Ready to get started? Download now from GitHub Releases! 🚀
