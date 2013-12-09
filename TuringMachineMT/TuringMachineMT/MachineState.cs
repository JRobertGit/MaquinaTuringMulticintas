//-----------------------------------------------------------------------
// <copyright file="MachineState.cs" company="M.Cunillé">
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
    using System.Xml.Serialization;

    /// <summary>
    /// This class represents a state in the Turing machine.
    /// </summary>
    public class MachineState : IXmlSerializable
    {
        #region Attributes

        /// <summary>
        /// The number of states currently created.
        /// </summary>
        private static int stateCount = 0;

        /// <summary>
        /// The id of the machine state.
        /// </summary>
        private int stateID;

        /// <summary>
        /// The descriptive name of the machine state.
        /// </summary>
        private string name;

        /// <summary>
        /// A list with all the possible transitions from this state.
        /// </summary>
        private HashSet<Transition> transitions;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MachineState"/> class.
        /// </summary>
        public MachineState()
        {
            this.stateID = stateCount++;
            this.name = string.Format("q{0}", this.stateID);
            this.transitions = new HashSet<Transition>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MachineState"/> class.
        /// </summary>
        /// <param name="name">The descriptive name of the machine state.</param>
        public MachineState(string name) : this()
        {
            this.name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MachineState"/> class.
        /// </summary>
        /// <param name="name">The descriptive name of the machine state.</param>
        /// <param name="transitions">A list with all the possible transitions from this state.</param>
        public MachineState(string name, HashSet<Transition> transitions) : this()
        {
            this.name = name;
            this.transitions = transitions;
        }

        #endregion

        #region Getters

        /// <summary>
        /// Gets the descriptive name of the state.
        /// </summary>
        /// <returns>The name of the state</returns>
        public string GetName()
        {
            return this.name;
        }

        /// <summary>
        /// Gets the state transitions in the form (Qi, I1, I2,..., Ik) -> (Qo, O1, O2,..., Ok, D1, D2,..., Dk)
        /// </summary>
        /// <returns>The state transitions.</returns>
        public string[] GetTransitionsAsString()
        {
            string[] transitionsStr = new string[this.transitions.Count];

            int counter = 0;
            foreach (Transition transition in this.transitions)
            {
                string inputStateNameStr = this.name;
                char[] inputSymbolsChar = transition.GetInputSymbols();
                string outputStateNameStr = transition.GetOutputStateName();
                char[] outputSymbolsChar = transition.GetOutputSymbols();
                Tape.Direction[] headsDirections = transition.GetHeadsDirections();

                string inputSymbolsStr = string.Empty;
                for (int i = 0; i < inputSymbolsChar.Length; i++)
                {
                    if (i == inputSymbolsChar.Length - 1)
                    {
                        inputSymbolsStr = string.Format("{0}{1}", inputSymbolsStr, inputSymbolsChar[i]);
                    } 
                    else
                    {
                        inputSymbolsStr = string.Format("{0}{1}, ", inputSymbolsStr, inputSymbolsChar[i]);
                    }
                }

                string outputSymbolsStr = string.Empty;
                for (int i = 0; i < outputSymbolsChar.Length; i++)
                {
                    if (i == outputSymbolsChar.Length - 1)
                    {
                        outputSymbolsStr = string.Format("{0}{1}", outputSymbolsStr, outputSymbolsChar[i]);
                    }
                    else
                    {
                        outputSymbolsStr = string.Format("{0}{1}, ", outputSymbolsStr, outputSymbolsChar[i]);
                    }
                }

                string headsDirectionsStr = string.Empty;
                for (int i = 0; i < headsDirections.Length; i++)
                {
                    if (i == headsDirections.Length - 1)
                    {
                        headsDirectionsStr = string.Format("{0}{1}", headsDirectionsStr, headsDirections[i].ToString());
                    }
                    else
                    {
                        headsDirectionsStr = string.Format("{0}{1}, ", headsDirectionsStr, headsDirections[i].ToString());
                    }
                }

                string transitionStr = string.Format("({0}, {1}) -> ({2}, {3}, {4})", inputStateNameStr, inputSymbolsStr, outputStateNameStr, outputSymbolsStr, headsDirectionsStr);
                transitionsStr[counter++] = transitionStr;
            }

            return transitionsStr;
        }

        #endregion

        #region AddMethods

        /// <summary>
        /// Adds a new transition to the current state.
        /// </summary>
        /// <param name="outputState">The next state after the transition.</param>
        /// <param name="inputSymbols">The k-symbols of the k-tapes that are needed to be read
        /// before using this transition.</param>
        /// <param name="tapeInstructions">The instructions that will be followed on the k-tapes.</param>
        public void AddTransition(MachineState outputState, char[] inputSymbols, List<TapeInstruction> tapeInstructions)
        {
            this.transitions.Add(new Transition(outputState, inputSymbols, tapeInstructions));
        }

        /// <summary>
        /// Adds a new transition to the current state.
        /// </summary>
        /// <param name="transition">The transition that will be added.</param>
        public void AddTransition(Transition transition)
        {
            this.transitions.Add(transition);
        }

        #endregion

        #region RunningMethods

        /// <summary>
        /// Changes to the next state of the Turing machine.
        /// </summary>
        /// <param name="tapes">The Turing machine tapes</param>
        /// <returns>The new state of the Turing machine.</returns>
        public MachineState NextStep(Tape[] tapes)
        {
            char[] inputSymbols = new char[tapes.Length];

            for (int tape = 0; tape < tapes.Length; tape++)
            {
                inputSymbols[tape] = tapes[tape].ReadSymbol();
            }

            foreach (Transition transition in this.transitions)
            {
                if (transition.Validate(inputSymbols))
                {
                    return transition.Execute(tapes);
                }
            }

            return null;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Overrides the Equals method.
        /// </summary>
        /// <param name="obj">The two states.</param>
        /// <returns>If the machine states are equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            MachineState state = obj as MachineState;
            if (state == null)
            {
                return false;
            }

            return this.name == state.name;
        }

        /// <summary>
        /// Gets the object hash code.
        /// </summary>
        /// <returns>The object hash code.</returns>
        public override int GetHashCode()
        {
            return this.name.GetHashCode();
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
            writer.WriteAttributeString("ID", this.stateID.ToString());
            writer.WriteAttributeString("Name", this.name);

            writer.WriteStartElement("Transitions");
            foreach (Transition transition in this.transitions)
            {
                writer.WriteStartElement("Transition");
                transition.WriteXml(writer);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        #endregion
    }
}
