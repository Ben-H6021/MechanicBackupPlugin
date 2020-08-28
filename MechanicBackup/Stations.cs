using Rage;
namespace MechanicBackup
{
    class Stations
    {

        public static Vector3[] spawnLocations =
        {
            new Vector3(552.0185f, -56.24221f, 71.04436f),
            new Vector3(826.7781f, -1264.895f, 26.19597f),
            new Vector3(-577.7094f, -145.3423f, 37.34693f),
            new Vector3(-1055.879f, -859.0846f, 4.815023f),
            new Vector3(483.0124f, -1021.343f, 27.85753f),
            //new Vector3(397.8063f, -1613.463f, 29.22214f), replacement: 
            new Vector3(405.238f, -1607.4f, 29.18381f),
            new Vector3(1870.137f, 3691.292f, 33.57906f),
            new Vector3(-454.858f, 6026.713f, 31.27069f),
            new Vector3(373.6319f, 795.257f, 187.3156f),
            new Vector3(-910.1017f, -2400.782f, 13.8593f),
            new Vector3(1877.111f, 2600.95f, 45.60293f),
            new Vector3(-693.9974f, -1408.864f, 4.931546f),
            new Vector3(-1622.916f, -3145.871f, 13.92284f),
            new Vector3(-331.6401f, -2764.188f, 4.932288f)
        };

        public static float[] spawnHeadings =
        {
            225.4077f,
            88.04159f,
            202.2667f,
            138.2651f,
            276.9456f,
            //319.4296f, replacement:
            229.0123f,
            208.8443f,
            312.7682f,
            180.6773f,
            147.7543f,
            269.7515f,
            138.9901f,
            325.3116f,
            88.94954f
        };

        public static int getNearestStationIndex(Vector3 playerPos)
        {
            float lastDistance = 1000000000000;
            int bestSpawnIndex = -1;
            for (int i = 0; i < spawnLocations.Length; i++)
            {
                Vector3 spawnLocation = spawnLocations[i];
                if (playerPos.DistanceTo(spawnLocation) < lastDistance)
                {
                    lastDistance = playerPos.DistanceTo(spawnLocation);
                    bestSpawnIndex = i;
                }
            }

            return bestSpawnIndex;
        }


    }
}
