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
    public sealed partial class About : Page
    {





        // Wird beim ersten Aufruf der Seite geladen
        // -------------------------------------------------------------------------------
        public About()
        {
            // UI Komponenten laden
            this.InitializeComponent();

            // Angeben welche Seite in Frame geladen ist
            MainPage.framePage = "About";
        }
        // -------------------------------------------------------------------------------





        // Buttons der Seite
        // -------------------------------------------------------------------------------
        // Rate
        private async void btnRate(object sender, PointerRoutedEventArgs e)
        {
            // Zur Bewertungsseite
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + CurrentApp.AppId));
        }

        // Contact
        private async void btnContact_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            //Support Mail
            await Launcher.LaunchUriAsync(new Uri("mailto:xtrose@hotmail.com"));
        }

        // Website
        private async void btnWebsite(object sender, PointerRoutedEventArgs e)
        {
            //Link zu xtrose
            await Launcher.LaunchUriAsync(new Uri("http://www.xtrose.com"));
        }

        // Facebook
        private async void btnFacebook(object sender, PointerRoutedEventArgs e)
        {
            //Link zu Facebook
            await Launcher.LaunchUriAsync(new Uri("https://www.facebook.com/xtrose.xtrose"));
        }

        // VK
        private async void BtnVK(object sender, PointerRoutedEventArgs e)
        {
            //Link zu VK
            await Launcher.LaunchUriAsync(new Uri("http://vk.com/public54083459"));
        }

        // Twitter
        private async void BtnTwitter(object sender, PointerRoutedEventArgs e)
        {
            //Link zu Twitter
            await Launcher.LaunchUriAsync(new Uri("https://twitter.com/xtrose"));
        }

        // You Tube
        private async void BtnYT(object sender, PointerRoutedEventArgs e)
        {
            //Link zu xtrose
            await Launcher.LaunchUriAsync(new Uri("https://www.youtube.com/channel/UCFZUWrnyo4qdcvc8H4sYyBg"));
        }

        // Music
        private void BtnMusic(object sender, PointerRoutedEventArgs e)
        {
            //Link zu xtrose
            MainPage.frMain.Navigate(typeof(Music));
        }
        // -------------------------------------------------------------------------------





    }
}
