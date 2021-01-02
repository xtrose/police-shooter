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
    public sealed partial class Music : Page
    {





        // Wird beim ersten Aufruf der Seite geladen
        // -------------------------------------------------------------------------------
        public Music()
        {
            // UI Komponenten laden
            this.InitializeComponent();

            // Angeben welche Seite in Frame geladen ist
            MainPage.framePage = "Music";
        }
        // -------------------------------------------------------------------------------





        // Buttons der Seite
        // -------------------------------------------------------------------------------
        // Link License
        private async void License(object sender, PointerRoutedEventArgs e)
        {
            //Link zu xtrose
            await Launcher.LaunchUriAsync(new Uri("http://creativecommons.org/licenses/by/3.0/"));
        }

        // Lied 1
        private void Song1_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            // Hintergundmusik abspielen
            MainPage.backgroundMusic.Source = new Uri("ms-appx:///Sounds/LevelMusic.mp3", UriKind.RelativeOrAbsolute);
            MainPage.backgroundMusic.Play();
        }

        // Lied 2
        private void Song2_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            // Hintergundmusik abspielen
            MainPage.backgroundMusic.Source = new Uri("ms-appx:///Sounds/MenuMain.mp3", UriKind.RelativeOrAbsolute);
            MainPage.backgroundMusic.Play();
        }

        // Lied 3
        private void Song3_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            // Hintergundmusik abspielen
            MainPage.backgroundMusic.Source = new Uri("ms-appx:///Sounds/LevelMusic2.mp3", UriKind.RelativeOrAbsolute);
            MainPage.backgroundMusic.Play();
        }
        // -------------------------------------------------------------------------------





    }
}
