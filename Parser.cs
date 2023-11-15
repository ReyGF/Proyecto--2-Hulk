using System.Net.Http.Headers;
using System.Net.NetworkInformation;

namespace Hulk
{
    public abstract class Expression
    {
        abstract public ExpressionKind Kind { get; }
    }
    class UnaryExpression : Expression
    {
        public Token Sign { get; }
        public Expression Expression { get; }
        public UnaryExpression(Token sign, Expression expression)
        {
            Sign = sign;
            Expression = expression;
        }

        public override ExpressionKind Kind => ExpressionKind.UnaryExpressionKind;

    }
    class StringExpression : Expression
    {
        public Token StringToken;
        public StringExpression(Token stringtoken)
        {
            StringToken = stringtoken;
        }
        public override ExpressionKind Kind => ExpressionKind.StringExpressionKind;
    }
    class NumberExpression : Expression
    {
        public readonly Token numbertoken;

        public NumberExpression(Token Numbertoken)
        {
            numbertoken = Numbertoken;
        }

        public override ExpressionKind Kind => ExpressionKind.NumberExpressionKind;
    }
    class BoolExpression : Expression
    {
        public readonly Token BoolToken;

        public bool Type
        {
            get
            {
                return "true" == BoolToken.Text;
            }
        }
        public BoolExpression(Token booltoken)
        {
            BoolToken = booltoken;
        }

        public override ExpressionKind Kind => ExpressionKind.BoolExpressionKind;
    }
    class VariableSExpression : Expression
    {
        public string Name { get; }

        public Expression Value { get; set; }
        public VariableSExpression(string name, Expression value)
        {
            Name = name;
            Value = value;
        }

        public override ExpressionKind Kind => ExpressionKind.VariablesExpressionKind;
    }
    class IncrementDecrementExpression : Expression
    {
        public VariableSExpression Variable { get; }
        public Token Sign { get; }
        public IncrementDecrementExpression(VariableSExpression variable, Token incrementordecrement)
        {
            Variable = variable;
            Sign = incrementordecrement;
        }
        public override ExpressionKind Kind => ExpressionKind.IncrementDecrementExpressionKind;
    }
    class NullExpression : Expression
    {
        public override ExpressionKind Kind => ExpressionKind.NullExpressionKind;
    }
    class ErrorExpression : Expression
    {
        public string Message { get; }
        public ErrorExpression(string message)
        {
            Message = message;
        }
        public override ExpressionKind Kind => ExpressionKind.ErrorExpressionKind;
    }
    class FinalExpression : Expression
    {
        Token FinalToken { get; }
        public FinalExpression(Token finaltoken)
        {
            FinalToken = finaltoken;
        }
        public override ExpressionKind Kind => ExpressionKind.FinalExpressionKind;
    }
    class BinaryExpression : Expression
    {
        public Expression Left { get; }

        public Token Operator { get; }

        public Expression Right { get; }
        public BinaryExpression(Expression left, Token operatortoken, Expression right)
        {
            Left = left;
            Operator = operatortoken;
            Right = right;
        }

        public override ExpressionKind Kind => ExpressionKind.BinaryExpressionKind;

    }
    class ParenthesisExpression : Expression
    {
        public Token OpenParenthesis { get; }

        public Expression Expression { get; }

        public Token CloseParenthesis { get; }
        public ParenthesisExpression(Token openparenthesis, Expression expression, Token closeparenthesis)
        {
            OpenParenthesis = openparenthesis;
            Expression = expression;
            CloseParenthesis = closeparenthesis;
        }

        public override ExpressionKind Kind => ExpressionKind.ParenthesisExpressionKind;
    }
    class LetInExpression : Expression
    {
        Token LetToken { get; }
        public Dictionary<string, Expression> Variables { get; }
        Token Intoken { get; }
        public Expression Expression { get; }
        public LetInExpression(Token letToken, Dictionary<string, Expression> variables, Token intoken, Expression expression)
        {
            LetToken = letToken;
            Variables = variables;
            Intoken = intoken;
            Expression = expression;
        }
        public override ExpressionKind Kind => ExpressionKind.LetInExpressionKind;
    }
    class AsignamentVariablesExpression : Expression
    {
        public Dictionary<string, Expression> Variables { get; }
        public AsignamentVariablesExpression(Dictionary<string, Expression> variables)
        {
            Variables = variables;
        }

        public override ExpressionKind Kind => ExpressionKind.AsignamentVariablesExpressionKind;
    }
    class ConditionalExpression : Expression
    {
        Token IfToken { get; }

        public ParenthesisExpression ParenthesisExpression { get; }

