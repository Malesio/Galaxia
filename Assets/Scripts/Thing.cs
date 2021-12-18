using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Thing : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        FindObjectOfType<GraphicStatistics>()?.ToggleDebugInformation();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Text>().color = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Text>().color = new Color(112 / 255.0f, 173 / 255.0f, 172 / 255.0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
