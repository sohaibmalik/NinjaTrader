using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ModernNotifier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        public MainWindow()
        {
            InitializeComponent();

             byte Red = Properties.Settings.Default.ForeColor_R;
            byte Blue = Properties.Settings.Default.ForeColor_B;
            byte Green = Properties.Settings.Default.ForeColor_G;

            var AccentColor = Color.FromRgb(Red, Green, Blue);
            var Theme = Properties.Settings.Default.Theme;
            var Fontsize = Properties.Settings.Default.FontSize;
            SetTheme(AccentColor, Fontsize, Theme);
        }

        private void SetTheme(Color AccentColor, FontSize size, string Theme)
        {
            AppearanceManager.Current.AccentColor = AccentColor;
            AppearanceManager.Current.FontSize = size;
            if (Theme == "light")
                AppearanceManager.Current.ThemeSource = FirstFloor.ModernUI.Presentation.AppearanceManager.LightThemeSource;
            if (Theme == "dark")
                AppearanceManager.Current.ThemeSource = FirstFloor.ModernUI.Presentation.AppearanceManager.DarkThemeSource;
        }
    }
}
