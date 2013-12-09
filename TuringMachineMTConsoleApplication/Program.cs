using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuringMachineMT;

namespace TuringMachineMTConsoleApplication
{
    class Program
    {
        const int BUFFER_WIDTH = 80;
        const int BUFFER_HEIGHT = 300;
        const int WINDOW_WIDTH = 80;
        const int WINDOW_HEIGHT = 27;

        static string ProgramTitle;
        static string ProgramSubtitle;

        static void Main(string[] args)
        {
            StartScreenHelper();

            StartScreen();
            LoadMenu();
        }

        private static void StartScreen()
        {
            PrintHeader();
            Console.WriteLine(CenterStringWithChar(' '," ._________________. "));
            Console.WriteLine(CenterStringWithChar(' '," | _______________ | "));
            Console.WriteLine(CenterStringWithChar(' '," | I             I | "));
            Console.WriteLine(CenterStringWithChar(' '," | I             I | "));
            Console.WriteLine(CenterStringWithChar(' '," | I             I | "));
            Console.WriteLine(CenterStringWithChar(' '," | I             I | "));
            Console.WriteLine(CenterStringWithChar(' '," | I_____________I | "));
            Console.WriteLine(CenterStringWithChar(' '," !_________________! "));
            Console.WriteLine(CenterStringWithChar(' ',"    ._[_______]_.    "));
            Console.WriteLine(CenterStringWithChar(' ',".___|___________|___."));
            Console.WriteLine(CenterStringWithChar(' ',"|::: ____           |"));
            Console.WriteLine(CenterStringWithChar(' ',"|    ~~~~ [CD-ROM]  |"));
            Console.WriteLine(CenterStringWithChar(' ',"!___________________!"));
            Console.WriteLine();
            Console.WriteLine(FormatToLeft("v 1.0 "));
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Press ENTER to continue...");
            Console.ReadLine();
            Console.Clear();
        }
        private static void LoadMenu()
        {
            bool runningProgram = true;
            while (runningProgram)
            {
                PrintHeader();
                Console.WriteLine(" Menu Options                                   ");
                Console.WriteLine("              1. Load machine from file.        ");
                Console.WriteLine("              2. Create new machine.            ");
                Console.WriteLine();
                Console.WriteLine();
                bool invalidOption = true;
                while (invalidOption)
                {
                    Console.Write(" Enter the selected option (Press 'x' to exit): ");
                    string menuOption = Console.ReadLine();

                    if (menuOption.Equals("x"))
                    {
                        runningProgram = false;
                        break;
                    }

                    switch (menuOption)
                    {
                        case "1":
                            invalidOption = false;
                            LoadMachineFromFile();
                            break;
                        case "2":
                            invalidOption = false;
                            break;
                        default:
                            Console.WriteLine(" The selected option is not valid... ");
                            break;
                    }
                }

                Console.Clear();
            }
        }

        private static void LoadMachineFromFile()
        {
            string fileName = string.Empty;
            FormalDescription TuringMachineDescription = new FormalDescription();

            PrintHeader();
            Console.WriteLine(" You selected: Load machine from file...");
            Console.WriteLine();
            Console.WriteLine(" In order to load a machine you must specify a file.");
            Console.WriteLine(" You can exit at any time by pressing 'x'.");
            Console.WriteLine();
            while (true)
            {
                Console.Write(" Please, enter the file name: ");
                fileName = Console.ReadLine();

                if (fileName.Equals("x"))
                {
                    return;
                }

                if (!System.IO.File.Exists(fileName))
                {
                    Console.WriteLine(" The specified file doesn't exist...");
                    Console.WriteLine();
                }
                else
                {
                    break;
                }
            }

            Console.WriteLine();
            Console.Write(" Loading file.......... ");
            try
            {
                TuringMachineDescription.Load(fileName);
            }
            catch
            {
                Console.WriteLine(" Error! The file wasn't loaded.");
                Console.WriteLine();
                Console.WriteLine(" Press ENTER to continue");
                Console.ReadLine();
                return;
            }

            Console.WriteLine(" Ready!");
            Console.WriteLine();
            Console.WriteLine(" Press ENTER to continue");
            Console.ReadLine();

            ExecuteTuringMachine(TuringMachineDescription);
        }

