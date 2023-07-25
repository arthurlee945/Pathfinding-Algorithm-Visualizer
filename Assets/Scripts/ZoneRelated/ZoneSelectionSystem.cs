
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using RaycastHit = Unity.Physics.RaycastHit;

public partial class ZoneSelectionSystem : SystemBase
{
    Camera mainCam;
    PhysicsWorldSingleton buildPhysicsWorld;
    CollisionWorld collisionWorld;
    ZoneManagerComponent zoneManager;
    Entity hoveredZone;
    Entity pressedZone;
    protected override void OnCreate()
    {
        mainCam = Camera.main;
    }
    protected override void OnStartRunning()
    {
        zoneManager = SystemAPI.GetSingleton<ZoneManagerComponent>();
    }
    protected override void OnUpdate()
    {
        //------ add guard clause for when algo playing
        var ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        var rayStart = ray.origin;
        var rayEnd = ray.GetPoint(500f);

        if (Raycast(rayStart, rayEnd, out var hit))
        {
            ZoneComponent zc = EntityManager.GetComponentData<ZoneComponent>(hit.Entity);
            if (Mouse.current.leftButton.isPressed)
            {
                if (pressedZone == hit.Entity) return;
                zc.isWalkable = !zc.isWalkable;
                EntityManager.SetComponentData<ZoneComponent>(hit.Entity, zc);
                URPMaterialPropertyBaseColor clickedBC = EntityManager.GetComponentData<URPMaterialPropertyBaseColor>(hit.Entity);
                clickedBC.Value = zc.isWalkable ? zoneManager.defaultColor : zoneManager.notWalkableColor;
                EntityManager.SetComponentData<URPMaterialPropertyBaseColor>(hit.Entity, clickedBC);
                pressedZone = hit.Entity;
            }
            else
            {

                if (hoveredZone != hit.Entity && hoveredZone != Entity.Null && EntityManager.GetComponentData<ZoneComponent>(hoveredZone).isWalkable)
                {
                    URPMaterialPropertyBaseColor prevBc = EntityManager.GetComponentData<URPMaterialPropertyBaseColor>(hoveredZone);
                    prevBc.Value = zoneManager.defaultColor;
                    EntityManager.SetComponentData<URPMaterialPropertyBaseColor>(hoveredZone, prevBc);
                }
                if (!zc.isWalkable || hoveredZone == hit.Entity) return;
                URPMaterialPropertyBaseColor newBc = EntityManager.GetComponentData<URPMaterialPropertyBaseColor>(hit.Entity);
                newBc.Value = zoneManager.hoverColor;
                EntityManager.SetComponentData<URPMaterialPropertyBaseColor>(hit.Entity, newBc);
                hoveredZone = hit.Entity;
            }
        }
        else if (hoveredZone != Entity.Null || pressedZone != Entity.Null)
        {
            pressedZone = Entity.Null;
            hoveredZone = Entity.Null;
        }
    }

    private bool Raycast(float3 RayFrom, float3 RayTo, out RaycastHit hit)
    {
        EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp).WithAll<PhysicsWorldSingleton>();
        EntityQuery singletonQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(builder);
        var collisionWorld = singletonQuery.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;
        singletonQuery.Dispose();

        RaycastInput input = new RaycastInput()
        {
            Start = RayFrom,
            End = RayTo,
            Filter = new CollisionFilter()
            {
                BelongsTo = (uint)CollisionLayers.Zones,
                CollidesWith = ~0u,
            }
        };
        return collisionWorld.CastRay(input, out hit);
    }
}
