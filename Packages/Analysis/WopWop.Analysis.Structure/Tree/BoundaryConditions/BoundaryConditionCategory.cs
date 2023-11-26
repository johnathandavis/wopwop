namespace WopWop.Analysis.Structure.Tree.BoundaryConditions;

public enum BoundaryConditionCategory
{
    /// <summary>
    /// Branching Condition
    /// </summary>
    IfStatement,
    SwitchStatement,
    SwitchExpression, // (Pattern matching)
    /// <summary>
    /// Iterating
    /// </summary>
    For,
    ForEach,
    While,
    DoWhile,
    /// <summary>
    /// Terminating
    /// </summary>
    Throws,
    Returns
}
