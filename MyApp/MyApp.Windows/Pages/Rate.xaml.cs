using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
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
    




    // About
    public sealed partial class Rate : Page
    {





        // Variablen
        // -------------------------------------------------------------------------------
        bool boBack = false;
        // -------------------------------------------------------------------------------





        // Wird beim ersten Aufruf der Seite geladen
        // -------------------------------------------------------------------------------
        public Rate()
        {
            // UI Komponenten laden
            this.InitializeComponent();

            // Angeben auf welcher Seite
            MainPage.framePage = "Rate";

            // Seite einstellen
            tbRate.Text = MainPage.resource.GetString("001_rateText1");
            spMail.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }
        // -------------------------------------------------------------------------------





        // Buttons der Seite
        // -------------------------------------------------------------------------------
        // Mail
        private async void mail_po(object sender, PointerRoutedEventArgs e)
        {
            // Mail
            await Launcher.LaunchUriAsync(new Uri("mailto:xtrose@hotmail.com"));
            MainPage.frMain.GoBack();
        }

        // Bewerten
        private async void rate_po(object sender, PointerRoutedEventArgs e)
        {
            // Bewerten
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + CurrentApp.AppId));
            MainPage.frMain.GoBack();
        }

        // Zurück
        private void back_po(object sender, PointerRoutedEventArgs e)
        {
            if (!boBack)
            {
                tbRate.Text = MainPage.resource.GetString("001_rateText2");
                spMail.Visibility = Windows.UI.Xaml.Visibility.Visible;
                boBack = true;
            }
            else
            {
                MainPage.frMain.GoBack();
            }
        }
        // -------------------------------------------------------------------------------





    }
}
