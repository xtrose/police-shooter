using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class Settings : Page
    {





        // Variablen
        // -------------------------------------------------------------------------------
        float volumeMusic = 0.0f;
        float volumeEffects = 0.0f;
        // -------------------------------------------------------------------------------





        // Wird beim ersten Aufruf der Seite geladen
        // -------------------------------------------------------------------------------
        public Settings()
        {
            // UI Komponenten laden
            this.InitializeComponent();

            // Angeben welche Seite in Frame geladen ist
            MainPage.framePage = "Settings";

            // Variablen festlegen
            volumeMusic = MainPage.volumeMusic;
            volumeEffects = MainPage.volumeEffects;

            // Display einstellen
            SetDisplay();
        }
        // -------------------------------------------------------------------------------





        // Buttons der Seite
        // -------------------------------------------------------------------------------
        // Musik -
        private void MM(object sender, PointerRoutedEventArgs e)
        {
            if (volumeMusic > 0.0f)
            {
                volumeMusic = selectFloat(volumeMusic, "minus");
                MainPage.volumeMusic = volumeMusic;
                SetDisplay();
                SetMusic();
            }
        }
        // Musik +
        private void MP(object sender, PointerRoutedEventArgs e)
        {
            if (volumeMusic < 1.0f)
            {
                volumeMusic = selectFloat(volumeMusic, "add");
                MainPage.volumeMusic = volumeMusic; ;
                SetDisplay();
                SetMusic();
            }
        }
        // Effekte -
        private void EM(object sender, PointerRoutedEventArgs e)
        {
            if (MainPage.volumeEffects > 0.0f)
            {
                volumeEffects = selectFloat(volumeEffects, "minus");
                MainPage.volumeEffects = volumeEffects;
                SetDisplay();
                SetEffects();
            }
        }
        // Effekte +
        private void EP(object sender, PointerRoutedEventArgs e)
        {
            if (MainPage.volumeEffects < 1.0f)
            {
                volumeEffects = selectFloat(volumeEffects, "add");
                MainPage.volumeEffects = volumeEffects;
                SetDisplay();
                SetEffects();
            }
        }
        // -------------------------------------------------------------------------------





        // Music einstellen
        // -------------------------------------------------------------------------------
        async void SetMusic()
        {
            // Lautstärke anwenden
            MainPage.backgroundMusic.Volume = volumeMusic;
            // Punkte speichern
            var fileSettings = await MainPage.SF.CreateFileAsync("s.txt", CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(fileSettings, (volumeMusic.ToString() + ";" + volumeEffects.ToString()));
        }
        // -------------------------------------------------------------------------------





        // Effekte einstellen
        // -------------------------------------------------------------------------------
        async void SetEffects()
        {
            // Ton ausgeben
            MainPage.PlaySound("Sounds/Gun.wav", volumeEffects);
            // Punkte speichern
            var fileSettings = await MainPage.SF.CreateFileAsync("s.txt", CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(fileSettings, (volumeMusic.ToString() + ";" + volumeEffects.ToString()));
        }
        // -------------------------------------------------------------------------------





        // Display einstellen
        // -------------------------------------------------------------------------------
        void SetDisplay()
        {
            // Musik
            if (volumeMusic >= 0.1f)
            {
                rtM1.Opacity = 1.0;
            }
            else
            {
                rtM1.Opacity = 0.3;
            }
            if (volumeMusic >= 0.2f)
            {
                rtM2.Opacity = 1.0;
            }
            else
            {
                rtM2.Opacity = 0.3;
            }
            if (volumeMusic >= 0.3f)
            {
                rtM3.Opacity = 1.0;
            }
            else
            {
                rtM3.Opacity = 0.3;
            }
            if (volumeMusic >= 0.4f)
            {
                rtM4.Opacity = 1.0;
            }
            else
            {
                rtM4.Opacity = 0.3;
            }
            if (volumeMusic >= 0.5f)
            {
                rtM5.Opacity = 1.0;
            }
            else
            {
                rtM5.Opacity = 0.3;
            }
            if (volumeMusic >= 0.6f)
            {
                rtM6.Opacity = 1.0;
            }
            else
            {
                rtM6.Opacity = 0.3;
            }
            if (volumeMusic >= 0.7f)
            {
                rtM7.Opacity = 1.0;
            }
            else
            {
                rtM7.Opacity = 0.3;
            }
            if (volumeMusic >= 0.8f)
            {
                rtM8.Opacity = 1.0;
            }
            else
            {
                rtM8.Opacity = 0.3;
            }
            if (volumeMusic >= 0.9f)
            {
                rtM9.Opacity = 1.0;
            }
            else
            {
                rtM9.Opacity = 0.3;
            }
            if (volumeMusic >= 1.0f)
            {
                rtM10.Opacity = 1.0;
            }
            else
            {
                rtM10.Opacity = 0.3;
            }

            // Effekte
            if (volumeEffects >= 0.1f)
            {
                rtE1.Opacity = 1.0;
            }
            else
            {
                rtE1.Opacity = 0.3;
            }
            if (volumeEffects >= 0.2f)
            {
                rtE2.Opacity = 1.0;
            }
            else
            {
                rtE2.Opacity = 0.3;
            }
            if (volumeEffects >= 0.3f)
            {
                rtE3.Opacity = 1.0;
            }
            else
            {
                rtE3.Opacity = 0.3;
            }
            if (volumeEffects >= 0.4f)
            {
                rtE4.Opacity = 1.0;
            }
            else
            {
                rtE4.Opacity = 0.3;
            }
            if (volumeEffects >= 0.5f)
            {
                rtE5.Opacity = 1.0;
            }
            else
            {
                rtE5.Opacity = 0.3;
            }
            if (volumeEffects >= 0.6f)
            {
                rtE6.Opacity = 1.0;
            }
            else
            {
                rtE6.Opacity = 0.3;
            }
            if (volumeEffects >= 0.7f)
            {
                rtE7.Opacity = 1.0;
            }
            else
            {
                rtE7.Opacity = 0.3;
            }
            if (volumeEffects >= 0.8f)
            {
                rtE8.Opacity = 1.0;
            }
            else
            {
                rtE8.Opacity = 0.3;
            }
            if (volumeEffects >= 0.9f)
            {
                rtE9.Opacity = 1.0;
            }
            else
            {
                rtE9.Opacity = 0.3;
            }
            if (volumeEffects >= 1.0f)
            {
                rtE10.Opacity = 1.0;
            }
            else
            {
                rtE10.Opacity = 0.3;
            }
        }
        // -------------------------------------------------------------------------------





        // Lautstärke einstellen
        // -------------------------------------------------------------------------------
        float selectFloat(double input, string action)
        {
            float output = 0.0f;
            if (action == "add")
            {
                if (input == 0.0f)
                {
                    output = 0.1f;
                }
                if (input == 0.1f)
                {
                    output = 0.2f;
                }
                if (input == 0.2f)
                {
                    output = 0.3f;
                }
                if (input == 0.3f)
                {
                    output = 0.4f;
                }
                if (input == 0.4f)
                {
                    output = 0.5f;
                }
                if (input == 0.5f)
                {
                    output = 0.6f;
                }
                if (input == 0.6f)
                {
                    output = 0.7f;
                }
                if (input == 0.7f)
                {
                    output = 0.8f;
                }
                if (input == 0.8f)
                {
                    output = 0.9f;
                }
                if (input == 0.9f)
                {
                    output = 1.0f;
                }
            }

            if (action == "minus")
            {
                if (input == 0.1f)
                {
                    output = 0.0f;
                }
                if (input == 0.2f)
                {
                    output = 0.1f;
                }
                if (input == 0.3f)
                {
                    output = 0.2f;
                }
                if (input == 0.4f)
                {
                    output = 0.3f;
                }
                if (input == 0.5f)
                {
                    output = 0.4f;
                }
                if (input == 0.6f)
                {
                    output = 0.5f;
                }
                if (input == 0.7f)
                {
                    output = 0.6f;
                }
                if (input == 0.8f)
                {
                    output = 0.7f;
                }
                if (input == 0.9f)
                {
                    output = 0.8f;
                }
                if (input == 1.0f)
                {
                    output = 0.9f;
                }
            }

              
            return output;
        }
        // -------------------------------------------------------------------------------




    }
}
