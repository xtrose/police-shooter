using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using System.Linq;



// Namespace
namespace MyApp
{



    // Klasse die die Männle erzeugt
    public class ClassMaennle
    {



        // Variablen
        // -------------------------------------------------------------------------------
        public BitmapImage bImage0 { get; set; }
        public BitmapImage bImage1 { get; set; }
        public BitmapImage bImage2 { get; set; }
        public BitmapImage bImage3 { get; set; }
        public BitmapImage bImage4 { get; set; }
        public int id { get; set; }
        public int layer { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public bool bad { get; set; }
        public bool bonus { get; set; }
        public int count;
        public int endCount { get; set; }
        public Grid grid = new Grid();
        public Grid grid2 = new Grid();
        public Image image = new Image();
        public Rectangle rectangle = new Rectangle();
        public int hit = 0;
        Random rand = new Random();
        // -------------------------------------------------------------------------------



        // Der Liste hinzufügen
        // -------------------------------------------------------------------------------
        public ClassMaennle(BitmapImage bImage0, BitmapImage bImage1, BitmapImage bImage2, BitmapImage bImage3, BitmapImage bImage4, int id, int layer, int x, int y, bool bad, bool bonus, int endCount)
        {
            //Variablen umwandeln
            this.id = id;
            this.layer = layer;
            this.x = x;
            this.y = y;
            this.count = 0;
            this.bad = bad;
            this.bonus = bonus;
            this.endCount = endCount;

            // Grid erstellen
            grid = new Grid();
            grid.Margin = new Thickness(x, y, 0, 0);
            grid.VerticalAlignment = VerticalAlignment.Top;
            grid.HorizontalAlignment = HorizontalAlignment.Left;

            // Grid erstellen
            grid2 = new Grid();
            grid2.Margin = new Thickness(x, y, 0, 0);
            grid2.VerticalAlignment = VerticalAlignment.Top;
            grid2.HorizontalAlignment = HorizontalAlignment.Left;

            // Bild erstellen
            this.bImage0 = bImage0;
            this.bImage1 = bImage1;
            this.bImage2 = bImage2;
            this.bImage3 = bImage3;
            this.bImage4 = bImage4;
            image = new Image();
            image.Source = bImage0;

            // Rechteck einstellen
            rectangle = new Rectangle();
            rectangle.VerticalAlignment = VerticalAlignment.Bottom;
            rectangle.HorizontalAlignment = HorizontalAlignment.Center;
            rectangle.Fill = new SolidColorBrush(Colors.Red);
            rectangle.Opacity = 0.0;
            rectangle.PointerPressed += rectangle_PointerPressed;
            rectangle.PointerMoved += rectangle_PointerMoved;
            rectangle.PointerEntered += rectangle_PointerMoved;

            // Größen einstellen
            if (layer == 1)
            {
                grid.Width = 70;
                grid.Height = 70;
                grid2.Width = 70;
                grid2.Height = 70;
                image.Width = 70;
                image.Height = 70;
                rectangle.Height = 60;
                rectangle.Width = 30;
            }
            else if (layer == 2)
            {
                grid.Width = 80;
                grid.Height = 80;
                grid2.Width = 80;
                grid2.Height = 80;
                image.Width = 80;
                image.Height = 80;
                rectangle.Height = 70;
                rectangle.Width = 40;
            }
            else if (layer == 3)
            {
                grid.Width = 90;
                grid.Height = 90;
                grid2.Width = 90;
                grid2.Height = 90;
                image.Width = 90;
                image.Height = 90;
                rectangle.Height = 80;
                rectangle.Width = 50;
            }
            else if (layer == 4)
            {
                grid.Width = 100;
                grid.Height = 100;
                grid2.Width = 100;
                grid2.Height = 100;
                image.Width = 100;
                image.Height = 100;
                rectangle.Height = 90;
                rectangle.Width = 60;
            }

            // Teile zusammenfügen
            grid.Children.Add(image);
            grid2.Children.Add(rectangle);
            
        }
        // -------------------------------------------------------------------------------



        // Das zweite Bild erstellen
        // -------------------------------------------------------------------------------
        public void setImage1()
        {
            image.Source = bImage1;
        }
        // -------------------------------------------------------------------------------





        // Wenn Pointer gedrückt wird
        // -------------------------------------------------------------------------------
        void rectangle_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            // Wenn nicht Game Over
            if (MainPage.inLives > 0 & !MainPage.boPause)
            {
                // Angeben das Wann nächste Schuß möglich ist
                MainPage.inShotNext = MainPage.inFrameCount + 2;
                // Schuss abfeuern
                shot();
            }
        }
        // -------------------------------------------------------------------------------





