using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class HighlightedTextSize : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public TextMeshProUGUI buttonText;
    public int highlightedTextSize = 65;
    public float originalTextSize = 50;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ChangeTextSize(highlightedTextSize);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ChangeTextSize(originalTextSize);
    }

    public void OnSelect(BaseEventData eventData)
    {
        ChangeTextSize(highlightedTextSize);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        ChangeTextSize(originalTextSize);
    }

    private void ChangeTextSize(float newSize)
    {
        buttonText.fontSize = newSize;
    }
}
