using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using static BlackTitlebar.RegUtils;

namespace BlackTitlebar
{
    public partial class MainForm : Form
    {
        private readonly ComponentResourceManager ResourceManager = new(typeof(MainForm));

        private readonly CommandLinkButton _btnDisableBtb = new()
        {
            Text = @"Disable black titlebar",
            Size = new Size(245, 45)
        };

        private readonly PictureBox _imgGitHub = new()
        {
            Size = new Size(16, 16)
        };

        private readonly CommandLinkButton _btnEnableBtb = new()
        {
            Text = @"Enable black titlebar",
            Size = new Size(245, 45)
        };

        private readonly Panel _bottomPanel = new()
        {
            Dock = DockStyle.Bottom,
            Height = 35
        };

        private static readonly Label _labelStatus = new();

        public MainForm()
        {
            InitializeComponent();
            InitializeControls();
            InitializeEvents();
        }

        private void InitializeEvents()
        {
            _btnEnableBtb.Click += (_, _) => EnableBlackTitlebar();
            _btnDisableBtb.Click += (_, _) => DisableBlackTitlebar();
            Activated += (_, _) => UpdateStatus();
            _imgGitHub.Click += (_, _) => Process.Start(new ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = "https://github.com/AlphaNecron"
            });
        }

        private void InitializeControls()
        {
            _btnDisableBtb.Location = new Point(20, 10);
            _btnEnableBtb.Location = new Point(20, 20 + _btnDisableBtb.Height);
            _imgGitHub.BackgroundImageLayout = ImageLayout.Stretch;
            _imgGitHub.BackgroundImage = (Image) ResourceManager.GetObject("imgGitHub"); 
            _imgGitHub.Location = new Point(_bottomPanel.Width + _imgGitHub.Width * 3 + 12, 15);
            _labelStatus.Location = new Point(5, 15);
            _labelStatus.Width = Width - _imgGitHub.Width * 3;
            _bottomPanel.Controls.Add(_imgGitHub);
            _bottomPanel.Controls.Add(_labelStatus);
            Controls.Add(_bottomPanel);
            Controls.Add(_btnDisableBtb);
            Controls.Add(_btnEnableBtb);
        }

        private static void DisableBlackTitlebar()
        {
            try
            {
                SetAccentColor(Fallback);
                SetInactiveAccentColor(Fallback);
                SetColorPrevalence(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                UpdateStatus(true);
            }
        }

        private static void UpdateStatus(bool showMsg = false)
        {
            var status = GetStatus(out var accentColor) ? "Enabled" : "Disabled";
             _labelStatus.Text = $@"Black Titlebar: {status} (#{accentColor:X})";
            if (showMsg) MessageBox.Show($@"Black Titlebar: {status}");
        }

        private static void EnableBlackTitlebar()
        {
            try
            {
                SetAccentColor(Black);
                SetInactiveAccentColor(Black);
                SetColorPrevalence(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
               UpdateStatus(true);
            }
        }
    }
}