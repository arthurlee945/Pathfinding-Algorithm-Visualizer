
using System;
using UnityEngine;
using Unity.Mathematics;

public class ECSSceneManager : MonoBehaviour
{
    public static ECSSceneManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public static void CreateZones(Action<int2> action)
    {
        // ContainerMode currentMode = GameManager.GM.SelectedMode;
        int2 currentSize = new int2(GameManager.GM.panelSize.x, GameManager.GM.panelSize.y);
        for (int x = 0; x < currentSize.x; x++)
        {
            for (int y = 0; y < currentSize.y; y++)
            {
                action(new int2(x, y));
            }
        }
    }
}
