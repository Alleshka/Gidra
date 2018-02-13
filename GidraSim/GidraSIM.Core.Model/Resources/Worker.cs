using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace GidraSIM.Core.Model.Resources
{
    [DataContract(IsReference = true)]
    public class Worker
    {
        //public virtual short WorkerId { get; set; }

        [DataMember]
        private string _name;

        [DataMember]
        private decimal _salaryPerHour;

        [DataMember]
        public virtual Qualification Qualification { get; set; }

        public virtual string Name
        {
            get { return _name; }
            set
            {
                if (value==string.Empty)
                {
                    throw new Exception($"Строка Name не может быть пустой");
                }
                else if (value.Length>20)
                {
                    throw new Exception($"Строка Name не может быть длинее 20 символов");
                }
                else
                {
                    _name = value;

                }
            }
        }

        public virtual decimal SalaryPerHour
        {
            get { return _salaryPerHour; }
            set
            {   
                if(value>=100&&value<=2000)
                    _salaryPerHour = value;
                else
                {
                    throw new Exception($"Значение SalaryPerHour должно входить в диапазон от 100 до 2000");
                }
            }
        }

        //public IEnumerable<Process> Processes { get; set; }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
                return false;

            if (!(obj is Worker))
                return false;

            var temp = obj as Worker;

            if (temp.Name != this.Name) return false;
            if (temp.SalaryPerHour != this.SalaryPerHour) return false;
            if (!Equals(temp.Qualification, this.Qualification)) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
