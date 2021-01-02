using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;





// Namespace
namespace MyApp.Pages
{





    // Seite des Spiels
    public sealed partial class Game8 : Page
    {





        // Variablen
        // -------------------------------------------------------------------------------
        // Timer, der den Spielablauf steuert
        DispatcherTimer timer = new DispatcherTimer();

        // DateTime, welches das Blinken steuert
        DateTime dtBlink = new DateTime();
        bool blBlink = false;

        // Random erstellen
        Random rand = new Random();

        // Koordinaten der Figuren // Ebene, X, Y
        string[] coordinates = new string[] { "1;90;78", "1;161;78", "1;231;78", "1;301;78", "1;371;65", "1;441;78", "1;511;78", "1;581;78", "1;647;78", "2;140;298", "2;241;298", "2;330;298", "2;392;298", "2;482;298", "2;583;298", "2;715;196", "2;4;196", "2;76;142", "2;156;142", "2;236;142", "2;481;142", "2;561;142", "2;641;142", "3;5;292", "3;700;288", "4;76;385", "4;176;385", "4;276;385", "4;376;385", "4;476;385", "4;576;385", "4;676;385" };
        //string stLevelDatas = new string[] { "20 30 40", "19 29 39" };

        // Liste der Bilder
        List<ClassMaennle> listMaennle = new List<ClassMaennle>();

        // Liste der Koordinaten
        List<ClassCoordinates> listCoordinates = new List<ClassCoordinates>();

        // Liste der Bilder der Männla laden
        List<ClassImagesMaennle> listImagesMaennle = new List<ClassImagesMaennle>();

        // Bauteile, die geladen werden
        BitmapImage reload;
        BitmapImage reloadBlink;
        BitmapImage biPistol;
        BitmapImage biShotgun;
        BitmapImage biMg;
        // -------------------------------------------------------------------------------


        


        // Wird beim ersten Start der Seite ausgeführt
        // -------------------------------------------------------------------------------
        public Game8()
        {
            // UI Komponenten laden
            this.InitializeComponent();

            // Navigationsvariable erstellen
            MainPage.framePage = "Game8";

            MainPage.boPause = false;

            // Temponäre Werbeanzeige entfernen
            rtTempAd.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            // Timer einstellen
            timer.Interval = new TimeSpan(0, 0, 0, 0, 33);
            timer.Tick += timerTick;
            timer.Start();

            // Liste der Koordinaten erstellen
            for (int i = 0; i < coordinates.Count(); i++)
            {
                string test = coordinates[i];
                string[] splitStCoordinates = Regex.Split(coordinates[i], ";");
                listCoordinates.Add(new ClassCoordinates(i, Convert.ToInt32(splitStCoordinates[0]), Convert.ToInt32(splitStCoordinates[1]), Convert.ToInt32(splitStCoordinates[2])));
            }

            // Bilder der Männle erstellen
            for (int i = 1; i <= 13; i++)
            {
                listImagesMaennle.Add(new ClassImagesMaennle(i));
            }

            // Sonstige Bilder laden
            reload = new BitmapImage();
            reload.UriSource = new Uri("ms-appx:/Images/Reload.png", UriKind.RelativeOrAbsolute);
            reloadBlink = new BitmapImage();
            reloadBlink.UriSource = new Uri("ms-appx:/Images/Reload_Blink.png", UriKind.RelativeOrAbsolute);
            biPistol = new BitmapImage();
            biPistol.UriSource = new Uri("ms-appx:/Images/Pistol.png", UriKind.RelativeOrAbsolute);
            biShotgun = new BitmapImage();
            biShotgun.UriSource = new Uri("ms-appx:/Images/Shotgun.png", UriKind.RelativeOrAbsolute);
            biMg = new BitmapImage();
            biMg.UriSource = new Uri("ms-appx:/Images/Mg.png", UriKind.RelativeOrAbsolute);
        }
        // -------------------------------------------------------------------------------





