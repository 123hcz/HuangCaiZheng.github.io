using UnityEngine;
using UnityEngine.UI;
using System.Collections;


[RequireComponent(typeof(Image))]
public class CRGTSpriteToggle : MonoBehaviour {

    public Sprite ImageOn;
    public Sprite ImageOff;
    public Sprite ImageLocked;

    private Image targetImage;
    private int toggleState;

    void Start()
    {
        targetImage = GetComponent<Image>();
    }

    public void SetToggleValue(int value)
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>();

        toggleState = value;

        if (toggleState == -1)
            targetImage.sprite = ImageLocked;
        if (toggleState == 0)
            targetImage.sprite = ImageOff;
        if (toggleState == 1)
            targetImage.sprite = ImageOn;
    }

    public int ToggleState
    {
        get { return toggleState; }
        set { SetToggleValue(value); }
    }
}
