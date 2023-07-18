using Unity.Entities;
using Unity.Scenes;

public partial class SubSceneLoaderSystem : SystemBase
{
    ContainerMode currentSceneId;
    Entity loadedSceneEntity;
    protected override void OnStartRunning()
    {
        HandleSubSceneRender(currentSceneId);
    }
    protected override void OnUpdate()
    {
        ContainerMode selectedScene = GameManager.GM.SelectedMode;
        if ((selectedScene == ContainerMode.Scene2D && currentSceneId != ContainerMode.Scene2D)
        || (selectedScene == ContainerMode.Scene3D && currentSceneId != ContainerMode.Scene3D))
        {
            currentSceneId = selectedScene;
            HandleSubSceneRender(currentSceneId);
        }
    }

    void HandleSubSceneRender(ContainerMode sceneIdentifier)
    {
        Hash128 selectedScene = sceneIdentifier == ContainerMode.Scene2D ? GameManager.GM.Scene2D.SceneGUID : GameManager.GM.Scene3D.SceneGUID;
        if (loadedSceneEntity != null)
        {
            SceneSystem.UnloadScene(World.Unmanaged, loadedSceneEntity);
        }
        loadedSceneEntity = SceneSystem.LoadSceneAsync(World.Unmanaged, selectedScene);
    }
}
