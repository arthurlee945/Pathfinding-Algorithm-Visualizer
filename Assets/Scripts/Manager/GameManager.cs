using TMPro;
using Unity.Scenes;
using Unity.Entities;
using UnityEngine;
using Unity.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager GM { get; private set; }
    [SerializeField] Camera mainCamera;
    [Header("Dropdown Fields")]
    [SerializeField] TMP_Dropdown algorithmDropdown;
    [Header("Display Text")]
    [SerializeField] TMP_Text stateChangeDisplay;
    [SerializeField] TMP_Text algorithmDisplay;
    public Algorithms SelectedAlgo { get; private set; } = Algorithms.BreadthFirstSearch;
    public Vector2Int panelSize { get; private set; } = new Vector2Int(50, 50);

    void Awake()
    {
        if (GM != null && GM != this)
        {
            Destroy(this);
            return;
        }
        GM = this;
    }
    void OnEnable()
    {
        algorithmDropdown.onValueChanged.AddListener(func => HandleAlgorithmToggle());
    }
    void OnDisable()
    {
        algorithmDropdown.onValueChanged.RemoveListener(func => HandleAlgorithmToggle());
    }
    private void HandleAlgorithmToggle()
    {
        string currSelectedAlgo = algorithmDropdown.options[algorithmDropdown.value].text;
        switch (currSelectedAlgo)
        {
            case "Breadth-First Search":
                SelectedAlgo = Algorithms.BreadthFirstSearch;
                break;
            case "Depth-First Search":
                SelectedAlgo = Algorithms.DepthFirstSearch;
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
    public void UpdatePanelSize(Vector2Int newPanelSize)
    {
        panelSize = newPanelSize;
        mainCamera.transform.position = new Vector3(panelSize.x / 2, 50, panelSize.y / 2);
    }
}
