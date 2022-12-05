using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ConnectorManager : MonoBehaviour
{
    [SerializeField] GameObject connector;
    [SerializeField] Transform storage;

    Machine machine;
    Machine target;

    void Update()
    {
        // Reset belt
        foreach(Transform t in storage)
        {
            Destroy(t.gameObject);
        }

        // Loop every node
        foreach (var n in PlaceObjectOnGrid.Instance.nodes)
        {
            // Check if occupied
            if (n.thingPlaced != null)
            {
                machine = n.thingPlaced.GetComponent<Machine>();
                
                // Check every gate for connection
                foreach (var g in machine.gateDict)
                {
                    int x = (int)n.cellPosition.x;
                    int z = (int)n.cellPosition.z;

                    // Categorized by direction
                    switch (g.Value.direction)
                    {
                        case Direction.North:
                            z++;
                            if (z <= 4)
                            {
                                target = PlaceObjectOnGrid.Instance.nodes[x, z].thingPlaced?.GetComponent<Machine>();
                                // Check if target is available for connection
                                if (target != null && CheckGate(g.Value, Direction.South))
                                {
                                    Instantiate(connector, new Vector3(x, .3f, z - .5f), Quaternion.AngleAxis(90f, Vector3.up), storage);
                                }
                            }
                            break;
                        case Direction.East:
                            x++;
                            if (x <= 4)
                            {
                                target = PlaceObjectOnGrid.Instance.nodes[x, z].thingPlaced?.GetComponent<Machine>();
                                // Check if target is available for connection
                                if (target != null && CheckGate(g.Value, Direction.West))
                                {
                                    Instantiate(connector, new Vector3(x - .5f, .3f, z), Quaternion.AngleAxis(0f, Vector3.up), storage);
                                }
                            }
                            break;
                        case Direction.South:
                            z--;
                            if (z >= 0)
                            {
                                target = PlaceObjectOnGrid.Instance.nodes[x, z].thingPlaced?.GetComponent<Machine>();
                                // Check if target is available for connection
                                if (target != null && CheckGate(g.Value, Direction.North))
                                {
                                    Instantiate(connector, new Vector3(x, .3f, z + .5f), Quaternion.AngleAxis(90f, Vector3.up), storage);
                                }
                            }
                            break;
                        case Direction.West:
                            x--;
                            if (x >= 0)
                            {
                                target = PlaceObjectOnGrid.Instance.nodes[x, z].thingPlaced?.GetComponent<Machine>();
                                // Check if target is available for connection
                                if (target != null && CheckGate(g.Value, Direction.East))
                                {
                                    Instantiate(connector, new Vector3(x + .5f, .3f, z), Quaternion.AngleAxis(0f, Vector3.up), storage);
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    // Check if opposing gate is valid
    private bool CheckGate(Gate g, Direction d)
    {
        bool ans = false;

        if (!target.gateDict.ContainsKey(d)) return ans;

        var firstNotSecond = g.dataTypeList.Except(target.gateDict[d].dataTypeList).ToList();
        var secondNotFirst = target.gateDict[d].dataTypeList.Except(g.dataTypeList).ToList();
        bool ok = !firstNotSecond.Any() && !secondNotFirst.Any();

        switch (g.gateType)
        {
            case GateType.Entrance:
                if ((target.gateDict[d].gateType == GateType.Exit && ok) ||
                    target.gateDict[d].gateType == GateType.Belt)
                {
                    ans = true;
                }
                break;
            case GateType.Exit:
                if ((target.gateDict[d].gateType == GateType.Entrance && ok) ||
                    target.gateDict[d].gateType == GateType.Belt)
                {
                    ans = true;
                }
                break;
            case GateType.Belt:
                if (target.gateDict[d].gateType == GateType.Belt)
                {
                    ans = true;
                }
                break;
            default:
                break;
        }
        return ans;
    }
}





/*every output gate have a number or a unique identifier
every gate or belt connected to it have the same identifier as the output gate
when new machine/ belt connect to the chain copy data over
- also check if head exist connect to chain
- if head don't exist connect to the chainn

create chain object
	identifier start as null
	the head/output gate
	list of input gates
	list of all machine/gate in chain
	if two chain connect
	merge into one chain
	if one chain have identifier, it have priority
	if there are more than one identifier raise error

    every output gate is a chain
    update chain connection everytime belt or machine with input gate is placed

when to create new chain
    only create when thing placed is not adjecant to existing chain
    machine with output is placed
    belt is placed
 
when destory machine also remove from chain
when pick up also remove from chain
*/


public class Chain
{
    static int created = 0;

    public int chainID; /*for order when transfering data, may not be needed*/
    public Dictionary<int,Machine> machineInChainDict; /*keep machine and their order in chain*/
    public Dictionary<int, List<Direction>> gatesOfMachineInChain; /*for machine that is not belt*/

    public Chain(Machine headOfChain)
    {
        chainID = created;
        created++;

        machineInChainDict[0] = headOfChain;
        if (headOfChain.type!=MachineType.Belt)
        {
            Direction exitGateDir = headOfChain.getExitGate();
            if (exitGateDir != Direction.None)
            {
                gatesOfMachineInChain[0] = new List<Direction>();
                gatesOfMachineInChain[0].Add(exitGateDir);
            }
            else
            {
                Debug.Log("error chain without exit gate");
            }
        }
    }

    public void addToChain()
    {

    }

    public void removeFromChain()
    {

    }

    /*add any new Belt or machine to chain or if chain is cut also update that*/
    public void updateChain()
    {

    }
}

/*
every exit is the chain head
every update
try to find end of chain and if new add to chain if found another exit raise error
 
 */