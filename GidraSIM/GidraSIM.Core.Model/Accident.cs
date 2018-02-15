using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GidraSIM.Core.Model
{
    [DataContract(IsReference = true)]
    public class Accident
    {
        [DataMember(EmitDefaultValue = false)]
        public double StartTime { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public double EndTime { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Description { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public IResource Source { get; set; }


        public override string ToString()
        {
            return String.Format("Accident:start time= {0},end time= {1}," +
                "descr={2}, by {3}",
                this.StartTime,
                this.EndTime,
                this.Description,
                this.Source);
        }

        public override bool Equals(object obj)
        {
            Accident temp = obj as Accident;

            if (temp.StartTime != this.StartTime)
                return false;
            if (temp.EndTime != this.EndTime)
                return false;

            if ((!Equals(temp.Source, this.Source)))
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
