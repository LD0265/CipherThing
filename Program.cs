using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace CipherThing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
                Error("Not enough args");

            Encoder encoder = new Encoder();
            Decoder decoder = new Decoder();

            bool mode = false;
            bool showKey = false;

            switch (args[0])
            {
                case "-encode":
                case "-e":
                    mode = true;
                    break;

                case "-decode":
                case "-d":
                    mode = false;
                    break;

                case "-help":
                case "-h":
                    Console.WriteLine("Usage:");
                    Console.WriteLine("-encode | -e <>: Sets mode to encode.");
                    Console.WriteLine("-decode | -d <>: Sets mode to decode.");
                    Console.WriteLine("-input | -i <text/filepath>: Input text or file.");
                    Console.WriteLine("-output | -o <directory>: Directory for output.");
                    Console.WriteLine("-key | -k <key>: Key for encoding.");
                    Console.WriteLine("-showkey | -sk <>: Include the key in output file");
                    Environment.Exit(0);

                    break;

                default:
                    Error("First argument must be either '-e', '-d', or '-h'");
                    break;
            }

            for (int i = 1; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-input":
                    case "-i":
                        try
                        {
                            string input = args[i + 1];

                            if (input.EndsWith(".txt"))
                            {
                                try
                                {
                                    input = File.ReadAllText(input);
                                }
                                catch (FileNotFoundException)
                                {
                                    Error($"File {input} not found");
                                }
                                catch (UnauthorizedAccessException)
                                {
                                    Error($"Cannot access file {input}, no permission");
                                }
                                catch (Exception ex)
                                {
                                    Error($"An error occurred: {ex.Message}");
                                }
                            }

                            encoder.SetInput(input);
                            decoder.SetInput(input);
                        }
                        catch (IndexOutOfRangeException) { Error($"No input was found after {args[i]}"); }

                        break;

                    case "-output":
                    case "-o":
                        try
                        {
                            string dir = args[i + 1];

                            if (!Directory.Exists(dir))
                                Error($"No such directory {dir}");

                            encoder.SetOutput(dir);
                            decoder.SetOutput(dir);
                        }
                        catch (IndexOutOfRangeException) { Error($"No input was found after {args[i]}"); }

                        break;

                    case "-key":
                    case "-k":
                        try
                        {
                            encoder.SetKey(args[i + 1]);
                            encoder.SetMap();

                            decoder.SetKey(args[i + 1]);
                        } catch (IndexOutOfRangeException) { Error($"No key was found after {args[i]}"); }
                        
                        break;

                    case "-showkey":
                    case "-sk":
                        showKey = true;
                        break;
                }
            }

            string output = mode ? encoder.GetOutput() : decoder.GetOutput();

            if (string.IsNullOrEmpty(output))
                Error("No output was provided");

            string path = Path.Combine(output, "output.txt");
            string content = mode ? encoder.EncodeText() : decoder.DecodeText();

            if (showKey)
            {
                string k = mode ? encoder.GetKey() : decoder.GetKey();
                File.WriteAllText(path, $"{content}:{k}");
            }
            else
                File.WriteAllText(path, content);
        }

        public static void Error(string msg)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ForegroundColor = oldColor;
            Environment.Exit(1);
        }

        public static void Info(string msg)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(msg);
            Console.ForegroundColor = oldColor;
        }
    }
}