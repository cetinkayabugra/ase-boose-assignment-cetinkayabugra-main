using System;
using System.Collections.Generic;
using System.Linq;

namespace BOOSEapp
{
    /// <summary>
    /// Simple parser for BOOSE programs with control flow support
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

            // First pass: create all commands
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

            // Second pass: link control flow commands
            LinkControlFlow(commands);

            return commands;
        }

        private void LinkControlFlow(List<ISimpleCommand> commands)
        {
            var ifStack = new Stack<(int line, IfCommand cmd)>();
            var whileStack = new Stack<(int line, WhileCommand cmd)>();

            for (int i = 0; i < commands.Count; i++)
            {
                var command = commands[i];

                if (command is IfCommand ifCmd)
                {
                    ifStack.Push((i, ifCmd));
                }
                else if (command is ElseCommand elseCmd)
                {
                    if (ifStack.Count == 0)
                        throw new Exception($"'else' without matching 'if' at line {i + 1}");

                    var (ifLine, ifCommand) = ifStack.Peek();
                    ifCommand.SetElseLine(i);
                }
                else if (command is EndIfCommand)
                {
                    if (ifStack.Count == 0)
                        throw new Exception($"'end if' without matching 'if' at line {i + 1}");

                    var (ifLine, ifCommand) = ifStack.Pop();
                    ifCommand.SetEndLine(i);

                    // Also set end for else if present
                    if (i > 0 && commands[i - 1] is ElseCommand previousElse)
                    {
                        previousElse.SetEndLine(i);
                    }
                    else
                    {
                        // Look backwards for else
                        for (int j = ifLine + 1; j < i; j++)
                        {
                            if (commands[j] is ElseCommand ec)
                            {
                                ec.SetEndLine(i);
                                break;
                            }
                        }
                    }
                }
                else if (command is WhileCommand whileCmd)
                {
                    whileStack.Push((i, whileCmd));
                    whileCmd.SetStartLine(i);
                }
                else if (command is EndWhileCommand endWhileCmd)
                {
                    if (whileStack.Count == 0)
                        throw new Exception($"'end while' without matching 'while' at line {i + 1}");

                    var (whileLine, whileCommand) = whileStack.Pop();
                    whileCommand.SetEndLine(i);
                    endWhileCmd.SetWhileLine(whileLine);
                }
            }

            if (ifStack.Count > 0)
                throw new Exception("Unmatched 'if' statement");
            if (whileStack.Count > 0)
                throw new Exception("Unmatched 'while' statement");
        }

        private ISimpleCommand? ParseLine(string line)
        {
            var parts = SplitLine(line);
            if (parts.Length == 0) return null;

            string command = parts[0].ToLower();

            // Control flow
            if (command == "if")
                return new IfCommand(_variables, _evaluator, parts);
            if (command == "else")
                return new ElseCommand();
            if (command == "end")
            {
                if (parts.Length >= 2)
                {
                    string endType = parts[1].ToLower();
                    if (endType == "if") return new EndIfCommand();
                    if (endType == "while") return new EndWhileCommand();
                }
                throw new Exception("'end' requires type: 'end if' or 'end while'");
            }
            if (command == "endif")
                return new EndIfCommand();
            if (command == "while")
                return new WhileCommand(_variables, _evaluator, parts);
            if (command == "endwhile")
                return new EndWhileCommand();

            // Variable declarations
            if (command == "int")
                return new IntCommand(_variables, _evaluator, parts);
            if (command == "real")
                return new RealCommand(_variables, _evaluator, parts);

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

            // Variable assignment
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