using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;


public struct ZoneManagerComponent : IComponentData
{
    public Entity zonePrefab;
    public float4 defaultColor, hoverColor, notWalkableColor, exploredColor, pathColor;

}
public class ZoneManager : MonoBehaviour
{
    public GameObject zonePrefab;
    [Header("Initial Colors")]
    public Color defaultColor;
    public Color hoverColor;
    public Color notWalkableColor;
    [Header("Algorithm Colors")]
    public Color exploredColor;
    public Color pathColor;

    class Baker : Baker<ZoneManager>
    {
        public override void Bake(ZoneManager authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new ZoneManagerComponent
            {
                zonePrefab = GetEntity(authoring.zonePrefab, TransformUsageFlags.Renderable),
                defaultColor = new float4(authoring.defaultColor.r, authoring.defaultColor.g, authoring.defaultColor.b, authoring.defaultColor.a),
                hoverColor = new float4(authoring.hoverColor.r, authoring.hoverColor.g, authoring.hoverColor.b, authoring.hoverColor.a),
                notWalkableColor = new float4(authoring.notWalkableColor.r, authoring.notWalkableColor.g, authoring.notWalkableColor.b, authoring.notWalkableColor.a),
                exploredColor = new float4(authoring.exploredColor.r, authoring.exploredColor.g, authoring.exploredColor.b, authoring.exploredColor.a),
                pathColor = new float4(authoring.pathColor.r, authoring.pathColor.g, authoring.pathColor.b, authoring.pathColor.a),
            });
        }
    }
}
