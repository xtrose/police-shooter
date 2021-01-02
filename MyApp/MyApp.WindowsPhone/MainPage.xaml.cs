using SharpDX.IO;
using SharpDX.Multimedia;
using SharpDX.XAudio2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MyApp.Pages;
using Windows.Media;
using Windows.Storage;
using Microsoft.Advertising.Mobile.UI;
using System.Text.RegularExpressions;





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

        // Werbung erstellen
        public static AdControl ac = new AdControl
        {
            AdUnitId = "226947",
            ApplicationId = "dd8f2a8a-d98c-48c9-95cd-65bf65efb2df",
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Right,
            Width = 300,
            Height = 50,
        };

        // Main Frame
        public static Frame frMain = new Frame();
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

            // Back Button festlegen
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            // Statusbar entfernen
            hideStatusBar();

            // Werbung hinzufügen
            grOverAll.Children.Add(ac);

            // Main Frame einstellen
            frMain.Width = 800;
            frMain.Height = 480;
            frMain.Margin = new Thickness(0, 0, 0, 0);
            frMain.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
            frMain.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            grMain.Children.Add(frMain);

            // Menü laden
            frMain.Navigate(typeof(MenuMain));

            // Musikplayer in Grid für alles hinzufügen
            grOverAll.Children.Add(backgroundMusic);
        }


        // StatusBar verschwinden lassen
        private async void hideStatusBar()
        {
            StatusBar statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
            await statusBar.HideAsync();
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
        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
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

            // Ausblenden Abbrechen
            e.Handled = true;
        }
        // -------------------------------------------------------------------------------




    }
}
