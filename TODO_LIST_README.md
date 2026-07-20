# 📝 Todo List Manager - Complete Guide

## Overview

**Todo List Manager** is a lightweight, fast Windows desktop application for managing your daily tasks with local storage. Built with C# and .NET 8, it provides a professional interface with no installation required.

## ✨ Features

### Core Functionality
- ✅ **Add Tasks** - Quickly add new tasks with a single click
- ✏️ **Edit Tasks** - Modify existing tasks easily
- 🗑️ **Delete Tasks** - Remove completed or unwanted tasks
- ✔️ **Mark Complete** - Track task progress with visual indicators

### Organization
- 📂 **5 Categories**: Work, Personal, Shopping, Health, Other
- 📅 **Due Dates**: Set and manage task deadlines
- 🎨 **Color Coding**: Each category has a distinct color for quick identification
- 📊 **Statistics**: View total, completed, and pending tasks at a glance

### Search & Filter
- 🔍 **Search**: Find tasks by keyword
- 🔎 **Filter by Category**: View tasks by specific category
- 📋 **Status Filter**: Show completed or pending tasks only

### Storage
- 💾 **Local JSON Storage**: All data saved in `todos.json`
- 🔐 **No Cloud Required**: Complete privacy and offline functionality
- 📦 **Portable**: Move the .exe and todos.json anywhere

### User Interface
- 🎯 **Intuitive Design**: Clean, modern interface
- ⌨️ **Keyboard Shortcuts**:
  - `Enter`: Add new task
  - `Delete`: Remove selected task
  - `Double-Click`: Toggle task completion

## 📥 Installation & Download

### Option 1: Download Pre-built .exe (Recommended)
1. Go to the **GitHub Releases** page
2. Download the latest `TodoList.exe`
3. Save it anywhere on your Windows computer
4. Double-click to run

### Option 2: Build from Source
```bash
# Clone the repository
git clone https://github.com/botanywomenscollege-gif/Probal.git
cd Probal/TodoListApp

# Build the application
dotnet publish -c Release -r win-x64 --self-contained

# Run the .exe
dist/TodoList.exe
```

## 📋 System Requirements

| Requirement | Specification |
|------------|---------------|
| **OS** | Windows 10 or later (64-bit) |
| **Disk Space** | ~80-100 MB |
| **.NET Runtime** | Pre-included (no separate installation) |
| **RAM** | 256 MB minimum |

## 🚀 Getting Started

### First Launch
1. Run `TodoList.exe`
2. Application creates `todos.json` in the same folder
3. Start adding tasks!

### Adding a Task
1. Type task description in the text box
2. Select a category from the dropdown
3. Choose a due date using the date picker
4. Click **"Add Task"** or press `Enter`

### Managing Tasks
- **Mark Complete**: Double-click a task to toggle completion status
- **Delete Task**: Select a task and press `Delete` key
- **Search**: Type in the search box to filter tasks
- **Filter**: Use the dropdown to view tasks by category or status

### Data Storage
- Tasks are automatically saved after each action
- `todos.json` is created in the application folder
- **Backup your `todos.json` file regularly!**

## 📂 Data Format

Tasks are stored in JSON format for easy backup and transfer:

```json
[
  {
    "Id": "unique-id-here",
    "Title": "Complete project report",
    "Description": "",
    "Category": "Work",
    "DueDate": "2024-12-31T00:00:00",
    "CreatedDate": "2024-12-20T10:30:00",
    "IsCompleted": false
  }
]
```

## 🎨 Category Colors

| Category | Color | Best For |
|----------|-------|----------|
| **Work** | Blue | Professional tasks |
| **Personal** | Pink | Personal activities |
| **Shopping** | Orange | Shopping lists |
| **Health** | Green | Health & fitness |
| **Other** | Gray | Miscellaneous |

## ⌨️ Keyboard Shortcuts

| Shortcut | Action |
|----------|--------|
| `Enter` | Add new task |
| `Delete` | Delete selected task |
| `Double-Click` | Toggle task completion |
| `Ctrl+F` | Focus search box |

