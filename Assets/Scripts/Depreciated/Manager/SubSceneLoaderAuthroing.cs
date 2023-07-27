// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Unity.Entities;
// using Unity.Entities.Serialization;

// public struct SubSceneLoader : IComponentData
// {
//     public EntitySceneReference scene2DReference;
//     public EntitySceneReference scene3DReference;
// }

// public class SubSceneLoaderAuthroing : MonoBehaviour
// {
//     public UnityEditor.SceneAsset scene2D;
//     public UnityEditor.SceneAsset scene3D;
//     class Baker : Baker<SubSceneLoaderAuthroing>
//     {
//         public override void Bake(SubSceneLoaderAuthroing authoring)
//         {
//             Entity entity = GetEntity(TransformUsageFlags.None);
//             AddComponent(entity, new SubSceneLoader
//             {
//                 scene2DReference = new EntitySceneReference(authoring.scene2D),
//                 scene3DReference = new EntitySceneReference(authoring.scene3D),
//             });
//         }
//     }
// }
