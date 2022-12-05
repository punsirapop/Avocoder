using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Belt : Machine
{
    Machine outputDirection = null;
    Machine inputDirection = null;


    public override void GenerateGate()
    {
        List<DataType> dt = new List<DataType>();

        AssignGate(new Gate(GateType.Belt, Direction.North, dt), Direction.North);
        AssignGate(new Gate(GateType.Belt, Direction.East, dt), Direction.East);
        AssignGate(new Gate(GateType.Belt, Direction.West, dt), Direction.West);
        AssignGate(new Gate(GateType.Belt, Direction.South, dt), Direction.South);
    }

    public void updateInputOutputDirection()
    {

    }

}


/*
when place solo belt all gate should be output
when connected to another output auto have gate be input
when machine output is place on chain fix gate input output of belts
 
 
 */