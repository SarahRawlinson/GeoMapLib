using GeoMapLib;
using SixLabors.ImageSharp.PixelFormats;

class Program
{
    static void Main(string[] args)
    {
        string mapPath = "../../../maps/test_map.png";  // Path to your test map image
        string csvPath = "../../../csv/terrain_mapping.csv";  // Path to the terrain mapping CSV
        string csvPathSave = "../../../csv/terrain_mapping_save.csv";  // Path to your test save terrain mapping CSV
        string mapPathSave = "../../../maps/test_map_save.png";  // Path to your test save map image

        // Load the terrain mappings from the CSV
        MapKeyRef mapKeyRef = PixelEnvironmentMapper.LoadTerrainMappings(csvPath);
        
        // Load the map
        MapData mapData = MapLoader.LoadMap(mapPath, mapKeyRef);
        
        // Get Console Colours
        var colors = PrettyMapConsole.GetConsoleColors(mapKeyRef);
        
        // Print original map
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("***** - Original Map - *****");
        Console.ForegroundColor = default;
        PrettyMapConsole.PrintMapKeyTable(mapKeyRef, colors);
        PrettyMapConsole.PrintMap(mapData, colors, 50, 50);
        
        // Edit map
        mapKeyRef.AddMapKey("unknown", "?", new Rgba32());
        MapKey unknown = new MapKey();
        mapData.SetTerrain(0,0, unknown);
        mapData.SetTerrain(0,1, unknown);
        mapData.SetTerrain(0,2, unknown);
        mapData.SetTerrain(0,3, unknown);
        mapData.SetTerrain(0,4, unknown);

        // Print edited map
        //colors = PrettyMapConsole.GetConsoleColors(mapKeyRef);
        colors.Add("?", ConsoleColor.Red);
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("***** - Edited Map - *****");
        Console.ForegroundColor = default;
        PrettyMapConsole.PrintMapKeyTable(mapKeyRef, colors);
        PrettyMapConsole.PrintMap(mapData, colors, 50, 50);
        
        // Save map to png
        MapSaver.SaveMap(mapPathSave, mapData);
        
        // Save map key to csv
        PixelEnvironmentMapper.SaveTerrainMappings(csvPathSave, mapKeyRef);
        
        // See saved data loaded to make sure results are expected
        MapKeyRef savedMapKeyRef = PixelEnvironmentMapper.LoadTerrainMappings(csvPathSave);
        MapData saveMapData = MapLoader.LoadMap(mapPathSave, mapKeyRef);
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("***** - Saved Map - *****");
        Console.ForegroundColor = default;
        PrettyMapConsole.PrintMapKeyTable(savedMapKeyRef, colors);
        PrettyMapConsole.PrintMap(saveMapData, colors, 50, 50);
        
    }

    
}