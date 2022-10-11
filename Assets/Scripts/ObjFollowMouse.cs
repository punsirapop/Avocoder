using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjFollowMouse : MonoBehaviour
{

    private PlaceObjectOnGrid placeObjectOnGrid;
    public bool isOnGrid;


    // Start is called before the first frame update
    void Start()
    {
        placeObjectOnGrid = FindObjectOfType<PlaceObjectOnGrid>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOnGrid)
        {
            transform.position = placeObjectOnGrid.smoothMousePosition + new Vector3(0, 1.5f, 0);
            foreach (var node in placeObjectOnGrid.nodes)
            {
                if (node.cellPosition == placeObjectOnGrid.gridMousePosition && node.isPlacable)
                {
                    transform.position = placeObjectOnGrid.gridMousePosition + new Vector3(0, 1.5f, 0);
                }
            }
        }

    }
}
