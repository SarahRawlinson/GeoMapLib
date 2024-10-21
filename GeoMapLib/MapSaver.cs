namespace GeoMapLib;

using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

public class MapSaver
{
    
    public static void SaveMap(string savePath, MapData terrainData)
    {
        using (Image<Rgba32> image = new Image<Rgba32>(terrainData.Width, terrainData.Height))
        {
            for (int x = 0; x < terrainData.Width; x++)
            {
                for (int y = 0; y < terrainData.Height; y++)
                {
                    MapKey terrainType = terrainData.GetTerrainAt(y, x);
                    Rgba32 pixelColor = terrainType.Colour;
                    image[x, y] = pixelColor;
                }
            }
            image.Save(savePath);
        }
    }
}
