using System;
using System.Collections.Generic;
using NLog;

namespace ComplexNumberCalculator
{
    public interface IComplexNumberOperation
    {
        ComplexNumber Execute(ComplexNumber a, ComplexNumber b);
    }

    public class ComplexNumber
    {
        public double Real { get; set; }
        public double Imaginary { get; set; }

        public ComplexNumber(double real, double imaginary)
        {
            Real = real;
            Imaginary = imaginary;
        }

        public static ComplexNumber operator +(ComplexNumber a, ComplexNumber b)
        {
            return new ComplexNumber(a.Real + b.Real, a.Imaginary + b.Imaginary);
        }

        public static ComplexNumber operator *(ComplexNumber a, ComplexNumber b)
        {
            double real = a.Real * b.Real - a.Imaginary * b.Imaginary;
            double imaginary = a.Real * b.Imaginary + a.Imaginary * b.Real;
            return new ComplexNumber(real, imaginary);
        }

        public static ComplexNumber operator /(ComplexNumber a, ComplexNumber b)
        {
            double denominator = b.Real * b.Real + b.Imaginary * b.Imaginary;
            double real = (a.Real * b.Real + a.Imaginary * b.Imaginary) / denominator;
            double imaginary = (a.Imaginary * b.Real - a.Real * b.Imaginary) / denominator;
            return new ComplexNumber(real, imaginary);
        }

        public override string ToString()
        {
            return $"{Real} + {Imaginary}i";
        }
    }

    public class ComplexNumberCalculator
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private readonly Dictionary<string, IComplexNumberOperation> _operations = new Dictionary<string, IComplexNumberOperation>
        {
            ["+"] = new AdditionOperation(),
            ["*"] = new MultiplicationOperation(),
            ["/"] = new DivisionOperation()
        };

        public ComplexNumber Calculate(string operation, ComplexNumber a, ComplexNumber b)
        {
            if (!_operations.ContainsKey(operation))
            {
                _logger.Error($"Unknown operation: {operation}");
                throw new ArgumentException($"Unknown operation: {operation}");
            }

            _logger.Info($"Performing operation: {operation} on {a} and {b}");

            return _operations[operation].Execute(a, b);
        }
    }

    public class AdditionOperation : IComplexNumberOperation
    {
        public ComplexNumber Execute(ComplexNumber a, ComplexNumber b)
        {
            return a + b;
        }
    }

    public class MultiplicationOperation : IComplexNumberOperation
    {
        public ComplexNumber Execute(ComplexNumber a, ComplexNumber b)
        {
            return a * b;
        }
    }

    public class DivisionOperation : IComplexNumberOperation
    {
        public ComplexNumber Execute(ComplexNumber a, ComplexNumber b)
        {
            return a / b;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var calculator = new ComplexNumberCalculator();

            var a = new ComplexNumber(3, 4);
            var b = new ComplexNumber(5, 6);

            Console.WriteLine($"Addition: {calculator.Calculate("+", a, b)}");
            Console.WriteLine($"Multiplication: {calculator.Calculate("*", a, b)}");
            Console.WriteLine($"Division: {calculator.Calculate("/", a, b)}");
        }
    }
}
