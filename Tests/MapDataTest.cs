using GeoMapLib;
using NUnit.Framework;

namespace Tests;

[TestFixture]
[TestOf(typeof(MapData))]
public class MapDataTest
{
    private MapData _mapData;
    private const int TestWidth = 5;
    private const int TestHeight = 5;

    [SetUp]
    public void Setup()
    {
        var mapKeyRef = new MapKeyRef();
        _mapData = new MapData(TestWidth, TestHeight, mapKeyRef);
    }

    [Test]
    public void Constructor_InitializesCorrectly()
    {
        Assert.That(_mapData.Width, Is.EqualTo(TestWidth));
        Assert.That(_mapData.Height, Is.EqualTo(TestHeight));
        Assert.IsInstanceOf<MapKeyRef>(_mapData.MapKeyRef);
    }

    [Test]
    public void GetTerrainAt_ReturnsNewTerrainIfNoneExists()
    {
        var terrain = _mapData.GetTerrainAt(0, 0);
        Assert.IsNotNull(terrain);
        Assert.IsInstanceOf<MapKey>(terrain);
    }

    [Test]
    public void GetTerrainAt_ReturnsExistingTerrain()
    {
        _mapData.SetTerrain(0, 0, new MapKey());

        var terrain = _mapData.GetTerrainAt(0, 0);

        Assert.IsNotNull(terrain);
        Assert.IsInstanceOf<MapKey>(terrain);
    }

    [Test]
    public void SetTerrain_SetsTerrainCorrectly()
    {
        var newTerrain = new MapKey();
        _mapData.SetTerrain(0, 0, newTerrain);

        var terrain = _mapData.GetTerrainAt(0, 0);

        Assert.That(terrain, Is.EqualTo(newTerrain));
    }
}