using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Var : Machine
{

    public int intData;
    public float floatData;
    public bool boolData;

    public List<DataType> possibleDataType = new List<DataType>() { DataType.Int,DataType.Float,DataType.Bool};
    public int dataTypeIndex = 0;

    public override void GenerateGate()
    {
        List<DataType> dt = new List<DataType>();

        // AssignGate(new Gate(GateType.None, Direction.North, dt), Direction.North);
        // AssignGate(new Gate(GateType.None, Direction.East, dt), Direction.East);
        // AssignGate(new Gate(GateType.None, Direction.South, dt), Direction.South);
        // AssignGate(new Gate(GateType.None, Direction.West, dt), Direction.West);
    }
    public override void activate()
    {
        print("variable activated");
    }

    public void setDataType(DataType newDataType)
    {
        int count = 0;
        foreach (DataType dt in possibleDataType)
        {

            if (dt == newDataType)
            {
                dataTypeIndex = count;
                break;
            }
            count++;
        }
    }

    public void setIntData(int newIntData)
    {
        intData = newIntData;
    }

    public void setFloatData(float newFloatData)
    {
        floatData = newFloatData;
    }

    public void setBoolData(bool newBoolData)
    {
        boolData = newBoolData;
    }

    public int getIntData()
    {
        return intData;
    }

    public float getFloatData()
    {
        return floatData;
    }

    public bool getBoolData()
    {
        return boolData;
    }

    public DataType getDataType()
    {
        return possibleDataType[dataTypeIndex];
    }

    public void toggleBool()
    {
        boolData = !boolData;
    }
    
    public void toggleDataType()
    {
        dataTypeIndex = (dataTypeIndex + 1) % possibleDataType.Count;
    }
}
