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
    /// Simple command interface
    /// </summary>
    public interface ISimpleCommand
    {
        void Execute();
    }

    /// <summary>
    /// Control flow command interface
    /// </summary>
    public interface IControlFlowCommand : ISimpleCommand
    {
        void SetExecutor(ProgramExecutor executor);
    }
}