        public Expression IfExpression { get; }

        Token ElseToken { get; }

        public Expression ElseExpression { get; }
        public ConditionalExpression(Token iftoken, ParenthesisExpression parenthesisExpression, Expression ifExpression, Token elsetoken, Expression elseExpression)
        {
            IfToken = iftoken;
            ParenthesisExpression = parenthesisExpression;
            IfExpression = ifExpression;
            ElseToken = elsetoken;
            ElseExpression = elseExpression;
        }
        public override ExpressionKind Kind => ExpressionKind.ConditionalExpressionKind;

    }
    public class DeclarateFunctionExpression : Expression
    {
        Token Identifier { get; }
        public Token Name { get; }
        Token OpenParenthesis { get; }
        public ParameterExpression Parameters { get; }
        Token CloseParenthesis { get; }
        Token Lamda { get; }
        public Expression Expression;
        public DeclarateFunctionExpression(Token identifier, Token namefunctiontoken, Token openparenthesis, ParameterExpression parameters, Token closeparenthesis,
                                            Token lambda, Expression functionexpression)
        {
            Identifier = identifier;
            Name = namefunctiontoken;

            OpenParenthesis = openparenthesis;
            Parameters = parameters;
            CloseParenthesis = closeparenthesis;

            Lamda = lambda;

            Expression = functionexpression;
        }

        public override ExpressionKind Kind => ExpressionKind.DeclarateFuncitonExpressionKind;
    }
    public class ParameterExpression : Expression
    {
        public string[] NameVariables = new string[9];
        public Dictionary<string, Expression> Values = new Dictionary<string, Expression>();
        public ParameterExpression(Dictionary<string, Expression> parametersValues, string[] namevariables)
        {
            NameVariables = namevariables;
            Values = parametersValues;
        }
        public override ExpressionKind Kind => ExpressionKind.ParameterExpressionKind;
    }
    class FunctionExpression : Expression
    {
        public string Name { get; }
        Token Open { get; }
        public Expression[] ParametersValues { get; }
        Token Close { get; }
        public FunctionExpression(string name, Token open, Expression[] parametersvalues, Token close)
        {
            Name = name;
            Open = open;
            ParametersValues = parametersvalues;
            Close = close;
        }
        public override ExpressionKind Kind => ExpressionKind.FuncitonExpressionKind;
    }
    class FunctionVariableExpresison : Expression
    {
        public string Name { get; }

        public string NameFunction { get; }

        public object Value { get; set; }
        public FunctionVariableExpresison(string name, string namefunction, object value)
        {
            Name = name;
            NameFunction = namefunction;
            Value = value;
        }
        public override ExpressionKind Kind => ExpressionKind.FuncitonVariableExpressionKind;
    }
    class SenExpression : Expression
    {
        public Expression value;
        public SenExpression(Expression Value)
        {
            value = Value;
        }
        public override ExpressionKind Kind => ExpressionKind.SenExpressionKind;
    }
    class CosExpression : Expression
    {
        public Expression value;
        public CosExpression(Expression Value)
        {
            value = Value;
        }
        public override ExpressionKind Kind => ExpressionKind.CosExpressionKind;
    }
    class TanExpression : Expression
    {
        public Expression value;
        public TanExpression(Expression Value)
        {
            value = Value;
        }
        public override ExpressionKind Kind => ExpressionKind.TanExpressionKind;
    }
    class LogExpression : Expression
    {
        public Expression value1;
        public Expression value2;
        public LogExpression(Expression Value1, Expression Value2)
        {
            value1 = Value1;
            value2 = Value2;
        }
        public override ExpressionKind Kind => ExpressionKind.LogExpressionKind;
    }
    class PrintExpression : Expression
    {
        public Expression value;
        public PrintExpression(Expression Value)
        {
            value = Value;
        }
        public override ExpressionKind Kind => ExpressionKind.PrintExpressionKind;
    }

