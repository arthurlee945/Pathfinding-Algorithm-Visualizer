using UnityEngine;
using TMPro;

public class SizePanelControl : MonoBehaviour
{
    [SerializeField] TMP_Dropdown modeDropdown;
    [SerializeField] GameObject panel2D, panel3D;
    [SerializeField] int minSize = 1;
    [SerializeField] int maxSize = 200;
    // [SerializeField] int max3DSize = 100;
    [Header("Panel Vectors 2D")]
    [SerializeField] TMP_InputField x2D;
    [SerializeField] TMP_InputField y2D;
    [Header("Panel Vectors 3D")]
    [SerializeField] TMP_InputField x3D;
    [SerializeField] TMP_InputField y3D;
    [SerializeField] TMP_InputField z3D;
    public Vector2Int panelSize { get; private set; }
    void Awake()
    {
        panelSize = new Vector2Int(int.Parse(x2D.text), int.Parse(y2D.text));
    }
    void OnEnable()
    {
        //-----------event 2d
        x2D.onValueChanged.AddListener(func => HandleIntClampAndValidation(x2D));
        y2D.onValueChanged.AddListener(func => HandleIntClampAndValidation(y2D));
    }
    void OnDisable()
    {
        //-----------event 2d
        x2D.onValueChanged.RemoveListener(func => HandleIntClampAndValidation(x2D));
        y2D.onValueChanged.RemoveListener(func => HandleIntClampAndValidation(y2D));

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
        panelSize = new Vector2Int(int.Parse(x2D.text), int.Parse(y2D.text));
        GameManager.GM.UpdatePanelSize(panelSize);
    }
}
