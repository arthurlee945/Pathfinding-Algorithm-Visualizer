
using System;
using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }
    public static Dictionary<Vector2Int, Entity> Zones = new Dictionary<Vector2Int, Entity>();
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
    void OnDestroy()
    {
        Zones.Clear();
    }
    public static void CreateZones(Func<int2, Entity> action)
    {
        int2 currentSize = new int2(GameManager.GM.panelSize.x, GameManager.GM.panelSize.y);
        for (int x = 0; x < currentSize.x; x++)
        {
            for (int y = 0; y < currentSize.y; y++)
            {
                Entity newZone = action(new int2(x, y));
                Zones.Add(new Vector2Int(x, y), newZone);
            }
        }
    }
}
