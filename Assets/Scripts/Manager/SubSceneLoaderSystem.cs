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

    /*
    void HandleSubSceneLoading(string sceneIdentifier)
    {
        ResetSceneEntities();
        var requests = newRequest.ToComponentDataArray<SubSceneLoader>(Allocator.Temp);

        for (int i = 0; i < requests.Length; i++)
        {
            EntitySceneReference selectedScene = sceneIdentifier == "2D" ? requests[i].scene2DReference : requests[i].scene3DReference;
            EntitySceneReference unloadableScene = sceneIdentifier == "2D" ? requests[i].scene3DReference : requests[i].scene2DReference;
            SceneSystem.LoadSceneAsync(World.Unmanaged, selectedScene);
            // currentSceneEntities.Add(SceneSystem.LoadSceneAsync(World.Unmanaged, selectedScene));
        }
        Debug.Log("Between");
        requests.Dispose();
        EntityManager.DestroyEntity(newRequest);
    }
    void ResetSceneEntities()
    {
        if (currentSceneEntities.Count <= 0) return;
        foreach (Entity sceneE in currentSceneEntities)
        {
            SceneSystem.UnloadScene(World.Unmanaged, sceneE);
        }
    }
    */
}
