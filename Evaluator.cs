using System.Globalization;
using System.Net.NetworkInformation;

namespace Hulk
{

    public class Evaluator
    {
        public Dictionary<string, Expression> Variables = new Dictionary<string, Expression>();
        public Dictionary<string, DeclarateFunctionExpression> DeclarateFunction = new Dictionary<string, DeclarateFunctionExpression>();
        Dictionary<string, object>[] stack = new Dictionary<string, object>[500];
        int index = 0;
        Dictionary<string, object> CurrentFunctionParametersValues = new Dictionary<string, object>();
        object[] EvaluateExpressions = new object[10];
        int i = 0;
        public Evaluator(string text, Dictionary<string, Expression> variables,
                        Dictionary<string, DeclarateFunctionExpression> declarateFunction)
        {
            var parser = new Parser(text, variables);

            var expression = parser.Parse();

            Variables = variables;

            foreach (var item in declarateFunction.Keys)
            {
                if (DeclarateFunction.ContainsKey(item))
                {
                    DeclarateFunction[item] = declarateFunction[item];
                }
                else
                {
                    DeclarateFunction.Add(item, declarateFunction[item]);
                }
            }
            if (expression.Kind == ExpressionKind.ErrorExpressionKind)
            {
                EvaluateExpressions[0] = EvaluateExpression(expression);
            }
            else
            {
                while (expression.Kind != ExpressionKind.FinalExpressionKind)
                {
                    EvaluateExpressions[i] = EvaluateExpression(expression);
                    if (parser.GetCurrent().Kind == TokenKind.FinalInstructionToken)
                    {
                        parser.SetPosition();
                    }
                    else
                    {
                        EvaluateExpressions[i] = $"Invalid Token '{parser.GetCurrent().Text}' : Se esperaba ';' : evaluator.51";
                        break;
                    }

                    expression = parser.Parse();
                    i++;
                }
            }


        }
        public object[] Evaluate()
        {
            return EvaluateExpressions;
        }

