using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;

namespace BlackTitlebar
{
    public partial class MainForm : Form
    {
        private const long Black = 0xF000000;
        private const long Fallback = 0x2A9DF4;
        private const int ColoredTitlebar = 0x1;
        private const int WhiteTitlebar = 0x0;
        private const string KeyPath = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\DWM";
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
        
        private static bool GetStatus(out long accentColor)
        {
            var colorPrevalence = int.Parse(Registry.GetValue(KeyPath, "ColorPrevalence", -1)?.ToString()!);
            var activeAccentColor = accentColor = long.Parse(Registry.GetValue(KeyPath, "AccentColor", -1)?.ToString()!);
            var isTitlebarBlack = (colorPrevalence == ColoredTitlebar && activeAccentColor == Black);
            return isTitlebarBlack;
        }

        private void InitializeEvents()
        {
            _btnEnableBtb.Click += (_, _) => EnableBlackTitlebar();
            _btnDisableBtb.Click += (_, _) => DisableBlackTitlebar();
            this.Activated += (_, _) => UpdateStatus();
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
            _labelStatus.Width = this.Width - _imgGitHub.Width * 3;
            _bottomPanel.Controls.Add(_imgGitHub);
            _bottomPanel.Controls.Add(_labelStatus);
            this.Controls.Add(_bottomPanel);
            this.Controls.Add(_btnDisableBtb);
            this.Controls.Add(_btnEnableBtb);
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

        private static void SetAccentColor(long hex)
        {
            Registry.SetValue(KeyPath, "AccentColor", hex, RegistryValueKind.DWord);
        }

        private static void SetInactiveAccentColor(long hex)
        {
            Registry.SetValue(KeyPath, "AccentColorInactive", hex, RegistryValueKind.DWord);
        }

        private static void SetColorPrevalence(bool enable)
        {
            Registry.SetValue(KeyPath, "ColorPrevalence", enable ? ColoredTitlebar : WhiteTitlebar, RegistryValueKind.DWord);
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