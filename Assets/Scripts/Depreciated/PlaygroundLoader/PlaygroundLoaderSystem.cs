// using Unity.Entities;

// public partial class PlaygroundLoaderSystem : SystemBase
// {
//     ContainerMode currentSceneId;
//     Entity loadedPlayground;
//     protected override void OnStartRunning()
//     {
//         HandleSubSceneRender(currentSceneId);
//     }
//     protected override void OnUpdate()
//     {
//         ContainerMode selectedScene = GameManager.GM.SelectedMode;
//         if ((selectedScene == ContainerMode.Scene2D && currentSceneId != ContainerMode.Scene2D)
//         || (selectedScene == ContainerMode.Scene3D && currentSceneId != ContainerMode.Scene3D))
//         {
//             currentSceneId = selectedScene;
//             HandleSubSceneRender(currentSceneId);
//         }
//     }

//     void HandleSubSceneRender(ContainerMode sceneIdentifier)
//     {
//         PlaygroundLoaderComponent pgLoader = SystemAPI.GetSingleton<PlaygroundLoaderComponent>();
//         if (loadedPlayground != Entity.Null)
//         {
//             EntityManager.DestroyEntity(loadedPlayground);
//         }
//         loadedPlayground = EntityManager.Instantiate(sceneIdentifier == ContainerMode.Scene2D ? pgLoader.playground2D : pgLoader.playground3D);
//     }
// }
