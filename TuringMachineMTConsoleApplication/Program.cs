﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuringMachineMT;

namespace TuringMachineMTConsoleApplication
{
    class Program
    {
        #region ScreenVariables
        const int BUFFER_WIDTH = 80;
        const int BUFFER_HEIGHT = 300;
        const int WINDOW_WIDTH = 80;
        const int HEADER_SIZE = 7;
        const int CONTENT_SIZE = 25;
        const int FOOTER_SIZE = 1;
        const int WINDOW_HEIGHT = HEADER_SIZE + CONTENT_SIZE + FOOTER_SIZE + 2;

        static string[] ScreenBuffer;
        static int ContentOffset = 0;
        static string ProgramTitle;
        static string ProgramSubtitle;
        #endregion

        static void Main(string[] args)
        {
            StartScreenHelper();

            StartScreen();
            LoadMenu();
        }

        static void StartScreen()
        {
            ScreenClearContent();
            ScreenWriteContent(2 , CenterStringWithChar(' ', " ._________________. "));
            ScreenWriteContent(3 , CenterStringWithChar(' ', " | _______________ | "));
            ScreenWriteContent(4 , CenterStringWithChar(' ', " | I             I | "));
            ScreenWriteContent(5 , CenterStringWithChar(' ', " | I             I | "));
            ScreenWriteContent(6 , CenterStringWithChar(' ', " | I             I | "));
            ScreenWriteContent(7 , CenterStringWithChar(' ', " | I             I | "));
            ScreenWriteContent(8 , CenterStringWithChar(' ', " | I_____________I | "));
            ScreenWriteContent(9 , CenterStringWithChar(' ', " !_________________! "));
            ScreenWriteContent(10, CenterStringWithChar(' ', "    ._[_______]_.    "));
            ScreenWriteContent(11, CenterStringWithChar(' ', ".___|___________|___."));
            ScreenWriteContent(12, CenterStringWithChar(' ', "|::: ____           |"));
            ScreenWriteContent(13, CenterStringWithChar(' ', "|    ~~~~ [CD-ROM]  |"));
            ScreenWriteContent(14, CenterStringWithChar(' ', "!___________________!"));
            ScreenWriteContent(17, FormatToLeft("v 1.0 "));

            ScreenWriteFooter(" Press ENTER to continue...");
            ScreenFlush();

            Console.ReadLine();
        }
        
        static void LoadMenu()
        {
            bool invalidOption = false;

            while (true)
            {
                ScreenClearContent();
                ScreenWriteContent(0, " *** Menu Options ***                           ");
                ScreenWriteContent(2, "              1. Load machine from file.        ");
                ScreenWriteContent(3, "              2. Create new machine.            ");

                if (invalidOption) ScreenWriteContent(CONTENT_SIZE - 1, " The selected option is not valid... ");
                ScreenWriteFooter(" Enter the selected option (Press 'x' to exit): ");
                ScreenFlush();

                string menuOption = Console.ReadLine();
                if (menuOption.Equals("x")) return;

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
                        invalidOption = true;
                        break;
                }
            }
        }

        static void LoadMachineFromFile()
        {
            bool validOption = true;
            string fileName = string.Empty;

            ScreenClearContent();
            ScreenWriteContent(0, " *** Load Turing machine from file ***");
            ScreenWriteContent(2, " In order to load a machine you must specify a file.");
            ScreenWriteContent(3, " You can exit at any time by pressing 'x'.");
            
            while (true)
            {
                if (!validOption) ScreenWriteContent(CONTENT_SIZE - 1, " The specified file doesn't exist...");
                ScreenWriteFooter(" Enter the file name: ");
                ScreenFlush();

                fileName = Console.ReadLine();
                
                if (fileName.Equals("x")) return;

                if (!System.IO.File.Exists(fileName)) validOption = false;
                else validOption = true;

                if (validOption) break;
            }
            ScreenWriteContent(CONTENT_SIZE - 1, string.Empty);
            ScreenWriteContent(7,CenterStringWithChar(' ', " ... Loading file ... "));
            ScreenFlush();

            FormalDescription TuringMachineDescription = new FormalDescription();
            try 
            {
                TuringMachineDescription.Load(fileName);
            } catch 
            {
                ScreenWriteContent(6, " Error! The file wasn't loaded.");
                ScreenWriteFooter(" Press ENTER to continue");
                Console.ReadLine();
                return;
            }

            ScreenWriteContent(9,CenterStringWithChar(' ', " The file was succesfully loaded!"));
            ScreenWriteContent(11,CenterStringWithChar(' ', " Machine Ready!"));
            ScreenWriteFooter(" Press ENTER to continue");
            ScreenFlush();

            Console.ReadLine();

            TuringMachineMenu(TuringMachineDescription);
        }

