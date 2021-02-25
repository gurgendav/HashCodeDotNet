using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HashCode
{
    public class Result
    {
        public List<string> Pizza { get; set; }
    }

    public class Input
    {
        public List<List<string>> Lines { get; set; }
    }
    
    
    public class Program
    {




        static Result Solve(Input input)
        {
            var result = new Result
            {
                Pizza = new List<string> {"A", "B", "C"}
            };

            return result;
        }

        static int Validate(Input input, Result result)
        {
            var score = 0;
            
            if (false)
            {
                throw new Exception("Invalid something");
            }

            score += 90;

            return score;
        }

        static void Execute(string fileName)
        {
            Console.WriteLine($"Processing: {fileName}");

            var input = ReadFile(fileName);

            var result = Solve(input);
            
            var score = Validate(input, result);
            Console.WriteLine($"Processing finished, score: {score}");
            
            WriteResult(fileName, result);
        }

        static void Main(string[] args)
        {
            int? fileNumber = null;

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (fileNumber != null)
            {
                Execute(Files[fileNumber.Value]);
            }
            else
            {
                foreach (var file in Files)
                {
                    Execute(file);
                }
            }
        }
        
        private static void WriteResult(string fileName, Result result)
        {
            var filePath = $"out/{fileName}.out";

            var resultText = string.Join(" ", result.Pizza);
            
            File.WriteAllText(filePath, resultText);
        }

        private static Input ReadFile(string fileName)
        {
            var filePath = $"in/{fileName}.in";

            var input = new Input {Lines = new List<List<string>>()};

            foreach (var line in File.ReadLines(filePath))
            {
                var parts = line.Split(" ");
                
                input.Lines.Add(parts.ToList());
            }

            return input;
        }
        
        private static readonly List<string> Files = new()
        {
            "a_example",
            "b_little_bit_of_everything",
            "c_many_ingredients"
        };
    }
}
