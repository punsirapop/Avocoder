using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComOp : Machine
{

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
    }
}
