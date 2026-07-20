using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OfficeOpenXml;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Table = iText.Layout.Element.Table;
using Paragraph = iText.Layout.Element.Paragraph;

namespace ExamSeatPlanGenerator
{
    public partial class MainForm : Form
    {
        private Dictionary<string, List<string>> worksheetData = new();
        private Dictionary<string, TextBox> roomEntries = new();
        private Label importedFileLabel;

        private const string DEFAULT_EXAM_NAME = "FYUG-NEP EVEN SEMESTER EXAMINATION - 2026";
        private const int SEATS_PER_ROW_BLOCK = 3;
        private const float TABLE_NO_COL_MM = 18f;
        private const float ENROL_COL_MM = 44f;

        private static readonly Dictionary<string, string> SUBJECT_EXACT_CODES = new()
        {
            { "AEC-01", "Understanding and Connecting with Environment" },
            { "AEC-03", "English Communication" },
            { "AEC-04", "Personal Communication" }
        };

        private static readonly Dictionary<string, string> SUBJECT_ABBREVIATIONS = new()
        {
            { "BN", "Bengali" },
            { "EC", "Economics" },
            { "ED", "Education" },
            { "EN", "English" },
            { "HS", "History" },
            { "HN", "Hindi" },
            { "KB", "Kokborok" },
            { "PL", "Philosophy" },
            { "PS", "Political Science" },
            { "PE", "Physical Education" },
            { "PH", "Physics" },
            { "CH", "Chemistry" },
            { "BECV", "Music Minor" },
            { "BICV", "Music Id" },
            { "SK", "Sanskrit" },
            { "SC", "Sociology" },
            { "MT", "Mathematics" },
            { "BT", "Botany" },
            { "ZL", "Zoology" },
            { "HP", "Human Physiology" },
            { "GE", "Geography" },
            { "NCC", "NCC" }
        };

        private static readonly string[] ROOM_NAMES = new[]
        {
            "Hall-I", "Hall-II", "Hall-III", "Hall-IV", "Room-101", "Room-102",
            "Room-103", "Room-104", "Room-105", "Room-202", "Room-203", "Room-204",
            "S1", "S2", "S3", "S4", "S5", "Science-B", "S-10", "S-16", "S-17",
            "PG-Bengali", "PG-English", "PG-A1", "PG-A2", "18", "19", "20", "Library"
        };

        public MainForm()
        {
            EPPlus.LicenseContext.SetLicenseContext(LicenseContext.NonCommercial);
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Exam Seat Plan Generator";
            this.Width = 650;
            this.Height = 900;
            this.MinimumSize = new System.Drawing.Size(550, 400);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;

            var mainPanel = new Panel { Dock = DockStyle.Fill, AutoScroll = true };

            // Title
            var titleLabel = new Label
            {
                Text = DEFAULT_EXAM_NAME,
                Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold),
                AutoSize = false,
                Height = 50,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top
            };
            mainPanel.Controls.Add(titleLabel);

            // Import Section
            var importPanel = new Panel { Dock = DockStyle.Top, Height = 70, BorderStyle = BorderStyle.FixedSingle };
            var importButton = new Button
            {
                Text = "Import Worksheet",
                Width = 200,
                Height = 35,
                Left = 20,
                Top = 10,
                BackColor = System.Drawing.Color.FromArgb(255, 152, 0),
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 10)
            };
            importButton.Click += ImportWorksheet_Click;
            importPanel.Controls.Add(importButton);

            importedFileLabel = new Label
            {
                Text = "No file imported",
                Left = 20,
                Top = 50,
                Width = 300,
                Height = 15,
                Font = new System.Drawing.Font("Segoe UI", 8),
                ForeColor = System.Drawing.Color.Gray
            };
            importPanel.Controls.Add(importedFileLabel);
            mainPanel.Controls.Add(importPanel);

