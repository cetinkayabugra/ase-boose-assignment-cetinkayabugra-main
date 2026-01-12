using System;
using System.Linq;
using System.Collections.Generic;

namespace BOOSEapp
{
    // ==================== VARIABLE COMMANDS ====================

    public class IntCommand : ISimpleCommand
    {
        private readonly VariableStore _variables;
        private readonly ExpressionEvaluator _evaluator;
        private readonly string _name;
        private readonly string? _expression;

        public IntCommand(VariableStore variables, ExpressionEvaluator evaluator, string[] parts)
        {
            _variables = variables;
            _evaluator = evaluator;

            if (parts.Length < 2)
                throw new Exception("int requires a variable name");

            _name = parts[1];

            if (parts.Length >= 4 && parts[2] == "=")
            {
                _expression = string.Join(" ", parts.Skip(3));
            }
        }

        public void Execute()
        {
            int value = 0;
            if (_expression != null)
                value = _evaluator.EvaluateInt(_expression);
            _variables.SetInt(_name, value);
        }
    }

    public class RealCommand : ISimpleCommand
    {
        private readonly VariableStore _variables;
        private readonly ExpressionEvaluator _evaluator;
        private readonly string _name;
        private readonly string? _expression;

        public RealCommand(VariableStore variables, ExpressionEvaluator evaluator, string[] parts)
        {
            _variables = variables;
            _evaluator = evaluator;

            if (parts.Length < 2)
                throw new Exception("real requires a variable name");

            _name = parts[1];

            if (parts.Length >= 4 && parts[2] == "=")
            {
                _expression = string.Join(" ", parts.Skip(3));
            }
        }

        public void Execute()
        {
            double value = 0.0;
            if (_expression != null)
                value = _evaluator.Evaluate(_expression);
            _variables.SetReal(_name, value);
        }
    }

    public class AssignCommand : ISimpleCommand
    {
        private readonly VariableStore _variables;
        private readonly ExpressionEvaluator _evaluator;
        private readonly string _name;
        private readonly string _expression;

        public AssignCommand(VariableStore variables, ExpressionEvaluator evaluator, string[] parts)
        {
            _variables = variables;
            _evaluator = evaluator;

            if (parts.Length < 3 || parts[1] != "=")
                throw new Exception("Assignment must be: name = expression");

            _name = parts[0];
            _expression = string.Join(" ", parts.Skip(2));
        }

        public void Execute()
        {
            if (_variables.HasInt(_name))
            {
                int value = _evaluator.EvaluateInt(_expression);
                _variables.SetInt(_name, value);
            }
            else if (_variables.HasReal(_name))
            {
                double value = _evaluator.Evaluate(_expression);
                _variables.SetReal(_name, value);
            }
            else
            {
                throw new Exception($"Variable '{_name}' not declared");
            }
        }
    }

    // ==================== ARRAY COMMANDS ====================

    public class ArrayCommand : ISimpleCommand
    {
        private readonly VariableStore _variables;
        private readonly string _type;
        private readonly string _name;
        private readonly int _size;

        public ArrayCommand(VariableStore variables, string[] parts)
        {
            _variables = variables;

            if (parts.Length < 4)
                throw new Exception("array requires: array type name size");

            _type = parts[1].ToLower();
            _name = parts[2];
            _size = int.Parse(parts[3]);
        }

        public void Execute()
        {
            if (_type == "int")
                _variables.CreateIntArray(_name, _size);
            else if (_type == "real")
                _variables.CreateRealArray(_name, _size);
            else
                throw new Exception($"Unknown array type: {_type}");
        }
    }

    public class PokeCommand : ISimpleCommand
    {
        private readonly VariableStore _variables;
        private readonly ExpressionEvaluator _evaluator;
        private readonly string _arrayName;
        private readonly int _index;
        private readonly string _expression;

        public PokeCommand(VariableStore variables, ExpressionEvaluator evaluator, string[] parts)
        {
            _variables = variables;
            _evaluator = evaluator;

            if (parts.Length < 5 || parts[3] != "=")
                throw new Exception("poke requires: poke arrayname index = value");

            _arrayName = parts[1];
            _index = int.Parse(parts[2]);
            _expression = string.Join(" ", parts.Skip(4));
        }

