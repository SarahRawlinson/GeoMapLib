namespace GeoMapLib;

public static class PrettyMapConsole
{
    private static Random _random = new Random();
    public static void PrintMap(MapData mapData, Dictionary<string, ConsoleColor> colors, int width, int height)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("***** - Map Image - *****");
        // Print the terrain type at each location
        for (int x = 0; x < height; x++)
        {
            for (int y = 0; y < width; y++)
            {
                MapKey terrainType = mapData.GetTerrainAt(x, y);
                Console.ForegroundColor = colors[terrainType.Symbol];
                Console.Write($"{terrainType.Symbol} ");
            }
            Console.WriteLine();
        }
        Console.ForegroundColor = default;
    }

    public static Dictionary<string, ConsoleColor> GetConsoleColors(MapKeyRef mapKeyRef)
    {
        // Get all possible ConsoleColor values for printing to console
        Array colorValues = Enum.GetValues(typeof(ConsoleColor));
        
        Dictionary<string, ConsoleColor> colors = new Dictionary<string, ConsoleColor>();
        foreach (var key in mapKeyRef.GetAllTerrains())
        {
            // try not to use the same color multiple times for printing to console
            ConsoleColor randomColor = (ConsoleColor)(colorValues.GetValue(_random.Next(colorValues.Length)) ?? ConsoleColor.White);
            if (colors.Count < colorValues.Length)
            {
                while (colors.ContainsValue(randomColor))
                {
                    randomColor = (ConsoleColor)(colorValues.GetValue(_random.Next(colorValues.Length)) ?? ConsoleColor.White);
                }
            }
            colors.Add(key.Value.Symbol, randomColor);
            Console.ForegroundColor = randomColor;
        }

        return colors;
    }
    
    public static void PrintMapKeyTable(MapKeyRef mapKeyRef, Dictionary<string, ConsoleColor> colors)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("***** - Map Key Table - *****");

        foreach (var key in mapKeyRef.GetAllTerrains())
        {
            // Set console colour
            Console.ForegroundColor = colors.GetValueOrDefault(key.Value.Symbol);
            // Get map key
            MapKey terrainType = mapKeyRef.GetTerrainType(key.Key);
            // Print key
            Console.WriteLine($"{terrainType.Symbol} = {terrainType.TerrainType}");
        }
        Console.ForegroundColor = default;
    }
}