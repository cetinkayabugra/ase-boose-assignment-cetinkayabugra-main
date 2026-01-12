using BOOSE;

namespace BOOSEapp
{
    /// <summary>
    /// Represents an unrestricted integer variable within the BOOSE interpreter.
    /// </summary>
    /// <remarks>
    /// This class overrides the default BOOSE integer handling in order to
    /// remove restrictions on the number of integer variables that can be
    /// declared in a program.
    /// </remarks>
    public class UnrestrictedInt : Evaluation
    {
        /// <summary>
        /// Initializes a new unrestricted integer variable.
        /// </summary>
        public UnrestrictedInt()
        {
            
        }
        //COMPILE PHASE
        public override void Compile()
        {
            base.Compile();
            base.Program.AddVariable(this);
        }
        //EXECUTE PHASE EVALUATES THE EXPRESSION AND UPDATES THE VARIABLE
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