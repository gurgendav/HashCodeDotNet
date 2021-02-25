using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HashCode
{
    public class Result
    {
        
    }

    public class Street
    {
        public int IntersectionStart { get; set; }
        public int IntersectionEnd { get; set; }
        public string Name { get; set; }
        public int Time { get; set; }

        public int TotalCarCount { get; set; } = 0;
    }

    public class Schedule
    {
        public Street Street { get; set; }
        public int Duration { get; set; }
    }

    public class Intersection
    {
        public int Id { get; set; }
        
        public List<Street> In { get; set; }
        public List<Street> Out { get; set; }

        public List<Schedule> Schedule { get; set; } = new();
    }

    public class Path
    {
        public List<Street> Streets { get; set; }
        
        public HashSet<string> StreetsSet { get; set; }
    }

    public class Input
    {
        public int Duration { get; set; }
        public int IntersectionCount { get; set; }
        public int StreetCount { get; set; }
        public int CarCount { get; set; }
        public int BonusPoints { get; set; }
        public Dictionary<string, Street> Streets { get; set; }
        
        public List<Path> Paths { get; set; }

        public Dictionary<int, Intersection> Intersections { get; set; } = new();
    }
    
    
    public class Program
    {
        private static Random _random = new Random();

        static Input Solve(Input input)
        {
            foreach (var intersection in input.Intersections.Values)
            {
                if (intersection.In.Count == 1)
                {
                    intersection.Schedule.Add(new Schedule
                    {
                        Duration = 1,
                        Street = intersection.In.First()
                    });
                    continue;
                }

                var totalCarCount = intersection.In.Sum(i => i.TotalCarCount);
                if (totalCarCount == 0)
                {
                    continue;
                }
                
                foreach (var street in intersection.In)
                {
                    var portion = (double)street.TotalCarCount / totalCarCount;

                    if (portion < 0.1)
                    {
                        continue;
                    }

                    var time = (int)Math.Round(portion * 100);
                    
                    intersection.Schedule.Add(new Schedule
                    {
                        Duration = time,
                        Street = street
                    });
                }
            }
            
            return input;
        }

        static int Validate(Input input)
        {
            
            
            for (int i = 0; i < input.Duration; i++)
            {
                
            }

            return 0;
        }

        static void Execute(string fileName)
        {
            Console.WriteLine($"Processing: {fileName}");

            var input = ReadFile(fileName);

            var result = Solve(input);
            
            //var score = Validate(input, result);
            //Console.WriteLine($"Processing finished, score: {score}");
            
            WriteResult(fileName, result);
        }

        static void Main(string[] args)
        {
            int? fileNumber = null;
            
            if (args.Length == 1)
            {
                if (int.TryParse(args[0], out var result))
                {
                    fileNumber = result;
                }
                else
                {
                    Console.Error.WriteLine("Argument should be a number");
                    return;
                }
            } 
            else if (args.Length != 0)
            {
                Console.Error.WriteLine("Only one argument is accepted, index of file");
                return;
            }

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
        
        private static void WriteResult(string fileName, Input input)
        {
            var filePath = $"out/{fileName}.out";

            StringBuilder stringBuilder = new();

            var intersectionsWithSchedule = input.Intersections.Values.Where(i => i.Schedule.Any()).ToList();
            stringBuilder.AppendLine(intersectionsWithSchedule.Count.ToString());
            
            foreach (var intersection in intersectionsWithSchedule)
            {
                stringBuilder.AppendLine(intersection.Id.ToString());
                stringBuilder.AppendLine(intersection.Schedule.Count.ToString());

                foreach (var schedule in intersection.Schedule)
                {
                    stringBuilder.AppendLine($"{schedule.Street.Name} {schedule.Duration}");
                }
            }
            
            File.WriteAllText(filePath, stringBuilder.ToString());
        }

        private static Input ReadFile(string fileName)
        {
            var filePath = $"in/{fileName}.txt";

            var lines = File.ReadAllLines(filePath);

            var initialNumbers = lines[0].Split(" ");

            var input = new Input
            {
                Duration = int.Parse(initialNumbers[0]),
                IntersectionCount = int.Parse(initialNumbers[1]),
                StreetCount = int.Parse(initialNumbers[2]),
                CarCount = int.Parse(initialNumbers[3]),
                BonusPoints = int.Parse(initialNumbers[4])
            };

            var streets = new Dictionary<string, Street>();
            
            for (int i = 0; i < input.StreetCount; i++)
            {
                var line = lines[1 + i];
                var parts = line.Split(" ");

                var street = new Street
                {
                    IntersectionStart = int.Parse(parts[0]),
                    IntersectionEnd = int.Parse(parts[1]),
                    Name = parts[2],
                    Time = int.Parse(parts[3])
                };

                if (!input.Intersections.TryGetValue(street.IntersectionEnd, out var intersection))
                {
                    intersection = new Intersection
                    {
                        Id = street.IntersectionEnd,
                        In = new List<Street>(),
                        Out = new List<Street>()
                    };

                    input.Intersections[street.IntersectionEnd] = intersection;
                }
                intersection.In.Add(street);
                
                if (!input.Intersections.TryGetValue(street.IntersectionStart, out var intersection2))
                {
                    intersection2 = new Intersection
                    {
                        Id = street.IntersectionStart,
                        In = new List<Street>(),
                        Out = new List<Street>()
                    };

                    input.Intersections[street.IntersectionStart] = intersection2;
                }
                intersection.Out.Add(street);

                streets[street.Name] = street;
            }

            input.Streets = streets;

            var paths = new List<Path>();
            
            for (int i = 0; i < input.CarCount; i++)
            {
                var line = lines[1 + input.StreetCount + i];
                var parts = line.Split(" ");

                var carPaths = parts[1..].Select(s => input.Streets[s]).ToList();

                var path = new Path
                {
                    Streets = carPaths,
                    StreetsSet = new HashSet<string>(parts[1..])
                };

                foreach (var street in carPaths)
                {
                    street.TotalCarCount++;
                }
                
                paths.Add(path);
            }

            input.Paths = paths;
            
            return input;
        }
        
        private static readonly List<string> Files = new()
        {
            "a",
            "b",
            "c",
            "d",
            "e",
            "f"
        };
    }
}
