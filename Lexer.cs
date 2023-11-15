namespace Hulk
{
    public class Lexer
    {
        private readonly string _text;

        private int _position;

        public Lexer(string text)
        {
            _text = text;
        }

        private char Current
        {
            get
            {
                if (_position >= _text.Length)
                    return '\0';

                return _text[_position];
            }
        }
        public Token[] GetTokens()
        {
            var tokens = new List<Token>();

            while (_position <= _text.Length)
            {
                var currenttoken = GetToken();

                if (/*currenttoken.Kind != TokenKind.InvalidToken &&*/ currenttoken.Kind != TokenKind.WhiteSpaceToken)
                    tokens.Add(currenttoken);


                _position++;
            }
            return tokens.ToArray();
        }
        private Token GetToken()
        {
            if (_position >= _text.Length)
                return new Token(TokenKind.EndToken, "\n");

            if (char.IsDigit(Current))
            {
                string cadena = "";

                while (char.IsDigit(Current))
                {
                    cadena += Current;
                    _position++;
                }
                if (Current == '.')
                {
                    do
                    {
                        cadena += Current;
                        _position++;
                    }
                    while (char.IsDigit(Current));
                    if (Current == '.')
                        return new Token(TokenKind.InvalidToken, cadena);
                }
                _position--;


                return new Token(TokenKind.NumberToken, cadena);
            }

            if (char.IsLetter(Current))
            {
                string cadena = "";

                while (char.IsLetter(Current))
                {
                    cadena += Current;
                    _position++;
                }
                _position--;

                switch (cadena)
                {
                    case "true":
                        return new Token(TokenKind.TrueToken, cadena);
                    case "false":
                        return new Token(TokenKind.FlaseToken, cadena);
                    case "let":
                        return new Token(TokenKind.LetToken, cadena);
                    case "in":
                        return new Token(TokenKind.InToken, cadena);
                    case "if":
                        return new Token(TokenKind.IfToken, cadena);
                    case "else":
                        return new Token(TokenKind.ElseToken, cadena);
                    case "function":
                        return new Token(TokenKind.IdentifierFunctionToken, cadena);
                    case "sin":
                        return new Token(TokenKind.SenToken, cadena);
                    case "cos":
                        return new Token(TokenKind.CosToken, cadena);
                    case "tan":
                        return new Token(TokenKind.TanToken, cadena);
                    case "log":
                        return new Token(TokenKind.LogToken, cadena);
                    case "print":
                        return new Token(TokenKind.PrintToken, cadena);
                    default:
                        _position++;
                        while (char.IsWhiteSpace(Current))
                        {
                            _position++;
                        }
                        if (Current == '(')
                        {
                            _position--;
                            return new Token(TokenKind.FunctionNameToken, cadena);

                        }
                        else
                        {
                            _position--;
                            return new Token(TokenKind.VariableToken, cadena);
                        }
                }
            }

            if (char.IsWhiteSpace(Current))
            {
                while (char.IsWhiteSpace(Current))
                {
                    _position++;
                }
                _position--;
                return new Token(TokenKind.WhiteSpaceToken, " ");
            }

            switch (Current)
            {
                case '+':
                    _position++;
                    if (Current == '+')
                        return new Token(TokenKind.IncrementToken, "++");
                    else if (Current == '-')
                    {
                        return new Token(TokenKind.InvalidToken, "+-");
                    }
                    else
                        _position--;

                    return new Token(TokenKind.PlusToken, "+");

                case '-':
                    _position++;
                    if (Current == '-')
                        return new Token(TokenKind.DecrementToken, "--");
                    else if (Current == '+')
                    {
                        return new Token(TokenKind.InvalidToken, "-+");
                    }
                    else
                        _position--;
                    return new Token(TokenKind.MinusToken, "-");
                case '*':
                    return new Token(TokenKind.StarToken, "*");
                case '/':
                    return new Token(TokenKind.SlashToken, "/");
                case '@':
                    return new Token(TokenKind.UnionToken, "@");
                case '%':
                    return new Token(TokenKind.ModuleToken, "%");
                case '^':
                    return new Token(TokenKind.PowToken, "^");
                case '(':
                    return new Token(TokenKind.OpenParenthesisToken, "(");
                case ')':
                    return new Token(TokenKind.CloseParenthesisToken, ")");
                case '&':
                    return new Token(TokenKind.LogicalAndToken, "&");
                case '|':
                    return new Token(TokenKind.LogicalOrToken, "|");
                case ',':
                    return new Token(TokenKind.CommaToken, ",");
                case ';':
                    return new Token(TokenKind.FinalInstructionToken, ";");
                case '"':
                    string cadena = "";
                    _position++;

                    while (Current != '"')
                    {
                        cadena += Current;
                        _position++;

                    }
                    return new Token(TokenKind.StringToken, cadena);
                case '!':
                    _position++;
                    if (Current == '=')
                        return new Token(TokenKind.DifferentToken, "!=");
                    else
                    {
                        _position--;
                        return new Token(TokenKind.LogicalNegationToken, "!");
                    }
                case '=':
                    _position++;
                    if (Current == '=')
                        return new Token(TokenKind.EqualEqualToken, "==");
                    else if (Current == '>')
                        return new Token(TokenKind.LambdaToken, "=>");
                    else
                    {
                        _position--;
                        return new Token(TokenKind.EqualToken, "=");
                    }
                case '<':
                    _position++;
                    if (Current == '=')
                        return new Token(TokenKind.LessThanEqualToken, "<=");
                    else
                    {
                        _position--;
                        return new Token(TokenKind.LessThanToken, "<");
                    }
                case '>':
                    _position++;
                    if (Current == '=')
                        return new Token(TokenKind.GreaterThanEqualToken, ">=");
                    else
                    {
                        _position--;
                        return new Token(TokenKind.GreaterThanToken, ">");
                    }
                default:
                    return new Token(TokenKind.InvalidToken, "" + Current);
            }

        }
    }
}