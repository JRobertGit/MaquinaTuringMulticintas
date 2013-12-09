//-----------------------------------------------------------------------
// <copyright file="TuringMachine.cs" company="M.Cunillé">
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
    /// This class represents the execution of a <see cref="FormalDescription"/>.
    /// </summary>
    public class TuringMachine
    {
        /// <summary>
        /// The k-tapes of the Turing Machine.
        /// </summary>
        private Tape[] tapes;

        /// <summary>
        /// The current machine state.
        /// </summary>
        private MachineState currentState;

        /// <summary>
        /// The formal description of the multi-tape Turing machine.
        /// </summary>
        private FormalDescription formalDescription;

        /// <summary>
        /// The machine status.
        /// </summary>
        private MachineStatus machineStatus;

        /// <summary>
        /// Initializes a new instance of the <see cref="TuringMachine"/> class.
        /// </summary>
        /// <param name="inputString">The string to be recognized by the Turing machine.</param>
        /// <param name="tapesInitLength">The initial length of the tapes.</param>
        /// <param name="formalDescription">The formal description of the Turing machine.</param>
        public TuringMachine(string inputString, int tapesInitLength, FormalDescription formalDescription)
        {
            this.machineStatus = MachineStatus.WAITING;

            if (!formalDescription.VerifyDescription())
            {
                throw new Exception("The given formal description is not valid.");
            }

            this.formalDescription = formalDescription;

            if (!this.VerifyInputString(inputString))
            {
                throw new Exception("The given input string is not valid.");
            }

            this.tapes = new Tape[this.formalDescription.GetNumberOfTapes()];
            for (int tapeNumber = 0; tapeNumber < this.tapes.Length; tapeNumber++)
            {
                this.tapes[tapeNumber] = new Tape(tapesInitLength, this.formalDescription.GetBlankSymbol(), this.formalDescription.IsLeftBounded());
            }

            this.tapes[0].WriteString(inputString);

            this.currentState = this.formalDescription.GetInitialState();
        }

        /// <summary>
        /// The possible machine statuses.
        /// </summary>
        public enum MachineStatus 
        {
            /// <summary>
            /// The machine is waiting for the user/system.
            /// </summary>
            WAITING,

            /// <summary>
            /// The machine is currently running.
            /// </summary>
            RUNNING,

            /// <summary>
            /// The machine has accepted the input.
            /// </summary>
            ACCEPTED,

            /// <summary>
            /// The machine has rejected the input.
            /// </summary>
            REJECTED
        }

        /// <summary>
        /// Gets the machine status.
        /// </summary>
        /// <returns>The machine status.</returns>
        public MachineStatus GetMachineStatus()
        {
            return this.machineStatus;
        }

        /// <summary>
        /// Gets the tape contents of the specified tape.
        /// </summary>
        /// <param name="tapeID">The tape identifier.</param>
        /// <returns>The tape contents.</returns>
        public char[] GetTapeContent(int tapeID)
        {
            if (tapeID < 0 || tapeID <= this.tapes.Length)
            {
                throw new Exception("The tape you want to acces does not exist.");
            }

            return this.tapes[tapeID].GetContents();
        }

        /// <summary>
        /// Gets the head location of the specified tape.
        /// </summary>
        /// <param name="tapeID">The tape identifier.</param>
        /// <returns>The head location.</returns>
        public int GetHeadLocation(int tapeID)
        {
            if (tapeID < 0 || tapeID <= this.tapes.Length)
            {
                throw new Exception("The tape you want to acces does not exist.");
            }

            return this.tapes[tapeID].GetHead();
        }

        /// <summary>
        /// Gets the current status name.
        /// </summary>
        /// <returns>The current status name.</returns>
        public string GetCurrentStatusName()
        {
            return this.currentState.GetName();
        }

        /// <summary>
        /// Verifies that the input string contains only the input alphabet symbols.
        /// </summary>
        /// <param name="inputString">The input string.</param>
        /// <returns>If the input string is valid.</returns>
        public bool VerifyInputString(string inputString)
        {
            foreach (char symbol in inputString)
            {
                if (!this.formalDescription.GetInputAlphabet().Contains(symbol))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Executes the Turing machine in order to recognize the input string.
        /// </summary>
        public void RunAll()
        {
            this.machineStatus = MachineStatus.RUNNING;

            while (!this.currentState.Equals(this.formalDescription.GetAcceptState()) &&
                !this.currentState.Equals(this.formalDescription.GetRejectState()))
            {
                this.currentState = this.currentState.NextStep(this.tapes);
                if (this.currentState == null)
                {
                    this.currentState = this.formalDescription.GetRejectState();
                }
            }

            if (this.currentState.Equals(this.formalDescription.GetAcceptState())) 
            {
                this.machineStatus = MachineStatus.ACCEPTED;
            } 
            else 
            {
                this.machineStatus = MachineStatus.REJECTED;
            }
        }

        /// <summary>
        /// Executes the Turing machine step by step in order to recognize the input string.
        /// </summary>
        /// <returns>If the machine has finished recognizing the string.</returns>
        public bool NextStep()
        {
            if (this.machineStatus == MachineStatus.WAITING &&
                !this.currentState.Equals(this.formalDescription.GetAcceptState()) &&
                !this.currentState.Equals(this.formalDescription.GetRejectState()))
            {
                this.machineStatus = MachineStatus.RUNNING;
                this.currentState = this.currentState.NextStep(this.tapes);
            }

            if (this.currentState.Equals(this.formalDescription.GetAcceptState()))
            {
                this.machineStatus = MachineStatus.ACCEPTED;
                return true;
            }
            else if (this.currentState.Equals(this.formalDescription.GetRejectState()))
            {
                this.machineStatus = MachineStatus.REJECTED;
                return true;
            }
            else
            {
                this.machineStatus = MachineStatus.WAITING;
            }

            return false;
        } 
    }
}
