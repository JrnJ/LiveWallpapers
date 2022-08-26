using Microsoft.UI.Windowing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LiveWallpapers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public static AppWindow AppWindow { get; private set; }

        private MediaElement _mediaElement;

        public MediaElement MediaElement
        {
            get { return _mediaElement; }
            set { _mediaElement = value; OnPropertyChanged(); }
        }

        private WallpaperRenderer _wallpaperRendererWindow;

        public WallpaperRenderer WallpaperRendererWindow
        {
            get { return _wallpaperRendererWindow; }
            set { _wallpaperRendererWindow = value; OnPropertyChanged(); }
        }


        public MainWindow()
        {
            MediaElement = new MediaElement()
            {
                LoadedBehavior = MediaState.Manual
            };

            MediaElement.MediaOpened += MediaElement_MediaOpened;
            MediaElement.MediaEnded += MediaElement_MediaEnded;

            WallpaperRendererWindow = new(MediaElement);
            WallpaperRendererWindow.Show();

            InitializeComponent();

            TryTitlebarCustomization();
        }

        public void TryTitlebarCustomization()
        {
            try
            {
                AppWindow = AppWindowExtensions.GetAppWindowFromWPFWindow(this);

                if (AppWindow != null)
                {
                    if (AppWindowTitleBar.IsCustomizationSupported())
                    {
                        CustomizeTitleBar();
                    }
                    else
                    {
                        MessageBox.Show("Titlebar customization not supported on this device!", "Error");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex);

                //MessageBox.Show("Could not load custom window!", "Error");
                this.Title = "Custom Window Not Supported";
            }
        }

        public void CustomizeTitleBar()
        {
            // AppWindowTitleBar
            AppWindowTitleBar titleBar = AppWindow.TitleBar;
            titleBar.ExtendsContentIntoTitleBar = true;

            // Icon
            //titleBar.IconShowOptions = IconShowOptions.HideIconAndSystemMenu;)
            AppWindow.SetIcon("/Images/LockScreenLogo.scale-200.png");

            // Title


            // Bar
            titleBar.BackgroundColor = Windows.UI.Color.FromArgb(255, 32, 32, 32);
            titleBar.ForegroundColor = Windows.UI.Color.FromArgb(255, 255, 255, 255);
            /// Inactive
            titleBar.InactiveBackgroundColor = Windows.UI.Color.FromArgb(255, 32, 32, 32);
            titleBar.InactiveForegroundColor = Windows.UI.Color.FromArgb(255, 255, 255, 255);

            // Buttons
            titleBar.ButtonBackgroundColor = Windows.UI.Color.FromArgb(0, 32, 32, 32);
            titleBar.ButtonForegroundColor = Windows.UI.Color.FromArgb(0, 255, 255, 255);
            titleBar.ButtonHoverBackgroundColor = Windows.UI.Color.FromArgb(255, 37, 37, 37);
            titleBar.ButtonPressedBackgroundColor = Windows.UI.Color.FromArgb(255, 41, 41, 41);
            /// Inactive
            titleBar.ButtonInactiveBackgroundColor = Windows.UI.Color.FromArgb(0, 32, 32, 32);
            titleBar.ButtonInactiveForegroundColor = Windows.UI.Color.FromArgb(0, 255, 255, 255);
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            MediaElement.Position = new TimeSpan(0, 0, 0, 0, 0);
            MediaElement.Play();
        }

        private void MediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            int bp = 0;
        }

        //public bool IsThisIdk = true;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MediaElement.Source = new Uri(videoSource.Text, UriKind.RelativeOrAbsolute);
            MediaElement.Play();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            WallpaperRendererWindow.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MediaElement.IsMuted = !MediaElement.IsMuted;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
