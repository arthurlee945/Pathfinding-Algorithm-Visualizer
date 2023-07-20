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

        // NativeArray<Entity> zones = new NativeArray<Entity>(neededZones, Allocator.Temp);

        int3 currentSize = currentMode == ContainerMode.Scene2D ?
            new int3(GameManager.GM.panel2DSize.x, GameManager.GM.panel2DSize.y, 0) :
            new int3(GameManager.GM.panel3DSize.x, GameManager.GM.panel3DSize.y, GameManager.GM.panel3DSize.z);
        EntityCommandBuffer ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.Unmanaged);

        for (int x = 0; x < currentSize.x; x++)
        {
            for (int y = 0; y < currentSize.y; y++)
            {

                if (currentMode == ContainerMode.Scene3D)
                {

                    for (int z = 0; z < currentSize.z; z++)
                    {
                        // new CreateZoneJob
                        // {
                        //     currentMode = currentMode,
                        //     coor = new int3(x, y, z),
                        //     entityManager = EntityManager,
                        //     zonePrefab = zoneManager.zone3DPrefab
                        // }.Run();

                        Entity spawnedZone = ecb.Instantiate(zoneManager.zone3DPrefab);
                        ecb.SetComponent<Zone3D>(spawnedZone, new Zone3D()
                        {
                            coordinates = new int3(x, y, z),
                        });
                        ecb.SetComponent<LocalTransform>(spawnedZone, new LocalTransform
                        {
                            Position = new float3(x + 0.5f, y + 0.5f, z + 0.5f),
                            Scale = 1f,
                            Rotation = quaternion.identity
                        });
                    }
                }
                else
                {
                    Entity spawnedZone = ecb.Instantiate(zoneManager.zone2DPrefab);
                    ecb.SetComponent<Zone2D>(spawnedZone, new Zone2D()
                    {
                        coordinates = new int2(x, y),
                    });
                    ecb.SetComponent<LocalTransform>(spawnedZone, new LocalTransform
                    {
                        Position = new float3(x + 0.5f, 0.1f, y + 0.5f),
                        Scale = 1f,
                        Rotation = quaternion.identity
                    });
                }
            }
        }
        // zones.Dispose();
    }

    [BurstCompile]
    public partial struct CreateZoneJob : IJobEntity
    {
        public int3 coor;
        public Entity zonePrefab;
        public ContainerMode currentMode;
        public void Execute(ZoneManager zoneManger)
        {
            UnityEngine.Debug.Log("EGILSEGHLESIG");
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            Entity spawnedZone = ecb.Instantiate(zonePrefab);
            if (currentMode == ContainerMode.Scene2D)
            {
                ecb.AddComponent<Zone2D>(spawnedZone, new Zone2D()
                {
                    coordinates = new int2(coor.x, coor.y),
                });
            }
            else
            {
                ecb.AddComponent<Zone3D>(spawnedZone, new Zone3D()
                {
                    coordinates = coor,
                });
            }
            ecb.AddComponent<LocalTransform>(spawnedZone, new LocalTransform
            {
                Position = currentMode == ContainerMode.Scene2D ?
                new float3(coor.x + 0.5f, 0.1f, coor.y + 0.5f) : new float3(coor.x + 0.5f, coor.y + 0.5f, coor.z + 0.5f),
                Scale = 1f,
                Rotation = quaternion.identity
            });
            ecb.Dispose();
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


