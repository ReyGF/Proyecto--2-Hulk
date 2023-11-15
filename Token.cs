
namespace Hulk
{
    public class Token
    {
        public string Text { get; }
        public TokenKind Kind { get; }
        public Token(TokenKind kind, string text)
        {
            Kind = kind;
            Text = text;

        }
    }
}