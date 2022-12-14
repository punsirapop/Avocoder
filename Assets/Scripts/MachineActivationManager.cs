using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineActivationManager : MonoBehaviour
{

    public static MachineActivationManager Instance;
    public static List<GameObject> allMachineList = new List<GameObject>();
    public static List<OrderManager> allOrderManagerList = new List<OrderManager>();
    public static List<GameObject> activatedMachineList = new List<GameObject>();
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
        /*
        foreach (GameObject machine in allMachineList)
        {
            print(machine.GetComponent<Machine>().myName);
        }
        */
    }

    // add when IF get activated
    void addOrderManager()
    {
        // create orderManger and add to a list
    }

    // use a button or something to activate this function to start activating machines
    void doCurrentOrder()
    {
        // loop through orderManager list and activate all of them *make sure to not activate same machine*
        // keep list of machine activated
        // make sure all is done
        foreach (OrderManager orderManager in allOrderManagerList)
        {
            orderManager.activateCurrentOrder();
        }
        waitAllMachineDone();
        endOfOrderReset();
    }

    public void waitAllMachineDone()
    {

    }

    void endOfOrderReset()
    {
        // reset all the temp list used
        activatedMachineList.Clear();
    }

    

    public class OrderManager
    {
        public int currentOrder = 1;

        public OrderManager(int currentOrder)
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

        bool isNextOrder(GameObject machine)
        {
            if (machine.GetComponent<Machine>().order == currentOrder+1)
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

        List<GameObject> getMachinesWithNextOrder()
        {
            List<GameObject> result = new List<GameObject>(allMachineList.FindAll(isNextOrder));
            return result;
        }

        public void activateCurrentOrder()
        {
            List<GameObject>  machinesToActivate = getMachinesWithCurrentOrder();
            // send signal to machine in current order
            foreach (GameObject machine in machinesToActivate)
            {
                if (!activatedMachineList.Contains(machine))
                {
                    if (machine.GetComponent<Machine>().type == MachineType.If)
                    {
                        activateIFMachine(machine);
                    }
                    else
                    {
                        activateMachine(machine);
                    }
                    activatedMachineList.Add(machine);
                }
            }
            currentOrder++;
        }

        public void activateMachine(GameObject machine)
        {
            // activate the machine
        }

        public void activateIFMachine(GameObject machine)
        {
            List<GameObject> nextMachinesToActivate = getMachinesWithNextOrder();
            // if machine activated by IF is not in next order list
            if (!nextMachinesToActivate.Contains(machine))
            {
                Instance.addOrderManager();
            }
            // activate the IF machine
        }
    }
}
