using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObjectOnGrid : MonoBehaviour
{
    public static PlaceObjectOnGrid Instance;

    public Transform gridCellPrefab;
    public Transform cube;
    public Transform gridHolder;

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
        GetMousePositionOnGrid();
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
                // mouse on empty grid
                if (node.cellPosition == mousePosition && node.isPlacable)
                {
                    if (Input.GetMouseButtonUp(0) && playerHolding != null)
                    {
                        node.isPlacable = false;
                        playerHolding.GetComponent<ObjFollowMouse>().isOnGrid = true;
                        playerHolding.position = node.cellPosition + new Vector3(0, 0.4075f, 0);
                        node.thingPlaced = playerHolding;
                        playerHolding = null;
                    }
                    
                }
                // mouse on occupied grid 
                else if (node.cellPosition == mousePosition && !node.isPlacable)
                {
                    // left click on machine
                    if (Input.GetMouseButtonUp(0) && playerHolding == null)
                    {
                        playerHolding = node.thingPlaced;
                        node.isPlacable = true;
                        node.thingPlaced = null;
                        playerHolding.GetComponent<ObjFollowMouse>().isOnGrid = false;
                        //playerHolding.position = node.cellPosition + new Vector3(0, 0.5f, 0);
                    }
                    // right click on machine
                    else if (Input.GetMouseButtonUp(1) && playerHolding == null)
                    {
                        print("Open config for this machine");
                        selectedMachineForConfig = node.thingPlaced;
                        configTab.SetActive(true);
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

    public void createMachineFromClick()
    {
        if (playerHolding == null)
        {
            playerHolding = Instantiate(cube, mousePosition, Quaternion.identity);
        }
    }

    public void trashMachine()
    {
        if (playerHolding != null)
        {
            Destroy(playerHolding.gameObject);
            playerHolding = null;
        }
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
                nodes[i, j] = new Node(isPlacable: true, worldPosition, obj,null);
                name++;
            }
        }
    }

    private Node GetNode(Transform t)
    {
        Node node = null;
        foreach (Node n in nodes)
        {
            if (n.obj == t || n.thingPlaced == t) node = n;
        }
        return node;
    }
}


public class Node
{
    public bool isPlacable;
    public Vector3 cellPosition;
    public Transform obj;
    public Transform thingPlaced;

    public Node(bool isPlacable, Vector3 cellPosition, Transform obj,Transform thingPlaced)
    {
        this.isPlacable = isPlacable;
        this.cellPosition = cellPosition;
        this.obj = obj;
        this.thingPlaced = thingPlaced;
    }
}