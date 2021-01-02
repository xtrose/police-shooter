using MyApp.Pages;
using SharpDX.IO;
using SharpDX.Multimedia;
using SharpDX.XAudio2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Advertising.WinRT.UI;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.ApplicationSettings;





// Namespace
namespace MyApp
{





    // Main PAge
    public sealed partial class MainPage : Page
    {





        // public static variablen
        // -------------------------------------------------------------------------------
        // CultureInfo laden
        public static string language = "en-us";

        // Resource loader erstellen
        public static ResourceLoader resource = new ResourceLoader();

        // Storage Daten festlegen
        public static StorageFolder SF = ApplicationData.Current.LocalFolder;
        public static StorageFile file;

        // Ausseres Grid erstellen
        public static Grid grOverAll = new Grid
        {
            Width = 928,
            Height = 480,
        };

        // Main Grid
        public static Grid grMain = new Grid
        {
            Width = 800,
            Height = 480,
            HorizontalAlignment = HorizontalAlignment.Left,
        };

        // Viewbox für die Werbung
        public static Viewbox vbAd = new Viewbox
        {
            Width = 128,
            Height = 480,
            HorizontalAlignment = HorizontalAlignment.Right,
        };

        // Grid für die Werbung
        public static Grid grAd = new Grid
        {
            Height = 600,
            Width = 160,
        };

        // Werbung erstellen
        public static AdControl ac = new AdControl
        {
            AdUnitId = "226948",
            ApplicationId = "73a5bb12-608a-4028-a1ab-64a33fe2774b",
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Right,
            Width = 160,
            Height = 600,
        };

        // Main Frame
        public static Frame frMain = new Frame
        {
            Width = 800,
            Height = 480,
            Margin = new Thickness(0, 0, 0, 0),
            VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top,
            HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center,
        };

        // Bitmap Image
        public static BitmapImage bi = new BitmapImage
        {
            UriSource = new Uri("ms-appx:/Images/Back.png", UriKind.RelativeOrAbsolute),
        };

        // Button Back
        public static Image imgBack = new Image
        {
            Source = bi,
            Width = 40,
            Height = 40,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Right,
            Margin = new Thickness(0,14,20,0),
        };

        // Gibt an welche Seite geladen ist
        public static string framePage = "MainPage";

        // Bildschirm
        DisplayRequest displayRequest = null;

        // Sound einstellungen
        public static float volumeEffects = 1.0f;
        public static float volumeMusic = 0.8f;
        public static bool settingLoaded = false;

        // Spiel Variablen
        public static int inFrameCount = 0;
        public static string gun = "pistol";
        public static int ammunition = 12;
        public static int inPoints = 0;
        public static int inPointsAll = 0;
        public static int inLives = 0;
        public static int inLevel = 10;
        public static int inShotNext = 0;
        public static string stSide = "white";
        public static string stage = "";

        public static int inNext1 = 15;
        public static int inNext2 = 30;
        public static int inNextCount = 20;
        public static int inEndCount = 40;
        public static int inEndCount2 = 50;
        public static int inPointsNextLevel = 100;
        public static int inPointsNextLevelR = 100;
        public static bool boFirstGame = true;
        public static bool boReload = false;

        // Gibt an ob Pause ist
        public static bool boPause = false;

        // Gibt an ob das Spiel gespeichet wurde
        public static bool saved = true;

        // Gibt an ob getroffen wurde
        public static bool hit = false;

        // Media Element für Hintergundmusik
        public static MediaElement backgroundMusic = new MediaElement
        {
            AudioCategory = AudioCategory.GameEffects,
            IsLooping = true,
            AutoPlay = true,
        };

        // Ob gekauft
        public static bool buyed = false;
        // -------------------------------------------------------------------------------





