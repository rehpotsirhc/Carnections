using Common.Interfaces;
using Common.Utils;
using System;
using System.ComponentModel;

namespace GoogleDistance.Models
{

    public enum StateNameForm
    {
        NoPreference,
        Full,
        Abbreviation
    }

    public enum StateEnumeration
    {
        [Description("UNKNOWN")]
        UNKNOWN,

        [Description("Alabama")]
        AL,

        [Description("Alaska")]
        AK,

        [Description("Arkansas")]
        AR,

        [Description("Arizona")]
        AZ,

        [Description("California")]
        CA,

        [Description("Colorado")]
        CO,

        [Description("Connecticut")]
        CT,

        [Description("D.C.")]
        DC,

        [Description("Delaware")]
        DE,

        [Description("Florida")]
        FL,

        [Description("Georgia")]
        GA,

        [Description("Hawaii")]
        HI,

        [Description("Iowa")]
        IA,

        [Description("Idaho")]
        ID,

        [Description("Illinois")]
        IL,

        [Description("Indiana")]
        IN,

        [Description("Kansas")]
        KS,

        [Description("Kentucky")]
        KY,

        [Description("Louisiana")]
        LA,

        [Description("Massachusetts")]
        MA,

        [Description("Maryland")]
        MD,

        [Description("Maine")]
        ME,

        [Description("Michigan")]
        MI,

        [Description("Minnesota")]
        MN,

        [Description("Missouri")]
        MO,

        [Description("Mississippi")]
        MS,

        [Description("Montana")]
        MT,

        [Description("North Carolina")]
        NC,

        [Description("North Dakota")]
        ND,

        [Description("Nebraska")]
        NE,

        [Description("New Hampshire")]
        NH,

        [Description("New Jersey")]
        NJ,

        [Description("New Mexico")]
        NM,

        [Description("Nevada")]
        NV,

        [Description("New York")]
        NY,

        [Description("Oklahoma")]
        OK,

        [Description("Ohio")]
        OH,

        [Description("Oregon")]
        OR,

        [Description("Pennsylvania")]
        PA,

        [Description("Rhode Island")]
        RI,

        [Description("South Carolina")]
        SC,

        [Description("South Dakota")]
        SD,

        [Description("Tennessee")]
        TN,

        [Description("Texas")]
        TX,

        [Description("Utah")]
        UT,

        [Description("Virginia")]
        VA,

        [Description("Vermont")]
        VT,

        [Description("Washington")]
        WA,

        [Description("Wisconsin")]
        WI,

        [Description("West Virginia")]
        WV,

        [Description("Wyoming")]
        WY


    }

    public class StateBuilder
    {
        private string _abbreviation;
        private string _fullName;
        private const StateNameForm STATE_NAME_FORM_DEFAULT = StateNameForm.Abbreviation;
        private StateNameForm StateNameFormFromInput = StateNameForm.NoPreference;

        public StateBuilder(string stateString)
        {
            stateString = stateString.RemoveWhiteSpace().Trim();
            StateEnumeration tmp;

            //try to convert based on StateEnumeration value (which are the abbreviations)
            if (Enum.TryParse(stateString, true, out tmp))
            {
                StateNameFormFromInput = StateNameForm.Abbreviation;
                this.StateEnum = tmp;
            }
            else
            {
                StateNameFormFromInput = StateNameForm.Full;
                this.StateEnum = EnumHelper.TryGetValueFromDescriptionWithDefault<StateEnumeration>(stateString);
            }
        }
        public StateEnumeration StateEnum { get; private set; }
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

        public string ToString(StateNameForm stateNameFormOverride)
        {
            //If the override is not no-preference, use it. 
            //If it is no-preference use the one determined by the initial input, unless that is also no-preference (for some reason), in which case use the default
            return SetStateForm(stateNameFormOverride == StateNameForm.NoPreference ?
              DetermineStateFormFromInput() : stateNameFormOverride);
        }

        public override bool Equals(object obj)
        {
            StateBuilder that;
            if (obj is string)
                that = (string)obj;
            else
                that = (StateBuilder)obj;
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

        private StateNameForm DetermineStateFormFromInput()
        {
            return StateNameFormFromInput == StateNameForm.NoPreference ? STATE_NAME_FORM_DEFAULT : StateNameFormFromInput;
        }
        private string SetStateForm(StateNameForm stateform)
        {
            return stateform == StateNameForm.Full ? FullName : Abbreviation;
        }

        public static bool IsState(string state)
        {
            var stateBuilder = new StateBuilder(state);
            return stateBuilder.StateEnum != StateEnumeration.UNKNOWN;
        }

        //public static implicit operator string(State stateEnum)
        //{
        //    return stateEnum.StateString;
        //}
    }
}
