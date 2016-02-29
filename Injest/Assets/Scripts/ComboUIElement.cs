using UnityEngine;
using System.Collections;

public class ComboUIElement : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Image image;

    public Color ElementColor
    {
        get
        {
            return image.color;
        }

        set
        {
            image.color = value;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
