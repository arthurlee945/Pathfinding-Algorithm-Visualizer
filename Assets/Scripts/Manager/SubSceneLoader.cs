using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Scenes;

public partial class SubSceneLoader : SystemBase
{
    string currentScene;
    private SceneSystem sceneSystem;
    protected override void OnUpdate()
    {
        Debug.Log(GameManager.GM.SelectedMode + "From Enetitity");
        if (GameManager.GM.SelectedMode == "2D" && currentScene != "2D")
        {

        }
        else if (currentScene != "3D")
        {

        }
    }
    private void OnEnable()
    {
        // sceneSystem = World.GetOrCreateSystem<SceneSystem>();
    }
}
