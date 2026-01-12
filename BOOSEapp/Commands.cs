public class ForCommand : IControlFlowCommand
{
    private readonly VariableStore _variables;
    private readonly ExpressionEvaluator _evaluator;
    private readonly string _varName;
    private readonly string _startExpr;
    private readonly string _endExpr;
    private readonly string _stepExpr;
    private ProgramExecutor? _executor;
    private int _endLine = -1;

    public ForCommand(VariableStore variables, ExpressionEvaluator evaluator, string[] parts)
    {
        _variables = variables;
        _evaluator = evaluator;

        if (parts.Length < 5)
            throw new Exception("for requires: for var = start to end [step increment]");

        _varName = parts[1];

        int equalIndex = -1;
        int toIndex = -1;
        int stepIndex = -1;

        for (int i = 0; i < parts.Length; i++)
        {
            if (parts[i] == "=") equalIndex = i;
            else if (parts[i].ToLower() == "to") toIndex = i;
            else if (parts[i].ToLower() == "step") stepIndex = i;
        }

        if (equalIndex < 0 || toIndex < 0)
            throw new Exception("for requires: for var = start to end");

        _startExpr = parts[equalIndex + 1];
        _endExpr = parts[toIndex + 1];
        _stepExpr = (stepIndex >= 0 && stepIndex + 1 < parts.Length) ? parts[stepIndex + 1] : "1";
    }

    public void SetExecutor(ProgramExecutor executor)
    {
        _executor = executor;
    }

    public void SetEndLine(int endLine)
    {
        _endLine = endLine;
    }

    public string GetVarName()
    {
        return _varName;
    }

    public void Execute()
    {
        if (_executor == null)
            throw new Exception("Executor not set for for command");

        // Check if this is initialization or condition check
        string initFlag = "__for_" + _varName + "_init";

        if (!_variables.HasInt(initFlag) || _variables.GetInt(initFlag) == 0)
        {
            // Initialize loop
            int startValue = _evaluator.EvaluateInt(_startExpr);
            int endValue = _evaluator.EvaluateInt(_endExpr);
            int stepValue = _evaluator.EvaluateInt(_stepExpr);

            _variables.SetInt(_varName, startValue);
            _variables.SetInt("__for_" + _varName + "_end", endValue);
            _variables.SetInt("__for_" + _varName + "_step", stepValue);
            _variables.SetInt(initFlag, 1);

            // Continue to loop body
            return;
        }

        // Check condition
        int current = _variables.GetInt(_varName);
        int end = _variables.GetInt("__for_" + _varName + "_end");
        int step = _variables.GetInt("__for_" + _varName + "_step");

        bool shouldContinue = (step > 0 && current <= end) || (step < 0 && current >= end);

        if (!shouldContinue)
        {
            // Loop finished - clean up and jump to end
            _variables.SetInt(initFlag, 0);

            if (_endLine >= 0)
            {
                _executor.SetLine(_endLine);
            }
        }
    }
}

public class EndForCommand : IControlFlowCommand
{
    private readonly VariableStore _variables;
    private ProgramExecutor? _executor;
    private int _forLine = -1;
    private string _varName = "";

    public EndForCommand(VariableStore variables)
    {
        _variables = variables;
    }

    public void SetExecutor(ProgramExecutor executor)
    {
        _executor = executor;
    }

    public void SetForLine(int forLine)
    {
        _forLine = forLine;
    }

    public void SetVarName(string varName)
    {
        _varName = varName;
    }

    public void Execute()
    {
        if (_executor == null)
            throw new Exception("Executor not set for end for command");

        if (string.IsNullOrEmpty(_varName))
            throw new Exception("Variable name not set for end for command");

        // Increment loop variable
        int current = _variables.GetInt(_varName);
        int step = _variables.GetInt("__for_" + _varName + "_step");
        _variables.SetInt(_varName, current + step);

        // Jump back to for statement to check condition
        if (_forLine >= 0)
        {
            _executor.SetLine(_forLine - 1); // -1 because it will be incremented
        }
    }
}