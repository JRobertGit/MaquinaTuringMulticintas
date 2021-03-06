﻿//-----------------------------------------------------------------------
// <copyright file="Transition.cs" company="M.Cunillé">
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
    /// This class represents a transition between two states given input symbols.
    /// </summary>
    public class Transition : IXmlSerializable
    {
        #region Attributes

        /// <summary>
        /// The next state of the Turing machine following this transition.
        /// </summary>
        private MachineState outputState;

        /// <summary>
        /// The k-symbols belonging to the tape alphabet that the k-tapes 
        /// must contain in order to use this transition.
        /// </summary>
        private char[] inputSymbols;

        /// <summary>
        /// The instructions that the k-tapes will follow after using this transition.
        /// </summary>
        private List<TapeInstruction> tapeInstructions;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Transition"/> class.
        /// </summary>
        /// <param name="outputState">The next state of the Turing machine following this transition.</param>
        /// <param name="inputSymbols">The k-symbols belonging to the tape alphabet that the k-tapes
        /// must contain in order to use this transition.</param>
        /// <param name="tapeInstructions">The instructions that the k-tapes will follow after using this transition.</param>
        public Transition(MachineState outputState, char[] inputSymbols, List<TapeInstruction> tapeInstructions)
        {
            this.outputState = outputState;
            this.inputSymbols = inputSymbols;
            this.tapeInstructions = tapeInstructions;
        }

        #endregion

        #region Getters

        /// <summary>
        /// Gets the output state name.
        /// </summary>
        /// <returns>The state name.</returns>
        public string GetOutputStateName()
        {
            return this.outputState.GetName();
        }

        /// <summary>
        /// Gets the input symbols.
        /// </summary>
        /// <returns>The input symbols.</returns>
        public char[] GetInputSymbols()
        {
            return this.inputSymbols;
        }

        /// <summary>
        /// Gets the output symbols.
        /// </summary>
        /// <returns>The output symbols.</returns>
        public char[] GetOutputSymbols()
        {
            char[] outputSymbols = new char[this.tapeInstructions.Count];

            for (int i = 0; i < this.tapeInstructions.Count; i++)
            {
                outputSymbols[i] = this.tapeInstructions[i].GetOutputSymbol();
            }

            return outputSymbols;
        }

        /// <summary>
        /// Gets the heads directions.
        /// </summary>
        /// <returns>The heads directions.</returns>
        public Tape.Direction[] GetHeadsDirections()
        {
            Tape.Direction[] headDirections = new Tape.Direction[this.tapeInstructions.Count];

            for (int i = 0; i < this.tapeInstructions.Count; i++)
            {
                headDirections[i] = this.tapeInstructions[i].GetHeadDirection();
            }

            return headDirections;
        }

        #endregion

        #region RunningMethods

        /// <summary>
        /// Makes the transition and executes the instructions given for each tape.
        /// </summary>
        /// <param name="tapes">The Turing machine tapes</param>
        /// <returns>The current state after executing the transition.</returns>
        public MachineState Execute(Tape[] tapes)
        {
            foreach (TapeInstruction instruction in this.tapeInstructions)
            {
                instruction.Execute(tapes);
            }

            return this.outputState;
        }

        /// <summary>
        /// Checks if the input symbols are the same in order to use this transition.
        /// </summary>
        /// <param name="inputSymbols">The k-symbols of the k-tapes located in the k-headers location.</param>
        /// <returns>If the input symbols correspond to the ones needed in order to use this transition.</returns>
        public bool Validate(char[] inputSymbols)
        {
            for (int i = 0; i < inputSymbols.Length; i++)
            {
                if (this.inputSymbols[i] != inputSymbols[i])
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Overrides the Equals method.
        /// </summary>
        /// <param name="obj">The transition to compare with this transition.</param>
        /// <returns>If the transitions are equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Transition transition = obj as Transition;
            if (transition == null)
            {
                return false;
            }

            bool inputSymbolsEqual = true;

            for (int i = 0; i < this.inputSymbols.Length; i++)
            {
                if (this.inputSymbols[i] != transition.inputSymbols[i])
                {
                    inputSymbolsEqual = false;
                    break;
                }
            }

            return inputSymbolsEqual;
        }

        /// <summary>
        /// Gets the object hash code.
        /// </summary>
        /// <returns>The object hash code.</returns>
        public override int GetHashCode()
        {
            string inputConfiguration = new string(this.inputSymbols);
            return inputConfiguration.GetHashCode();
        }

        #endregion

        #region XmlSerialization

        /// <summary>
        /// Gets the XmlSchema.
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
            writer.WriteAttributeString("OutputState", this.outputState.GetName());

            writer.WriteStartElement("InputSymbols");
            foreach (char symbol in this.inputSymbols)
            {
                writer.WriteElementString("Symbol", symbol.ToString());
            }

            writer.WriteEndElement();

            writer.WriteStartElement("TapeInstructions");
            foreach (TapeInstruction instruction in this.tapeInstructions)
            {
                writer.WriteStartElement("TapeInstruction");
                instruction.WriteXml(writer);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        #endregion
    }
}