        static void ExecuteTuringMachine(FormalDescription machineDescription)
        {
            bool validOption = true;

            while (true)
            {
                PrintHeader();
                Console.WriteLine(string.Format(" *** Machine Formal Description *** "));
                Console.WriteLine();
                Console.WriteLine(string.Format(" Machine name:    {0}", machineDescription.GetName()));
                Console.WriteLine(string.Format(" Description:     {0}", machineDescription.GetDescription()));
                Console.WriteLine(string.Format(" Number of tapes: {0}", machineDescription.GetNumberOfTapes().ToString()));
                Console.WriteLine();
                Console.WriteLine(string.Format(" States set:      {0}", machineDescription.GetMachineStatesToString()));
                Console.WriteLine(string.Format(" Input alphabet:  {0}", machineDescription.GetInputAlphabetToString()));
                Console.WriteLine(string.Format(" Tape alphabet:   {0}", machineDescription.GetTapeAlphabetToString()));
                Console.WriteLine(string.Format(" Blank symbol:    {0}", machineDescription.GetBlankSymbol()));
                Console.WriteLine(string.Format(" Initial state:   {0}", machineDescription.GetInitialState().GetName()));
                Console.WriteLine(string.Format(" Accept state:    {0}", machineDescription.GetAcceptState().GetName()));
                Console.WriteLine(string.Format(" Reject state:    {0}", machineDescription.GetRejectState().GetName()));
                Console.WriteLine();
                Console.WriteLine("Machine Options:     1. See transition function.");
                Console.WriteLine("                     2. Run machine.");
                Console.WriteLine();
                if (!validOption) Console.WriteLine(" The selected option is not valid... ");
                Console.Write(" Enter the selected option (Press 'x' to exit): ");
                string option = Console.ReadLine();

                if (option.Equals("x"))
                {
                    return;
                }

                switch (option)
                {
                    case "1":
                        validOption = true;
                        break;
                    case "2":
                        validOption = true;
                        break;
                    default:
                        validOption = false;
                        break;
                }
            }
        }

        #region FormatHelpers

        static void StartScreenHelper()
        {
            Console.SetBufferSize(BUFFER_WIDTH, BUFFER_HEIGHT);
            Console.SetWindowSize(WINDOW_WIDTH, WINDOW_HEIGHT);

            ProgramTitle = CenterStringWithChar('*', " Computational Theory - Final Project ");
            ProgramSubtitle = CenterStringWithChar(' ', " Multi-Tape Turing Machine ");
        }

        static void PrintHeader()
        {
            Console.Clear();
            Console.WriteLine(string.Empty);
            Console.WriteLine(ProgramTitle);
            Console.WriteLine(ProgramSubtitle);
            Console.WriteLine(string.Empty);
            Console.WriteLine(FormatToLeft("Written by Mauricio Cunille"));
            Console.WriteLine(FormatToLeft("           Roberto Torres  "));
            Console.WriteLine(string.Empty);
        }

        static string CenterStringWithChar(char fillingChar, string text)
        {
            int fillingCharNumber = ((BUFFER_WIDTH - text.Length) / 2) - 1;
            string fillingString = new string(fillingChar, fillingCharNumber);

            return string.Format(" {0}{1}{0}", fillingString, text);
        }

        static string FormatToLeft(string text)
        {
            int numberOfSpaces = BUFFER_WIDTH - (text.Length + 1);
            string fillingstring = new string(' ', numberOfSpaces);

            return string.Format("{0}{1}", fillingstring, text);
        }
        
        #endregion
    }
}
