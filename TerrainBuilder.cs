using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSA
{
    /// <summary>
    /// Builds a complex terrain map from non-complex DSA tiles that have shared boundries
    /// </summary>
    abstract class terrainBuilder
    {
        //A random generator
        private static Random r = new Random();

        /// <summary>
        /// Builds the terrain of a specified segment size and map width in segments
        /// </summary>
        /// <param name="segmentSize">The width/length of each segment in pixels</param>
        /// <param name="mapWidth">The width/length of the map in segments</param>
        /// <returns>A completed heightmap</returns>
        public static double[,] buildTerrain(int segmentSize, int mapWidth)
        {
            //Calculate the absolute width/length of each segment
            int segmentWidth = (int)Math.Pow(2, segmentSize) + 1;
            //Creates an array that represents the entire terrain. This is not memory-efficient, and a solution involving the use of main storage is being developed.
            double[,] terrain = new double[segmentWidth * mapWidth, segmentWidth * mapWidth];
            //Creates a boolean array to represent the segments of the map that have already been generated.
            bool[,] filledSeg = new bool[mapWidth, mapWidth];

            //Populate the generation record with false
            for (int i = 0; i < mapWidth; i++)
                for (int j = 0; j < mapWidth; j++)
                    filledSeg[i, j] = false;

            //For each segment that needs generating...
            for(int i = 0; i < mapWidth; i++)
                for (int j = 0; j < mapWidth; j++)
                {
                    //Set up the segment to share edges with any adjacent generated segments
                    double[,] mapSeg = setUpSeg(terrain, filledSeg, i, j, segmentWidth, mapWidth);
                    //Generate the map
                    DiamondSquareAlgorithm.generateDSAMap(segmentSize, mapSeg);

                    //Insert the data into the map
                    for(int x = 0; x < segmentWidth; x++)
                        for (int y = 0; y < segmentWidth; y++)
                        {
                            terrain[i * segmentWidth + x, j * segmentWidth + y] = mapSeg[x, y];
                        }

                    //Set the generation record for the segment to true
                    filledSeg[i, j] = true;

                }

            //After the segments have been generated, normalise the terrain
            normalize(terrain, segmentWidth * mapWidth);

            //Return the terrain
            return terrain;
        }

        /// <summary>
        /// Sets up the segment by transfering the very edge of any generated adjacent segments to the relevant edge of the current seg. Any unassigned corners are randomised.
        /// </summary>
        /// <param name="terrain">The terrain as it currently stands</param>
        /// <param name="filledMap">The generation record</param>
        /// <param name="x">The horizontal segment location in the terrain</param>
        /// <param name="y">The vertical segment location in the terrain</param>
        /// <param name="segmentWidth">The width of a segment in pixels</param>
        /// <param name="mapWidth">The width of the map in segments</param>
        /// <returns>A segment that shares borders with previously generated adjancent segments</returns>
        private static double[,] setUpSeg(double[,] terrain, bool[,] filledMap, int x, int y, int segmentWidth, int mapWidth)
        {
            //Create a new segment of the correct size
            double[,] seg = new double[segmentWidth, segmentWidth];

            //If the segment is not on the left hand edge, and the segment to the left has been generated...
            if (x > 0 && filledMap[x - 1, y])
                //Copy the right hand edge of that segment to the left hand edge of the current segment
                for (int i = 0; i < segmentWidth; i++)
                    if (seg[0, i] == 0.0)
                        seg[0, i] = terrain[x * segmentWidth - 1, y * segmentWidth + i];

            //If the segment is not on the top edge, and the segment above has been generated...
            if (y > 0 && filledMap[x, y - 1])
                //Copy the top edge of that segment to the bottom edge of the current segment
                for (int i = 0; i < segmentWidth; i++)
                    if (seg[i, 0] == 0.0)
                        seg[i, 0] = terrain[x * segmentWidth + i, y * segmentWidth - 1];

            //If the segment is not on the right hand edge, and the segment to the right has been generated...
            if (x < mapWidth - 1 && filledMap[x + 1, y])
                //Copy the left hand edge of that segment to the right hand edge of the current segment
                for (int i = 0; i < segmentWidth; i++)
                    if (seg[segmentWidth - 1, i] == 0.0)
                        seg[segmentWidth - 1, i] = terrain[(x + 1) * segmentWidth, y * segmentWidth + i];

            //If the segment is not on the bottom edge, and the segment below has been generated...
            if (y < mapWidth - 1 && filledMap[x, y + 1])
                //Copy the bottom edge of that segment to the top edge of the current segment
                for (int i = 0; i < segmentWidth; i++)
                    if (seg[i, segmentWidth - 1] == 0.0)
                        seg[i, segmentWidth - 1] = terrain[x * segmentWidth + i, (y + 1) * segmentWidth];

            //If any of the corners are left undefined, assign them as a random double
            if (seg[0, 0] == 0)
                seg[0, 0] = r.NextDouble();
            if (seg[0, segmentWidth - 1] == 0)
                seg[0, segmentWidth - 1] = r.NextDouble();
            if (seg[segmentWidth - 1, 0] == 0)
                seg[segmentWidth - 1, 0] = r.NextDouble();
            if (seg[segmentWidth - 1, segmentWidth - 1] == 0)
                seg[segmentWidth - 1, segmentWidth - 1] = r.NextDouble();

            //Return the setup segment
            return seg;
        }

        private static void normalize(double[,] map, int size)
        {
            // Default the highest and lowest value. Make a diff
            double highest = Double.MinValue;
            double lowest = Double.MaxValue;
            double diff;

            // Find the highest and lowest values
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
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
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    map[i, j] -= lowest;
                    map[i, j] /= diff;
                }
        }
    }
}
