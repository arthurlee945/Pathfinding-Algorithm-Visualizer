using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public static PathFinder Instance { get; private set; }
    [SerializeField] Vector2Int startCoors, endCoors;
    [SerializeField] float searchSpeed = 0.0001f;
    [SerializeField] float speedUpIncrement = 0.25f;
    [SerializeField] float maxSpeed = 4f;
    EntityManager entityManager;
    BreadthFirstSearch breadthFirstSearch;
    DepthFirstSearch depthFirstSearch;
    Dijkstra dijkstra;
    AStar aStar;
    float currentSpeed = 1;
    bool hardResetPath;
    public bool IsRunning { get; set; }
    public bool IsPreview { get; set; }
    public Vector2Int StartCoors { get { return startCoors; } set { startCoors = value; } }
    public Vector2Int EndCoors { get { return endCoors; } set { endCoors = value; } }
    public float SearchSpeed { get { return searchSpeed; } }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        breadthFirstSearch = GetComponent<BreadthFirstSearch>();
        depthFirstSearch = GetComponent<DepthFirstSearch>();
        dijkstra = GetComponent<Dijkstra>();
        aStar = GetComponent<AStar>();
    }
    void start()
    {
        SetStartAndEndPoint();
    }
    //------------------------- Control Event Fucs
    public void StartFindPath()
    {
        if (IsRunning)
        {
            StateChangeDisplay.Instance.DisplayState("Resumed");
            currentSpeed = 1;
            Time.timeScale = currentSpeed;
            return;
        }
        if (!IsRunning && IsPreview) ResetZones();
        StateChangeDisplay.Instance.DisplayState("Started");
        SetStartAndEndPoint();
        if (!ZoneStore.Instance.Zones.ContainsKey(startCoors)) startCoors = new Vector2Int(0, 0);
        if (!ZoneStore.Instance.Zones.ContainsKey(endCoors)) endCoors = (GameManager.GM.panelSize - new Vector2Int(1, 1));

        switch (GameManager.GM.SelectedAlgo)
        {
            case Algorithms.BreadthFirstSearch:
                breadthFirstSearch.FindPath(startCoors, endCoors);
                break;
            case Algorithms.DepthFirstSearch:
                depthFirstSearch.FindPath(startCoors, endCoors);
                break;
            case Algorithms.Dijkstra:
                dijkstra.FindPath(startCoors, endCoors);
                break;
            case Algorithms.A_star:
                aStar.FindPath(startCoors, endCoors);
                break;
            default:
                break;
        }
    }
    public void PausePath()
    {
        if (!IsRunning && !IsPreview) return;
        StateChangeDisplay.Instance.DisplayState(IsPreview ? "Ended" : "Paused");
        currentSpeed = 0;
        Time.timeScale = currentSpeed;
    }
    public void StopPathFinding()
    {
        if (hardResetPath) ResetZones();
        if (!IsRunning && !IsPreview) return;
        StateChangeDisplay.Instance.DisplayState("Cleared");
        currentSpeed = 1;
        Time.timeScale = 1;
        switch (GameManager.GM.SelectedAlgo)
        {
            case Algorithms.BreadthFirstSearch:
                breadthFirstSearch.ResetPath();
                break;
            case Algorithms.DepthFirstSearch:
                depthFirstSearch.ResetPath();
                break;
            case Algorithms.Dijkstra:
                dijkstra.ResetPath();
                break;
            case Algorithms.A_star:
                aStar.ResetPath();
                break;
            default:
                break;
        }
        ResetZones();
    }
    public void SpeedControl(bool isFastForward)
    {
        if (!IsRunning) return;
        float speed = Mathf.Clamp(currentSpeed + (isFastForward ? speedUpIncrement : -speedUpIncrement), 0.25f, maxSpeed);
        currentSpeed = speed;
        StateChangeDisplay.Instance.DisplayState(currentSpeed.ToString() + "x");
        Time.timeScale = currentSpeed;
    }
    //--------------------- zone validate
    public void SetStartAndEndPoint()
    {
        if (startCoors.x >= GameManager.GM.panelSize.x || startCoors.y >= GameManager.GM.panelSize.y)
        {
            startCoors = new Vector2Int(0, 0);
            Entity startZone = ZoneStore.Instance.Zones[startCoors];
            ZoneComponent zc = entityManager.GetComponentData<ZoneComponent>(startZone);
            zc.isWalkable = false;
            zc.isStart = true;
            entityManager.SetComponentData<URPMaterialPropertyBaseColor>(startZone, new URPMaterialPropertyBaseColor
            {
                Value = StateColors.Instance.StartColor
            });
        }
        if (endCoors.x >= GameManager.GM.panelSize.x || endCoors.y >= GameManager.GM.panelSize.y)
        {
            endCoors = (GameManager.GM.panelSize - new Vector2Int(1, 1));
            Entity endZone = ZoneStore.Instance.Zones[endCoors];
            ZoneComponent zc = entityManager.GetComponentData<ZoneComponent>(endZone);
            zc.isWalkable = false;
            zc.isStart = true;
            entityManager.SetComponentData<URPMaterialPropertyBaseColor>(endZone, new URPMaterialPropertyBaseColor
            {
                Value = StateColors.Instance.StartColor
            });
        }
    }
    void ResetZones()
    {
        foreach (Entity e in ZoneStore.Instance.Zones.Values)
        {
            ZoneComponent zc = entityManager.GetComponentData<ZoneComponent>(e);
            if (!zc.isWalkable && !hardResetPath) continue;
            zc.isWalkable = true;
            zc.isPath = false;
            zc.isExplored = false;
            zc.gCost = 0;
            zc.hCost = 0;
            entityManager.SetComponentData<ZoneComponent>(e, zc);
            if (zc.isStart || zc.isEnd) continue;
            URPMaterialPropertyBaseColor baseColor = entityManager.GetComponentData<URPMaterialPropertyBaseColor>(e);
            baseColor.Value = StateColors.Instance.DefaultColor;
            entityManager.SetComponentData<URPMaterialPropertyBaseColor>(e, baseColor);
        }
        hardResetPath = !hardResetPath;
        IsPreview = false;
        IsRunning = false;
        Time.timeScale = 1;
    }

}
