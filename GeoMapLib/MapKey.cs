using SixLabors.ImageSharp.PixelFormats;

namespace GeoMapLib;

public class MapKey
{
    public string Name;
    public string Key;
    public Rgba32 Colour;

    public MapKey(string name, string key, Rgba32 colour)
    {
        Name = name;
        Key = key;
        Colour = colour;
    }
}