using System;
using System.Globalization;

namespace BOOSEapp
{
    /// <summary>
    /// Evaluates mathematical expressions with variables
    /// Supports: +, -, *, /, parentheses
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
    }
}