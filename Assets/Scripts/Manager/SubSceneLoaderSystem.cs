using Unity.Entities;
using Unity.Scenes;
public partial class SubSceneLoaderSystem : SystemBase
{
    string currentSceneId;
    protected override void OnUpdate()
    {
        string selectedScene = GameManager.GM.SelectedMode;
        if (selectedScene == "2D" && currentSceneId != "2D")
        {
            currentSceneId = selectedScene;
            HandleSubSceneRender(currentSceneId);
        }
        else if (selectedScene == "3D" && currentSceneId != "3D")
        {
            currentSceneId = selectedScene;
            HandleSubSceneRender(currentSceneId);
        }
    }

    void HandleSubSceneRender(string sceneIdentifier)
    {
        Hash128 selectedScene = sceneIdentifier == "2D" ? GameManager.GM.Scene2D.SceneGUID : GameManager.GM.Scene3D.SceneGUID;
        Hash128 unloadableScene = sceneIdentifier == "2D" ? GameManager.GM.Scene3D.SceneGUID : GameManager.GM.Scene2D.SceneGUID;
        SceneSystem.LoadSceneAsync(World.Unmanaged, selectedScene);
        SceneSystem.UnloadScene(World.Unmanaged, unloadableScene);
    }
}
