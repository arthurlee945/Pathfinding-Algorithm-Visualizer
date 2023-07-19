using Unity.Entities;
using UnityEngine;

public struct PlaygroundLoader : IComponentData
{
    public Entity playground2D;
    public Entity playground3D;
}

public class PlaygroundLoaderAuthoring : MonoBehaviour
{
    public GameObject playground2D;
    public GameObject playground3D;
    class Baker : Baker<PlaygroundLoaderAuthoring>
    {
        public override void Bake(PlaygroundLoaderAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new PlaygroundLoader
            {
                playground2D = GetEntity(authoring.playground2D, TransformUsageFlags.Renderable),
                playground3D = GetEntity(authoring.playground3D, TransformUsageFlags.Renderable)
            });
        }
    }
}
