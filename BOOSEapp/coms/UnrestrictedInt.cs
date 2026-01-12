using BOOSE;

namespace BOOSEapp
{
    /// <summary>
    /// Unrestricted Int variable class - no limit on number of variables
    /// </summary>
    public class UnrestrictedInt : Evaluation
    {
        public UnrestrictedInt()
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

            if (!int.TryParse(evaluatedExpression, out int value))
            {
                if (double.TryParse(evaluatedExpression, out var _))
                {
                    throw new StoredProgramException(
                        $"Cannot assign real value to int variable '{varName}'"
                    );
                }
                throw new StoredProgramException(
                    $"Invalid integer value for variable '{varName}'"
                );
            }

            base.Program.UpdateVariable(varName, value);
        }
    }
}