using SixLabors.ImageSharp.PixelFormats;

namespace GeoMapLib;

public class MapKey
{
    public string Name;
    public string Key;
    public Rgba32 Colour;

    public MapKey(string name = "unknown", string key = "?", Rgba32 colour = new Rgba32())
    {
        Name = name;
        Key = key;
        Colour = colour;
    }
}