        static void TuringMachineMenu(FormalDescription machineDescription)
        {
            string option = string.Empty;
            bool validOption = true;

            while (true)
            {
                ScreenClearContent();
                ScreenWriteContent(0 , " *** Machine Formal Description ***");
                ScreenWriteContent(2 , string.Format(" Machine name:    {0}", machineDescription.GetName()));
                ScreenWriteContent(3 , string.Format(" Description:     {0}", machineDescription.GetDescription()));
                ScreenWriteContent(4 , string.Format(" Number of tapes: {0}", machineDescription.GetNumberOfTapes().ToString()));
                ScreenWriteContent(6 , string.Format(" States set:      {0}", machineDescription.GetMachineStatesAsString()));
                ScreenWriteContent(7 , string.Format(" Input alphabet:  {0}", machineDescription.GetInputAlphabetAsString()));
                ScreenWriteContent(8 , string.Format(" Tape alphabet:   {0}", machineDescription.GetTapeAlphabetAsString()));
                ScreenWriteContent(9 , string.Format(" Blank symbol:    {0}", machineDescription.GetBlankSymbol()));
                ScreenWriteContent(10, string.Format(" Initial state:   {0}", machineDescription.GetInitialState().GetName()));
                ScreenWriteContent(11, string.Format(" Accept state:    {0}", machineDescription.GetAcceptState().GetName()));
                ScreenWriteContent(12, string.Format(" Reject state:    {0}", machineDescription.GetRejectState().GetName()));

                ScreenWriteContent(14, " *** Machine Options *** ");
                ScreenWriteContent(16, " 1. See transition function.");
                ScreenWriteContent(17, " 2. Instant run.");
                ScreenWriteContent(18, " 3. Step by step run.");
                ScreenWriteContent(19, " 4. Edit machine.");

                if (!validOption) ScreenWriteContent(CONTENT_SIZE - 1, " The selected option is not valid... ");
                ScreenWriteFooter(" Enter the selected option (Press 'x' to exit): ");
                ScreenFlush();

                option = Console.ReadLine();
                
                if (option.Equals("x")) return;

                switch (option)
                {
                    case "1":
                        validOption = true;
                        ShowTransitionFunction(machineDescription);
                        break;
                    case "2":
                        validOption = true;
                        InstantRun(machineDescription);
                        break;
                    case "3":
                        validOption = true;
                        break;
                    case "4":
                        validOption = true;
                        break;
                    default:
                        validOption = false;
                        break;
                }
            }
        }

        static void ShowTransitionFunction(FormalDescription machineDescription)
        {
            MachineState[] states = machineDescription.GetMachineStatesSet();
            List<string> transitions = new List<string>();

            foreach (MachineState state in states)
            {
                foreach (string transition in state.GetTransitionsAsString())
                {
                    transitions.Add(transition);
                }
            }

            int pages = transitions.Count / 10;
            if (transitions.Count % 10 != 0 && pages > 0) pages++;

            int currentPage = 0;
            bool validOption = true;
            string option = string.Empty;

            ScreenClearContent();
            ScreenWriteContent(0 , " *** Machine Transition Function ***");
            
            while (true)
            {
                for (int i = 0; i < 10; i++ )
                {
                    if (currentPage * 10 + i >= transitions.Count)
                        break;
                    ScreenWriteContent(i + 2,string.Format(" {0}", transitions[currentPage * 10 + i]));
                }

                if (pages > 0)
                {
                    if (currentPage == 0)
                        ScreenWriteContent(13, " Press 'n' for next page");
                    else if (currentPage == pages)
                        ScreenWriteContent(13, " Press 'p' for prev page");
                    else
                    {
                        ScreenWriteContent(13, " Press 'n' for next page");
                        ScreenWriteContent(13, " Press 'n' for next page");
                    }
                }

                if (!validOption) ScreenWriteContent(CONTENT_SIZE - 1, " The selected option is not valid... ");
                ScreenWriteFooter(" Enter the selected option (Press 'x' to exit): ");
                ScreenFlush();

                option = Console.ReadLine();

                if (option.Equals("x")) return;

                switch (option)
                {
                    case "n":
                        if (pages > 0 && currentPage < pages)
                        {
                            validOption = true;
                            currentPage++;
                        }
                        else
                        {
                            validOption = false;
                        }
                        break;
                    case "p":
                        if (pages > 0 && currentPage > 0)
                        {
                            validOption = true;
                            currentPage--;
                        }
                        else
                        {
                            validOption = false;
                        }
                        break;
                    default:
                        validOption = false;
                        break;
                }
            }
        }

