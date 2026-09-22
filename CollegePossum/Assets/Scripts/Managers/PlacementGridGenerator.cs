using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementGridGenerator : MonoBehaviour
{
    public GameObject nodePrefab;
    public int rows = 6;
    public int columns = 7;
    public float spacing = 0;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = -13; x < columns-13; x++)
        {
            for (int y = -6; y < rows-6; y++)
            {
                Vector2 position = new Vector2(
                    x * spacing,
                    y * spacing
                );

                Instantiate(nodePrefab, position, Quaternion.identity, transform);
            }
        }
    }
}
