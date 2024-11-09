using GeoMapLib;
using NUnit.Framework;
using System.Text;
using SixLabors.ImageSharp.PixelFormats;

namespace Tests;

[TestFixture]
[TestOf(typeof(PixelEnvironmentMapper))]
public class PixelEnvironmentMapperTest
{
    [Test]
    public void LoadTerrainMappings_ValidCsvInput_ReturnsNotEmptyMap()
    {
        var csvContent = new StringBuilder();
        csvContent.AppendLine("ColorHex,TerrainType,Symbol");
        csvContent.AppendLine("#FFFFFF,Mountain,M");
        var csvPath = WriteToFile(csvContent);
        
        var result = PixelEnvironmentMapper.LoadTerrainMappings(csvPath);
        
        Assert.IsNotEmpty(result.GetAllTerrains());
    }

    [Test]
    public void LoadTerrainMappings_EmptyCsvInput_ReturnsEmptyMap()
    {
        var csvContent = new StringBuilder();
        csvContent.AppendLine("ColorHex,TerrainType,Symbol");
        var csvPath = WriteToFile(csvContent);
        
        var result = PixelEnvironmentMapper.LoadTerrainMappings(csvPath);
        
        Assert.IsEmpty(result.GetAllTerrains());
    }

    [Test]
    public void LoadTerrainMappings_InvalidHexColor_UsesBlankRGBA()
    {
        var csvContent = new StringBuilder();
        csvContent.AppendLine("ColorHex,TerrainType,Symbol");
        csvContent.AppendLine("INVALID_COLOR,Mountain,M");
        var csvPath = WriteToFile(csvContent);
        
        var result = PixelEnvironmentMapper.LoadTerrainMappings(csvPath);
        var data = new MapKey("Mountain", "M");
        Assert.That(result.GetAllTerrains().First().Value.Symbol, Is.EqualTo(data.Symbol));
        Assert.That(result.GetAllTerrains().First().Value.TerrainType, Is.EqualTo(data.TerrainType));
        Assert.That(result.GetAllTerrains().First().Value.ColorHex, Is.EqualTo(data.ColorHex));
        Assert.That(result.GetAllTerrains().First().Key, Is.EqualTo(new Rgba32()));
    }
    
    [Test]
    public void SaveTerrainMappings_ValidInput_WritesToFileCorrectly()
    {
        var originalTerrainMap = new MapKeyRef();
        var testTerrainMapKeys = new List<MapKey>
        {
            new("Mountain", "M", new Rgba32(255,255,255)),
            new("Grassland", "G", new Rgba32(0,128,0))
        };
        foreach (var key in testTerrainMapKeys)
        {
            originalTerrainMap.AddMapKey(key.TerrainType, key.Symbol, key.ColorHex);
        }

        var csvPath = Path.GetTempFileName();
        PixelEnvironmentMapper.SaveTerrainMappings(csvPath, originalTerrainMap);

        var loadedTerrainMap = PixelEnvironmentMapper.LoadTerrainMappings(csvPath);
        
        foreach (var originalTerrain in originalTerrainMap.GetAllTerrains())
        {
            Assert.That(loadedTerrainMap.GetAllTerrains(), Does.ContainKey(originalTerrain.Key));
            Assert.That(loadedTerrainMap.GetTerrainType(originalTerrain.Key).TerrainType, Is.EqualTo(originalTerrain.Value.TerrainType));
            Assert.That(loadedTerrainMap.GetTerrainType(originalTerrain.Key).Symbol, Is.EqualTo(originalTerrain.Value.Symbol));
            Assert.That(loadedTerrainMap.GetTerrainType(originalTerrain.Key).ColorHex, Is.EqualTo(originalTerrain.Value.ColorHex));
        }
    }

    [Test]
    public void LoadTerrainMappings_EmptyHexColor_IgnoresThisLine()
    {
        var csvContent = new StringBuilder();
        csvContent.AppendLine("ColorHex,TerrainType,Symbol");
        csvContent.AppendLine(",Mountain,M");
        var csvPath = WriteToFile(csvContent);
        
        var result = PixelEnvironmentMapper.LoadTerrainMappings(csvPath);
        
        Assert.IsEmpty(result.GetAllTerrains());
    }

    private static string WriteToFile(StringBuilder content)
    {
        var path = Path.GetTempFileName();
        File.WriteAllText(path, content.ToString());
        return path;
    }
}