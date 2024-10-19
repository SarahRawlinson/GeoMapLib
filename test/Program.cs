using GeoMapLib;

class Program
{
    static void Main(string[] args)
    {
        string mapPath = "../../../maps/test_map.png";  // Path to your test map image
        string csvPath = "../../../csv/terrain_mapping.csv";  // Path to the terrain mapping CSV

        // Load the terrain mappings from the CSV
        PixelEnvironmentMapper.LoadTerrainMappings(csvPath);
        // Load the map
        MapLoader mapLoader = new MapLoader();
        mapLoader.LoadMap(mapPath);
        Dictionary<string, ConsoleColor> colors = new Dictionary<string, ConsoleColor>();

        // Get all possible ConsoleColor values
        Array colorValues = Enum.GetValues(typeof(ConsoleColor));
        Random random = new Random();
        foreach (var key in PixelEnvironmentMapper.GetAllTerrains())
        {
            ConsoleColor randomColor = (ConsoleColor)colorValues.GetValue(random.Next(colorValues.Length));
            if (colors.Count < colorValues.Length)
            {
                while (colors.ContainsValue(randomColor))
                {
                    randomColor = (ConsoleColor)colorValues.GetValue(random.Next(colorValues.Length));
                }
            }
            colors.Add(key.Value.Key, randomColor);
            Console.ForegroundColor = randomColor;
            MapKey terrainType = PixelEnvironmentMapper.GetTerrainType(key.Key);
            Console.WriteLine($"{terrainType.Key} = {terrainType.Name}");
        }

        // Print the terrain type at each location
        for (int x = 0; x < 50; x++)  // Assuming a 10x10 map for this example
        {
            for (int y = 0; y < 50; y++)
            {
                MapKey terrainType = mapLoader.GetTerrainAt(x, y);
                Console.ForegroundColor = colors[terrainType.Key];
                Console.Write($"{terrainType.Key} ");
            }
            Console.WriteLine();
        }
    }
}