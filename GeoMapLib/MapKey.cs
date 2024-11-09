using SixLabors.ImageSharp.PixelFormats;

namespace GeoMapLib;

public class MapKey
{
    public readonly string TerrainType;
    public readonly string Symbol;
    public Rgba32 ColorHex;

    public MapKey(string terrainType = "unknown", string symbol = "?", Rgba32 colorHex = new Rgba32())
    {
        TerrainType = terrainType;
        Symbol = symbol;
        ColorHex = colorHex;
    }
}