            // Details Section
            var detailsPanel = new Panel { Dock = DockStyle.Top, Height = 280, BorderStyle = BorderStyle.FixedSingle };
            int yPos = 10;

            AddFormField(detailsPanel, "Name of Exam:", ref yPos, "ExamName");
            AddFormField(detailsPanel, "College/Institution:", ref yPos, "College");
            AddFormField(detailsPanel, "Semester:", ref yPos, "Semester");
            AddFormField(detailsPanel, "Date:", ref yPos, "Date");
            AddFormField(detailsPanel, "Paper Code(s):", ref yPos, "PaperCodes");
            AddFormField(detailsPanel, "Time of Exam:", ref yPos, "Time");
            AddFormField(detailsPanel, "Expelled Candidate(s):", ref yPos, "Expelled");

            mainPanel.Controls.Add(detailsPanel);

            // Halls Section
            var hallsLabel = new Label
            {
                Text = "Number of Tables (leave blank for 0)",
                Dock = DockStyle.Top,
                Height = 25,
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                Padding = new Padding(10)
            };
            mainPanel.Controls.Add(hallsLabel);

            var hallsPanel = new Panel { Dock = DockStyle.Top, Height = 450, BorderStyle = BorderStyle.FixedSingle, AutoScroll = true };
            yPos = 10;
            foreach (var roomName in ROOM_NAMES)
            {
                var label = new Label
                {
                    Text = roomName + ":",
                    Left = 20,
                    Top = yPos,
                    Width = 150,
                    Height = 20,
                    Font = new System.Drawing.Font("Segoe UI", 10)
                };
                hallsPanel.Controls.Add(label);

                var textBox = new TextBox
                {
                    Name = $"Hall_{roomName}",
                    Left = 180,
                    Top = yPos,
                    Width = 100,
                    Height = 20,
                    Font = new System.Drawing.Font("Segoe UI", 10)
                };
                roomEntries[roomName] = textBox;
                hallsPanel.Controls.Add(textBox);

                yPos += 25;
            }
            mainPanel.Controls.Add(hallsPanel);

            // Buttons Section
            var buttonsPanel = new Panel { Dock = DockStyle.Bottom, Height = 60, BorderStyle = BorderStyle.FixedSingle };