        static void InstantRun(FormalDescription machineDescription)
        {
            string inputString = string.Empty;

            ScreenClearContent();
            ScreenWriteContent(0, " *** Instant Run ***");
            ScreenWriteContent(2, string.Format(" Valid input alphabet: {0}", machineDescription.GetInputAlphabetAsString()));
            ScreenWriteContent(4, " Waiting for input...");
            ScreenWriteFooter(" Enter the input string:");
            ScreenFlush();
            inputString = Console.ReadLine();

            TuringMachine machine;
            try
            {
                machine = new TuringMachine(inputString, WINDOW_WIDTH / 5, machineDescription);
            }
            catch
            {
                ScreenWriteContent(5, " INVALID input detected...");
                ScreenWriteContent(7, " The input symbols does not belong to the input alphabet.");
                ScreenWriteContent(8, " The machine will exit.");
                ScreenWriteFooter(" Press ENTER to continue:");
                ScreenFlush();
                Console.ReadLine();
                return;
            }

            ScreenWriteContent(5, " The input was validated!");
            ScreenWriteContent(7, string.Format(" Running machine for string '{0}'...", inputString));
            ScreenFlush();
            machine.RunAll();
            ScreenWriteContent(9, string.Format(" The input string was {0}!", machine.GetMachineStatus()));
            
            ScreenWriteFooter(" Press ENTER to continue:");
            ScreenFlush();
            Console.ReadLine();
            return;
        }

        #region FormatHelpers

        static void StartScreenHelper()
        {
            Console.SetBufferSize(BUFFER_WIDTH, BUFFER_HEIGHT);
            Console.SetWindowSize(WINDOW_WIDTH, WINDOW_HEIGHT);
            ScreenBuffer = new string[HEADER_SIZE + CONTENT_SIZE + FOOTER_SIZE];

            ProgramTitle = CenterStringWithChar('*', " Computational Theory - Final Project ");
            ProgramSubtitle = CenterStringWithChar(' ', " Multi-Tape Turing Machine ");

            SetHeader();
        }

        static void ScreenClearContent()
        {
            for (int i = HEADER_SIZE; i < (HEADER_SIZE + CONTENT_SIZE + FOOTER_SIZE); i++)
            {
                ScreenBuffer[i] = string.Empty;
            }
        }

        static void ScreenWriteContent(int consoleLine, string text)
        {
            if (consoleLine >= CONTENT_SIZE) 
                throw new IndexOutOfRangeException("Invalid console line.");

            string formattedText = text.Length > WINDOW_WIDTH ? string.Format("{0}... ", text.Substring(0, WINDOW_WIDTH - 5)) : text;
            ScreenBuffer[HEADER_SIZE + consoleLine] = formattedText;
        }

        static void ScreenWriteFooter(string text)
        {
            string formattedText = text.Length > WINDOW_WIDTH ? string.Format("{0}... ", text.Substring(0, WINDOW_WIDTH - 5)) : text;
            ScreenBuffer[HEADER_SIZE + CONTENT_SIZE + ContentOffset++] = formattedText;
            if (ContentOffset >= FOOTER_SIZE) ContentOffset = 0;
        }

        static void ScreenFlush()
        {
            Console.Clear();
            foreach (string line in ScreenBuffer)
                Console.WriteLine(line);
            Console.Write(" > ");
        }

        static void SetHeader()
        {
            ScreenBuffer[0] = string.Empty;
            ScreenBuffer[1] = ProgramTitle;
            ScreenBuffer[2] = ProgramSubtitle;
            ScreenBuffer[3] = string.Empty;
            ScreenBuffer[4] = FormatToLeft("Written by Mauricio Cunille");
            ScreenBuffer[5] = FormatToLeft("           Roberto Torres  ");
            ScreenBuffer[6] = string.Empty;
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
