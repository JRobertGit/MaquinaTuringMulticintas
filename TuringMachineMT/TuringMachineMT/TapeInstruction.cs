//-----------------------------------------------------------------------
// <copyright file="TapeInstruction.cs" company="M.Cunillé">
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
    /// This class represents a tape instruction with a tape number, 
    /// the symbol to be written and the direction of the head movement.
    /// </summary>
    public class TapeInstruction
    {
        /// <summary>
        /// The tape that will write the symbol and move its head.
        /// </summary>
        private int tapeID;

        /// <summary>
        /// The symbol that will be written into the tape.
        /// </summary>
        private char outputSymbol;

        /// <summary>
        /// The direction to which the head will move.
        /// </summary>
        private Tape.Direction headDirection;

        /// <summary>
        /// Initializes a new instance of the <see cref="TapeInstruction"/> class.
        /// </summary>
        /// <param name="tapeID">write the symbol and move its head.</param>
        /// <param name="outputSymbol">The symbol that will be written into the tape.</param>
        /// <param name="headDirection">The direction to which the head will move.</param>
        public TapeInstruction(int tapeID, char outputSymbol, Tape.Direction headDirection)
        {
            this.SetTapeID(tapeID);
            this.outputSymbol = outputSymbol;
            this.headDirection = headDirection;
        }

        /// <summary>
        /// Executes the current tape instruction in the given tape.
        /// </summary>
        /// <param name="tapes">The tapes of the Turing machine.</param>
        public void Execute(Tape[] tapes)
        {
            tapes[this.tapeID].WriteSymbol(this.outputSymbol);
            tapes[this.tapeID].MoveHead(this.headDirection);
        }

        /// <summary>
        /// Sets the tape that will execute the instruction.
        /// </summary>
        /// <param name="tapeID">The tape identifier in the Tape[] array.</param>
        private void SetTapeID(int tapeID)
        {
            if (tapeID < 0)
            {
                throw new Exception("The tape ID must be a number between 0 and (k-1). Where k is the number of tapes in the Turing machine.");
            }

            this.tapeID = tapeID;
        }
    }
}
