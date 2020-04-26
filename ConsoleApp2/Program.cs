
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp2
{
    public class MathEvaluator
    {
        public static void Main()
        {
            Calculator calc = new Calculator();
            Console.WriteLine(calc.Calculate("0 + (10- 2) * 3"));
            Console.ReadLine();
        }
        public class Calculator
        {
            public double Calculate(string expression)
            {
                string polishExpression =  GetPolishNotation(expression);
                string[] expressionToCalculate = polishExpression.Split(" ",StringSplitOptions.RemoveEmptyEntries);
                return CalculatePolishExpression(expressionToCalculate);
            }
            /// <summary>
            /// Вычисляет выражение в обратной польской нотации
            /// </summary>
            /// <param name="expressionToCalculate"></param>
            /// <returns></returns>
            private double CalculatePolishExpression(string[] expressionToCalculate)
            {
                Stack<double> digitsStack = new Stack<double>();
                foreach(string i in expressionToCalculate)
                {
                    if((double.TryParse(i, out double digit)))
                    {
                        digitsStack.Push(digit);
                    }
                    else
                    {
                        switch(i)
                        {
                            case "+":
                                digitsStack.Push(digitsStack.Pop() + digitsStack.Pop());
                                break;
                            case "-":
                                digitsStack.Push(digitsStack.Pop() - digitsStack.Pop());
                                break;
                            case "*":
                                digitsStack.Push(digitsStack.Pop() * digitsStack.Pop());
                                break;
                            case "/":
                                digitsStack.Push(digitsStack.Pop() * digitsStack.Pop());
                                break;
                            case "^":
                                digitsStack.Push(Math.Pow(digitsStack.Pop(),digitsStack.Pop()));
                                break;
                        }
                    }
                }
                return digitsStack.Pop();
            }

            /// <summary>
            /// Приводит выражение к обратной польской нотации
            /// </summary>
            /// <param name="expression"></param>
            /// <returns></returns>
            private string GetPolishNotation(string expression)
            {
                Stack<string> _expressionStack = new Stack<string>();
                string ouptutString = "";

                expression = expression.Replace(" ", "").Replace(",", ".");
                foreach (char i in expression.ToCharArray())
                {
                    if (char.IsDigit(i) || i == '.')
                    {
                        ouptutString += i;
                    }
                    else
                    {
                        ouptutString += " ";
                        byte a = GetPriority(i);
                        if (a == 0)
                        {
                            continue;
                        }
                        if ((_expressionStack.Count == 0) || (GetPriority(_expressionStack.Peek().ToCharArray()[0]) < a)) // Если стек все еще пуст, или находящиеся в нем символы (а находится в нем могут только знаки операций и открывающая скобка) имеют меньший приоритет, чем приоритет текущего символа, то помещаем текущий символ в стек.
                        {
                            _expressionStack.Push(i.ToString());
                        }
                        else if (i == '(')
                        {
                            _expressionStack.Push(i.ToString());
                        }
                        else
                        {
                            while (a >= GetPriority(_expressionStack.Peek().ToCharArray()[0])) //Если символ, находящийся на вершине стека имеет приоритет, больший или равный приоритету текущего символа, то извлекаем символы из стека в выходную строку до тех пор, пока выполняется это условие;).
                            {
                                ouptutString += " " + _expressionStack.Pop();
                            }
                            _expressionStack.Push(i.ToString()); //  затем переходим к пункту а 
                        }

                    }
                }
                foreach (string i in _expressionStack)
                {
                    ouptutString += " " + i ;
                }
                return ouptutString;

                byte GetPriority(char oper)
                {
                    switch (oper)
                    {
                        case ('^'):
                            return 4;
                        case ('*'):
                        case ('/'):
                            return 3;
                        case ('+'):
                        case ('-'):
                            return 2;
                        case ('('):
                            return 1;
                        case (')'):
                            CloseBracketProcess();
                            return 0;
                        default:
                            throw new InvalidOperationException("Не определен приоритет оператора");
                    }
                }
                void CloseBracketProcess()
                {
                    while (_expressionStack.Peek() != "(")
                    {
                        ouptutString += " " + _expressionStack.Pop();
                    }
                    _expressionStack.Pop();
                }
            }
            

        }
    }
}