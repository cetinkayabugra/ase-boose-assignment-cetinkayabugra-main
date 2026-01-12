using BOOSE;

namespace BOOSEapp
{
    /// <summary>
    /// Unrestricted Real variable class - no limit on number of variables
    /// </summary>
    public class UnrestrictedReal : Evaluation
    {
        //CONSTRUCTOR
        public UnrestrictedReal()
        {
            // No restriction check
        }
        // BASIC COMPILE PHASE
        public override void Compile()
        {
            base.Compile();
            base.Program.AddVariable(this);
        }
        // EXECUTE PHASE EVALUATES THE EXPRESSION AND UPDATES THE VARIABLE
        public override void Execute()
        {
            base.Execute();
            //ENSURES THE RESULT IS VALID FLOATING-POINT NUMBER
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