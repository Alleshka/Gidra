using GidraSIM.Core.Model.Resources;
using System;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GidraSIM.Core.Model.Procedures
{
    ///3-D ассемблирование
    [DataContract(IsReference = true)]
    public class Assembling : AbstractProcedure
    {

        public Assembling() : base(1, 1)
        {
            Description = "3-D ассемблирование";
        }

        public override void Update(ModelingTime modelingTime)
        {
            if (inputQueue[0].Count() > 0)
            {
                Random rand = new Random();
                var token = inputQueue[0].Peek();

                var worker = resources.Find(res => res is WorkerResource) as WorkerResource;
                var cad = resources.Find(res => res is CadResource) as CadResource;
                var comp = resources.Find(res => res is TechincalSupportResource) as TechincalSupportResource;

                if (worker == null || cad == null || comp == null)
                {
                    StringBuilder message = new StringBuilder("3-D ассемблирование. Ошибка: ");
                    if (worker == null) message.Append(Environment.NewLine + "Отсутствует ресурс типа \"Исполнитель\"");
                    if (cad == null) message.Append(Environment.NewLine + "Отсутствует ресурс типа \"САПР\"");
                    if (comp == null) message.Append(Environment.NewLine + "Отсутствует ресурс типа \"Техническое обеспечение\"");
                    throw new ArgumentNullException(message.ToString());
                }

                int resourceCount = 0;

                if (token.Progress < 0.001)
                {
                    token.ProcessedByBlock = this;
                    token.ProcessStartTime = modelingTime.Now;
                    //блокируем ресурсы для него

                    if (worker.TryGetResource())
                    {
                        resourceCount++;
                    }
                    else worker.ReleaseResource();

                    if (cad.TryGetResource())
                    {
                        resourceCount++;
                    }
                    else cad.ReleaseResource();

                    if (comp.TryGetResource())
                    {
                        resourceCount++;
                    }
                    else comp.ReleaseResource();
                }
                else
                {
                    resourceCount = 3;
                }

                

                double time = token.Complexity / 10;//базовое время

                // Влияние ПК
                #region PcImpact
                double base_frequency = 1.5;//частота
                double base_memory_proc = 2;//объем памяти процессора
                double base_memory_video = 1;//объем памяти ведеокарты

                time += (base_frequency - comp.Frequency) / 1000; //порядок влияния на время
                time += (base_memory_proc - comp.Ram) / 10000;
                time += (base_memory_video - comp.Vram) / 10000;
                #endregion

                // Влияние рабочего
                #region WorkerImpact
                //временная задержка для анализа полученного результата
                time += 0.02 * rand.NextDouble(); //30(0,02 дня) минут - максимальное время для анализа
                #endregion

                if (resourceCount == 3 && worker.TryUseResource(modelingTime))
                {
                    token.Progress += modelingTime.Delta/time;
                }
                else
                {
                }

                if (token.Progress > 0.999)
                {
                    inputQueue[0].Dequeue();
                    token.ProcessEndTime = modelingTime.Now;
                    collector.Collect(token);

                    outputs[0] = new Token(modelingTime.Now, token.Complexity) { Parent = this };

                    // Освобождаем ресурсы
                    worker.ReleaseResource();
                    comp.ReleaseResource();
                    cad.ReleaseResource();
                }
            }
        }
    }
}