using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineActivationManager : MonoBehaviour
{

    public static MachineActivationManager Instance;
    public int tick = 0;
    public static List<GameObject> allMachineList = new List<GameObject>();
    public int startOrder = 1;


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
        addOrderManager();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject machine in allMachineList)
        {
            print(machine.GetComponent<Machine>().myName);
        }
    }

    void nextTick()
    {
        tick++;
    }

    void addOrderManager()
    {
        // create orderManger and add to a list
    }

    void doCurrentOrder()
    {
        // loop through orderManager list and activate all of them *make sure to not activate same machine*
        // keep list of machine activated
        // make sure all is done
    }

    void waitAllMachineDone()
    {

    }

    

    public class orderManager
    {
        public int currentOrder = 1;

        public orderManager(int currentOrder)
        {
            this.currentOrder = currentOrder;
        }

        bool isCurrentOrder(GameObject machine)
        {
            if (machine.GetComponent<Machine>().order == currentOrder)
            {
                return true;
            }
            else { return false; }
        }

        List<GameObject> getMachinesWithCurrentOrder()
        {
            List<GameObject> result = new List<GameObject>(allMachineList.FindAll(isCurrentOrder));
            return result;
        }

        void activateCurrentOrder()
        {
            // send signal to machine in current order
        }
    }
}
