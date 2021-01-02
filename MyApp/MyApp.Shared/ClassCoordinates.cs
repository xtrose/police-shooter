using System;
using System.Collections.Generic;
using System.Text;



// Namespace
namespace MyApp
{



    // Klasse der Koordinaten
    class ClassCoordinates
    {



        // Variablen
        // -------------------------------------------------------------------------------
        public int id { get; set; }
        public int layer { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public bool used;
        // -------------------------------------------------------------------------------



        // Der Liste hinzufügen
        // -------------------------------------------------------------------------------
        public ClassCoordinates(int id, int layer, int x, int y)
        {
            this.id = id;
            this.layer = layer;
            this.x = x;
            this.y = y;
            used = false;
        }
        // -------------------------------------------------------------------------------



    }
}
