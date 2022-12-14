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
        addOrderManager(1);
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
    void addOrderManager(int currentOrder)
    {
        // create orderManger and add to a list
        OrderManager newOrderManager = new OrderManager(currentOrder);
        allOrderManagerList.Add(newOrderManager);
    }

    // use a button or something to activate this function to start activating machines
    public void doCurrentOrder()
    {
        // loop through orderManager list and activate all of them *make sure to not activate same machine*
        // keep list of machine activated
        // make sure all is done
        int index = 0;
        while (index < allOrderManagerList.Count)
        {
            Debug.Log("Doing order manager " + index);
            allOrderManagerList[index].activateCurrentOrder();
            index++;
        }
        /*foreach (OrderManager orderManager in allOrderManagerList)
        {
            orderManager.activateCurrentOrder();
        }*/
        //waitAllMachineDone();
        endOfOrderReset();
        foreach (GameObject machineObj in allMachineList)
        {
            //PlaceObjectOnGrid.Instance.selectedMachineForConfig = machineObj.transform;
            ConfigComponent.Instance.selectedMachine =  machineObj.GetComponent<Machine>();
            ConfigComponent.Instance.updateConfigForSelectedMachine();
        }


    }

    /*public void waitAllMachineDone()
    {

    }*/

    

    void endOfOrderReset()
    {
        // reset all the temp list used
        allOrderManagerList.Clear();
        addOrderManager(1);

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
            print("Doing order " + currentOrder);
            List<GameObject>  machinesToActivate = getMachinesWithCurrentOrder();

            if (currentOrder > 30)
            {
                return;
            }

            if (machinesToActivate.Count == 0)
            {
                Debug.Log("No more machine to activate");
                currentOrder++;
                activateCurrentOrder();
                return;
            }
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
            transferAllData();
            activatedMachineList.Clear();
            currentOrder++;
            activateCurrentOrder();
        }

        public void transferAllData()
        {
            foreach (Chain chain in ConnectorManager.chainsList)
            {
                chain.transferData();
            }
        }

        public void activateMachine(GameObject machine)
        {
            // activate the machine
            MachineType type = machine.GetComponent<Machine>().type;
            if (type == MachineType.Comparison)
            {
                ComOp compareOpMachine = machine.GetComponent<ComOp>();
                compareOpMachine.activate();
            }
            else if (type == MachineType.Logical) 
            {
                LogOp logicalOpMachine = machine.GetComponent<LogOp>();
                logicalOpMachine.activate();
            }
            else if (type == MachineType.Numeric)
            {
                NumOp numericOpMachine = machine.GetComponent<NumOp>();
                numericOpMachine.activate();
            }
            else if (type == MachineType.Variable)
            {
                Var variableMachine = machine.GetComponent<Var>();
                variableMachine.activate();
            }
            else if (type == MachineType.Function)
            {
                Func functionMachine = machine.GetComponent<Func>();
                functionMachine.activate();
            }
            else if (type == MachineType.Belt)
            {
                Belt beltMachine = machine.GetComponent<Belt>();
                beltMachine.activate();
            }
        }

        public void activateIFMachine(GameObject machine)
        {
            List<GameObject> nextMachinesToActivate = getMachinesWithNextOrder();
            If ifMachine = machine.GetComponent<If>();
            MachineType type = machine.GetComponent<Machine>().type;

            if (type == MachineType.If)
            {
                ifMachine.activate();
            }

            foreach (Machine targetMachines in ifMachine.ifSignalValidMachines)
            {
                // if machine queued to be activated by IF is not in next order list
                if (!nextMachinesToActivate.Contains(targetMachines.gameObject))
                {
                    Instance.addOrderManager(currentOrder);
                }
            }
        }
    }
}
