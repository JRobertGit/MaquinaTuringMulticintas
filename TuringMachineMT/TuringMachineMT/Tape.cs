//-----------------------------------------------------------------------
// <copyright file="Tape.cs" company="M.Cunillé">
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
    /// This class represents a tape.
    /// </summary>
    public class Tape
    {
        /// <summary>
        /// If the current tape is left-bounded.
        /// </summary>
        private bool leftBounded;

        /// <summary>
        /// The blank symbol of the tape.
        /// </summary>
        private char blankSymbol;

        /// <summary>
        /// The tape content.
        /// </summary>
        private List<char> content;

        /// <summary>
        /// The read/write head location.
        /// </summary>
        private int head;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tape"/> class.
        /// </summary>
        /// <param name="initialLength">The initial length of the tape.</param>
        /// <param name="blankSymbol">The blank symbol.</param>
        public Tape(int initialLength, char blankSymbol)
        {
            this.leftBounded = true;
            this.blankSymbol = blankSymbol;
            this.content = new List<char>();

            for (int i = 0; i < initialLength; i++)
            {
                this.AddCell();
            }

            this.head = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tape"/> class.
        /// </summary>
        /// <param name="initialLength">The initial length of the tape.</param>
        /// <param name="blankSymbol">The blank symbol.</param>
        /// <param name="leftBounded">If the parameter is left-bounded.</param>
        public Tape(int initialLength, char blankSymbol, bool leftBounded)
            : this(initialLength, blankSymbol)
        {
            this.leftBounded = leftBounded;
        }

        /// <summary>
        /// The direction of the head's movement.
        /// </summary>
        public enum Direction
        {
            /// <summary>
            /// Move to the RIGHT.
            /// </summary>
            RIGHT = 1,

            /// <summary>
            /// Move to the LEFT.
            /// </summary>
            LEFT = -1,

            /// <summary>
            /// STAY in current position.
            /// </summary>
            STAY = 0
        }

        /// <summary>
        /// Returns the symbol in the current head position.
        /// </summary>
        /// <returns>The symbol currently in the tape.</returns>
        public char ReadSymbol()
        {
            return this.content[this.head];
        }

        /// <summary>
        /// Writes the given symbol into the tape at the current head position.
        /// </summary>
        /// <param name="symbol">The symbol to be written.</param>
        public void WriteSymbol(char symbol)
        {
            this.content[this.head] = symbol;
        }

        /// <summary>
        /// Writes the given string of symbols into the tape at the current head position.
        /// </summary>
        /// <param name="symbols">The symbols to be written.</param>
        public void WriteString(string symbols)
        {
            int initialHeadPosition = this.head;

            foreach (char symbol in symbols)
            {
                this.WriteSymbol(symbol);
                this.MoveHead(Tape.Direction.RIGHT);
            }

            this.head = initialHeadPosition;
        }

        /// <summary>
        /// Moves the head to the specified direction.
        /// </summary>
        /// <param name="direction">The direction of the head movement.</param>
        public void MoveHead(Direction direction)
        {
            if ((int)direction < 0 && this.head == 0)
            {
                if (this.leftBounded)
                {
                    return;
                }
                else
                {
                    this.AddCellBeginning();
                    return;
                }
            }
                
            this.head += (int)direction;

            if (this.head == this.content.Count - 1)
            {
                this.AddCell();
            }
        }

        /// <summary>
        /// Adds a cell to the tape increasing its length by one.
        /// </summary>
        private void AddCell()
        {
            this.content.Add(this.blankSymbol);
        }

        /// <summary>
        /// Adds a cell at the beginning of the tape increasing its length by one.
        /// </summary>
        private void AddCellBeginning()
        {
            this.content.Insert(0, this.blankSymbol);
        }
    }
}
