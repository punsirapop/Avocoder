using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

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

        foreach (Direction d in Enum.GetValues(typeof(Direction)))
        {
            string t = "";
            Gate g = component.gateDict.Any(x => x.Key == d) ? component.gateDict.First(x => x.Key == d).Value : null;
            if (g != null)
            {
                switch (g.gateType)
                {
                    case GateType.Entrance:
                        t += "In: ";
                        break;
                    case GateType.Exit:
                        t += "Out: ";
                        break;
                    case GateType.Belt:
                        t += "Belt";
                        break;
                }
                foreach (var item in g.dataTypeList)
                {
                    t += item.ToString() + " ";
                }
            }
            else
            {
                t = "-";
            }
            Debug.Log((int)d);
            Gates[(int)d].text = t;

        }

        /*
        foreach (var g in Gates)
        {
            g.text = "-";
        }
        foreach (var item in component.gateDict)
        {
            Gates[(int)item.Key].text = item.Value.gateType.ToString();
        }
        */

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