    public class Parser
    {
        public Dictionary<string, Expression> Variables = new Dictionary<string, Expression>();
        //public List<string> NameFunctions = new List<string>();
        private readonly Token[] _tokens = new Token[20];
        private int _pos;
        private bool IsFunctionVariable = false;
        private Token Current
        {
            get
            {
                if (_pos >= _tokens.Length)
                    return new Token(TokenKind.EndToken, "\n");

                return _tokens[_pos];
            }
        }
        public Parser(string text, Dictionary<string, Expression> variables)
        {
            var lexer = new Lexer(text);

            var tokens = lexer.GetTokens();

            _tokens = tokens;

            Variables = variables;

            /*for (int i = 0; i < _tokens.Length; i++)
             {
                 Console.WriteLine($"{_tokens[i].Kind} : {_tokens[i].Text}");
             }*/
        }
        public void SetPosition()
        {
            _pos++;
        }
        public Token GetCurrent()
        {
            return Current;
        }
        public Expression Parse() => B();
        public Expression B() => O();
        private Expression O()
        {
            var left = A();

            if (Current.Kind != TokenKind.EndToken && Current.Kind != TokenKind.FinalInstructionToken)
            {
                while (Current.Kind == TokenKind.LogicalOrToken)
                {
                    var operatortoken = Current; _pos++;

                    var right = A();

                    left = new BinaryExpression(left, operatortoken, right);
                }
            }
            return left;
        }
        private Expression A()
        {
            var left = N();

            if (Current.Kind != TokenKind.EndToken && Current.Kind != TokenKind.FinalInstructionToken)
            {
                while (Current.Kind == TokenKind.LogicalAndToken)
                {
                    var operatortoken = Current; _pos++;

                    var right = N();

                    left = new BinaryExpression(left, operatortoken, right);
                }
            }
            return left;
        }
        private Expression N()
        {
            var left = T();

            if (Current.Kind != TokenKind.EndToken && Current.Kind != TokenKind.FinalInstructionToken)
            {
                while (Current.Kind == TokenKind.LessThanEqualToken || Current.Kind == TokenKind.LessThanToken ||
                    Current.Kind == TokenKind.GreaterThanEqualToken || Current.Kind == TokenKind.GreaterThanToken
                    || Current.Kind == TokenKind.EqualEqualToken)
                {
                    var operatortoken = Current; _pos++;

                    var right = T();

                    left = new BinaryExpression(left, operatortoken, right);
                }
            }
            return left;
        }



        private Expression T()
        {
            var left = F();
            if (Current.Kind != TokenKind.EndToken && Current.Kind != TokenKind.FinalInstructionToken)
            {
                while (Current.Kind == TokenKind.PlusToken || Current.Kind == TokenKind.MinusToken || Current.Kind == TokenKind.UnionToken)
                {
                    var operatortoken = Current; _pos++;

                    var right = F();

                    left = new BinaryExpression(left, operatortoken, right);
                }
            }
            return left;
        }
        private Expression F()
        {
            var left = P();

            if (Current.Kind != TokenKind.EndToken && Current.Kind != TokenKind.FinalInstructionToken)
            {
                while (Current.Kind == TokenKind.StarToken || Current.Kind == TokenKind.SlashToken || Current.Kind == TokenKind.ModuleToken)
                {
                    var operatortoken = Current; _pos++;

                    var right = P();

                    left = new BinaryExpression(left, operatortoken, right);
                }
            }
            return left;
        }
        private Expression P()
        {
            var left = E(Current);

            if (Current.Kind != TokenKind.EndToken && Current.Kind != TokenKind.FinalInstructionToken)
            {
                while (Current.Kind == TokenKind.PowToken)
                {
                    var operatortoken = Current; _pos++;

                    var right = E(Current);

                    left = new BinaryExpression(left, operatortoken, right);
                }
            }
            return left;
        }

