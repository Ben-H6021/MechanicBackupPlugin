using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechanicBackup
{
    class Workshop
    {

        static Vector3[] spawnLocations =
        {
            new Vector3(1997.856f, 3774.236f, 31.76904f),
            new Vector3(2525.661f, 4209.972f, 39.54697f),
            new Vector3(-87.42248f, 6431.153f, 31.03912f),
            new Vector3(2131.261f, 4795.306f, 40.72886f),
            new Vector3(-229.3102f, 6243.297f, 31.07885f),
            new Vector3(250.6418f, 2599.719f, 44.54448f),
            new Vector3(-35.39799f, -1079.063f, 26.25356f),
            new Vector3(424.9336f, -1019.86f, 28.57421f),
            new Vector3(635.5191f, 258.6078f, 102.6733f),
            new Vector3(2533.16f, 2610.943f, 37.53647f),
            new Vector3(-194.2721f, -1390.372f, 30.65935f),
            new Vector3(856.2881f, -2129.305f, 30.1485f),
            new Vector3(487.0076f, -1883.452f, 25.68256f),
            new Vector3(860.673f, -1068.036f, 28.11518f),
            new Vector3(-1393.298f, -337.4848f, 39.70993f),
            new Vector3(-466.4483f, -2169.624f, 9.563994f),
            new Vector3(-1124.008f, -2000.99f, 12.76198f),
            new Vector3(-374.337f, -108.8118f, 38.2759f),
            new Vector3(60.45909f, 2778.823f, 57.46626f),
            new Vector3(267.743f, -1828.577f, 26.40591f),
            new Vector3(-335.8957f, -1473.12f, 30.1278f),
            new Vector3(9.259551f, -1413.284f, 28.92362f),
            new Vector3(-48.03771f, -1673.715f, 28.92811f),
            new Vector3(526.6412f, -171.3772f, 54.67404f)
        };

        static float[] spawnHeadings =
        {
            121.7595f,
            328.124f,
            43.20144f,
            23.76526f,
            132.7134f,
            102.5733f,
            71.95773f,
            89.68717f,
            152.4313f,
            276.5262f,
            213.1187f,
            268.4644f,
            300.5139f,
            180.6591f,
            121.7012f,
            19.04376f,
            224.3832f,
            74.22604f,
            143.751f,
            232.0235f,
            0.7358139f,
            90.44456f,
            50.54033f,
            35.14077f
        };

        public static int getNearestWorkshop(Vector3 playerPos)
        {
            float lastDistance = 1000000000000;
            int bestSpawnIndex = -1;
            for (int i = 0; i < spawnLocations.Length; i++)
            {
                Vector3 spawnLocation = spawnLocations[i];
                if (playerPos.DistanceTo(spawnLocation) < lastDistance) {
                    lastDistance = playerPos.DistanceTo(spawnLocation);
                    bestSpawnIndex = i;
                }
            }

            return bestSpawnIndex;
        }


    }
}
