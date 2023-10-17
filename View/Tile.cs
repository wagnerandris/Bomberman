using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View
{
    // Class to obscure constructing Panel with the right parameters
    internal class Tile : Panel
    {
        public static int TileWidth { get; set; }
        public static int TileHeight { get; set; }

        public Tile(Bitmap image) : base()
        {
            BackgroundImage = image;
            BackgroundImageLayout = ImageLayout.Stretch;
            Height = TileHeight;
            Width = TileWidth;
            Margin = new Padding(0);
        }
    }
}
