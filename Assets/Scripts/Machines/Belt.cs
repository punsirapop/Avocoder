using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Belt : Machine
{

    public bool inChain = false;

    public override void GenerateGate()
    {
        order = -1;
        List<DataType> dt = new List<DataType>();
        
        AssignGate(new Gate(GateType.Belt, Direction.North, dt), Direction.North);
        AssignGate(new Gate(GateType.Belt, Direction.East, dt), Direction.East);
        AssignGate(new Gate(GateType.Belt, Direction.West, dt), Direction.West);
        AssignGate(new Gate(GateType.Belt, Direction.South, dt), Direction.South);
    }

    public void resetOrderAndDataType()
    {
        order = -1;
        List<DataType> dt = new List<DataType>();
    }

    public override void activate()
    {
        print("belt activated");
    }

}