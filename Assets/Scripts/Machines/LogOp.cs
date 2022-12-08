using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LogOp : Machine
{
    public bool activated = false;
    public LogicalOperator operatorSelected;

    public override void GenerateGate()
    {
        List<DataType> dt = new List<DataType>();

        // AssignGate(new Gate(GateType.None, Direction.North, dt), Direction.North);

        entranceDataType.Add(DataType.Bool);
        exitDataType.Add(DataType.Bool);

        dt.Add(DataType.Bool);

        AssignGate(new Gate(GateType.Entrance, Direction.East, dt), Direction.East);
        AssignGate(new Gate(GateType.Entrance, Direction.West, dt), Direction.West);
        AssignGate(new Gate(GateType.Exit, Direction.South, dt), Direction.South);
    }

    public void selectOperator(LogicalOperator opSelected)
    {
        operatorSelected = opSelected;
    }

    public void activate()
    {
        if (operatorSelected == LogicalOperator.And)
        {

        }
        else if (operatorSelected == LogicalOperator.Or)
        {

        }
        activated = true;
    }

    public void acceptInput()
    {
        foreach (KeyValuePair<Direction, Gate> gate in gateDict)
        {
            
        }
    }

    public void releaseOutput()
    {
        foreach (KeyValuePair<Direction, Gate> gate in gateDict)
        {

        }
    }


}
