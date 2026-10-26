using System;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Student_Manager
{
    internal sealed class SettingsDialog : Form
    {
        private const string RepositoryUrl = "https://github.com/xxAkUrAxx/Student-Manager";
        private const string LatestReleaseApiUrl = "https://api.github.com/repos/xxAkUrAxx/Student-Manager/releases/latest";

        private readonly Form1 ownerForm;
        private readonly Label titleLabel;
        private readonly Label themeDescription;
        private readonly Button darkModeButton;
        private readonly Button lightModeButton;
        private readonly Label updateStatusLabel;
        private readonly LinkLabel releaseLink;
        private readonly Button checkUpdatesButton;
        private readonly Button doneButton;
        private string releaseUrl = RepositoryUrl;

        public SettingsDialog(Form1 owner)
        {
            ownerForm = owner;
            Text = "Student Manager Settings";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(540, 400);
            Font = new Font("Segoe UI", 9F);

            titleLabel = new Label();
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
            titleLabel.Text = "Settings";
            titleLabel.Location = new Point(28, 24);

            Label appearanceHeading = CreateHeading("APPEARANCE", 28, 84);
            themeDescription = new Label();
            themeDescription.AutoSize = true;
            themeDescription.Text = "Choose the display mode used throughout Student Manager.";
            themeDescription.Location = new Point(28, 109);

            darkModeButton = CreateChoiceButton("☾   Dark mode", 28, 142);
            lightModeButton = CreateChoiceButton("☀   Light mode", 218, 142);
            darkModeButton.Click += delegate { ChangeTheme(true); };
            lightModeButton.Click += delegate { ChangeTheme(false); };

            Label updatesHeading = CreateHeading("UPDATES", 28, 214);
            updateStatusLabel = new Label();
            updateStatusLabel.AutoSize = false;
            updateStatusLabel.Size = new Size(470, 42);
            updateStatusLabel.Text = "Check GitHub for a newer published release.";
            updateStatusLabel.Location = new Point(28, 239);

            checkUpdatesButton = CreateActionButton("↻   Check for updates", 28, 288, 180);
            checkUpdatesButton.Click += CheckUpdatesButton_Click;

            releaseLink = new LinkLabel();
            releaseLink.AutoSize = true;
            releaseLink.Text = "Open GitHub repository";
            releaseLink.Location = new Point(226, 300);
            releaseLink.LinkClicked += ReleaseLink_LinkClicked;

            doneButton = CreateActionButton("Done", 402, 348, 110);
            doneButton.Click += delegate { Close(); };

            Controls.Add(titleLabel);
            Controls.Add(appearanceHeading);
            Controls.Add(themeDescription);
            Controls.Add(darkModeButton);
            Controls.Add(lightModeButton);
            Controls.Add(updatesHeading);
            Controls.Add(updateStatusLabel);
            Controls.Add(checkUpdatesButton);
            Controls.Add(releaseLink);
            Controls.Add(doneButton);

            AcceptButton = doneButton;
            ApplyDialogTheme();
        }

        private Label CreateHeading(string text, int x, int y)
        {
            Label label = new Label();
            label.AutoSize = true;
            label.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);
            label.Text = text;
            label.Location = new Point(x, y);
            return label;
        }

        private Button CreateChoiceButton(string text, int x, int y)
        {
            Button button = new Button();
            button.Text = text;
            button.TextAlign = ContentAlignment.MiddleLeft;
            button.Padding = new Padding(14, 0, 0, 0);
            button.Location = new Point(x, y);
            button.Size = new Size(174, 46);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;
            button.Cursor = Cursors.Hand;
            Form1.ApplyRoundedRegion(button, 10);
            return button;
        }

        private Button CreateActionButton(string text, int x, int y, int width)
        {
            Button button = new Button();
            button.Text = text;
            button.Location = new Point(x, y);
            button.Size = new Size(width, 38);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Cursor = Cursors.Hand;
            Form1.ApplyRoundedRegion(button, 10);
            return button;
        }

        private void ChangeTheme(bool darkMode)
        {
            ownerForm.SetApplicationTheme(darkMode);
            ApplyDialogTheme();
        }

        private void ApplyDialogTheme()
        {
            bool darkMode = Properties.Settings.Default.DarkMode;
            BackColor = Form1.AppBackground;
            ForeColor = Form1.PrimaryTextColor;
            titleLabel.ForeColor = Form1.PrimaryTextColor;
            themeDescription.ForeColor = Form1.MutedTextColor;
            updateStatusLabel.ForeColor = Form1.MutedTextColor;

            foreach (Control control in Controls)
            {
                Label label = control as Label;
                if (label != null && label != titleLabel && label != themeDescription && label != updateStatusLabel)
                {
                    label.ForeColor = Form1.MutedTextColor;
                }
            }

            StyleChoiceButton(darkModeButton, darkMode);
            StyleChoiceButton(lightModeButton, !darkMode);
            StyleAccentButton(checkUpdatesButton);
            StyleAccentButton(doneButton);
            releaseLink.LinkColor = Form1.AccentColor;
            releaseLink.ActiveLinkColor = Form1.AccentHoverColor;
            releaseLink.VisitedLinkColor = Form1.AccentColor;
        }

        private void StyleChoiceButton(Button button, bool selected)
        {
            button.BackColor = selected ? Form1.AccentColor : Form1.ElevatedSurfaceColor;
            button.ForeColor = selected ? Color.White : Form1.PrimaryTextColor;
            button.FlatAppearance.BorderColor = selected ? Form1.AccentColor : Form1.BorderColor;
            button.FlatAppearance.MouseOverBackColor = selected ? Form1.AccentHoverColor : Form1.SurfaceColor;
        }

        private void StyleAccentButton(Button button)
        {
            button.BackColor = Form1.AccentColor;
            button.ForeColor = Color.White;
            button.FlatAppearance.MouseOverBackColor = Form1.AccentHoverColor;
        }

        private async void CheckUpdatesButton_Click(object sender, EventArgs e)
        {
            checkUpdatesButton.Enabled = false;
            checkUpdatesButton.Text = "Checking GitHub...";
            updateStatusLabel.Text = "Contacting GitHub for the latest published release.";
            releaseLink.Visible = false;

            try
            {
                await CheckForUpdatesAsync();
            }
            catch (Exception ex)
            {
                updateStatusLabel.Text = "Unable to check for updates. " + ex.Message;
                releaseUrl = RepositoryUrl;
                releaseLink.Text = "Open GitHub repository";
                releaseLink.Visible = true;
            }
            finally
            {
                checkUpdatesButton.Enabled = true;
                checkUpdatesButton.Text = "↻   Check for updates";
            }
        }

        private async Task CheckForUpdatesAsync()
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(12);
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Student-Manager/1.0");
                HttpResponseMessage response = await client.GetAsync(LatestReleaseApiUrl);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    updateStatusLabel.Text = "No published GitHub releases were found yet.";
                    releaseUrl = RepositoryUrl;
                    releaseLink.Text = "Open GitHub repository";
                    releaseLink.Visible = true;
                    return;
                }

                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                Match tagMatch = Regex.Match(json, "\\\"tag_name\\\"\\s*:\\s*\\\"([^\\\"]+)\\\"");
                Match urlMatch = Regex.Match(json, "\\\"html_url\\\"\\s*:\\s*\\\"([^\\\"]+)\\\"");
                if (!tagMatch.Success)
                {
                    throw new InvalidOperationException("GitHub returned a release without a version tag.");
                }

                string tag = tagMatch.Groups[1].Value;
                string cleanTag = tag.Trim().TrimStart('v', 'V');
                Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
                Version latestVersion;
                bool parsed = Version.TryParse(cleanTag, out latestVersion);

                releaseUrl = urlMatch.Success ? urlMatch.Groups[1].Value.Replace("\\/", "/") : RepositoryUrl;
                if (parsed && latestVersion > currentVersion)
                {
                    updateStatusLabel.Text = "A newer version is available: " + tag + " (installed " + currentVersion.ToString(3) + ").";
                    releaseLink.Text = "View update on GitHub";
                }
                else if (parsed)
                {
                    updateStatusLabel.Text = "Student Manager is up to date (version " + currentVersion.ToString(3) + ").";
                    releaseLink.Text = "View latest release";
                }
                else
                {
                    updateStatusLabel.Text = "Latest GitHub release: " + tag + ".";
                    releaseLink.Text = "View release on GitHub";
                }

                releaseLink.Visible = true;
            }
        }

        private void ReleaseLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo(releaseUrl) { UseShellExecute = true });
        }
    }
}