        // Spiel zurücksetzen
        // -------------------------------------------------------------------------------
        void resetGame()
        {
            // Spielvariablen zurücksetzen
            MainPage.inFrameCount = 0;
            MainPage.inShotNext = 2;
            MainPage.gun = "pistol";
            MainPage.ammunition = 14;
            MainPage.inLives = 10;
            MainPage.inLevel = 1;
            MainPage.inPoints = 0;
            dtBlink = DateTime.Now;

            MainPage.inNext1 = 15;
            MainPage.inNext2 = 30;
            MainPage.inNextCount = 20;
            MainPage.inEndCount = 40;
            MainPage.inEndCount2 = 50;
            MainPage.inPointsNextLevel = 100;
            MainPage.inPointsNextLevelR = 100;
            MainPage.boReload = false;
            MainPage.saved = false;

            // Listen leeren
            listMaennle.Clear();
            grLayer1.Children.Clear();
            grLayer2.Children.Clear();
            grLayer3.Children.Clear();
            grLayer4.Children.Clear();
        }
        // -------------------------------------------------------------------------------





        // Funktion um einen String zu Shuffeln
        // -------------------------------------------------------------------------------
        string[] RandomizeStrings(string[] arr)
        {
            List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();
            // Add all strings from array
            // Add new random int each time
            foreach (string s in arr)
            {
                list.Add(new KeyValuePair<int, string>(rand.Next(), s));
            }
            // Sort the list by the random number
            var sorted = from item in list
                         orderby item.Key
                         select item;
            // Allocate new string array
            string[] result = new string[arr.Length];
            // Copy values to array
            int index = 0;
            foreach (KeyValuePair<int, string> pair in sorted)
            {
                result[index] = pair.Value;
                index++;
            }
            // Return copied array
            return result;
        }
        // -------------------------------------------------------------------------------





