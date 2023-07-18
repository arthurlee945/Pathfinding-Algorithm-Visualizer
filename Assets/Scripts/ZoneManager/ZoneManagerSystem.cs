using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial class ZoneManagerSystem : SystemBase
{
    ContainerMode currentMode;

    [BurstCompile]
    protected override void OnStartRunning()
    {
        CreateZones(currentMode);
    }

    [BurstCompile]
    protected override void OnUpdate()
    {
        ContainerMode selectedMode = GameManager.GM.SelectedMode;
        if ((selectedMode == ContainerMode.Scene2D && currentMode != ContainerMode.Scene2D)
        || (selectedMode == ContainerMode.Scene3D && currentMode != ContainerMode.Scene3D))
        {
            ResetZones(selectedMode);
            currentMode = selectedMode;
            CreateZones(selectedMode);
        }
    }
    [BurstCompile]
    void CreateZones(ContainerMode mode)
    {
        EntityQuery zoneEntityQuery = EntityManager.CreateEntityQuery(typeof(Zone));
        ZoneManager zoneManager = SystemAPI.GetSingleton<ZoneManager>();
        currentMode = GameManager.GM.SelectedMode;
        int neededZones = currentMode == ContainerMode.Scene2D ? GameManager.GM.panel2DSize.x * GameManager.GM.panel2DSize.y
        : GameManager.GM.panel3DSize.x * GameManager.GM.panel3DSize.y * GameManager.GM.panel3DSize.z;
        if (zoneEntityQuery.CalculateEntityCount() >= neededZones && currentMode == GameManager.GM.SelectedMode) return;

        EntityCommandBuffer ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.Unmanaged);

        if (currentMode == ContainerMode.Scene2D)
        {
            int2 currentSize = new int2(GameManager.GM.panel2DSize.x, GameManager.GM.panel2DSize.y);
            for (int x = 0; x < currentSize.x; x++)
            {
                for (int y = 0; y < currentSize.y; y++)
                {
                    Entity spawnedZone = ecb.Instantiate(zoneManager.zone2DPrefab);
                    // Zone zoneData = EntityManager.GetComponentData<Zone>(spawnedZone);
                    // zoneData.coordinates = new int2(x, y);
                    LocalToWorld ltw = EntityManager.GetComponentData<LocalToWorld>(spawnedZone);
                    // float4x4 newPos = new float4x4(ltw.Value.Rotation(), new float3(x, 0.2f, y));
                    // ecb.SetComponent(spawnedZone, new LocalToWorld()
                    // {
                    //     Value = newPos
                    // });
                    // EntityManager.SetComponentData<LocalToWorld>(spawnedZone, new LocalToWorld()
                    // {
                    //     Value = newPos
                    // });

                }
            }
        }
        else if (currentMode == ContainerMode.Scene3D)
        {
            int3 currentSize = new int3(GameManager.GM.panel3DSize.x, GameManager.GM.panel3DSize.y, GameManager.GM.panel3DSize.z);

            for (int x = 0; x < currentSize.x; x++)
            {
                for (int y = 0; y < currentSize.y; y++)
                {
                    for (int z = 0; z < currentSize.z; z++)
                    {
                        // Entity spawnedZone = entityCommandBuffer.Instantiate(zoneManager.zone2DPrefab);
                        // spawnedZones.Add(spawnedZone);
                        // Zone zoneData = EntityManager.GetComponentData<Zone>(spawnedZone);
                    }
                }
            }
        }

    }
    void ResetZones(ContainerMode selectedMode)
    {
        // EntityCommandBuffer ecb =
        //     SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.Unmanaged);
        // ZoneManager zoneManager = SystemAPI.GetSingleton<ZoneManager>();

        // zoneManager.zones.Clear();
    }
}


// public partial struct InstantiateZoneJob : IJobEntity
// {
//     public float DeltaTime;
//     public int2 coordinates;
//     private void Execute()
//     {

//     }
// }