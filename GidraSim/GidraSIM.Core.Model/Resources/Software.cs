using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace GidraSIM.Core.Model.Resources
{
    public enum TypeSoftware
    {
        [Description("ОС")]
        OS,
        [Description("Редактор")]
        Redactor,
        [Description("САПР")]
        CAD
    }

    public enum TypeLicenseForm
    {
        [Description("Открытая")]
        Open,
        [Description("Бесплатная")]
        Free,
        [Description("Условно-бесплатная")]
        ShareWare,
        [Description("Коммерческая")]
        Commerc

    }

    [DataContract(IsReference = true)]
    public class Software : AbstractResource
    {
        //public virtual short SoftwareId { get; set; }

        [DataMember]
        private string _name;

        // TODO: По-моему, это тоже должно быть enum
        [DataMember]
        private string _licenseStatus;

        [DataMember]
        public virtual TypeSoftware Type { get; set; }

        [DataMember]
        public virtual TypeLicenseForm LicenseForm { get; set; }


        public virtual string LicenseStatus 
        {
            get { return _licenseStatus; }
            set
            {
                if (value == String.Empty)
                    throw new Exception("Строка LicenseStatus не может быть пустой");
                else if (value.Length > 15)
                    throw new Exception("Строка LicenseStatus не может быть длинее 50 символов");
                else
                    _licenseStatus = value;
            }
        }

        public virtual string Name
        {
            get { return _name; }
            set
            {
                if (value == String.Empty)
                    throw new Exception("Строка Name не может быть пустой");
                else if (value.Length > 50)
                    throw new Exception("Строка Name не может быть длинее 50 символов");
                else
                    _name = value;
            }
        }

        //public IEnumerable<Process> Processes { get; set; }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
                return false;

            if (!(obj is Software))
                return false;

            var temp = obj as Software;

            if (temp.Name != this.Name) return false;
            if (temp.LicenseStatus != this.LicenseStatus) return false;
            if (temp.LicenseForm != this.LicenseForm) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
