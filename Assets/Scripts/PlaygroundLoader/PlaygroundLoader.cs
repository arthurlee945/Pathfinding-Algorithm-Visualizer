using Unity.Entities;
using UnityEngine;

public struct PlaygroundLoaderComponent : IComponentData
{
    public Entity playground2D;
    public Entity playground3D;
}

public class PlaygroundLoader : MonoBehaviour
{
    public GameObject playground2D;
    public GameObject playground3D;
    class Baker : Baker<PlaygroundLoader>
    {
        public override void Bake(PlaygroundLoader authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new PlaygroundLoaderComponent
            {
                playground2D = GetEntity(authoring.playground2D, TransformUsageFlags.Renderable),
                playground3D = GetEntity(authoring.playground3D, TransformUsageFlags.Renderable)
            });
        }
    }
}