            var pdfButton = new Button
            {
                Text = "Generate PDF",
                Width = 150,
                Height = 40,
                Left = 20,
                Top = 10,
                BackColor = System.Drawing.Color.FromArgb(76, 175, 80),
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold)
            };
            pdfButton.Click += GeneratePDF_Click;
            buttonsPanel.Controls.Add(pdfButton);

            var wordButton = new Button
            {
                Text = "Save as Word",
                Width = 150,
                Height = 40,
                Left = 180,
                Top = 10,
                BackColor = System.Drawing.Color.FromArgb(43, 87, 154),
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold)
            };
            wordButton.Click += GenerateWord_Click;
            buttonsPanel.Controls.Add(wordButton);

            mainPanel.Controls.Add(buttonsPanel);
            this.Controls.Add(mainPanel);
        }

        private void AddFormField(Panel parent, string label, ref int yPos, string name)
        {
            var labelControl = new Label
            {
                Text = label,
                Left = 20,
                Top = yPos,
                Width = 150,
                Height = 20,
                Font = new System.Drawing.Font("Segoe UI", 10),
                TextAlign = System.Drawing.ContentAlignment.MiddleRight
            };
            parent.Controls.Add(labelControl);

            var textBox = new TextBox
            {
                Name = name,
                Left = 180,
                Top = yPos,
                Width = 400,
                Height = 20,
                Font = new System.Drawing.Font("Segoe UI", 10)
            };
            parent.Controls.Add(textBox);

            yPos += 30;
        }

        private TextBox GetField(string name) => this.Controls.OfType<Control>()
            .SelectMany(c => c.Controls.OfType<TextBox>())
            .FirstOrDefault(t => t.Name == name) ?? new TextBox();

        private void ImportWorksheet_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                Title = "Import Worksheet"
            })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        worksheetData.Clear();
                        using (var package = new ExcelPackage(new FileInfo(openFileDialog.FileName)))
                        {
                            var worksheet = package.Workbook.Worksheets[0];
                            var headers = new List<string>();

                            // Read headers
                            for (int col = 1; col <= worksheet.Dimension?.Columns; col++)
                            {
                                var headerValue = worksheet.Cells[1, col].Value?.ToString()?.Trim() ?? "";
                                if (!string.IsNullOrEmpty(headerValue))
                                    headers.Add(headerValue);
                            }

                            foreach (var header in headers)
                                worksheetData[header] = new List<string>();

                            // Read data
                            for (int row = 2; row <= (worksheet.Dimension?.Rows ?? 0); row++)
                            {
                                for (int col = 0; col < headers.Count; col++)
                                {
                                    var cellValue = worksheet.Cells[row, col + 1].Value?.ToString()?.Trim() ?? "";
                                    if (!string.IsNullOrEmpty(cellValue))
                                        worksheetData[headers[col]].Add(cellValue);
                                }
                            }
                        }

                        importedFileLabel.Text = $"Imported: {Path.GetFileName(openFileDialog.FileName)}";
                        MessageBox.Show(
                            $"Loaded {worksheetData.Count} paper code column(s):\n{string.Join(", ", worksheetData.Keys)}",
                            "Import Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Could not read the worksheet:\n{ex.Message}", "Import Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void GeneratePDF_Click(object sender, EventArgs e)
        {
            var data = GatherInputs();
            if (data == null) return;

            using (var saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf",
                FileName = $"{data["filename_stub"]}.pdf",
                Title = "Save Exam Seat Plan as PDF"
            })
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        BuildPDF(saveFileDialog.FileName, data);
                        MessageBox.Show($"Exam seat plan saved to:\n{saveFileDialog.FileName}",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error generating PDF:\n{ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void GenerateWord_Click(object sender, EventArgs e)
        {
            var data = GatherInputs();
            if (data == null) return;

            using (var saveFileDialog = new SaveFileDialog
            {
                Filter = "Word documents (*.docx)|*.docx",
                FileName = $"{data["filename_stub"]}.docx",
                Title = "Save Exam Seat Plan as Word Document"
            })
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        BuildDocx(saveFileDialog.FileName, data);
                        MessageBox.Show($"Exam seat plan saved to:\n{saveFileDialog.FileName}",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error generating Word document:\n{ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private Dictionary<string, object> GatherInputs()
        {
            if (worksheetData.Count == 0)
            {
                MessageBox.Show("Please import an Excel worksheet first.", "No worksheet",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            string paperCodesRaw = GetField("PaperCodes").Text.Trim();
            string timeOfExam = GetField("Time").Text.Trim();

            if (string.IsNullOrEmpty(paperCodesRaw) || string.IsNullOrEmpty(timeOfExam))
            {
                MessageBox.Show("Please fill in Paper Code(s) and Time of Exam.", "Missing info",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            var paperCodes = paperCodesRaw.Split(',').Select(p => p.Trim()).Where(p => !string.IsNullOrEmpty(p)).ToList();
            var expelledSet = new HashSet<string>(GetField("Expelled").Text.Split(',')
                .Select(e => e.Trim().ToLower()).Where(e => !string.IsNullOrEmpty(e)));

            var matchedPairs = new List<(string, string, List<string>)>();
            var missingCodes = new List<string>();

            foreach (var pc in paperCodes)
            {
                var matched = FindMatchingColumn(pc);
                if (matched == null)
                    missingCodes.Add(pc);
                else
                    matchedPairs.Add((pc, matched, worksheetData[matched]));
            }

            if (missingCodes.Count > 0)
            {
                MessageBox.Show(
                    $"No column matching: {string.Join(", ", missingCodes)}\n\n" +
                    $"Available columns: {string.Join(", ", worksheetData.Keys)}",
                    "Paper code(s) not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            var combinedSeats = new List<(string, string, string)>();
            int excludedCount = 0;

            foreach (var (pc, header, rolls) in matchedPairs)
            {
                var filteredRolls = new List<string>();
                foreach (var roll in rolls)
                {
                    if (IsExpelledSuffix(roll) || expelledSet.Contains(roll.ToLower()))
                    {
                        excludedCount++;
                        continue;
                    }
                    filteredRolls.Add(roll);
                }

                if (filteredRolls.Count > 0)
                {
                    foreach (var (i, roll) in filteredRolls.Select((r, idx) => (idx, r)))
                    {
                        var marker = i == 0 ? header : null;
                        combinedSeats.Add((roll, header, marker));
                    }
                }
            }

            if (combinedSeats.Count == 0)
            {
                MessageBox.Show(
                    "No roll numbers found after excluding expelled candidates.",
                    "No data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            var hallCounts = new List<(string, int)>();
            foreach (var roomName in ROOM_NAMES)
            {
                if (roomEntries.TryGetValue(roomName, out var entry))
                {
                    int.TryParse(entry.Text.Trim(), out int count);
                    hallCounts.Add((roomName, count));
                }
            }

            if (hallCounts.Sum(x => x.Item2) == 0)
            {
                MessageBox.Show("Please enter the number of tables for at least one hall/room.",
                    "No tables", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            var (allocation, leftover) = AllocateStudentsToHalls(combinedSeats, hallCounts);

            if (leftover.Count > 0)
            {
                var result = MessageBox.Show(
                    $"There are {combinedSeats.Count} roll numbers but only {combinedSeats.Count - leftover.Count} table(s) available.\n" +
                    $"{leftover.Count} student(s) will be listed on a separate 'Unallotted / Waiting List' page.\n\nDo you want to continue?",
                    "More students than tables", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                    return null;
            }

            var dateText = GetField("Date").Text.Trim();
            var fileNameStub = !string.IsNullOrEmpty(dateText)
                ? $"Hall Sheet {Regex.Replace(dateText, @"[\\/:*?\"<>|]", "-")}"
                : "Hall Sheet";

            return new Dictionary<string, object>
            {
                { "exam_name", GetField("ExamName").Text.Trim() },
                { "college_name", GetField("College").Text.Trim() },
                { "semester", GetField("Semester").Text.Trim() },
                { "date_text", dateText },
                { "time_of_exam", timeOfExam },
                { "allocation", allocation },
                { "leftover", leftover },
                { "excluded_count", excludedCount },
                { "filename_stub", fileNameStub }
            };
        }

        private string FindMatchingColumn(string paperCode)
        {
            var target = paperCode.Replace(" ", "").ToLower();
            return worksheetData.Keys.FirstOrDefault(h => h.Replace(" ", "").ToLower() == target);
        }

        private (Dictionary<string, List<(string, string, string)>>, List<(string, string, string)>) 
            AllocateStudentsToHalls(List<(string, string, string)> rollNumbers, List<(string, int)> hallCounts)
        {
            var allocation = new Dictionary<string, List<(string, string, string)>>();
            int idx = 0;

            foreach (var (hallName, count) in hallCounts)
            {
                var seats = new List<(string, string, string)>();
                for (int i = 0; i < count; i++)
                {
                    if (idx < rollNumbers.Count)
                    {
                        seats.Add(rollNumbers[idx++]);
                    }
                }
                allocation[hallName] = seats;
            }

            var leftover = rollNumbers.Skip(idx).ToList();
            return (allocation, leftover);
        }

        private void BuildPDF(string filePath, Dictionary<string, object> data)
        {
            var writer = new PdfWriter(filePath);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            var examName = data["exam_name"]?.ToString() ?? "";
            var headerText = !string.IsNullOrEmpty(examName) ? examName : DEFAULT_EXAM_NAME;

            var allocation = (Dictionary<string, List<(string, string, string)>>)data["allocation"];
            var leftover = (List<(string, string, string)>)data["leftover"];
            var collegeName = data["college_name"]?.ToString() ?? "";
            var semester = data["semester"]?.ToString() ?? "";
            var dateText = data["date_text"]?.ToString() ?? "";
            var timeOfExam = data["time_of_exam"]?.ToString() ?? "";

            bool firstPage = true;
            foreach (var roomName in ROOM_NAMES)
            {
                if (!allocation.TryGetValue(roomName, out var seats) || seats.Count == 0)
                    continue;

                if (!firstPage)
                    document.Add(new AreaBreak());

                AddPageHeader(document, headerText, collegeName, roomName, timeOfExam, dateText, semester, seats);
                firstPage = false;
            }

            if (leftover.Count > 0)
            {
                if (!firstPage)
                    document.Add(new AreaBreak());
                AddWaitingListPage(document, headerText, collegeName, leftover);
            }

            document.Close();
        }

        private void AddPageHeader(Document document, string examName, string collegeName, string hallName,
            string timeOfExam, string dateText, string semester, List<(string, string, string)> seats)
        {
            document.Add(new Paragraph(examName)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(13)
                .SetBold());

            if (!string.IsNullOrEmpty(collegeName))
            {
                document.Add(new Paragraph(collegeName)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(12)
                    .SetBold());
            }

            document.Add(new Paragraph(hallName)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(12)
                .SetBold());

            var timeDateLine = $"TIME: {timeOfExam}" + (!string.IsNullOrEmpty(dateText) ? $"    Date: {dateText}" : "");
            document.Add(new Paragraph(timeDateLine)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(11)
                .SetBold());

            if (!string.IsNullOrEmpty(semester))
            {
                document.Add(new Paragraph($"SEMESTER: {semester}")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(11)
                    .SetBold());
            }

            document.Add(new Paragraph($"Subject/Paper: {BuildSubjectLine(seats)}")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(11)
                .SetBold());

            document.Add(new Paragraph(""));
        }

        private void AddWaitingListPage(Document document, string examName, string collegeName, 
            List<(string, string, string)> leftover)
        {
            document.Add(new Paragraph(examName)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(13)
                .SetBold());

            if (!string.IsNullOrEmpty(collegeName))
            {
                document.Add(new Paragraph(collegeName)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(12)
                    .SetBold());
            }

            document.Add(new Paragraph("Unallotted / Waiting List")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(12)
                .SetBold());

            document.Add(new Paragraph($"Subject/Paper: {BuildSubjectLine(leftover)}")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(11)
                .SetBold());
        }

        private string BuildSubjectLine(List<(string, string, string)> seats)
        {
            var codes = new List<string>();
            foreach (var (roll, code, marker) in seats)
            {
                if (!codes.Contains(code))
                    codes.Add(code);
            }
            return string.Join(", ", codes.Select(c => $"{SubjectForCode(c)} ({c})"));
        }

        private string SubjectForCode(string code)
        {
            var codeUpper = code.Trim().ToUpper();
            if (SUBJECT_EXACT_CODES.ContainsKey(codeUpper))
                return SUBJECT_EXACT_CODES[codeUpper];

            foreach (var kvp in SUBJECT_ABBREVIATIONS.OrderByDescending(x => x.Key.Length))
            {
                if (codeUpper.StartsWith(kvp.Key))
                    return kvp.Value;
            }

            return code;
        }

        private bool IsExpelledSuffix(string roll)
        {
            return roll.Trim().ToLower().EndsWith("expl");
        }

        private void BuildDocx(string filePath, Dictionary<string, object> data)
        {
            // Word generation will be implemented in next phase
            MessageBox.Show("Word generation feature coming soon!", "Info",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
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
