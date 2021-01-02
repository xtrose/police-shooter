using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
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
    




    // Hauptmenü
    public sealed partial class MenuMain : Page
    {





        // Variablen
        // -------------------------------------------------------------------------------
        // Lizens Informationen
        LicenseInformation licenseInformation;
        ListingInformation listing;

        // Gibt an ob App gekauft ist
        bool buyed = false;
        // -------------------------------------------------------------------------------





        // Wird beim ersten Aufruf der Seite geladen
        // -------------------------------------------------------------------------------
        public MenuMain()
        {
            // UI Komponenten laden
            this.InitializeComponent();

            // Angeben auf welcher Seite
            MainPage.framePage = "MenuMain";

            // Punkte laden und Levels freischalten
            loadPoints();

            // Einstellungen laden
            if (!MainPage.settingLoaded)
            {
                loadSettings();
            }

            // Hintergundmusik abspielen
            MainPage.backgroundMusic.Source = new Uri("ms-appx:///Sounds/MenuMain.mp3", UriKind.RelativeOrAbsolute);
            MainPage.backgroundMusic.Play();

            // Einstellungen laden
            if (!MainPage.settingLoaded)
            {
                checkRate();
                MainPage.settingLoaded = true;
            }
        }
        // -------------------------------------------------------------------------------





        // Bewertungen prüfen
        //---------------------------------------------------------------------------------------------------------
        public async void checkRate()
        {
            // ausgabe
            bool rt = false;

            // Open Count laden
            var tempFile = await MainPage.SF.CreateFileAsync("c.txt", CreationCollisionOption.OpenIfExists);
            // String aus File lesen
            var tempStringCount = await FileIO.ReadTextAsync(tempFile);
            // Count erstellen
            int openCount = -1;
            // Wenn Count bereits existiert
            if (tempStringCount.Length > 0)
            {
                // Count erstellen
                openCount = Convert.ToInt32(tempStringCount.Trim());
            }
            // Wenn Count nicht existiert
            else
            {
                // Count erstellen
                openCount = 0;
            }
            // Count neu erstellen
            openCount++;
            await FileIO.WriteTextAsync(tempFile, openCount.ToString());

            // Wenn Rate ausgegeben wird
            if (openCount == 4)
            {
                rt = true;
            }

            // Bewertung öffnen wenn ausgewählt
            if (rt)
            {
                MainPage.frMain.Navigate(typeof(Pages.Rate));
            }
        }
        //---------------------------------------------------------------------------------------------------------





        // Punkte laden
        // -------------------------------------------------------------------------------
        async void loadPoints()
        {
            // Gesamtpunktzahl laden
            var filePoints = await MainPage.SF.CreateFileAsync("p.txt", CreationCollisionOption.OpenIfExists);
            var tempPoints = await FileIO.ReadTextAsync(filePoints);
            // Wenn Datei mit den Punkten noch nicht besteht
            if ((Convert.ToString(tempPoints)).Length > 0)
            {
                // Gesamtpunktzahl festlegen
                MainPage.inPointsAll = Convert.ToInt32(tempPoints);
                // Gesamtpunktzahl ausgeben
                tbPointsAll.Text = tempPoints;
                tbPointsAll2.Text = tempPoints;
                // Levels freischalten
                if (Convert.ToInt32(tempPoints) >= 1000)
                {
                    spLock2.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    imgLock2.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    spLock5.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }
                if (Convert.ToInt32(tempPoints) >= 2500)
                {
                    spLock3.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    imgLock3.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    spLock4.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }
                if (Convert.ToInt32(tempPoints) >= 5000)
                {
                    spLock6.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }
                if (Convert.ToInt32(tempPoints) >= 8000)
                {
                    spLock7.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }
                if (Convert.ToInt32(tempPoints) >= 12000)
                {
                    spLock8.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }
            }
            // Wenn keine Sprachdatei besteht
            else
            {
                await FileIO.WriteTextAsync(filePoints, "0");
            }
        }
        // -------------------------------------------------------------------------------





        // Einstellungen laden
        // ---------------------------------------------------------------------------------------------------
        public static async void loadSettings()
        {
            // Temp Music
            float tempMusic = 0.8f;
            // Einstellungs Datei laden
            var fileSettings = await MainPage.SF.CreateFileAsync("s.txt", CreationCollisionOption.OpenIfExists);
            var tempSettings = await FileIO.ReadTextAsync(fileSettings);
            // Wenn Datei besteht
            if ((Convert.ToString(tempSettings)).Length > 0)
            {
                // Datei erstellen
                string[] arTemp = Regex.Split(tempSettings, ";");
                MainPage.volumeMusic = Convert.ToSingle(arTemp[0]);
                MainPage.volumeEffects = Convert.ToSingle(arTemp[1]);
                tempMusic = Convert.ToSingle(arTemp[0]);
            }
            // Wenn Datei nicht besteht
            else
            {
                await FileIO.WriteTextAsync(fileSettings, (MainPage.volumeMusic.ToString() +  ";" + MainPage.volumeEffects.ToString()));
            }
            MainPage.backgroundMusic.Volume = tempMusic;
        }
        // ---------------------------------------------------------------------------------------------------





        // Wird zu jedem Start der Seite geladen
        // ---------------------------------------------------------------------------------------------------
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Prüfen ob bereits geprüft oder gekauft
            if (MainPage.buyed)
            {
                // Angeben das gekauft ist
                buyed = true;
                setFullVersion();
            }
            // Wenn noch nicht geprüft oder gekauft
            else
            {


#if DEBUG
                // Storage Proxy erstellen
                StorageFolder proxyDataFolder = await Package.Current.InstalledLocation.GetFolderAsync("data");
                StorageFile proxyFile = await proxyDataFolder.GetFileAsync("InAppPurchase.xml");
                await CurrentAppSimulator.ReloadSimulatorAsync(proxyFile);
                // Lizensinformationen überprüfen
                licenseInformation = CurrentAppSimulator.LicenseInformation;
                // Lizenzinformationen erstellen
                listing = await CurrentAppSimulator.LoadListingInformationAsync();
#else
                try
                {
                    // Lizensinformationen überprüfen
                    licenseInformation = CurrentApp.LicenseInformation;
                    // Lizenzinformationen erstellen
                    listing = await CurrentApp.LoadListingInformationAsync();
                }
                catch
                { }
#endif


                // Prüfen ob App gekauft ist
                try
                {
                    if (licenseInformation.ProductLicenses["US_Police_Shooter_FV"].IsActive)
                    {
                        setFullVersion();
                    }
                }
                catch
                {
                }
            }

        }
        // ---------------------------------------------------------------------------------------------------





        // Vollversion
        // ---------------------------------------------------------------------------------------------------
        void setFullVersion()
        {
            // Abgeben das App gekauft ist
            buyed = true;
            MainPage.buyed = true;

            // Werbung entfernen
            MainPage.ac.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            // Kaufen Button ausblenden
            btnBuy.Opacity = 0.0;
            imgBuy4.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            imgBuy5.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            imgBuy6.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            imgBuy7.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            imgBuy8.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            // Prüfen ob man Lock Zeichen entfernen kann
            if (MainPage.inPointsAll >= 1000)
            {
                imgLock5.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            if (MainPage.inPointsAll >= 2500)
            {
                imgLock4.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            if (MainPage.inPointsAll >= 5000)
            {
                imgLock6.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            if (MainPage.inPointsAll >= 8000)
            {
                imgLock7.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            if (MainPage.inPointsAll >= 12000)
            {
                imgLock8.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }
        // ---------------------------------------------------------------------------------------------------


        
        
        
        // Buttons der Levels
        // -------------------------------------------------------------------------------
        // Button Ferguson
        private void Image_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            MainPage.stage = "Milwaukee";
            startGame();
        }

        // Button New York
        private void Image_PointerPressed_1(object sender, PointerRoutedEventArgs e)
        {
            if (MainPage.inPointsAll >= 1000)
            {
                MainPage.stage = "NewYork";
                startGame();
            }
            else
            {
                MainPage.PlaySound("Sounds/DontWork.wav", MainPage.volumeEffects);
            }
        }

        // Button Arizona
        private void Image_PointerPressed_2(object sender, PointerRoutedEventArgs e)
        {
            if (MainPage.inPointsAll >= 2500)
            {
                MainPage.stage = "Arizona";
                startGame();
            }
            else
            {
                MainPage.PlaySound("Sounds/DontWork.wav", MainPage.volumeEffects);
            }
        }

        // Button Texas
        private void Image_PointerPressed_5(object sender, PointerRoutedEventArgs e)
        {
            if (MainPage.inPointsAll >= 1000 & buyed)
            {
                MainPage.stage = "Texas";
                startGame();
            }
            else
            {
                MainPage.PlaySound("Sounds/DontWork.wav", MainPage.volumeEffects);
            }
        }

        // Button Washington
        private void Image_PointerPressed_3(object sender, PointerRoutedEventArgs e)
        {
            if (MainPage.inPointsAll >= 2500 & buyed)
            {
                MainPage.stage = "Washington";
                startGame();
            }
            else
            {
                MainPage.PlaySound("Sounds/DontWork.wav", MainPage.volumeEffects);
            }
        }

        // Button San Francisco
        private void Image_PointerPressed_6(object sender, PointerRoutedEventArgs e)
        {
            if (MainPage.inPointsAll >= 5000 & buyed)
            {
                MainPage.stage = "SanFrancisco";
                startGame();
            }
            else
            {
                MainPage.PlaySound("Sounds/DontWork.wav", MainPage.volumeEffects);
            }
        }

        // Button Tennessee
        private void Image_PointerPressed_7(object sender, PointerRoutedEventArgs e)
        {
            if (MainPage.inPointsAll >= 8000 & buyed)
            {
                MainPage.stage = "Tennessee";
                startGame();
            }
            else
            {
                MainPage.PlaySound("Sounds/DontWork.wav", MainPage.volumeEffects);
            }
        }

        // Button Berlin
        private void Image_PointerPressed_8(object sender, PointerRoutedEventArgs e)
        {
            if (MainPage.inPointsAll >= 12000 & buyed)
            {
                MainPage.stage = "Berlin";
                startGame();
            }
            else
            {
                MainPage.PlaySound("Sounds/DontWork.wav", MainPage.volumeEffects);
            }
        }
        // -------------------------------------------------------------------------------





        // Seiten Auswahlmenü
        // -------------------------------------------------------------------------------
        private void White_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            MainPage.stSide = "white";
            startGame();
        }

        private void Black_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            MainPage.stSide = "black";
            startGame();
        }

        private void Back_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            grSelectSide.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }
        // -------------------------------------------------------------------------------





        // Level Starten
        // -------------------------------------------------------------------------------
        void startGame()
        {
            // Variablen zurücksetzen
            MainPage.inFrameCount = 0;
            MainPage.gun = "pistol";
            MainPage.ammunition = 12;
            MainPage.inPoints = 0;
            MainPage.inLives = 0;
            MainPage.inLevel = 1;
            MainPage.inShotNext = 0;

            MainPage.inNext1 = 15;
            MainPage.inNext2 = 30;
            MainPage.inNextCount = 20;
            MainPage.inEndCount = 40;
            MainPage.inEndCount2 = 50;
            MainPage.inPointsNextLevel = 100;
            MainPage.inPointsNextLevelR = 100;
            MainPage.boFirstGame = true;
            MainPage.boReload = false;

            // Level Starten
            if (MainPage.stage == "Milwaukee")
            {
                // Hintergundmusik abspielen
                MainPage.backgroundMusic.Source = new Uri("ms-appx:///Sounds/LevelMusic.mp3", UriKind.RelativeOrAbsolute);
                MainPage.backgroundMusic.Play();
                MainPage.frMain.Navigate(typeof(Game));
            }
            else if (MainPage.stage == "NewYork")
            {
                // Hintergundmusik abspielen
                MainPage.backgroundMusic.Source = new Uri("ms-appx:///Sounds/LevelMusic2.mp3", UriKind.RelativeOrAbsolute);
                MainPage.backgroundMusic.Play();
                MainPage.frMain.Navigate(typeof(Game2));
            }
            else if (MainPage.stage == "Arizona")
            {
                // Hintergundmusik abspielen
                MainPage.backgroundMusic.Source = new Uri("ms-appx:///Sounds/LevelMusic.mp3", UriKind.RelativeOrAbsolute);
                MainPage.backgroundMusic.Play();
                MainPage.frMain.Navigate(typeof(Game3));
            }
            else if (MainPage.stage == "Texas")
            {
                // Hintergundmusik abspielen
                MainPage.backgroundMusic.Source = new Uri("ms-appx:///Sounds/LevelMusic2.mp3", UriKind.RelativeOrAbsolute);
                MainPage.backgroundMusic.Play();
                MainPage.frMain.Navigate(typeof(Game5));
            }
            else if (MainPage.stage == "SanFrancisco")
            {
                // Hintergundmusik abspielen
                MainPage.backgroundMusic.Source = new Uri("ms-appx:///Sounds/LevelMusic2.mp3", UriKind.RelativeOrAbsolute);
                MainPage.backgroundMusic.Play();
                MainPage.frMain.Navigate(typeof(Game6));
            }
            else if (MainPage.stage == "Tennessee")
            {
                // Hintergundmusik abspielen
                MainPage.backgroundMusic.Source = new Uri("ms-appx:///Sounds/LevelMusic.mp3", UriKind.RelativeOrAbsolute);
                MainPage.backgroundMusic.Play();
                MainPage.frMain.Navigate(typeof(Game7));
            }
            else if (MainPage.stage == "Berlin")
            {
                // Hintergundmusik abspielen
                MainPage.backgroundMusic.Source = new Uri("ms-appx:///Sounds/LevelMusic2.mp3", UriKind.RelativeOrAbsolute);
                MainPage.backgroundMusic.Play();
                MainPage.frMain.Navigate(typeof(Game8));
            }
            else
            {
                // Hintergundmusik abspielen
                MainPage.backgroundMusic.Source = new Uri("ms-appx:///Sounds/LevelMusic.mp3", UriKind.RelativeOrAbsolute);
                MainPage.backgroundMusic.Play();
                MainPage.frMain.Navigate(typeof(Game4));
            }
        }
        // -------------------------------------------------------------------------------





        // Buttons About und Buy
        // -------------------------------------------------------------------------------
        // Button About
        private void About_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            // About Seite öffnen
            MainPage.frMain.Navigate(typeof(About));
        }

        // Button Settings
        private void Settings_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            // About Seite öffnen
            MainPage.frMain.Navigate(typeof(Settings));
        }

        // Button Buy
        private async void Buy_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            // Wenn App noch nicht gekauft
            if (buyed == false)
            {
                // Wenn noch nicht gekauft wurde und noch nicht alle gekauft wurden
                if (!licenseInformation.ProductLicenses["US_Police_Shooter_FV"].IsActive)
                {
                    // Versuchen kauf auszuführen
                    try
                    {
#if DEBUG
                        await CurrentAppSimulator.RequestProductPurchaseAsync("US_Police_Shooter_FV");
#else
                        // Kauf aufrufen
                        await CurrentApp.RequestProductPurchaseAsync("US_Police_Shooter_FV");
#endif


                        // Wenn gekauft wurde
                        if (licenseInformation.ProductLicenses["US_Police_Shooter_FV"].IsActive)
                        {
                            setFullVersion();
                        }
                    }
                    // Wenn Kauf fehlgeschlagen
                    catch (Exception)
                    {
                        // Benachrichtigung ausgeben, das Kauf nicht abgeschlossen
                        MessageDialog messageDialog = new MessageDialog(MainPage.resource.GetString("001_connectionFailedString"));
                        messageBox(messageDialog);
                    }
                }
                // Wenn bereits gekauft wurde
                else
                {
                    // Auf Vollversion stellen
                    setFullVersion();
                }
            }
        }
        // -------------------------------------------------------------------------------





        // Einfache Benachrichtigung
        // ---------------------------------------------------------------------------------------------------
        async void messageBox(MessageDialog messageDialog)
        {
            // Benachrichtigung ausgeben
            await messageDialog.ShowAsync();
        }
        // ---------------------------------------------------------------------------------------------------        




    }
}
