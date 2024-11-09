using SixLabors.ImageSharp.PixelFormats;
using System.Globalization;
using CsvHelper;
using System.IO;

namespace GeoMapLib;

public static class PixelEnvironmentMapper
{
    public static MapKeyRef LoadTerrainMappings(string csvPath)
    {
        MapKeyRef terrainMappings = new MapKeyRef();
        using var reader = new StreamReader(csvPath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        csv.Read();
        csv.ReadHeader();
        while (csv.Read())
        {
            string colorHex = csv.GetField<string>("ColorHex") ?? string.Empty;
            string terrainType = csv.GetField<string>("TerrainType")  ?? string.Empty;
            string symbol = csv.GetField<string>("Symbol") ?? string.Empty;
            if (colorHex != string.Empty)
            {
                if (!Rgba32.TryParseHex(colorHex, out Rgba32 color))
                {
                    color = new Rgba32();
                }
                terrainMappings.AddMapKey(terrainType, symbol, color);
            }
        }

        return terrainMappings;
    }

    public static void SaveTerrainMappings(string csvPath, MapKeyRef terrainMappings)
    {

        using var writer = new StreamWriter(csvPath);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        csv.WriteField("ColorHex");
        csv.WriteField("TerrainType");
        csv.WriteField("Symbol");
        csv.NextRecord();
        foreach (var terrainMapping in terrainMappings.GetAllTerrains())
        {
            csv.WriteField(terrainMapping.Key.ToHex());
            csv.WriteField(terrainMapping.Value.TerrainType);
            csv.WriteField(terrainMapping.Value.Symbol);
            csv.NextRecord();
        }
    }
}
