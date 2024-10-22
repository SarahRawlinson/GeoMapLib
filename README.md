# GeoMapLib

```GeoMapLib``` is a cross-platform C# library for generating and managing geographic maps 
with terrain types dynamically loaded from png image files and CSV data. The library allows you 
to interpret pixel colors in a PNG image and map them to specific terrain types based on a 
CSV key and can conversely take the map data to make a new PNG image. the library is ideal for 
simulations, games, or other projects where geographic or environmental context is required.

Features:
- Cross-Platform: Works seamlessly across Windows, macOS, and Linux using ImageSharp for image processing.
- Image-Based Terrain Loading: Load terrain types directly from a PNG image, using pixel colors to represent different terrains.
- Image-Based Terrain Saving: Saves terrain directly to a PNG image, using the map key to build an image from map data.
- CSV-Based Mapping: Customize terrain types and their corresponding symbols and colors via a CSV file.
- Data-Driven Flexibility: Modify the terrain mapping and terrain symbols by simply editing the CSV file without modifying the code.
- Console Visualization: Provides the ability to visualize the map in the console with random colors assigned to each terrain type.


## Structure

```
/GeoMapLib
├── /src
│   ├── MapData.cs                          # Holds info for building map (width, height, keys, terrain)
│   ├── MapKey.cs                           # Holds terrain key data
│   ├── MapKeyRef.cs                        # Holds terrain keys for building or displaying map (colour, symble, name)
│   ├── MapLoader.cs                        # Loads map image files and generates terrain data
│   ├── MapSaver.cs                         # Creates a png image from map data
│   └── PixelEnvironmentMapper.cs           # Maps pixel colors to attributes
├── /test
│   ├── /csv                                # Folder for test csv files
│   │   └── terrain_mapping.csv             # CSV file for loading the keys for colour on the map
│   ├── /maps                               # Folder for test map images
│   │   │── test_map.png                    # PNG file for loading the map data
│   │   └── test_map_save.png               # saving location for PNG image
│   └── Program.cs                          # Example usage of the GeoMapLib
└── README.md                               # Documentation for the library
```

## Image File
use a png image to load terrain types (test image 50px x 50px)

<img alt="image" height="400" width="400" src="/test/maps/test_map.png"/>

an example output of the saved image

<img alt="image" height="400" width="400" src="/test/maps/test_map_save.png"/>

## CSV Map Keys
use csv to interpret the colours of the image pixel to a terrain type
```
ColorHex,TerrainType,Symbol
#000000,Water,%
#FFFFFF,Land,x
#808080,Mountain,^
#00FF00,Forest,*
```