        object EvaluateExpression(Expression expression)
        {
            if (expression is ErrorExpression e)
            {
                return e.Message;
            }
            if (expression is FinalExpression)
            {
                return "";
            }
            if (expression is NumberExpression number)
            {
                return double.Parse(number.numbertoken.Text);
            }
            else if (expression is VariableSExpression v)
            {
                if (v.Value is NullExpression)
                {
                    return $"El valor de '{v.Name}' es null aqui : evaluator.77";
                }
                return EvaluateExpression(v.Value);
            }
            else if (expression is FunctionVariableExpresison fve)
            {
                return stack[index][fve.Name];

            }
            else if (expression is StringExpression stringExpression)
            {
                return stringExpression.StringToken.Text;
            }
            else if (expression is BoolExpression boolExpression)
            {
                return boolExpression.Type;
            }
            else if (expression is BinaryExpression binaryExpression)
            {
                var left = EvaluateExpression(binaryExpression.Left);
                var right = EvaluateExpression(binaryExpression.Right);
                if (left is ErrorExpression || right is ErrorExpression)
                {
                    return "!!!!!!!!1Error!!!!!!!!";
                }

                switch (binaryExpression.Operator.Kind)
                {
                    case TokenKind.PlusToken:
                        try
                        {
                            return (double)left + (double)right;
                        }
                        catch (InvalidCastException)
                        {
                            return "Solo se pueden sumar numeros";
                        }
                    case TokenKind.MinusToken:
                        try
                        {
                            return (double)left - (double)right;
                        }
                        catch (InvalidCastException)
                        {
                            return "Solo se pueden restar numeros";
                        }
                    case TokenKind.StarToken:
                        try
                        {
                            return (double)left * (double)right;
                        }
                        catch (InvalidCastException)
                        {
                            return "Solo se pueden multiplicar numeros";
                        }
                    case TokenKind.SlashToken:
                        try
                        {
                            return (double)left / (double)right;
                        }
                        catch (InvalidCastException)
                        {
                            return "Solo se pueden dividir numeros";
                        }
                    case TokenKind.UnionToken:
                        return left.ToString() + right.ToString();
                    case TokenKind.ModuleToken:
                        try
                        {
                            return (double)left % (double)right;
                        }
                        catch (InvalidCastException)
                        {
                            return "La operacion '%' solo esta definida para numeros";
                        }
                    case TokenKind.PowToken:
                        try
                        {
                            return Math.Pow((double)left, (double)right);
                        }
                        catch (InvalidCastException)
                        {
                            return "La operacion '^' solo esta definida para numeros";
                        }
                    case TokenKind.LogicalAndToken:
                        try
                        {
                            left = (double)left;
                        }
                        catch (InvalidCastException)
                        {
                            left = (bool)left;
                        }
                        try
                        {
                            right = (double)right;
                        }
                        catch (InvalidCastException)
                        {
                            right = (bool)right;
                        }
                        if (left is double la)
                        {
                            left = la != 0;
                        }
                        if (right is double ra)
                        {
                            right = ra != 0;
                        }
                        return (bool)left && (bool)right;
                    case TokenKind.LogicalOrToken:
                        try
                        {
                            left = (double)left;
                        }
                        catch (InvalidCastException)
                        {
                            left = (bool)left;
                        }
                        try
                        {
                            right = (double)right;
                        }
                        catch (InvalidCastException)
                        {
                            right = (bool)right;
                        }
                        if (left is double lo)
                        {
                            left = lo != 0;
                        }
                        if (right is double ro)
                        {
                            right = ro != 0;
                        }
                        return (bool)left || (bool)right;
                    case TokenKind.EqualEqualToken:
                        try
                        {
                            return (double)left == (double)right;
                        }
                        catch (InvalidCastException)
                        {
                            try
                            {
                                return (bool)left == (bool)right;
                            }
                            catch (InvalidCastException)
                            {
                                try
                                {
                                    return (string)left == (string)right;
                                }
                                catch (InvalidCastException)
                                {
                                    return "Solo puedes comprar tipos iguales";
                                }
                            }
                        }

                    case TokenKind.DifferentToken:
                        try
                        {
                            return (double)left != (double)right;
                        }
                        catch (InvalidCastException)
                        {
                            try
                            {

                                return (bool)left != (bool)right;
                            }
                            catch (InvalidCastException)
                            {
                                return (string)left != (string)right;
                            }
                        }
                    case TokenKind.LessThanToken:
                        try
                        {

                            return (double)left < (double)right;
                        }
                        catch (InvalidCastException)
                        {
                            return "La operacion '<' solo esta definida para numeros";
                        }
                    case TokenKind.LessThanEqualToken:
                        try
                        {

                            return (double)left <= (double)right;
                        }
                        catch (InvalidCastException)
                        {
                            return "La operacion '<' solo esta definida para numeros";
                        }
                    case TokenKind.GreaterThanToken:
                        try
                        {

                            return (double)left > (double)right;
                        }
                        catch (InvalidCastException)
                        {
                            return "La operacion '<' solo esta definida para numeros";
                        }
                    case TokenKind.GreaterThanEqualToken:
                        try
                        {

                            return (double)left >= (double)right;
                        }
                        catch (InvalidCastException)
                        {
                            return "La operacion '<' solo esta definida para numeros";
                        }
                    default:
                        return "Unexpected token";
                }

            }
            else if (expression is ParenthesisExpression parenthesisExpression)
            {
                return EvaluateExpression(parenthesisExpression.Expression);
            }
            else if (expression is UnaryExpression unaryExpression)
            {
                if (unaryExpression.Sign.Kind == TokenKind.MinusToken)
                    try
                    {

                        return -(double)EvaluateExpression(unaryExpression.Expression);
                    }
                    catch (InvalidCastException)
                    {
                        return "el operador '-' solo se puede usar en numeros";
                    }

                else if (unaryExpression.Sign.Kind == TokenKind.LogicalNegationToken)
                    try
                    {

                        return !(bool)EvaluateExpression(unaryExpression.Expression);
                    }
                    catch (InvalidCastException)
                    {
                        return "el operador '!' solo se puede usar en booleanos";
                    }

                else return EvaluateExpression(unaryExpression.Expression);
            }
            else if (expression is IncrementDecrementExpression IDE)
            {
                try
                {
                    return (IDE.Sign.Kind == TokenKind.IncrementToken) ? ((double)EvaluateExpression(IDE.Variable) + 1)
                                                                       : ((double)EvaluateExpression(IDE.Variable) - 1);
                }
                catch (InvalidCastException)
                {
                    return "solo se aceptan numeros : evaluator.242";
                }
            }
            else if (expression is ConditionalExpression conditionalExpression)
            {
                var conditional = EvaluateExpression(conditionalExpression.ParenthesisExpression);
                try
                {
                    conditional = (bool)conditional;
                }
                catch (InvalidCastException)
                {
                    conditional = (double)conditional;
                }
                if (conditional is double i)
                {
                    conditional = i != 0;
                }
                if ((bool)conditional)
                {
                    return EvaluateExpression(conditionalExpression.IfExpression);
                }
                else
                {
                    return EvaluateExpression(conditionalExpression.ElseExpression);
                }
            }
            else if (expression is LetInExpression l)
            {
                if (l.Variables.ContainsKey("error"))
                {
                    return l.Variables["error"];
                }
                else
                {
                    foreach (var item in l.Variables.Keys)
                    {
                        if (!Variables.ContainsKey(item))
                            Variables.Add(item, l.Variables[item]);
                        else
                            Variables[item] = l.Variables[item];
                    }

                    return EvaluateExpression(l.Expression);
                }
            }
            else if (expression is AsignamentVariablesExpression a)
            {
                foreach (var namevar in a.Variables.Keys)
                {

                    if (!Variables.ContainsKey(namevar))
                    {
                        return $"La variable '{namevar}' no existe en el contexto actual";
                    }
                    else
                    {
                        Variables[namevar] = a.Variables[namevar];
                    }
                }
                return "";
            }
            else if (expression is DeclarateFunctionExpression declarateFunctionExpression)
            {
                DeclarateFunction.Add(declarateFunctionExpression.Name.Text, declarateFunctionExpression);
                return "";
            }
            else if (expression is FunctionExpression functionExpression)
            {
                int i = 0;
                var function = DeclarateFunction[functionExpression.Name]; //f()
                var DictionaryParametersValues = function.Parameters.Values; //f(n = null, x = 5)
                var ArrayNamevar = function.Parameters.NameVariables; //[n, x]

                if (CountParameters(ArrayNamevar) < CountParameters(functionExpression.ParametersValues))
                {
                    return "faltan parametros : evaluator.310";
                }

                while (ArrayNamevar[i] != null)
                {
                    if (functionExpression.ParametersValues[i] == null) //f(n - 1,2)
                    {
                        // Console.WriteLine("aqui es null" + EvaluateExpression(DictionaryParametersValues[ArrayNamevar[i]]));                                        // n = n - 1 ; x = 2 
                        CurrentFunctionParametersValues[ArrayNamevar[i]] = EvaluateExpression(DictionaryParametersValues[ArrayNamevar[i]]);
                    }
                    else
                    {
                        //Console.WriteLine("aqui no es null " + EvaluateExpression(functionExpression.ParametersValues[i]));
                        CurrentFunctionParametersValues[ArrayNamevar[i]] = EvaluateExpression(functionExpression.ParametersValues[i]);
                    }
                    i++;
                }
                index++;
                stack[index] = new Dictionary<string, object>();

                foreach (var item in CurrentFunctionParametersValues.Keys)
                {
                    stack[index].Add(item, CurrentFunctionParametersValues[item]);
                }

                //fib(n) => if(n > 1) fib(n-1) + fib(n-2) else 1; f(3)
                var funexp = EvaluateExpression(function.Expression);

                index--;

                return funexp;

            }
            if (expression is SenExpression sen)
            {
                try
                {
                    return Math.Sin((double)EvaluateExpression(sen.value));
                }
                catch (InvalidCastException)
                {
                    return "la funcion sin solo acepta valores double : parser.351";
                }
            }
            if (expression is CosExpression cos)
            {
                try
                {
                    return Math.Cos((double)EvaluateExpression(cos.value));

                }
                catch (InvalidCastException)
                {
                    return "la funcion cos solo acepta valores double : parser.363";
                }
            }
            if (expression is TanExpression tan)
            {
                try
                {
                    return Math.Tan((double)EvaluateExpression(tan.value));
                }
                catch (InvalidCastException)
                {
                    return "la funcion tan solo acepta valores double : parser.374";
                }
            }
            if (expression is LogExpression log)
            {
                try
                {
                    return Math.Log((double)EvaluateExpression(log.value1), (double)EvaluateExpression(log.value2));
                }
                catch (InvalidCastException)
                {
                    return "la funcion log solo acepta valores double : parser.385";
                }
            }
            if (expression is PrintExpression print)
            {
                return EvaluateExpression(print.value);
            }

            return "Unexpected expression";
        }
        static bool Find(string[] array, string element)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == element)
                {
                    return true;
                }
            }
            return false;
        }
        static int CountParameters(string[] ArrayNamevar)
        {
            int count = 0;
            for (int j = 0; j < ArrayNamevar.Length; j++)
            {
                if (ArrayNamevar[j] != null) count++;
            }
            return count;
        }
        static int CountParameters(Expression[] ArrayNamevar)
        {
            int count = 0;
            for (int j = 0; j < ArrayNamevar.Length; j++)
            {
                if (ArrayNamevar[j] != null) count++;
            }
            return count;
        }

    }
}