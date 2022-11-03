using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogOp : Machine
{
    public override void GenerateGate()
    {
        List<DataType> dt = new List<DataType>();

        dt.Add(DataType.Bool);

        AssignGate(new Gate(GateType.Entrance, Direction.East, dt), Direction.East);
        AssignGate(new Gate(GateType.Entrance, Direction.West, dt), Direction.West);
        AssignGate(new Gate(GateType.Exit, Direction.South, dt), Direction.South);
    }
}
