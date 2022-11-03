using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class If : Machine
{
    public override void GenerateGate()
    {
        List<DataType> dt = new List<DataType>();

        dt.Add(DataType.Bool);

        AssignGate(new Gate(GateType.Exit, Direction.South, dt), Direction.South);
    }
}
