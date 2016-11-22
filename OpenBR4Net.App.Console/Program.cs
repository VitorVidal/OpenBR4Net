using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBR4Net.App.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            OpenBR4NetWrapper.OpenBR4Net o = null;
            try
            {
                System.Console.WriteLine("OpenBR4Net SDK Sample application");

                if (args.Length > 0)
                {
                    o = new OpenBR4NetWrapper.OpenBR4Net();
                    debug("Initialize OpenBR ...");                    
                    o.initialize(Environment.CurrentDirectory);
                    debug("Ok ...");
                }
                else
                {
                    help();
                    return;
                }

                ConsoleKey option = menu();
                while (option != ConsoleKey.D3)
                {


                    if (option == ConsoleKey.D1)
                    {
                        if (args.Length > 0 && System.IO.File.Exists(args[0]))
                        {
                            debug("Get Template from " + args[0]);
                            OpenBR4NetWrapper.Template tmpl = o.getTemplate(args[0]);
                            debug("Ok ...");
                            if (tmpl != null)
                            {
                                debug(" Template has " + tmpl.bytes.Length.ToString() + " bytes ...");
                                debug(" Features:");
                                debug("   " + tmpl.F1.Name + " (" + tmpl.F1.X.ToString() + "," + tmpl.F1.Y.ToString() + ")");
                                debug("   " + tmpl.F2.Name + " (" + tmpl.F2.X.ToString() + "," + tmpl.F2.Y.ToString() + ")");

                                System.IO.File.WriteAllBytes("template1", tmpl.bytes);
                            }
                            else
                                debug("No template was returned ...");
                        }
                        else
                        {
                            if (args == null)
                                help();
                            else
                                debug("Can't find file '" + args[0] + "'");

                        }
                    }

                    if (option == ConsoleKey.D2)
                    {

                        if (args.Length > 1)
                        {
                            if (!System.IO.File.Exists(args[0]))
                            {
                                debug("File " + args[0] + "doesn't exist!");
                                return;
                            }

                            if (!System.IO.File.Exists(args[1]))
                            {
                                debug("File " + args[1] + "doesn't exist!");
                                return;
                            }

                            debug(args[0] + " (query) x " + args[1] + " (target)");
                            float score = 0f;
                            o.verify(args[0], args[1], ref score);
                            debug("score:" + score.ToString());
                        }
                        else
                            help();
                    }

                    if (args.Length > 2)
                        help();

                    option = menu();
                    
                }

                debug("BYE!");

                if (o != null && o.isInitialized())
                {
                    debug("Finalize OpenBR ...");
                    o.finalize();
                    debug("Ok ...");

                }

            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error:" + ex.ToString());
            }           
        }

        private static void debug(string message)
        {
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine(string.Format("{0:G}", DateTime.Now) + " - " + message);
            System.Console.ResetColor();
        }

        private static ConsoleKey menu()
        {

            
            ConsoleKeyInfo g = new ConsoleKeyInfo();

            while (g.Key != ConsoleKey.D1 && g.Key != ConsoleKey.D2 && g.Key != ConsoleKey.D3)
            {
                System.Console.WriteLine("Please choose a valid option ...");
                System.Console.WriteLine("1-Get Template");
                System.Console.WriteLine("2-Verify");
                System.Console.WriteLine("3-Exit");
                System.Console.WriteLine("");

                g = System.Console.ReadKey();
            }

            return g.Key;

        }

        private static void help()
        {
            debug("OpenBR4Net SDK Sample application");
            debug(" example: OpenBR4net.App.Console <facial jpg file>");
            debug("          Get template from facial jpg file.");
            debug(" example: OpenBR4net.App.Console <facial jpg file> <facial jpg file>");
            debug("          Match");

        }
    }
}
