using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Belt : Machine
{
    public override void GenerateGate()
    {
        List<DataType> dt = new List<DataType>();

        AssignGate(new Gate(GateType.Belt, Direction.North, dt), Direction.North);
        AssignGate(new Gate(GateType.Belt, Direction.East, dt), Direction.East);
        AssignGate(new Gate(GateType.Belt, Direction.West, dt), Direction.West);
        AssignGate(new Gate(GateType.Belt, Direction.South, dt), Direction.South);
    }

    protected override void Update()
    {
        base.Update();

        // Change gate type depends on connections

        if (gateDict.All(x => x.Value.gateType == GateType.Belt))
        {
            // Detect for a connection
            // Change any other gates to opposite type
        }
        else if (gateDict.Where(x => x.Value.gateType == GateType.Entrance).Count() > 1 ||
            gateDict.Where(x => x.Value.gateType == GateType.Exit).Count() > 1)
        {

        }
        else if (gateDict.Where(x => x.Value.gateType == GateType.Entrance).Count() == 1 &&
            gateDict.Where(x => x.Value.gateType == GateType.Exit).Count() == 1)

            // Check every gate for connection
            foreach (var g in gateDict)
        {
            /*
            Node n = PlaceObjectOnGrid.Instance.GetNode(transform);
            int x = (int)n.cellPosition.x;
            int z = (int)n.cellPosition.z;
            Machine target;
            
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
            */
        }
    }
}
