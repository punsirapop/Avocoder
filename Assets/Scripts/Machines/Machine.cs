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
    If,
    Function,
    Belt
}

public enum GateType
{
    Entrance,
    Exit,
    Belt
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
    None = -1,
    North,
    East,
    South,
    West
}

public enum LogicalOperator
{
    And,
    Or,
}

public class Gate
{
    public Direction direction;
    public GateType gateType;
    public List<DataType> dataTypeList;
    public Transform connection;

    public Gate(GateType gt, Direction d, List<DataType> dt)
    {
        gateType = gt;
        direction = d;
        dataTypeList = dt;
        connection = null;
    }
}

abstract public class Machine : MonoBehaviour
{
    public MachineType type;
    // public int[] gateNo;

    // temp for debugging
    static int created = 0;
    public int myName = 0;
    public int order = 1;

    public Dictionary<Direction, Gate> gateDict = new Dictionary<Direction, Gate>();

    [SerializeField] Transform[] gatePos;
    [SerializeField] Material[] gateMat;

    private void Awake()
    {
        created++;
        myName = created;

        GenerateGate();
    }

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
        foreach (Transform g in gatePos)
        {
            g.gameObject.SetActive(false);
        }

        if(type != MachineType.Belt)
        {
            foreach (var d in gateDict.Values)
            {
                if((int)d.direction != -1)
                {
                    gatePos[(int)d.direction].gameObject.SetActive(true);
                    gatePos[(int)d.direction].GetComponent<MeshRenderer>().material = gateMat[(int)d.gateType];
                }
            }
        }
        
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
