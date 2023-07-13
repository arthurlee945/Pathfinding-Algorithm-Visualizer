using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Scenes;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GM { get; private set; }
    [SerializeField] Camera mainCamera;
    [SerializeField] SubScene scene2D, scene3D;
    [Header("Dropdown Fields")]
    [SerializeField] TMP_Dropdown modeDropdown;
    [SerializeField] TMP_Dropdown algorithmDropdown;
    SceneSystem sceneSystem;
    public string SelectedMode { get; private set; }
    public string SelectedAlgo { get; private set; }
    public Vector2Int panel2DSize { get; private set; } = new Vector2Int(100, 100);
    public Vector3Int panel3DSize { get; private set; } = new Vector3Int(100, 100, 100);
    void Awake()
    {
        if (GM != null && GM != this)
        {
            Destroy(this);
            return;
        }
        GM = this;
        DontDestroyOnLoad(this.gameObject);
        HandleModeToggle();
        HandleAlgorithmToggle();
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
    void Start()
    {

    }
    //-----------dropdown events
    private void HandleModeToggle()
    {
        SelectedMode = modeDropdown.options[modeDropdown.value].text;
        if (SelectedMode == "2D")
        {
            panel3DSize = new Vector3Int(100, 100, 100);
        }
        else
        {
            panel2DSize = new Vector2Int(100, 100);
        }
        SetCameraOnModeChange(SelectedMode);
    }
    private void HandleAlgorithmToggle()
    {
        SelectedAlgo = algorithmDropdown.options[algorithmDropdown.value].text;
    }
    public void UpdatePanelSize(Vector3Int newPanelSize) => panel3DSize = newPanelSize;
    public void UpdatePanelSize(Vector2Int newPanelSize) => panel2DSize = newPanelSize;
    //------------------------- private funcs
    void SetCameraOnModeChange(string mode)
    {
        mainCamera.transform.position = mode == "2D" ? new Vector3(-25f, 50, -25) : new Vector3(-50f, 125f, -50f);
        mainCamera.transform.eulerAngles = new Vector3(30, 45, 0);
    }
}
