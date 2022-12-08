
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigComponent : MonoBehaviour
{
    Machine selectedMachine;

    public Image NorthGateDisplay, SouthGateDisplay, EastGateDisplay, WestGateDisplay;
    public Sprite inputSprite, outputSprite, noneSprite, beltConnectSprite;
    public GameObject configTab;
    public bool configuring = false;

    public static ConfigComponent Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlaceObjectOnGrid.Instance.selectedMachineForConfig != null)
        {
            selectedMachine = PlaceObjectOnGrid.Instance.selectedMachineForConfig.GetComponent<Machine>();
            if (selectedMachine != null)
            {
                updateConfigForSelectedMachine();
            }
        }
    }

    public void updateConfigForSelectedMachine()
    {
        print("East: " + selectedMachine.GetGateTypeAtDir(Direction.East));
        print("South: " + selectedMachine.GetGateTypeAtDir(Direction.South));
        print("North: " + selectedMachine.GetGateTypeAtDir(Direction.North));
        print("West: " + selectedMachine.GetGateTypeAtDir(Direction.West));
        updateGateDisplay();
    }

    public void openConfig()
    {
        configTab.SetActive(true);
        configuring = true;
        gameObject.SetActive(true);
    }

    public void closeConfig()
    {
        configuring = false;
        configTab.SetActive(false);
        gameObject.SetActive(false);
    }

    public void updateGateDisplay()
    {
        foreach (Direction directionEnum in System.Enum.GetValues(typeof(Direction)))
        {
            Image dirDisplay = null;
            float rotateAngle = 0f;
            if (directionEnum == Direction.North)
            {
                dirDisplay = NorthGateDisplay;
            }
            else if (directionEnum == Direction.South)
            {
                dirDisplay = SouthGateDisplay;
                rotateAngle = 180f;
            }
            else if (directionEnum == Direction.East)
            {
                dirDisplay = EastGateDisplay;
                rotateAngle = -90f;
            }
            else if (directionEnum == Direction.West)
            {
                dirDisplay = WestGateDisplay;
                rotateAngle = 90f;
            }

            if (dirDisplay != null)
            {
                if (selectedMachine.GetGateTypeAtDir(directionEnum) == GateType.None)
                {
                    dirDisplay.sprite = noneSprite;
                    dirDisplay.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
                }
                else if (selectedMachine.GetGateTypeAtDir(directionEnum) == GateType.Entrance)
                {
                    dirDisplay.sprite = inputSprite;
                    dirDisplay.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, rotateAngle));
                }
                else if (selectedMachine.GetGateTypeAtDir(directionEnum) == GateType.Exit)
                {
                    dirDisplay.sprite = outputSprite;
                    dirDisplay.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, rotateAngle));
                }
                else if (selectedMachine.GetGateTypeAtDir(directionEnum) == GateType.Belt)
                {
                    dirDisplay.sprite = beltConnectSprite;
                    dirDisplay.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
                }
            }
        }
    }

    public void toggleGate(string direction)
    {
        // String to enum
        Direction directionEnum = (Direction)System.Enum.Parse(typeof(Direction), direction);
        List<GateType> availableGates = new List<GateType>() {GateType.None,GateType.Entrance,GateType.Exit};

        List<DataType> newDataType = new List<DataType>();

        if (selectedMachine.type == MachineType.Belt)
        {
            availableGates = new List<GateType>() { GateType.None, GateType.Belt };
        }

        int gateCount = availableGates.Count;
        GateType currentGateType = selectedMachine.GetGateTypeAtDir(directionEnum);
        Debug.Log("gateCount: "+ gateCount);
        int oldGateIndex = availableGates.IndexOf(currentGateType);
        Debug.Log("oldGateIndex: " + oldGateIndex);
        int newGateIndex = (oldGateIndex + 1) % gateCount;
        Debug.Log("newGateIndex: " + newGateIndex);


        GateType newGateType = availableGates[newGateIndex];


        if (newGateType == GateType.None)
        {
            newDataType = new List<DataType>() { };
        }
        else if (newGateType == GateType.Entrance)
        {
            newDataType = selectedMachine.entranceDataType;
        }
        else if (newGateType == GateType.Exit)
        {
            newDataType = selectedMachine.exitDataType;
        }

        if (!selectedMachine.gateDict.ContainsKey(directionEnum))
        {
            selectedMachine.AssignGate(new Gate(newGateType, directionEnum, newDataType), directionEnum);
        }
        else
        {
            selectedMachine.gateDict[directionEnum].changeGateType(newGateType);
        }

    }
}
