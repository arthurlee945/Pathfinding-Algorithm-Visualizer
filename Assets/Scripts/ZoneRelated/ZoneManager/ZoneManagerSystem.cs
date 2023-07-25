using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial class ZoneManagerSystem : SystemBase
{
    int2 currentDimension;
    [BurstCompile]
    protected override void OnStartRunning()
    {
        CreateZones();
        currentDimension = new int2(GameManager.GM.panelSize.x, GameManager.GM.panelSize.y);
    }
    [BurstCompile]
    protected override void OnUpdate()
    {
        if (currentDimension.x != GameManager.GM.panelSize.x || currentDimension.y != GameManager.GM.panelSize.y)
        {
            currentDimension = new int2(GameManager.GM.panelSize.x, GameManager.GM.panelSize.y);
            ResetZones();
            CreateZones();
        }
    }
    [BurstCompile]
    void CreateZones()
    {
        if (EntityManager.CreateEntityQuery(typeof(ZoneComponent)).CalculateEntityCount() > 2) return;
        EntityCommandBuffer ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.Unmanaged);
        ZoneManagerComponent zm = SystemAPI.GetSingleton<ZoneManagerComponent>();
        ZoneStore.Instance.CreateZones((coor) =>
        {
            Entity entity = EntityManager.Instantiate(zm.zonePrefab);
            EntityManager.SetComponentData<ZoneComponent>(entity, new ZoneComponent
            {
                coordinates = coor,
                isWalkable = true,
            });
            EntityManager.SetComponentData<LocalTransform>(entity, new LocalTransform
            {
                Position = new float3(coor.x + 0.5f, 0.2f, coor.y + 0.5f),
                Scale = 1f,
                Rotation = quaternion.identity
            });

            return entity;
        });
    }
    [BurstCompile]
    void ResetZones()
    {
        ZoneStore.Instance.Zones.Clear();
        EntityQuery prevZones = EntityManager.CreateEntityQuery(typeof(ZoneComponent));
        EntityManager.DestroyEntity(prevZones);
    }
}

/*
[BurstCompile]
public partial struct CreateZoneJob : IJobEntity
{
    public EntityCommandBuffer ecb;
    public int2 coor;
    private void Execute(ZoneManagerComponent zm)
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
    }
}
*/