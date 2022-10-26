using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumOp : Machine
{
    public override void GenerateGate()
    {
        List<DataType> dt = new List<DataType>();

        dt.Add(DataType.Int);
        dt.Add(DataType.Float);

        new Gate(GateType.Entrance, Direction.East, dt);
        new Gate(GateType.Entrance, Direction.West, dt);
        new Gate(GateType.Exit, Direction.South, dt);
    }
}
