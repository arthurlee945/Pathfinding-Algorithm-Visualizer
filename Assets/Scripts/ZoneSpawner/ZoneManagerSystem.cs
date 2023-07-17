using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

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
            currentMode = selectedMode;
            CreateZones(selectedMode);
        }
    }
    void CreateZones(ContainerMode mode)
    {
        EntityQuery zoneEntityQuery = EntityManager.CreateEntityQuery(typeof(ZoneTag));
        ZoneManager zoneManager = SystemAPI.GetSingleton<ZoneManager>();
        EntityCommandBuffer entityCommandBuffer =
            SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.Unmanaged);
        if (zoneEntityQuery.CalculateEntityCount() > 10 && currentMode == GameManager.GM.SelectedMode) return;
        currentMode = GameManager.GM.SelectedMode;
        if (currentMode == ContainerMode.Scene2D)
        {
            Entity spawnedZone = entityCommandBuffer.Instantiate(zoneManager.zone2DPrefab);
        }
        else if (currentMode == ContainerMode.Scene3D)
        {

        }
    }
}
