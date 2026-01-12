using System;
using System.Collections.Generic;
using System.Linq;

namespace BOOSEapp
{
    /// <summary>
    /// Simple parser for BOOSE programs
    /// Parses line by line and creates command objects
    /// </summary>
    public class SimpleParser
    {
        private readonly VariableStore _variables;
        private readonly AppCanvas _canvas;
        private readonly ExpressionEvaluator _evaluator;

        public SimpleParser(AppCanvas canvas, VariableStore variables)
        {
            _canvas = canvas;
            _variables = variables;
            _evaluator = new ExpressionEvaluator(variables);
        }

        public List<ISimpleCommand> Parse(string programText)
        {
            var commands = new List<ISimpleCommand>();
            var lines = programText.Split('\n')
                .Select(l => l.Trim())
                .Where(l => l.Length > 0 && !l.StartsWith("//"))
                .ToList();

            for (int i = 0; i < lines.Count; i++)
            {
                try
                {
                    var cmd = ParseLine(lines[i]);
                    if (cmd != null)
                        commands.Add(cmd);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error on line {i + 1}: {ex.Message}");
                }
            }

            return commands;
        }

        private ISimpleCommand? ParseLine(string line)
        {
            var parts = SplitLine(line);
            if (parts.Length == 0) return null;

            string command = parts[0].ToLower();

            // Variable declarations
            if (command == "int")
                return new IntCommand(_variables, _evaluator, parts);
            if (command == "real")
                return new RealCommand(_variables, _evaluator, parts);
            if (command == "boolean")
                return new BooleanCommand(_variables, _evaluator, parts);

            // Arrays
            if (command == "array")
                return new ArrayCommand(_variables, parts);
            if (command == "poke")
                return new PokeCommand(_variables, _evaluator, parts);
            if (command == "peek")
                return new PeekCommand(_variables, _evaluator, parts);

            // Drawing commands
            if (command == "moveto")
                return new MoveToCommand(_canvas, _evaluator, parts);
            if (command == "drawto")
                return new DrawToCommand(_canvas, _evaluator, parts);
            if (command == "circle")
                return new CircleCommand(_canvas, _evaluator, parts);
            if (command == "rect" || command == "rectangle")
                return new RectCommand(_canvas, _evaluator, parts);
            if (command == "tri" || command == "triangle")
                return new TriCommand(_canvas, _evaluator, parts);
            if (command == "pen" || command == "pencolour" || command == "pencolor")
                return new PenCommand(_canvas, _evaluator, parts);
            if (command == "clear")
                return new ClearCommand(_canvas);
            if (command == "reset")
                return new ResetCommand(_canvas);
            if (command == "write" || command == "writetext")
                return new WriteCommand(_canvas, _variables, _evaluator, line);

            // Variable assignment (e.g., "width = 2*radius")
            if (parts.Length >= 3 && parts[1] == "=")
                return new AssignCommand(_variables, _evaluator, parts);

            throw new Exception($"Unknown command: {command}");
        }

        private string[] SplitLine(string line)
        {
            var parts = new List<string>();
            var current = "";
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '"')
                {
                    inQuotes = !inQuotes;
                    current += c;
                }
                else if ((c == ' ' || c == ',' || c == '\t') && !inQuotes)
                {
                    if (current.Length > 0)
                    {
                        parts.Add(current);
                        current = "";
                    }
                }
                else
                {
                    current += c;
                }
            }

            if (current.Length > 0)
                parts.Add(current);

            return parts.ToArray();
        }
    }
}