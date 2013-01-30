using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace DSA
{
    /// <summary>
    /// A class to handle the drawing of the maps
    /// </summary>
    abstract class drawing
    {
        /// <summary>
        /// Draws the map
        /// </summary>
        /// <param name="size">The absolute size of the map</param>
        /// <param name="map">The map itself</param>
        /// <param name="path">The folder path to which the map is saved</param>
        public static void drawMap(int size, double[,] map, string path)
        {
            //Sets up the gradient used in drawing
            Gradient grad = setupGradient();

            //Creates a file path to save the map in
            string wholePath = path + "\\map" + Environment.TickCount + ".bmp";
            //Creates a map of the correct size
            Bitmap mapImage = new Bitmap(size, size);
            //Creates a graphic of the map
            Graphics g = Graphics.FromImage(mapImage);

            //Draws the map to the graphic, using each value in the map (range 0 - 1) to map to a colour
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    g.DrawRectangle(new Pen(grad.getColor(map[i,j])), i, j, 1, 1);
                }

            //Save the map image
            mapImage.Save(wholePath, ImageFormat.Bmp);
            //Dispose of the map image
            mapImage.Dispose();
        }

        /// <summary>
        /// Sets up the gradient for use by the drawing method
        /// </summary>
        /// <returns></returns>
        private static Gradient setupGradient()
        {
            //Create a new gradient
            Gradient g = new Gradient();

            //Assign it values
            g.addColor(0, Color.Black);
            g.addColor(0.01, Color.DarkBlue);
            g.addColor(0.1, Color.Blue);
            g.addColor(0.485, Color.LightBlue);
            g.addColor(0.5, Color.LightGoldenrodYellow);
            g.addColor(0.55, Color.Green);
            g.addColor(0.7, Color.ForestGreen);
            g.addColor(0.8, Color.DarkGreen);
            g.addColor(0.9, Color.DarkGray);
            g.addColor(0.95, Color.Gray);
            g.addColor(1.0, Color.White);

            //return the gradient
            return g;
        }

    }
}