        // Wird beim ersten Start der Seite ausgeführt
        // -------------------------------------------------------------------------------
        public MainPage()
        {
            // UI Komponenten laden
            this.InitializeComponent();

            // Settings Flyout erstellen
            SettingsPane.GetForCurrentView().CommandsRequested += (s, e) =>
            {
                SettingsCommand defaultsCommand = new SettingsCommand(MainPage.resource.GetString("001_PrivacyString"), MainPage.resource.GetString("001_PrivacyString"),
                    (handler) =>
                    {
                        SettingsFlyout1 sf = new SettingsFlyout1();
                        sf.Show();
                    });
                e.Request.ApplicationCommands.Add(defaultsCommand);
            };

            // Seite zusammenbauen
            vbOverAll.Children.Add(grOverAll);
            grOverAll.Children.Add(grMain);

            grAd.Children.Add(ac);
            vbAd.Child = grAd;
            grOverAll.Children.Add(vbAd);

            grMain.Children.Add(frMain);
            grMain.Children.Add(imgBack);
            imgBack.PointerReleased += imgBack_PointerReleased;
            

            // Menü laden
            frMain.Navigate(typeof(MenuMain));

            // Musikplayer in Grid für alles hinzufügen
            grOverAll.Children.Add(backgroundMusic);
        }
        // -------------------------------------------------------------------------------





        // Wird bei jedem Start der Seite ausgeführt
        // -------------------------------------------------------------------------------
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Bildschirmschoner deaktivieren
            if (displayRequest == null)
            {
                displayRequest = new DisplayRequest();
                displayRequest.RequestActive();
            }
        }
        // -------------------------------------------------------------------------------





        // Sound Effekt erstellen
        // ---------------------------------------------------------------------------------------------------
        private static Dictionary<string, SourceVoice> LoadedSounds = new Dictionary<string, SourceVoice>();
        private static Dictionary<string, AudioBufferAndMetaData> AudioBuffers = new Dictionary<string, AudioBufferAndMetaData>();
        private static MasteringVoice m_MasteringVoice;
        public static MasteringVoice MasteringVoice
        {
            get
            {
                if (m_MasteringVoice == null)
                {
                    m_MasteringVoice = new MasteringVoice(XAudio);
                    m_MasteringVoice.SetVolume(1, 0);
                }
                return m_MasteringVoice;
            }
        }
        private static XAudio2 m_XAudio;
        public static XAudio2 XAudio
        {
            get
            {
                if (m_XAudio == null)
                {
                    m_XAudio = new XAudio2();
                    var voice = MasteringVoice; //touch voice to create it
                    m_XAudio.StartEngine();
                }
                return m_XAudio;
            }
        }
        public static void PlaySound(string soundfile, float volume = 1)
        {
            SourceVoice sourceVoice;
            if (!LoadedSounds.ContainsKey(soundfile))
            {

                var buffer = GetBuffer(soundfile);
                sourceVoice = new SourceVoice(XAudio, buffer.WaveFormat, true);
                sourceVoice.SetVolume(volume, SharpDX.XAudio2.XAudio2.CommitNow);
                sourceVoice.SubmitSourceBuffer(buffer, buffer.DecodedPacketsInfo);
                sourceVoice.Start();
            }
            else
            {
                sourceVoice = LoadedSounds[soundfile];
                if (sourceVoice != null)
                    sourceVoice.Stop();
            }
        }
        private static AudioBufferAndMetaData GetBuffer(string soundfile)
        {
            if (!AudioBuffers.ContainsKey(soundfile))
            {
                var nativefilestream = new NativeFileStream(
                        soundfile,
                        NativeFileMode.Open,
                        NativeFileAccess.Read,
                        NativeFileShare.Read);

                var soundstream = new SoundStream(nativefilestream);

                var buffer = new AudioBufferAndMetaData()
                {
                    Stream = soundstream.ToDataStream(),
                    AudioBytes = (int)soundstream.Length,
                    Flags = BufferFlags.EndOfStream,
                    WaveFormat = soundstream.Format,
                    DecodedPacketsInfo = soundstream.DecodedPacketsInfo
                };
                AudioBuffers[soundfile] = buffer;
            }
            return AudioBuffers[soundfile];

        }
        private sealed class AudioBufferAndMetaData : AudioBuffer
        {
            public WaveFormat WaveFormat { get; set; }
            public uint[] DecodedPacketsInfo { get; set; }
        }
        // ---------------------------------------------------------------------------------------------------





