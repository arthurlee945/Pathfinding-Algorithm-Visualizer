// using System.Collections.Generic;
// using Unity.Burst;
// using Unity.Collections;
// using Unity.Entities;
// using Unity.Mathematics;
// using Unity.Transforms;

// [BurstCompile]
// public partial struct ZoneManagerISystem : ISystem
// {
//     private ContainerMode currentMode;
//     int x;
//     [BurstCompile]
//     public void OnCreate(ref SystemState state)
//     {
//         var ecbSingleton = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();
//     }
//     [BurstCompile]
//     public void OnStartRunning(ref SystemState state)
//     {
//         UnityEngine.Debug.Log("debug");
//         ZoneManager zm = SystemAPI.GetSingleton<ZoneManager>();
//         var ecbSingleton = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

//         EntityQuery zoneEntityQuery;
//         int neededZones;
//         if (currentMode == ContainerMode.Scene2D)
//         {
//             EntityQueryBuilder eqb = new EntityQueryBuilder().WithAll<Zone2D>();
//             zoneEntityQuery = state.GetEntityQuery(eqb);
//             neededZones = GameManager.GM.panel2DSize.x * GameManager.GM.panel2DSize.y;
//         }
//         else
//         {
//             EntityQueryBuilder eqb = new EntityQueryBuilder().WithAll<Zone3D>();
//             zoneEntityQuery = state.GetEntityQuery(eqb);
//             neededZones = GameManager.GM.panel3DSize.x * GameManager.GM.panel3DSize.y * GameManager.GM.panel3DSize.z;
//         }

//         if (zoneEntityQuery.CalculateEntityCount() >= neededZones && currentMode == GameManager.GM.SelectedMode) return;

//         int3 currentSize = currentMode == ContainerMode.Scene2D ?
//           new int3(GameManager.GM.panel2DSize.x, GameManager.GM.panel2DSize.y, 0) :
//           new int3(GameManager.GM.panel3DSize.x, GameManager.GM.panel3DSize.y, GameManager.GM.panel3DSize.z);

//         for (int x = 0; x < currentSize.x; x++)
//         {
//             for (int y = 0; y < currentSize.y; y++)
//             {
//                 if (currentMode == ContainerMode.Scene3D)
//                 {
//                     for (int z = 0; z < currentSize.z; z++)
//                     {
//                         new CreateZoneJob
//                         {
//                             currentMode = currentMode,
//                             coor = new int3(x, y, z),
//                             entityManager = state.EntityManager,
//                             zonePrefab = zm.zone3DPrefab
//                         }.ScheduleParallel(state.Dependency);
//                     }
//                 }
//                 else
//                 {
//                     new CreateZoneJob
//                     {
//                         currentMode = currentMode,
//                         coor = new int3(x, y, 0),
//                         entityManager = state.EntityManager,
//                         zonePrefab = zm.zone2DPrefab
//                     }.ScheduleParallel(state.Dependency);

//                 }
//             }
//         }
//     }
//     [BurstCompile]
//     public void OnUpdate(ref SystemState state)
//     {
//         x++;
//     }
//     [BurstCompile]
//     public void OnDestroy(ref SystemState state)
//     {

//     }
//     // [BurstCompile]
//     // public partial struct CreateZonesJob : IJobEntity
//     // {
//     //     public ContainerMode currentMode;
//     //     public EntityCommandBuffer entityManager;

//     //     public void Execute(ZoneManager zoneManager)
//     //     {

//     //         EntityQuery zoneEntityQuery;
//     //         int neededZones;
//     //         if (currentMode == ContainerMode.Scene2D)
//     //         {
//     //             EntityQueryBuilder eqb = new EntityQueryBuilder().WithAll<Zone2D>();
//     //             zoneEntityQuery = entityManager.CreateEntityQuery(typeof(Zone2D));
//     //             neededZones = GameManager.GM.panel2DSize.x * GameManager.GM.panel2DSize.y;
//     //         }
//     //         else
//     //         {
//     //             zoneEntityQuery = entityManager.CreateEntityQuery(typeof(Zone3D));
//     //             neededZones = GameManager.GM.panel3DSize.x * GameManager.GM.panel3DSize.y * GameManager.GM.panel3DSize.z;
//     //         }

//     //         if (zoneEntityQuery.CalculateEntityCount() >= neededZones && currentMode == GameManager.GM.SelectedMode) return;

//     //         int3 currentSize = currentMode == ContainerMode.Scene2D ?
//     //           new int3(GameManager.GM.panel2DSize.x, GameManager.GM.panel2DSize.y, 0) :
//     //           new int3(GameManager.GM.panel3DSize.x, GameManager.GM.panel3DSize.y, GameManager.GM.panel3DSize.z);

//     //         for (int x = 0; x < currentSize.x; x++)
//     //         {
//     //             for (int y = 0; y < currentSize.y; y++)
//     //             {
//     //                 if (currentMode == ContainerMode.Scene3D)
//     //                 {
//     //                     for (int z = 0; z < currentSize.z; z++)
//     //                     {
//     //                         new CreateZoneJob
//     //                         {
//     //                             currentMode = currentMode,
//     //                             coor = new int3(x, y, z),
//     //                             entityManager = entityManager,
//     //                             zonePrefab = zoneManager.zone3DPrefab
//     //                         }.ScheduleParallel();
//     //                     }
//     //                 }
//     //                 else
//     //                 {
//     //                     new CreateZoneJob
//     //                     {
//     //                         currentMode = currentMode,
//     //                         coor = new int3(x, y, 0),
//     //                         entityManager = entityManager,
//     //                         zonePrefab = zoneManager.zone2DPrefab
//     //                     }.ScheduleParallel();

//     //                 }
//     //             }
//     //         }
//     //     }
//     // }

//     [BurstCompile]
//     public partial struct CreateZoneJob : IJobEntity
//     {
//         public int3 coor;
//         public Entity zonePrefab;
//         public EntityManager entityManager;
//         public ContainerMode currentMode;
//         public void Execute()
//         {
//             UnityEngine.Debug.Log("HIOSDIGHOSDHGOSIHDGIOS");
//             Entity spawnedZone = entityManager.Instantiate(zonePrefab);
//             if (currentMode == ContainerMode.Scene2D)
//             {
//                 // entityManager.SetComponentD
//                 entityManager.SetComponentData<Zone2D>(spawnedZone, new Zone2D()
//                 {
//                     coordinates = new int2(coor.x, coor.y),
//                 });
//             }
//             else
//             {
//                 entityManager.SetComponentData<Zone3D>(spawnedZone, new Zone3D()
//                 {
//                     coordinates = coor,
//                 });
//             }
//             entityManager.SetComponentData<LocalTransform>(spawnedZone, new LocalTransform
//             {
//                 Position = currentMode == ContainerMode.Scene2D ?
//                 new float3(coor.x + 0.5f, 0.1f, coor.y + 0.5f) : new float3(coor.x + 0.5f, coor.y + 0.5f, coor.z + 0.5f),
//                 Scale = 1f,
//                 Rotation = quaternion.identity
//             });
//         }
//     }

// }

