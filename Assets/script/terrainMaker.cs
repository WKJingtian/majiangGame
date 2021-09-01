using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class terrainMaker : MonoBehaviour
{
    public int width;
    public int height;
    public int depth;
    public float scaler;
    Terrain ter;
    private void Start()
    {
        ter = GetComponent<Terrain>();
        ter.terrainData = generateTer(ter.terrainData);
    }

    TerrainData generateTer(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, depth, height);
        float[,] heights = new float [width,height];
        for (int x = 0; x < width; x++) for (int y = 0; y < height; y++)
                heights[x, y] = Mathf.PerlinNoise((float)x / width * scaler, (float)y / height * scaler);
        terrainData.SetHeights(0, 0, heights);
        return terrainData;
    }
}
