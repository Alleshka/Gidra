using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GidraSim.Model.Processes;

namespace GidraSim.Model.Resources
{
    public class Worker
    {
        private string _name;

        private decimal _salaryPerHour;

        public virtual short WorkerId { get; set; }

        public virtual string Name
        {
            get { return _name; }
            set
            {
                if (value==string.Empty)
                {
                    throw new Exception($"Строка не может быть пустой");
                }
                else if (value.Length>20)
                {
                    throw new Exception($"Строка не может быть длинее 20 символов");
                }
                else
                {
                    _name = value;

                }
            }
        }

        public virtual Qualification Qualification { get; set; }

        public virtual decimal SalaryPerHour
        {
            get { return _salaryPerHour; }
            set
            {   
                if(value>=100&&value<=2000)
                    _salaryPerHour = value;
                else
                {
                    throw new Exception($"Значение должно входить в диапазон от 100 до 2000");
                }
            }
        }

        public IEnumerable<Process> Processes { get; set; }

    }
}
