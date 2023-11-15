using System.Data;
using System.Security.Cryptography;

namespace Hulk
{
    //Por favor leerse las instrucciones de uso al final del readme
    class Program
    {
        public static List<string> NameFunctions = new List<string>();
        static void Main()
        {
            Dictionary<string, Expression> Variables = new Dictionary<string, Expression>();
            Variables.Add("PI", new VariableSExpression("PI", new NumberExpression(new Token(TokenKind.NumberToken, "3.14"))));
            Dictionary<string, DeclarateFunctionExpression> DeclarateFunction = new Dictionary<string, DeclarateFunctionExpression>();

            Console.WriteLine("Hulk");
            Console.WriteLine("::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::");

            while (true)
            {
                //Arreglar cierre parentesis
                Console.Write(">");
                var line = //"log(10,100);";
                Console.ReadLine();
                //"let x = 5;";

                if (string.IsNullOrWhiteSpace(line)) return;

                var evaluate = new Evaluator(line, Variables, DeclarateFunction);

                var ouput = evaluate.Evaluate();

                foreach (var item in evaluate.Variables.Keys)
                {
                    if (Variables.ContainsKey(item))
                        Variables[item] = evaluate.Variables[item];
                    else
                        Variables.Add(item, evaluate.Variables[item]);
                }
                foreach (var item in evaluate.DeclarateFunction.Keys)
                {

                    if (DeclarateFunction.ContainsKey(item))
                        DeclarateFunction[item] = evaluate.DeclarateFunction[item];
                    else
                        DeclarateFunction.Add(item, evaluate.DeclarateFunction[item]);
                }


                for (int i = 0; i < ouput.Length; i++)
                {
                    if (ouput[i] == null)
                        break;
                    if (ouput[i] != (object)"")
                        Console.WriteLine(ouput[i]);

                }

                //PirntExpression(parser.Parse());

            }

        }
        static void PirntExpression(Expression expression)
        {
            if (expression is NumberExpression n)
            {
                Console.WriteLine(n.Kind);
            }
            if (expression is BinaryExpression b)
            {
                Console.WriteLine(b.Kind);
                PirntExpression(b.Left);
                Console.WriteLine(b.Operator.Kind);
                PirntExpression(b.Right);
            }
            if (expression is ParenthesisExpression p)
            {
                Console.WriteLine(p.Kind);
                PirntExpression(p.Expression);
            }
        }
    }
}