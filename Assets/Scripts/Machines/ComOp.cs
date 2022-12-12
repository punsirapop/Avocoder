using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComOp : Machine
{
    List<string> availableSigns = new List<string>() { ">" , "<", "==", ">=","<=","!="};
    int selectedSignIndex = 0;
    public bool output;
    public override void GenerateGate()
    {
        List<DataType> dt = new List<DataType>();

        // AssignGate(new Gate(GateType.None, Direction.North, dt), Direction.North);

        entranceDataType.Add(DataType.Int);
        entranceDataType.Add(DataType.Float);
        exitDataType.Add(DataType.Bool);


        dt.Add(DataType.Int);
        dt.Add(DataType.Float);
        dt.Add(DataType.Bool);

        AssignGate(new Gate(GateType.Entrance, Direction.East, entranceDataType), Direction.East);
        AssignGate(new Gate(GateType.Entrance, Direction.West, entranceDataType), Direction.West);
        AssignGate(new Gate(GateType.Exit, Direction.South, exitDataType), Direction.South);
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
        DataType leftDataType = left.getData(out leftIntData,out leftFloatData,out leftBoolData);
        DataType rightDataType = right.getData(out rightIntData, out rightFloatData, out rightBoolData);
        if (leftDataType == DataType.Int)
        {
            if (rightDataType == DataType.Int)
            {
                if (availableSigns[selectedSignIndex] == ">")
                {
                    output = leftIntData > rightIntData ? true : false;
                }
                else if (availableSigns[selectedSignIndex] == "<")
                {
                    output = leftIntData < rightIntData ? true : false;
                }
                else if (availableSigns[selectedSignIndex] == "==")
                {
                    output = leftIntData == rightIntData ? true : false;
                }
                else if (availableSigns[selectedSignIndex] == ">=")
                {
                    output = leftIntData >= rightIntData ? true : false;
                }
                else if (availableSigns[selectedSignIndex] == "<=")
                {
                    output = leftIntData <= rightIntData ? true : false;
                }
                else if (availableSigns[selectedSignIndex] == "!=")
                {
                    output = leftIntData != rightIntData ? true : false;
                }
            }
            else if (rightDataType == DataType.Float)
            {
                if (availableSigns[selectedSignIndex] == ">")
                {
                    output = leftIntData > rightFloatData ? true : false;
                }
                else if (availableSigns[selectedSignIndex] == "<")
                {
                    output = leftIntData < rightFloatData ? true : false;
                }
                else if (availableSigns[selectedSignIndex] == "==")
                {
                    output = leftIntData == rightFloatData ? true : false;
                }
                else if (availableSigns[selectedSignIndex] == ">=")
                {
                    output = leftIntData >= rightFloatData ? true : false;
                }
                else if (availableSigns[selectedSignIndex] == "<=")
                {
                    output = leftIntData <= rightFloatData ? true : false;
                }
                else if (availableSigns[selectedSignIndex] == "!=")
                {
                    output = leftIntData != rightFloatData ? true : false;
                }
            }
        }
        else if (leftDataType == DataType.Float)
        {
            if (rightDataType == DataType.Int)
            {
                if (availableSigns[selectedSignIndex] == ">")
                {
                    output = leftFloatData > rightIntData ? true : false;
                }
                else if (availableSigns[selectedSignIndex] == "<")
                {
                    output = leftFloatData < rightIntData ? true : false;
                }
                else if (availableSigns[selectedSignIndex] == "==")
                {
                    output = leftFloatData == rightIntData ? true : false;
                }
                else if (availableSigns[selectedSignIndex] == ">=")
                {
                    output = leftFloatData >= rightIntData ? true : false;
                }
                else if (availableSigns[selectedSignIndex] == "<=")
                {
                    output = leftFloatData <= rightIntData ? true : false;
                }
                else if (availableSigns[selectedSignIndex] == "!=")
                {
                    output = leftFloatData != rightIntData ? true : false;
                }
            }
            else if (rightDataType == DataType.Float)
            {
                if (availableSigns[selectedSignIndex] == ">")
                {
                    output = leftFloatData > rightFloatData ? true : false;
                }
                else if (availableSigns[selectedSignIndex] == "<")
                {
                    output = leftFloatData < rightFloatData ? true : false;
                }
                else if (availableSigns[selectedSignIndex] == "==")
                {
                    output = leftFloatData == rightFloatData ? true : false;
                }
                else if (availableSigns[selectedSignIndex] == ">=")
                {
                    output = leftFloatData >= rightFloatData ? true : false;
                }
                else if (availableSigns[selectedSignIndex] == "<=")
                {
                    output = leftFloatData <= rightFloatData ? true : false;
                }
                else if (availableSigns[selectedSignIndex] == "!=")
                {
                    output = leftFloatData != rightFloatData ? true : false;
                }
            }
        }

    }

    public void toggleSign()
    {
        selectedSignIndex = (selectedSignIndex+1)%availableSigns.Count;
    }
    public string getCurrentSign()
    {
        return availableSigns[selectedSignIndex];
    }

    public bool getOutput()
    {
        return output;
    }
}
