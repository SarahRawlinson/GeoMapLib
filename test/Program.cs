using GeoMapLib;

class Program
{
    static void Main(string[] args)
    {
        string mapPath = "../../../maps/test_map.png";  // Path to your test map image
        string csvPath = "../../../csv/terrain_mapping.csv";  // Path to the terrain mapping CSV
        string mapPathSave = "../../../maps/test_map_save.png";  // Path to your test save map image

        // Load the terrain mappings from the CSV
        MapKeyRef mapKeyRef = PixelEnvironmentMapper.LoadTerrainMappings(csvPath);
        
        // Load the map
        MapData mapData = MapLoader.LoadMap(mapPath, mapKeyRef);
        

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("***** - Map Key Table - *****");
        
        // Get all possible ConsoleColor values for printing to console
        Array colorValues = Enum.GetValues(typeof(ConsoleColor));
        Random random = new Random();
        Dictionary<string, ConsoleColor> colors = new Dictionary<string, ConsoleColor>();
        foreach (var key in mapKeyRef.GetAllTerrains())
        {
            // try not to use the same color multiple times for printing to console
            ConsoleColor randomColor = (ConsoleColor)(colorValues.GetValue(random.Next(colorValues.Length)) ?? ConsoleColor.White);
            if (colors.Count < colorValues.Length)
            {
                while (colors.ContainsValue(randomColor))
                {
                    randomColor = (ConsoleColor)(colorValues.GetValue(random.Next(colorValues.Length)) ?? ConsoleColor.White);
                }
            }
            colors.Add(key.Value.Key, randomColor);
            Console.ForegroundColor = randomColor;
            
            // Get map key
            MapKey terrainType = mapKeyRef.GetTerrainType(key.Key);
            // Print key
            Console.WriteLine($"{terrainType.Key} = {terrainType.Name}");
        }
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("***** - Original Map - *****");
        
        // Print original map
        PrintMap(mapData, colors);
        
        // Edit map
        colors.Add("?", ConsoleColor.Red);
        MapKey unknown = new MapKey();
        mapData.SetTerrain(0,0, unknown);
        mapData.SetTerrain(0,1, unknown);
        mapData.SetTerrain(0,2, unknown);
        mapData.SetTerrain(0,3, unknown);
        mapData.SetTerrain(0,4, unknown);

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("***** - Edited Map - *****");
        
        // Print edited map
        PrintMap(mapData, colors);
        
        // Save map to png
        MapSaver.SaveMap(mapPathSave, mapData);
    }
    
    private static void PrintMap(MapData mapData, Dictionary<string, ConsoleColor> colors)
    {
        // Print the terrain type at each location
        for (int x = 0; x < 50; x++)
        {
            for (int y = 0; y < 50; y++)
            {
                MapKey terrainType = mapData.GetTerrainAt(x, y);
                Console.ForegroundColor = colors[terrainType.Key];
                Console.Write($"{terrainType.Key} ");
            }
            Console.WriteLine();
        }
    }
}