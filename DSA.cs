using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSA
{   
    /// <summary>
    /// The Diamond Square algorithm, generating a grid based on a size given, with the option to base it off of a map
    /// It will only create data for a cell in the array if the cell in question has a value of 0.0d
    /// Given the range of potential values for a double and the nature of the algorithm, it has been determined that it is unlikely this will ever become an issue.
    /// </summary>
    abstract class DiamondSquareAlgorithm
    {
        //A Random Generator
        private static Random r = new Random();

        /// <summary>
        /// Controller for generating a completely random DSA Map
        /// </summary>
        /// <param name="size">The size of the map, were mapWidth = 2^size + 1, ie 1 = 2, 2 = 5, 3 = 9, 4 = 17...</param>
        /// <returns></returns>
        public static double[,] generateDSAMap(int size)
        {
            //Calculate real map width
            int width = (int)(Math.Pow(2, size) + 1);

            //Declare a new map with the correct widths
            double[,] map = new double[width, width];

            //Define the corners of the map as a random value
            map[0, 0] = r.NextDouble();
            map[0, width - 1] = r.NextDouble();
            map[width - 1, 0] = r.NextDouble();
            map[width - 1, width - 1] = r.NextDouble();

            //Perform the algorithm on the map
            performDSA(map, size);

            //Return the completed map
            return map;
        }

        /// <summary>
        /// Controller for generating a completely random, normalised DSA Map
        /// </summary>
        /// <param name="size">The size of the map, were mapWidth = 2^size + 1, ie 1 = 2, 2 = 5, 3 = 9, 4 = 17...</param>
        /// <returns></returns>
        public static double[,] generateNDSAMap(int size)
        {
            //Generate the non-normalised map
            double[,] completedMap = generateDSAMap(size);

            //Normalise the map
            normalize(completedMap, size);

            //Return the normalised map
            return completedMap;
        }

        /// <summary>
        /// Controller for generating a DSA Map based on incomplete data. The corners of the map must be seeded already, but all other data is optional
        /// </summary>
        /// <param name="size">The size of the map, were mapWidth = 2^size + 1, ie 1 = 2, 2 = 5, 3 = 9, 4 = 17...</param>
        /// <param name="map">The incomplete map upon which the completed map is based</param>
        /// <returns></returns>
        public static double[,] generateDSAMap(int size, double[,] map)
        {
            //Calculate real map width
            int width = (int)(Math.Pow(2, size) + 1);

            //Perform the algorithm on the map
            performDSA(map, size);

            //Return the completed map
            return map;
        }

        /// <summary>
        /// Controller for generating a completely random, normalised DSA Map
        /// </summary>
        /// <param name="size">The size of the map, were mapWidth = 2^size + 1, ie 1 = 2, 2 = 5, 3 = 9, 4 = 17...</param>
        /// <returns></returns>
        public static double[,] generateNDSAMap(int size, double[,] map)
        {
            //Generate the non-normalised map
            double[,] completedMap = generateDSAMap(size, map);

            //Normalise the map
            normalize(map, size);

            //Return the normalised map
            return map;
        }

        /// <summary>
        /// Perform the diamond square algorithm on the incomplete map
        /// </summary>
        /// <param name="map">The incomplete map upon which the completed map is based</param>
        /// <param name="size">The size of the map, were mapWidth = 2^size + 1, ie 1 = 2, 2 = 5, 3 = 9, 4 = 17...</param>
        private static void performDSA(double[,] map, int size)
        {
            //For each value in ceiling[log2(mapWidth)]...
            for (int i = 0; i < size; i++)
            {
                //..Perform the square step
                square(map, size, i);
                //Perform the diamond step
                diamond(map, size, i);
            }
        }

        /// <summary>
        /// The diamond step of the Diamond Square Algorithm.
        /// This selects the cell above, below, left and right of the cell it's generating, ignoring them if they're out of bounds.
        /// The algorithm is designed such that each of relevant cells around the generating cell have been previously generated.
        /// 
        ///|-------OFFSET---------|
        ///|-----------------------WIDTH---------------------|_
        ///[G][ ][ ][ ][ ][ ][ ][ ][X][ ][ ][ ][ ][ ][ ][ ][G]|
        ///[ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]|O
        ///[ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]|F
        ///[ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]|F
        ///[ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]|S
        ///[ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]|E
        ///[ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]|T
        ///[ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]_
        ///[X][ ][ ][ ][ ][ ][ ][ ][O][ ][ ][ ][ ][ ][ ][ ][X]
        ///[ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]
        ///[ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]
        ///[ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]
        ///[ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]
        ///[ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]
        ///[ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]
        ///[ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]
        ///[G][ ][ ][ ][ ][ ][ ][ ][X][ ][ ][ ][ ][ ][ ][ ][G]
        ///
        /// O is generated in the first iteration of the Square algorithm
        /// X is generated in the first iteration of the diamond algorithm
        /// </summary>
        /// <param name="map">The map at its current stage of generation</param>
        /// <param name="size">The size of the map, were mapWidth = 2^size + 1, ie 1 = 2, 2 = 5, 3 = 9, 4 = 17...</param>
        /// <param name="depth">How many iterations the program has operated for</param>
        private static void diamond(double[,] map, int size, int depth)
        {
            //Calculate the distance from the edge of the map to 
            int offset = (int)Math.Pow(2, size - depth - 1);
            //Calculate the actual width of the map
            int width = (int)Math.Pow(2, size) + 1;

            //For each of the cells that needs to be generated during this iteration of the algorithm...
            for (int y = 0; y < width; y += offset)
                for (int x = (y % offset == 0 ? 0 : (int)offset); x < width; x += offset)
                {
                    //..if it hasn't been assigned...
                    if (map[x, y] == 0)
                        //..Assign it as the average of the surrounding cells, plus a random value that is proportional to the number of iterations of the algorithm that've already been applied
                        //If the surrounding cell is out of bounds, the parameter is set to the maximum value of a Double
                        map[x, y] = averageWithRand(
                            x >= offset ? map[x - offset, y] : Double.MaxValue,
                            x < width - offset ? map[x + offset, y] : Double.MaxValue,
                            y >= offset ? map[x, y - offset] : Double.MaxValue,
                            y < width - offset ? map[x, y + offset] : Double.MaxValue,
                            depth + 1);
                }
        }

        /// <summary>
        /// The Square step of the Diamond Square Algorithm.
        /// This selects the cell above, below, left and right of the cell it's generating, ignoring them if they're out of bounds.
        /// The algorithm is designed such that each of relevant cells around the generating cell have been previously generated.
        /// 
        ///|-------OFFSET---------|
        ///|-----------------------WIDTH---------------------|_
        ///[G][ ][ ][ ][ ][ ][ ][ ][X][ ][ ][ ][ ][ ][ ][ ][G]|
        ///[ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]|O
        ///[ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]|F
        ///[ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]|F
        ///[ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]|S
        ///[ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]|E
        ///[ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]|T
        ///[ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]_
        ///[X][ ][ ][ ][ ][ ][ ][ ][O][ ][ ][ ][ ][ ][ ][ ][X]
        ///[ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]
        ///[ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]
        ///[ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]
        ///[ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]
        ///[ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]
        ///[ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]
        ///[ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]
        ///[G][ ][ ][ ][ ][ ][ ][ ][X][ ][ ][ ][ ][ ][ ][ ][G]
        ///
        /// O is generated in the first iteration of the Square algorithm
        /// X is generated in the first iteration of the diamond algorithm
        /// </summary>
        /// <param name="map">The map at its current stage of generation</param>
        /// <param name="size">The size of the map, were mapWidth = 2^size + 1, ie 1 = 2, 2 = 5, 3 = 9, 4 = 17...</param>
        /// <param name="depth">How many iterations the program has operated for</param>
        private static void square(double[,] map, int size, int depth)
        {
            //Calculate the distance from the edge of the map to 
            int offset = (int)Math.Pow(2, size - depth - 1);
            //Calculate the actual width of the map
            int width = (int)Math.Pow(2, size) + 1;

            //For each of the cells that needs to be generated during this iteration of the algorithm...
            for (int y = offset; y < width; y += offset * 2)
                for (int x = offset; x < width; x += offset * 2)
                {
                    //..if it hasn't been assigned...
                    if (map[x, y] == 0)
                        map[x, y] = averageWithRand(
                            //..Assign it as the average of the surrounding cells, plus a random value that is proportional to the number of iterations of the algorithm that've already been applied
                            //If the surrounding cell is out of bounds, the parameter is set to the maximum value of a Double
                            (x >= offset && y >= offset) ? map[x - offset, y - offset] : Double.MaxValue,
                            (x < width - offset && y >= offset) ? map[x + offset, y - offset] : Double.MaxValue,
                            (x >= offset && y < width - offset) ? map[x - offset, y + offset] : Double.MaxValue,
                            (x < width - offset && y < width - offset) ? map[x + offset, y + offset] : Double.MaxValue,
                            depth);
                }
        }

        /// <summary>
        /// Averages four doubles given to it, with validation as to whether all of them should be used, and applies a random value that is proportionate to a Random Depreciation Magnitude
        /// </summary>
        /// <param name="a">The first value to average</param>
        /// <param name="b">The second value to average</param>
        /// <param name="c">The third value to average</param>
        /// <param name="d">The fourth value to average</param>
        /// <param name="randDeprMag">The number of times the random component is halved before being applied to the random value. Determined by the number of iterations of the DSA applied so far.</param>
        /// <returns></returns>
        private static double averageWithRand(double a, double b, double c, double d, int randDeprMag)
        {
            //set the number of valid entries and the average as zero
            int valids = 0;
            double ret = 0;

            //For each of the values, if it's not the maximum possible value for a double, increment the number of valid values given, and add it to the return value
            if (a != Double.MaxValue)
            {
                valids++;
                ret += a;
            }
            if (b != Double.MaxValue)
            {
                valids++;
                ret += b;
            }
            if (c != Double.MaxValue)
            {
                valids++;
                ret += c;
            }
            if (d != Double.MaxValue)
            {
                valids++;
                ret += d;
            }

            //Divide the return value by the number of valid values given
            //This provides an average value for all the valid parameters given
            ret /= valids;

            //Add a random double between -0.5d and 0.4999..d, divided by 2 for each iteration of the algorithm applied so far  
            ret += (r.NextDouble() - 0.5d) / Math.Pow(2, randDeprMag);

            //return the averaged/randomed value
            return ret;
        }

        /// <summary>
        /// Normalises the map such that all values retain their relative value, but the absolute values lies in the range [0 - 1]
        /// </summary>
        /// <param name="map"></param>
        /// <param name="size"></param>
        private static void normalize(double[,] map, int size)
        {
            int width = (int)(Math.Pow(2, size) + 1);

            // Default the highest and lowest value. Make a diff
            double highest = Double.MinValue;
            double lowest = Double.MaxValue;
            double diff;

            // Find the highest and lowest values
            for (int i = 0; i < width; i++)
                for (int j = 0; j < width; j++)
                {
                    if (highest < map[i, j])
                        highest = map[i, j];
                    if (lowest > map[i, j])
                        lowest = map[i, j];
                }

            // Calculate the difference between highest and lowest
            diff = highest - lowest;

            // Subtract the lowest value from each value (defaulting range to 0.0F+)
            // Divide each range by the difference between the two (defaulting range
            // to 0.0F -> 1.0F)
            for (int i = 0; i < width; i++)
                for (int j = 0; j < width; j++)
                {
                    map[i, j] -= lowest;
                    map[i, j] /= diff;
                }
        }
    }
}
