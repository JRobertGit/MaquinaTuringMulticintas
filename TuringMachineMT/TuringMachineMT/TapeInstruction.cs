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
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// This class represents a tape instruction with a tape number, 
    /// the symbol to be written and the direction of the head movement.
    /// </summary>
    public class TapeInstruction : IXmlSerializable
    {
        #region Attributes

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

        #endregion

        #region Constructors

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

        #endregion

        #region Getters

        /// <summary>
        /// Gets the tape identifier.
        /// </summary>
        /// <returns>The tape ID.</returns>
        public int GetTapeID()
        {
            return this.tapeID;
        }

        /// <summary>
        /// Gets the output symbol.
        /// </summary>
        /// <returns>The output symbol.</returns>
        public char GetOutputSymbol()
        {
            return this.outputSymbol;
        }

        /// <summary>
        /// Gets the head direction.
        /// </summary>
        /// <returns>The head direction.</returns>
        public Tape.Direction GetHeadDirection()
        {
            return this.headDirection;
        }

        #endregion

        #region Setters

        /// <summary>
        /// Sets the tape that will execute the instruction.
        /// </summary>
        /// <param name="tapeID">The tape identifier in the Tape[] array.</param>
        public void SetTapeID(int tapeID)
        {
            if (tapeID < 0)
            {
                throw new Exception("The tape ID must be a number between 0 and (k-1). Where k is the number of tapes in the Turing machine.");
            }

            this.tapeID = tapeID;
        }

        #endregion

        #region RunningMethods

        /// <summary>
        /// Executes the current tape instruction in the given tape.
        /// </summary>
        /// <param name="tapes">The tapes of the Turing machine.</param>
        public void Execute(Tape[] tapes)
        {
            tapes[this.tapeID].WriteSymbol(this.outputSymbol);
            tapes[this.tapeID].MoveHead(this.headDirection);
        }

        #endregion

        #region XmlSerialization

        /// <summary>
        /// Gets the XmlSchema
        /// </summary>
        /// <returns>Returns null.</returns>
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// The ReadXml serialization method is not implemented.
        /// </summary>
        /// <param name="reader">The parameter is not being used.</param>
        public void ReadXml(System.Xml.XmlReader reader)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The WriteXml serialization method.
        /// </summary>
        /// <param name="writer">The xml writer.</param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("TapeID", this.tapeID.ToString());
            writer.WriteAttributeString("OutputSymbol", this.outputSymbol.ToString());
            writer.WriteAttributeString("HeadDirection", this.headDirection.ToString());
        }

        #endregion
    }
}
