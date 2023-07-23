using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

[ExecuteAlways]
public partial class ZoneSelectionSystem : SystemBase
{
    Camera mainCam;
    BuildPhysicsWorld buildPhysicsWorld;
    CollisionWorld collisionWorld;
    protected override void OnCreate()
    {
        mainCam = Camera.main;
        SystemHandle bpw = World.GetOrCreateSystem<BuildPhysicsWorld>();
        // buildPhysicsWorld = World.DefaultGameObjectInjectionWorld.GetExistingSystem<BuildPhysicsWorld>();

    }
    protected override void OnUpdate()
    {
        // collisionWorld
    }

}
