using GeoMapLib;
using NUnit.Framework;
using SixLabors.ImageSharp.PixelFormats;

namespace Tests;

[TestFixture]
[TestOf(typeof(MapKeyRef))]
public class MapKeyRefTest
{
    [Test]
    public void GetTerrainType_ReturnsCorrectMapKeyWhenColorIsPresent()
    {
        var mapKeyRef = new MapKeyRef();
        var color = new Rgba32(255, 255, 255, 255);
        mapKeyRef.AddMapKey("terrainType", "symbol", color);
        var result = mapKeyRef.GetTerrainType(color);

        Assert.That(result.Name, Is.EqualTo("terrainType"));
        Assert.That(result.Key, Is.EqualTo("symbol"));
        Assert.That(result.Colour, Is.EqualTo(color));
    }

    [Test]
    public void GetTerrainType_ReturnsUnknownMapKeyWhenColorIsNotPresent()
    {
        var mapKeyRef = new MapKeyRef();
        var result = mapKeyRef.GetTerrainType(new Rgba32(255, 255, 255, 255));
        Assert.That(result.Name, Is.EqualTo("unknown"));
        Assert.That(result.Key, Is.EqualTo("?"));
    }

    [Test]
    public void GetAllTerrains_ReturnsAllTerrainMappings()
    {
        var mapKeyRef = new MapKeyRef();
        var color = new Rgba32(255, 255, 255, 255);
        mapKeyRef.AddMapKey("terrainType", "symbol", color);
        Dictionary<Rgba32, MapKey> result = mapKeyRef.GetAllTerrains();

        Assert.That(result.ContainsKey(color));
        Assert.That(result[color].Name, Is.EqualTo("terrainType"));
        Assert.That(result[color].Key, Is.EqualTo("symbol"));
        Assert.That(result[color].Colour, Is.EqualTo(color));
    }

    [Test]
    public void AddMapKey_AddsMapKeySuccessfullyWhenColorIsNotPresent()
    {
        var mapKeyRef = new MapKeyRef();
        var color = new Rgba32(255, 255, 255, 255);
        mapKeyRef.AddMapKey("terrainType", "symbol", color);
        var result = mapKeyRef.GetTerrainType(color);

        Assert.That(result.Name, Is.EqualTo("terrainType"));
        Assert.That(result.Key, Is.EqualTo("symbol"));
        Assert.That(result.Colour, Is.EqualTo(color));
    }

    [Test]
    public void AddMapKey_DoesNotAddMapKeyWhenColorIsPresent()
    {
        var mapKeyRef = new MapKeyRef();
        var color = new Rgba32(255, 255, 255, 255);
        mapKeyRef.AddMapKey("terrainType1", "symbol1", color);
        mapKeyRef.AddMapKey("terrainType2", "symbol2", color);
        var result = mapKeyRef.GetTerrainType(color);

        Assert.That(result.Name, Is.EqualTo("terrainType1"));
        Assert.That(result.Key, Is.EqualTo("symbol1"));
        Assert.That(result.Colour, Is.EqualTo(color));
    }

    [Test]
    public void AddMapKey_DoesNotAddMapKeyWhenTerrainTypeOrSymbolIsEmpty()
    {
        var mapKeyRef = new MapKeyRef();
        var color = new Rgba32(255, 255, 255, 255);
        mapKeyRef.AddMapKey("", "symbol", color);
        mapKeyRef.AddMapKey("terrainType", "", color);
        var result = mapKeyRef.GetTerrainType(color);

        Assert.That(result.Name, Is.EqualTo("unknown"));
        Assert.That(result.Key, Is.EqualTo("?"));
    }
}