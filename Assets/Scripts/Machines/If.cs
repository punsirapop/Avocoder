using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class If : Machine
{
    public List<Machine> ifSignalValidMachines = new List<Machine>();
    public override void GenerateGate()
    {
        List<DataType> dt = new List<DataType>();

        // AssignGate(new Gate(GateType.None, Direction.North, dt), Direction.North);
        // AssignGate(new Gate(GateType.None, Direction.East, dt), Direction.East);
        // AssignGate(new Gate(GateType.None, Direction.West, dt), Direction.West);
        entranceDataType.Add(DataType.Bool);

        dt.Add(DataType.Bool);

        AssignGate(new Gate(GateType.Entrance, Direction.North, dt), Direction.North);
    }

    public override void activate()
    {
        print("IF machine activated");
        // queue all machine with matching singal and passed condition to the next activation order
        // add the valid machinen found to this ifSignalValidMachines
    }
}
