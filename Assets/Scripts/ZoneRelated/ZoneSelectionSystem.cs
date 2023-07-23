using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;
using UnityEngine.InputSystem;
using RaycastHit = Unity.Physics.RaycastHit;

public partial class ZoneSelectionSystem : SystemBase
{
    Camera mainCam;
    PhysicsWorldSingleton buildPhysicsWorld;
    CollisionWorld collisionWorld;
    protected override void OnCreate()
    {
        mainCam = Camera.main;


    }
    protected override void OnUpdate()
    {
        var ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        var rayStart = ray.origin;
        var rayEnd = ray.GetPoint(100f);

        if (Raycast(rayStart, rayEnd, out var hit))
        {
            UnityEngine.Debug.Log(hit.Entity);
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
