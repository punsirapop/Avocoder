using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MachineType
{
    Variable,
    Numeric,
    Comparison,
    Logical,
    Conditional,
    Loop,
    Other
}

public enum MachineSubType
{
    #region Variable
    VariableIn,
    VariableOut,
    #endregion Variable
    #region NumericOper
    NumericAdd,
    NumericSub,
    NumericMul,
    NumericDiv,
    NumericMod,
    #endregion NumericOper
    #region ComparisonOper
    ComparisonGreaterThan,
    ComparisonGreaterThanOrEqual,
    ComparisonLessThan,
    ComparisonLessThanOrEqual,
    ComparisonEqual,
    ComparisonNotEqual,
    #endregion ComparisonOper
    #region LogicalOper
    LogicalAnd,
    LogicalOr,
    LogicalXor,
    LogicalNot,
    #endregion LogicalOper
    #region Conditional
    ConditionalIf,
    #endregion Conditional
    #region Loop
    LoopFor,
    LoopWhile,
    #endregion Loop
    Other
}

public enum GateType
{
    Entrance,
    Exit
}

public enum DataType
{
    Int,
    Float,
    Bool,
    Char
}

public enum Direction
{
    None,
    North,
    South,
    West,
    East
}

public class Gate
{
    public Direction Direction;
    public GateType GateType;
    public List<DataType> DataTypeList;

    public Gate(GateType gateType)
    {
        GateType = gateType;
    }
}

public class Machine : MonoBehaviour
{
    public MachineType type;
    [HideInInspector]
    public List<Gate> gateList;
}
