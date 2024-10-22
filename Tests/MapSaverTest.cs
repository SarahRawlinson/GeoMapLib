using GeoMapLib;
using NUnit.Framework;
using SixLabors.ImageSharp.PixelFormats;

namespace Tests
{
    [TestFixture]
    [TestOf(typeof(MapSaver))]
    public class MapSaverTest
    {
        private static readonly string TestImagePath =
            Path.Combine(TestContext.CurrentContext.TestDirectory, "test_image.png");

        private static readonly MapKey TestMapKey = new MapKey("test", "t", Rgba32.ParseHex("#ffffff"));

        [SetUp]
        public void Setup()
        {
            if (File.Exists(TestImagePath))
            {
                File.Delete(TestImagePath);
            }
        }

        [TearDown]
        public void Teardown()
        {
            if (File.Exists(TestImagePath))
            {
                File.Delete(TestImagePath);
            }
        }

        [Test]
        public void SaveMap_ValidMapData_SavesImageToFile()
        {
            var mapData = CreateSampleMapData(10, 10);
            Assert.DoesNotThrow(() => MapSaver.SaveMap(TestImagePath, mapData));
            Assert.IsTrue(File.Exists(TestImagePath));
        }
        

        [Test]
        public void SaveMap_NullMapData_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => MapSaver.SaveMap(TestImagePath, null));
        }
        
        
        [Test]
        public void SaveMap_MapDataWidthLessThan1_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => MapSaver.SaveMap(TestImagePath, new MapData(0,1,new MapKeyRef())));
        }
        
        [Test]
        public void SaveMap_MapDataHeightLessThan1_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => MapSaver.SaveMap(TestImagePath, new MapData(1,0,new MapKeyRef())));
        }

        [Test]
        public void SaveMap_InvalidFilePath_ThrowsArgumentException()
        {
            var mapData = CreateSampleMapData(10, 10);
            Assert.Throws<ArgumentException>(() => MapSaver.SaveMap(string.Empty, mapData));
        }

        private static MapData CreateSampleMapData(int width, int height)
        {
            var mapData = new MapData(width, height, new MapKeyRef());
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    mapData.SetTerrain(x, y, TestMapKey);
                }
            }

            return mapData;
        }
    }
}