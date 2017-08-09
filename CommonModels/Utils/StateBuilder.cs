using Common.Enums;
using Common.Utils;
using System;
using System.ComponentModel;

namespace GoogleDistance.Models
{
    public class StateBuilder: IEquatable<StateBuilder>
    {
        private string _abbreviation;
        private string _fullName;
        private const EStateNameForm STATE_NAME_FORM_DEFAULT = EStateNameForm.Abbreviation;
        private EStateNameForm StateNameFormFromInput = EStateNameForm.NoPreference;

        public StateBuilder(string stateString)
        {
            stateString = stateString.RemoveWhiteSpace().Trim();
            EStateEnumeration tmp;

            //try to convert based on StateEnumeration value (which are the abbreviations)
            if (Enum.TryParse(stateString, true, out tmp))
            {
                StateNameFormFromInput = EStateNameForm.Abbreviation;
                this.StateEnum = tmp;
            }
            else
            {
                StateNameFormFromInput = EStateNameForm.Full;
                this.StateEnum = EnumHelper.TryGetValueFromDescriptionWithDefault<EStateEnumeration>(stateString);
            }
        }
        public EStateEnumeration StateEnum { get; private set; }
        public string FullName
        {
            get
            {
                if (_fullName == null)
                    _fullName = StateEnum.GetAttributeOfType<DescriptionAttribute>().Description;

                return _fullName;
            }
        }
        public string Abbreviation
        {
            get
            {
                if (_abbreviation == null)
                    _abbreviation = StateEnum.ToString();

                return _abbreviation;
            }
        }

        public static implicit operator StateBuilder(string stateString)
        {
            if (!String.IsNullOrWhiteSpace(stateString))
                return new StateBuilder(stateString);
            else return null;
        }

     
        public string ToString(EStateNameForm stateNameFormOverride)
        {
            //If the override is not no-preference, use it. 
            //If it is no-preference use the one determined by the initial input, unless that is also no-preference (for some reason), in which case use the default
            return SetStateForm(stateNameFormOverride == EStateNameForm.NoPreference ?
              DetermineStateFormFromInput() : stateNameFormOverride);
        }

        public override bool Equals(object obj)
        {
            if (obj is string)
                return String.Equals(this, (string)obj);
            else
                return Equals((StateBuilder)obj);
        }
        public  bool Equals(StateBuilder that)
        {
            return that.StateEnum == this.StateEnum;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + StateEnum.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return SetStateForm(DetermineStateFormFromInput());
        }

        private EStateNameForm DetermineStateFormFromInput()
        {
            return StateNameFormFromInput == EStateNameForm.NoPreference ? STATE_NAME_FORM_DEFAULT : StateNameFormFromInput;
        }
        private string SetStateForm(EStateNameForm stateform)
        {
            return stateform == EStateNameForm.Full ? FullName : Abbreviation;
        }

        public static bool IsState(string state)
        {
            var stateBuilder = new StateBuilder(state);
            return stateBuilder.StateEnum != EStateEnumeration.UNKNOWN;
        }

        public static StateBuilder GetState(string stateString)
        {
            StateBuilder stateBuilder = stateString;
            return stateBuilder;
        }
        public static string GetAbbreviation(string stateString)
        {
            return GetState(stateString).Abbreviation;
        }

        public static string GetFullName(string stateString)
        {
            return GetState(stateString).FullName;
        }

        //public static implicit operator string(State stateEnum)
        //{
        //    return stateEnum.StateString;
        //}
    }
}
