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
        Assert.That(result.GetAllTerrains().First().Value.Key, Is.EqualTo(data.Key));
        Assert.That(result.GetAllTerrains().First().Value.Name, Is.EqualTo(data.Name));
        Assert.That(result.GetAllTerrains().First().Value.Colour, Is.EqualTo(data.Colour));
        Assert.That(result.GetAllTerrains().First().Key, Is.EqualTo(new Rgba32()));
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