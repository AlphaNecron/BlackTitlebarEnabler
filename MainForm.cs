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
        private const long Black = 0xFF00000;
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
            Size = new Size(20, 20),
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

        private readonly Label _labelStatus = new();

        public MainForm()
        {
            InitializeComponent();
            InitializeControls();
            InitializeEvents();
        }
        
        private static bool GetStatus()
        {
            var colorPrevalence = Convert.ToInt16(Registry.GetValue(KeyPath, "ColorPrevalence", -1));
            var activeColor = long.Parse(Registry.GetValue(KeyPath, "AccentColor", -1)?.ToString()!);
            var isTitlebarBlack = (colorPrevalence == ColoredTitlebar && activeColor == Black);
            return isTitlebarBlack;
        }

        private void InitializeEvents()
        {
            _btnEnableBtb.Click += (_, _) => EnableBlackTitlebar();
            _btnDisableBtb.Click += (_, _) => DisableBlackTitlebar(); 
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
            _imgGitHub.Location = new Point(_bottomPanel.Width + this.Width / 6, 10);
            _labelStatus.Location = new Point(10, 10);
            _labelStatus.Text = $@"Black Titlebar: {(GetStatus() ? "Enabled" : "Disabled")}";
            _labelStatus.Width = 150;
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
                SetColorPrevalence(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                MessageBox.Show($@"Status: {(GetStatus() ? "Enabled" : "Disabled")}");
            }
        }

        private static void SetColorPrevalence(bool enable)
        {
            Registry.SetValue(KeyPath, "ColorPrevalence", enable ? ColoredTitlebar : WhiteTitlebar, RegistryValueKind.DWord);
        }

        private static void EnableBlackTitlebar()
        {
            try
            {
                SetColorPrevalence(true);
                Registry.SetValue(KeyPath, "AccentColor", Black, RegistryValueKind.DWord);
                Registry.SetValue(KeyPath, "AccentColorInactive", Black, RegistryValueKind.DWord);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                MessageBox.Show($@"Status: {(GetStatus() ? "Enabled" : "Disabled")}");
            }
        }
    }
}