        public void Execute()
        {
            if (_variables.HasIntArray(_arrayName))
            {
                int value = _evaluator.EvaluateInt(_expression);
                _variables.SetIntArrayValue(_arrayName, _index, value);
            }
            else if (_variables.HasRealArray(_arrayName))
            {
                double value = _evaluator.Evaluate(_expression);
                _variables.SetRealArrayValue(_arrayName, _index, value);
            }
            else
            {
                throw new Exception($"Array '{_arrayName}' not found");
            }
        }
    }

    public class PeekCommand : ISimpleCommand
    {
        private readonly VariableStore _variables;
        private readonly ExpressionEvaluator _evaluator;
        private readonly string _varName;
        private readonly string _arrayName;
        private readonly int _index;

        public PeekCommand(VariableStore variables, ExpressionEvaluator evaluator, string[] parts)
        {
            _variables = variables;
            _evaluator = evaluator;

            if (parts.Length < 5 || parts[2] != "=")
                throw new Exception("peek requires: peek varname = arrayname index");

            _varName = parts[1];
            _arrayName = parts[3];
            _index = int.Parse(parts[4]);
        }

        public void Execute()
        {
            if (_variables.HasIntArray(_arrayName))
            {
                int value = _variables.GetIntArrayValue(_arrayName, _index);
                _variables.SetInt(_varName, value);
            }
            else if (_variables.HasRealArray(_arrayName))
            {
                double value = _variables.GetRealArrayValue(_arrayName, _index);
                _variables.SetReal(_varName, value);
            }
            else
            {
                throw new Exception($"Array '{_arrayName}' not found");
            }
        }
    }

    // ==================== DRAWING COMMANDS ====================

    public class MoveToCommand : ISimpleCommand
    {
        private readonly AppCanvas _canvas;
        private readonly ExpressionEvaluator _evaluator;
        private readonly string _xExpr;
        private readonly string _yExpr;

        public MoveToCommand(AppCanvas canvas, ExpressionEvaluator evaluator, string[] parts)
        {
            _canvas = canvas;
            _evaluator = evaluator;

            if (parts.Length < 3)
                throw new Exception("moveto requires: moveto x y");

            _xExpr = parts[1];
            _yExpr = parts[2];
        }

        public void Execute()
        {
            int x = _evaluator.EvaluateInt(_xExpr);
            int y = _evaluator.EvaluateInt(_yExpr);
            _canvas.MoveTo(x, y);
        }
    }

    public class DrawToCommand : ISimpleCommand
    {
        private readonly AppCanvas _canvas;
        private readonly ExpressionEvaluator _evaluator;
        private readonly string _xExpr;
        private readonly string _yExpr;

        public DrawToCommand(AppCanvas canvas, ExpressionEvaluator evaluator, string[] parts)
        {
            _canvas = canvas;
            _evaluator = evaluator;

            if (parts.Length < 3)
                throw new Exception("drawto requires: drawto x y");

            _xExpr = parts[1];
            _yExpr = parts[2];
        }

        public void Execute()
        {
            int x = _evaluator.EvaluateInt(_xExpr);
            int y = _evaluator.EvaluateInt(_yExpr);
            _canvas.DrawTo(x, y);
        }
    }

    public class CircleCommand : ISimpleCommand
    {
        private readonly AppCanvas _canvas;
        private readonly ExpressionEvaluator _evaluator;
        private readonly string _radiusExpr;

        public CircleCommand(AppCanvas canvas, ExpressionEvaluator evaluator, string[] parts)
        {
            _canvas = canvas;
            _evaluator = evaluator;

            if (parts.Length < 2)
                throw new Exception("circle requires: circle radius");

            _radiusExpr = parts[1];
        }

        public void Execute()
        {
            int radius = _evaluator.EvaluateInt(_radiusExpr);
            _canvas.Circle(radius, false);
        }
    }

