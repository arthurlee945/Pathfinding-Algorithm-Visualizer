
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
    Entity prevDestinationZone;
    bool isStartSwitching, isEndSwitching;
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
        if (PathFinder.Instance.IsRunning || PathFinder.Instance.IsPreview) return;
        //------ add guard clause for when algo playing
        var ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        var rayStart = ray.origin;
        var rayEnd = ray.GetPoint(500f);

        if (Raycast(rayStart, rayEnd, out var hit))
        {
            ZoneComponent zc = EntityManager.GetComponentData<ZoneComponent>(hit.Entity);
            if (Mouse.current.leftButton.isPressed)
            {
                if (zc.isStart || isStartSwitching)
                {
                    if (Mouse.current.leftButton.wasPressedThisFrame) isStartSwitching = true;
                    SetDestinationZone(hit.Entity, zc, true);
                    return;
                }
                if (zc.isEnd || isEndSwitching)
                {
                    if (Mouse.current.leftButton.wasPressedThisFrame) isEndSwitching = true;
                    SetDestinationZone(hit.Entity, zc, false);
                    return;
                }
                if (pressedZone == hit.Entity || zc.isEnd || zc.isStart) return;
                zc.isWalkable = !zc.isWalkable;
                EntityManager.SetComponentData<ZoneComponent>(hit.Entity, zc);
                URPMaterialPropertyBaseColor clickedBC = EntityManager.GetComponentData<URPMaterialPropertyBaseColor>(hit.Entity);
                clickedBC.Value = zc.isWalkable ? StateColors.Instance.DefaultColor : StateColors.Instance.NotWalkableColor;
                EntityManager.SetComponentData<URPMaterialPropertyBaseColor>(hit.Entity, clickedBC);
                pressedZone = hit.Entity;
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame && (isStartSwitching || isEndSwitching))
            {
                isEndSwitching = false;
                isStartSwitching = false;
            }
            else
            {
                if (zc.isEnd || zc.isStart) return;
                if (hoveredZone != hit.Entity && hoveredZone != Entity.Null && EntityManager.GetComponentData<ZoneComponent>(hoveredZone).isWalkable)
                {
                    URPMaterialPropertyBaseColor prevBc = EntityManager.GetComponentData<URPMaterialPropertyBaseColor>(hoveredZone);
                    prevBc.Value = StateColors.Instance.DefaultColor;
                    EntityManager.SetComponentData<URPMaterialPropertyBaseColor>(hoveredZone, prevBc);
                }
                if (!zc.isWalkable || hoveredZone == hit.Entity) return;
                URPMaterialPropertyBaseColor newBc = EntityManager.GetComponentData<URPMaterialPropertyBaseColor>(hit.Entity);
                newBc.Value = StateColors.Instance.HoverColor;
                EntityManager.SetComponentData<URPMaterialPropertyBaseColor>(hit.Entity, newBc);
                hoveredZone = hit.Entity;
            }
        }
        else
        {
            isStartSwitching = false;
            isEndSwitching = false;
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
    void SetDestinationZone(Entity currentZone, ZoneComponent zc, bool forStart)
    {
        if ((forStart && zc.isEnd) || (!forStart && zc.isStart)) return;
        if ((zc.isStart && forStart) || (zc.isEnd && !forStart))
        {
            prevDestinationZone = currentZone;
            return;
        };
        if (prevDestinationZone != Entity.Null && prevDestinationZone != currentZone)
        {
            ZoneComponent prevZC = EntityManager.GetComponentData<ZoneComponent>(prevDestinationZone);
            prevZC.isStart = false;
            prevZC.isEnd = false;
            EntityManager.SetComponentData<ZoneComponent>(prevDestinationZone, prevZC);
            URPMaterialPropertyBaseColor prevBc = EntityManager.GetComponentData<URPMaterialPropertyBaseColor>(prevDestinationZone);
            prevBc.Value = StateColors.Instance.DefaultColor;
            EntityManager.SetComponentData<URPMaterialPropertyBaseColor>(prevDestinationZone, prevBc);
        }
        if (forStart)
        {
            zc.isStart = true;
            PathFinder.Instance.StartCoors = new Vector2Int(zc.coordinates.x, zc.coordinates.y);
        }
        else
        {
            zc.isEnd = true;
            PathFinder.Instance.EndCoors = new Vector2Int(zc.coordinates.x, zc.coordinates.y);
        }
        zc.isWalkable = true;
        EntityManager.SetComponentData<ZoneComponent>(currentZone, zc);
        URPMaterialPropertyBaseColor baseColor = EntityManager.GetComponentData<URPMaterialPropertyBaseColor>(currentZone);
        baseColor.Value = forStart ? StateColors.Instance.StartColor : StateColors.Instance.EndColor;
        EntityManager.SetComponentData<URPMaterialPropertyBaseColor>(currentZone, baseColor);
        prevDestinationZone = currentZone;
    }
}
