using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

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
            Camera.main.transform.position = new Vector3(currentDimension.x / 2, Mathf.Max(currentDimension.x, currentDimension.y), currentDimension.y / 2);
            ResetZones();
            CreateZones();
        }
    }
    [BurstCompile]
    void CreateZones()
    {
        if (EntityManager.CreateEntityQuery(typeof(ZoneComponent)).CalculateEntityCount() > 2) return;
        ZoneManagerComponent zm = SystemAPI.GetSingleton<ZoneManagerComponent>();
        ZoneStore.Instance.CreateZones((coor) =>
        {
            Entity entity = EntityManager.Instantiate(zm.zonePrefab);

            EntityManager.SetComponentData<ZoneComponent>(entity, new ZoneComponent
            {
                coordinates = coor,
                isWalkable = true,
                isStart = new Vector2Int(coor.x, coor.y) == PathFinder.Instance.StartCoors,
                isEnd = new Vector2Int(coor.x, coor.y) == PathFinder.Instance.EndCoors,
                gCost = 0,
                hCost = 0,
            });
            EntityManager.SetComponentData<LocalTransform>(entity, new LocalTransform
            {
                Position = new float3(coor.x + 0.5f, 0.2f, coor.y + 0.5f),
                Scale = 1f,
                Rotation = quaternion.identity
            });

            if (new Vector2Int(coor.x, coor.y) == PathFinder.Instance.StartCoors)
                EntityManager.SetComponentData<URPMaterialPropertyBaseColor>(entity, new URPMaterialPropertyBaseColor
                {
                    Value = StateColors.Instance.StartColor
                });

            if (new Vector2Int(coor.x, coor.y) == PathFinder.Instance.EndCoors)
                EntityManager.SetComponentData<URPMaterialPropertyBaseColor>(entity, new URPMaterialPropertyBaseColor
                {
                    Value = StateColors.Instance.EndColor
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