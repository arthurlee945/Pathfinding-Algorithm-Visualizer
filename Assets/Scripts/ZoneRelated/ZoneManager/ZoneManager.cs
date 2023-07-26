using Unity.Entities;
using UnityEngine;


public struct ZoneManagerComponent : IComponentData
{
    public Entity zonePrefab;
}
public class ZoneManager : MonoBehaviour
{
    public GameObject zonePrefab;

    class Baker : Baker<ZoneManager>
    {
        public override void Bake(ZoneManager authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new ZoneManagerComponent
            {
                zonePrefab = GetEntity(authoring.zonePrefab, TransformUsageFlags.Renderable),
            });
        }
    }
}
