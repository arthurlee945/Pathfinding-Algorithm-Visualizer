
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

    public static void CreateZones(Action<int3> action)
    {
        ContainerMode currentMode = GameManager.GM.SelectedMode;
        int3 currentSize = currentMode == ContainerMode.Scene2D ?
        new int3(GameManager.GM.panel2DSize.x, GameManager.GM.panel2DSize.y, 0) :
        new int3(GameManager.GM.panel3DSize.x, GameManager.GM.panel3DSize.y, GameManager.GM.panel3DSize.z);

        for (int x = 0; x < currentSize.x; x++)
        {
            for (int y = 0; y < currentSize.y; y++)
            {
                if (currentMode == ContainerMode.Scene3D)
                {
                    for (int z = 0; z < currentSize.z; z++)
                    {
                        action(new int3(x, y, z));
                    }
                }
                else
                {
                    action(new int3(x, y, 0));
                }
            }
        }
    }
}
