using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumOp : Machine
{
    /*public List<DataType> entranceDataType = new List<DataType>() { DataType.Int, DataType.Float };
    public List<DataType> exitDataType = new List<DataType>() { DataType.Int, DataType.Float };*/

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
}
