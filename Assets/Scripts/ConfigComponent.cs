/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigComponent : MonoBehaviour
{
    Machine selectedMachine;

    public Image NorthGateDisplay, SouthGateDisplay, EastGateDisplay, WestGateDisplay;
    public Sprite inputSprite, outputSprite, noneSprite;

    // Update is called once per frame
    void Update()
    {
        selectedMachine = PlaceObjectOnGrid.Instance.selectedMachineForConfig.GetComponent<Machine>();
        if (selectedMachine != null)
        {
            updateConfigForSelectedMachine();
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

    public void closeConfig()
    {
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

            if (selectedMachine.GetGateTypeAtDir(directionEnum) == GateType.None)
            {
                dirDisplay.sprite = noneSprite;
                //dirDisplay.transform.Rotate(0.0f, 0.0f, 0.0f);
                dirDisplay.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
            }
            else if (selectedMachine.GetGateTypeAtDir(directionEnum) == GateType.Entrance)
            {
                dirDisplay.sprite = inputSprite;
                //dirDisplay.transform.Rotate(0.0f, 0.0f, rotateAngle);
                dirDisplay.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, rotateAngle));
            }
            else if (selectedMachine.GetGateTypeAtDir(directionEnum) == GateType.Exit)
            {
                dirDisplay.sprite = outputSprite;
                //dirDisplay.transform.Rotate(0.0f, 0.0f, rotateAngle);
                dirDisplay.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, rotateAngle));
            }
        }
    }

    public void toggleGate(string direction)
    {
        // String to enum
        Direction directionEnum = (Direction)System.Enum.Parse(typeof(Direction), direction);

        GateType newGateType = GateType.None;
        if (selectedMachine.GetGateTypeAtDir(directionEnum) == GateType.None)
        {
            newGateType = GateType.Entrance;
        }
        else if (selectedMachine.GetGateTypeAtDir(directionEnum) == GateType.Entrance)
        {
            newGateType = GateType.Exit;
        }
        else if (selectedMachine.GetGateTypeAtDir(directionEnum) == GateType.Exit)
        {
            newGateType = GateType.None;
        }
        selectedMachine.AssignGate(new Gate(newGateType), directionEnum);
    }
}
*/