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