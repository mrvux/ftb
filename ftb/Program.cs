using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ftb
{
    class Program
    {
        public static void PrintUsage()
        {
            Console.WriteLine("Usage: ");
            Console.WriteLine("-i {s}            Input file name");
            Console.WriteLine("-o {s}            Output file name");
            Console.WriteLine("-col {i}          Variable count per line");
            Console.WriteLine("-ns {s}           Namespace");
            Console.WriteLine("-class {s}        Class name (Default to input file name)");
            Console.WriteLine("-field {s}          Variable name (Default to input file name)");
            Console.WriteLine("-classmod {s}     Class modifier (Defaults to public static partial)");
            Console.WriteLine("-fieldmod {s}       Variable modifier (Default to public static readonly)");
        }

        static int Main(string[] args)
        {
            if (args.Length < 2)
            {
                PrintUsage();
                return -1;
            }

            int defaultColCount = 5;
            int colcount = defaultColCount;
            string file_in = "";
            string file_out = "";
            string ns = "";
            string varName = "";
            string className = "";
            string classModifier = "public static partial";
            string varModifier = "public static readonly";

            int i = 0;
            for(i = 0; i < args.Length; i++)
            {
                if (args[i] == "-i" && i < args.Length - 1)
                {
                    file_in = args[i + 1];
                    i++;
                }
                else if (args[i] == "-o" && i < args.Length - 1)
                {
                    file_out = args[i + 1];
                    i++;
                }
                else if (args[i] == "-col" && i < args.Length - 1)
                {
                    bool res = int.TryParse(args[i + 1], out colcount);
                    if (!res)
                    {
                        colcount = defaultColCount;
                    }
                    else
                    {
                        colcount = colcount < 1 ? defaultColCount : colcount;
                    }

                    i++;
                }
                else if (args[i] == "-ns" && i < args.Length - 1)
                {
                    ns = args[i + 1];
                    i++;
                }
                else if (args[i] == "-var" && i < args.Length - 1)
                {
                    varName = args[i + 1];
                    i++;
                }
                else if (args[i] == "-class" && i < args.Length - 1)
                {
                    className = args[i + 1];
                    i++;
                }
                else if (args[i] == "-classmod" && i < args.Length - 1)
                {
                    classModifier = args[i + 1];
                    i++;
                }
                else if (args[i] == "-varmod" && i < args.Length - 1)
                {
                    varModifier = args[i + 1];
                    i++;
                }
            }

            if (string.IsNullOrEmpty(file_out))
            {
                file_out = Path.Combine(Path.GetDirectoryName(file_in), Path.GetFileNameWithoutExtension(file_in) + ".cs");
            }

            if (string.IsNullOrEmpty(className))
            {
                className = Path.GetFileNameWithoutExtension(file_in);
            }

            if (string.IsNullOrEmpty(varName))
            {
                varName = Path.GetFileNameWithoutExtension(file_in);
            }



            if (File.Exists(file_in))
            {
                try
                {
                    byte[] bdata = File.ReadAllBytes(file_in);

                    StringBuilder sb = new StringBuilder();

                    List<string> nsArray = ns.Split('.').Where(s => string.IsNullOrEmpty(s) == false).ToList();

                    sb.AppendLine("using System;");

                    if (!string.IsNullOrWhiteSpace(ns))
                        sb.AppendLine("namespace " + ns + " { ");

                    sb.AppendLine("    " + classModifier + " class " + className + " { ");
                    sb.AppendLine("        " + varModifier + " byte[] " + varName + "  = { ");

                    int bcnt = 0;
                    for (int bid = 0; bid < bdata.Length; bid++)
                    {
                        sb.Append(bdata[bid].ToString());

                        if (bid < bdata.Length - 1)
                            sb.Append(" , ");

                        bcnt++;
                        if (bcnt == colcount)
                        {
                            sb.AppendLine("");
                            bcnt = 0;
                        }
                    }


                    sb.AppendLine("        };");
                    sb.AppendLine("    }");

                    if (!string.IsNullOrWhiteSpace(ns))
                        sb.AppendLine("}");


                    File.WriteAllText(file_out, sb.ToString());

                    return 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return -1;
                }

            }
            else
            {
                Console.WriteLine("Please specify input");
                PrintUsage();
                return -1;
            }
        }
    }
}
