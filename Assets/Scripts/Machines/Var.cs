using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Var : Machine
{
    public override void GenerateGate()
    {
        List<DataType> dt = new List<DataType>();

        // AssignGate(new Gate(GateType.None, Direction.North, dt), Direction.North);
        // AssignGate(new Gate(GateType.None, Direction.East, dt), Direction.East);
        // AssignGate(new Gate(GateType.None, Direction.South, dt), Direction.South);
        // AssignGate(new Gate(GateType.None, Direction.West, dt), Direction.West);
    }
}
