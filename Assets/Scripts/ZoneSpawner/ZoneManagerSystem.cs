using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

public partial class ZoneManagerSystem : SystemBase
{
    ContainerMode currentMode;
    protected override void OnStartRunning()
    {
        CreateZones(currentMode);
    }
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
    void CreateZones(ContainerMode mode)
    {
        EntityQuery zoneEntityQuery = EntityManager.CreateEntityQuery(typeof(Zone));
        ZoneManager zoneManager = SystemAPI.GetSingleton<ZoneManager>();
        EntityCommandBuffer entityCommandBuffer =
            SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.Unmanaged);
        if (zoneEntityQuery.CalculateEntityCount() > 10 && currentMode == GameManager.GM.SelectedMode) return;
        currentMode = GameManager.GM.SelectedMode;
        if (currentMode == ContainerMode.Scene2D)
        {
            int2 currentSize = new int2(GameManager.GM.panel2DSize.x, GameManager.GM.panel2DSize.y);
            for (int x = 0; x < currentSize.x; x++)
            {
                for (int y = 0; y < currentSize.y; y++)
                {
                    Entity spawnedZone = entityCommandBuffer.Instantiate(zoneManager.zone2DPrefab);
                    Zone zoneData = EntityManager.GetComponentData<Zone>(spawnedZone);
                    zoneData.coordinates = new int2(x, y);
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
        EntityCommandBuffer entityCommandBuffer =
            SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.Unmanaged);
        ZoneManager zoneManager = SystemAPI.GetSingleton<ZoneManager>();
        NativeArray<Entity> zoneEntities = zoneManager.zones.GetValueArray(Allocator.Temp);
        entityCommandBuffer.DestroyEntity(zoneEntities);
        zoneManager.zones.Clear();
        if (selectedMode == ContainerMode.Scene2D)
        {
            zoneManager.mode3DStart = new int3(0, 0, 0);
            zoneManager.mode3DEnd = new int3(GameManager.GM.panel3DSize.x, GameManager.GM.panel3DSize.y, GameManager.GM.panel3DSize.z);
        }
        else
        {
            zoneManager.mode2DStart = new int2(0, 0);
            zoneManager.mode2DEnd = new int2(GameManager.GM.panel2DSize.x, GameManager.GM.panel2DSize.y);
        }
    }
}
