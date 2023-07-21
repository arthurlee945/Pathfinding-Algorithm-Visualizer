using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;


public struct ZoneManagerComponent : IComponentData
{
    public Entity zone2DPrefab;
    public Entity zone3DPrefab;

}
public class ZoneManager : MonoBehaviour
{
    public GameObject zone2DPrefab;
    public GameObject zone3DPrefab;

    class Baker : Baker<ZoneManager>
    {
        public override void Bake(ZoneManager authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new ZoneManagerComponent
            {
                zone2DPrefab = GetEntity(authoring.zone2DPrefab, TransformUsageFlags.Renderable),
                zone3DPrefab = GetEntity(authoring.zone3DPrefab, TransformUsageFlags.Renderable),
            });
        }
    }
}
