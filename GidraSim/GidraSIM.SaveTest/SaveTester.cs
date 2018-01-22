using System;
using GidraSIM.Core.Model.Procedures;
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

            var types = new Type[]
            {
                typeof(CadResource),
                typeof(WorkerResource),
                typeof(TechincalSupportResource),
                typeof(MethodolgicalSupportResource),
                typeof(TokensCollector),
                typeof(ConnectionManager),
                typeof(ArrangementProcedure),
                typeof(Assembling),
                typeof(ClientCoordinationPrrocedure),
                typeof(DocumentationCoordinationProcedure),
                typeof(ElectricalSchemeSimulation),
                typeof(FixedTimeBlock),
                typeof(FormingDocumentationProcedure),
                typeof(Geometry2D),
                typeof(KDT),
                typeof(KinematicСalculations),
                typeof(PaperworkProcedure),
                typeof(QualityCheckProcedure),
                typeof(SampleTestingProcedure),
                typeof(SchemaCreationProcedure),
                typeof(StrengthСalculations),
                typeof(TracingProcedure),
                typeof(Process)
            };

            // Сериализуем
            using (MemoryStream stream = new MemoryStream())
            {
                DataContractSerializer ser = new DataContractSerializer((typeof(T)), types);
                ser.WriteObject(stream, Base); // Записываем в поток

                dataString = System.Text.Encoding.UTF8.GetString(stream.ToArray()); ;
            }

            // Достаём из памяти
            using (MemoryStream stream = new MemoryStream())
            {
                bytes = System.Text.Encoding.UTF8.GetBytes(dataString);
                stream.Write(bytes, 0, bytes.Length);
                stream.Position = 0;

                DataContractSerializer ser = new DataContractSerializer((typeof(T)), types);
                newBlock = (T) ser.ReadObject(stream);
            }

            return newBlock;
        }
    }
}