    public class RectCommand : ISimpleCommand
    {
        private readonly AppCanvas _canvas;
        private readonly ExpressionEvaluator _evaluator;
        private readonly string _widthExpr;
        private readonly string _heightExpr;

        public RectCommand(AppCanvas canvas, ExpressionEvaluator evaluator, string[] parts)
        {
            _canvas = canvas;
            _evaluator = evaluator;

            if (parts.Length < 3)
                throw new Exception("rect requires: rect width height");

            _widthExpr = parts[1];
            _heightExpr = parts[2];
        }

        public void Execute()
        {
            int width = _evaluator.EvaluateInt(_widthExpr);
            int height = _evaluator.EvaluateInt(_heightExpr);
            _canvas.Rect(width, height, false);
        }
    }

    public class TriCommand : ISimpleCommand
    {
        private readonly AppCanvas _canvas;
        private readonly ExpressionEvaluator _evaluator;
        private readonly string _widthExpr;
        private readonly string _heightExpr;

        public TriCommand(AppCanvas canvas, ExpressionEvaluator evaluator, string[] parts)
        {
            _canvas = canvas;
            _evaluator = evaluator;

            if (parts.Length < 3)
                throw new Exception("tri requires: tri width height");

            _widthExpr = parts[1];
            _heightExpr = parts[2];
        }

        public void Execute()
        {
            int width = _evaluator.EvaluateInt(_widthExpr);
            int height = _evaluator.EvaluateInt(_heightExpr);
            _canvas.Tri(width, height);
        }
    }

    public class PenCommand : ISimpleCommand
    {
        private readonly AppCanvas _canvas;
        private readonly ExpressionEvaluator _evaluator;
        private readonly string _rExpr;
        private readonly string _gExpr;
        private readonly string _bExpr;

        public PenCommand(AppCanvas canvas, ExpressionEvaluator evaluator, string[] parts)
        {
            _canvas = canvas;
            _evaluator = evaluator;

            if (parts.Length < 4)
                throw new Exception("pen requires: pen r g b");

            _rExpr = parts[1];
            _gExpr = parts[2];
            _bExpr = parts[3];
        }

        public void Execute()
        {
            int r = _evaluator.EvaluateInt(_rExpr);
            int g = _evaluator.EvaluateInt(_gExpr);
            int b = _evaluator.EvaluateInt(_bExpr);
            _canvas.SetColour(r, g, b);
        }
    }

    public class WriteCommand : ISimpleCommand
    {
        private readonly AppCanvas _canvas;
        private readonly VariableStore _variables;
        private readonly ExpressionEvaluator _evaluator;
        private readonly string _text;

        public WriteCommand(AppCanvas canvas, VariableStore variables, ExpressionEvaluator evaluator, string line)
        {
            _canvas = canvas;
            _variables = variables;
            _evaluator = evaluator;

            int startIndex = line.IndexOf(' ');
            if (startIndex < 0)
            {
                _text = "";
            }
            else
            {
                _text = line.Substring(startIndex + 1).Trim();
            }
        }

        public void Execute()
        {
            string output = _text;

            if (output.StartsWith("\"") && output.EndsWith("\""))
            {
                output = output.Substring(1, output.Length - 2);
            }
            else if (output.Contains("+") && output.Contains("\""))
            {
                var parts = output.Split('+');
                output = "";
                foreach (var part in parts)
                {
                    var trimmed = part.Trim();
                    if (trimmed.StartsWith("\"") && trimmed.EndsWith("\""))
                    {
                        output += trimmed.Substring(1, trimmed.Length - 2);
                    }
                    else
                    {
                        output += EvaluateForOutput(trimmed);
                    }
                }
            }
            else
            {
                output = EvaluateForOutput(output);
            }

            _canvas.WriteText(output);
        }

        private string EvaluateForOutput(string expr)
        {
            try
            {
                expr = expr.Trim();
                double value = _evaluator.Evaluate(expr);

                if (Math.Abs(value - Math.Round(value)) < 0.0001)
                {
                    return ((int)Math.Round(value)).ToString();
                }
                return value.ToString();
            }
            catch
            {
                return expr;
            }
        }
    }

