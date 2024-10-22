using SixLabors.ImageSharp.PixelFormats;

namespace GeoMapLib;

public class MapKeyRef
{
    private readonly Dictionary<Rgba32, MapKey> _terrainMappings = new Dictionary<Rgba32, MapKey>();
    private Rgba32 _blank = new Rgba32();
    public MapKey GetTerrainType(Rgba32 pixelColor)
    {
        if (_terrainMappings.TryGetValue(pixelColor, out var type))
        {
            return type;
        }
        return new MapKey("unknown", "?",  _blank);
    }

    public Dictionary<Rgba32, MapKey> GetAllTerrains()
    {
        return _terrainMappings;
    }

    public void AddMapKey(string terrainType, string symbol, Rgba32 color)
    {
        if (!_terrainMappings.ContainsKey(color))
        {
            if (terrainType != String.Empty && symbol != String.Empty)
            {
                _terrainMappings.Add(color, new MapKey(terrainType, symbol, color));
            }
        }
    }
}