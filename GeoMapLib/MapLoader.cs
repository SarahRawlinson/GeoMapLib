using GeoMapLib;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

public class MapLoader
{
    public MapData LoadMap(string imagePath, MapKeyRef mapKeyRef)
    {
        using (Image<Rgba32> image = Image.Load<Rgba32>(imagePath))
        {
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
}