        // Wenn Pointer bewegt wird
        // -------------------------------------------------------------------------------
        private void rectangle_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            // Prüfen ob Maus gedrückt ist
            bool mousePressed = false;
            Windows.UI.Xaml.Input.Pointer ptr = e.Pointer;
            Windows.UI.Input.PointerPoint ptrPt = e.GetCurrentPoint(rectangle);
            if (ptrPt.Properties.IsLeftButtonPressed)
            {
                mousePressed = true;
            }


            // Wenn nicht Game Over
            if (MainPage.inLives > 0 & !MainPage.boPause)
            {
                // Nur bei bestimmten Waffen
                if (MainPage.gun == "mg" & mousePressed)
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





        // Schuß abfeuern
        // -------------------------------------------------------------------------------
        void shot ()
        {
            // Bei normalen Männla
            if (!bonus)
            {
                // Bei Pistole
                if (MainPage.gun == "pistol")
                {
                    // Prüfen ob noch Munition vorhanden
                    if (MainPage.ammunition > 0)
                    {
                        // Munition abziehen
                        MainPage.ammunition--;
                        // Schaden abziehen
                        if (hit == 0)
                        {
                            image.Source = bImage2;
                            if (!bad)
                            {
                                MainPage.inLives--;
                                MainPage.hit = true;
                                MainPage.PlaySound("Sounds/Hit.wav", MainPage.volumeEffects);
                            }
                            else
                            {
                                MainPage.inPoints++;
                            }
                        }
                        else if (hit == 1)
                        {
                            image.Source = bImage3;
                            if (!bad)
                            {
                                MainPage.inLives--;
                                MainPage.hit = true;
                                MainPage.PlaySound("Sounds/Hit.wav");
                            }
                            else
                            {
                                MainPage.inPoints += 2;
                            }
                        }
                        else if (hit == 2)
                        {
                            image.Source = bImage4;
                            if (!bad)
                            {
                                MainPage.inLives--;
                                MainPage.hit = true;
                                MainPage.PlaySound("Sounds/Hit.wav", MainPage.volumeEffects);
                            }
                            else
                            {
                                MainPage.inPoints += 3;
                            }
                        }
                        hit++;
                        // Sound ausgeben
                        MainPage.PlaySound("Sounds/Gun.wav", MainPage.volumeEffects);
                    }
                }

                // Bei Shotgun
                else if (MainPage.gun == "shotgun")
                {
                    // Munition abziehen
                    MainPage.ammunition--;
                    // Schaden abziehen
                    if (hit == 0)
                    {
                        image.Source = bImage4;
                        if (!bad)
                        {
                            MainPage.inLives--;
                            MainPage.hit = true;
                            MainPage.PlaySound("Sounds/Hit.wav", MainPage.volumeEffects);
                        }
                        else
                        {
                            MainPage.inPoints += 8;
                        }
                    }
                    else if (hit == 1)
                    {
                        image.Source = bImage4;
                        if (!bad)
                        {
                            MainPage.inLives--;
                            MainPage.hit = true;
                            MainPage.PlaySound("Sounds/Hit.wav", MainPage.volumeEffects);
                        }
                        else
                        {
                            MainPage.inPoints += 8;
                        }
                    }
                    else if (hit == 2)
                    {
                        image.Source = bImage4;
                        if (!bad)
                        {
                            MainPage.inLives--;
                            MainPage.hit = true;
                            MainPage.PlaySound("Sounds/Hit.wav", MainPage.volumeEffects);
                        }
                        else
                        {
                            MainPage.inPoints += 8;
                        }
                    }
                    hit = 3;
                    // Sound ausgeben
                    MainPage.PlaySound("Sounds/Shotgun.wav", MainPage.volumeEffects);

                    if (MainPage.ammunition == 0)
                    {
                        MainPage.ammunition = 12;
                        MainPage.gun = "pistol";
                    }
                }

                // Bei MG
                else if (MainPage.gun == "mg")
                {
                    // Prüfen ob noch Munition vorhanden
                    if (MainPage.ammunition > 0)
                    {
                        // Munition abziehen
                        MainPage.ammunition--;
                        // Schaden abziehen
                        if (hit == 0)
                        {
                            image.Source = bImage2;
                            if (!bad)
                            {
                                MainPage.inLives--;
                                MainPage.hit = true;
                                MainPage.PlaySound("Sounds/Hit.wav", MainPage.volumeEffects);
                            }
                            else
                            {
                                MainPage.inPoints++;
                            }
                        }
                        else if (hit == 1)
                        {
                            image.Source = bImage3;
                            if (!bad)
                            {
                                MainPage.inLives--;
                                MainPage.hit = true;
                                MainPage.PlaySound("Sounds/Hit.wav", MainPage.volumeEffects);
                            }
                            else
                            {
                                MainPage.inPoints += 2;
                            }
                        }
                        else if (hit == 2)
                        {
                            image.Source = bImage4;
                            if (!bad)
                            {
                                MainPage.inLives--;
                                MainPage.hit = true;
                                MainPage.PlaySound("Sounds/Hit.wav", MainPage.volumeEffects);
                            }
                            else
                            {
                                MainPage.inPoints += 3;
                            }
                        }
                        hit++;
                        // Sound ausgeben
                        MainPage.PlaySound("Sounds/Mg.wav", MainPage.volumeEffects);

                        if (MainPage.ammunition == 0)
                        {
                            MainPage.ammunition = 12;
                            MainPage.gun = "pistol";
                        }
                    }
                }

                // Männle löschen, wenn zerstört ist
                if (hit >= 3 & count < (endCount - 3))
                {
                    count = (endCount - 3);
                }
            }


            // Bei Bonus Mänle
            else
            {
                // Bei Pistole
                if (MainPage.gun == "pistol")
                {
                    // Prüfen ob noch Munition vorhanden
                    if (MainPage.ammunition > 0)
                    {
                        // Munition abziehen
                        MainPage.ammunition--;
                        // Schaden abziehen
                        if (hit == 0)
                        {
                            MainPage.inPoints++;
                        }
                        else if (hit == 1 | hit == 2)
                        {
                            image.Source = bImage2;
                            MainPage.inPoints += 2;
                        }
                        else if (hit == 3 | hit == 4)
                        {
                            image.Source = bImage3;
                            MainPage.inPoints += 3;
                        }
                        else if (hit == 5)
                        {
                            image.Source = bImage4;
                            MainPage.inPoints += 3;
                            createBonus();
                        }
                        hit++;
                        // Sound ausgeben
                        MainPage.PlaySound("Sounds/Gun.wav", MainPage.volumeEffects);
                    }
                }

                // Bei Shotgun
                else if (MainPage.gun == "shotgun")
                {
                    // Munition abziehen
                    MainPage.ammunition--;
                    // Schaden abziehen
                    if (hit == 0 | hit == 1)
                    {
                        image.Source = bImage2;
                        MainPage.inPoints += 2;
                    }
                    else if (hit == 2 | hit == 3)
                    {
                        image.Source = bImage3;
                        MainPage.inPoints += 4;
                    }
                    else if (hit >= 4 & hit <= 10)
                    {
                        image.Source = bImage4;
                        MainPage.inPoints += 6;
                        createBonus();
                        hit = 11;
                    }
                    hit += 2;
                    // Sound ausgeben
                    MainPage.PlaySound("Sounds/Shotgun.wav", MainPage.volumeEffects);

                    if (MainPage.ammunition == 0)
                    {
                        MainPage.ammunition = 12;
                        MainPage.gun = "pistol";
                    }
                }

                // Bei MG
                else if (MainPage.gun == "mg")
                {
                    // Prüfen ob noch Munition vorhanden
                    if (MainPage.ammunition > 0)
                    {
                        // Munition abziehen
                        MainPage.ammunition--;
                        // Schaden abziehen
                        if (hit == 0)
                        {
                            MainPage.inPoints++;
                        }
                        else if (hit == 1 | hit == 2)
                        {
                            image.Source = bImage2;
                            MainPage.inPoints += 2;
                        }
                        else if (hit == 3 | hit == 4)
                        {
                            image.Source = bImage3;
                            MainPage.inPoints += 3;
                        }
                        else if (hit == 5)
                        {
                            image.Source = bImage4;
                            MainPage.inPoints += 3;
                            createBonus();
                        }
                        hit++;
                        // Sound ausgeben
                        MainPage.PlaySound("Sounds/Mg.wav", MainPage.volumeEffects);

                        if (MainPage.ammunition == 0)
                        {
                            MainPage.ammunition = 12;
                            MainPage.gun = "pistol";
                        }
                    }
                }

                // Männle löschen, wenn zerstört ist
                if (hit >= 6 & count < (endCount - 3))
                {
                    count = (endCount - 3);
                }
            }
        }
        // -------------------------------------------------------------------------------





        // Bonus erstellen
        // -------------------------------------------------------------------------------
        void createBonus ()
        {
            // Bonuse erstellen
            string[] arBonuses = new string[] { "live", "shotgun", "50Shots", "mg" };
            string[] arBonusesSelect = new string[(arBonuses.Count() * 10)];
            int inTemp = 0;

            for (int i = 0; i <  arBonusesSelect.Count(); i++)
            {
                arBonusesSelect[i] = arBonuses[inTemp];
                inTemp++;
                if (inTemp == arBonuses.Count())
                {
                    inTemp = 0;
                }
            }

            // Bonuse auswählen
            string stBonus = arBonusesSelect[rand.Next(0, arBonusesSelect.Count())];

            // Bonus erstellen
            if (stBonus == "live")
            {
                MainPage.inLives++;
                MainPage.PlaySound("Sounds/SpeechExtraLive.wav", MainPage.volumeEffects);
            }
            else if (stBonus == "shotgun")
            {
                if (MainPage.gun != "shotgun")
                {
                    MainPage.gun = "shotgun";
                    MainPage.ammunition = 16;
                }
                else
                {
                    if (MainPage.ammunition < 16)
                    {
                        MainPage.ammunition = 16;
                    }
                }
                MainPage.PlaySound("Sounds/SpeechShotgun.wav", MainPage.volumeEffects);
            }
            else if (stBonus == "50Shots")
            {
                if (MainPage.gun == "shotgun")
                {
                    MainPage.ammunition += 16;
                }
                else if (MainPage.gun == "mg")
                {
                    MainPage.ammunition += 100;
                }
                else
                {
                    MainPage.ammunition += 14;
                }
                MainPage.PlaySound("Sounds/SpeechExtraAmmo.wav", MainPage.volumeEffects);
            }
            else if (stBonus == "mg")
            {
                if (MainPage.gun != "mg")
                {
                    MainPage.gun = "mg";
                    MainPage.ammunition = 100;
                }
                else
                {
                    if (MainPage.ammunition < 100)
                    {
                        MainPage.ammunition = 100;
                    }
                }
                MainPage.PlaySound("Sounds/SpeechMachineGun.wav", MainPage.volumeEffects);
            }
        }
        // -------------------------------------------------------------------------------


        

    }



}
