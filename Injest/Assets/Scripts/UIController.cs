using UnityEngine;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
    public GameObject Cursor;
    public float DeadZone = 0.1f;
    public float MaxRadius = 64.0f;
    public bool SnapToTarget = false;
    public float SnapDistance = 0.75f;
    public bool PressToConfirm = false;
    public bool RequireInputReleaseReset = false;
    public UnityEngine.UI.Text DebugText;

    public UnityEngine.UI.Image Output;

    public ComboUIElement ElementPrefab;
    public List<Color> ElementColors = new List<Color>();

    [SerializeField]
    private UnityEngine.UI.Image inputPrefab;
    private List<UnityEngine.UI.Image> inputs = new List<UnityEngine.UI.Image>();
    [SerializeField]
    private int MaxInputs = 3;
    private int currentInputIndex = 0;

    private List<ComboUIElement> elements = new List<ComboUIElement>();
    private int selectedElement = -1;
    private int SelectedElement
    {
        get { return selectedElement; }
        set
        {
            if (value != selectedElement)
            {
                selectedElement = value;
            }
        }
    }
    
    private float InputAngle;
    private float SnappedAngle;

    public bool ComboUIVisible { get { return gameObject.activeSelf; } }

    // Use this for initialization
    void Start()
    {
        // Create the menu elements
        float angle = 360.0f / ElementColors.Count;
        for (int elementIndex = 0; elementIndex < ElementColors.Count; ++elementIndex)
        {
            ComboUIElement newElement = Instantiate(ElementPrefab) as ComboUIElement;

            newElement.transform.position = new Vector3(MaxRadius * Mathf.Cos(angle * elementIndex * Mathf.Deg2Rad), MaxRadius * Mathf.Sin(angle * elementIndex * Mathf.Deg2Rad), 0.0f);
            newElement.transform.SetParent(transform, false);
            newElement.ElementColor = ElementColors[elementIndex];

            elements.Add(newElement);
        }

        // Inputs
        float width = inputPrefab.rectTransform.rect.width * 1.5f;
        for (int inputIndex = 0; inputIndex < MaxInputs; ++inputIndex)
        {
            float inputX = inputIndex * width + (width * 0.5f) * (1.0f - MaxInputs);
            Vector3 position = new Vector3(inputX, 138.0f, 0.0f);
            
            inputs.Add(Instantiate(inputPrefab) as UnityEngine.UI.Image);
            inputs[inputIndex].transform.localPosition = position;
            inputs[inputIndex].transform.SetParent(transform, false);
        }

        // Draw the cursor last
        Cursor.transform.SetAsLastSibling();
    }

    // Update is called once per frame
    void Update()
    {
        // Reset states
        for (int elementIndex = 0; elementIndex < elements.Count; ++elementIndex)
        {
            elements[elementIndex].transform.localScale = Vector3.one;
        }

        int lastSelectedElement = SelectedElement;

        // Calculate which element to hover
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (input.sqrMagnitude > DeadZone)
        {
            if (SnapToTarget && input.sqrMagnitude > SnapDistance)
            {
                float snapAngle = 360.0f / ElementColors.Count;
                InputAngle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
                if (InputAngle < 0.0f)
                {
                    InputAngle += 360.0f;
                }
                SelectedElement = Mathf.RoundToInt(InputAngle / snapAngle) % elements.Count;
                
                SnappedAngle = SelectedElement * snapAngle;
                Cursor.transform.localPosition = new Vector3(MaxRadius * Mathf.Cos(SnappedAngle * Mathf.Deg2Rad), MaxRadius * Mathf.Sin(SnappedAngle * Mathf.Deg2Rad), 0.0f);

                elements[SelectedElement].transform.localScale = 2.0f * Vector3.one;
                
                if (!PressToConfirm && lastSelectedElement != SelectedElement)
                {
                    SelectInput();
                }
            }
            else
            {
                SelectedElement = -1;
                Cursor.transform.localPosition = input * MaxRadius;
            }
        }
        else
        {
            SelectedElement = -1;
            Cursor.transform.localPosition = Vector3.zero;
        }

        // Determine if user selected an element
        if (PressToConfirm && Input.GetButtonDown("Fire1") && SelectedElement >= 0)
        {
            SelectInput();
        }

        // Debug
        DebugText.text = string.Format("Press To Confirm: {0}\nRelease To Reset: {1}", PressToConfirm, RequireInputReleaseReset);

    }

    private void SelectInput()
    {
        if (RequireInputReleaseReset && currentInputIndex >= inputs.Count)
        {
            return;
        }

        if (currentInputIndex >= inputs.Count)
        {
            ResetInputs();
        }

        inputs[currentInputIndex].color = elements[SelectedElement].ElementColor;
        ++currentInputIndex;
        if (currentInputIndex >= inputs.Count)
        {
            Vector4 colors = Color.black;
            for (int inputIndex = 0; inputIndex < inputs.Count; ++inputIndex)
            {
                colors += (Vector4)inputs[inputIndex].color;
            }
            colors /= inputs.Count;
            Output.color = colors;
        }
    }

    private void ResetInputs()
    {
        currentInputIndex = 0;
        for (int inputIndex = 0; inputIndex < inputs.Count; ++inputIndex)
        {
            inputs[inputIndex].color = Color.white;
        }
        Output.color = Color.white;
    }

    public void ShowComboUI(bool show = true)
    {
        if (show)
        {
            ResetInputs();
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
       
}
