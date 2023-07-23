using System.Collections;
using System.Collections.Generic;
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
    BuildPhysicsWorld buildPhysicsWorld;
    CollisionWorld collisionWorld;
    protected override void OnCreate()
    {
        mainCam = Camera.main;
        // SystemHandle bpw = World.GetOrCreateSystem<BuildPhysicsWorld>();
        // buildPhysicsWorld = new BuildPhysicsWorld().;
        // World.DefaultGameObjectInjectionWorld.GetExistingSystem<CollisionWorld>();
        // new CollisionWorld().
        collisionWorld = new CollisionWorld();

    }
    protected override void OnUpdate()
    {
        var ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        var rayStart = ray.origin;
        var rayEnd = ray.GetPoint(100f);

    }
    // private bool Raycast(float3 rayStart, float3 rayEnd, out RaycastHit raycastHit)
    // {

    // }
}
