using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace binary
{
    class Program
    {
        static void Main(string[] args)
        {
            var timer = new Stopwatch();
            timer.Start();

            string outputFolder = @"D:\temp\prime-numbers\binary\";
            string[] dataLocations = { 
                @"..\..\data\primes-to-100k.txt", 
                @"..\..\data\primes-to-200k.txt", 
                @"..\..\data\primes-to-300k.txt", 
                @"..\..\data\primes-to-400k.txt", 
                @"..\..\data\primes-to-500k.txt", 
                @"..\..\data\primes-to-600k.txt", 
                @"..\..\data\primes-to-700k.txt", 
                @"..\..\data\primes-to-800k.txt", 
                @"..\..\data\primes-to-900k.txt", 
                @"..\..\data\primes-to-1000k.txt",
            };

            Console.WriteLine($"------------------------------");
            Console.WriteLine($"Parameters");
            Console.WriteLine($"    outputFolder: {outputFolder}");
            Console.WriteLine($"");

            if (!Directory.Exists(outputFolder))
            {
                Console.WriteLine($"Directory does not exist.  Creating {outputFolder}");
                Directory.CreateDirectory(outputFolder);
            }

            for (var ii = 0; ii < dataLocations.Length; ii++)
            {
                var outputFile = $"D:\\temp\\prime-numbers\\binary\\{Path.GetFileName(dataLocations[ii])}";
                 
                var data = LoadData(dataLocations[ii]);
                var binaryString = ConvertToBinaryString(data);
                
                SaveData(outputFile, binaryString);
            }

            timer.Stop();

            Console.WriteLine($"");
            Console.WriteLine($"Total runtime: {timer.Elapsed.ToString("mm':'ss'.'fff")}");
            Console.WriteLine($"------------------------------");
        }
        
        static int[] LoadData(string fileName)
        {
            var data = new List<int>();
            
            Console.WriteLine($"Loading data from {fileName}");
            var strings = File.ReadAllLines(fileName);
            data.AddRange(strings.Select(x => Int32.Parse(x)).ToList());

            return data.ToArray();
        }

        static string[] ConvertToBinaryString(int[] data)
        {
            return data.Select(x => Convert.ToString(x, 2)).ToArray();
        }

        static void SaveData(string fileName, string[] data)
        {
            Console.WriteLine($"Saving binary strings to {fileName}");            
            File.WriteAllLines(fileName, data);
        }
    }
}
