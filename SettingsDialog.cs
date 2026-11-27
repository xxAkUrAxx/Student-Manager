using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web.Script.Serialization;

namespace Student_Manager
{
    internal sealed class GitHubReleaseInfo
    {
        public string tag_name { get; set; }
        public string html_url { get; set; }
        public GitHubReleaseAsset[] assets { get; set; }
    }

    internal sealed class GitHubReleaseAsset
    {
        public string name { get; set; }
        public string browser_download_url { get; set; }
        public string digest { get; set; }
    }

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
            doneButton.Enabled = false;
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
                doneButton.Enabled = true;
                checkUpdatesButton.Text = "↻   Check for updates";
            }
        }

        private async Task CheckForUpdatesAsync()
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(10);
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Student-Manager/1.0.2");
                HttpResponseMessage response = await client.GetAsync(LatestReleaseApiUrl);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    updateStatusLabel.Text = "The update service could not find a published release.";
                    releaseUrl = RepositoryUrl;
                    releaseLink.Text = "Open GitHub repository";
                    releaseLink.Visible = true;
                    response.Dispose();
                    return;
                }

                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                response.Dispose();

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                GitHubReleaseInfo release = serializer.Deserialize<GitHubReleaseInfo>(json);
                if (release == null || string.IsNullOrWhiteSpace(release.tag_name))
                {
                    throw new InvalidOperationException("GitHub returned a release without a version tag.");
                }

                string tag = release.tag_name;
                string cleanTag = tag.Trim().TrimStart('v', 'V');
                Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
                Version latestVersion;
                bool parsed = Version.TryParse(cleanTag, out latestVersion);

                releaseUrl = string.IsNullOrWhiteSpace(release.html_url) ? RepositoryUrl : release.html_url;
                if (parsed && latestVersion > currentVersion)
                {
                    GitHubReleaseAsset installerAsset = FindInstallerAsset(release);
                    ValidateInstallerAsset(installerAsset);

                    updateStatusLabel.Text = "Version " + tag + " is available. Starting download...";
                    checkUpdatesButton.Text = "Downloading update...";
                    await DownloadAndStartInstallerAsync(client, installerAsset, cleanTag);
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

        private static GitHubReleaseAsset FindInstallerAsset(GitHubReleaseInfo release)
        {
            if (release.assets == null)
            {
                return null;
            }

            return release.assets.FirstOrDefault(asset =>
                asset != null &&
                !string.IsNullOrWhiteSpace(asset.name) &&
                asset.name.EndsWith(".exe", StringComparison.OrdinalIgnoreCase));
        }

        private static void ValidateInstallerAsset(GitHubReleaseAsset asset)
        {
            if (asset == null || string.IsNullOrWhiteSpace(asset.browser_download_url))
            {
                throw new InvalidOperationException("The latest release does not contain a Windows installer.");
            }

            Uri downloadUri;
            if (!Uri.TryCreate(asset.browser_download_url, UriKind.Absolute, out downloadUri) ||
                downloadUri.Scheme != Uri.UriSchemeHttps ||
                !string.Equals(downloadUri.Host, "github.com", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("The release installer has an invalid download address.");
            }

            if (string.IsNullOrWhiteSpace(asset.digest) ||
                !asset.digest.StartsWith("sha256:", StringComparison.OrdinalIgnoreCase) ||
                asset.digest.Length != 71)
            {
                throw new InvalidOperationException("The release installer does not have a valid security checksum.");
            }
        }

        private async Task DownloadAndStartInstallerAsync(
            HttpClient client,
            GitHubReleaseAsset asset,
            string version)
        {
            string safeVersion = new string(version.Where(character =>
                char.IsDigit(character) || character == '.').ToArray());
            string installerPath = Path.Combine(
                Path.GetTempPath(),
                "StudentManagerUpdate-" + safeVersion + ".exe");

            using (HttpResponseMessage downloadResponse = await client.GetAsync(
                asset.browser_download_url,
                HttpCompletionOption.ResponseHeadersRead))
            {
                downloadResponse.EnsureSuccessStatusCode();
                long? totalBytes = downloadResponse.Content.Headers.ContentLength;

                using (Stream downloadStream = await downloadResponse.Content.ReadAsStreamAsync())
                using (FileStream fileStream = new FileStream(
                    installerPath,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.None,
                    81920,
                    true))
                {
                    byte[] buffer = new byte[81920];
                    long downloadedBytes = 0;
                    int bytesRead;
                    while ((bytesRead = await downloadStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        await fileStream.WriteAsync(buffer, 0, bytesRead);
                        downloadedBytes += bytesRead;

                        if (totalBytes.HasValue && totalBytes.Value > 0)
                        {
                            int percentage = (int)(downloadedBytes * 100L / totalBytes.Value);
                            updateStatusLabel.Text = "Downloading version " + version + "... " + percentage + "%";
                        }
                    }
                }
            }

            string expectedHash = asset.digest.Substring("sha256:".Length);
            string actualHash = ComputeSha256(installerPath);
            if (!string.Equals(expectedHash, actualHash, StringComparison.OrdinalIgnoreCase))
            {
                File.Delete(installerPath);
                throw new InvalidOperationException("The downloaded installer failed its security check.");
            }

            updateStatusLabel.Text = "Download complete. Starting the installer...";
            checkUpdatesButton.Text = "Installing update...";

            Process.Start(new ProcessStartInfo(installerPath)
            {
                UseShellExecute = true,
                Arguments = "/SILENT /SUPPRESSMSGBOXES /NORESTART /CLOSEAPPLICATIONS /NORESTARTAPPLICATIONS"
            });
        }

        private static string ComputeSha256(string filePath)
        {
            using (SHA256 sha256 = SHA256.Create())
            using (FileStream stream = File.OpenRead(filePath))
            {
                return BitConverter.ToString(sha256.ComputeHash(stream)).Replace("-", string.Empty);
            }
        }

        private void ReleaseLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo(releaseUrl) { UseShellExecute = true });
        }
    }
}
