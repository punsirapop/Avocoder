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
            default:
                break;
        }
        return ans;
    }
}
