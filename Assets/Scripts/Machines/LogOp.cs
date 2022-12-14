using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LogOp : Machine
{
    List<string> availableLogicalOperator = new List<string>() { "and", "or" };
    int selectedOperatorIndex = 0;
    public bool output;
    
    public override void GenerateGate()
    {
        List<DataType> dt = new List<DataType>();

        // AssignGate(new Gate(GateType.None, Direction.North, dt), Direction.North);

        entranceDataType.Add(DataType.Bool);
        exitDataType.Add(DataType.Bool);

        dt.Add(DataType.Bool);

        AssignGate(new Gate(GateType.Entrance, Direction.East, dt), Direction.East);
        AssignGate(new Gate(GateType.Entrance, Direction.West, dt), Direction.West);
        AssignGate(new Gate(GateType.Exit, Direction.South, dt), Direction.South);
    }

    public override void activate()
    {
        print("comparison operator activated");
        Gate left = gateDict[Direction.West];
        Gate right = gateDict[Direction.East];
        int leftIntData;
        float leftFloatData;
        bool leftBoolData;
        int rightIntData;
        float rightFloatData;
        bool rightBoolData;
        DataType leftDataType = left.getData(out leftIntData, out leftFloatData, out leftBoolData);
        DataType rightDataType = right.getData(out rightIntData, out rightFloatData, out rightBoolData);

        if (availableLogicalOperator[selectedOperatorIndex] == "and")
        {
            output = leftBoolData && rightBoolData;
        }
        else if (availableLogicalOperator[selectedOperatorIndex] == "or")
        {
            output = leftBoolData || rightBoolData;
        }

    }


    public void toggleSign()
    {
        selectedOperatorIndex = (selectedOperatorIndex + 1) % availableLogicalOperator.Count;
    }
    public string getCurrentSign()
    {
        return availableLogicalOperator[selectedOperatorIndex];
    }

    public bool getOutput()
    {
        return output;
    }

    public void updateCenterDisplay()
    {
        centerDisplay.text = getCurrentSign();
    }


}
