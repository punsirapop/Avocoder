using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

[CustomEditor(typeof(Machine))]
public class MachineEditor : Editor
{
    int selectedTypeIndex = 0;
    // int selectedDirIndex = 0;
    int entranceCount = 0;
    int exitCount = 0;
    bool showGates = true;
    List<Gate> gateList = new List<Gate>();
    List<Direction> selectedDir = new List<Direction>();
    List<DataType> dtEnt = new List<DataType>();
    List<DataType> dtExt = new List<DataType>();
    Gate newGate;

    string[] allDir = Enum.GetNames(typeof(Direction));

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Machine msc = (Machine)target;
        MachineType type = msc.type;

        AddGates(msc, type);
        EditorGUILayout.Space();

        showGates = EditorGUILayout.Foldout(showGates, "Gates");

        for (int i = 0; i < gateList.Count; i++)
        {
            selectedDir.Add(Direction.None);
        }

        if (showGates)
        {
            EditorGUI.indentLevel++;
            for(int i = 0; i < gateList.Count; i++)
            {
                string dataTypeDis = "";
                foreach(DataType d in gateList[i].DataTypeList)
                {
                    dataTypeDis += d.ToString() + ", ";
                }
                EditorGUILayout.LabelField("Gate No. " + (i+1));
                EditorGUI.indentLevel++;

                /*
                selectedDirIndex = EditorGUILayout.Popup("Direction", selectedDirIndex, allDir);
                var newValue = (Direction)Enum.Parse(typeof(Direction), allDir[selectedTypeIndex]);
                */

                selectedDir[i] = (Direction)EditorGUILayout.EnumPopup
                    (new GUIContent("Direction"), selectedDir[i], DisplayDir, true);
                selectedDir[i] |= 0;
                
                EditorGUILayout.LabelField("Gate Type", gateList[i].GateType.ToString());
                EditorGUILayout.LabelField("Data Type", dataTypeDis.Substring(0, dataTypeDis.Length - 2));
                EditorGUI.indentLevel--;
            }
        }
    }

    bool DisplayDir(Enum enumVal)
    {
        bool displayMode = true;
        if (allDir.Contains(enumVal.ToString()))
        {
            displayMode = false;
        }
        return displayMode;
    }

    void AddGates(Machine msc, MachineType type)
    {
        var allST = Enum.GetNames(typeof(MachineSubType));
        var allDT = Enum.GetNames(typeof(DataType));
        var toShowST = allST.Where(n => n.StartsWith(type.ToString())).ToArray();
        selectedTypeIndex = EditorGUILayout.Popup("Sub Type", selectedTypeIndex, toShowST);
        var newValue = (MachineSubType)Enum.Parse(typeof(MachineSubType), toShowST[selectedTypeIndex]);

        entranceCount = 0;
        exitCount = 0;
        gateList.Clear();
        dtEnt.Clear();
        dtExt.Clear();

        switch (type)
        {
            case MachineType.Variable:
                if (newValue == MachineSubType.VariableIn)
                {
                    entranceCount = 1;
                    dtEnt.Add(DataType.Int);
                    dtEnt.Add(DataType.Float);
                    dtEnt.Add(DataType.Bool);
                    dtEnt.Add(DataType.Char);
                }
                else if (newValue == MachineSubType.VariableOut)
                {
                    exitCount = 1;
                    dtExt.Add(DataType.Int);
                    dtExt.Add(DataType.Float);
                    dtExt.Add(DataType.Bool);
                    dtExt.Add(DataType.Char);
                }
                break;
            case MachineType.Numeric:
                entranceCount = 2;
                dtEnt.Add(DataType.Int);
                dtEnt.Add(DataType.Float);
                exitCount = 1;
                dtExt.Add(DataType.Int);
                dtExt.Add(DataType.Float);
                break;
            case MachineType.Comparison:
                entranceCount = 2;
                dtEnt.Add(DataType.Int);
                dtEnt.Add(DataType.Float);
                exitCount = 1;
                dtExt.Add(DataType.Bool);
                break;
            case MachineType.Logical:
                if (newValue == MachineSubType.LogicalNot)
                {
                    entranceCount = 1;
                }
                else
                {
                    entranceCount = 2;
                }
                dtEnt.Add(DataType.Bool);
                exitCount = 1;
                dtExt.Add(DataType.Bool);
                break;
            case MachineType.Conditional:
                entranceCount = 1;
                dtEnt.Add(DataType.Bool);
                break;
            case MachineType.Loop:
                break;
            case MachineType.Other:
                break;
            default:
                break;
        }

        for (int i = 0; i < entranceCount; i++)
        {
            newGate = new Gate(GateType.Entrance);
            newGate.DataTypeList = dtEnt;
            gateList.Add(newGate);
        }
        for (int i = 0; i < exitCount; i++)
        {
            newGate = new Gate(GateType.Exit);
            newGate.DataTypeList = dtExt;
            gateList.Add(newGate);
        }
    }
}
