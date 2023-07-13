using UnityEngine;
using TMPro;

public class SizePanelControl : MonoBehaviour
{

    [SerializeField] TMP_Dropdown modeDropdown;
    [SerializeField] GameObject panel2D, panel3D;
    [SerializeField] int minSize = 10;
    [SerializeField] int maxSize = 2000;
    [Header("Panel Vectors 2D")]
    [SerializeField] TMP_InputField x2D;
    [SerializeField] TMP_InputField y2D;
    [Header("Panel Vectors 3D")]
    [SerializeField] TMP_InputField x3D;
    [SerializeField] TMP_InputField y3D;
    [SerializeField] TMP_InputField z3D;
    public Vector2Int panel2DSize { get; private set; }
    public Vector3Int panel3DSize { get; private set; }
    void Awake()
    {
        TogglePanel(true);
        panel2DSize = new Vector2Int(int.Parse(x2D.text), int.Parse(y2D.text));
        panel3DSize = new Vector3Int(int.Parse(x3D.text), int.Parse(y3D.text), int.Parse(z3D.text));
    }
    void OnEnable()
    {
        //-----------event 2d
        x2D.onValueChanged.AddListener(func => HandleIntClampAndValidation(x2D));
        y2D.onValueChanged.AddListener(func => HandleIntClampAndValidation(y2D));
        //-----------event 3d
        x3D.onValueChanged.AddListener(func => HandleIntClampAndValidation(x3D));
        y3D.onValueChanged.AddListener(func => HandleIntClampAndValidation(y3D));
        z3D.onValueChanged.AddListener(func => HandleIntClampAndValidation(z3D));
    }
    void OnDisable()
    {
        //-----------event 2d
        x2D.onValueChanged.RemoveListener(func => HandleIntClampAndValidation(x2D));
        y2D.onValueChanged.RemoveListener(func => HandleIntClampAndValidation(y2D));
        //-----------event 3d
        x3D.onValueChanged.RemoveListener(func => HandleIntClampAndValidation(x3D));
        y3D.onValueChanged.RemoveListener(func => HandleIntClampAndValidation(y3D));
        z3D.onValueChanged.RemoveListener(func => HandleIntClampAndValidation(z3D));
    }
    public void HandleSizeToggle()
    {
        string selectedMode = modeDropdown.options[modeDropdown.value].text;
        TogglePanel(selectedMode == "2D");
    }
    void TogglePanel(bool is2D)
    {
        panel2D.SetActive(is2D);
        panel3D.SetActive(!is2D);
    }
    void HandleIntClampAndValidation(TMP_InputField intInput)
    {
        int value;
        bool parseInt = int.TryParse(intInput.text, out value);
        if (!parseInt) value = 100;
        int clampedValue = Mathf.Clamp(value, minSize, maxSize);
        if (value > maxSize || value < minSize)
        {
            intInput.text = clampedValue.ToString();
            return;
        }


        if (intInput.transform.parent.tag == "2DSizePanel")
        {
            panel2DSize = new Vector2Int(int.Parse(x2D.text), int.Parse(y2D.text));
            GameManager.GM.UpdatePanelSize(panel2DSize);
        }
        else
        {
            panel3DSize = new Vector3Int(int.Parse(x3D.text), int.Parse(y3D.text), int.Parse(z3D.text));
            GameManager.GM.UpdatePanelSize(panel3DSize);
        }
    }
}
