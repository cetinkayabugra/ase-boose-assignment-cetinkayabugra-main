using BOOSE;

namespace BOOSEapp
{
    /// <summary>
    /// Handles variable assignments (not declarations)
    /// Example: width = 2*radius
    /// </summary>
    public class UnrestrictedEvaluation : Evaluation
    {
        public UnrestrictedEvaluation()
        {
            // No restrictions
        }

        public override void Compile()
        {
            base.Compile();
            // Don't add to variables - it should already exist
        }

        public override void Execute()
        {
            base.Execute();

            // The variable should already exist in the program
            // We just need to update its value with the evaluated expression

            // Try to parse as int first, then real
            if (int.TryParse(evaluatedExpression, out int intValue))
            {
                base.Program.UpdateVariable(varName, intValue);
            }
            else if (double.TryParse(evaluatedExpression, out double realValue))
            {
                base.Program.UpdateVariable(varName, realValue);
            }
            else
            {
                throw new StoredProgramException(
                    $"Cannot evaluate expression '{evaluatedExpression}' for variable '{varName}'"
                );
            }
        }
    }
}