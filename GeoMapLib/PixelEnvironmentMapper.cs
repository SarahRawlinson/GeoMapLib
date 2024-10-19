namespace GeoMapLib;

using System;
using System.Collections.Generic;
using SixLabors.ImageSharp.PixelFormats;
using System.Globalization;
using System.IO;
using CsvHelper;

public static class PixelEnvironmentMapper
{
    private static Dictionary<Rgba32, MapKey> terrainMappings = new Dictionary<Rgba32, MapKey>();
    
    // Load terrain mappings from a CSV file
    public static void LoadTerrainMappings(string csvPath)
    {
        using (var reader = new StreamReader(csvPath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            csv.Read();
            csv.ReadHeader();

            while (csv.Read())
            {
                string colorHex = csv.GetField<string>("ColorHex") ?? string.Empty;
                string terrainType = csv.GetField<string>("TerrainType")  ?? string.Empty;
                string symbol = csv.GetField<string>("Symbol") ?? string.Empty;

                if (colorHex != string.Empty)
                {
                    Rgba32 color = Rgba32.ParseHex(colorHex);

                    if (!terrainMappings.ContainsKey(color))
                    {
                        if (terrainType != String.Empty && symbol != String.Empty)
                        {
                            terrainMappings.Add(color, new MapKey(terrainType, symbol, colorHex));
                        }
                    }
                }
            }
        }
    }

    // Get terrain type from the pixel color
    public static MapKey GetTerrainType(Rgba32 pixelColor)
    {
        if (terrainMappings.ContainsKey(pixelColor))
        {
            return terrainMappings[pixelColor];
        }

        return new MapKey("unknown", "?", "#ff3300");
    }

    public static Dictionary<Rgba32, MapKey> GetAllTerrains()
    {
        return terrainMappings;
    }
    
}
