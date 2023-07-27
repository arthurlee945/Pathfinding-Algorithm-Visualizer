using UnityEngine;
using TMPro;

public class SizePanelControl : MonoBehaviour
{
    [SerializeField] GameObject panel2D;
    [SerializeField] int minSize = 1;
    [SerializeField] int maxSize = 200;
    // [SerializeField] int max3DSize = 100;
    [Header("Panel Vectors 2D")]
    [SerializeField] TMP_InputField xInput;
    [SerializeField] TMP_InputField yInput;
    UtilFunc utilFunc = new UtilFunc();
    public Vector2Int panelSize { get; private set; }
    void Awake()
    {
        panelSize = new Vector2Int(int.Parse(xInput.text), int.Parse(yInput.text));
    }
    void OnEnable()
    {
        //-----------event 2d
        xInput.onValueChanged.AddListener(_ => HandleIntClampAndValidation(xInput));
        yInput.onValueChanged.AddListener(_ => HandleIntClampAndValidation(yInput));
    }
    void OnDisable()
    {
        //-----------event 2d
        xInput.onValueChanged.RemoveListener(_ => HandleIntClampAndValidation(xInput));
        yInput.onValueChanged.RemoveListener(_ => HandleIntClampAndValidation(yInput));

    }
    void HandleIntClampAndValidation(TMP_InputField intInput)
    {
        int value;
        bool parseInt = int.TryParse(intInput.text, out value);
        if (!parseInt)
        {
            value = 50;
            intInput.text = "50";
        };
        int clampedValue = Mathf.Clamp(value, minSize, maxSize);
        if (value > maxSize || value < minSize)
        {
            intInput.text = clampedValue.ToString();
            return;
        }
        utilFunc.Debounce(500, _ =>
        {
            panelSize = new Vector2Int(int.Parse(xInput.text), int.Parse(yInput.text));
            GameManager.GM.UpdatePanelSize(panelSize);
        });
    }
}
