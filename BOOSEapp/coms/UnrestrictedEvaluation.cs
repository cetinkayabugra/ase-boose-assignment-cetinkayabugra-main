using BOOSE;

namespace BOOSEapp
{
    /// <summary>
    /// Handles variable assignments (not declarations)
    /// Example: width = 2*radius
    /// </summary>
    public class UnrestrictedEvaluation : Evaluation
    {
        //CONSTRUCTOR COMPILE EXECUTE
        public UnrestrictedEvaluation()
        {
            // No restrictions
        }

        public override void Compile()
        {
            base.Compile();
       
        }

        public override void Execute()
        {
            base.Execute();

        
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
                //ERROR HANDLING
                throw new StoredProgramException(
                    $"Cannot evaluate expression '{evaluatedExpression}' for variable '{varName}'"
                );
            }
        }
    }
}