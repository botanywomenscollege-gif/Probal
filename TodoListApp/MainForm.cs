using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace TodoListApp
{
    /// <summary>
    /// Todo List Application with Local Storage
    /// Features:
    /// - Add, edit, delete tasks
    /// - Mark tasks as complete
    /// - Local JSON file storage
    /// - Task categories
    /// - Due dates
    /// - Search functionality
    /// </summary>
    public partial class MainForm : Form
    {
        private List<TodoItem> todoItems = new();
        private const string DATA_FILE = "todos.json";
        private string selectedCategory = "All";

        public MainForm()
        {
            InitializeComponent();
            LoadTodos();
        }

        private void InitializeComponent()
        {
            this.Text = "Todo List Manager";
            this.Width = 900;
            this.Height = 700;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Icon = SystemIcons.Application;
            this.FormClosing += (s, e) => SaveTodos();

            // Main container
            var mainPanel = new Panel { Dock = DockStyle.Fill };

            // Top control panel
            var topPanel = new Panel { Dock = DockStyle.Top, Height = 80, BorderStyle = BorderStyle.FixedSingle };
            topPanel.BackColor = System.Drawing.Color.FromArgb(63, 81, 181);

            var titleLabel = new Label
            {
                Text = "📝 Todo List Manager",
                Dock = DockStyle.Top,
                Height = 40,
                Font = new System.Drawing.Font("Segoe UI", 16, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            topPanel.Controls.Add(titleLabel);

            var inputPanel = new Panel { Dock = DockStyle.Bottom, Height = 35 };
            inputPanel.BackColor = System.Drawing.Color.FromArgb(63, 81, 181);

            var taskInput = new TextBox
            {
                Name = "TaskInput",
                Left = 10,
                Top = 5,
                Width = 400,
                Height = 25,
                Font = new System.Drawing.Font("Segoe UI", 10),
                PlaceholderText = "Enter a new task..."
            };
            inputPanel.Controls.Add(taskInput);

            var categoryCombo = new ComboBox
            {
                Name = "CategoryCombo",
                Left = 420,
                Top = 5,
                Width = 120,
                Height = 25,
                Font = new System.Drawing.Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Items = { "Work", "Personal", "Shopping", "Health", "Other" }
            };
            categoryCombo.SelectedIndex = 0;
            inputPanel.Controls.Add(categoryCombo);

            var dueDatePicker = new DateTimePicker
            {
                Name = "DueDatePicker",
                Left = 550,
                Top = 5,
                Width = 120,
                Height = 25,
                Font = new System.Drawing.Font("Segoe UI", 10),
                Format = DateTimePickerFormat.Short
            };
            inputPanel.Controls.Add(dueDatePicker);

            var addButton = new Button
            {
                Text = "Add Task",
                Left = 680,
                Top = 5,
                Width = 90,
                Height = 25,
                BackColor = System.Drawing.Color.FromArgb(76, 175, 80),
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold)
            };
            addButton.Click += (s, e) => AddTodo(taskInput, categoryCombo, dueDatePicker);
            inputPanel.Controls.Add(addButton);

            var clearButton = new Button
            {
                Text = "Clear All",
                Left = 780,
                Top = 5,
                Width = 90,
                Height = 25,
                BackColor = System.Drawing.Color.FromArgb(244, 67, 54),
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold)
            };
            clearButton.Click += (s, e) => ClearAllTodos();
            inputPanel.Controls.Add(clearButton);

            topPanel.Controls.Add(inputPanel);
            mainPanel.Controls.Add(topPanel);

            // Middle section with filter and search
            var filterPanel = new Panel { Dock = DockStyle.Top, Height = 50, BorderStyle = BorderStyle.FixedSingle };
            filterPanel.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);

            var filterLabel = new Label
            {
                Text = "Filter:",
                Left = 10,
                Top = 12,
                Width = 50,
                Font = new System.Drawing.Font("Segoe UI", 10)
            };
            filterPanel.Controls.Add(filterLabel);

            var categoryFilter = new ComboBox
            {
                Name = "CategoryFilter",
                Left = 60,
                Top = 10,
                Width = 120,
                Height = 25,
                Font = new System.Drawing.Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Items = { "All", "Work", "Personal", "Shopping", "Health", "Other", "Completed", "Pending" }
            };
            categoryFilter.SelectedIndex = 0;
            categoryFilter.SelectedIndexChanged += (s, e) => RefreshTodoList();
            filterPanel.Controls.Add(categoryFilter);

            var searchLabel = new Label
            {
                Text = "Search:",
                Left = 200,
                Top = 12,
                Width = 50,
                Font = new System.Drawing.Font("Segoe UI", 10)
            };
            filterPanel.Controls.Add(searchLabel);

            var searchBox = new TextBox
            {
                Name = "SearchBox",
                Left = 250,
                Top = 10,
                Width = 200,
                Height = 25,
                Font = new System.Drawing.Font("Segoe UI", 10),
                PlaceholderText = "Search tasks..."
            };
            searchBox.TextChanged += (s, e) => RefreshTodoList();
            filterPanel.Controls.Add(searchBox);

            var statsLabel = new Label
            {
                Name = "StatsLabel",
                Left = 550,
                Top = 12,
                Width = 320,
                Font = new System.Drawing.Font("Segoe UI", 10),
                TextAlign = System.Drawing.ContentAlignment.MiddleRight
            };
            filterPanel.Controls.Add(statsLabel);

            mainPanel.Controls.Add(filterPanel);

            // Todo list
            var todoListBox = new ListBox
            {
                Name = "TodoListBox",
                Dock = DockStyle.Fill,
                Font = new System.Drawing.Font("Segoe UI", 10),
                DrawMode = DrawMode.OwnerDrawVariable,
                ItemHeight = 20
            };
            todoListBox.DrawItem += (s, e) => DrawTodoItem(e);
            todoListBox.MeasureItem += (s, e) => e.ItemHeight = 60;
            todoListBox.DoubleClick += (s, e) => EditTodo(todoListBox);
            todoListBox.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Delete)
                    DeleteTodo(todoListBox);
                e.Handled = false;
            };
            mainPanel.Controls.Add(todoListBox);

            this.Controls.Add(mainPanel);

            taskInput.KeyPress += (s, e) =>
            {
                if (e.KeyCode == Keys.Return)
                {
                    AddTodo(taskInput, categoryCombo, dueDatePicker);
                    e.Handled = true;
                }
            };
        }

        private void AddTodo(TextBox taskInput, ComboBox categoryCombo, DateTimePicker dueDatePicker)
        {
            string taskText = taskInput.Text.Trim();
            if (string.IsNullOrEmpty(taskText))
            {
                MessageBox.Show("Please enter a task description.", "Empty Task", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var todo = new TodoItem
            {
                Id = Guid.NewGuid().ToString(),
                Title = taskText,
                Category = categoryCombo.SelectedItem?.ToString() ?? "Other",
                DueDate = dueDatePicker.Value,
                CreatedDate = DateTime.Now,
                IsCompleted = false,
                Description = ""
            };

            todoItems.Add(todo);
            SaveTodos();
            RefreshTodoList();
            taskInput.Clear();
            taskInput.Focus();
        }

        private void DeleteTodo(ListBox todoListBox)
        {
            if (todoListBox.SelectedIndex < 0)
                return;

            var result = MessageBox.Show("Delete this task?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                var selected = GetFilteredTodos()[todoListBox.SelectedIndex];
                todoItems.Remove(todoItems.FirstOrDefault(t => t.Id == selected.Id));
                SaveTodos();
                RefreshTodoList();
            }
        }

        private void EditTodo(ListBox todoListBox)
        {
            if (todoListBox.SelectedIndex < 0)
                return;

            var filteredTodos = GetFilteredTodos();
            var selected = filteredTodos[todoListBox.SelectedIndex];
            var todo = todoItems.FirstOrDefault(t => t.Id == selected.Id);

            if (todo != null)
            {
                todo.IsCompleted = !todo.IsCompleted;
                SaveTodos();
                RefreshTodoList();
            }
        }

        private void ClearAllTodos()
        {
            var result = MessageBox.Show("Delete all completed tasks?", "Clear Completed", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                todoItems = todoItems.Where(t => !t.IsCompleted).ToList();
                SaveTodos();
                RefreshTodoList();
            }
        }

        private void RefreshTodoList()
        {
            var todoListBox = this.Controls.OfType<Panel>().First().Controls.OfType<ListBox>().First();
            var statsLabel = this.Controls.OfType<Panel>().First().Controls.OfType<Label>().FirstOrDefault(l => l.Name == "StatsLabel");
            var searchBox = this.Controls.OfType<Panel>().First().Controls.OfType<TextBox>().FirstOrDefault(t => t.Name == "SearchBox");
            var categoryFilter = this.Controls.OfType<Panel>().First().Controls.OfType<ComboBox>().FirstOrDefault(c => c.Name == "CategoryFilter");

            todoListBox.Items.Clear();

            var filtered = GetFilteredTodos();
            foreach (var todo in filtered)
            {
                todoListBox.Items.Add(todo);
            }

            // Update stats
            int total = todoItems.Count;
            int completed = todoItems.Count(t => t.IsCompleted);
            int pending = total - completed;
            statsLabel.Text = $"Total: {total} | Completed: {completed} | Pending: {pending}";
        }

        private List<TodoItem> GetFilteredTodos()
        {
            var categoryFilter = this.Controls.OfType<Panel>().First().Controls.OfType<ComboBox>().FirstOrDefault(c => c.Name == "CategoryFilter");
            var searchBox = this.Controls.OfType<Panel>().First().Controls.OfType<TextBox>().FirstOrDefault(t => t.Name == "SearchBox");

            var filter = categoryFilter?.SelectedItem?.ToString() ?? "All";
            var search = searchBox?.Text?.ToLower() ?? "";

            var filtered = todoItems.AsEnumerable();

            // Apply category filter
            if (filter == "Completed")
                filtered = filtered.Where(t => t.IsCompleted);
            else if (filter == "Pending")
                filtered = filtered.Where(t => !t.IsCompleted);
            else if (filter != "All")
                filtered = filtered.Where(t => t.Category == filter);

            // Apply search filter
            if (!string.IsNullOrEmpty(search))
                filtered = filtered.Where(t => t.Title.ToLower().Contains(search));

            return filtered.OrderByDescending(t => t.CreatedDate).ToList();
        }

        private void DrawTodoItem(DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            var todo = (TodoItem)e.Items[e.Index];
            var isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            // Background
            e.Graphics.FillRectangle(
                isSelected ? new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(220, 220, 220)) :
                new System.Drawing.SolidBrush(System.Drawing.Color.White),
                e.Bounds);

            // Checkbox
            var checkboxRect = new System.Drawing.Rectangle(e.Bounds.X + 5, e.Bounds.Y + 5, 20, 20);
            e.Graphics.DrawRectangle(System.Drawing.Pens.Gray, checkboxRect);
            if (todo.IsCompleted)
            {
                e.Graphics.FillRectangle(System.Drawing.Brushes.Green, checkboxRect);
                e.Graphics.DrawString("✓", new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold),
                    System.Drawing.Brushes.White, checkboxRect.X + 3, checkboxRect.Y - 2);
            }

            // Title
            var titleBrush = todo.IsCompleted ? System.Drawing.Brushes.Gray : System.Drawing.Brushes.Black;
            var titleFont = todo.IsCompleted ?
                new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Strikeout) :
                new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            e.Graphics.DrawString(todo.Title, titleFont, titleBrush, e.Bounds.X + 35, e.Bounds.Y + 5);

            // Category and Due Date
            var categoryColor = GetCategoryColor(todo.Category);
            var infoBrush = new System.Drawing.SolidBrush(categoryColor);
            var infoFont = new System.Drawing.Font("Segoe UI", 8);
            var dueDateText = $"📅 Due: {todo.DueDate:MMM dd, yyyy}";
            e.Graphics.DrawString($"[{todo.Category}]", infoFont, infoBrush, e.Bounds.X + 35, e.Bounds.Y + 25);
            e.Graphics.DrawString(dueDateText, infoFont, System.Drawing.Brushes.DarkGray, e.Bounds.X + 150, e.Bounds.Y + 25);

            // Border
            e.Graphics.DrawLine(System.Drawing.Pens.LightGray, e.Bounds.X, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);

            e.DrawFocusRectangle();
        }

        private System.Drawing.Color GetCategoryColor(string category)
        {
            return category switch
            {
                "Work" => System.Drawing.Color.FromArgb(63, 81, 181),
                "Personal" => System.Drawing.Color.FromArgb(233, 30, 99),
                "Shopping" => System.Drawing.Color.FromArgb(255, 152, 0),
                "Health" => System.Drawing.Color.FromArgb(76, 175, 80),
                _ => System.Drawing.Color.FromArgb(158, 158, 158)
            };
        }

        private void SaveTodos()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(todoItems, options);
                File.WriteAllText(DATA_FILE, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving todos: {ex.Message}", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTodos()
        {
            try
            {
                if (File.Exists(DATA_FILE))
                {
                    var json = File.ReadAllText(DATA_FILE);
                    todoItems = JsonSerializer.Deserialize<List<TodoItem>>(json) ?? new();
                }
                RefreshTodoList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading todos: {ex.Message}", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public class TodoItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsCompleted { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
