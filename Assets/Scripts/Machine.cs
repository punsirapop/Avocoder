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
    None,
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

    Dictionary<Direction, Gate> gates = new Dictionary<Direction, Gate>();

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

    public virtual void addGateToDirection(Gate g, Direction d)
    {
        if (gates.ContainsKey(d))
        {
            gates[d] = g;
        }
        else
        {
            gates.Add(d, g);
        }
    }

    public virtual GateType GetGateTypeAtDir(Direction d)
    {
        if (gates.ContainsKey(d))
        {
            return gates[d].gateType;
        }
        else
        {
            AssignGateDir(new Gate(GateType.None), d);
            return gates[d].gateType;
        }
        
    }

    public virtual void AssignThis()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, new Vector3(0, -10, 0), out hit);
        hit.collider.gameObject.SendMessage("AddMe", gameObject);
    }
}