        public Expression E(Token currentToken)
        {
            _pos++;
            if (currentToken.Kind == TokenKind.InvalidToken)
            {
                return new ErrorExpression($"Invalid Token {currentToken.Text} : parser.455");
            }
            if (currentToken.Kind == TokenKind.EndToken)
            {
                return new FinalExpression(currentToken);
            }
            if (currentToken.Kind == TokenKind.NumberToken)
                return new NumberExpression(currentToken);
            else if (currentToken.Kind == TokenKind.TrueToken || currentToken.Kind == TokenKind.FlaseToken)
                return new BoolExpression(currentToken);
            else if (currentToken.Kind == TokenKind.VariableToken)
            {
                if (Current.Kind == TokenKind.EqualToken)
                {
                    _pos--;
                    Dictionary<string, Expression> Var = new Dictionary<string, Expression>();
                    ParseAssignamentvariable(Var);

                    return new AsignamentVariablesExpression(Var);
                }
                else if (Current.Kind == TokenKind.IncrementToken || Current.Kind == TokenKind.DecrementToken)
                {
                    if (!Variables.ContainsKey(currentToken.Text))
                        return new ErrorExpression($"La variable {currentToken.Text} dossent exist!! : parser.478");
                    else
                    {
                        var sign = Current;
                        _pos++;
                        return new IncrementDecrementExpression(new VariableSExpression(currentToken.Text, Variables[currentToken.Text]), sign);
                    }
                }
                else
                {
                    if (IsFunctionVariable)
                    {
                        return new FunctionVariableExpresison(currentToken.Text, Program.NameFunctions[Program.NameFunctions.Count - 1], new NullExpression());
                    }
                    else
                    {
                        if (!Variables.ContainsKey(currentToken.Text))
                        {
                            return new VariableSExpression(currentToken.Text, new NullExpression());
                        }
                        else
                            return new VariableSExpression(currentToken.Text, Variables[currentToken.Text]);
                    }
                }
            }
            else if (currentToken.Kind == TokenKind.StringToken)
                return new StringExpression(currentToken);
            else if (currentToken.Kind == TokenKind.LogicalNegationToken)
            {
                var left = currentToken;

                var expression = E(Current);

                return new UnaryExpression(left, expression);
            }

            else if (currentToken.Kind == TokenKind.MinusToken || currentToken.Kind == TokenKind.PlusToken)
            {
                var left = currentToken;

                var expression = E(Current);

                return new UnaryExpression(left, expression);
            }
            else if (currentToken.Kind == TokenKind.SenToken || currentToken.Kind == TokenKind.CosToken ||
                     currentToken.Kind == TokenKind.TanToken || currentToken.Kind == TokenKind.PrintToken)
            {
                if (Current.Kind != TokenKind.OpenParenthesisToken)
                    return new ErrorExpression("Se esperaba '(' : parser.527");

                _pos++;
                var argument = Parse();

                if (Current.Kind != TokenKind.CloseParenthesisToken)
                    return new ErrorExpression("se esperaba ')' : parser.541");
                _pos++;
                switch (currentToken.Kind)
                {
                    case TokenKind.SenToken:
                        return new SenExpression(argument);

                    case TokenKind.CosToken:
                        return new CosExpression(argument);

                    case TokenKind.TanToken:
                        return new TanExpression(argument);

                    case TokenKind.PrintToken:
                        return new PrintExpression(argument);

                }
            }
            else if (currentToken.Kind == TokenKind.LogToken)
            {
                var arguments = new Expression[2];
                if (Current.Kind != TokenKind.OpenParenthesisToken)
                    return new ErrorExpression("Se esperaba '(' : parser.555");

                _pos++;
                arguments[0] = Parse();
                _pos++;
                arguments[1] = Parse();

                if (Current.Kind != TokenKind.CloseParenthesisToken)
                    return new ErrorExpression("Se esperaba '(' : parser.555");
                _pos++;

                return new LogExpression(arguments[0], arguments[1]);
            }
            else if (currentToken.Kind == TokenKind.IdentifierFunctionToken)
            {
                Dictionary<string, Expression> parameters = new Dictionary<string, Expression>();

                var identifier = currentToken;
                if (Current.Kind != TokenKind.FunctionNameToken)
                    return new ErrorExpression($"El nombre '{Current.Text}' no es valido para una funcion   : parser.564");

                var name = Current;
                _pos++;
                if (Current.Kind != TokenKind.OpenParenthesisToken)
                    return new ErrorExpression("Se esperaba '(' : parser. 569");

                var openparenthesis = Current;

                if (!Program.NameFunctions.Contains(name.Text))
                {
                    Program.NameFunctions.Add(name.Text);

                    string[] namevariables = new string[9];
                    int k = 0;
                    do
                    {
                        _pos++;
                        try
                        {
                            var error = int.Parse(Current.Text.ToString());
                            return new ErrorExpression($"nombre de variable '{error} invalido' :parser.585");
                        }
                        catch (FormatException)
                        {
                            var variable = Current.Text;
                            namevariables[k] = variable;
                            k++;
                            _pos++;

                            if (Current.Kind == TokenKind.EqualToken)
                            {
                                _pos++;
                                var expression = Parse();

                                parameters.Add(variable, expression);
                            }
                            else
                            {
                                parameters.Add(variable, new NullExpression());
                            }
                        }
                    }
                    while (Current.Kind == TokenKind.CommaToken);

                    var functionparameters = new ParameterExpression(parameters, namevariables);

                    if (Current.Kind != TokenKind.CloseParenthesisToken)
                        return new ErrorExpression("se esperaba ')' :parser.614");

                    var closeparenthesis = Current; _pos++;

                    if (Current.Kind != TokenKind.LambdaToken)
                        return new ErrorExpression("se esperaba '=>' : ");

                    var lambdaToken = Current; _pos++;

                    IsFunctionVariable = true;

                    var functionexpression = Parse();

                    IsFunctionVariable = false;

                    return new DeclarateFunctionExpression(identifier, name, openparenthesis, functionparameters, closeparenthesis,
                                                             lambdaToken, functionexpression);
                }
                else
                    return new ErrorExpression($"la funcion '{name.Text}' ya fue declarada : parser.633 ");
            }
            else if (currentToken.Kind == TokenKind.FunctionNameToken)
            {
                if (!Program.NameFunctions.Contains(currentToken.Text))
                {
                    return new ErrorExpression($" la funcion '{currentToken.Text}' no existe : parser.639");
                }
                else
                {
                    int i = 0;
                    if (Current.Kind != TokenKind.OpenParenthesisToken)
                        return new ErrorExpression("se esperaba '(' : parser.645");

                    var openparenthesis = Current;
                    var parametersvalues = new Expression[9];
                    do
                    {
                        _pos++;

                        var parametervalue = Parse();

                        parametersvalues[i] = parametervalue;
                        i++;

                    } while (Current.Kind == TokenKind.CommaToken);

                    if (Current.Kind != TokenKind.CloseParenthesisToken)
                        return new ErrorExpression("se esperaba ')' : parser.661");

                    var close = Current; _pos++;

                    return new FunctionExpression(currentToken.Text, openparenthesis, parametersvalues, close);
                }

            }
            else if (currentToken.Kind == TokenKind.CloseParenthesisToken)
            {
                return new ErrorExpression("Falta '(' : parser.675");
            }

            else if (currentToken.Kind == TokenKind.OpenParenthesisToken)
            {
                var open = currentToken;

                var parenthesisexpression = Parse();

                if (Current.Kind != TokenKind.CloseParenthesisToken)
                    return new ErrorExpression("se esperaba ')' : parser.677");

                var close = Current;
                _pos++;

                return new ParenthesisExpression(open, parenthesisexpression, close);
            }
            else if (currentToken.Kind == TokenKind.IfToken)
            {
                var iftoken = currentToken;

                var parenthesisExpression = (ParenthesisExpression)Parse();

                var ifExpression = Parse();

                if (Current.Kind != TokenKind.ElseToken)
                    return new ErrorExpression("se esperaba 'else' : parser.693");

                var elseToken = Current;

                _pos++;

                var elseExpression = Parse();

                return new ConditionalExpression(iftoken, parenthesisExpression, ifExpression, elseToken, elseExpression);

            }
            else if (currentToken.Kind == TokenKind.LetToken)
            {
                var Varibales = new Dictionary<string, Expression>();

                var let = currentToken;

                ParseAssignamentvariable(Varibales);

                foreach (var item in Varibales.Keys)
                {
                    if (!Variables.ContainsKey(item))
                        Variables.Add(item, Varibales[item]);
                    else
                        Variables[item] = Varibales[item];
                }

                if (Current.Kind == TokenKind.InToken)
                {
                    var intoken = Current; _pos++;
                    var expression = Parse();
                    return new LetInExpression(let, Varibales, intoken, expression);
                }
                else return new AsignamentVariablesExpression(Varibales);
            }

            return new ErrorExpression($"unexpected token {Current.Text} : {_pos}");
        }

