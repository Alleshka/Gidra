using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GidraSIM.Model
{
    public class SchemaCreationProcedure : Procedure
    {
        private double prevTime;

        public SchemaCreationProcedure (ITokensCollector collector) : base(1, 1, collector)
        {

            
        }

        public override void Update(double globalTime)
        {
            //првоеряем, есть ли вообще что-то на входе
            if (inputQueue[0].Count > 0)
            {
                //смотрим на первыйтокен
                var token = inputQueue[0].Peek();

                var worker = resources.Find(res => res is WorkerResource)  as WorkerResource;
                var cad = resources.Find(res => res is CadResource) as CadResource;
                var techSupport = resources.Find(res => res is TechincalSupportResource) as TechincalSupportResource;

                if (worker == null || cad == null || techSupport == null)
                    throw new ArgumentNullException("SchemaCreationProcedure - не присустствуют все ресурсы");

                int resourceCount = 0;
                

                //токен в первый раз?
                if (token.Progress < 0.01)
                {
                    token.ProcessedByBlock = this;
                    token.ProcessStartTime = globalTime;
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
                    if (techSupport.TryGetResource())
                    {
                        resourceCount++;
                    }
                    else
                    {
                        techSupport.ReleaseResource();
                    }

                }
                //токен тут уже был, ресурсы уже заблочены
                else
                {
                    //поэтому сразу знаем, что все ресурсы есть
                    resourceCount = 3;
                }

                //общее время, которое должно бытьл затрачено на процедуру
                double time = 0;
                //TODO брать это из ресурсов
                double frequency = techSupport.Frequency;
                double memory_proc = techSupport.Ram;
                double memory_video = techSupport.Vram;
                                                                                                                            //базовые параметры: 
                double base_frequency = 1.5;//частота
                double base_memory_proc = 2;//объем памяти процессора
                double base_memory_video = 1;//объем памяти ведеокарты
                                                //выражения полученные аналитическим способом
                time += (base_frequency - techSupport.Frequency) / 1000; //порядок влияния на время
                time += (base_memory_proc - techSupport.Ram) / 10000;
                time += (base_memory_video - techSupport.Vram) / 10000; //TODO ээ, нулевое влияние времени в случае если всё также????

                //TODO добавить влияние категории рабочего на скорость работы

                //обновляем прогресс задачи
                token.Progress += time /(globalTime - prevTime); //делим общее время на dt

                //задача выполнена
                if (token.Progress >= 0.99)
                {
                    inputQueue[0].Dequeue();
                    collector.Collect(token);

                    outputs[0] = new Token(globalTime, token.Complexity) { Parent = this };

                    //освобождаем все ресурсы
                    worker.ReleaseResource();
                    cad.ReleaseResource();
                    techSupport.ReleaseResource();
                }

                //все ресурсы взяли
                if (resourceCount == 3)
                {
                    
                }
            }
            prevTime = globalTime;
        }
    }
}
