namespace GeoMapLib;

public class MapData
{
    private readonly MapKey?[,] _terrainMap;
    public readonly int Width;
    public readonly int Height;
    public readonly MapKeyRef MapKeyRef;

    public MapData(int x, int y, MapKeyRef mapKeyRef)
    {
        _terrainMap = new MapKey[x, y];
        Width = x;
        Height = y;
        MapKeyRef = mapKeyRef;
    }
    
    public MapKey GetTerrainAt(int x, int y)
    {
        var terrain = _terrainMap[x, y];
        if (terrain == null) 
        {
            terrain = new MapKey();
            _terrainMap[x, y] = terrain;
        }
        return terrain; 
    }

    public void SetTerrain(int x, int y, MapKey? terrainType)
    {
        _terrainMap[x, y] = terrainType;
    }
}