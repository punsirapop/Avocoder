using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Grid : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public static GameObject currentlySelected;
    public Material[] materials;

    [SerializeField] MeshRenderer thisMat;
    [SerializeField] GameObject myMachine;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(currentlySelected != gameObject)
        {
            thisMat.material = materials[1];
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
            thisMat.material = materials[0];
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(currentlySelected != gameObject)
        {
            currentlySelected?.SendMessage("ResetMat");
            currentlySelected = gameObject;
            thisMat.material = materials[2];
        }
    }

    public void ResetMat()
    {
        thisMat.material = materials[0];
    }
}