    public class ClearCommand : ISimpleCommand
    {
        private readonly AppCanvas _canvas;

        public ClearCommand(AppCanvas canvas)
        {
            _canvas = canvas;
        }

        public void Execute()
        {
            _canvas.Clear();
        }
    }

    public class ResetCommand : ISimpleCommand
    {
        private readonly AppCanvas _canvas;

        public ResetCommand(AppCanvas canvas)
        {
            _canvas = canvas;
        }

        public void Execute()
        {
            _canvas.Reset();
        }
    }

    // ==================== CONTROL FLOW COMMANDS ====================

    public class IfCommand : IControlFlowCommand
    {
        private readonly VariableStore _variables;
        private readonly ExpressionEvaluator _evaluator;
        private readonly string _condition;
        private ProgramExecutor? _executor;
        private int _endLine = -1;
        private int _elseLine = -1;

        public IfCommand(VariableStore variables, ExpressionEvaluator evaluator, string[] parts)
        {
            _variables = variables;
            _evaluator = evaluator;

            if (parts.Length < 2)
                throw new Exception("if requires a condition");

            _condition = string.Join(" ", parts.Skip(1));
        }

        public void SetExecutor(ProgramExecutor executor)
        {
            _executor = executor;
        }

        public void SetEndLine(int endLine)
        {
            _endLine = endLine;
        }

        public void SetElseLine(int elseLine)
        {
            _elseLine = elseLine;
        }

        public void Execute()
        {
            if (_executor == null)
                throw new Exception("Executor not set for if command");

            bool condition = EvaluateCondition(_condition);

            if (!condition)
            {
                if (_elseLine >= 0)
                {
                    _executor.SetLine(_elseLine);
                }
                else if (_endLine >= 0)
                {
                    _executor.SetLine(_endLine);
                }
            }
        }

        private bool EvaluateCondition(string condition)
        {
            condition = condition.Replace(" ", "");

            if (condition.Contains("=="))
            {
                var parts = condition.Split(new[] { "==" }, StringSplitOptions.None);
                double left = _evaluator.Evaluate(parts[0]);
                double right = _evaluator.Evaluate(parts[1]);
                return Math.Abs(left - right) < 0.0001;
            }

            if (condition.Contains("!="))
            {
                var parts = condition.Split(new[] { "!=" }, StringSplitOptions.None);
                double left = _evaluator.Evaluate(parts[0]);
                double right = _evaluator.Evaluate(parts[1]);
                return Math.Abs(left - right) > 0.0001;
            }

            if (condition.Contains("<="))
            {
                var parts = condition.Split(new[] { "<=" }, StringSplitOptions.None);
                double left = _evaluator.Evaluate(parts[0]);
                double right = _evaluator.Evaluate(parts[1]);
                return left <= right;
            }

            if (condition.Contains(">="))
            {
                var parts = condition.Split(new[] { ">=" }, StringSplitOptions.None);
                double left = _evaluator.Evaluate(parts[0]);
                double right = _evaluator.Evaluate(parts[1]);
                return left >= right;
            }

            if (condition.Contains("<"))
            {
                var parts = condition.Split('<');
                double left = _evaluator.Evaluate(parts[0]);
                double right = _evaluator.Evaluate(parts[1]);
                return left < right;
            }

            if (condition.Contains(">"))
            {
                var parts = condition.Split('>');
                double left = _evaluator.Evaluate(parts[0]);
                double right = _evaluator.Evaluate(parts[1]);
                return left > right;
            }

            double value = _evaluator.Evaluate(condition);
            return Math.Abs(value) > 0.0001;
        }
    }

    public class ElseCommand : IControlFlowCommand
    {
        private ProgramExecutor? _executor;
        private int _endLine = -1;

        public void SetExecutor(ProgramExecutor executor)
        {
            _executor = executor;
        }

        public void SetEndLine(int endLine)
        {
            _endLine = endLine;
        }

