using System;
using System.Runtime.Serialization;

namespace GidraSIM.Core.Model
{
    // TODO: НЕ уверен, что это то, что надо
    //[DataContract(IsReference = true)]
    //public class Qualification
    //{
    //    [DataMember]
    //    private byte _efficiency;
    //    [DataMember]
    //    private byte _errorProbability;

    //    public virtual short QualificationId { get; set; }

    //    public virtual byte Efficiency  
    //    {
    //        get { return _efficiency; }
    //        set
    //        {   if(value>=25&&value<=200)
    //                _efficiency = value;
    //            else
    //            {
    //                throw new Exception($"Значение должно входить в диапазон от 25 до 200");
    //            }
    //        }
    //    }

    //    public virtual byte ErrorProbability    
    //    {
    //        get { return _errorProbability; }
    //        set
    //        {
    //            if(value>=0&&value<=25)
    //                _errorProbability = value;
    //            else
    //            {
    //                throw new Exception($"Значение должно входить в диапазон от 0 до 25");
    //            }
    //        }
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        if (!(obj is Qualification))
    //            return false;

    //        var temp = obj as Qualification;

    //        if (temp.Efficiency != this.Efficiency) return false;
    //        if (temp.ErrorProbability != this.ErrorProbability) return false;

    //        return true;
    //    }

    //    public override int GetHashCode()
    //    {
    //        return base.GetHashCode();
    //    }
    //}
}
