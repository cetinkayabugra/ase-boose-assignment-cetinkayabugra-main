using System;
using System.Collections.Generic;

namespace BOOSEapp
{
    /// <summary>
    /// Executes parsed commands with support for control structures
    /// Handles if/else/end, while loops, for loops
    /// </summary>
    public class ProgramExecutor
    {
        private readonly List<ISimpleCommand> _commands;
        private readonly VariableStore _variables;
        private readonly ExpressionEvaluator _evaluator;
        private int _currentLine;

        public ProgramExecutor(List<ISimpleCommand> commands, VariableStore variables, ExpressionEvaluator evaluator)
        {
            _commands = commands;
            _variables = variables;
            _evaluator = evaluator;
            _currentLine = 0;
        }

        public void Execute()
        {
            _currentLine = 0;
            while (_currentLine < _commands.Count)
            {
                var command = _commands[_currentLine];
                command.Execute();
                _currentLine++;
            }
        }

        public void SetLine(int line)
        {
            _currentLine = line;
        }

        public int GetCurrentLine()
        {
            return _currentLine;
        }
    }
}