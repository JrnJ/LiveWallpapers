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
using System.Windows.Shapes;
using static LiveWallpapers.MainWindow;

namespace LiveWallpapers
{
    /// <summary>
    /// Interaction logic for WallpaperRenderer.xaml
    /// </summary>
    public partial class WallpaperRenderer : Window, INotifyPropertyChanged
    {
        public WallpaperRenderer(MediaElement mediaElement)
        {
            InitializeComponent();
            myGrid.Children.Add(mediaElement);

            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AppWindowExtensions.RenderAppAsBackground(this);
            AppWindowExtensions.SetWindowPosition(this, 1920, 0);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void myMediaElement_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }
    }
}
