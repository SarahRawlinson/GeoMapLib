using GeoMapLib;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

public class MapLoader
{
    private MapKey[,] terrainMap; // Store terrain data as a 2D array of strings

    // Load the map and the terrain mappings
    public void LoadMap(string imagePath)
    {
        
        // Load the map image using ImageSharp
        using (Image<Rgba32> image = Image.Load<Rgba32>(imagePath))
        {
            int width = image.Width;
            int height = image.Height;

            terrainMap = new MapKey[width, height];

            // Loop through each pixel in the image
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Rgba32 pixelColor = image[x, y];

                    // Convert the pixel color to a terrain type and store it in the map array
                    MapKey terrainType = PixelEnvironmentMapper.GetTerrainType(pixelColor);
                    terrainMap[y, x] = terrainType;
                }
            }
        }
    }

    // Get the terrain type at a specific location
    public MapKey GetTerrainAt(int x, int y)
    {
        return terrainMap[x, y];
    }
}
