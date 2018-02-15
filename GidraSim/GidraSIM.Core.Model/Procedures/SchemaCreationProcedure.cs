using System;
using GidraSIM.Core.Model.Resources;
using System.Runtime.Serialization;

namespace GidraSIM.Core.Model.Procedures
{
    [DataContract(IsReference = true)]
    public class SchemaCreationProcedure : AbstractProcedure
    {

        public SchemaCreationProcedure () : base(1, 1)
        {
            Description = "Создание электрической схемы";
        }

        public override void Update(ModelingTime modelingTime)
        {
            //првоеряем, есть ли вообще что-то на входе
            if (inputQueue[0].Count > 0)
            {
                Random rand = new Random();
                //смотрим на первыйтокен
                var token = inputQueue[0].Peek();

                var worker = resources.Find(res => res is WorkerResource)  as WorkerResource;
                var cad = resources.Find(res => res is CadResource) as CadResource;
                var computer = resources.Find(res => res is TechincalSupportResource) as TechincalSupportResource;

                if (worker == null || cad == null || computer == null)
                    throw new ArgumentNullException("SchemaCreationProcedure - не присустствуют все ресурсы");

                int resourceCount = 0;
                

                //токен в первый раз?
                if (token.Progress < 0.01)
                {
                    token.ProcessedByBlock = this;
                    token.ProcessStartTime = modelingTime.Now;
                    //блокируем ресурсы для него

                    //пробуем взять рабочего
                    if (worker.TryGetResource())
                    {
                        resourceCount++;
                    }
                    else
                    {
                        worker.ReleaseResource();
                    }

                    //пробуем взять CAD
                    if (cad.TryGetResource())
                    {
                        resourceCount++;
                    }
                    else
                    {
                        cad.ReleaseResource();
                    }

                    //пробеум взять методичку
                    if (computer.TryGetResource())
                    {
                        resourceCount++;
                    }
                    else
                    {
                        computer.ReleaseResource();
                    }

                }
                //токен тут уже был, ресурсы уже заблочены
                else
                {
                    //поэтому сразу знаем, что все ресурсы есть
                    resourceCount = 3;
                }


                //общее время, которое должно бытьл затрачено на процедуру
                double time = token.Complexity;

                //влияние ПК на скорость работы
                #region PC impact

                double frequency = computer.Frequency;
                double memory_proc = computer.Ram;
                double memory_video = computer.Vram;
                                                                                                                            //базовые параметры: 
                double base_frequency = 1.5;//частота
                double base_memory_proc = 2;//объем памяти процессора
                double base_memory_video = 1;//объем памяти ведеокарты
                                                //выражения полученные аналитическим способом
                time += (base_frequency - computer.Frequency) / 1000; //порядок влияния на время
                time += (base_memory_proc - computer.Ram) / 10000;
                time += (base_memory_video - computer.Vram) / 10000; //TODO ээ, нулевое влияние времени в случае если всё также????
                #endregion

                //влияние рабочего на скорость работы
                #region Worker impact

                switch(worker.WorkerQualification)
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
                if((methodSupport!=null)&&(methodSupport.TryGetResource()))
                {
                    time -= 0.01 * rand.NextDouble(); //от 0 до 15 минут
                }
                #endregion


                //если все ресурсы взяли, то выполняем задачу
                if (resourceCount == 3 && worker.TryUseResource(modelingTime))
                {
                    //обновляем прогресс задачи
                    token.Progress += modelingTime.Delta/time; //делим общее время на dt
                }

                //задача выполнена
                if (token.Progress >= 0.99)
                {
                    inputQueue[0].Dequeue();
                    token.ProcessEndTime = modelingTime.Now;
                    collector.Collect(token);

                    outputs[0] = new Token(modelingTime.Now, token.Complexity) { Parent = this };

                    //освобождаем все ресурсы
                    worker.ReleaseResource();
                    cad.ReleaseResource();
                    computer.ReleaseResource();
                    if (methodSupport != null) methodSupport.ReleaseResource();
                }

            }
        }
    }
}
