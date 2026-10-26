using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.Model;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static NPOI.HSSF.Util.HSSFColor;

namespace Student_Manager
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer timer;

        public Form1()
        {
            InitializeComponent();
            ApplyModernTheme();

            this.Load += Form1_Load;
            OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            // Set ActiveLbl and InactiveLbl labels to invisible
            ActiveLbl.Visible = false;
            InactiveLbl.Visible = false;
            // Initialize the timer
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000; // 1 second interval
            timer.Tick += Timer_Tick; // Event handler for timer tick
            timer.Start(); // Start the timer
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateTime(); // Update the time label every second
        }

        private void UpdateTime()
        {
            DateTime currentTime = DateTime.Now;
            string formattedDateTime = currentTime.ToString("ddd d MMM HH:mm");
            timelbl.Text = formattedDateTime;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateTimeLabel(); // Call UpdateTimeLabel during form load
            CustomizeColumnHeadersWrapMode(); // Apply customization for column headers wrap mode, alignment, and width
            dataGridView1.DataSource = dataTable;

        }

        private Button selectedButton = null;

        private void UnbookedStudentsButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            if (selectedButton != clickedButton)
            {
                if (selectedButton != null)
                {
                    selectedButton.BackColor = SystemColors.Control;
                }

                clickedButton.BackColor = Color.Red;
                selectedButton = clickedButton;
            }

            DataTable filteredDataTable = dataTable.Clone();
            foreach (DataRow row in dataTable.Rows)
            {
                if (HasNoNextEncounter(row))
                {
                    filteredDataTable.ImportRow(row);
                }
            }

            dataGridView1.DataSource = filteredDataTable;
            dataGridView1.Refresh();
        }

        private static bool HasNoNextEncounter(DataRow row)
        {
            if (row == null || !row.Table.Columns.Contains("Next ENC") || row.IsNull("Next ENC"))
            {
                return true;
            }

            object value = row["Next ENC"];
            if (value is DateTime)
            {
                return (DateTime)value == DateTime.MinValue;
            }

            DateTime parsedDate;
            return string.IsNullOrWhiteSpace(Convert.ToString(value))
                || !DateTime.TryParse(Convert.ToString(value), out parsedDate)
                || parsedDate == DateTime.MinValue;
        }

        private void fileselectbtn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel workbooks (*.xls;*.xlsx)|*.xls;*.xlsx";
                openFileDialog.Title = "Choose a student action list";
                if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    string inputFilePath = openFileDialog.FileName;
                    try
                    {
                        UseWaitCursor = true;
                        fileselectbtn.Enabled = false;
                        selectedFileLbl.Text = "Loading " + Path.GetFileName(inputFilePath) + "...";
                        Refresh();

                        if (!File.Exists(inputFilePath))
                        {
                            throw new FileNotFoundException("The selected file could not be found.", inputFilePath);
                        }

                        string workbookPath = inputFilePath;
                        if (string.Equals(Path.GetExtension(inputFilePath), ".xls", StringComparison.OrdinalIgnoreCase))
                        {
                            workbookPath = Path.ChangeExtension(inputFilePath, ".xlsx");
                            ConvertToXlsx(inputFilePath, workbookPath);
                        }

                        LoadDataFromExcel(workbookPath);
                        selectedFileLbl.Text = Path.GetFileName(inputFilePath) + "  •  " + dataTable.Rows.Count + " students loaded";
                    }
                    catch (Exception ex)
                    {
                        selectedFileLbl.Text = "Import failed — choose another action list";
                        MessageBox.Show(this, ex.Message, "Could not import action list", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        fileselectbtn.Enabled = true;
                        UseWaitCursor = false;
                    }
                }
            }
        }

        private void UpdateTimeLabel()
        {
            UpdateTime();
        }

        private void ConvertToXlsx(string inputFilePath, string outputFilePath)
        {
            using (FileStream fileStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read))
            {
                HSSFWorkbook hssfwb = new HSSFWorkbook(fileStream);
                XSSFWorkbook xssfwb = new XSSFWorkbook();
                XSSFSheet sheet = (XSSFSheet)xssfwb.CreateSheet("Sheet1");

                for (int i = 0; i < hssfwb.NumberOfSheets; i++)
                {
                    HSSFSheet hssfSheet = (HSSFSheet)hssfwb.GetSheetAt(i);
                    foreach (IRow row in hssfSheet)
                    {
                        XSSFRow xssfRow = (XSSFRow)sheet.CreateRow(row.RowNum);
                        for (int j = 0; j < row.LastCellNum; j++)
                        {
                            ICell cell = row.GetCell(j);
                            if (cell != null)
                            {
                                if (cell.CellType == CellType.Numeric && DateUtil.IsCellDateFormatted(cell))
                                {
                                    // Handle date cells
                                    DateTime dateValue = DateTime.FromOADate(cell.NumericCellValue);

                                    // Create a cell in the XLSX file and set the date value
                                    XSSFCell xssfCell = (XSSFCell)xssfRow.CreateCell(j);
                                    xssfCell.SetCellValue(dateValue);
                                    xssfCell.CellStyle.DataFormat = new XSSFDataFormat(new StylesTable()).GetFormat("dd-MM-yyyy hh:mm");
                                }
                                else
                                {
                                    // For non-date cells, copy the value as is
                                    xssfRow.CreateCell(j).SetCellValue(cell.ToString());
                                }
                            }
                        }
                    }
                }

                using (FileStream fs = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write))
                {
                    xssfwb.Write(fs);
                }
            }
        }//Method to convert .xls to .xlsx
        private DataTable dataTable = new DataTable();

        private void LoadDataFromExcel(string filePath)
        {
            // Set ActiveLbl text to "Active Students:" in black color and make it visible
            ActiveLbl.Text = "Active Students:";
            ActiveLbl.ForeColor = MutedTextColor;
            ActiveLbl.Visible = true;

            // Set InactiveLbl text to "Inactive Students:" in black color and make it visible
            InactiveLbl.Text = "Inactive Students:";
            InactiveLbl.ForeColor = MutedTextColor;
            InactiveLbl.Visible = true;

            // Load data from Excel file
            using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
            {
                if (package.Workbook.Worksheets.Count > 0) // Check if there are any worksheets
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Access the first worksheet (zero-based index)

                    // Clear existing data in the DataTable
                    dataTable.Clear();

                    // Add columns to the DataTable
                    dataTable.Columns.Clear();
                    dataTable.Columns.Add("Called", typeof(bool));
                    dataTable.Columns.Add("Student Code");
                    dataTable.Columns.Add("First Name");
                    dataTable.Columns.Add("Last Name");
                    dataTable.Columns.Add("Current Unit");
                    dataTable.Columns.Add("Next ENC", typeof(DateTime)); // Specify DateTime type for date columns
                    dataTable.Columns.Add("Recent Follow-up Date", typeof(DateTime)); // Specify DateTime type for date columns
                    dataTable.Columns.Add("Last ENC Result");
                    dataTable.Columns.Add("Last ENC Date", typeof(DateTime)); // Specify DateTime type for date columns
                    dataTable.Columns.Add("# Days Since Last Study");
                    dataTable.Columns.Add("# Days Since Last Encounter");
                    dataTable.Columns.Add("Access Type");
                    dataTable.Columns.Add("Booking Type");
                    dataTable.Columns.Add("Next CC / SC date", typeof(DateTime)); // Specify DateTime type for date columns
                    dataTable.Columns.Add("Contract Status");
                    dataTable.Columns.Add("Coach");
                    dataTable.Columns.Add("EC");
                    dataTable.Columns.Add("Contract Start", typeof(DateTime)); // Specify DateTime type for date columns
                    dataTable.Columns.Add("Contract End", typeof(DateTime)); // Specify DateTime type for date columns
                    dataTable.Columns.Add("Multimedia Bookmarks");
                    dataTable.Columns.Add("Recent Follow-Up Notes");
                    dataTable.Columns.Add("Phone Number");

                    // Add rows to the DataTable
                    for (int rowNum = 8; rowNum <= worksheet.Dimension.End.Row; rowNum++)
                    {
                        string firstName = worksheet.Cells[rowNum, 2].Text; // First Name
                        string lastName = worksheet.Cells[rowNum, 1].Text; // Last Name
                        string contractStatus = worksheet.Cells[rowNum, 9].Text; // Contract Status

                        // Check if any of the values are blank
                        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(contractStatus))
                        {
                            // Skip adding the row if any value is blank
                            continue;
                        }

                        DataRow row = dataTable.Rows.Add();
                        // Add data from Excel columns to DataTable columns
                        row["Called"] = false; // Placeholder for checkbox column (Column1)
                        row["Student Code"] = worksheet.Cells[rowNum, 3].Text; // Column2
                        row["First Name"] = firstName; // Column3
                        row["Last Name"] = lastName; // Column4
                        row["Current Unit"] = worksheet.Cells[rowNum, 29].Text; // Column5
                        row["Next ENC"] = ConvertToDateTime(worksheet.Cells[rowNum, 28].Value); // Column6
                        row["Recent Follow-up Date"] = ConvertToDateTime(worksheet.Cells[rowNum, 42].Value); // Column7
                        row["Last ENC Result"] = worksheet.Cells[rowNum, 25].Text; // Column8
                        row["Last ENC Date"] = ConvertToDateTime(worksheet.Cells[rowNum, 24].Value); // Column9
                        row["# Days Since Last Study"] = worksheet.Cells[rowNum, 20].Text; // Column10
                        row["# Days Since Last Encounter"] = worksheet.Cells[rowNum, 27].Text; // Column11
                        row["Access Type"] = worksheet.Cells[rowNum, 50].Text; // Column12
                        row["Booking Type"] = worksheet.Cells[rowNum, 51].Text; // Column13
                        row["Next CC / SC date"] = ConvertToDateTime(worksheet.Cells[rowNum, 41].Value); // Column14
                        row["Contract Status"] = contractStatus;

                        // Apply cell style based on contract status
                        ApplyCellStyleBasedOnContractStatus(contractStatus, rowNum);

                        // Extract text inside brackets for "Coach" column
                        string coachValue = worksheet.Cells[rowNum, 6].Text;
                        row["Coach"] = ExtractTextInBrackets(coachValue); // Column16

                        row["EC"] = ExtractTextInBrackets(worksheet.Cells[rowNum, 7].Text); // Column17 with text in brackets extracted
                        row["Contract Start"] = ConvertToDateTime(worksheet.Cells[rowNum, 13].Value); // Column18
                        row["Contract End"] = ConvertToDateTime(worksheet.Cells[rowNum, 21].Value); // Column19
                        row["Multimedia Bookmarks"] = worksheet.Cells[rowNum, 23].Text; // Column20
                        row["Recent Follow-Up Notes"] = worksheet.Cells[rowNum, 43].Text; // Column21
                        row["Phone Number"] = worksheet.Cells[rowNum, 4].Text; // Column22

                        // Debugging: Output contract status for each row
                        Console.WriteLine($"Row {rowNum}: Contract Status = {contractStatus}");
                    }

                    // Filter the dataTable to show only rows where "Contract Status" is "Active"
                    var activeRows = dataTable.AsEnumerable()
                                              .Where(row => row.Field<string>("Contract Status").Equals("Active", StringComparison.OrdinalIgnoreCase));

                    // Update ActiveLbl label with the count of active students in green font
                    int activeStudentsCount = activeRows.Count();
                    ActiveLbl.Text += $" {activeStudentsCount}";
                    ActiveLbl.ForeColor = ActiveGreen; // Set font color to green

                    // Filter the dataTable to show only rows where "Contract Status" is not "Active"
                    var inactiveRows = dataTable.AsEnumerable()
                                                .Where(row => !row.Field<string>("Contract Status").Equals("Active", StringComparison.OrdinalIgnoreCase));

                    // Update InactiveLbl label with the count of inactive students in red font
                    int inactiveStudentsCount = inactiveRows.Count();
                    InactiveLbl.Text += $" {inactiveStudentsCount}";
                    InactiveLbl.ForeColor = ErrorRed; // Set font color to red

                    // Refresh the DataGridView
                    dataGridView1.Refresh();

                    // Print the count of active and inactive students (for debugging)
                    Console.WriteLine($"Active students count: {activeStudentsCount}");
                    Console.WriteLine($"Inactive students count: {inactiveStudentsCount}");
                }
                else
                {
                    MessageBox.Show("No worksheets found in the Excel file.");
                }
            }
        }//This is the logic to populate the datagrid with a table 
        private void ApplyCellStyleBasedOnContractStatus(string contractStatus, int rowNum)
        {
            DataGridViewCellStyle style = new DataGridViewCellStyle();

            if (contractStatus.Equals("Active", StringComparison.OrdinalIgnoreCase))
            {
                style.ForeColor = ActiveGreen;
            }
            else if (contractStatus.Equals("InActive", StringComparison.OrdinalIgnoreCase))
            {
                style.ForeColor = ErrorRed;
            }
            else
            {
                // Set default style when contract status is empty
                style.ForeColor = dataGridView1.DefaultCellStyle.ForeColor;
            }

            // Apply style to the corresponding row in the DataGridView
            int dataGridViewRowIndex = rowNum - 8; // Adjust the index
            if (dataGridViewRowIndex >= 0 && dataGridViewRowIndex < dataGridView1.Rows.Count)
            {
                dataGridView1.Rows[dataGridViewRowIndex].Cells["Contract Status"].Style = style;
            }
        }//Method to apply color to cells
        private string ExtractTextInBrackets(string text)
        {
            // Check if the text contains brackets
            if (text.Contains("(") && text.Contains(")"))
            {
                // Find the position of the opening bracket
                int startIndex = text.IndexOf("(");
                // Find the position of the closing bracket
                int endIndex = text.IndexOf(")");

                // Check if the closing bracket comes after the opening bracket
                if (endIndex > startIndex)
                {
                    // Extract the text between the brackets
                    return text.Substring(startIndex + 1, endIndex - startIndex - 1).Trim();
                }
            }

            // Return an empty string if no text is found between the brackets
            return string.Empty;
        }//Method to take name out of brackets for coaches and EC's
        private DateTime? ConvertToDateTime(object value)
        {
            if (value != null)
            {
                if (value is double excelDateValue)
                {
                    // Handle Excel date value
                    try
                    {
                        return DateTime.FromOADate(excelDateValue);
                    }
                    catch (ArgumentException)
                    {
                        // Failed to convert to DateTime, return DateTime.MinValue
                        return DateTime.MinValue;
                    }
                }
                else if (value is string stringValue)
                {
                    // Handle string date value
                    if (DateTime.TryParseExact(stringValue, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                    {
                        return result;
                    }
                    else
                    {
                        // If parsing fails, return DateTime.MinValue
                        return DateTime.MinValue;
                    }
                }
            }

            // Return DateTime.MinValue for null value or unsupported format
            return DateTime.MinValue;
        } // Method to format date and time correctly
        private void RemoveRowsWithEmptyStudentCode()
        {
            // Index of the "Student Code" column
            int studentCodeColumnIndex = dataGridView1.Columns["Student Code"].Index;

            // Iterate through the rows in reverse order
            for (int i = dataGridView1.Rows.Count - 1; i >= 0; i--)
            {
                // Check if the Student Code cell in the current row is empty
                if (string.IsNullOrWhiteSpace(dataGridView1.Rows[i].Cells[studentCodeColumnIndex].Value?.ToString()))
                {
                    // Remove the row if the Student Code is empty
                    dataGridView1.Rows.RemoveAt(i);
                }
            }
        }// Method to clean up table
        private void LoadColumnWidths()
        {
            if (dataGridView1 != null)
            {
                // Assuming you're using application settings to save column widths
                dataGridView1.Columns["Called"].Width = Properties.Settings.Default.CalledColumnWidth;
                dataGridView1.Columns["Student Code"].Width = Properties.Settings.Default.StudentCodeColumnWidth;
                dataGridView1.Columns["First Name"].Width = Properties.Settings.Default.FirstNameColumnWidth;
                dataGridView1.Columns["Last Name"].Width = Properties.Settings.Default.LastNameColumnWidth;
                dataGridView1.Columns["Current Unit"].Width = Properties.Settings.Default.CurrentUnitColumnWidth;
                dataGridView1.Columns["Next ENC"].Width = Properties.Settings.Default.NextENCColumnWidth;
                dataGridView1.Columns["Recent Follow-up Date"].Width = Properties.Settings.Default.RecentFollowUpDateColumnWidth;
                dataGridView1.Columns["Last ENC Result"].Width = Properties.Settings.Default.LastENCResultColumnWidth;
                dataGridView1.Columns["Last ENC Date"].Width = Properties.Settings.Default.LastENCDateColumnWidth;
                dataGridView1.Columns["# Days Since Last Study"].Width = Properties.Settings.Default.DaysSinceLastStudyColumnWidth;
                dataGridView1.Columns["# Days Since Last Encounter"].Width = Properties.Settings.Default.DaysSinceLastEncounterColumnWidth;
                dataGridView1.Columns["Access Type"].Width = Properties.Settings.Default.AccessTypeColumnWidth;
                dataGridView1.Columns["Booking Type"].Width = Properties.Settings.Default.BookingTypeColumnWidth;
                dataGridView1.Columns["Next CC / SC date"].Width = Properties.Settings.Default.NextCCSCDateColumnWidth;
                dataGridView1.Columns["Contract Status"].Width = Properties.Settings.Default.ContractStatusColumnWidth;
                dataGridView1.Columns["Coach"].Width = Properties.Settings.Default.CoachColumnWidth;
                dataGridView1.Columns["EC"].Width = Properties.Settings.Default.ECColumnWidth;
                dataGridView1.Columns["Contract Start"].Width = Properties.Settings.Default.ContractStartColumnWidth;
                dataGridView1.Columns["Contract End"].Width = Properties.Settings.Default.ContractEndColumnWidth;
                dataGridView1.Columns["Multimedia Bookmarks"].Width = Properties.Settings.Default.MultimediaBookmarksColumnWidth;
                dataGridView1.Columns["Recent Follow-Up Notes"].Width = Properties.Settings.Default.RecentFollowUpNotesColumnWidth;
                dataGridView1.Columns["Phone Number"].Width = Properties.Settings.Default.PhoneNumberColumnWidth;
            }
            else
            {
                // Handle the case where dataGridView1 is null or not fully initialized
                MessageBox.Show("dataGridView1 is null or not fully initialized.");
            }
        }// Method for default settings for columns widths
        private void CustomizeColumnHeadersWrapMode()
        {
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                // Apply wrap mode to column header
                column.HeaderCell.Style.WrapMode = DataGridViewTriState.True;

                // Center-align text within column header
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // Dynamically adjust column width to fit header text length
                int headerTextLength = TextRenderer.MeasureText(column.HeaderText, dataGridView1.Font).Width;
                int maxCellTextLength = 0;

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[column.Index].Value != null)
                    {
                        int cellTextLength = TextRenderer.MeasureText(row.Cells[column.Index].Value.ToString(), dataGridView1.Font).Width;
                        maxCellTextLength = Math.Max(maxCellTextLength, cellTextLength);
                    }
                }

                int preferredWidth = Math.Max(headerTextLength, maxCellTextLength) + 20; // Add padding

                column.Width = preferredWidth;
            }
        }//Method for headers wrap mode
        private DateTime ConvertExcelSerialDate(double serialDate)
        {
            // Base date for Excel serial numbers
            DateTime baseDate = new DateTime(1899, 12, 30);

            // Add the number of days (integer part) to the base date
            DateTime date = baseDate.AddDays(serialDate);

            return date;
        } //More date conversions for table
        private void ShowRowsWithTodayDate(DateTime date)
        {
            // Filter the dataTable to show only rows with the specified date in the "Next ENC" column
            var filteredRows = dataTable.AsEnumerable()
                                        .Where(row =>
                                            !row.IsNull("Next ENC") && // Check for null values
                                            row.Field<DateTime>("Next ENC").Date == date.Date);

            // Create a new DataTable to hold the filtered rows
            DataTable filteredDataTable = dataTable.Clone();

            // Add filtered rows to the new DataTable
            foreach (var row in filteredRows)
            {
                filteredDataTable.Rows.Add(row.ItemArray);
            }

            // Assign the new DataTable to the DataGridView's DataSource
            dataGridView1.DataSource = filteredDataTable;

            // Refresh the DataGridView
            dataGridView1.Refresh();

            // Print the count of filtered rows (for debugging)
            Console.WriteLine($"Filtered rows count: {filteredDataTable.Rows.Count}");
        }//Logic for encounters today
        private void ENCtodaybtn_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            // Check if the clicked button is already selected
            if (clickedButton != selectedButton)
            {
                // Deselect the previously selected button (if any)
                if (selectedButton != null)
                {
                    selectedButton.BackColor = SystemColors.Control;
                }

                // Select the clicked button and change its color to green
                clickedButton.BackColor = Color.Green;
                selectedButton = clickedButton;

                // Perform the desired action here
            }
            else
            {
                // Check if the button is currently green
                if (clickedButton.BackColor == Color.Green)
                {
                    // Keep the button highlighted (green)
                    clickedButton.BackColor = Color.Green;
                }
                else
                {
                    // Deselect the previously selected button
                    if (selectedButton != null)
                    {
                        selectedButton.BackColor = SystemColors.Control;
                    }
                }
            }



            ShowRowsWithTodayDate(DateTime.Today);
        }// Method for checking encounters today
        private void ENC1daybtn_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            // Check if the clicked button is different from the previously selected button
            if (clickedButton != selectedButton)
            {
                // Deselect the previously selected button (if any)
                if (selectedButton != null)
                {
                    selectedButton.BackColor = SystemColors.Control;
                }

                // Select the clicked button and change its color to green
                clickedButton.BackColor = Color.Green;
                selectedButton = clickedButton;

                // Perform the desired action here
            }
            else
            {
                // Check if the button is currently green
                if (clickedButton.BackColor == Color.Green)
                {
                    // Keep the button highlighted (green)
                    clickedButton.BackColor = Color.Green;
                }
                else
                {
                    // Deselect the previously selected button
                    if (selectedButton != null)
                    {
                        selectedButton.BackColor = SystemColors.Control;
                    }
                }
            }

            ShowRowsWithTodayDate(DateTime.Today.AddDays(1));
        }// Method for checking encounters tomorrow
        private void ENC4daybtn_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            // Check if the clicked button is different from the previously selected button
            if (clickedButton != selectedButton)
            {
                // Deselect the previously selected button (if any)
                if (selectedButton != null)
                {
                    selectedButton.BackColor = SystemColors.Control;
                }

                // Select the clicked button and change its color to green
                clickedButton.BackColor = Color.Green;
                selectedButton = clickedButton;

                // Perform the desired action here
            }
            else
            {
                // Check if the button is currently green
                if (clickedButton.BackColor == Color.Green)
                {
                    // Keep the button highlighted (green)
                    clickedButton.BackColor = Color.Green;
                }
                else
                {
                    // Deselect the previously selected button
                    if (selectedButton != null)
                    {
                        selectedButton.BackColor = SystemColors.Control;
                    }
                }
            }

            ShowRowsWithTodayDate(DateTime.Today.AddDays(3));
        }// Method for checking encounters in 3 days counting 3 days from today 
        private void ENCweekbtn_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            // Check if the clicked button is different from the previously selected button
            if (clickedButton != selectedButton)
            {
                // Deselect the previously selected button (if any)
                if (selectedButton != null)
                {
                    selectedButton.BackColor = SystemColors.Control;
                }

                // Select the clicked button and change its color to green
                clickedButton.BackColor = Color.Green;
                selectedButton = clickedButton;

                // Perform the desired action here
            }
            else
            {
                // Check if the button is currently green
                if (clickedButton.BackColor == Color.Green)
                {
                    // Keep the button highlighted (green)
                    clickedButton.BackColor = Color.Green;
                }
                else
                {
                    // Deselect the previously selected button
                    if (selectedButton != null)
                    {
                        selectedButton.BackColor = SystemColors.Control;
                    }
                }
            }

            ShowRowsWithTodayDate(DateTime.Today.AddDays(7));
        }// Method for checking encounters in  1 week
        private void button10_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            // Check if the clicked button is different from the previously selected button
            if (clickedButton != selectedButton)
            {
                // Deselect the previously selected button (if any)
                if (selectedButton != null)
                {
                    selectedButton.BackColor = SystemColors.Control;
                }

                // Select the clicked button and change its color to green
                clickedButton.BackColor = Color.Green;
                selectedButton = clickedButton;

                // Perform the desired action here
            }
            else
            {
                // Check if the button is currently green
                if (clickedButton.BackColor == Color.Green)
                {
                    // Keep the button highlighted (green)
                    clickedButton.BackColor = Color.Green;
                }
                else
                {
                    // Deselect the previously selected button
                    if (selectedButton != null)
                    {
                        selectedButton.BackColor = SystemColors.Control;
                    }
                }
            }

            // Reload the original data into the DataGridView
            dataGridView1.DataSource = dataTable;

            // Refresh the DataGridView
            dataGridView1.Refresh();

            // Remove any completely empty rows
            RemoveEmptyRows();

            // Print a message (optional, for debugging)
            Console.WriteLine("Original data loaded without empty rows.");
        }//Method for showing all students
        private void RemoveEmptyRows()
        {

            // Iterate through the rows in reverse order to avoid index issues when removing rows
            for (int i = dataGridView1.Rows.Count - 1; i >= 0; i--)
            {
                bool isEmptyRow = true;

                // Check if all visible cells in the current row are empty or contain only whitespace
                foreach (DataGridViewCell cell in dataGridView1.Rows[i].Cells)
                {
                    if (cell.Visible && cell.Value != null && !string.IsNullOrWhiteSpace(cell.Value.ToString()))
                    {
                        // If any visible cell is not empty or contains non-whitespace characters, the row is not empty
                        isEmptyRow = false;
                        break;
                    }
                }

                // If the row is completely empty, remove it
                if (isEmptyRow)
                {
                    dataGridView1.Rows.RemoveAt(i);
                }
            }
        } //Method to remove rows 
        private void button1_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            // Check if the clicked button is different from the previously selected button
            if (clickedButton != selectedButton)
            {
                // Deselect the previously selected button (if any)
                if (selectedButton != null)
                {
                    selectedButton.BackColor = SystemColors.Control;
                }

                // Select the clicked button and change its color to green
                clickedButton.BackColor = Color.Green;
                selectedButton = clickedButton;

                // Perform the desired action here
            }
            else
            {
                // Check if the button is currently green
                if (clickedButton.BackColor == Color.Green)
                {
                    // Keep the button highlighted (green)
                    clickedButton.BackColor = Color.Green;
                }
                else
                {
                    // Deselect the previously selected button
                    if (selectedButton != null)
                    {
                        selectedButton.BackColor = SystemColors.Control;
                    }
                }
            }

            // Filter the dataTable to show only rows where "Last ENC Date" is blank and "First Name" is not blank
            var filteredRows = dataTable.AsEnumerable()
                                            .Where(row =>
                                                (row.IsNull("Last ENC Date") || row.Field<DateTime>("Last ENC Date") == DateTime.MinValue) && // Check for null or DateTime.MinValue
                                                !string.IsNullOrEmpty(row.Field<string>("First Name"))); // Check for non-empty First Name

            // Create a new DataTable to hold the filtered rows
            DataTable filteredDataTable = dataTable.Clone();

            // Add filtered rows to the new DataTable
            foreach (var row in filteredRows)
            {
                filteredDataTable.Rows.Add(row.ItemArray);
            }

            // Sort the rows by the "Next ENC" date column
            var sortedRows = filteredDataTable.AsEnumerable()
                                               .OrderBy(row => (row.Field<DateTime>("Next ENC") - DateTime.Today).TotalDays);

            // Create a new DataTable to hold the sorted rows
            DataTable sortedDataTable = filteredDataTable.Clone();

            // Add sorted rows to the new DataTable
            foreach (var row in sortedRows)
            {
                sortedDataTable.Rows.Add(row.ItemArray);
            }

            // Assign the new DataTable to the DataGridView's DataSource
            dataGridView1.DataSource = sortedDataTable;

            // Refresh the DataGridView
            dataGridView1.Refresh();

            // Print the count of filtered rows (for debugging)
            Console.WriteLine($"Filtered and sorted rows count: {sortedDataTable.Rows.Count}");
        }//Method to check for first encounters
        private void button2_Click(object sender, EventArgs e)
        {

            Button clickedButton = (Button)sender;

            // Check if the clicked button is different from the previously selected button
            if (clickedButton != selectedButton)
            {
                // Deselect the previously selected button (if any)
                if (selectedButton != null)
                {
                    selectedButton.BackColor = SystemColors.Control;
                }

                // Select the clicked button and change its color to green
                clickedButton.BackColor = Color.Green;
                selectedButton = clickedButton;

                // Perform the desired action here
            }
            else
            {
                // Check if the button is currently green
                if (clickedButton.BackColor == Color.Green)
                {
                    // Keep the button highlighted (green)
                    clickedButton.BackColor = Color.Green;
                }
                else
                {
                    // Deselect the previously selected button
                    if (selectedButton != null)
                    {
                        selectedButton.BackColor = SystemColors.Control;
                    }
                }
            }

            // Get today's date
            DateTime today = DateTime.Today;

            // Filter the dataTable to show only rows where "Next ENC" is today and "Current Unit" is a multiple of 4
            var filteredRows = dataTable.AsEnumerable()
                                        .Where(row =>
                                            row.Field<DateTime>("Next ENC").Date == today.Date &&
                                            IsMultipleOfFour(Convert.ToInt32(row["Current Unit"])));

            // Create a new DataTable to hold the filtered rows
            DataTable filteredDataTable = dataTable.Clone();

            // Add filtered rows to the new DataTable
            foreach (var row in filteredRows)
            {
                filteredDataTable.Rows.Add(row.ItemArray);
            }

            // Assign the new DataTable to the DataGridView's DataSource
            dataGridView1.DataSource = filteredDataTable;

            // Refresh the DataGridView
            dataGridView1.Refresh();

            // Print the count of filtered rows (for debugging)
            Console.WriteLine($"Filtered rows count: {filteredDataTable.Rows.Count}");
        }//Method to check advising sessions for today
        private bool IsMultipleOfFour(int number)
        {
            return number % 4 == 0;
        } // Method to check if a number is a multiple of 4
        private void fllwupcall7btn_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            // Check if the clicked button is different from the previously selected button
            if (clickedButton != selectedButton)
            {
                // Deselect the previously selected button (if any)
                if (selectedButton != null)
                {
                    selectedButton.BackColor = SystemColors.Control;
                }

                // Select the clicked button and change its color to green
                clickedButton.BackColor = Color.Green;
                selectedButton = clickedButton;

                // Perform the desired action here
            }
            else
            {
                // Check if the button is currently green
                if (clickedButton.BackColor == Color.Green)
                {
                    // Keep the button highlighted (green)
                    clickedButton.BackColor = Color.Green;
                }
                else
                {
                    // Deselect the previously selected button
                    if (selectedButton != null)
                    {
                        selectedButton.BackColor = SystemColors.Control;
                    }
                }
            }

            // Get today's date
            DateTime today = DateTime.Today;

            // Get the date 7 days ago
            DateTime sevenDaysAgo = today.AddDays(-7);

            // Filter the dataTable to show only rows where "# Days Since Last Study" is 7 days from today
            var filteredRows = dataTable.AsEnumerable()
                                        .Where(row =>
                                            row.Field<DateTime>("Next ENC").Date == sevenDaysAgo.Date);

            // Create a new DataTable to hold the filtered rows
            DataTable filteredDataTable = dataTable.Clone();

            // Add filtered rows to the new DataTable
            foreach (var row in filteredRows)
            {
                filteredDataTable.Rows.Add(row.ItemArray);
            }

            // Assign the new DataTable to the DataGridView's DataSource
            dataGridView1.DataSource = filteredDataTable;

            // Refresh the DataGridView
            dataGridView1.Refresh();

            // Print the count of filtered rows (for debugging)
            Console.WriteLine($"Filtered rows count: {filteredDataTable.Rows.Count}");
        }//Method for 7 days follow-up call
        private void fllwupcall10btn_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            // Check if the clicked button is different from the previously selected button
            if (clickedButton != selectedButton)
            {
                // Deselect the previously selected button (if any)
                if (selectedButton != null)
                {
                    selectedButton.BackColor = SystemColors.Control;
                }

                // Select the clicked button and change its color to green
                clickedButton.BackColor = Color.Yellow;
                selectedButton = clickedButton;

                // Perform the desired action here
            }
            else
            {
                // Check if the button is currently green
                if (clickedButton.BackColor == Color.Yellow)
                {
                    // Keep the button highlighted (green)
                    clickedButton.BackColor = Color.Yellow;
                }
                else
                {
                    // Deselect the previously selected button
                    if (selectedButton != null)
                    {
                        selectedButton.BackColor = SystemColors.Control;
                    }
                }
            }

            // Get today's date
            DateTime today = DateTime.Today;

            // Get the date 10 days ago
            DateTime tenDaysAgo = today.AddDays(-10);

            // Filter the dataTable to show only rows where "# Days Since Last Study" is 10 days ago from today
            var filteredRows = dataTable.AsEnumerable()
                                        .Where(row =>
                                            !string.IsNullOrWhiteSpace(row.Field<string>("First Name")) && // Check if "First Name" is not empty
                                            int.TryParse(row.Field<string>("# Days Since Last Study"), out int daysSinceLastStudy) && // Convert "# Days Since Last Study" to int
                                            daysSinceLastStudy == 10);

            // Create a new DataTable to hold the filtered rows
            DataTable filteredDataTable = dataTable.Clone();

            // Add filtered rows to the new DataTable
            foreach (var row in filteredRows)
            {
                filteredDataTable.Rows.Add(row.ItemArray);
            }

            // Assign the new DataTable to the DataGridView's DataSource
            dataGridView1.DataSource = filteredDataTable;

            // Refresh the DataGridView
            dataGridView1.Refresh();

            // Print the count of filtered rows (for debugging)
            Console.WriteLine($"Filtered rows count: {filteredDataTable.Rows.Count}");
        }//Method for 10 days follow-up call
        private void fllwupcall14btn_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            // Check if the clicked button is different from the previously selected button
            if (clickedButton != selectedButton)
            {
                // Deselect the previously selected button (if any)
                if (selectedButton != null)
                {
                    selectedButton.BackColor = SystemColors.Control;
                }

                // Select the clicked button and change its color to green
                clickedButton.BackColor = Color.Red;
                selectedButton = clickedButton;

                // Perform the desired action here
            }
            else
            {
                // Check if the button is currently Red
                if (clickedButton.BackColor == Color.Red)
                {
                    // Keep the button highlighted (Red)
                    clickedButton.BackColor = Color.Red;
                }
                else
                {
                    // Deselect the previously selected button
                    if (selectedButton != null)
                    {
                        selectedButton.BackColor = SystemColors.Control;
                    }
                }
            }

            // Get today's date
            DateTime today = DateTime.Today;

            // Filter the dataTable to show only rows where "# Days Since Last Study" is 14 days or more from today
            var filteredRows = dataTable.AsEnumerable()
                                        .Where(row =>
                                            !string.IsNullOrWhiteSpace(row.Field<string>("First Name")) && // Check if "First Name" is not empty
                                            int.TryParse(row.Field<string>("# Days Since Last Study"), out int daysSinceLastStudy) && // Convert "# Days Since Last Study" to int
                                            daysSinceLastStudy >= 14);

            // Create a new DataTable to hold the filtered rows
            DataTable filteredDataTable = dataTable.Clone();

            // Add filtered rows to the new DataTable
            foreach (var row in filteredRows)
            {
                filteredDataTable.Rows.Add(row.ItemArray);
            }

            // Assign the new DataTable to the DataGridView's DataSource
            dataGridView1.DataSource = filteredDataTable;

            // Refresh the DataGridView
            dataGridView1.Refresh();

            // Print the count of filtered rows (for debugging)
            Console.WriteLine($"Filtered rows count: {filteredDataTable.Rows.Count}");
        }//Method for 14+ days follow-up call
        private void button11_Click(object sender, EventArgs e)
        {

            Button clickedButton = (Button)sender;

            // Check if the clicked button is different from the previously selected button
            if (clickedButton != selectedButton)
            {
                // Deselect the previously selected button (if any)
                if (selectedButton != null)
                {
                    selectedButton.BackColor = SystemColors.Control;
                }

                // Select the clicked button and change its color to green
                clickedButton.BackColor = Color.Green;
                selectedButton = clickedButton;

                // Perform the desired action here
            }
            else
            {
                // Check if the button is currently green
                if (clickedButton.BackColor == Color.Green)
                {
                    // Keep the button highlighted (green)
                    clickedButton.BackColor = Color.Green;
                }
                else
                {
                    // Deselect the previously selected button
                    if (selectedButton != null)
                    {
                        selectedButton.BackColor = SystemColors.Control;
                    }
                }
            }

            // Filter the dataTable to show only rows where "Contract Status" is "Active"
            var filteredRows = dataTable.AsEnumerable()
                                            .Where(row =>
                                                row.Field<string>("Contract Status").Equals("Active", StringComparison.OrdinalIgnoreCase));

            // Create a new DataTable to hold the filtered rows
            DataTable filteredDataTable = dataTable.Clone();

            // Add filtered rows to the new DataTable
            foreach (var row in filteredRows)
            {
                filteredDataTable.Rows.Add(row.ItemArray);
            }

            // Assign the new DataTable to the DataGridView's DataSource
            dataGridView1.DataSource = filteredDataTable;

            // Refresh the DataGridView
            dataGridView1.Refresh();

            // Print the count of filtered rows (for debugging)
            Console.WriteLine($"Filtered rows count: {filteredDataTable.Rows.Count}");
        }//Method to show Active students only
        private void button12_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            // Check if the clicked button is different from the previously selected button
            if (clickedButton != selectedButton)
            {
                // Deselect the previously selected button (if any)
                if (selectedButton != null)
                {
                    selectedButton.BackColor = SystemColors.Control;
                }

                // Select the clicked button and change its color to RED
                clickedButton.BackColor = Color.Red;
                selectedButton = clickedButton;

                // Perform the desired action here
            }
            else
            {
                // Check if the button is currently RED
                if (clickedButton.BackColor == Color.Red)
                {
                    // Keep the button highlighted RED
                    clickedButton.BackColor = Color.Red;
                }
                else
                {
                    // Deselect the previously selected button
                    if (selectedButton != null)
                    {
                        selectedButton.BackColor = SystemColors.Control;
                    }
                }
            }


            // Filter the dataTable to show only rows where "Contract Status" is "InActive"
            var filteredRows = dataTable.AsEnumerable()
                                            .Where(row =>
                                                row.Field<string>("Contract Status").Equals("InActive", StringComparison.OrdinalIgnoreCase));

            // Create a new DataTable to hold the filtered rows
            DataTable filteredDataTable = dataTable.Clone();

            // Add filtered rows to the new DataTable
            foreach (var row in filteredRows)
            {
                filteredDataTable.Rows.Add(row.ItemArray);
            }

            // Assign the new DataTable to the DataGridView's DataSource
            dataGridView1.DataSource = filteredDataTable;

            // Refresh the DataGridView
            dataGridView1.Refresh();

            // Print the count of filtered rows (for debugging)
            Console.WriteLine($"Filtered rows count: {filteredDataTable.Rows.Count}");
        }//Method to show InActive students only
        private void button13_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            // Check if the clicked button is different from the previously selected button
            if (clickedButton != selectedButton)
            {
                // Deselect the previously selected button (if any)
                if (selectedButton != null)
                {
                    selectedButton.BackColor = SystemColors.Control;
                }

                // Select the clicked button and change its color to Green
                clickedButton.BackColor = Color.Green;
                selectedButton = clickedButton;

                // Perform the desired action here
            }
            else
            {
                // Check if the button is currently Green
                if (clickedButton.BackColor == Color.Green)
                {
                    // Keep the button highlighted Green
                    clickedButton.BackColor = Color.Green;
                }
                else
                {
                    // Deselect the previously selected button
                    if (selectedButton != null)
                    {
                        selectedButton.BackColor = SystemColors.Control;
                    }
                }
            }
            // Filter the dataTable to show only rows where "Access Type" is "In Center & Online"
            var filteredRows = dataTable.AsEnumerable()
                                        .Where(row => row.Field<string>("Access Type").Equals("In Center & Online", StringComparison.OrdinalIgnoreCase));

            // Create a new DataTable to hold the filtered rows
            DataTable filteredDataTable = dataTable.Clone();

            // Add filtered rows to the new DataTable
            foreach (var row in filteredRows)
            {
                filteredDataTable.Rows.Add(row.ItemArray);
            }

            // Assign the new DataTable to the DataGridView's DataSource
            dataGridView1.DataSource = filteredDataTable;

            // Refresh the DataGridView
            dataGridView1.Refresh();

            // Print the count of filtered rows (for debugging)
            Console.WriteLine($"Filtered rows count: {filteredDataTable.Rows.Count}");
        }//Method to Show only online access students
        private void button14_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            // Check if the clicked button is different from the previously selected button
            if (clickedButton != selectedButton)
            {
                // Deselect the previously selected button (if any)
                if (selectedButton != null)
                {
                    selectedButton.BackColor = SystemColors.Control;
                }

                // Select the clicked button and change its color to Green
                clickedButton.BackColor = Color.Green;
                selectedButton = clickedButton;

                // Perform the desired action here
            }
            else
            {
                // Check if the button is currently Green
                if (clickedButton.BackColor == Color.Green)
                {
                    // Keep the button highlighted Green
                    clickedButton.BackColor = Color.Green;
                }
                else
                {
                    // Deselect the previously selected button
                    if (selectedButton != null)
                    {
                        selectedButton.BackColor = SystemColors.Control;
                    }
                }
            }

            // Get today's date
            DateTime today = DateTime.Today;

            // Filter the dataTable to show only rows where "Next CC / SC date" is today and not blank
            var filteredRows = dataTable.AsEnumerable()
                                        .Where(row => !row.IsNull("Next CC / SC date") &&
                                                      row.Field<DateTime>("Next CC / SC date").Date == today);

            // Create a new DataTable to hold the filtered rows
            DataTable filteredDataTable = dataTable.Clone();

            // Add filtered rows to the new DataTable
            foreach (var row in filteredRows)
            {
                filteredDataTable.Rows.Add(row.ItemArray);
            }

            // Assign the new DataTable to the DataGridView's DataSource
            dataGridView1.DataSource = filteredDataTable;

            // Refresh the DataGridView
            dataGridView1.Refresh();

            // Print the count of filtered rows (for debugging)
            Console.WriteLine($"Filtered rows count: {filteredDataTable.Rows.Count}");


        }//Method to Check SC & CC
        private void ShowRowsWithSearchText(string searchText)
        {
            // Convert the search text to lowercase for case-insensitive comparison
            string searchTextLower = searchText.ToLower();

            // Filter the dataTable to show only rows where either the First Name or Last Name contains the search text
            var filteredRows = dataTable.AsEnumerable()
                .Where(row =>
                    row.Field<string>("First Name").ToLower().Contains(searchTextLower) ||
                    row.Field<string>("Last Name").ToLower().Contains(searchTextLower));

            // Create a new DataTable to hold the filtered rows
            DataTable filteredDataTable = dataTable.Clone();

            // Add filtered rows to the new DataTable
            foreach (var row in filteredRows)
            {
                filteredDataTable.Rows.Add(row.ItemArray);
            }

            // Assign the new DataTable to the DataGridView's DataSource
            dataGridView1.DataSource = filteredDataTable;

            // Refresh the DataGridView
            dataGridView1.Refresh();

            // Print the count of filtered rows (for debugging)
            Console.WriteLine($"Filtered rows count: {filteredDataTable.Rows.Count}");
        }
        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            // Call the method to filter rows based on the search text
            ShowRowsWithSearchText(SearchTextBox.Text);
        }
    }
}



