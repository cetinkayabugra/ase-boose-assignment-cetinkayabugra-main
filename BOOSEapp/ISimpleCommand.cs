namespace BOOSEapp
{
    /// <summary>
    /// Simple command interface
    /// All commands implement this
    /// </summary>
    public interface ISimpleCommand
    {
        void Execute();
    }

    /// <summary>
    /// Control flow command interface
    /// Used by if/while/for/else/end commands
    /// </summary>
    public interface IControlFlowCommand : ISimpleCommand
    {
        void SetExecutor(ProgramExecutor executor);
    }
}