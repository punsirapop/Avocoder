 using System.Collections;
using System.Collections.Generic;
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

    public Gate(GateType gt, Direction d, List<DataType> dt)
    {
        gateType = gt;
        direction = d;
        dataTypeList = dt;
    }
}

abstract public class Machine : MonoBehaviour
{
    public MachineType type;
    public int[] gateNo;

    // temp for debugging
    static int created = 0;
    public int myName = 0;
    public int order = 1;
    private void Awake()
    {
        created++;
        myName = created;
    }

    [SerializeField] GameObject[] gateModels;

    Dictionary<Direction, Gate> gateDict = new Dictionary<Direction, Gate>();

    public abstract void GenerateGate();

    public void AssignGate(Gate g, Direction d)
    {
        // switching 2 gates
        if (gateDict.ContainsKey(d))
        {
            gateDict.Remove(g.direction);
            gateDict[d].direction = g.direction;
            g.direction = d;

            AssignGate(gateDict[d], gateDict[d].direction);
            AssignGate(g, g.direction);
        }
        // move gate to empty space
        else
        {
            gateDict.Remove(g.direction);
            g.direction = d;
            gateDict.Add(d, g);
        }
    }

    public void RemoveGate(Direction d)
    {
        gateDict.Remove(d);
    }

    public virtual void Update()
    {

    }

    /*
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

    public GateType GetGateTypeAtDir(Direction d)
    {
        if (gateDict.ContainsKey(d))
        {
            return gateDict[d].gateType;
        }
        else
        {
            AssignGate(new Gate(GateType.None), d);
            return gateDict[d].gateType;
        }
    }
    */
}
