using SixLabors.ImageSharp.PixelFormats;
using System.Globalization;
using CsvHelper;

namespace GeoMapLib;

public static class PixelEnvironmentMapper
{
    public static MapKeyRef LoadTerrainMappings(string csvPath)
    {
        MapKeyRef terrainMappings = new MapKeyRef();
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
                    terrainMappings.AddMapKey(terrainType, symbol, color);
                }
            }
        }
        return terrainMappings;
    }
}