## Example Usage
```
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
```
## Example Output
```
***** - Map Key Table - *****
% = Water
x = Land
^ = Mountain
* = Forest
***** - Original Map - *****
% % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % x x x x x x x x x % % % % % % % % % % % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % x x x x x x x x x x x x x x % % % % % % % % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % x x x x x x x x x x x x x x x x x x x x % % % % % % % % % % % % % % % % % 
% % % % % % % % % % % x x x x x x x x x x ^ ^ x x x x x x x x x x % % % % % % % % % % % % % % % % % 
% % % % % % % % % % x x x x x x x x x x x ^ ^ x x x x x x x x x x % % % % % % % % % % % % % % % % % 
% % % % % % % % % x x x x x x x x x ^ x x x x x * * * x x x x x x % % % % % % % % % % % % % % % % % 
% % % % % % % % x x x x x x x x x ^ ^ ^ ^ x x ^ ^ * * x x x x x x x % % % % % % % % % % % % % % % % 
% % % % % % % % x x x x x x x x ^ ^ ^ ^ ^ ^ x ^ ^ ^ * x ^ x x x x x % % % % % % % % % % % % % % % % 
% % % % % % % % x x x x x ^ x x ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ * * ^ ^ x x x x % % % % % % % % % % % % % % % % 
% % % % % % % % x x x x ^ ^ ^ x x ^ ^ ^ * * ^ ^ ^ ^ * * x * * x x x % % % % % % % x x x x % % % % % 
% % % % % % % % x x x x ^ x ^ x * * * * * ^ ^ ^ ^ x x * * * x * x x % % % x x x x x x x x x % % % % 
% % % % % % % % x x x ^ ^ ^ ^ ^ * * * * * ^ * x ^ ^ ^ ^ ^ ^ ^ * x x x x x x x x x x x x x x % % % % 
% % % % % % % % % x x * * ^ ^ ^ * * * * ^ ^ * * * x ^ ^ ^ ^ ^ * * * * * * x x x x x x x x x % % % % 
% % % % % % % % % x x * * * * * * * * * ^ x x x * * ^ ^ ^ ^ ^ * * * ^ ^ ^ ^ ^ x x x x x x x x % % % 
% % % % % % % % % x x * * * * * * * * * * x ^ ^ ^ * x ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ x ^ x x x x x x x % % % 
% % % % % % % % % x x * * * * * x ^ ^ ^ x ^ ^ ^ ^ * * x * x ^ ^ ^ ^ ^ ^ x ^ ^ ^ * * x x x x x % % % 
% % % % % % % % % x x * * * * * x ^ x ^ ^ ^ * * * * * * * ^ ^ x ^ ^ ^ x * ^ x ^ * * * x x x x % % % 
% % % % % % % % % x x * * * * * * * * * * * * * * * * * x ^ ^ ^ ^ ^ ^ x * * * x x x * x x x x % % % 
% % % % % % % % % x x * * * x x * * * x * * * * * * * * * x ^ ^ ^ ^ ^ x * * * x ^ ^ ^ x x x x % % % 
% % % % % % % % % x x x * * x x x * * * * * * x x * * * * * x ^ ^ x ^ ^ x x * * ^ ^ x x x x x % % % 
% % % % % % % % % x x x x * x x x x x * x * * * * * * * * * * x ^ ^ ^ x ^ ^ x * x x x x x x % % % % 
% % % % % % % % x x x x * * * * * * x x x * * * * * * * * * * x ^ x * x x x x x x x x x x x % % % % 
% % % % % % % % x x x x x x x x x * x x x x * * * * * * x * * * x * * x x x x x x x x x x x % % % % 
% % % % % % % % % % x x x x x x * * * x x x * * * * * * * x * * * * * x x x x x x x x x x x % % % % 
% % % % % % % % % % % % % x x x x * * * x x x x * * * x * x * * * * x x x x x x x x x x x % % % % % 
% % % % % % % % % % % % % % x x x x x x x x x x * * * * * * * * * * x x x x x x x x x x x % % % % % 
% % % % % % % % % % % % % % % x x x x x x x * * * * * * * * * * x x x x x x x x x x x x x % % % % % 
% % % % % % % % % % % % % % % % % x x x x x * * * * * * * x * x x x x x x x x x x x x x x % % % % % 
% % % % % % % % % % % % % % % % % % x x x x x x x * * x * * * x x x x x x x x % % % x x % % % % % % 
% % % % % % % % % % % % % % % % % % x x x x x x x x x x * * x x x x x x x % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % x x x x x x x x x * x x x x x x x % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % % x x x x x x x x * x x x x x x % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % % x x x x x x x x x x x x x x x % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % % % x x x x x x x x x x x x x % % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % % % % % % x x x x x x x x x % % % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % % % % % % % x x x x x x x % % % % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % % % % % % % % % x x x % % % % % % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
***** - Edited Map - *****
? ? ? ? ? % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % x x x x x x x x x % % % % % % % % % % % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % x x x x x x x x x x x x x x % % % % % % % % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % x x x x x x x x x x x x x x x x x x x x % % % % % % % % % % % % % % % % % 
% % % % % % % % % % % x x x x x x x x x x ^ ^ x x x x x x x x x x % % % % % % % % % % % % % % % % % 
% % % % % % % % % % x x x x x x x x x x x ^ ^ x x x x x x x x x x % % % % % % % % % % % % % % % % % 
% % % % % % % % % x x x x x x x x x ^ x x x x x * * * x x x x x x % % % % % % % % % % % % % % % % % 
% % % % % % % % x x x x x x x x x ^ ^ ^ ^ x x ^ ^ * * x x x x x x x % % % % % % % % % % % % % % % % 
% % % % % % % % x x x x x x x x ^ ^ ^ ^ ^ ^ x ^ ^ ^ * x ^ x x x x x % % % % % % % % % % % % % % % % 
% % % % % % % % x x x x x ^ x x ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ * * ^ ^ x x x x % % % % % % % % % % % % % % % % 
% % % % % % % % x x x x ^ ^ ^ x x ^ ^ ^ * * ^ ^ ^ ^ * * x * * x x x % % % % % % % x x x x % % % % % 
% % % % % % % % x x x x ^ x ^ x * * * * * ^ ^ ^ ^ x x * * * x * x x % % % x x x x x x x x x % % % % 
% % % % % % % % x x x ^ ^ ^ ^ ^ * * * * * ^ * x ^ ^ ^ ^ ^ ^ ^ * x x x x x x x x x x x x x x % % % % 
% % % % % % % % % x x * * ^ ^ ^ * * * * ^ ^ * * * x ^ ^ ^ ^ ^ * * * * * * x x x x x x x x x % % % % 
% % % % % % % % % x x * * * * * * * * * ^ x x x * * ^ ^ ^ ^ ^ * * * ^ ^ ^ ^ ^ x x x x x x x x % % % 
% % % % % % % % % x x * * * * * * * * * * x ^ ^ ^ * x ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ x ^ x x x x x x x % % % 
% % % % % % % % % x x * * * * * x ^ ^ ^ x ^ ^ ^ ^ * * x * x ^ ^ ^ ^ ^ ^ x ^ ^ ^ * * x x x x x % % % 
% % % % % % % % % x x * * * * * x ^ x ^ ^ ^ * * * * * * * ^ ^ x ^ ^ ^ x * ^ x ^ * * * x x x x % % % 
% % % % % % % % % x x * * * * * * * * * * * * * * * * * x ^ ^ ^ ^ ^ ^ x * * * x x x * x x x x % % % 
% % % % % % % % % x x * * * x x * * * x * * * * * * * * * x ^ ^ ^ ^ ^ x * * * x ^ ^ ^ x x x x % % % 
% % % % % % % % % x x x * * x x x * * * * * * x x * * * * * x ^ ^ x ^ ^ x x * * ^ ^ x x x x x % % % 
% % % % % % % % % x x x x * x x x x x * x * * * * * * * * * * x ^ ^ ^ x ^ ^ x * x x x x x x % % % % 
% % % % % % % % x x x x * * * * * * x x x * * * * * * * * * * x ^ x * x x x x x x x x x x x % % % % 
% % % % % % % % x x x x x x x x x * x x x x * * * * * * x * * * x * * x x x x x x x x x x x % % % % 
% % % % % % % % % % x x x x x x * * * x x x * * * * * * * x * * * * * x x x x x x x x x x x % % % % 
% % % % % % % % % % % % % x x x x * * * x x x x * * * x * x * * * * x x x x x x x x x x x % % % % % 
% % % % % % % % % % % % % % x x x x x x x x x x * * * * * * * * * * x x x x x x x x x x x % % % % % 
% % % % % % % % % % % % % % % x x x x x x x * * * * * * * * * * x x x x x x x x x x x x x % % % % % 
% % % % % % % % % % % % % % % % % x x x x x * * * * * * * x * x x x x x x x x x x x x x x % % % % % 
% % % % % % % % % % % % % % % % % % x x x x x x x * * x * * * x x x x x x x x % % % x x % % % % % % 
% % % % % % % % % % % % % % % % % % x x x x x x x x x x * * x x x x x x x % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % x x x x x x x x x * x x x x x x x % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % % x x x x x x x x * x x x x x x % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % % x x x x x x x x x x x x x x x % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % % % x x x x x x x x x x x x x % % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % % % % % % x x x x x x x x x % % % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % % % % % % % x x x x x x x % % % % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % % % % % % % % % x x x % % % % % % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
% % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
```