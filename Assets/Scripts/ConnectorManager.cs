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
        updateAllChains();

        // Reset belt
        foreach (Transform t in storage)
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
                                if (target != null && CheckGate(g.Value, Direction.South, target))
                                {
                                    print("check north found target: "+target.name);
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
                                if (target != null && CheckGate(g.Value, Direction.West, target))
                                {
                                    print("check east found target: " + target.name);
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
                                if (target != null && CheckGate(g.Value, Direction.North, target))
                                {
                                    print("check south found target: " + target.name);
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
                                if (target != null && CheckGate(g.Value, Direction.East, target))
                                {
                                    print("check west found target: " + target.name);
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
    public bool CheckGate(Gate g, Direction d, Machine target)
    {
        bool ans = false;
        bool ok = false;
        bool inChain = true;


        if (!target.gateDict.ContainsKey(d)) return ans;


        if (target.type == MachineType.Belt)
        {
            Belt b = target.gameObject.GetComponent<Belt>();
            inChain = b.inChain;
            Debug.Log("check " + d + " gate in chain?: " + inChain);
        }

        /*var firstNotSecond = g.dataTypeList.Except(target.gateDict[d].dataTypeList).ToList();
        var secondNotFirst = target.gateDict[d].dataTypeList.Except(g.dataTypeList).ToList();
        bool ok = !firstNotSecond.Any() && !secondNotFirst.Any();*/

        foreach (DataType type in g.dataTypeList)
        {
            //Debug.Log("check west gate selected: " + type);
            foreach (DataType targetType in target.gateDict[d].dataTypeList)
            {
                //Debug.Log("check west gate target: " + type);
                if (type == targetType)
                {
                    Debug.Log("check "+d+" gate same data type: " + type);
                    ok = true;
                    break;
                }
            }
        }
        Debug.Log("check "+ d+" gate belt: " + ok);

        switch (g.gateType)
        {
            case GateType.Entrance:

                if ((target.gateDict[d].gateType == GateType.Exit && ok) ||
                    (target.gateDict[d].gateType == GateType.Belt && (!inChain || (inChain && ok))))
                {
                    ans = true;
                }
                break;
            case GateType.Exit:
                if ((target.gateDict[d].gateType == GateType.Entrance && ok) ||
                    (target.gateDict[d].gateType == GateType.Belt && (!inChain || (inChain && ok))))
                {
                    ans = true;
                }
                break;
            case GateType.Belt:
                if ((target.gateDict[d].gateType == GateType.Belt && (!inChain || (inChain && ok))) ||
                    (target.gateDict[d].gateType == GateType.Entrance && (!inChain || (inChain && ok))) ||
                    (target.gateDict[d].gateType == GateType.Exit && (!inChain || (inChain && ok))))
                {
                    ans = true;
                }
                break;
            default:
                break;
        }
        Debug.Log("check " + d + " gate belt ans: " + ans);
        Debug.Log("1 Connecting to " + target.name + ": " + ans);
        Debug.Log("1 Connecting gate " + g.gateType + " to " + target.gateDict[d].gateType + ": " + ans);
        
        if (target.gateDict[d].gateType == GateType.None || g.gateType == GateType.None) {
            ans = false;
            }

            return ans;
    }

    public void updateAllChains()
    {
        resetBeltInChain();
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
                foreach (KeyValuePair<int, List<Machine>> keypair in chain.machineInChainDict)
                {
                    int chainOrder = keypair.Key;
                    List<Machine> machineList = keypair.Value;
                    foreach (Machine m in machineList)
                    {
                        print(m.myName);
                        if (m.type == MachineType.Belt)
                        {
                            foreach (Direction d in (Direction[])Enum.GetValues(typeof(Direction)))
                            {
                                if (d == Direction.None) break;
                                string result = "     " + d + " dataType: ";
                                foreach (var item in m.gateDict[d].dataTypeList)
                                {
                                    result += item.ToString() + ", ";
                                }
                                Debug.Log(result);
                            }
                        }

                        if (m.type != MachineType.Belt)
                        {
                            foreach (Direction d in chain.gatesOfMachineInChain[chainOrder][m.myName])
                            {
                                string result = "     " + d + " dataType: ";
                                foreach (var item in m.gateDict[d].dataTypeList)
                                {
                                    result += item.ToString() + ", ";
                                }
                                Debug.Log(result);
                            }
                        }
                    }

                }
                print("-----------------------");
            }
            printBeltInChain();
            
        }


    }

    public void resetBeltInChain()
    {
        foreach (GameObject machineObj in MachineActivationManager.allMachineList)
        {
            Machine m = machineObj.GetComponent<Machine>();
            if (m.type == MachineType.Belt)
            {
                Belt b = machineObj.GetComponent<Belt>();
                b.inChain = false;
                b.resetOrderAndDataType();
            }
        }
    }
    public void printBeltInChain()
    {
        foreach (GameObject machineObj in MachineActivationManager.allMachineList)
        {
            Machine m = machineObj.GetComponent<Machine>();
            if (m.type == MachineType.Belt)
            {
                Belt b = machineObj.GetComponent<Belt>();
                if (b.inChain)
                {
                    Debug.Log("Belt in chain: "+b.myName);
                }
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
    public List<int> machineNameList = new List<int>();
    public Dictionary<int, Dictionary<int,List<Direction>>> gatesOfMachineInChain = new Dictionary<int, Dictionary<int, List<Direction>>>();
    public Machine head;
    public Direction exitDir;
    public Gate exitGate;
    public List<Gate> allEntranceGates = new List<Gate>();

    public Chain(Machine headOfChain)
    {
        chainID = created;
        created++;

        machineNameList.Add(headOfChain.myName);

        List<Machine> machinesList = new List<Machine>();
        machineInChainDict[0] = machinesList;
        machineInChainDict[0].Add(headOfChain);
        chainLenth+=1;
        tailID = 0;
        head = headOfChain;
        exitDir = headOfChain.getExitGate();
        if (headOfChain.type != MachineType.Belt)
        {
            Direction exitGateDir = headOfChain.getExitGate();
            if (exitGateDir != Direction.None)
            {
                gatesOfMachineInChain[0] = new Dictionary<int, List<Direction>>();
                List<Direction> dirList = new List<Direction>();
                gatesOfMachineInChain[0].Add(headOfChain.myName, dirList);
                gatesOfMachineInChain[0][headOfChain.myName].Add(exitGateDir);
                exitDir = exitGateDir;
            }
            else
            {
                Debug.Log("error chain without exit gate");
            }
        }
        ConnectorManager.chainsList.Add(this);
    }

    public void transferData()
    {
        if (head.type == MachineType.Variable)
        {
            Debug.Log("transfer variable");
            Var variable = head.gameObject.GetComponent<Var>();
            DataType dataType = variable.getDataType();
            if (dataType == DataType.Int)
            {
                int intData = variable.getIntData();
                assignAllEntranceGateWithThisData(DataType.Int, intData,0,false);
                return;
            }
            else if (dataType == DataType.Float)
            {
                float floatData = variable.getFloatData();
                assignAllEntranceGateWithThisData(DataType.Float, 0, floatData, false);
                return;
            }
            else if (dataType == DataType.Bool)
            {
                bool boolData = variable.getBoolData();
                assignAllEntranceGateWithThisData(DataType.Bool, 0, 0, boolData);
                return;
            }
            
        }
        else if (head.type == MachineType.Comparison)
        {
            ComOp comOp = head.gameObject.GetComponent<ComOp>();
            bool output = comOp.getOutput();
            assignAllEntranceGateWithThisData(DataType.Bool, 0, 0, output);
            return;
        }
        else if (head.type == MachineType.Numeric)
        {
            NumOp numOp = head.gameObject.GetComponent<NumOp>();
            float output = numOp.getOutput();
            assignAllEntranceGateWithThisData(DataType.Float, (int)output, output, false);
            return;
        }
        else if (head.type == MachineType.Logical)
        {
            LogOp logOp = head.gameObject.GetComponent<LogOp>();
            bool output = logOp.getOutput();
            assignAllEntranceGateWithThisData(DataType.Float, 0, 0, output);
            return;
        }
        
    }
    
    public void assignAllEntranceGateWithThisData(DataType dataType, int intData, float floatData, bool boolData)
    {
        foreach (Gate g in allEntranceGates)
        {
            g.assignData(dataType, intData, floatData, boolData);
            if (g.onVar)
            {
                Debug.Log("assign data to var");
                g.onVar.setIntData(intData);
                g.onVar.setFloatData(floatData);
                g.onVar.setBoolData(boolData);
            }
        }
    }


    public void removeThisChainFromList()
    {
        ConnectorManager.chainsList.Remove(this);
    }

    public void addToChain(int placeOrderInChain,Machine machine,Direction gateDir)
    {
        Debug.Log("Adding: "+machine.type);
        if (machine.type == MachineType.Logical)
        {
            Debug.Log("found logical: "+machine.myName);
        }

        if (machine.type == MachineType.Belt)
        {
            Belt b = machine.gameObject.GetComponent<Belt>();
            if (!b.inChain)
            {
                b.inChain = true;
                foreach (Direction dir in (Direction[])Enum.GetValues(typeof(Direction)))
                {
                    if (dir != Direction.None && head.gateDict.ContainsKey(exitDir) && exitDir != Direction.None)
                    {
                        b.gateDict[dir].dataTypeList =  head.gateDict[exitDir].dataTypeList;
                    }
                }
            }
        }

        tailID = placeOrderInChain;
        if (!machineInChainDict.ContainsKey(placeOrderInChain))
        {
            machineInChainDict[placeOrderInChain] = new List<Machine>();
            chainLenth += 1;
        }
        if (!machineInChainDict[placeOrderInChain].Contains(machine))
        {
            machineInChainDict[placeOrderInChain].Add(machine);
        }
        if (!machineNameList.Contains(machine.myName))
        {
            machineNameList.Add(machine.myName);
        }

        if (machine.type != MachineType.Belt && gateDir != Direction.None)
        {
            Debug.Log("Adding not belt: " + machine.type);
            if (!gatesOfMachineInChain.ContainsKey(placeOrderInChain))
            {
                gatesOfMachineInChain[placeOrderInChain] = new Dictionary<int, List<Direction>>();
            }

            if (!gatesOfMachineInChain[placeOrderInChain].ContainsKey(machine.myName))
            {
                gatesOfMachineInChain[placeOrderInChain][machine.myName] = new List<Direction>();
            }
            
            gatesOfMachineInChain[placeOrderInChain][machine.myName].Add(gateDir);

            Debug.Log("Before Adding entrance gate" + machine.gateDict[gateDir].gateType);
            if (machine.gateDict[gateDir].gateType == GateType.Entrance)
            {
                Debug.Log("Adding entrance gate");
                allEntranceGates.Add(machine.gateDict[gateDir]);
            }

        }
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
        if (gateDirToCheck == Direction.None) return;
        Debug.Log("updating machine "+ machineToCheck.myName +" of type "+machineToCheck.type+" at dir "+gateDirToCheck);
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
                    checkDir(x, z, Direction.South, ref continueChain, ref currentTailID, ref target, ref dirWithConnection, ref g, ref hasConnection);
                }
                else return;
                break;
            case Direction.East:
                x++;
                if (x <= 4)
                {
                    checkDir(x, z, Direction.West, ref continueChain, ref currentTailID, ref target, ref dirWithConnection, ref g, ref hasConnection);
                }
                else return;
                break;
            case Direction.South:
                z--;
                if (z >= 0)
                {
                    checkDir(x, z, Direction.North, ref continueChain, ref currentTailID, ref target, ref dirWithConnection, ref g, ref hasConnection);
                }
                else return;
                break;
            case Direction.West:
                x--;
                if (x >= 0)
                {
                    checkDir(x, z, Direction.East, ref continueChain, ref currentTailID, ref target, ref dirWithConnection, ref g, ref hasConnection);
                }
                else return;
                break;
            default:
                break;
        }
        if (hasConnection && continueChain)
        {
            foreach (Direction dir in (Direction[]) Enum.GetValues(typeof(Direction)))
            {
                if (dir != dirWithConnection && dir != Direction.None && dirWithConnection != Direction.None)
                {
                    Debug.Log("Thing sending in update chain"+target.myName+" "+dir+" "+currentTailID);
                    updateChain(target, dir, currentTailID);
                }
            }
                
        }
    }

    public void checkDir(int x, int z, Direction dir, ref bool continueChain, ref int currentTailID, ref Machine target, ref Direction dirWithConnection, ref Gate g, ref bool hasConnection)
    {
        target = PlaceObjectOnGrid.Instance.nodes[x, z].thingPlaced?.GetComponent<Machine>();
        if (target == null) return;

        // Check if target is available for connection
        if ((target != null && CheckGate(g, dir, target)))
        {
            dirWithConnection = dir;
            Debug.Log("target found machine" + target.myName + "of type " + target.type + " at dir " + dirWithConnection);
            Debug.Log("target gate type" + target.gateDict[dir].gateType);
            if (target.gateDict[dir].gateType == GateType.Entrance)
            {
                Debug.Log("Found entrance gate");
                continueChain = false;
            }
            else if (target.gateDict[dir].gateType == GateType.Exit)
            {
                continueChain = false;
                Debug.Log("!!!! Error exit connected to another exit!");
            }


            if (!machineNameList.Contains(target.myName))
            {
                addToChain(currentTailID, target, dir);
                hasConnection = true;
            }
            else if (target.type != MachineType.Belt)
            {
                addToChain(currentTailID, target, dir);
            }
            else
            {
                Debug.Log("!!!! Error loop in chain");
            }
        }
    }


    // Check if opposing gate is valid
    public bool CheckGate(Gate g, Direction d, Machine target)
    {
        bool ans = false;
        bool ok = false;
        bool inChain = true;

        if (!target.gateDict.ContainsKey(d)) return ans;

        if (target.type == MachineType.Belt)
        {
            Belt b = target.gameObject.GetComponent<Belt>();
            inChain = b.inChain;
        }

        /*var firstNotSecond = g.dataTypeList.Except(target.gateDict[d].dataTypeList).ToList();
        var secondNotFirst = target.gateDict[d].dataTypeList.Except(g.dataTypeList).ToList();
        bool ok = !firstNotSecond.Any() && !secondNotFirst.Any();*/

        foreach (DataType type in g.dataTypeList)
        {
            Debug.Log("check chain selected: " + type);
            foreach (DataType targetType in target.gateDict[d].dataTypeList)
            {
                Debug.Log("check chain target: " + type);
                if (type == targetType)
                {
                    Debug.Log("check chain same type: " + type);
                    ok = true;
                    break;
                }
            }
        }
        Debug.Log("check chain belt: " + ok);

        switch (g.gateType)
        {
            case GateType.Entrance:

                if ((target.gateDict[d].gateType == GateType.Exit && ok) ||
                    (target.gateDict[d].gateType == GateType.Belt && (!inChain || (inChain && ok))))
                {
                    ans = true;
                }
                break;
            case GateType.Exit:
                if ((target.gateDict[d].gateType == GateType.Entrance && ok) ||
                    (target.gateDict[d].gateType == GateType.Belt && (!inChain || (inChain && ok))))
                {
                    ans = true;
                }
                break;
            case GateType.Belt:
                if (target.gateDict[d].gateType == GateType.Belt && (!inChain || (inChain && ok)) ||
                    (target.gateDict[d].gateType == GateType.Entrance && (!inChain || (inChain && ok))) ||
                    (target.gateDict[d].gateType == GateType.Exit && (!inChain || (inChain && ok))))
                {
                    ans = true;
                }
                break;
            default:
                break;
        }
        Debug.Log("2 Connecting to "+target.name+": "+ans);
        return ans;
    }


    public void reset()
    {
        machineNameList.Clear();
        machineNameList.Add(head.myName);
        machineInChainDict.Clear();
        machineInChainDict[0] = new List<Machine>();
        machineInChainDict[0].Add(head);
        gatesOfMachineInChain.Clear();
        Direction exitGateDir = head.getExitGate();
        gatesOfMachineInChain[0] = new Dictionary<int, List<Direction>>();
        List<Direction> dirList = new List<Direction>();
        gatesOfMachineInChain[0].Add(head.myName, dirList);
        gatesOfMachineInChain[0][head.myName].Add(exitGateDir);
        exitDir = exitGateDir;
        tailID = 0;

        allEntranceGates.Clear();
    }
}

/*
every exit is the chain head
every update
try to find end of chain and if new add to chain if found another exit raise error
 
 */