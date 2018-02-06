using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GidraSim.Model.Resources
{
    public enum TypeInputDevices
    {
        Клавиатура,
        Мышка
    }

    public class InputDevices:ThePrice
    {
        public virtual short InputDevicesId { get; set; }

        public virtual TypeInputDevices Type { get; set; }

        public IEnumerable<TechnicalSupport> TechnicalSupports { get; set; }    

    }
}
