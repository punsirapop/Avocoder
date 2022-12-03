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
    Belt,
    None
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
        gateDict[d] = g;
        /*
        if (g.direction != d)
        {
            gateDict[d].direction = g.direction;
            g.direction = d;

            gateDict[gateDict[d].direction] = gateDict[d];
            gateDict[d] = g;
            /*
            AssignGate(gateDict[d], gateDict[d].direction);
            AssignGate(g, g.direction);
            
        }
        else
        {
            gateDict[d] = g;
        }
        */

        /*
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
        */
    }

    public void RemoveGate(Direction d)
    {
        gateDict.Remove(d);
    }

    public void Rotate(bool left)
    {
        Dictionary<Direction, Gate> gateDictN = new Dictionary<Direction, Gate>();

        int offset = left ? 3 : 5;
        foreach (var g in gateDict)
        {
            g.Value.direction = (Direction)((int)(g.Value.direction + offset) % 4);
            gateDictN.Add(g.Value.direction, g.Value);
        }

        gateDict = gateDictN;
    }

    /*
    public void Rotate(bool left)
    {
        
        Direction a = left ? Direction.East : Direction.West;
        Direction b = left ? Direction.West : Direction.East;

        /*
        if (gateDict.ContainsKey(Direction.North)) Debug.Log(gateDict[Direction.North].direction++);
        if (gateDict.ContainsKey(a)) Debug.Log(gateDict[a].direction++.ToString());
        if (gateDict.ContainsKey(Direction.South)) Debug.Log(gateDict[Direction.South].direction++);
        if (gateDict.ContainsKey(b)) Debug.Log(gateDict[b].direction++.ToString());
        
        
        AssignGate(gateDict[Direction.North], a);
        AssignGate(gateDict[a], Direction.South);
        AssignGate(gateDict[Direction.South], b);
        AssignGate(gateDict[b], Direction.North);
        
    }
    */

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
                if((int)d.direction != -1 && d.gateType != GateType.None)
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
