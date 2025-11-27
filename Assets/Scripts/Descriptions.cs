using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Description : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject dropDown;
    // Start is called before the first frame update
    void Start()
    {
        dropDown.SetActive(false);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        dropDown.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        dropDown.SetActive(false);
    }
}
