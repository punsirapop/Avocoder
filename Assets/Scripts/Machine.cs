using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public enum MachineType
{
    Variable,
    Numeric,
    Comparison,
    Logical,
    Conditional,
    Loop,
    Other
}

public enum GateType
{
    Entrance,
    Exit
}

public enum DataType
{
    Int,
    Float,
    Bool,
    Char
}

public enum Direction
{
    None,
    North,
    South,
    West,
    East
}

public class Gate
{
    public Direction direction;
    public GateType gateType;
    public List<DataType> dataTypeList;

    public Gate(GateType gt)
    {
        gateType = gt;
    }
}

public class Machine : MonoBehaviour
{
    public MachineType type;

    [SerializeField] GameObject assignedGrid;

    Dictionary<Direction, Gate> gates;

    public virtual void AssignGateDir(Gate g, Direction d)
    {
        if (gates.ContainsKey(d))
        {
            gates[d].direction = g.direction;
        }
        else
        {
            gates.Add(d, g);
        }
    }

    public virtual void AssignThis()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, new Vector3(0, -10, 0), out hit);
        hit.collider.gameObject.SendMessage("AddMe", gameObject);
    }
}
