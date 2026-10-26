using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Student_Manager
{
    public partial class Form1
    {
        internal static Color AppBackground = Color.FromArgb(14, 15, 17);
        internal static Color SidebarBackground = Color.FromArgb(18, 19, 21);
        internal static Color SurfaceColor = Color.FromArgb(22, 23, 26);
        internal static Color ElevatedSurfaceColor = Color.FromArgb(28, 29, 33);
        internal static Color BorderColor = Color.FromArgb(43, 45, 51);
        internal static Color PrimaryTextColor = Color.FromArgb(239, 240, 244);
        internal static Color MutedTextColor = Color.FromArgb(142, 145, 155);
        internal static Color AccentColor = Color.FromArgb(108, 92, 231);
        internal static Color AccentHoverColor = Color.FromArgb(124, 109, 245);
        internal static Color ActiveGreen = Color.FromArgb(48, 196, 141);
        internal static Color ErrorRed = Color.FromArgb(244, 105, 105);
        internal static Color WarningAmber = Color.FromArgb(244, 196, 72);
        private static Color SelectedSuccessColor = Color.FromArgb(30, 99, 78);
        private static Color SelectedErrorColor = Color.FromArgb(111, 46, 51);
        private static Color SelectedWarningColor = Color.FromArgb(111, 88, 31);

        private Panel headerPanel;
        private Panel toolbarPanel;
        private FlowLayoutPanel sidebarStack;
        private Label pageTitleLabel;
        private Label searchLabel;
        private Button unbookedStudentsButton;
        private Button settingsButton;

        private void ApplyModernTheme()
        {
            ConfigurePalette(Properties.Settings.Default.DarkMode);
            SuspendLayout();

            Text = "Student Manager";
            BackColor = AppBackground;
            ForeColor = PrimaryTextColor;
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            MinimumSize = new Size(1100, 700);
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;

            TableLayoutPanel shell = new TableLayoutPanel();
            shell.Name = "appShell";
            shell.BackColor = AppBackground;
            shell.Dock = DockStyle.Fill;
            shell.Margin = Padding.Empty;
            shell.Padding = Padding.Empty;
            shell.ColumnCount = 2;
            shell.RowCount = 1;
            shell.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 260F));
            shell.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            shell.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Panel sidebar = BuildSidebar();
            Control mainContent = BuildMainContent();

            Controls.Clear();
            shell.Controls.Add(sidebar, 0, 0);
            shell.Controls.Add(mainContent, 1, 0);
            Controls.Add(shell);

            ConfigureGrid();
            EnableNaturalColumnSorting();
            ResumeLayout(true);
        }

        internal void SetApplicationTheme(bool darkMode)
        {
            Properties.Settings.Default.DarkMode = darkMode;
            Properties.Settings.Default.Save();
            ConfigurePalette(darkMode);
            ApplyModernTheme();
        }

        private static void ConfigurePalette(bool darkMode)
        {
            if (darkMode)
            {
                AppBackground = Color.FromArgb(14, 15, 17);
                SidebarBackground = Color.FromArgb(18, 19, 21);
                SurfaceColor = Color.FromArgb(22, 23, 26);
                ElevatedSurfaceColor = Color.FromArgb(28, 29, 33);
                BorderColor = Color.FromArgb(43, 45, 51);
                PrimaryTextColor = Color.FromArgb(239, 240, 244);
                MutedTextColor = Color.FromArgb(142, 145, 155);
                ActiveGreen = Color.FromArgb(48, 196, 141);
                ErrorRed = Color.FromArgb(244, 105, 105);
                SelectedSuccessColor = Color.FromArgb(30, 99, 78);
                SelectedErrorColor = Color.FromArgb(111, 46, 51);
                SelectedWarningColor = Color.FromArgb(111, 88, 31);
            }
            else
            {
                AppBackground = Color.FromArgb(244, 246, 250);
                SidebarBackground = Color.White;
                SurfaceColor = Color.White;
                ElevatedSurfaceColor = Color.FromArgb(236, 239, 245);
                BorderColor = Color.FromArgb(214, 219, 228);
                PrimaryTextColor = Color.Black;
                MutedTextColor = Color.Black;
                ActiveGreen = Color.FromArgb(22, 130, 93);
                ErrorRed = Color.FromArgb(205, 62, 72);
                SelectedSuccessColor = Color.FromArgb(211, 242, 229);
                SelectedErrorColor = Color.FromArgb(249, 220, 222);
                SelectedWarningColor = Color.FromArgb(250, 239, 199);
            }
        }

        private Panel BuildSidebar()
        {
            Panel sidebar = new Panel();
            sidebar.Name = "sidebarPanel";
            sidebar.BackColor = SidebarBackground;
            sidebar.Dock = DockStyle.Fill;
            sidebar.Padding = new Padding(10, 0, 10, 12);

            Panel brandPanel = new Panel();
            brandPanel.BackColor = SidebarBackground;
            brandPanel.Dock = DockStyle.Top;
            brandPanel.Height = 76;
            brandPanel.Paint += DrawBottomBorder;

            Label mark = new Label();
            mark.AutoSize = false;
            mark.BackColor = AccentColor;
            mark.ForeColor = Color.White;
            mark.Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold);
            mark.Text = "SM";
            mark.TextAlign = ContentAlignment.MiddleCenter;
            mark.Location = new Point(4, 20);
            mark.Size = new Size(38, 38);

            Label brand = new Label();
            brand.AutoSize = true;
            brand.ForeColor = PrimaryTextColor;
            brand.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            brand.Text = "Student Manager";
            brand.Location = new Point(52, 20);

            Label brandSubtitle = new Label();
            brandSubtitle.AutoSize = true;
            brandSubtitle.ForeColor = MutedTextColor;
            brandSubtitle.Font = new Font("Segoe UI", 8F);
            brandSubtitle.Text = "Student success dashboard";
            brandSubtitle.Location = new Point(53, 42);

            brandPanel.Controls.Add(mark);
            brandPanel.Controls.Add(brand);
            brandPanel.Controls.Add(brandSubtitle);

            sidebarStack = new FlowLayoutPanel();
            sidebarStack.Name = "sidebarStack";
            sidebarStack.BackColor = SidebarBackground;
            sidebarStack.Dock = DockStyle.Fill;
            sidebarStack.FlowDirection = FlowDirection.TopDown;
            sidebarStack.WrapContents = false;
            sidebarStack.AutoScroll = false;
            sidebarStack.AutoSize = false;
            sidebarStack.Margin = Padding.Empty;
            sidebarStack.Padding = new Padding(0, 10, 0, 10);

            unbookedStudentsButton = new Button();
            unbookedStudentsButton.Name = "unbookedStudentsButton";
            unbookedStudentsButton.Text = "✕   Unbooked students";
            unbookedStudentsButton.Click += UnbookedStudentsButton_Click;

            sidebarStack.Controls.Add(CreateNavigationCard("ENC BOOKINGS", "▣", 256,
                new Button[] { ENCtodaybtn, ENC1daybtn, ENC4daybtn, ENCweekbtn, unbookedStudentsButton }));
            sidebarStack.Controls.Add(CreateNavigationCard("TOUCHPOINTS", "✦", 132,
                new Button[] { button1, button2 }));
            sidebarStack.Controls.Add(CreateNavigationCard("ENCOURAGEMENT CALLS", "☎", 176,
                new Button[] { fllwupcall7btn, fllwupcall10btn, fllwupcall14btn }));
            sidebarStack.Controls.Add(CreateNavigationCard("CONTRACT MANAGEMENT", "◇", 296,
                new Button[] { button10, button11, button12, button13, button14 }));

            Panel settingsHost = new Panel();
            settingsHost.BackColor = SidebarBackground;
            settingsHost.Dock = DockStyle.Bottom;
            settingsHost.Height = 62;
            settingsHost.Padding = new Padding(2, 8, 2, 8);
            settingsHost.Paint += DrawTopBorder;

            settingsButton = new Button();
            settingsButton.Name = "settingsButton";
            settingsButton.Text = "⚙   Settings";
            StyleNavigationButton(settingsButton);
            settingsButton.Dock = DockStyle.Fill;
            settingsButton.Margin = Padding.Empty;
            settingsButton.Click += SettingsButton_Click;
            settingsHost.Controls.Add(settingsButton);

            sidebar.Controls.Add(sidebarStack);
            sidebar.Controls.Add(settingsHost);
            sidebar.Controls.Add(brandPanel);
            return sidebar;
        }

        private Control CreateNavigationCard(string title, string icon, int height, Button[] buttons)
        {
            ModernPanel card = new ModernPanel();
            card.BackColor = SurfaceColor;
            card.BorderColor = BorderColor;
            card.CornerRadius = 12;
            card.Width = 236;
            card.Height = 43;
            card.MinimumSize = new Size(236, 43);
            card.MaximumSize = new Size(236, 43);
            card.Margin = new Padding(2, 0, 2, 12);

            Button heading = new Button();
            heading.AutoSize = false;
            heading.BackColor = SurfaceColor;
            heading.ForeColor = MutedTextColor;
            heading.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);
            heading.FlatStyle = FlatStyle.Flat;
            heading.FlatAppearance.BorderSize = 0;
            heading.FlatAppearance.MouseOverBackColor = ElevatedSurfaceColor;
            heading.TextAlign = ContentAlignment.MiddleLeft;
            heading.Padding = new Padding(4, 0, 0, 0);
            heading.Text = "›   " + icon + "  " + title;
            heading.Location = new Point(9, 5);
            heading.Size = new Size(218, 32);
            heading.Cursor = Cursors.Hand;

            FlowLayoutPanel buttonStack = new FlowLayoutPanel();
            buttonStack.BackColor = SurfaceColor;
            buttonStack.FlowDirection = FlowDirection.TopDown;
            buttonStack.WrapContents = false;
            buttonStack.Location = new Point(9, 38);
            buttonStack.Size = new Size(218, height - 46);
            buttonStack.Padding = Padding.Empty;
            buttonStack.Visible = false;

            foreach (Button button in buttons)
            {
                StyleNavigationButton(button);
                buttonStack.Controls.Add(button);
            }

            card.Controls.Add(heading);
            card.Controls.Add(buttonStack);
            int expandedHeight = height;
            heading.Click += delegate
            {
                bool expand = !buttonStack.Visible;
                if (expand)
                {
                    foreach (Control sibling in sidebarStack.Controls)
                    {
                        if (sibling == card || !(sibling is ModernPanel))
                        {
                            continue;
                        }

                        FlowLayoutPanel siblingStack = null;
                        Button siblingHeader = null;
                        foreach (Control child in sibling.Controls)
                        {
                            if (child is FlowLayoutPanel)
                            {
                                siblingStack = (FlowLayoutPanel)child;
                            }
                            else if (child is Button)
                            {
                                siblingHeader = (Button)child;
                            }
                        }

                        if (siblingStack != null)
                        {
                            siblingStack.Visible = false;
                            sibling.MinimumSize = new Size(236, 43);
                            sibling.MaximumSize = new Size(236, 43);
                            sibling.Size = new Size(236, 43);
                        }

                        if (siblingHeader != null && siblingHeader.Text.StartsWith("⌄", StringComparison.Ordinal))
                        {
                            siblingHeader.Text = "›" + siblingHeader.Text.Substring(1);
                        }
                    }
                }

                buttonStack.Visible = expand;
                Size targetSize = new Size(236, expand ? expandedHeight : 43);
                card.MinimumSize = targetSize;
                card.MaximumSize = targetSize;
                card.Size = targetSize;
                heading.Text = (expand ? "⌄   " : "›   ") + icon + "  " + title;
                sidebarStack.PerformLayout();
            };
            return card;
        }

        private void StyleNavigationButton(Button button)
        {
            button.AutoSize = false;
            button.Size = new Size(218, 36);
            button.Margin = new Padding(0, 0, 0, 6);
            button.BackColor = ElevatedSurfaceColor;
            button.ForeColor = Properties.Settings.Default.DarkMode
                ? Color.FromArgb(190, 193, 201)
                : Color.Black;
            button.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = ElevatedSurfaceColor;
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(51, 47, 85);
            button.TextAlign = ContentAlignment.MiddleLeft;
            button.Padding = new Padding(10, 0, 6, 0);
            button.Cursor = Cursors.Hand;
            button.BackColorChanged += NormalizeNavigationButtonColor;
            ApplyRoundedRegion(button, 9);

            switch (button.Name)
            {
                case "ENCtodaybtn": button.Text = "●   Today"; break;
                case "ENC1daybtn": button.Text = "→   Tomorrow"; break;
                case "ENC4daybtn": button.Text = "▦   Next 3 days"; break;
                case "ENCweekbtn": button.Text = "▦   Next 7 days"; break;
                case "unbookedStudentsButton":
                    button.Text = "✕   Unbooked students";
                    button.ForeColor = ErrorRed;
                    break;
                case "button1": button.Text = "★   First encounter"; break;
                case "button2": button.Text = "◆   ADV today"; break;
                case "fllwupcall7btn": button.Text = "☎   7+ days"; break;
                case "fllwupcall10btn": button.Text = "☎   10+ days"; break;
                case "fllwupcall14btn": button.Text = "☎   14+ days"; break;
                case "button10": button.Text = "≡   All students"; break;
                case "button11": button.Text = "✓   Active students"; break;
                case "button12": button.Text = "!   Inactive students"; break;
                case "button13": button.Text = "◇   Full access"; break;
                case "button14": button.Text = "▣   SC / CC today"; break;
                case "settingsButton": button.Text = "⚙   Settings"; break;
            }
        }

        private Control BuildMainContent()
        {
            TableLayoutPanel main = new TableLayoutPanel();
            main.BackColor = AppBackground;
            main.Dock = DockStyle.Fill;
            main.Margin = Padding.Empty;
            main.Padding = Padding.Empty;
            main.ColumnCount = 1;
            main.RowCount = 4;
            main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            main.RowStyles.Add(new RowStyle(SizeType.Absolute, 76F));
            main.RowStyles.Add(new RowStyle(SizeType.Absolute, 68F));
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            main.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));

            headerPanel = new Panel();
            headerPanel.BackColor = AppBackground;
            headerPanel.Dock = DockStyle.Fill;
            headerPanel.Paint += DrawBottomBorder;

            pageTitleLabel = new Label();
            pageTitleLabel.AutoSize = true;
            pageTitleLabel.ForeColor = PrimaryTextColor;
            pageTitleLabel.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            pageTitleLabel.Text = "Students";
            pageTitleLabel.Location = new Point(20, 15);

            Label liveStatus = new Label();
            liveStatus.AutoSize = true;
            liveStatus.ForeColor = ActiveGreen;
            liveStatus.Font = new Font("Segoe UI Semibold", 8F);
            liveStatus.Text = "●  WORKSPACE";
            liveStatus.Location = new Point(110, 21);

            selectedFileLbl.AutoSize = true;
            selectedFileLbl.ForeColor = MutedTextColor;
            selectedFileLbl.Font = new Font("Segoe UI", 8.5F);
            selectedFileLbl.Text = "Import an action list to begin";
            selectedFileLbl.Location = new Point(22, 45);

            StylePrimaryButton(fileselectbtn);
            fileselectbtn.Text = "↑  Import Action List";
            fileselectbtn.Dock = DockStyle.Fill;
            fileselectbtn.Margin = new Padding(8, 0, 0, 0);

            timelbl.AutoSize = false;
            timelbl.ForeColor = MutedTextColor;
            timelbl.Font = new Font("Segoe UI", 9F);
            timelbl.TextAlign = ContentAlignment.MiddleRight;
            timelbl.Dock = DockStyle.Fill;
            timelbl.Margin = new Padding(0, 0, 10, 0);

            TableLayoutPanel headerActions = new TableLayoutPanel();
            headerActions.BackColor = AppBackground;
            headerActions.Dock = DockStyle.Right;
            headerActions.Width = 450;
            headerActions.ColumnCount = 2;
            headerActions.RowCount = 1;
            headerActions.Padding = new Padding(0, 20, 20, 20);
            headerActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            headerActions.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 176F));
            headerActions.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            headerActions.Controls.Add(timelbl, 0, 0);
            headerActions.Controls.Add(fileselectbtn, 1, 0);

            headerPanel.Controls.Add(pageTitleLabel);
            headerPanel.Controls.Add(liveStatus);
            headerPanel.Controls.Add(selectedFileLbl);
            headerPanel.Controls.Add(headerActions);

            toolbarPanel = new Panel();
            toolbarPanel.BackColor = SurfaceColor;
            toolbarPanel.Dock = DockStyle.Fill;
            toolbarPanel.Paint += DrawBottomBorder;

            searchLabel = new Label();
            searchLabel.AutoSize = true;
            searchLabel.ForeColor = MutedTextColor;
            searchLabel.Font = new Font("Segoe UI Semibold", 8F);
            searchLabel.Text = "SEARCH STUDENTS";
            searchLabel.Location = new Point(20, 9);

            ModernPanel searchBox = new ModernPanel();
            searchBox.BackColor = ElevatedSurfaceColor;
            searchBox.BorderColor = BorderColor;
            searchBox.CornerRadius = 10;
            searchBox.Location = new Point(20, 28);
            searchBox.Size = new Size(390, 34);

            Label searchIcon = new Label();
            searchIcon.AutoSize = false;
            searchIcon.ForeColor = MutedTextColor;
            searchIcon.Font = new Font("Segoe MDL2 Assets", 11F);
            searchIcon.Text = "\uE721";
            searchIcon.TextAlign = ContentAlignment.MiddleCenter;
            searchIcon.Location = new Point(8, 6);
            searchIcon.Size = new Size(24, 22);

            SearchTextBox.BackColor = ElevatedSurfaceColor;
            SearchTextBox.ForeColor = PrimaryTextColor;
            SearchTextBox.BorderStyle = BorderStyle.None;
            SearchTextBox.Font = new Font("Segoe UI", 10F);
            SearchTextBox.Location = new Point(38, 7);
            SearchTextBox.Size = new Size(340, 23);

            searchBox.Controls.Add(searchIcon);
            searchBox.Controls.Add(SearchTextBox);

            Label filterHint = new Label();
            filterHint.AutoSize = true;
            filterHint.BackColor = Color.Transparent;
            filterHint.ForeColor = MutedTextColor;
            filterHint.Font = new Font("Segoe UI", 8.5F);
            filterHint.Text = "Use the sidebar filters to focus the list";

            ModernPanel filterPill = new ModernPanel();
            filterPill.BackColor = ElevatedSurfaceColor;
            filterPill.BorderColor = BorderColor;
            filterPill.CornerRadius = 10;
            filterPill.Location = new Point(424, 28);
            filterPill.Size = new Size(244, 34);
            filterHint.Location = new Point(13, 8);
            filterPill.Controls.Add(filterHint);

            toolbarPanel.Controls.Add(searchLabel);
            toolbarPanel.Controls.Add(searchBox);
            toolbarPanel.Controls.Add(filterPill);

            panel1.BackColor = AppBackground;
            panel1.Padding = new Padding(18, 16, 18, 10);
            panel1.Dock = DockStyle.Fill;

            ModernPanel gridCard = new ModernPanel();
            gridCard.BackColor = SurfaceColor;
            gridCard.BorderColor = BorderColor;
            gridCard.CornerRadius = 12;
            gridCard.Dock = DockStyle.Fill;
            gridCard.Padding = new Padding(1);
            panel1.Controls.Clear();
            panel1.Controls.Add(gridCard);
            gridCard.Controls.Add(dataGridView1);
            dataGridView1.Dock = DockStyle.Fill;

            Panel footer = new Panel();
            footer.BackColor = SurfaceColor;
            footer.Dock = DockStyle.Fill;
            footer.Paint += DrawTopBorder;

            ActiveLbl.AutoSize = false;
            ActiveLbl.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            ActiveLbl.ForeColor = ActiveGreen;
            ActiveLbl.Dock = DockStyle.Fill;
            ActiveLbl.TextAlign = ContentAlignment.MiddleLeft;
            ActiveLbl.Padding = new Padding(20, 0, 0, 0);
            InactiveLbl.AutoSize = false;
            InactiveLbl.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            InactiveLbl.ForeColor = ErrorRed;
            InactiveLbl.Dock = DockStyle.Fill;
            InactiveLbl.TextAlign = ContentAlignment.MiddleRight;
            InactiveLbl.Padding = new Padding(0, 0, 20, 0);

            TableLayoutPanel statusSummary = new TableLayoutPanel();
            statusSummary.BackColor = SurfaceColor;
            statusSummary.Dock = DockStyle.Fill;
            statusSummary.ColumnCount = 2;
            statusSummary.RowCount = 1;
            statusSummary.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            statusSummary.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            statusSummary.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            statusSummary.Controls.Add(InactiveLbl, 0, 0);
            statusSummary.Controls.Add(ActiveLbl, 1, 0);
            footer.Controls.Add(statusSummary);

            main.Controls.Add(headerPanel, 0, 0);
            main.Controls.Add(toolbarPanel, 0, 1);
            main.Controls.Add(panel1, 0, 2);
            main.Controls.Add(footer, 0, 3);
            return main;
        }

        private void ConfigureGrid()
        {
            dataGridView1.BackgroundColor = AppBackground;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView1.GridColor = BorderColor;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = SurfaceColor;
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = MutedTextColor;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = SurfaceColor;
            dataGridView1.ColumnHeadersDefaultCellStyle.SelectionForeColor = MutedTextColor;
            dataGridView1.ColumnHeadersHeight = 42;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView1.DefaultCellStyle.BackColor = AppBackground;
            dataGridView1.DefaultCellStyle.ForeColor = Properties.Settings.Default.DarkMode
                ? Color.FromArgb(213, 215, 222)
                : PrimaryTextColor;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Properties.Settings.Default.DarkMode
                ? Color.FromArgb(43, 40, 67)
                : Color.FromArgb(224, 219, 253);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Properties.Settings.Default.DarkMode
                ? Color.White
                : PrimaryTextColor;
            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dataGridView1.DefaultCellStyle.Padding = new Padding(6, 0, 6, 0);
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Properties.Settings.Default.DarkMode
                ? Color.FromArgb(17, 18, 20)
                : Color.FromArgb(249, 250, 252);
            dataGridView1.AlternatingRowsDefaultCellStyle.ForeColor = Properties.Settings.Default.DarkMode
                ? Color.FromArgb(213, 215, 222)
                : PrimaryTextColor;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowTemplate.Height = 34;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        }

        private void StylePrimaryButton(Button button)
        {
            button.BackColor = AccentColor;
            button.ForeColor = Color.White;
            button.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = AccentHoverColor;
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(88, 73, 205);
            button.Cursor = Cursors.Hand;
            ApplyRoundedRegion(button, 10);
        }

        private void NormalizeNavigationButtonColor(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button == null)
            {
                return;
            }

            Color normalizedColor = button.BackColor;
            if (button.BackColor == SystemColors.Control)
            {
                normalizedColor = ElevatedSurfaceColor;
            }
            else if (button.BackColor == Color.Green)
            {
                normalizedColor = SelectedSuccessColor;
            }
            else if (button.BackColor == Color.Red)
            {
                normalizedColor = SelectedErrorColor;
            }
            else if (button.BackColor == Color.Yellow)
            {
                normalizedColor = SelectedWarningColor;
            }

            if (button.BackColor != normalizedColor)
            {
                button.BackColor = normalizedColor;
            }
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            using (SettingsDialog dialog = new SettingsDialog(this))
            {
                dialog.ShowDialog(this);
            }
        }

        private void DrawBottomBorder(object sender, PaintEventArgs e)
        {
            Control control = (Control)sender;
            using (Pen pen = new Pen(BorderColor))
            {
                e.Graphics.DrawLine(pen, 0, control.Height - 1, control.Width, control.Height - 1);
            }
        }

        private void DrawTopBorder(object sender, PaintEventArgs e)
        {
            Control control = (Control)sender;
            using (Pen pen = new Pen(BorderColor))
            {
                e.Graphics.DrawLine(pen, 0, 0, control.Width, 0);
            }
        }

        internal static void ApplyRoundedRegion(Control control, int radius)
        {
            EventHandler updateRegion = delegate
            {
                if (control.Width <= 0 || control.Height <= 0)
                {
                    return;
                }

                Region oldRegion = control.Region;
                using (GraphicsPath path = CreateRoundedPath(new Rectangle(0, 0, control.Width, control.Height), radius))
                {
                    control.Region = new Region(path);
                }

                if (oldRegion != null)
                {
                    oldRegion.Dispose();
                }
            };

            control.Resize += updateRegion;
            updateRegion(control, EventArgs.Empty);
        }

        internal static GraphicsPath CreateRoundedPath(Rectangle bounds, int radius)
        {
            int diameter = Math.Max(2, radius * 2);
            Rectangle arc = new Rectangle(bounds.X, bounds.Y, diameter, diameter);
            GraphicsPath path = new GraphicsPath();
            path.AddArc(arc, 180, 90);
            arc.X = bounds.Right - diameter - 1;
            path.AddArc(arc, 270, 90);
            arc.Y = bounds.Bottom - diameter - 1;
            path.AddArc(arc, 0, 90);
            arc.X = bounds.X;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    internal sealed class ModernPanel : Panel
    {
        public ModernPanel()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
            CornerRadius = 10;
            BorderColor = Color.Transparent;
        }

        public int CornerRadius { get; set; }
        public Color BorderColor { get; set; }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            if (Width <= 0 || Height <= 0)
            {
                return;
            }

            Region oldRegion = Region;
            using (GraphicsPath path = Form1.CreateRoundedPath(new Rectangle(0, 0, Width, Height), CornerRadius))
            {
                Region = new Region(path);
            }

            if (oldRegion != null)
            {
                oldRegion.Dispose();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath path = Form1.CreateRoundedPath(new Rectangle(0, 0, Width - 1, Height - 1), CornerRadius))
            using (Pen pen = new Pen(BorderColor))
            {
                e.Graphics.DrawPath(pen, path);
            }
        }
    }
}
