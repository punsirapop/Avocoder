using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumOp : Machine
{
    /*public List<DataType> entranceDataType = new List<DataType>() { DataType.Int, DataType.Float };
    public List<DataType> exitDataType = new List<DataType>() { DataType.Int, DataType.Float };*/

    List<string> availableOperators = new List<string>() { "+", "-", "*", "/", "%"};
    int selectedOperatorIndex = 0;
    public float output;

    public override void GenerateGate()
    {
        List<DataType> dt = new List<DataType>();
        
        entranceDataType.Add(DataType.Int);
        entranceDataType.Add(DataType.Float);

        exitDataType.Add(DataType.Int);
        exitDataType.Add(DataType.Float);
        // AssignGate(new Gate(GateType.None, Direction.North, dt), Direction.North);

        dt.Add(DataType.Int);
        dt.Add(DataType.Float);

        AssignGate(new Gate(GateType.Entrance, Direction.East, dt), Direction.East);
        AssignGate(new Gate(GateType.Entrance, Direction.West, dt), Direction.West);
        AssignGate(new Gate(GateType.Exit, Direction.South, dt), Direction.South);
    }

    public override void activate()
    {
        print("number operator activated");
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
        if (leftDataType == DataType.Int)
        {
            if (rightDataType == DataType.Int)
            {
                if (availableOperators[selectedOperatorIndex] == "+")
                {
                    output = leftIntData + rightIntData;
                }
                else if (availableOperators[selectedOperatorIndex] == "-")
                {
                    output = leftIntData - rightIntData;
                }
                else if (availableOperators[selectedOperatorIndex] == "*")
                {
                    output = leftIntData * rightIntData;
                }
                else if (availableOperators[selectedOperatorIndex] == "/")
                {
                    output = leftIntData / rightIntData;
                }
                else if (availableOperators[selectedOperatorIndex] == "%")
                {
                    output = leftIntData % rightIntData;
                }
            }
            else if (rightDataType == DataType.Float)
            {
                if (availableOperators[selectedOperatorIndex] == "+")
                {
                    output = leftIntData + rightFloatData;
                }
                else if (availableOperators[selectedOperatorIndex] == "-")
                {
                    output = leftIntData - rightFloatData;
                }
                else if (availableOperators[selectedOperatorIndex] == "*")
                {
                    output = leftIntData * rightFloatData;
                }
                else if (availableOperators[selectedOperatorIndex] == "/")
                {
                    output = leftIntData / rightFloatData;
                }
                else if (availableOperators[selectedOperatorIndex] == "%")
                {
                    output = leftIntData % rightFloatData;
                }
            }
        }
        else if (leftDataType == DataType.Float)
        {
            if (rightDataType == DataType.Int)
            {
                if (availableOperators[selectedOperatorIndex] == "+")
                {
                    output = leftFloatData + rightIntData;
                }
                else if (availableOperators[selectedOperatorIndex] == "-")
                {
                    output = leftFloatData - rightIntData;
                }
                else if (availableOperators[selectedOperatorIndex] == "*")
                {
                    output = leftFloatData * rightIntData;
                }
                else if (availableOperators[selectedOperatorIndex] == "/")
                {
                    output = leftFloatData / rightIntData;
                }
                else if (availableOperators[selectedOperatorIndex] == "%")
                {
                    output = leftFloatData % rightIntData;
                }
            }
            else if (rightDataType == DataType.Float)
            {
                if (availableOperators[selectedOperatorIndex] == "+")
                {
                    output = leftFloatData + rightFloatData;
                }
                else if (availableOperators[selectedOperatorIndex] == "-")
                {
                    output = leftFloatData - rightFloatData;
                }
                else if (availableOperators[selectedOperatorIndex] == "*")
                {
                    output = leftFloatData * rightFloatData;
                }
                else if (availableOperators[selectedOperatorIndex] == "/")
                {
                    output = leftFloatData / rightFloatData;
                }
                else if (availableOperators[selectedOperatorIndex] == "%")
                {
                    output = leftFloatData % rightFloatData;
                }
            }
        }
        print("number operator activated with output: "+output);
    }

    public void toggleOperator()
    {
        selectedOperatorIndex = (selectedOperatorIndex + 1) % availableOperators.Count;
    }
    public string getCurrentSign()
    {
        return availableOperators[selectedOperatorIndex];
    }

    public float getOutput()
    {
        return output;
    }


    public void updateCenterDisplay()
    {
        centerDisplay.text = getCurrentSign();
    }



}