## 💾 Backup & Restore

### Backup Your Data
```bash
# Copy todos.json to a safe location
copy todos.json D:\Backups\todos_backup.json
```

### Restore from Backup
```bash
# Copy backup back to application folder
copy D:\Backups\todos_backup.json todos.json
```

## 🐛 Troubleshooting

### Issue: .exe won't start
**Solution:**
- Ensure Windows 10 or later is installed
- Antivirus software may block first run (whitelist the .exe)
- Try running in compatibility mode (right-click → Properties → Compatibility)

### Issue: Data not saving
**Solution:**
- Check folder permissions
- Ensure you have write access to the application folder
- Move to a different directory if the current one is read-only

### Issue: Tasks disappear after restart
**Solution:**
- Verify `todos.json` exists in the application folder
- Check if file is being corrupted by antivirus
- Restore from backup

### Issue: Slow performance
**Solution:**
- Close other applications to free up RAM
- Delete very old completed tasks
- Reduce number of active tasks

## 🔐 Privacy & Security

- ✅ **No Internet Connection**: Application works completely offline
- ✅ **No Cloud Sync**: All data stays on your computer
- ✅ **No Account Required**: No registration or login needed
- ✅ **No Tracking**: Complete privacy guaranteed
- ✅ **Open Format**: JSON files can be edited with any text editor

## 📊 Statistics Dashboard

The application displays real-time statistics:
- **Total**: Total number of tasks
- **Completed**: Number of finished tasks
- **Pending**: Number of active tasks

## 🛠️ Advanced Features

### Edit via JSON
You can edit `todos.json` directly with any text editor if needed:
1. Open `todos.json` with Notepad
2. Make changes directly in JSON format
3. Save the file
4. Restart the application to see changes

### Export Tasks
```bash
# Simply copy todos.json to share or backup
copy todos.json emails/tasks_export.json
```

### Clear Completed Tasks
Click **"Clear All"** button to remove all completed tasks permanently

## 🚀 Performance Tips

1. **Limit Active Tasks**: Keep < 500 active tasks for best performance
2. **Archive Old Tasks**: Move completed tasks to separate file monthly
3. **Regular Backups**: Backup `todos.json` weekly
4. **Close Other Apps**: Free up system resources

## 📝 Sample Use Cases

### Work Management
- Track project deadlines
- Manage daily meetings
- Monitor deliverables
- Track bug fixes

### Personal Tasks
- Grocery shopping lists
- Home maintenance
- Family reminders
- Hobby projects

### Health & Fitness
- Workout schedules
- Medication reminders
- Doctor appointments
- Meal planning

## 🤝 Contributing

Found a bug? Have a feature request? 
- Open an issue on GitHub
- Submit a pull request with improvements

## 📄 License

This project is open source and available under the MIT License.

## 📞 Support

For issues, questions, or suggestions:
1. Check the **Troubleshooting** section above
2. Open an issue on GitHub
3. Review the FAQ section below

## ❓ FAQ

**Q: Is my data safe?**
A: Yes! All data is stored locally on your computer in `todos.json`

**Q: Can I use this on Mac/Linux?**
A: Currently Windows only. Mac/Linux versions possible in future

**Q: How do I uninstall?**
A: Simply delete `TodoList.exe` and `todos.json` (backup first!)

**Q: Can I sync across devices?**
A: You can manually copy `todos.json` between devices

**Q: Is there a portable version?**
A: The .exe is already portable - just copy it anywhere

**Q: Does it require internet?**
A: No, it works completely offline

## 🎉 What's Next?

Future planned features:
- ☐ Task priorities (High, Medium, Low)
- ☐ Recurring tasks
- ☐ Task notes/descriptions
- ☐ Export to PDF/Excel
- ☐ Dark mode
- ☐ Cloud sync option

## 🙏 Credits

Built with:
- C# .NET 8
- Windows Forms
- JSON for storage

---

**Version**: 1.0.0  
**Last Updated**: July 20, 2024  
**Build**: Production Ready

Enjoy managing your tasks! 🚀
