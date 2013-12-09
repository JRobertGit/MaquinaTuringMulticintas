//-----------------------------------------------------------------------
// <copyright file="FormalDescription.cs" company="M.Cunillé">
//     Copyright (c) M.Cunillé. All rights reserved.
// </copyright>
// <author>Mauricio Cunillé Blando</author>
//-----------------------------------------------------------------------
namespace TuringMachineMT
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// This class represents a multi-tape Turing machine formal description.
    /// </summary>
    public class FormalDescription : IXmlSerializable
    {
        #region Attributes
        /// <summary>
        /// The name of the Turing machine. Must be unique in order to save it.
        /// </summary>
        private string name;

        /// <summary>
        /// A brief description of what the Turing machine does.
        /// </summary>
        private string description;

        /// <summary>
        /// The number of tapes that the Turing machine will have.
        /// </summary>
        private int numberOfTapes;

        /// <summary>
        /// The set of states in the Turing machine.
        /// </summary>
        private HashSet<MachineState> machineStates;

        /// <summary>
        /// The input alphabet of the Turing machine.
        /// </summary>
        private HashSet<char> inputAlphabet;

        /// <summary>
        /// The tape alphabet minus the input alphabet of the Turing machine.
        /// </summary>
        private HashSet<char> tapeAlphabet;

        /// <summary>
        /// The initial state.
        /// </summary>
        private MachineState initialState;

        /// <summary>
        /// The accept state.
        /// </summary>
        private MachineState acceptState;

        /// <summary>
        /// The reject state.
        /// </summary>
        private MachineState rejectState;

        /// <summary>
        /// The blank symbol in the Turing machine.
        /// </summary>
        private char blankSymbol;

        /// <summary>
        /// If the tapes are left-bounded.
        /// </summary>
        private bool leftBounded;

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FormalDescription"/> class.
        /// </summary>
        public FormalDescription()
        {
            this.name = string.Empty;
            this.description = string.Empty;
            this.numberOfTapes = 0;
            this.machineStates = new HashSet<MachineState>();
            this.inputAlphabet = new HashSet<char>();
            this.tapeAlphabet = new HashSet<char>();
            this.initialState = null;
            this.acceptState = null;
            this.rejectState = null;
            this.leftBounded = true;

            this.blankSymbol = 'B';
            this.tapeAlphabet.Add(this.blankSymbol);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormalDescription"/> class.
        /// </summary>
        /// <param name="name">The name of the Turing machine.</param>
        /// <param name="description">A brief description of what the Turing machine does.</param>
        /// <param name="numberOfTapes">The number of tapes that the Turing machine will have.</param>
        /// <param name="tapeAlphabet">The tape alphabet minus the input alphabet.</param>
        /// <param name="inputAlphabet">The input alphabet.</param>
        /// <param name="blankSymbol">The blank symbol. Must be contained in the tape alphabet.</param>
        /// <param name="leftBounded">If the tapes are left-bounded.</param>
        public FormalDescription(
            string name, 
            string description, 
            int numberOfTapes,
            HashSet<char> tapeAlphabet,
            HashSet<char> inputAlphabet,
            char blankSymbol,
            bool leftBounded) : this()
        {
            this.SetName(name);
            this.SetDescription(description);
            this.SetNumberOfTapes(numberOfTapes);
            this.SetInputAlphabet(inputAlphabet);
            this.SetTapeAlphabet(tapeAlphabet);
            this.SetBlankSymbol(blankSymbol);
            this.leftBounded = leftBounded;
        }

        #endregion

        #region Setters

        /// <summary>
        /// Sets the name of the Turing machine.
        /// </summary>
        /// <param name="name">The name of the Turing machine.</param>
        public void SetName(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Sets the description of the Turing machine.
        /// </summary>
        /// <param name="description">A brief description.</param>
        public void SetDescription(string description)
        {
            this.description = description;
        }

        /// <summary>
        /// Sets the number of tapes that the Turing machine will have.
        /// </summary>
        /// <param name="numberOfTapes">The number of tapes.</param>
        public void SetNumberOfTapes(int numberOfTapes)
        {
            this.numberOfTapes = (numberOfTapes > 0) ? numberOfTapes : 0;
        }

        /// <summary>
        /// Sets the input alphabet.
        /// </summary>
        /// <param name="inputAlphabet">The input alphabet.</param>
        public void SetInputAlphabet(HashSet<char> inputAlphabet)
        {
            foreach (char symbol in inputAlphabet)
            {
                if (this.tapeAlphabet.Contains(symbol))
                {
                    throw new Exception("The input alphabet cannot be set since some of its elements are contained in the current tape alphabet.");
                }
            }

            this.inputAlphabet = inputAlphabet;
        }

        /// <summary>
        /// Sets the tape alphabet.
        /// </summary>
        /// <param name="tapeAlphabet">The tape alphabet.</param>
        public void SetTapeAlphabet(HashSet<char> tapeAlphabet)
        {
            foreach (char symbol in tapeAlphabet)
            {
                if (this.inputAlphabet.Contains(symbol))
                {
                    throw new Exception("The tape alphabet cannot be set since some of its elements are contained in the current input alphabet.");
                }
            }

            this.tapeAlphabet = tapeAlphabet;
        }

        /// <summary>
        /// Sets the blank symbol to be used in the Turing machine.
        /// </summary>
        /// <param name="symbol">The blank symbol.</param>
        public void SetBlankSymbol(char symbol)
        {
            if (!this.tapeAlphabet.Contains(symbol))
            {
                throw new Exception("The blank symbol is not contained in the tape alphabet.");
            }

            this.blankSymbol = symbol;
        }

        /// <summary>
        /// Sets the initial state of the Turing Machine
        /// </summary>
        /// <param name="initialState">The initial state.</param>
        public void SetInitialState(MachineState initialState)
        {
            if (!this.machineStates.Contains(initialState))
            {
                throw new Exception("The given state does not belong to the set of states.");
            }

            this.initialState = initialState;
        }

        /// <summary>
        /// Sets the accept state of the Turing Machine.
        /// </summary>
        /// <param name="acceptState">The accept state.</param>
        public void SetAcceptState(MachineState acceptState)
        {
            if (!this.machineStates.Contains(acceptState))
            {
                throw new Exception("The given state does not belong to the set of states.");
            }

            if (this.rejectState != null && this.rejectState.Equals(acceptState))
            {
                throw new Exception("The reject state and accept state must be different.");
            }

            this.acceptState = acceptState;
        }

        /// <summary>
        /// Sets the reject state of the Turing Machine.
        /// </summary>
        /// <param name="rejectState">The reject state.</param>
        public void SetRejectState(MachineState rejectState)
        {
            if (!this.machineStates.Contains(rejectState))
            {
                throw new Exception("The given state does not belong to the set of states.");
            }

            if (this.acceptState != null && this.acceptState.Equals(rejectState))
            {
                throw new Exception("The reject state and accept state must be different.");
            }

            this.rejectState = rejectState;
        }

        /// <summary>
        /// Sets if the tapes are left-bounded.
        /// </summary>
        /// <param name="leftBounded">If the tapes are left-bounded.</param>
        public void SetLeftBounded(bool leftBounded)
        {
            this.leftBounded = leftBounded;
        }

        #endregion

        #region Getters

        /// <summary>
        /// Gets the name of the machine.
        /// </summary>
        /// <returns>The name of the machine.</returns>
        public string GetName()
        {
            return this.name;
        }

        /// <summary>
        /// Gets the machine description.
        /// </summary>
        /// <returns>The machine description.</returns>
        public string GetDescription()
        {
            return this.description;
        }

        /// <summary>
        /// Gets the number of tapes.
        /// </summary>
        /// <returns>The number of tapes.</returns>
        public int GetNumberOfTapes()
        {
            return this.numberOfTapes;
        }

        /// <summary>
        /// Gets the input alphabet.
        /// </summary>
        /// <returns>The input alphabet.</returns>
        public HashSet<char> GetInputAlphabet()
        {
            return this.inputAlphabet;
        }

        /// <summary>
        /// Gets the tape alphabet.
        /// </summary>
        /// <returns>The tape alphabet.</returns>
        public HashSet<char> GetTapeAlphabet()
        {
            return this.tapeAlphabet;
        }

        /// <summary>
        /// Gets the initial state.
        /// </summary>
        /// <returns>The initial state.</returns>
        public MachineState GetInitialState()
        {
            return this.initialState;
        }

        /// <summary>
        /// Gets the accept state.
        /// </summary>
        /// <returns>The accept state.</returns>
        public MachineState GetAcceptState()
        {
            return this.acceptState;
        }

        /// <summary>
        /// Gets the reject state.
        /// </summary>
        /// <returns>The reject state.</returns>
        public MachineState GetRejectState()
        {
            return this.rejectState;
        }

        /// <summary>
        /// Gets the blank symbol.
        /// </summary>
        /// <returns>The blank symbol.</returns>
        public char GetBlankSymbol()
        {
            return this.blankSymbol;
        }

        /// <summary>
        /// Gets a machine state from the machine states set.
        /// </summary>
        /// <param name="name">The name of the state.</param>
        /// <returns>The machine state.</returns>
        public MachineState GetMachineState(string name)
        {
            foreach (MachineState state in this.machineStates)
            {
                if (state.GetName().Equals(name))
                {
                    return state;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets if the tapes are left-bounded.
        /// </summary>
        /// <returns>If the tapes are left-bounded.</returns>
        public bool IsLeftBounded()
        {
            return this.leftBounded;
        }

        /// <summary>
        /// Gets the machine states set.
        /// </summary>
        /// <returns>The machine states.</returns>
        public MachineState[] GetMachineStatesSet()
        {
            return this.machineStates.ToArray<MachineState>();
        }

        /// <summary>
        /// Gets the machine states in a string of type {q0, q1,..., qn}.
        /// </summary>
        /// <returns>The machine states.</returns>
        public string GetMachineStatesAsString()
        {
            string machineStatesStr = string.Empty;
            int machineStatesCounter = 0;

            foreach (MachineState state in this.machineStates)
            {
                if (machineStatesCounter != this.machineStates.Count - 1)
                {
                    machineStatesStr = string.Format("{0}{1}, ", machineStatesStr, state.GetName());
                    machineStatesCounter++;
                } 
                else
                {
                    machineStatesStr = string.Format("{0}{1}", machineStatesStr, state.GetName());
                }
            }

            return "{" + machineStatesStr + "}";
        }

        /// <summary>
        /// Gets the input alphabet in a string of type {s1, s2,..., sn}.
        /// </summary>
        /// <returns>The input alphabet.</returns>
        public string GetInputAlphabetAsString()
        {
            string inputAlphabetStr = string.Empty;
            int inputAlphabetCounter = 0;

            foreach (char symbol in this.inputAlphabet)
            {
                if (inputAlphabetCounter != this.inputAlphabet.Count - 1)
                {
                    inputAlphabetStr = string.Format("{0}{1}, ", inputAlphabetStr, symbol);
                    inputAlphabetCounter++;
                }
                else
                {
                    inputAlphabetStr = string.Format("{0}{1}", inputAlphabetStr, symbol);
                }
            }

            return "{" + inputAlphabetStr + "}";
        }

        /// <summary>
        /// Gets the tape alphabet in a string of type {s1, s2,..., sn}.
        /// </summary>
        /// <returns>The tape alphabet.</returns>
        public string GetTapeAlphabetAsString()
        {
            string tapeAlphabetStr = string.Empty;
            int tapeAlphabetCounter = 0;

            foreach (char symbol in this.tapeAlphabet)
            {
                if (tapeAlphabetCounter != this.tapeAlphabet.Count - 1)
                {
                    tapeAlphabetStr = string.Format("{0}{1}, ", tapeAlphabetStr, symbol);
                    tapeAlphabetCounter++;
                }
                else
                {
                    tapeAlphabetStr = string.Format("{0}{1}", tapeAlphabetStr, symbol);
                }
            }

            return "{" + tapeAlphabetStr + "}";
        }

        #endregion

        #region AddMethods

        /// <summary>
        /// Adds a new state to the Turing machine.
        /// </summary>
        public void AddMachineState()
        {
            this.machineStates.Add(new MachineState());
        }

        /// <summary>
        /// Adds a new state to the Turing machine.
        /// </summary>
        /// <param name="name">The name of the state.</param>
        public void AddMachineState(string name)
        {
            this.machineStates.Add(new MachineState(name));
        }

        /// <summary>
        /// Adds a new state to the Turing machine.
        /// </summary>
        /// <param name="name">The name of the state.</param>
        /// <param name="transitions">The list of transitions for that state.</param>
        public void AddMachineState(string name, HashSet<Transition> transitions)
        {
            this.machineStates.Add(new MachineState(name, transitions));
        }

        /// <summary>
        /// Adds a symbol to the input alphabet.
        /// </summary>
        /// <param name="symbol">The symbol to be added.</param>
        public void AddSymbolToInputAlphabet(char symbol)
        {
            if (this.tapeAlphabet.Contains(symbol))
            {
                throw new Exception("The symbol cannot be added to the input alphabet since it is being used in the tape alphabet.");
            }

            this.inputAlphabet.Add(symbol);
        }

        /// <summary>
        /// Adds a symbol to the tape alphabet.
        /// </summary>
        /// <param name="symbol">The symbol to be added.</param>
        public void AddSymbolToTapeAlphabet(char symbol)
        {
            if (this.inputAlphabet.Contains(symbol))
            {
                throw new Exception("The symbol cannot be added to the tape alphabet since it is being used in the input alphabet.");
            }
            
            this.tapeAlphabet.Add(symbol);
        }

        /// <summary>
        /// Creates a new transition for the Turing machine.
        /// </summary>
        /// <param name="inputState">The input state of the transition.</param>
        /// <param name="inputSymbols">The symbols that will be read in the k-tapes to use this transition.</param>
        /// <param name="outputState">The next state after the transition.</param>
        /// <param name="outputSymbols">The symbols that will be written in the k-tapes.</param>
        /// <param name="headsDirections">The direction of the k-heads movement.</param>
        public void AddTransition(MachineState inputState, char[] inputSymbols, MachineState outputState, char[] outputSymbols, Tape.Direction[] headsDirections)
        {
            if (!this.machineStates.Contains(inputState))
            {
                throw new Exception("The input state does not belong to the set of states.");
            }

            if (inputSymbols.Length != this.numberOfTapes || outputSymbols.Length != this.numberOfTapes)
            {
                throw new Exception("The length of the input/outputSymbols arrays must be equal to the number of tapes in the Turing machine.");
            }

            foreach (char symbol in inputSymbols)
            {
                if (!this.inputAlphabet.Contains(symbol) && !this.tapeAlphabet.Contains(symbol))
                {
                    throw new Exception("Not all the input symbols are being recognized by the specified alphabet.");
                }
            }

            if (!this.machineStates.Contains(outputState))
            {
                throw new Exception("The output state does not belong to the set of states.");
            }

            foreach (char symbol in outputSymbols)
            {
                if (!this.inputAlphabet.Contains(symbol) && !this.tapeAlphabet.Contains(symbol))
                {
                    throw new Exception("Not all the output symbols are being recognized by the specified alphabet.");
                }
            }

            List<TapeInstruction> tapeInstructions = new List<TapeInstruction>();

            for (int i = 0; i < this.numberOfTapes; i++)
            {
                tapeInstructions.Add(new TapeInstruction(i, outputSymbols[i], headsDirections[i]));
            }

            inputState.AddTransition(new Transition(outputState, inputSymbols, tapeInstructions));
        }

        #endregion

        #region VerificationMethods

        /// <summary>
        /// Verifies that the specified formal description for the Turing machine is valid.
        /// </summary>
        /// <returns>If the formal description is valid.</returns>
        public bool VerifyDescription()
        {
            if (string.IsNullOrWhiteSpace(this.name) ||
                this.numberOfTapes < 1 ||
                this.machineStates.Count < 1 ||
                this.inputAlphabet.Count < 1 ||
                this.tapeAlphabet.Count < 1 ||
                this.initialState == null ||
                this.acceptState == null ||
                this.rejectState == null)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region XmlSerialization

        /// <summary>
        /// Gets the schema.
        /// </summary>
        /// <returns>Xml schema.</returns>
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Reads a XML serialization
        /// </summary>
        /// <param name="reader">The xml reader.</param>
        public void ReadXml(System.Xml.XmlReader reader)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the XML serialization.
        /// </summary>
        /// <param name="writer">The xml writer.</param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("Name", this.name);
            writer.WriteAttributeString("Description", this.description);
            writer.WriteAttributeString("NumberOfTapes", this.numberOfTapes.ToString());
            writer.WriteAttributeString("BlankSymbol", this.blankSymbol.ToString());
            writer.WriteAttributeString("LeftBounded", this.leftBounded.ToString());

            writer.WriteStartElement("InputAlphabet");
            foreach (char symbol in this.inputAlphabet)
            {
                writer.WriteElementString("Symbol", symbol.ToString());
            }

            writer.WriteEndElement();

            writer.WriteStartElement("TapeAlphabet");
            foreach (char symbol in this.tapeAlphabet)
            {
                writer.WriteElementString("Symbol", symbol.ToString());
            }

            writer.WriteEndElement();

            writer.WriteStartElement("StatesSet");
            foreach (MachineState state in this.machineStates)
            {
                writer.WriteStartElement("MachineState");
                state.WriteXml(writer);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();

            writer.WriteStartElement("AcceptState");
            writer.WriteAttributeString("StateName", this.acceptState.GetName());
            writer.WriteEndElement();

            writer.WriteStartElement("RejectState");
            writer.WriteAttributeString("StateName", this.rejectState.GetName());
            writer.WriteEndElement();

            writer.WriteStartElement("InitialState");
            writer.WriteAttributeString("StateName", this.initialState.GetName());
            writer.WriteEndElement();
        }

        #endregion

        #region LoadSaveMethods 

        /// <summary>
        /// Saves to an xml file
        /// </summary>
        /// <param name="fileName">File path of the new xml file</param>
        public void Save(string fileName)
        {
            using (var writer = new System.IO.StreamWriter(fileName))
            {
                var serializer = new XmlSerializer(this.GetType());
                serializer.Serialize(writer, this);
                writer.Flush();
            }
        }

        /// <summary>
        /// Loads a formal description from an xml file;
        /// </summary>
        /// <param name="fileName">File path of the xml file to load.</param>
        public void Load(string fileName)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);

                XmlNodeList xmlFormalDescription = doc.GetElementsByTagName("FormalDescription");
                XmlAttributeCollection xmlFormalDescriptionAttributes = xmlFormalDescription[0].Attributes;

                this.SetName(xmlFormalDescriptionAttributes["Name"].Value);
                this.SetDescription(xmlFormalDescriptionAttributes["Description"].Value);
                this.SetNumberOfTapes(int.Parse(xmlFormalDescriptionAttributes["NumberOfTapes"].Value));
                this.SetLeftBounded(bool.Parse(xmlFormalDescriptionAttributes["LeftBounded"].Value));

                XmlNodeList xmlInputAlphabetSymbols = doc.GetElementsByTagName("InputAlphabet")[0].ChildNodes;
                foreach (XmlNode xmlSymbol in xmlInputAlphabetSymbols)
                {
                    this.AddSymbolToInputAlphabet(xmlSymbol.InnerText[0]);
                }

                XmlNodeList xmlTapeAlphabetSymbols = doc.GetElementsByTagName("TapeAlphabet")[0].ChildNodes;
                foreach (XmlNode xmlSymbol in xmlTapeAlphabetSymbols)
                {
                    this.AddSymbolToTapeAlphabet(xmlSymbol.InnerText[0]);
                }

                this.SetBlankSymbol(xmlFormalDescriptionAttributes["BlankSymbol"].Value[0]);

                XmlNodeList xmlStateSet = doc.GetElementsByTagName("StatesSet")[0].ChildNodes;
                foreach (XmlNode xmlState in xmlStateSet)
                {
                    this.AddMachineState(xmlState.Attributes["Name"].Value);
                }

                foreach (XmlNode xmlState in xmlStateSet)
                {
                    XmlNodeList xmlTransitions = xmlState.ChildNodes[0].ChildNodes;
                    foreach (XmlNode xmlTransition in xmlTransitions)
                    {
                        string xmlOutputStateID = xmlTransition.Attributes["OutputState"].Value;
                        string xmlInputStateID = xmlState.Attributes["Name"].Value;
                        
                        char[] xmlInputSymbolsChar = new char[this.numberOfTapes];
                        XmlNodeList xmlInputSymbols = xmlTransition.ChildNodes[0].ChildNodes;
                        for (int i = 0; i < xmlInputSymbols.Count; i++)
                        {
                            xmlInputSymbolsChar[i] = xmlInputSymbols[i].InnerText[0];
                        }

                        char[] xmlOutputSymbolsChar = new char[this.numberOfTapes];
                        Tape.Direction[] xmlHeadDirection = new Tape.Direction[this.numberOfTapes];
                        XmlNodeList xmlTapeInstructions = xmlTransition.ChildNodes[1].ChildNodes;
                        foreach (XmlNode xmlTapeInstruction in xmlTapeInstructions)
                        {
                            int xmlTapeID = int.Parse(xmlTapeInstruction.Attributes["TapeID"].Value);
                            xmlOutputSymbolsChar[xmlTapeID] = xmlTapeInstruction.Attributes["OutputSymbol"].Value[0];
                            xmlHeadDirection[xmlTapeID] = (Tape.Direction)Enum.Parse(typeof(Tape.Direction), xmlTapeInstruction.Attributes["HeadDirection"].Value);
                        }

                        this.AddTransition(
                            this.GetMachineState(xmlInputStateID),
                            xmlInputSymbolsChar,
                            this.GetMachineState(xmlOutputStateID),
                            xmlOutputSymbolsChar,
                            xmlHeadDirection);
                    }
                }

                XmlNodeList xmlAcceptState = doc.GetElementsByTagName("AcceptState");
                this.SetAcceptState(this.GetMachineState(xmlAcceptState[0].Attributes["StateName"].Value));

                XmlNodeList xmlRejectState = doc.GetElementsByTagName("RejectState");
                this.SetRejectState(this.GetMachineState(xmlRejectState[0].Attributes["StateName"].Value));

                XmlNodeList xmlInitState = doc.GetElementsByTagName("InitialState");
                this.SetInitialState(this.GetMachineState(xmlInitState[0].Attributes["StateName"].Value));
            }
            catch (Exception e)
            {
                throw new Exception("The xml file for the formal description is invalid.", e);
            }
        }

        /// <summary>
        /// Resets the formal description to default (not valid) values for loading
        /// a new description.
        /// </summary>
        private void ResetFormalDescription()
        {
            this.name = string.Empty;
            this.description = string.Empty;
            this.numberOfTapes = 0;
            this.machineStates = new HashSet<MachineState>();
            this.inputAlphabet = new HashSet<char>();
            this.tapeAlphabet = new HashSet<char>();
            this.initialState = null;
            this.acceptState = null;
            this.rejectState = null;
            this.leftBounded = true;
            this.blankSymbol = ' ';
        }

        #endregion
    }
}
