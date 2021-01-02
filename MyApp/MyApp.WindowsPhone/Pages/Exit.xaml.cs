using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;






// Namespace
namespace MyApp.Pages
{
    



    // Seite beim verlassen dees Spiels
    public sealed partial class Exit : Page
    {





        // Wird beim ersten aufruf der Seite ausgeführt
        // ---------------------------------------------------------------------------------------------------
        public Exit()
        {
            // XAML laden
            this.InitializeComponent();

            // Angeben welche Seite
            MainPage.framePage = "Exit";
        }
        // ---------------------------------------------------------------------------------------------------





        // Buttons Verlassen Menü
        // ---------------------------------------------------------------------------------------------------
        // Verlassen
        private void Image_PointerReleased_1(object sender, PointerRoutedEventArgs e)
        {
            // Spiel verlassen
            MyApp.App.Current.Exit();
        }

        // Doch nicht verlassen
        private void Image_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            // Spiel verlassen
            MainPage.frMain.GoBack();
        }
        // ---------------------------------------------------------------------------------------------------
    }
}
