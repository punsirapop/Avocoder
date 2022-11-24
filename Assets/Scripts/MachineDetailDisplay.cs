using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MachineDetailDisplay : MonoBehaviour
{
    public static MachineDetailDisplay Instance;

    [SerializeField] GameObject DetailTab;
    [SerializeField] GameObject ConfigTab;

    [SerializeField] TextMeshProUGUI Name;
    [SerializeField] TextMeshProUGUI Type;
    [SerializeField] TextMeshProUGUI[] Gates;
    [SerializeField] TextMeshProUGUI Position;

    Node selected;
    Machine component;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void SetSelection(Node node)
    {
        selected = node;
        component = node.thingPlaced.GetComponent<Machine>();

        Name.text = component.myName.ToString();
        Type.text = component.type.ToString();

        foreach (var g in Gates)
        {
            g.text = "-";
        }
        foreach (var item in component.gateDict)
        {
            Gates[(int)item.Key].text = item.Value.gateType.ToString();
        }

        Position.text = "(" + (node.cellPosition.x + 1) + ", " + (node.cellPosition.z + 1) + ")";
    }

    public void OpenDetail()
    {
        DetailTab.SetActive(true);
    }
    public void CloseDetail()
    {
        DetailTab.SetActive(false);
    }
}
