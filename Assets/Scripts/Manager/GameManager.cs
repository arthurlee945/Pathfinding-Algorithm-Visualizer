using TMPro;
using Unity.Scenes;
using Unity.Entities;
using UnityEngine;
using Unity.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager GM { get; private set; }
    [SerializeField] Camera mainCamera;
    // [SerializeField] SubScene scene2D, scene3D;
    // [SerializeField] Entity playzone2D, playzone3D;
    [Header("Dropdown Fields")]
    [SerializeField] TMP_Dropdown modeDropdown;
    [SerializeField] TMP_Dropdown algorithmDropdown;
    [Header("Display Text")]
    [SerializeField] TMP_Text stateChangeDisplay;
    [SerializeField] TMP_Text algorithmDisplay;
    SceneSystem sceneSystem;
    public ContainerMode SelectedMode { get; private set; } = ContainerMode.Scene2D;
    public Algorithms SelectedAlgo { get; private set; } = Algorithms.BreadthFirstSearch;
    public Vector2Int panel2DSize { get; private set; } = new Vector2Int(100, 100);
    public Vector3Int panel3DSize { get; private set; } = new Vector3Int(100, 100, 100);
    // public SubScene Scene2D { get { return scene2D; } }
    // public SubScene Scene3D { get { return scene3D; } }

    void Awake()
    {
        if (GM != null && GM != this)
        {
            Destroy(this);
            return;
        }
        GM = this;
        DontDestroyOnLoad(this.gameObject);
    }
    void OnEnable()
    {
        modeDropdown.onValueChanged.AddListener(func => HandleModeToggle());
        algorithmDropdown.onValueChanged.AddListener(func => HandleAlgorithmToggle());
    }
    void OnDisable()
    {
        modeDropdown.onValueChanged.RemoveListener(func => HandleModeToggle());
        algorithmDropdown.onValueChanged.RemoveListener(func => HandleAlgorithmToggle());
    }
    //-----------dropdown events
    private void HandleModeToggle()
    {
        SelectedMode = modeDropdown.options[modeDropdown.value].text == "2D" ? ContainerMode.Scene2D : ContainerMode.Scene3D;
        if (SelectedMode == ContainerMode.Scene2D)
        {
            panel3DSize = new Vector3Int(100, 100, 100);
            stateChangeDisplay.GetComponent<StateChangeDisplay>().DisplayState("2D Mode");
        }
        else
        {
            panel2DSize = new Vector2Int(100, 100);
            stateChangeDisplay.GetComponent<StateChangeDisplay>().DisplayState("3D Mode");
        }
        SetCameraOnModeChange(SelectedMode);
    }
    private void HandleAlgorithmToggle()
    {
        string currSelectedAlgo = algorithmDropdown.options[algorithmDropdown.value].text;
        switch (currSelectedAlgo)
        {
            case "Breadth-First Search":
                SelectedAlgo = Algorithms.BreadthFirstSearch;
                break;
            case "Dijkstra":
                SelectedAlgo = Algorithms.Dijkstra;
                break;
            case "A*":
                SelectedAlgo = Algorithms.A_star;
                break;
            default:
                SelectedAlgo = Algorithms.BreadthFirstSearch;
                break;
        }
        stateChangeDisplay.GetComponent<StateChangeDisplay>().DisplayState(currSelectedAlgo);
        algorithmDisplay.text = currSelectedAlgo;
    }
    public void UpdatePanelSize(Vector3Int newPanelSize) => panel3DSize = newPanelSize;
    public void UpdatePanelSize(Vector2Int newPanelSize) => panel2DSize = newPanelSize;
    //------------------------- private funcs
    void SetCameraOnModeChange(ContainerMode mode)
    {
        mainCamera.transform.position = mode == ContainerMode.Scene2D ? new Vector3(-25f, 50, -25) : new Vector3(-50f, 125f, -50f);
        mainCamera.transform.eulerAngles = new Vector3(30, 45, 0);
    }
    // void SetZoneManager()
    // {
    //     EntityQuery zoneManagerQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(typeof(ZoneManager));
    //     NativeArray<Entity> entityArray = zoneManagerQuery.ToEntityArray(Unity.Collections.Allocator.Temp);
    //     if (entityArray.Length <= 0) return;
    //     zoneManager = entityArray[0];
    //     Debug.Log(entityArray.Length);
    // }
}
