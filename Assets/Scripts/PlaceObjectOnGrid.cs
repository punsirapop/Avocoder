using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlaceObjectOnGrid : MonoBehaviour
{
    public static PlaceObjectOnGrid Instance;

    public Transform gridCellPrefab;
    public Transform gridHolder;
    public Transform[] components;

    public Transform orderDisplayPrefab;
    public Transform orderDisplayHolder;

    public Transform playerHolding;
    public Transform selectedMachineForConfig;
    public Vector3 smoothMousePosition;
    public Vector3 gridMousePosition;
    [SerializeField] private int height;
    [SerializeField] private int width;

    private Vector3 mousePosition;
    public Node[,] nodes;
    private Plane plane;

    public GameObject configTab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
        plane = new Plane(inNormal: Vector3.up, inPoint: transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (ConfigComponent.Instance.configuring) return;

        GetMousePositionOnGrid();

        if (Input.GetKeyDown(KeyCode.LeftArrow) && playerHolding != null)
        {
            playerHolding?.SendMessage("Rotate", true);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && playerHolding != null)
        {
            playerHolding?.SendMessage("Rotate", false);
        }
    }

    void GetMousePositionOnGrid()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out var enter))
        {
            mousePosition = ray.GetPoint(enter);
            mousePosition.y = 0;
            smoothMousePosition = mousePosition;
            mousePosition = Vector3Int.RoundToInt(mousePosition);
            gridMousePosition = mousePosition;
            foreach (var node in nodes)
            {
                if (ConfigComponent.Instance.orderMode)
                {
                    node.orderDisplay.gameObject.SetActive(true);
                    ConfigComponent.Instance.updateOrderDisplay(node);
                }
                else
                {
                    node.orderDisplay.gameObject.SetActive(false);
                }

                // mouse on empty grid
                if (node.cellPosition == mousePosition && node.isPlacable)
                {
                    //(place machine)
                    if (Input.GetMouseButtonUp(0) && playerHolding != null)
                    {
                        node.isPlacable = false;
                        playerHolding.GetComponent<ObjFollowMouse>().isOnGrid = true;
                        playerHolding.position = node.cellPosition; // + new Vector3(0, 0, 0);
                        node.thingPlaced = playerHolding;
                        playerHolding = null;
                        if (!MachineActivationManager.allMachineList.Contains(node.thingPlaced.gameObject))
                        {
                            MachineActivationManager.allMachineList.Add(node.thingPlaced.gameObject);
                        }

                        print("Creating chain");
                        Machine machine = node.thingPlaced.GetComponent<Machine>();
                        Chain newChain = new Chain(machine);
                        node.chainStart = newChain;
                    }
                    
                }
                // mouse on occupied grid 
                else if (node.cellPosition == mousePosition && !node.isPlacable)
                {
                    if (ConfigComponent.Instance.orderMode && Input.GetMouseButtonUp(0))
                    {
                        // left click on machine in orderMode
                        ConfigComponent.Instance.leftClickInOrderMode(node);
                        return;
                    }

                    else if (ConfigComponent.Instance.orderMode && Input.GetMouseButtonUp(1))
                    {
                        // right click on nothing in orderMode
                        ConfigComponent.Instance.rightClickInOrderMode(node);
                        return;
                    }

                    // left click on machine (pick up machine)
                    if (Input.GetMouseButtonUp(0) && playerHolding == null)
                    {
                        playerHolding = node.thingPlaced;
                        node.isPlacable = true;
                        node.thingPlaced = null;
                        playerHolding.GetComponent<ObjFollowMouse>().isOnGrid = false;
                        MachineDetailDisplay.Instance.CloseDetail();
                        if (MachineActivationManager.allMachineList.Contains(playerHolding.gameObject))
                        {
                            MachineActivationManager.allMachineList.Remove(playerHolding.gameObject);
                        }
                        //playerHolding.position = node.cellPosition + new Vector3(0, 0.5f, 0);

                        print("removing chain");
                        Chain newChain = node.chainStart;
                        if (newChain != null)
                        {
                            newChain.removeThisChainFromList();
                        }
                        node.chainStart = null;


                    }
                    // right click on machine
                    else if (Input.GetMouseButtonUp(1) && playerHolding == null)
                    {
                        print("Open config for this machine");
                        selectedMachineForConfig = node.thingPlaced;
                        //configTab.SetActive(true);
                        MachineDetailDisplay.Instance.SetSelection(node);
                        MachineDetailDisplay.Instance.OpenDetail();
                        //playerHolding.position = node.cellPosition + new Vector3(0, 0.5f, 0);
                    }

                }
                /*
                else if (node.cellPosition != mousePosition)
                {
                    Grid.currentlySelected?.SendMessage("ResetMat");
                    Grid.currentlySelected = null;
                }
                */
            }
        }
    }

    public void createMachineFromClick(int index)
    {
        if (playerHolding == null)
        {
            playerHolding = Instantiate(components[index], mousePosition, Quaternion.identity);
            if (!MachineActivationManager.allMachineList.Contains(playerHolding.gameObject))
            {
                MachineActivationManager.allMachineList.Add(playerHolding.gameObject);
            }
        }
    }

    public void trashMachine()
    {
        if (playerHolding != null)
        {
            if (MachineActivationManager.allMachineList.Contains(playerHolding.gameObject))
            {
                MachineActivationManager.allMachineList.Remove(playerHolding.gameObject);
            }
            Destroy(playerHolding.gameObject);
            playerHolding = null;
        }
    }

    public Node GetNode(Transform t)
    {
        Node node = null;
        foreach (Node n in nodes)
        {
            if (n.obj == t || n.thingPlaced == t) node = n;
        }
        return node;
    }

    private void CreateGrid()
    {
        nodes = new Node[width, height];
        var name = 0;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                // print("Creating");
                Vector3 worldPosition = new Vector3(i, 0, j);
                Transform obj = Instantiate(gridCellPrefab, worldPosition, Quaternion.identity, gridHolder);
                obj.name = "Cell " + name;

                // create orderDisplay
                Transform orderDisplay = Instantiate(orderDisplayPrefab, new Vector3(i, 2, j), Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f)), orderDisplayHolder);
                orderDisplay.name = "OrderDisplay " + name;

                nodes[i, j] = new Node(isPlacable: true, worldPosition, obj,null, orderDisplay);
                name++;
            }
        }
    }
}


public class Node
{
    public bool isPlacable;
    public Vector3 cellPosition;
    public Transform obj;
    public Transform thingPlaced;
    public Transform orderDisplay;
    public Chain chainStart;

    public Node(bool isPlacable, Vector3 cellPosition, Transform obj,Transform thingPlaced,Transform orderDisplay)
    {
        this.isPlacable = isPlacable;
        this.cellPosition = cellPosition;
        this.obj = obj;
        this.thingPlaced = thingPlaced;
        this.orderDisplay = orderDisplay;
    }
}