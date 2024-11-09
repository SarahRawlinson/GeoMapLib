using GeoMapLib;
using NUnit.Framework;
using SixLabors.ImageSharp.PixelFormats;

namespace Tests;

[TestFixture]
[TestOf(typeof(MapKey))]
public class MapKeyTest
{
    [Test]
    public void DefaultConstructor_ShouldInitializePropertiesCorrectly()
    {
        const string defaultName = "unknown";
        const string defaultKey = "?";
        Rgba32 defaultColour = new();
        
        MapKey mapKey = new();
        
        Assert.That(mapKey.TerrainType, Is.EqualTo(defaultName));
        Assert.That(mapKey.Symbol, Is.EqualTo(defaultKey));
        Assert.That(mapKey.ColorHex, Is.EqualTo(defaultColour));
    }

    [Test]
    [TestCase("name1", "key1", 100, 100, 100, 100)]
    [TestCase("name2", "undefined", 200, 200, 200, 200)]
    [TestCase("name3", "key3", 0, 0, 0, 0)]
    [TestCase("", "", 255, 255, 255, 255)]
    [TestCase(null, null, 0, 0, 0, 0)]
    public void ParametrizedConstructor_ShouldInitializePropertiesCorrectly(string name, string key, byte r, byte g,
        byte b, byte a)
    {
        Rgba32 expectedColour = new(r, g, b, a);

        MapKey mapKey = new(name, key, expectedColour);
        
        Assert.That(mapKey.TerrainType, Is.EqualTo(name));
        Assert.That(mapKey.Symbol, Is.EqualTo(key));
        Assert.That(mapKey.ColorHex, Is.EqualTo(expectedColour));
    }
}