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

    /// <summary>
    /// This class represents a multi-tape Turing machine formal description.
    /// </summary>
    public class FormalDescription
    {
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
        public FormalDescription(
            string name, 
            string description, 
            int numberOfTapes,
            HashSet<char> tapeAlphabet,
            HashSet<char> inputAlphabet,
            char blankSymbol) : this()
        {
            this.SetName(name);
            this.SetDescription(description);
            this.SetNumberOfTapes(numberOfTapes);
            this.SetInputAlphabet(inputAlphabet);
            this.SetTapeAlphabet(tapeAlphabet);
            this.SetBlankSymbol(blankSymbol);
        }

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
        /// Adds a symbol to the tape alphabet.
        /// </summary>
        /// <param name="symbol">The symbol to be added.</param>
        public void AddSymbolToTapeAlphabet(char symbol)
        {
            if (this.inputAlphabet.Contains(symbol))
            {
                throw new Exception("The symbol cannot be added to the tape alphabet since it is being used in the input alphabet.");
            }
            
            this.inputAlphabet.Add(symbol);
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
                if ((inputSymbols[i] != outputSymbols[i]) || headsDirections[i] != Tape.Direction.STAY)
                {
                    tapeInstructions.Add(new TapeInstruction(i, outputSymbols[i], headsDirections[i]));
                }
            }

            inputState.AddTransition(new Transition(outputState, inputSymbols, tapeInstructions));
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
    }
}
