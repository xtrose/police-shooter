using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Media.Imaging;



// Namespace
namespace MyApp
{



    // Klasse, die die Bilder der Männle verwaltet
    class ClassImagesMaennle
    {



        // Variablen
        // -------------------------------------------------------------------------------
        public BitmapImage img0 { get; set; }
        public BitmapImage img1 { get; set; }
        public BitmapImage img2 { get; set; }
        public BitmapImage img3 { get; set; }
        public BitmapImage img4 { get; set; }
        // -------------------------------------------------------------------------------



        // Der Liste hinzufügen
        // -------------------------------------------------------------------------------
        public ClassImagesMaennle(int id)
        {
            img0 = new BitmapImage();
            img0.UriSource = new Uri("ms-appx:/Images/" + id.ToString() + "_0.png", UriKind.RelativeOrAbsolute);
            img1 = new BitmapImage();
            img1.UriSource = new Uri("ms-appx:/Images/" + id.ToString() + "_1.png", UriKind.RelativeOrAbsolute);
            img2 = new BitmapImage();
            img2.UriSource = new Uri("ms-appx:/Images/" + id.ToString() + "_2.png", UriKind.RelativeOrAbsolute);
            img3 = new BitmapImage();
            img3.UriSource = new Uri("ms-appx:/Images/" + id.ToString() + "_3.png", UriKind.RelativeOrAbsolute);
            img4 = new BitmapImage();
            img4.UriSource = new Uri("ms-appx:/Images/" + id.ToString() + "_4.png", UriKind.RelativeOrAbsolute);
        }
        // -------------------------------------------------------------------------------



    }
}
