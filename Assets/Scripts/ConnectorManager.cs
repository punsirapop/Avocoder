using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ConnectorManager : MonoBehaviour
{

    public static ConnectorManager Instance;
    [SerializeField] GameObject connector;
    [SerializeField] Transform storage;

    Machine machine;
    Machine target;

    public static List<Chain> chainsList = new List<Chain>();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
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

        updateAllChains();
    }

    // Check if opposing gate is valid
    public bool CheckGate(Gate g, Direction d)
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

    public void updateAllChains()
    {
        foreach (Chain chain in chainsList)
        {
            if (chain.exitDir != Direction.None)
            {
                chain.reset();
                chain.updateChain(chain.head, chain.exitDir, 0);

                // for debugging
                print("-----------------------");
                print("Chain: ");
                print(chain.chainID);
                print("Machine in this chain: ");
                foreach (List<Machine> machineList in chain.machineInChainDict.Values)
                {
                    foreach (Machine m in machineList)
                    {
                        print(m.myName);
                    }

                }
                print("-----------------------");
            }
            
        }

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

    public int chainLenth = 0;
    public int tailID;

    public int chainID; /*for order when transfering data, may not be needed*/
    public Dictionary<int,List<Machine>> machineInChainDict = new Dictionary<int, List<Machine>>(); /*keep machine and their order in chain*/
    /*public Dictionary<int, List<Direction>> gatesOfMachineInChain = new Dictionary<int, List<Direction>>();*/
    public Machine head;
    public Direction exitDir;

    public Chain(Machine headOfChain)
    {
        chainID = created;
        created++;
        List<Machine> machinesList = new List<Machine>();
        machineInChainDict[0] = machinesList;
        machineInChainDict[0].Add(headOfChain);
        chainLenth+=1;
        tailID = 0;
        head = headOfChain;
        exitDir = headOfChain.getExitGate();
        /*if (headOfChain.type!=MachineType.Belt)
        {
            Direction exitGateDir = headOfChain.getExitGate();
            if (exitGateDir != Direction.None)
            {
                gatesOfMachineInChain[0] = new List<Direction>();
                gatesOfMachineInChain[0].Add(exitGateDir);
                exitDir = exitGateDir;
            }
            else
            {
                Debug.Log("error chain without exit gate");
            }
        }*/
        ConnectorManager.chainsList.Add(this);
    }

    public void removeThisChainFromList()
    {
        ConnectorManager.chainsList.Remove(this);
    }

    public void addToChain(int placeOrderInChain,Machine machine)
    {
        tailID = placeOrderInChain;
        if (!machineInChainDict.ContainsKey(placeOrderInChain))
        {
            machineInChainDict[placeOrderInChain] = new List<Machine>();
            chainLenth += 1;
        }
        machineInChainDict[placeOrderInChain].Add(machine);
    }

    public void removeFromChainStartingFrom(int placeOrderInChain)
    {

        // get placeOrder to remove
        List<int> toRemove = new List<int>();
        foreach (KeyValuePair<int,List<Machine>> pair in machineInChainDict)
        {
            int order = pair.Key;
            /*List<Machine> machines = pair.Value;*/

            if (order >= placeOrderInChain)
            {
                toRemove.Add(order);
            }
        }
        // remove machines over placeOrderInChain from chain
        foreach (int keyToRemove in toRemove)
        {
            if (machineInChainDict.ContainsKey(keyToRemove))
            { // check key before removing it
                machineInChainDict.Remove(keyToRemove);
            }
        }
        
    }

    /* start at head of chain
     * scan exit gate of head
     * if something connect add to chain also keep track of len and order it is in and the gate used to connect too
     *      then scan all gate of new part of chain except the one we just connect
     *      foreach gate in newpart of chain
     *          if connected to input stop this part of chain at this gate
     *      
     * if not stop
     */
    public void updateChain(Machine machineToCheck, Direction gateDirToCheck,int currentTailID)
    {
        /*
         * 1. machine = headofchain
         * 2. check only exit of headofchain
         * 3. if connected; add newPart to chain and if gate of newPart is entrace stop
         * 4. then check newPart gates
         *    foreach gate in newPart
         *          updateChain(newPart,gate)
         * 5. if not connected stop
         */

        Node currentNode = PlaceObjectOnGrid.Instance.GetNode(machineToCheck.transform);
        Machine target = null;

        bool hasConnection = false;
        bool continueChain = true;
        Direction dirWithConnection = Direction.None;



        Gate g = machineToCheck.gateDict[gateDirToCheck];
        int x = (int)currentNode.cellPosition.x;
        int z = (int)currentNode.cellPosition.z;

        currentTailID++;

        // Categorized by direction
        switch (gateDirToCheck)
        {
            case Direction.North:
                z++;
                if (z <= 4)
                {
                    target = PlaceObjectOnGrid.Instance.nodes[x, z].thingPlaced?.GetComponent<Machine>();
                    // Check if target is available for connection
                    if (target != null && CheckGate(g, Direction.South,target))
                    {
                        if (target.gateDict[Direction.South].gateType == GateType.Entrance)
                        {
                            continueChain = false;
                        }
                        else if (target.gateDict[Direction.South].gateType == GateType.Exit)
                        {
                            continueChain = false;
                            Debug.Log("!!!! Error exit connected to another exit!");
                        }
                        addToChain(currentTailID, target);
                        dirWithConnection = Direction.South;
                        hasConnection = true;
                    }
                }
                break;
            case Direction.East:
                x++;
                if (x <= 4)
                {
                    target = PlaceObjectOnGrid.Instance.nodes[x, z].thingPlaced?.GetComponent<Machine>();
                    // Check if target is available for connection
                    if (target != null && CheckGate(g, Direction.West,target))
                    {
                        if (target.gateDict[Direction.West].gateType == GateType.Entrance)
                        {
                            continueChain = false;
                        }
                        else if (target.gateDict[Direction.West].gateType == GateType.Exit)
                        {
                            continueChain = false;
                            Debug.Log("!!!! Error exit connected to another exit!");
                        }
                        addToChain(currentTailID, target);
                        dirWithConnection = Direction.West;
                        hasConnection = true;
                    }
                }
                break;
            case Direction.South:
                z--;
                if (z >= 0)
                {
                    target = PlaceObjectOnGrid.Instance.nodes[x, z].thingPlaced?.GetComponent<Machine>();
                    // Check if target is available for connection
                    if (target != null && CheckGate(g, Direction.North, target))
                    {
                        if (target.gateDict[Direction.North].gateType == GateType.Entrance)
                        {
                            continueChain = false;
                        }
                        else if (target.gateDict[Direction.North].gateType == GateType.Exit)
                        {
                            continueChain = false;
                            Debug.Log("!!!! Error exit connected to another exit!");
                        }
                        addToChain(currentTailID, target);
                        dirWithConnection = Direction.North;
                        hasConnection = true;
                    }
                }
                break;
            case Direction.West:
                x--;
                if (x >= 0)
                {
                    target = PlaceObjectOnGrid.Instance.nodes[x, z].thingPlaced?.GetComponent<Machine>();
                    // Check if target is available for connection
                    if (target != null && CheckGate(g, Direction.East, target))
                    {
                        if (target.gateDict[Direction.East].gateType == GateType.Entrance)
                        {
                            continueChain = false;
                        }
                        else if (target.gateDict[Direction.East].gateType == GateType.Exit)
                        {
                            continueChain = false;
                            Debug.Log("!!!! Error exit connected to another exit!");
                        }
                        addToChain(currentTailID, target);
                        dirWithConnection = Direction.East;
                        hasConnection = true;
                    }
                }
                break;
            default:
                dirWithConnection = Direction.None;
                hasConnection = false;
                break;
        }       
        if (hasConnection && continueChain)
        {
            foreach (Direction dir in (Direction[]) Enum.GetValues(typeof(Direction)))
            {
                if (dir != dirWithConnection && dir != Direction.None)
                {
                    updateChain(target, dir, currentTailID);
                }
            }
                
        }
    }

    // Check if opposing gate is valid
    public bool CheckGate(Gate g, Direction d,Machine target)
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


    public void reset()
    {
        machineInChainDict.Clear();
        machineInChainDict[0] = new List<Machine>();
        machineInChainDict[0].Add(head);
        tailID = 0;
    }
}

/*
every exit is the chain head
every update
try to find end of chain and if new add to chain if found another exit raise error
 
 */