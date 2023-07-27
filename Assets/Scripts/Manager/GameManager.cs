using TMPro;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GM { get; private set; }
    [SerializeField] Camera mainCamera;
    [Header("Algorithm Selector Fields")]
    [SerializeField] TMP_Dropdown algorithmDropdown;
    [Header("Obstacle Selector Fields")]
    [SerializeField] TMP_Dropdown obstaclesDropdown;
    [Header("Display Text")]
    [SerializeField] TMP_Text algorithmDisplay;
    public Algorithms SelectedAlgo { get; private set; } = Algorithms.BreadthFirstSearch;
    public bool ObstacleIsCustom { get; set; } = true;
    public Vector2Int panelSize { get; private set; } = new Vector2Int(50, 50);
    ObstacleSamples obsSample;
    EntityManager entityManager;
    Vector2Int defaultStartCoors = new Vector2Int(5, 5);
    Vector2Int defaultEndCoors = new Vector2Int(44, 44);
    void Awake()
    {
        if (GM != null && GM != this)
        {
            Destroy(this);
            return;
        }
        GM = this;
        obsSample = new ObstacleSamples();
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }
    void OnEnable()
    {
        algorithmDropdown.onValueChanged.AddListener(index => HandleAlgorithmSelect(index));
        obstaclesDropdown.onValueChanged.AddListener(index => HandleObstacleSelect(index));
    }
    void OnDisable()
    {
        algorithmDropdown.onValueChanged.RemoveListener(index => HandleAlgorithmSelect(index));
        obstaclesDropdown.onValueChanged.RemoveListener(index => HandleObstacleSelect(index));
    }

    private void HandleAlgorithmSelect(int selectedIndex)
    {
        if (PathFinder.Instance.IsRunning || PathFinder.Instance.IsPreview) return;
        string currSelectedAlgo = algorithmDropdown.options[selectedIndex].text;
        switch (selectedIndex)
        {
            case 0:
                SelectedAlgo = Algorithms.BreadthFirstSearch;
                break;
            case 1:
                SelectedAlgo = Algorithms.DepthFirstSearch;
                break;
            case 2:
                SelectedAlgo = Algorithms.Dijkstra;
                break;
            case 3:
                SelectedAlgo = Algorithms.A_star;
                break;
            default:
                SelectedAlgo = Algorithms.BreadthFirstSearch;
                break;
        }
        StateChangeDisplay.Instance.DisplayState(currSelectedAlgo);
        algorithmDisplay.text = currSelectedAlgo;
    }
    public void SetObstacleToCustom()
    {
        obstaclesDropdown.value = 0;
        ObstacleIsCustom = true;
    }
    private void HandleObstacleSelect(int selectedIndex)
    {
        if (PathFinder.Instance.IsRunning || PathFinder.Instance.IsPreview) return;
        if (panelSize.x != obsSample.Width || panelSize.x != obsSample.Height)
        {
            obstaclesDropdown.value = 0;
            StateChangeDisplay.Instance.DisplayState($"{obsSample.Width} x {obsSample.Height} required");
            return;
        }
        string currSelectedObs = obstaclesDropdown.options[selectedIndex].text;
        ObstacleIsCustom = selectedIndex == 0;
        switch (selectedIndex)
        {
            case 0:
                break;
            case 1:
                GenerateObstacle(obsSample.Maze);
                break;
            case 2:
                GenerateObstacle(obsSample.Blockade);

                break;
            case 3:
                GenerateObstacle(obsSample.Dog);

                break;
            default:
                GenerateObstacle(obsSample.Maze);
                break;
        }
        StateChangeDisplay.Instance.DisplayState(currSelectedObs);
    }
    void GenerateObstacle(int[,] obsPositions)
    {
        PreObstacleCleanUp();
        PathFinder.Instance.StartCoors = defaultStartCoors;
        PathFinder.Instance.EndCoors = defaultEndCoors;
        for (int i = 0; i < obsPositions.GetLength(0); i++)
        {
            Vector2Int coor = new Vector2Int(obsPositions[i, 0], obsPositions[i, 1]);
            if (!ZoneStore.Instance.Zones.ContainsKey(coor)) continue;
            Entity selectedZone = ZoneStore.Instance.Zones[coor];
            //---------------Set zone to not walkable;
            ZoneComponent zc = entityManager.GetComponentData<ZoneComponent>(selectedZone);
            zc.isWalkable = false;
            entityManager.SetComponentData<ZoneComponent>(selectedZone, zc);
            //---------------Set color
            URPMaterialPropertyBaseColor color = entityManager.GetComponentData<URPMaterialPropertyBaseColor>(selectedZone);
            color.Value = StateColors.Instance.NotWalkableColor;
            entityManager.SetComponentData<URPMaterialPropertyBaseColor>(selectedZone, color);
        }
    }
    void PreObstacleCleanUp()
    {
        foreach (Entity e in ZoneStore.Instance.Zones.Values)
        {
            ZoneComponent zc = entityManager.GetComponentData<ZoneComponent>(e);
            zc.isWalkable = true;
            zc.isPath = false;
            zc.isExplored = false;
            zc.isStart = new Vector2Int(zc.coordinates.x, zc.coordinates.y) == defaultStartCoors;
            zc.isEnd = new Vector2Int(zc.coordinates.x, zc.coordinates.y) == defaultEndCoors;
            zc.gCost = 0;
            zc.hCost = 0;
            entityManager.SetComponentData<ZoneComponent>(e, zc);
            URPMaterialPropertyBaseColor baseColor = entityManager.GetComponentData<URPMaterialPropertyBaseColor>(e);
            baseColor.Value = new Vector2Int(zc.coordinates.x, zc.coordinates.y) == defaultStartCoors ? StateColors.Instance.StartColor :
                new Vector2Int(zc.coordinates.x, zc.coordinates.y) == defaultEndCoors ? StateColors.Instance.EndColor : StateColors.Instance.DefaultColor;
            entityManager.SetComponentData<URPMaterialPropertyBaseColor>(e, baseColor);
        }
    }
    public void UpdatePanelSize(Vector2Int newPanelSize)
    {
        panelSize = newPanelSize;
        mainCamera.transform.position = new Vector3(panelSize.x / 2, 50, panelSize.y / 2);
    }
}
