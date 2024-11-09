namespace GeoMapLib;

using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

public static class MapSaver
{
    
    public static void SaveMap(string savePath, MapData mapData)
    {
        if (savePath == string.Empty)
        {
            throw new ArgumentException("Save path cannot be empty", nameof(savePath));
        }
        if (mapData == null)
        {
            throw new ArgumentNullException(nameof(mapData));
        }

        if (mapData.Width <= 0 || mapData.Height <= 0)
        {
            throw new ArgumentException("Map dimensions must be greater than 0", nameof(mapData));
        }
        
        using Image<Rgba32> image = new Image<Rgba32>(mapData.Width, mapData.Height);
        for (int x = 0; x < mapData.Width; x++)
        {
            for (int y = 0; y < mapData.Height; y++)
            {
                MapKey terrainType = mapData.GetTerrainAt(y, x);
                Rgba32 pixelColor = terrainType.ColorHex;
                image[x, y] = pixelColor;
            }
        }
        image.Save(savePath);
    }
}
