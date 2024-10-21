using SixLabors.ImageSharp.PixelFormats;

namespace GeoMapLib;

public class MapKeyRef
{
    private Dictionary<Rgba32, MapKey> terrainMappings = new Dictionary<Rgba32, MapKey>();
    private Rgba32 blank = new Rgba32();
    public MapKey GetTerrainType(Rgba32 pixelColor)
    {
        if (terrainMappings.ContainsKey(pixelColor))
        {
            return terrainMappings[pixelColor];
        }
        return new MapKey("unknown", "?",  blank);
    }

    public Dictionary<Rgba32, MapKey> GetAllTerrains()
    {
        return terrainMappings;
    }

    public void AddMapKey(string terrainType, string symbol, Rgba32 color)
    {
        if (!terrainMappings.ContainsKey(color))
        {
            if (terrainType != String.Empty && symbol != String.Empty)
            {
                terrainMappings.Add(color, new MapKey(terrainType, symbol, color));
            }
        }
    }
}