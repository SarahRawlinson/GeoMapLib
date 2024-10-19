# GeoMapLib

```GeoMapLib``` is a cross-platform C# library for generating and managing geographic maps 
with terrain types dynamically loaded from image files and CSV data. The library allows you 
to interpret pixel colors in a PNG image and map them to specific terrain types based on a 
CSV key, making it highly customizable and data-driven. With support for both image-based 
map loading and customizable color-to-terrain mappings, the library is perfect for 
simulations, games, or other projects where geographic or environmental context is required.

Features:
- Cross-Platform: Works seamlessly across Windows, macOS, and Linux using ImageSharp for image processing.
- Image-Based Terrain Loading: Load terrain types directly from a PNG image, using pixel colors to represent different terrains.
- CSV-Based Mapping: Customize terrain types and their corresponding symbols and colors via a CSV file.
- Data-Driven Flexibility: Modify the terrain mapping and terrain symbols by simply editing the CSV file without modifying the code.
- Console Visualization: Provides the ability to visualize the map in the console with random colors assigned to each terrain type.


## Structure

```
/GeoMapLib
├── /src
│   ├── MapKey.cs                           # Holds terrain key data
│   ├── MapLoader.cs                        # Loads map image files and generates terrain data
│   └── PixelEnvironmentMapper.cs           # Maps pixel colors to attributes
├── /test
│   ├── /csv                                # Folder for test csv files
│   │   └── terrain_mapping.csv             # CSV file for loading the keys for colour on the map
│   ├── /maps                               # Folder for test map images
│   │   └── test_map.png                    # PNG file for loading the map data
│   └── Program.cs                          # Example usage of the GeoMapLib
└── README.md                               # Documentation for the library
```

## Image File
use a png image to load terrain types (test image 50px x 50px)

<img alt="image" height="400" width="400" src="/test/maps/test_map.png"/>

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
```
## Example Output
```
% = Water
x = Land
^ = Mountain
* = Forest

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

```