        // Timer, der den Spielablauf steuert
        // -------------------------------------------------------------------------------
        void timerTick(object sender, object e)
        {
            // Wenn Hit Grid sichtbar
            if (grHit.Visibility == Windows.UI.Xaml.Visibility.Visible)
            {
                grHit.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }


            // Wenn Pause gedrückt wurde
            if (MainPage.boPause)
            {
                // Pause Bild einblenden
                grGameOver.Visibility = Windows.UI.Xaml.Visibility.Visible;
                if (!MainPage.boFirstGame)
                {
                    tbGameOver.Text = MainPage.resource.GetString("001_pause");
                    tbGameOver2.Text = MainPage.resource.GetString("001_pause");
                }
            }
            // Wenn keine Pause gedrückt ist
            else
            {
                // Leben updaten
                if (MainPage.inLives <= 0)
                {
                    // Spiel speichern
                    if (MainPage.saved == false)
                    {
                        MainPage.inPointsAll += MainPage.inPoints;
                        MainPage.loadSavePoints("save");
                        MainPage.saved = true;
                    }
                    // Anzeige auf 0 stellen
                    tbLives.Text = "0";
                    tbLives2.Text = "0";
                    // Game Over Bild einblenden
                    grGameOver.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    if (!MainPage.boFirstGame)
                    {
                        tbGameOver.Text = MainPage.resource.GetString("001_over");
                        tbGameOver2.Text = MainPage.resource.GetString("001_over");
                    }
                    // Blinken ausstellen
                    imgReload.Source = reload;
                }
                else
                {
                    // Anzeige einstellen
                    tbLives.Text = MainPage.inLives.ToString();
                    tbLives2.Text = MainPage.inLives.ToString();
                }


                // Allgemeine Aktionen
                // Count erhöhen
                MainPage.inFrameCount++;



                // Wenn nicht Game Over
                if (MainPage.inLives > 0)
                {
                    // Munitionsanzeige updaten
                    tbReload.Text = MainPage.ammunition.ToString();

                    // Punkte updaten
                    tbPoints.Text = MainPage.inPoints.ToString();
                    tbPoints2.Text = MainPage.inPoints.ToString();

                    // Level updaten
                    if (MainPage.inPoints >= MainPage.inPointsNextLevelR)
                    {
                        MainPage.inLevel++;
                        MainPage.inPointsNextLevelR += MainPage.inPointsNextLevel;
                        MainPage.inNext1--;
                        MainPage.inNext2--;
                        MainPage.inEndCount--;
                    }
                    tbLevel.Text = MainPage.inLevel.ToString();
                    tbLevel2.Text = MainPage.inLevel.ToString();

                    // Blinker einstellen
                    DateTime now = DateTime.Now;
                    if (dtBlink.AddMilliseconds(500) <= now)
                    {
                        if (blBlink)
                        {
                            blBlink = false;
                        }
                        else
                        {
                            blBlink = true;
                        }
                        dtBlink = DateTime.Now;
                    }

                    // Wenn Reload Blinkt
                    if (MainPage.ammunition == 0 & blBlink == true)
                    {
                        imgReload.Source = reloadBlink;
                    }
                    else
                    {
                        imgReload.Source = reload;
                    }

                    // Reload Sound ausgeben
                    if (MainPage.ammunition == 0 & !MainPage.boReload)
                    {
                        MainPage.PlaySound("Sounds/SpeechReload.wav", MainPage.volumeEffects);
                        MainPage.boReload = true;
                    }
                    else if (MainPage.ammunition > 0)
                    {
                        MainPage.boReload = false;
                    }

                    // Bild der Waffe einstellen
                    if (MainPage.gun == "pistol")
                    {
                        imgWeapon.Source = biPistol;
                    }
                    else if (MainPage.gun == "shotgun")
                    {
                        imgWeapon.Source = biShotgun;
                    }
                    else if (MainPage.gun == "mg")
                    {
                        imgWeapon.Source = biMg;
                    }
                }



                // Männla durhlaufen und ändern
                // String der zu löschenden Männla erstellen
                string stDeleteMaennle = "";
                // Männle durchlaufen
                for (int i = 0; i < listMaennle.Count(); i++)
                {
                    // Männle count erhöhen
                    listMaennle[i].count++;
                    // Count Aktionen ausführen
                    if (listMaennle[i].count == 1)
                    {
                        listMaennle[i].setImage1();
                    }
                    if (listMaennle[i].count == (MainPage.inEndCount - 1))
                    {
                        listMaennle[i].image.Margin = new Thickness(listMaennle[i].x, listMaennle[i].y + 30, 0, 0);
                    }
                    if (listMaennle[i].count == listMaennle[i].endCount)
                    {
                        stDeleteMaennle += i.ToString() + " ";
                    }
                }
                // Nicht mehr benötigte Männla löschen
                stDeleteMaennle = stDeleteMaennle.Trim();
                if (stDeleteMaennle != "")
                {
                    string[] arDeleteMaennle = Regex.Split(stDeleteMaennle, " ");
                    for (int i = (arDeleteMaennle.Count() - 1); i >= 0; i--)
                    {
                        int tMaennle = Convert.ToInt32(arDeleteMaennle[i]);
                        if ((listMaennle[tMaennle].bad & !listMaennle[tMaennle].bonus) & listMaennle[tMaennle].hit < 3)
                        {
                            MainPage.inLives--;
                            grHit.Visibility = Windows.UI.Xaml.Visibility.Visible;
                            MainPage.PlaySound("Sounds/Hit.wav", MainPage.volumeEffects);
                        }
                        if (listMaennle[tMaennle].layer == 1)
                        {
                            grLayer1.Children.Remove(listMaennle[tMaennle].grid);
                        }
                        else if (listMaennle[tMaennle].layer == 2)
                        {
                            grLayer2.Children.Remove(listMaennle[tMaennle].grid);
                        }
                        else if (listMaennle[tMaennle].layer == 3)
                        {
                            grLayer3.Children.Remove(listMaennle[tMaennle].grid);
                        }
                        else if (listMaennle[tMaennle].layer == 4)
                        {
                            grLayer4.Children.Remove(listMaennle[tMaennle].grid);
                        }
                        grMain.Children.Remove(listMaennle[tMaennle].grid2);
                        listCoordinates[listMaennle[tMaennle].id].used = false;
                        listMaennle.RemoveAt(tMaennle);
                    }
                }



                // Wenn neues Männle erzeugt wird
                if (MainPage.inFrameCount == MainPage.inNextCount & MainPage.inLives > 0)
                {
                    // Angeben wann das nächste Männle geladen wird
                    MainPage.inNextCount += rand.Next(MainPage.inNext1, MainPage.inNext2);

                    // Prüfen welche Slots noch frei sind
                    string freeSlots = "";
                    for (int i = 0; i < listCoordinates.Count(); i++)
                    {
                        if (!listCoordinates[i].used)
                        {
                            freeSlots += i.ToString() + ";";
                        }
                    }
                    // Freien Slot aussuchen
                    string[] splitFreeSlots = Regex.Split(freeSlots, ";");
                    // Array mit freien Slots erstellen
                    string[] arFreeSlots = new string[splitFreeSlots.Count() - 1];
                    for (int i = 0; i < splitFreeSlots.Count() - 1; i++)
                    {
                        arFreeSlots[i] = splitFreeSlots[i];
                    }
                    arFreeSlots = RandomizeStrings(arFreeSlots);
                    int inFreeSlot = Convert.ToInt32(arFreeSlots[rand.Next(0, arFreeSlots.Count())]);
                    // Slot belegen
                    listCoordinates[inFreeSlot].used = true;

                    // Art des Männles zufällig aussuchen
                    int inTemp = 1;
                    int[] arAllMännla = new int[107];
                    for (int i = 0; i < 100; i++)
                    {
                        arAllMännla[i] = inTemp;
                        inTemp++;
                        if (inTemp > 10)
                        {
                            inTemp = 1;
                        }
                    }
                    int inTempSide = rand.Next(11, 14);  
                    arAllMännla[100] = inTempSide;
                    arAllMännla[101] = inTempSide;
                    arAllMännla[102] = inTempSide;
                    arAllMännla[103] = inTempSide;
                    arAllMännla[104] = inTempSide;
                    arAllMännla[105] = inTempSide;
                    arAllMännla[106] = inTempSide;
                    int maennle = arAllMännla[rand.Next(0, 107)];

                    // Festlegen ob Männle gut oder böse ist
                    bool bad = true;
                    bool bonus = false;
                    if (maennle == 11 | maennle == 12 | maennle == 13)
                    {
                        bonus = true;
                    }
                    else if (maennle < 6)
                    {
                        if (MainPage.stSide == "white")
                        {
                            bad = false;
                        }
                    }
                    else
                    {
                        if (MainPage.stSide == "black")
                        {
                            bad = false;
                        }
                    }

                    // Endcount festlegen
                    int tEndCount = rand.Next(MainPage.inEndCount, MainPage.inEndCount2);

                    // Neues Männle erzeugen
                    listMaennle.Add(new ClassMaennle(listImagesMaennle[(maennle - 1)].img0, listImagesMaennle[(maennle - 1)].img1, listImagesMaennle[(maennle - 1)].img2, listImagesMaennle[(maennle - 1)].img3, listImagesMaennle[(maennle - 1)].img4, Convert.ToInt32(inFreeSlot), listCoordinates[Convert.ToInt32(inFreeSlot)].layer, listCoordinates[Convert.ToInt32(inFreeSlot)].x, listCoordinates[Convert.ToInt32(inFreeSlot)].y, bad, bonus, tEndCount));
                    if (listMaennle[listMaennle.Count() - 1].layer == 1)
                    {
                        grLayer1.Children.Add(listMaennle[listMaennle.Count() - 1].grid);
                    }
                    else if (listMaennle[listMaennle.Count() - 1].layer == 2)
                    {
                        grLayer2.Children.Add(listMaennle[listMaennle.Count() - 1].grid);
                    }
                    else if (listMaennle[listMaennle.Count() - 1].layer == 3)
                    {
                        grLayer3.Children.Add(listMaennle[listMaennle.Count() - 1].grid);
                    }
                    else if (listMaennle[listMaennle.Count() - 1].layer == 4)
                    {
                        grLayer4.Children.Add(listMaennle[listMaennle.Count() - 1].grid);
                    }
                    grMain.Children.Add(listMaennle[listMaennle.Count() - 1].grid2);
                }


                // Wenn falsches Männle getroffen
                if (MainPage.hit)
                {
                    MainPage.hit = false;
                    grHit.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }

            }
        }
        // -------------------------------------------------------------------------------





