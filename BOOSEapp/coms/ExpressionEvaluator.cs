using System;
using System.Globalization;

namespace BOOSEapp
{
    /// <summary>
    /// Evaluates mathematical and boolean expressions with variables
    /// Supports: +, -, *, /, parentheses, &&, ||, !, ==, !=, <, >, <=, >=
    /// </summary>
    public class ExpressionEvaluator
    {
        private readonly VariableStore _variables;

        public ExpressionEvaluator(VariableStore variables)
        {
            _variables = variables;
        }

        public double Evaluate(string expression)
        {
            expression = expression.Replace(" ", "");
            return ParseExpression(expression);
        }

        public int EvaluateInt(string expression)
        {
            return (int)Math.Round(Evaluate(expression));
        }

        public bool EvaluateBoolean(string expression)
        {
            expression = expression.Replace(" ", "");
            return ParseBooleanExpression(expression);
        }

        private bool ParseBooleanExpression(string expr)
        {
            // Handle true/false literals
            if (expr == "true") return true;
            if (expr == "false") return false;

            // Handle OR (||) - lowest precedence
            for (int i = expr.Length - 2; i >= 0; i--)
            {
                if (expr[i] == '|' && i + 1 < expr.Length && expr[i + 1] == '|' &&
                    !IsInsideParentheses(expr, i))
                {
                    return ParseBooleanExpression(expr.Substring(0, i)) ||
                           ParseBooleanExpression(expr.Substring(i + 2));
                }
            }

            // Handle AND (&&)
            for (int i = expr.Length - 2; i >= 0; i--)
            {
                if (expr[i] == '&' && i + 1 < expr.Length && expr[i + 1] == '&' &&
                    !IsInsideParentheses(expr, i))
                {
                    return ParseBooleanExpression(expr.Substring(0, i)) &&
                           ParseBooleanExpression(expr.Substring(i + 2));
                }
            }

            // Handle NOT (!)
            if (expr.StartsWith("!"))
            {
                return !ParseBooleanExpression(expr.Substring(1));
            }

            // Handle comparison operators
            // !=
            int notEqualPos = expr.IndexOf("!=");
            if (notEqualPos > 0 && !IsInsideParentheses(expr, notEqualPos))
            {
                double left = ParseExpression(expr.Substring(0, notEqualPos));
                double right = ParseExpression(expr.Substring(notEqualPos + 2));
                return Math.Abs(left - right) > 0.0001;
            }

            // ==
            int equalPos = expr.IndexOf("==");
            if (equalPos > 0 && !IsInsideParentheses(expr, equalPos))
            {
                double left = ParseExpression(expr.Substring(0, equalPos));
                double right = ParseExpression(expr.Substring(equalPos + 2));
                return Math.Abs(left - right) < 0.0001;
            }

            // <=
            int lessEqualPos = expr.IndexOf("<=");
            if (lessEqualPos > 0 && !IsInsideParentheses(expr, lessEqualPos))
            {
                double left = ParseExpression(expr.Substring(0, lessEqualPos));
                double right = ParseExpression(expr.Substring(lessEqualPos + 2));
                return left <= right;
            }

            // >=
            int greaterEqualPos = expr.IndexOf(">=");
            if (greaterEqualPos > 0 && !IsInsideParentheses(expr, greaterEqualPos))
            {
                double left = ParseExpression(expr.Substring(0, greaterEqualPos));
                double right = ParseExpression(expr.Substring(greaterEqualPos + 2));
                return left >= right;
            }

            // 
            int lessPos = FindOperator(expr, '<');
            if (lessPos > 0)
            {
                double left = ParseExpression(expr.Substring(0, lessPos));
                double right = ParseExpression(expr.Substring(lessPos + 1));
                return left < right;
            }

            // >
            int greaterPos = FindOperator(expr, '>');
            if (greaterPos > 0)
            {
                double left = ParseExpression(expr.Substring(0, greaterPos));
                double right = ParseExpression(expr.Substring(greaterPos + 1));
                return left > right;
            }

            // Handle parentheses
            if (expr.StartsWith("(") && expr.EndsWith(")"))
            {
                return ParseBooleanExpression(expr.Substring(1, expr.Length - 2));
            }

            // Try to get as boolean variable
            if (_variables.HasBoolean(expr))
            {
                return _variables.GetBoolean(expr);
            }

            // Try to evaluate as number (non-zero = true)
            double numValue = ParseExpression(expr);
            return Math.Abs(numValue) > 0.0001;
        }

        private double ParseExpression(string expr)
        {
            // Handle addition and subtraction (lowest precedence)
            for (int i = expr.Length - 1; i >= 0; i--)
            {
                if (expr[i] == '+' && !IsInsideParentheses(expr, i))
                {
                    return ParseExpression(expr.Substring(0, i)) +
                           ParseExpression(expr.Substring(i + 1));
                }
                if (expr[i] == '-' && i > 0 && !IsInsideParentheses(expr, i))
                {
                    return ParseExpression(expr.Substring(0, i)) -
                           ParseExpression(expr.Substring(i + 1));
                }
            }

            // Handle multiplication and division (higher precedence)
            for (int i = expr.Length - 1; i >= 0; i--)
            {
                if (expr[i] == '*' && !IsInsideParentheses(expr, i))
                {
                    return ParseExpression(expr.Substring(0, i)) *
                           ParseExpression(expr.Substring(i + 1));
                }
                if (expr[i] == '/' && !IsInsideParentheses(expr, i))
                {
                    double divisor = ParseExpression(expr.Substring(i + 1));
                    if (Math.Abs(divisor) < 0.0001)
                        throw new Exception("Division by zero");
                    return ParseExpression(expr.Substring(0, i)) / divisor;
                }
            }

            // Handle parentheses
            if (expr.StartsWith("(") && expr.EndsWith(")"))
            {
                return ParseExpression(expr.Substring(1, expr.Length - 2));
            }

            // Handle boolean literals
            if (expr == "true") return 1.0;
            if (expr == "false") return 0.0;

            // Handle numbers
            if (double.TryParse(expr, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
            {
                return value;
            }

            // Handle variables
            return _variables.GetValue(expr);
        }

        private bool IsInsideParentheses(string expr, int index)
        {
            int depth = 0;
            for (int i = 0; i < index; i++)
            {
                if (expr[i] == '(') depth++;
                if (expr[i] == ')') depth--;
            }
            return depth > 0;
        }

        private int FindOperator(string expr, char op)
        {
            for (int i = expr.Length - 1; i >= 0; i--)
            {
                if (expr[i] == op && !IsInsideParentheses(expr, i))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}