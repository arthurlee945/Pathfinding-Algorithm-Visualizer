using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

}