        // Wenn auf Main Frame gedrückt wird
        // -------------------------------------------------------------------------------
        private void grMainInner_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // Wenn nicht Game Over
            if (MainPage.inLives > 0 & !MainPage.boPause)
            {
                // Angeben wann nächste Schuß möglich ist
                MainPage.inShotNext = MainPage.inFrameCount + 2;
                // Schuss abfeuern
                shot();
            }
        }
        // -------------------------------------------------------------------------------





        // Wenn auf Main Frame bewegt wird
        // -------------------------------------------------------------------------------
        private void grMainInner_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            // Wenn nicht Game Over
            if (MainPage.inLives > 0 & !MainPage.boPause)
            {
                // Nur bei bestimmten Waffen
                if (MainPage.gun == "mg")
                {
                    // Prüfen ob Schuss möglic ist
                    if (MainPage.inFrameCount >= MainPage.inShotNext)
                    {
                        // Schuß abfeuern
                        shot();
                        // Angeben das Wann nächste Schuß möglich ist
                        MainPage.inShotNext = MainPage.inFrameCount + 2;
                    }
                }
                // Bei anderen Waffen angeben das Wann nächste Schuß möglich ist
                else
                {
                    MainPage.inShotNext = MainPage.inFrameCount + 2;
                }
            }
        }
        // -------------------------------------------------------------------------------





        // Schuss ausführen
        // -------------------------------------------------------------------------------
        void shot ()
        {
            // Prufen ob Schuss bereits gewertet wurde
            if (MainPage.gun == "pistol")
            {
                // Prüfen ob noch Munition vorhanden
                if (MainPage.ammunition > 0)
                {
                    // Munition abziehen
                    MainPage.ammunition--;

                    // Schuss ausgeben
                    MainPage.PlaySound("Sounds/Gun.wav", MainPage.volumeEffects);
                }
            }
            else if (MainPage.gun == "shotgun")
            {
                // Munition abziehen
                MainPage.ammunition--;

                // Schuss ausgeben
                MainPage.PlaySound("Sounds/Shotgun.wav", MainPage.volumeEffects);

                if (MainPage.ammunition == 0)
                {
                    MainPage.ammunition = 14;
                    MainPage.gun = "pistol";
                }
            }
            // Prufen ob Schuss bereits gewertet wurde
            else if (MainPage.gun == "mg")
            {
                // Munition abziehen
                MainPage.ammunition--;

                // Schuss ausgeben
                MainPage.PlaySound("Sounds/Mg.wav", MainPage.volumeEffects);

                if (MainPage.ammunition == 0)
                {
                    MainPage.ammunition = 14;
                    MainPage.gun = "pistol";
                }
            }
        }
        // -------------------------------------------------------------------------------





        // Button Reload
        // -------------------------------------------------------------------------------
        private void grReload_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            // Wenn Waffe Pistole
            if (MainPage.gun == "pistol" & MainPage.ammunition < 14 & MainPage.inLives > 0 & !MainPage.boPause)
            {
                // Munition zurücksetzen
                MainPage.ammunition = 14;
                // Schuss ausgeben
                MainPage.PlaySound("Sounds/Gun_Reload.wav", MainPage.volumeEffects);
            }
        }
        // -------------------------------------------------------------------------------





        // Buttons Game Over
        // -------------------------------------------------------------------------------
        private void Image_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            // Alle Listen leeren
            timer.Stop();
            // Zurück zum Menü
            MainPage.frMain.GoBack();
        }

        // Button erneut spielen, Pause aufheben
        private void Image_PointerReleased_1(object sender, PointerRoutedEventArgs e)
        {
            // Wenn Pause aktiv ist
            if (MainPage.boPause)
            {
                // Game Over Grid ausblenen
                grGameOver.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                // Pause deaktivieren
                MainPage.boPause = false;
            }
            // Wenn keine Pause aktiv ist
            else
            {
                // Spiel zurücksetzen
                resetGame();
                // Game Over Grid ausblenden
                grGameOver.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                // Angeben das bereist gespielt wurde
                MainPage.boFirstGame = false;
            }
        }
        // -------------------------------------------------------------------------------


        





    }
}
