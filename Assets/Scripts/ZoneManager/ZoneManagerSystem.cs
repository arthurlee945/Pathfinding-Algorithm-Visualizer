using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;



// change to isystem and implement IJobEntity
[BurstCompile]
public partial class ZoneManagerSystem : SystemBase
{
    ContainerMode currentMode;
    EntityManager entityManager;
    protected override void OnCreate()
    {
        entityManager = new EntityManager();
    }
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
            UnityEngine.Debug.Log("FIre");
            ResetZones(selectedMode);
            currentMode = selectedMode;
            CreateZones(selectedMode);
        }
    }
    [BurstCompile]
    void CreateZones(ContainerMode mode)
    {
        ZoneManager zoneManager = SystemAPI.GetSingleton<ZoneManager>();

        currentMode = GameManager.GM.SelectedMode;

        EntityQuery zoneEntityQuery;
        int neededZones;
        if (currentMode == ContainerMode.Scene2D)
        {
            zoneEntityQuery = EntityManager.CreateEntityQuery(typeof(Zone2D));
            neededZones = GameManager.GM.panel2DSize.x * GameManager.GM.panel2DSize.y;
        }
        else
        {
            zoneEntityQuery = EntityManager.CreateEntityQuery(typeof(Zone3D));
            neededZones = GameManager.GM.panel3DSize.x * GameManager.GM.panel3DSize.y * GameManager.GM.panel3DSize.z;
        }

        if (zoneEntityQuery.CalculateEntityCount() >= neededZones && currentMode == GameManager.GM.SelectedMode) return;

        // EntityCommandBuffer ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.Unmanaged);
        // EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob, PlaybackPolicy.MultiPlayback);

        if (currentMode == ContainerMode.Scene2D)
        {
            int2 currentSize = new int2(GameManager.GM.panel2DSize.x, GameManager.GM.panel2DSize.y);
            for (int x = 0; x < currentSize.x; x++)
            {
                for (int y = 0; y < currentSize.y; y++)
                {

                    Entity spawnedZone = EntityManager.Instantiate(zoneManager.zone2DPrefab);
                    EntityManager.AddComponentData<Zone2D>(spawnedZone, new Zone2D()
                    {
                        coordinates = new int2(x, y),
                    });
                    EntityManager.AddComponentData<LocalTransform>(spawnedZone, new LocalTransform
                    {
                        Position = new float3(x + 0.5f, 0.1f, y + 0.5f),
                        Scale = 1f,
                        Rotation = quaternion.identity
                    });
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
                        Entity spawnedZone = EntityManager.Instantiate(zoneManager.zone3DPrefab);
                        EntityManager.AddComponentData<Zone3D>(spawnedZone, new Zone3D()
                        {
                            coordinates = new int3(x, y, z),
                        });
                        EntityManager.AddComponentData<LocalTransform>(spawnedZone, new LocalTransform
                        {
                            Position = new float3(x + 0.5f, y + 0.5f, z + 0.5f),
                            Scale = 1f,
                            Rotation = quaternion.identity
                        });
                    }
                }
            }
        }

    }
    void ResetZones(ContainerMode selectedMode)
    {
        EntityQuery prevZones;
        if (selectedMode == ContainerMode.Scene2D)
        {
            prevZones = EntityManager.CreateEntityQuery(typeof(Zone3D));
        }
        else
        {
            prevZones = EntityManager.CreateEntityQuery(typeof(Zone2D));
        }
        EntityManager.DestroyEntity(prevZones);
    }
}

