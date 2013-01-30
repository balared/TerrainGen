using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace DSA
{
    /// <summary>
    /// Handles the creation and processing of basic gradients handed by relative ratios between key points
    /// Handled as if it were a Dictionary
    /// </summary>
    class Gradient
    {
        //An array of doubles and colours, each one having a correspondant in the other array
        private double[] keys;
        private Color[] cols;

        public Gradient()
        {
            //Declare a new gradient as two empty arrays
            keys = new double[0];
            cols = new Color[0];
        }

        //Adds a colour by RGB component
        public void addColor(double key, int r, int g, int b)
        {
            addColor(key, Color.FromArgb(r, g, b));
        }

        //Adds a colour by ARGB component
        public void addColor(double key, int a, int r, int g, int b)
        {
            addColor(key, Color.FromArgb(a, r, g, b));
        }

        //Adds a colour with a key
        public void addColor(double key, Color col)
        {
            //Make a new array with length equal to the previous array + 1
            double[] newKeys = new double[keys.Length + 1];
            Color[] newCols = new Color[cols.Length + 1];

            //Store whether the new value has been placed in the array or not
            Boolean placed = false;
            //For each value in the new array
            for (int i = 0; i < keys.Length + 1; i++)
            {
                //If the new key is greater than the old key in this array location...
                if (i >= keys.Length || keys[i] > key)
                {
                    //..place the old key/colour in the new array in this place
                    newKeys[i] = key;
                    newCols[i] = col;
                    placed = true;
                }
                //Else if the new key is placed...
                else if (placed)
                {
                    //..place the key/col in the new array in an offset location
                    newKeys[i] = keys[i - 1];
                    newCols[i] = cols[i - 1];
                }
                //Otherwise..
                else
                {
                    //Place the new key/col in the new array
                    newKeys[i] = keys[i];
                    newCols[i] = cols[i];
                }
            }

            //Set the key/col arrays to the new values.
            keys = newKeys;
            cols = newCols;
        }

        /// <summary>
        /// Get a colour at a specified key
        /// </summary>
        /// <param name="key">The requested key value that refers to a colour</param>
        /// <returns></returns>
        public Color getColor(double key)
        {
            //For each of the keys in the keys array...
            for (int i = 0; i < keys.Length; i++)
            {
                //If it is the requested key..
                if (keys[i] == key)
                    //Return the corrisponding colour
                    return cols[i];
                //Otherwise, if key array value is higher than the requested key, the key is not present in the array
                else if (keys[i] > key)
                {
                    //Calculate the ratio of the difference of the closest smaller key and the required key
                    double lowerRatio = (key - keys[i - 1]) / (keys[i] - keys[i - 1]);
                    //Calculate the ratio of the difference of the required key and the closest larger ket
                    double higherRatio = 1 - lowerRatio;

                    //return a colour that is part way between the nearest higher and lower colours. 
                    //The amount of influence each colour has is proportional to the ratio of the differences between them and the required key, 
                    //  allowing for a smooth gradient to a 32-bit ARGB colour resolution.
                    return Color.FromArgb(
                        (int)(cols[i - 1].R * higherRatio + cols[i].R * lowerRatio),
                        (int)(cols[i - 1].G * higherRatio + cols[i].G * lowerRatio),
                        (int)(cols[i - 1].B * higherRatio + cols[i].B * lowerRatio));
                }

            }

            //If there are no keys larger than the requested key, the colour with the largest key is returned
            return cols[cols.Length - 1];
        }
    }
}
