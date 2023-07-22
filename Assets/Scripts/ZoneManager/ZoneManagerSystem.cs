using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

[BurstCompile]
public partial class ZoneManagerSystem : SystemBase
{
    int2 currentDimension;
    [BurstCompile]
    protected override void OnStartRunning()
    {
        CreateZones();
    }
    [BurstCompile]
    protected override void OnUpdate()
    {
        if (currentDimension.x == GameManager.GM.panelSize.x && currentDimension.y != GameManager.GM.panelSize.y)
        {
            ResetZones();
            CreateZones();
        }
    }
    [BurstCompile]
    void CreateZones()
    {
        if (EntityManager.CreateEntityQuery(typeof(ZoneComponent)).CalculateEntityCount() > 0) return;
        EntityCommandBuffer ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.Unmanaged);
        ZoneManagerComponent zm = SystemAPI.GetSingleton<ZoneManagerComponent>();
        ECSSceneManager.CreateZones((coor) =>
        {
            Entity entity = ecb.Instantiate(zm.zonePrefab);
            ecb.SetComponent(entity, new ZoneComponent
            {
                coordinates = coor,
            });
            ecb.SetComponent<LocalTransform>(entity, new LocalTransform
            {
                Position = new float3(coor.x + 0.5f, 0.2f, coor.y + 0.5f),
                Scale = 1f,
                Rotation = quaternion.identity
            });
        });
    }
    [BurstCompile]
    void ResetZones()
    {
        EntityQuery prevZones = EntityManager.CreateEntityQuery(typeof(ZoneComponent));
        EntityManager.DestroyEntity(prevZones);
    }
    [BurstCompile]
    public partial struct CreateZoneJob : IJobEntity
    {
        EntityCommandBuffer ecb;
        Entity prefab;
        int2 coor;
        public void Execute()
        {
            Entity entity = ecb.Instantiate(prefab);
            ecb.SetComponent(entity, new ZoneComponent
            {
                coordinates = coor,
            });
            ecb.SetComponent<LocalTransform>(entity, new LocalTransform
            {
                Position = new float3(coor.x + 0.5f, 0.2f, coor.y + 0.5f),
                Scale = 1f,
                Rotation = quaternion.identity
            });
        }
    }
}
