using BOOSE;

namespace BOOSEapp
{
    /// <summary>
    /// Unrestricted Real variable class - no limit on number of variables
    /// </summary>
    public class UnrestrictedReal : Evaluation
    {
        public UnrestrictedReal()
        {
            // No restriction check
        }

        public override void Compile()
        {
            base.Compile();
            base.Program.AddVariable(this);
        }

        public override void Execute()
        {
            base.Execute();

            if (!double.TryParse(evaluatedExpression, out double value))
            {
                throw new StoredProgramException(
                    $"Invalid real value for variable '{varName}'"
                );
            }

            base.Program.UpdateVariable(varName, value);
        }
    }
}