        public void Execute()
        {
            if (_executor == null)
                throw new Exception("Executor not set for else command");

            if (_endLine >= 0)
            {
                _executor.SetLine(_endLine);
            }
        }
    }

    public class EndIfCommand : ISimpleCommand
    {
        public void Execute()
        {
        }
    }

    public class WhileCommand : IControlFlowCommand
    {
        private readonly VariableStore _variables;
        private readonly ExpressionEvaluator _evaluator;
        private readonly string _condition;
        private ProgramExecutor? _executor;
        private int _startLine = -1;
        private int _endLine = -1;

        public WhileCommand(VariableStore variables, ExpressionEvaluator evaluator, string[] parts)
        {
            _variables = variables;
            _evaluator = evaluator;

            if (parts.Length < 2)
                throw new Exception("while requires a condition");

            _condition = string.Join(" ", parts.Skip(1));
        }

        public void SetExecutor(ProgramExecutor executor)
        {
            _executor = executor;
        }

        public void SetStartLine(int startLine)
        {
            _startLine = startLine;
        }

        public void SetEndLine(int endLine)
        {
            _endLine = endLine;
        }

        public void Execute()
        {
            if (_executor == null)
                throw new Exception("Executor not set for while command");

            bool condition = EvaluateCondition(_condition);

            if (!condition)
            {
                if (_endLine >= 0)
                {
                    _executor.SetLine(_endLine);
                }
            }
        }

        private bool EvaluateCondition(string condition)
        {
            condition = condition.Replace(" ", "");

            if (condition.Contains("<"))
            {
                var parts = condition.Split('<');
                double left = _evaluator.Evaluate(parts[0]);
                double right = _evaluator.Evaluate(parts[1]);
                return left < right;
            }

            if (condition.Contains(">"))
            {
                var parts = condition.Split('>');
                double left = _evaluator.Evaluate(parts[0]);
                double right = _evaluator.Evaluate(parts[1]);
                return left > right;
            }

            double value = _evaluator.Evaluate(condition);
            return Math.Abs(value) > 0.0001;
        }
    }

    public class EndWhileCommand : IControlFlowCommand
    {
        private ProgramExecutor? _executor;
        private int _whileLine = -1;

        public void SetExecutor(ProgramExecutor executor)
        {
            _executor = executor;
        }

        public void SetWhileLine(int whileLine)
        {
            _whileLine = whileLine;
        }

        public void Execute()
        {
            if (_executor == null)
                throw new Exception("Executor not set for end while command");

            if (_whileLine >= 0)
            {
                _executor.SetLine(_whileLine - 1);
            }
        }
    }

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

            string initFlag = "__for_" + _varName + "_init";

            if (!_variables.HasInt(initFlag) || _variables.GetInt(initFlag) == 0)
            {
                int startValue = _evaluator.EvaluateInt(_startExpr);
                int endValue = _evaluator.EvaluateInt(_endExpr);
                int stepValue = _evaluator.EvaluateInt(_stepExpr);

                _variables.SetInt(_varName, startValue);
                _variables.SetInt("__for_" + _varName + "_end", endValue);
                _variables.SetInt("__for_" + _varName + "_step", stepValue);
                _variables.SetInt(initFlag, 1);

                return;
            }

            int current = _variables.GetInt(_varName);
            int end = _variables.GetInt("__for_" + _varName + "_end");
            int step = _variables.GetInt("__for_" + _varName + "_step");

            bool shouldContinue = (step > 0 && current <= end) || (step < 0 && current >= end);

