using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GidraSIM.Core.Model.Resources;
using GidraSIM.Core.Model;
using System.Runtime.Serialization;
using System.IO;

namespace GidraSIM.SaveTest
{
    public class SaveTester<T>
    {
        // Возвращает объект, достанный из памяти
        public static T StartSaveTest(T Base)
        {
            T newBlock;

            String dataString;
            Byte[] bytes;

            // Сериализуем
            using (MemoryStream stream = new MemoryStream())
            {
                NetDataContractSerializer ser = new NetDataContractSerializer();
                ser.WriteObject(stream, Base); // Записываем в поток

                dataString = System.Text.Encoding.UTF8.GetString(stream.ToArray()); ;
            }

            // Достаём из памяти
            using (MemoryStream stream = new MemoryStream())
            {
                bytes = System.Text.Encoding.UTF8.GetBytes(dataString);
                stream.Write(bytes, 0, bytes.Length);
                stream.Position = 0;

                NetDataContractSerializer ser = new NetDataContractSerializer();
                newBlock = (T) ser.ReadObject(stream);
            }

            return newBlock;
        }
    }
}
