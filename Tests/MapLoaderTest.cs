using GeoMapLib;
using NUnit.Framework;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Tests;

[TestFixture]
[TestOf(typeof(MapLoader))]
public class MapLoaderTest
{
    private MapKeyRef _mapKeyRef;
    private MapData _testMap;

    [SetUp]
    public void Setup()
    {
        _mapKeyRef = new MapKeyRef();
        _mapKeyRef.AddMapKey("sand", "s", new Rgba32(194, 178, 128, 255));
        _mapKeyRef.AddMapKey("grass", "g", new Rgba32(0, 255, 0, 255));
        _mapKeyRef.AddMapKey("water", "r", new Rgba32(0, 0, 255, 255));
        _mapKeyRef.AddMapKey("mountain", "m", new Rgba32(139, 69, 19, 255));
        _testMap = new MapData(2, 2, _mapKeyRef);
    }

    [TestCase("TestData/ValidImage.png", TestName = "LoadMap_WithValidImage_ShouldReturnExpectedMapData")]
    [TestCase("TestData/EmptyFile.png", TestName = "LoadMap_WithEmptyFile_ShouldThrowException")]
    [TestCase("TestData/NonExistentFile.png", TestName = "LoadMap_WithNonExistentFile_ShouldThrowException")]
    public void LoadMap(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Assert.Throws<FileNotFoundException>(() => MapLoader.LoadMap(filePath, _mapKeyRef));
        }
        else
        {
            var mapData = MapLoader.LoadMap(filePath, _mapKeyRef);

            Assert.That(mapData, Is.Not.Null);
            Assert.That(mapData.Width, Is.Positive);
            Assert.That(mapData.Height, Is.Positive);
        }
    }

    [Test]
    public void LoadMap_WithNullFilePath_ShouldThrowException()
    {
        string nullFilePath = null;

        Assert.Throws<ArgumentNullException>(() => MapLoader.LoadMap(nullFilePath, _mapKeyRef));
    }
    
    [Test]
    public void LoadMap_WithEmptyStringPath_ShouldThrowException()
    {
        string empty = string.Empty;

        Assert.Throws<ArgumentException>(() => MapLoader.LoadMap(empty, _mapKeyRef));
    }

    [Test]
    public void LoadMap_WithInvalidImage_ShouldThrowException()
    {
        var filePath = "../InvalidImage.txt";
        File.Create(filePath);
        Assert.Throws<ArgumentException>(() => MapLoader.LoadMap(filePath, _mapKeyRef));
        File.Delete(filePath);
    }
    
    [Test]
    public void LoadMap_WithoutMapKeyRef_ShouldThrowException()
    {
        var filePath = "../InvalidImage.txt";
        File.Create(filePath);
        Assert.Throws<ArgumentNullException>(() => MapLoader.LoadMap(filePath, null));
        File.Delete(filePath);
    }

    [Test]
    public void LoadMap_WithValidImage_ShouldBuildMap()
    {
        var validFilePath = "../ValidImage.png";
        BuildMap(_testMap, validFilePath);
        var result = MapLoader.LoadMap(validFilePath, _mapKeyRef);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Width, Is.EqualTo(_testMap.Width));
        Assert.That(result.Height, Is.EqualTo(_testMap.Height));
        Assert.That(result.MapKeyRef, Is.EqualTo(_mapKeyRef));
        
        for (int x = 0; x < _testMap.Width; x++)
        {
            for (int y = 0; y < _testMap.Height; y++)
            {
                var terrainAt = result.GetTerrainAt(x,y);
                var mapKey = _testMap.GetTerrainAt(x,y);
                Assert.That(terrainAt.Key, Is.EqualTo(mapKey.Key));
                Assert.That(terrainAt.Name, Is.EqualTo(mapKey.Name));
                Assert.That(terrainAt.Colour, Is.EqualTo(mapKey.Colour));
            }
        }
        File.Delete(validFilePath);
    }

    public static void BuildMap(MapData mapData, string savePath)
    {
        using Image<Rgba32> image = new Image<Rgba32>(mapData.Width, mapData.Height);
        for (int x = 0; x < mapData.Width; x++)
        {
            for (int y = 0; y < mapData.Height; y++)
            {
                MapKey terrainType = mapData.GetTerrainAt(y, x);
                Rgba32 pixelColor = terrainType.Colour;
                image[x, y] = pixelColor;
            }
        }
        image.Save(savePath);
    }
}