        // Punkte laden oder speichern
        // ---------------------------------------------------------------------------------------------------
        public static async void loadSavePoints(string stAction)
        {
            // Wenn Daten geladen werden
            if (stAction == "load")
            {
                // Gesamtpunktzahl laden
                var filePoints = await SF.CreateFileAsync("p.txt", CreationCollisionOption.OpenIfExists);
                var tempPoints = await FileIO.ReadTextAsync(filePoints);
                // Wenn Sprachdatei noch nicht bestht
                if ((Convert.ToString(tempPoints)).Length > 0)
                {
                    // Sprache festlegen
                    inPointsAll = Convert.ToInt32(tempPoints);
                }
                // Wenn keine Sprachdatei besteht
                else
                {
                    await FileIO.WriteTextAsync(filePoints, "0");
                }
            }
            // Wenn daten gespeichet werden
            else if (stAction == "save")
            {
                // Gesamtpunktzahl laden
                var filePoints = await SF.CreateFileAsync("p.txt", CreationCollisionOption.OpenIfExists);
                await FileIO.WriteTextAsync(filePoints, inPointsAll.ToString());
            }
        }
        // ---------------------------------------------------------------------------------------------------





        // Einstellungen laden und speichern
        // ---------------------------------------------------------------------------------------------------
        public static async void loadSaveSettings(string stAction)
        {
            // Wenn Daten geladen werden
            if (stAction == "load")
            {
                // Einstellungs Datei laden
                var fileSettings = await SF.CreateFileAsync("s.txt", CreationCollisionOption.OpenIfExists);
                var tempSettings = await FileIO.ReadTextAsync(fileSettings);
                // Wenn Datei besteht
                if ((Convert.ToString(tempSettings)).Length > 0)
                {
                    // Datei erstellen
                    string[] arTemp = Regex.Split(tempSettings, ";");
                    volumeMusic = Convert.ToSingle(arTemp[0]);
                    volumeEffects = Convert.ToSingle(arTemp[1]);
                }
                // Wenn Datei nicht besteht
                else
                {
                    await FileIO.WriteTextAsync(fileSettings, "0.8f;1.0f");
                }
            }
            // Wenn daten gespeichet werden
            else if (stAction == "save")
            {
                // Gesamtpunktzahl laden
                var fileSettings = await SF.CreateFileAsync("s.txt", CreationCollisionOption.OpenIfExists);
                await FileIO.WriteTextAsync(fileSettings, (volumeMusic.ToString() + ";" + volumeEffects.ToString()));
            }
        }
        // ---------------------------------------------------------------------------------------------------





        // Back Button
        // -------------------------------------------------------------------------------
        void imgBack_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            // Bei Levels
            if (framePage == "Game" | framePage == "Game2" | framePage == "Game3" | framePage == "Game4" | framePage == "Game5")
            {
                // Wenn Pause gedrückt ist
                if (boPause)
                {
                    // Pause beenden
                    boPause = false;
                }
                // Wenn Pause nicht gedrückt ist
                else
                {
                    // Wenn GameOver
                    if (inLives <= 0)
                    {
                        // Zurück zum Menü
                        frMain.GoBack();
                    }
                    // Wenn nicht Game Over
                    else
                    {
                        // Pause drücken
                        boPause = true;
                    }
                }
            }

            // Bei About Seite
            else if (framePage == "About")
            {
                // Hauptmenü laden
                frMain.GoBack();
            }

            // Bei Hauptmenü
            else if (framePage == "MenuMain")
            {
                // Zurückseite laden
                frMain.Navigate(typeof(Exit));
            }

            // Bei Exit Seite
            else if (framePage == "Exit")
            {
                // Zurückseite laden
                frMain.GoBack();
            }

            // Bei Musik Seite
            else if (framePage == "Music")
            {
                // Zurückseite laden
                frMain.GoBack();
            }

            // Bei Settings Seite
            else if (framePage == "Settings")
            {
                // Einstellungen speichern
                loadSaveSettings("save");
                // Zurückseite laden
                frMain.GoBack();
            }

            // Bei Settings Seite
            else if (framePage == "Rate")
            {
            }
        }
        // -------------------------------------------------------------------------------




    }
}
