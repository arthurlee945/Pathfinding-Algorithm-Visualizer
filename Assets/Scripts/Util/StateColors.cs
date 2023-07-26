using Unity.Mathematics;
using UnityEngine;

public class StateColors : MonoBehaviour
{

    [Header("Initial Colors")]
    public float4 DefaultColor;
    public float4 HoverColor;
    public float4 NotWalkableColor;
    [Header("Algorithm Colors")]
    public float4 ExploredColor;
    public float4 PathColor;
    [Header("Destination Colors")]
    public float4 StartColor;
    public float4 EndColor;
    public static StateColors Instance { get; private set; }
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
