using GidraSIM.Core.Model.Resources;
using System;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GidraSIM.Core.Model.Procedures
{
    [DataContract(IsReference = true)]
    public class FormingDocumentationProcedure:AbstractProcedure
    {

        public FormingDocumentationProcedure():base(1,1)
        {
            Description = "Формирование документации";
        }

        public override void Update(ModelingTime modelingTime)
        {
            if (inputQueue[0].Count() > 0)
            {
                Random rand = new Random();
                var token = inputQueue[0].Peek();

                var worker = resources.Find(res => res is WorkerResource) as WorkerResource;
                var comp = resources.Find(res => res is TechincalSupportResource) as TechincalSupportResource;

                if (worker == null || comp == null)
                {
                    StringBuilder message = new StringBuilder("Оформление документации схемы. Ошибка: ");
                    if (worker == null) message.Append(Environment.NewLine + "Отсутствует ресурс типа \"Исполнитель\"");
                    if (comp == null) message.Append(Environment.NewLine + "Отсутствует ресурс типа \"Техническое обеспечение\"");
                    throw new ArgumentNullException(message.ToString());
                }

                int resourceCount = 0;

                if (token.Progress < 0.01)
                {
                    token.ProcessedByBlock = this;
                    token.ProcessStartTime = modelingTime.Now;
                    //блокируем ресурсы для него

                    if (worker.TryGetResource())
                    {
                        resourceCount++;
                    }
                    else worker.ReleaseResource();

                    if (comp.TryGetResource())
                    {
                        resourceCount++;
                    }
                    else comp.ReleaseResource();
                }
                else
                {
                    resourceCount = 2;
                }


                double time = rand.Next(1, 30);

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
                switch (worker.WorkerQualification)
                {
                    case Qualification.LeadCategory:
                        time -= time / rand.Next(1, 4);
                        break;
                    case Qualification.FirstCategory:
                        time -= time / rand.Next(1, 5);//уменьшаем время, т.к. высокая категория\
                        break;
                    case Qualification.SecondCategory:
                        //базовое время подсчитано для второй категории
                        break;
                    case Qualification.ThirdCategory:
                        time += time / rand.Next(1, 5);
                        break;
                    case Qualification.NoCategory:
                        time += time / rand.Next(1, 4);
                        break;
                }
                #endregion

                //влияение методичики (необязательный ресурс)
                #region Methodical region

                var methodSupport = resources.Find(res => res is MethodolgicalSupportResource) as MethodolgicalSupportResource;
                //если есть методичка, то время немного экономится
                if ((methodSupport != null) && (methodSupport.TryGetResource()))
                {
                    time -= 0.01 * rand.NextDouble(); //от 0 до 15 минут
                }
                #endregion


                if (resourceCount == 2 && worker.TryUseResource(modelingTime))
                {
                    token.Progress += modelingTime.Delta / time;
                }

                if (token.Progress > 0.99)
                {
                    inputQueue[0].Dequeue();
                    token.ProcessEndTime = modelingTime.Now;
                    collector.Collect(token);

                    outputs[0] = new Token(modelingTime.Now, token.Complexity) { Parent = this };

                    // Освобождаем ресурсы
                    worker.ReleaseResource();
                    comp.ReleaseResource();
                    if (methodSupport != null) methodSupport.ReleaseResource();
                }
            }
        }
    }
}