        private void ParseAssignamentvariable(Dictionary<string, Expression> Varibales)
        {
            try
            {
                var error = int.Parse(Current.Text);
                Variables.Add(error.ToString(), new ErrorExpression("los parametro no pueden ser numeros : parser.737"));
            }
            catch (FormatException)
            {
                if (Current.Kind == TokenKind.VariableToken)
                {

                    var variablename = Current.Text;
                    _pos++;
                    if (Current.Kind == TokenKind.EqualToken)
                    {
                        _pos++;
                        if (Current.Kind == TokenKind.InToken)
                        {
                            Variables.Add("error", new ErrorExpression("Missing value in let in expression : parser,760"));
                        }
                        else
                        {
                            var item1 = variablename;
                            var item2 = Parse();
                            try
                            {

                                Varibales.Add(item1, item2);
                            }
                            catch (ArgumentException)
                            {
                                Varibales[variablename] = item2;
                            }

                        }
                    }
                    else
                    {
                        try
                        {
                            Varibales.Add(variablename, new NullExpression());
                        }
                        catch (ArgumentException)
                        {
                            Varibales[variablename] = new NullExpression();
                        }
                    }
                    while (Current.Kind == TokenKind.CommaToken)
                    {
                        _pos++;
                        ParseAssignamentvariable(Varibales);
                    }
                }
                else
                {
                    Varibales.Add("error", new ErrorExpression($"Invalid name '{Current.Text}' variable"));
                }
            }

        }
    }
}