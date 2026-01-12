/*
 * File: ISimpleCommand.cs
 * Author: Bugra Cetinkaya
 * Purpose: Defines core command interfaces used by the BOOSE
 *          interpreter and execution engine.
 * Notes: Interfaces separate execution logic from parsing logic.
 */

namespace BOOSEapp
{
    /// <summary>
    /// Represents a basic executable command within the BOOSE interpreter.
    /// </summary>
    /// <remarks>
    /// All commands parsed from a BOOSE program must implement this
    /// interface in order to be executed by the ProgramExecutor.
    /// </remarks>
    public interface ISimpleCommand
    {
        void Execute();
    }

    /// <summary>
    /// Represents a control flow command such as loops or conditionals.
    /// </summary>
    /// <remarks>
    /// Control flow commands require access to the ProgramExecutor
    /// to manipulate execution order (e.g. while loops).
    /// </remarks>
    public interface IControlFlowCommand : ISimpleCommand
    {
        void SetExecutor(ProgramExecutor executor);
    }
}