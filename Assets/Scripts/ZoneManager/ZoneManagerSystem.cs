using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

[BurstCompile]
public partial class ZoneManagerSystem : SystemBase
{
    ContainerMode currentMode;
    [BurstCompile]
    protected override void OnStartRunning()
    {
        CreateZones();
    }
    [BurstCompile]
    protected override void OnUpdate()
    {
        ContainerMode selectedMode = GameManager.GM.SelectedMode;
        if ((selectedMode == ContainerMode.Scene2D && currentMode != ContainerMode.Scene2D)
        || (selectedMode == ContainerMode.Scene3D && currentMode != ContainerMode.Scene3D))
        {
            ResetZones();
            currentMode = selectedMode;
            CreateZones();
        }
    }
    void CreateZones()
    {
        if (EntityManager.CreateEntityQuery(typeof(ZoneComponent)).CalculateEntityCount() > 0) return;
        EntityCommandBuffer ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.Unmanaged);
        ZoneManagerComponent zm = SystemAPI.GetSingleton<ZoneManagerComponent>();
        ECSSceneManager.CreateZones((coor) =>
        {
            Entity prefab = currentMode == ContainerMode.Scene2D ? zm.zone2DPrefab : zm.zone3DPrefab;
            Entity entity = ecb.Instantiate(prefab);
            ecb.SetComponent(entity, new ZoneComponent
            {
                coordinates = coor,
            });
            ecb.SetComponent<LocalTransform>(entity, new LocalTransform
            {
                Position = currentMode == ContainerMode.Scene2D ? new float3(coor.x + 0.5f, 0.2f, coor.y + 0.5f) : new float3(coor.x + 0.5f, coor.y + 0.5f, coor.z + 0.5f),
                Scale = 1f,
                Rotation = quaternion.identity
            });
        });
    }
    void ResetZones()
    {
        EntityQuery prevZones = EntityManager.CreateEntityQuery(typeof(ZoneComponent));
        EntityManager.DestroyEntity(prevZones);
    }

}
