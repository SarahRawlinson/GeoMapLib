using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace GeoMapLib;

public static class MapLoader
{
    public static MapData LoadMap(string imagePath, MapKeyRef mapKeyRef)
    {
        if (imagePath == null)
        {
            throw new ArgumentNullException(nameof(imagePath), "Image path cannot be null");
        }
        
        if (imagePath == string.Empty)
        {
            throw new ArgumentException("Image path cannot be empty");
        }

        if (mapKeyRef == null)
        {
            throw new ArgumentNullException(nameof(mapKeyRef), "MapKeyRef cannot be null");
        }

        if (!File.Exists(imagePath))
        {
            throw new FileNotFoundException("Image file not found", imagePath);
        }

        if (Path.GetExtension(imagePath) != ".png")
        {
            throw new ArgumentException("Image file must be a PNG file", nameof(imagePath));
        }

        using Image<Rgba32> image = Image.Load<Rgba32>(imagePath);
        int width = image.Width;
        int height = image.Height;

        MapData terrainMap = new MapData(width, height, mapKeyRef);
            
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Rgba32 pixelColor = image[x, y];
                MapKey terrainType = mapKeyRef.GetTerrainType(pixelColor);
                terrainMap.SetTerrain(y, x, terrainType);
            }
        }

        return terrainMap;
    }
}