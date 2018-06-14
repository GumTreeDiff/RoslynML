using System;
using System.Linq;

namespace CommandLineApp
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                string input = Console.ReadLine();
                if (input == "quit")
                    break;

                string[] command = (input ?? "").Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (command.Length == 0) 
                {
                    Console.WriteLine("Specify a command.");
                }
                else
                {
                    switch (command[0])
                    {
                        case "RoslynML":
                            if (command.Length < 2)
                                Console.WriteLine("RoslynML command takes at least 1 argument.");
                            else
                                HandleRoslynMLCommand(command[1], command.Length > 2 ? command.Skip(2).ToArray() : null);
                            break;
                        default:
                            Console.WriteLine("Unknown command.");
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the RoslynML command.
        /// </summary>
        /// <param name="fullPath">the full path from which loading the content.</param>
        private static void HandleRoslynMLCommand(string fullPath, params string[] options)
        {
            try
            {
                var loader = new RoslynML();
                var xElement = loader.Load(fullPath, true);

                var opts = options ?? new string[0];

                if (opts.Any(o => o == "-gumtreefy"))
                    xElement = loader.Gumtreefy(xElement);

                var saveToFile = opts.SingleOrDefault(o => o.StartsWith("-saveToFile="));
                if (saveToFile != null)
                {
                    var path = saveToFile.Replace("-saveToFile=", "");
                    System.IO.File.WriteAllText(path, xElement.ToString());
                }
                else
                {
                    Console.WriteLine(xElement.ToString());
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