            if (!shouldContinue)
            {
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

            int current = _variables.GetInt(_varName);
            int step = _variables.GetInt("__for_" + _varName + "_step");
            _variables.SetInt(_varName, current + step);

            if (_forLine >= 0)
            {
                _executor.SetLine(_forLine - 1);
            }
        }
    }

    // ==================== METHOD COMMANDS ====================

    public class MethodCommand : IControlFlowCommand
    {
        private readonly MethodStore _methodStore;
        private readonly string _returnType;
        private readonly string _name;
        private readonly List<(string type, string name)> _parameters;
        private ProgramExecutor? _executor;
        private int _endLine = -1;

        public MethodCommand(MethodStore methodStore, string[] parts)
        {
            _methodStore = methodStore;
            _parameters = new List<(string type, string name)>();

            if (parts.Length < 3)
                throw new Exception("method requires: method returnType name [parameters]");

            _returnType = parts[1].ToLower();
            _name = parts[2];

            for (int i = 3; i < parts.Length; i += 2)
            {
                if (i + 1 >= parts.Length) break;
                string paramType = parts[i].ToLower();
                string paramName = parts[i + 1];
                _parameters.Add((paramType, paramName));
            }
        }

        public void SetExecutor(ProgramExecutor executor)
        {
            _executor = executor;
        }

        public void SetEndLine(int endLine)
        {
            _endLine = endLine;
        }

        public string GetName()
        {
            return _name;
        }

        public List<(string type, string name)> GetParameters()
        {
            return _parameters;
        }

        public void Execute()
        {
            if (_executor == null)
                throw new Exception("Executor not set for method command");

            if (_endLine >= 0)
            {
                _executor.SetLine(_endLine);
            }
        }

        public void RegisterMethod(int startLine, int endLine)
        {
            var method = new MethodDefinition
            {
                Name = _name,
                ReturnType = _returnType,
                Parameters = _parameters,
                StartLine = startLine,
                EndLine = endLine
            };
            _methodStore.AddMethod(_name, method);
        }
    }

    public class EndMethodCommand : IControlFlowCommand
    {
        private readonly VariableStore _variables;
        private ProgramExecutor? _executor;

        public EndMethodCommand(VariableStore variables)
        {
            _variables = variables;
        }

        public void SetExecutor(ProgramExecutor executor)
        {
            _executor = executor;
        }

        public void Execute()
        {
            if (_executor == null)
                throw new Exception("Executor not set for end method");

            if (_variables.HasInt("__return_line"))
            {
                int returnLine = _variables.GetInt("__return_line");
                _executor.SetLine(returnLine);
            }
        }
    }

    public class CallCommand : IControlFlowCommand
    {
        private readonly VariableStore _variables;
        private readonly MethodStore _methodStore;
        private readonly ExpressionEvaluator _evaluator;
        private readonly string _methodName;
        private readonly List<string> _arguments;
        private ProgramExecutor? _executor;

        public CallCommand(VariableStore variables, MethodStore methodStore,
                          ExpressionEvaluator evaluator, string[] parts)
        {
            _variables = variables;
            _methodStore = methodStore;
            _evaluator = evaluator;
            _arguments = new List<string>();

            if (parts.Length < 2)
                throw new Exception("call requires: call methodName [arguments]");

            _methodName = parts[1];

            for (int i = 2; i < parts.Length; i++)
            {
                _arguments.Add(parts[i]);
            }
        }

        public void SetExecutor(ProgramExecutor executor)
        {
            _executor = executor;
        }

        public void Execute()
        {
            if (_executor == null)
                throw new Exception("Executor not set for call command");

            var method = _methodStore.GetMethod(_methodName);

            if (_arguments.Count != method.Parameters.Count)
                throw new Exception($"Method '{_methodName}' expects {method.Parameters.Count} arguments, got {_arguments.Count}");

            int returnLine = _executor.GetCurrentLine();

            for (int i = 0; i < method.Parameters.Count; i++)
            {
                var (paramType, paramName) = method.Parameters[i];
                string argValue = _arguments[i];

                if (paramType == "int")
                {
                    int value = _evaluator.EvaluateInt(argValue);
                    _variables.SetInt(paramName, value);
                }
                else if (paramType == "real")
                {
                    double value = _evaluator.Evaluate(argValue);
                    _variables.SetReal(paramName, value);
                }
            }

            if (method.ReturnType == "int")
            {
                _variables.SetInt(_methodName, 0);
            }
            else if (method.ReturnType == "real")
            {
                _variables.SetReal(_methodName, 0.0);
            }

            _variables.SetInt("__return_line", returnLine);

            _executor.SetLine(method.StartLine);
        }
    }
}