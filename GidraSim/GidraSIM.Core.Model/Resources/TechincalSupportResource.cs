using System.Runtime.Serialization;

namespace GidraSIM.Core.Model.Resources
{
    [DataContract(IsReference = true)]
    public class TechincalSupportResource: AbstractResource
    {
        public TechincalSupportResource()
        {
            Count = 1;
            Description = "Компьютер";
        }

        [DataMember(EmitDefaultValue = false)]
        public double Frequency
        {
            get;
            set;
        }

        [DataMember(EmitDefaultValue = false)]
        public double Ram
        {
            get;
            set;
        }
        [DataMember(EmitDefaultValue = false)]
        public double Vram
        {
            get;
            set;
        }

        /// <summary>
        /// число экземпляров
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int Count
        {
            get;
            set;
        }

        public override bool TryGetResource()
        {
            if(Count > 0)
            {
                Count--;
                return true;
            }
            return false;
        }

        public override void ReleaseResource()
        {
            //TODO чисто теоретически можно верунть больше чем есть, нужна защита от дурака
            Count++;
        }

        public override bool Equals(object obj)
        {

            if(!base.Equals(obj))
                return false;

            if (!(obj is TechincalSupportResource))
                return false;
            TechincalSupportResource temp = obj as TechincalSupportResource;

            if (temp.Frequency != this.Frequency)
                return false;
            if (temp.Ram != this.Ram)
                return false;
            if (temp.Vram != this.Vram)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
