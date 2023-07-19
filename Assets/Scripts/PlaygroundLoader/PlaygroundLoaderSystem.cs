using Unity.Entities;

public partial class PlaygroundLoaderSystem : SystemBase
{
    ContainerMode currentSceneId;
    Entity loadedPlayground;
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
        PlaygroundLoader zoneManager = SystemAPI.GetSingleton<PlaygroundLoader>();
        if (loadedPlayground != Entity.Null)
        {
            EntityManager.DestroyEntity(loadedPlayground);
        }
        loadedPlayground = EntityManager.Instantiate(sceneIdentifier == ContainerMode.Scene2D ? zoneManager.playground2D : zoneManager.playground3D);
    }
}
