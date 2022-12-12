
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ConfigComponent : MonoBehaviour
{
    Machine selectedMachine;

    public Image NorthGateDisplay, SouthGateDisplay, EastGateDisplay, WestGateDisplay, VariableTypeDisplay, ComparisonSignDisplay, boolDisplay;
    public Sprite inputSprite, outputSprite, noneSprite, beltConnectSprite;
    public Sprite intImg, floatImg, boolImg, trueImg, falseImg;
    public Sprite gt, lt, eq, neq, gteq, lteg;
    public GameObject configTab, variableConfigTab, comparisonConfigTab;
    public GameObject dataInputField, boolDataToggle;
    public GameObject orderModeInstructionDisplay;

    public bool configuring = false;

    public static ConfigComponent Instance;

    public bool orderMode = false;
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
        
        if (selectedMachine.type == MachineType.Variable)
        {
            updateVariableDisplay();
            updateBoolDisplay();
        }
        else if (selectedMachine.type == MachineType.Comparison)
        {
            updateComparisonDisplay();
        }
    }

    public void openConfig()
    {
        if (PlaceObjectOnGrid.Instance.selectedMachineForConfig != null)
        {
            selectedMachine = PlaceObjectOnGrid.Instance.selectedMachineForConfig.GetComponent<Machine>();
        }

        configTab.SetActive(true);

        if (selectedMachine.type == MachineType.Variable)
        {
            variableConfigTab.SetActive(true);
            comparisonConfigTab.SetActive(false);
            refillInputField();
        }
        else if (selectedMachine.type == MachineType.Comparison)
        {
            comparisonConfigTab.SetActive(true);
            variableConfigTab.SetActive(false);

        }

        configuring = true;
        gameObject.SetActive(true);
    }

    public void closeConfig()
    {
        variableConfigTab.SetActive(false);
        comparisonConfigTab.SetActive(false);
        configTab.SetActive(false);
        configuring = false;
        gameObject.SetActive(false);

        if (selectedMachine.type == MachineType.Variable)
        {
            emptyInputField();
        }
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

    public void updateVariableDisplay()
    {
        Var machine = selectedMachine.gameObject.GetComponent<Var>();
        DataType currentDataType = machine.getDataType();
        print("data: "+currentDataType);
        if (currentDataType == DataType.Int)
        {
            VariableTypeDisplay.sprite = intImg;
            boolDataToggle.SetActive(false);
            dataInputField.SetActive(true);
        }
        else if (currentDataType == DataType.Float)
        {
            VariableTypeDisplay.sprite = floatImg;
            boolDataToggle.SetActive(false);
            dataInputField.SetActive(true);

        }
        else if (currentDataType == DataType.Bool)
        {
            VariableTypeDisplay.sprite = boolImg;
            dataInputField.SetActive(false);
            boolDataToggle.SetActive(true);
        }
    }

    public void updateComparisonDisplay()
    {
        ComOp machine = selectedMachine.gameObject.GetComponent<ComOp>();
        string currentSign = machine.getCurrentSign();
        if (currentSign == ">")
        {
            ComparisonSignDisplay.sprite = gt;
        }
        else if (currentSign == "<")
        {
            ComparisonSignDisplay.sprite = lt;
        }
        else if (currentSign == "==")
        {
            ComparisonSignDisplay.sprite = eq;
        }
        else if (currentSign == "!=")
        {
            ComparisonSignDisplay.sprite = neq;
        }
        else if (currentSign == ">=")
        {
            ComparisonSignDisplay.sprite = gteq;
        }
        else if (currentSign == "<=")
        {
            ComparisonSignDisplay.sprite = lteg;
        }
    }

    public void updateBoolDisplay()
    {
        Var machine = selectedMachine.gameObject.GetComponent<Var>();
        bool currentBool = machine.getBoolData();
        if (currentBool)
        {
            boolDisplay.sprite = trueImg;
        }
        else
        {
            boolDisplay.sprite = falseImg;
        }
    }

    public void toggleOrderMode()
    {
        orderMode = !orderMode;
        if (orderMode)
        {
            orderModeInstructionDisplay.SetActive(true);
        }
        else
        {
            orderModeInstructionDisplay.SetActive(false);
        }
    }
    public void toggleGate(string direction)
    {
        Node currentNode = PlaceObjectOnGrid.Instance.GetNode(selectedMachine.transform);
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

        if (selectedMachine.type == MachineType.Variable)
        {
            Var variableMachine = selectedMachine.gameObject.GetComponent<Var>();
            print("Change gate on variable to type: "+variableMachine.getDataType());
            newDataType = new List<DataType>() { variableMachine.getDataType() };
        }
        else if (newGateType == GateType.None)
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


        if (newGateType == GateType.None)
        {
            print("removing chain");
            Chain newChain = currentNode.chainStart;
            if (newChain != null)
            {
                newChain.removeThisChainFromList();
            }
            currentNode.chainStart = null;
        }
        else if (newGateType == GateType.Entrance)
        {
            print("removing chain");
            Chain newChain = currentNode.chainStart;
            if (newChain != null)
            {
                newChain.removeThisChainFromList();
            }
            currentNode.chainStart = null;
        }
        else if (newGateType == GateType.Exit)
        {
            print("Creating chain");
            Machine machine = currentNode.thingPlaced.GetComponent<Machine>();
            Chain newChain = new Chain(machine);
            currentNode.chainStart = newChain;
        }



        MachineDetailDisplay.Instance.SetSelection(currentNode);

        

    }

    public void toggleCompareSign()
    {
        ComOp machine = selectedMachine.gameObject.GetComponent<ComOp>();
        machine.toggleSign();        
    }

    public void toggleDataType()
    {
        if (selectedMachine.type == MachineType.Variable)
        {
            Var machine = selectedMachine.gameObject.GetComponent<Var>();
            machine.toggleDataType();
            readDataInput();
            foreach (Gate g in machine.gateDict.Values)
            {
                g.dataTypeList = new List<DataType>() { machine.getDataType() };
            }
        }
    }
    
    public void toggleBool()
    {
        if (selectedMachine.type == MachineType.Variable)
        {
            Var machine = selectedMachine.gameObject.GetComponent<Var>();
            machine.toggleBool();
        }
    }

    public void readDataInput()
    {
        string s = dataInputField.GetComponent<TMP_InputField>().text;
        if (selectedMachine.type == MachineType.Variable)
        {
            Var machine = selectedMachine.gameObject.GetComponent<Var>();
            if (machine.getDataType() == DataType.Int)
            {
                print("data int raw "+s);

                int outInt;
                bool response;
                response = int.TryParse(s, out outInt);
                if (response)
                {
                    dataInputField.GetComponent<Image>().color = Color.green;
                    machine.setIntData(outInt);
                    print("data int " + outInt);
                }
                else
                {
                    Debug.Log("Invalid input");
                    if (string.IsNullOrEmpty(s))
                    {
                        dataInputField.GetComponent<Image>().color = Color.white;
                    }
                    else
                    {
                        dataInputField.GetComponent<Image>().color = Color.red;
                    }
                }
            }
            else if (machine.getDataType() == DataType.Float)
            {
                print("data float raw "+s);
                float outFloat;
                bool response;
                response = float.TryParse(s, out outFloat);
                if (response)
                {
                    dataInputField.GetComponent<Image>().color = Color.green;
                    machine.setFloatData(outFloat);
                    print("data float " + outFloat);
                }
                else
                {
                    Debug.Log("Invalid input");
                    if (string.IsNullOrEmpty(s))
                    {
                        dataInputField.GetComponent<Image>().color = Color.white;
                    }
                    else
                    {
                        dataInputField.GetComponent<Image>().color = Color.red;
                    }
                }
            }
        }
    }

    public void updateOrderDisplay(Node n)
    {
        if (n.thingPlaced != null)
        {
            Machine currentMachine = n.thingPlaced.GetComponent<Machine>();

            if (currentMachine.type == MachineType.Belt)
            {
                n.orderDisplay.GetComponent<TextMeshPro>().text = "";
            }
            else
            {
                n.orderDisplay.GetComponent<TextMeshPro>().text = "" + currentMachine.order;
            }

        }
        else
        {
            n.orderDisplay.GetComponent<TextMeshPro>().text = "";
        }
        
    }

    public void leftClickInOrderMode(Node n)
    {
        if (orderMode && n.thingPlaced != null)
        {
            Machine currentMachine = n.thingPlaced.GetComponent<Machine>();

            if (currentMachine.type == MachineType.Belt) return;

            int currentOrder = currentMachine.order;
            currentMachine.order = Math.Min(currentOrder + 1, 100);
            n.orderDisplay.GetComponent<TextMeshPro>().text = ""+ currentMachine.order;
        }
    }

    public void rightClickInOrderMode(Node n)
    {
        if (orderMode && n.thingPlaced != null)
        {
            Machine currentMachine = n.thingPlaced.GetComponent<Machine>();

            if (currentMachine.type == MachineType.Belt) return;

            int currentOrder = currentMachine.order;
            currentMachine.order =  Math.Max(currentOrder - 1,1);
            n.orderDisplay.GetComponent<TextMeshPro>().text = "" + currentMachine.order;
            
            
        }
    }

    public void emptyInputField()
    {
        dataInputField.GetComponent<TMP_InputField>().text = "";
        dataInputField.GetComponent<Image>().color = Color.white;
    }

    public void refillInputField()
    {
        DataType dataType = selectedMachine.gameObject.GetComponent<Var>().getDataType();
        TMP_InputField textField = dataInputField.GetComponent<TMP_InputField>();


        if (selectedMachine.type == MachineType.Variable)
        {
            if (dataType == DataType.Int)
            {
                textField.text = "" + selectedMachine.gameObject.GetComponent<Var>().getIntData();
            }
            else if (dataType == DataType.Float)
            {
                textField.text = "" + selectedMachine.gameObject.GetComponent<Var>().getFloatData();
            }
            else if (dataType == DataType.Bool)
            {
                textField.text = "" + selectedMachine.gameObject.GetComponent<Var>().getBoolData();
            }
            else
            {
                dataInputField.GetComponent<TMP_InputField>().text = "";
            }

        }
        
    }

}
