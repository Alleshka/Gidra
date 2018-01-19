using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GidraSim.Model.Resources
{
    public enum TypeSoftware
    {
        ОС,
        Редактор,
        САПР
    }

    public enum TypeLicenseForm
    {
        Открытая,
        Бесплатная,
        Условно_бесплатная,
        Коммерческая

    }

    public class Software:ResurcePrice
    {
        private string _name;
        private string _licenseStatus;
        public virtual short SoftwareId { get; set; }

        public virtual TypeSoftware Type { get; set; }

        public virtual string Name  
        {
            get { return _name; }
            set
            {
                if(value==String.Empty)
                    throw new Exception("Строка не может быть пустой");
                else if(value.Length>50)
                    throw new Exception("Строка не может быть длинее 50 символов");
                else
                    _name = value;
            }
        }

        public virtual TypeLicenseForm LicenseForm { get; set; }

        public virtual string LicenseStatus 
        {
            get { return _licenseStatus; }
            set
            {
                if (value == String.Empty)
                    throw new Exception("Строка не может быть пустой");
                else if (value.Length > 15)
                    throw new Exception("Строка не может быть длинее 50 символов");
                else
                    _licenseStatus = value;
            }
        }
    }
}
