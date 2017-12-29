using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using GidraSIM.Core.Model;
using GidraSIM.Core.Model.Procedures;
using GidraSIM.Core.Model.Resources;
using GidraSIM.GUI.Core.BlocksWPF;
using GidraSIM.GUI.Core.BlocksWPF.ViewModels.Procedures;
using GidraSIM.GUI.Core.BlocksWPF.ViewModels.Resources;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Windows;

namespace GidraSIM.GUI.Utility
{

    /// Класс, сериализующий процедуры
    public class SaveProcedure
    {
        public String TypeProc { get; set; }
        public Guid Id { get; set; }
        public String ProcessName { get; set; } // Имя процесса 
        public Point Position { get; set; } // Позиция блока 
        public int InputCount { get; set; } // Количество входом
        public int OutputCount { get; set; } // Количество выходов

        public static SaveProcedure ToSave(ProcedureWPF proc)
        {
            return new SaveProcedure()
            {
                TypeProc = ConvertTypeToSave(proc),
                InputCount = proc.InputCount,
                OutputCount = proc.OutputCount,
                Position = proc.Position,
                ProcessName = proc.BlockName,
                Id = Guid.NewGuid()
            };
        }

        public static String ConvertTypeToSave(ProcedureWPF procedureWPF)
        {
            if (procedureWPF is FixedTimeBlockViewModel)
            {
                return (procedureWPF as FixedTimeBlockViewModel).GetType().ToString();
            }
            else if (procedureWPF is QualityCheckProcedureViewModel)
            {
                return (procedureWPF as QualityCheckProcedureViewModel).GetType().ToString();
            }
            else if (procedureWPF is SchemaCreationProcedureViewModel)
            {
                return (procedureWPF as SchemaCreationProcedureViewModel).GetType().ToString();
            }
            else if (procedureWPF is ArrangementProcedureViewModel)
            {
                return (procedureWPF as ArrangementProcedureViewModel).GetType().ToString();
            }
            else if (procedureWPF is ClientCoordinationPrrocedureViewModel)
            {
                return (procedureWPF as ClientCoordinationPrrocedureViewModel).GetType().ToString();
            }
            else if (procedureWPF is DocumentationCoordinationProcedureViewModel)
            {
                return (procedureWPF as DocumentationCoordinationProcedureViewModel).GetType().ToString();
            }
            else if (procedureWPF is ElectricalSchemeSimulationViewModel)
            {
                return (procedureWPF as ElectricalSchemeSimulationViewModel).GetType().ToString();
            }
            else if (procedureWPF is FormingDocumentationProcedureViewModel)
            {
                return (procedureWPF as FormingDocumentationProcedureViewModel).GetType().ToString();
            }
            else if (procedureWPF is PaperworkProcedureViewModel)
            {
                return (procedureWPF as PaperworkProcedureViewModel).GetType().ToString();
            }
            else if (procedureWPF is QualityCheckProcedureViewModel)
            {
                return (procedureWPF as QualityCheckProcedureViewModel).GetType().ToString();
            }
            else if (procedureWPF is SampleTestingProcedureViewModel)
            {
                return (procedureWPF as SampleTestingProcedureViewModel).GetType().ToString();
            }
            else if (procedureWPF is TracingProcedureViewModel)
            {
                return (procedureWPF as TracingProcedureViewModel).GetType().ToString();
            }

            else if (procedureWPF is SubProcessWPF)
                return (procedureWPF as SubProcessWPF).GetType().ToString();

            else if (procedureWPF == null)
            {
                throw new NullReferenceException("Для процедуры WPF не указан прототип");
            }
            else
            {
                throw new NotImplementedException("Даный тип процедуры пока нельзя ковертировать");
            }
        }

        public static ProcedureWPF ToNormal(SaveProcedure proc)
        {
            //return new ProcedureWPF(proc.Position, proc.ProcessName, proc.InputCount, proc.OutputCount);
            System.Reflection.Assembly asm = System.Reflection.Assembly.LoadFrom("GidraSIM.GUI.Core.dll");
            Type type = asm.GetType(proc.TypeProc, true, true);
            return (ProcedureWPF)System.Activator.CreateInstance(type, proc.Position, proc.ProcessName);
        }
    }

    /// <summary>   
    ///  Класс сериализуюзий ресурсы
    /// </summary>
    public class SaveResource
    {
        public String TypeRes { get; set; }
        public Guid Id { get; set; }
        public Point Position { get; set; }
        public String Name { get; set; }

        public static ResourceWPF ToNormal(SaveResource res)
        {
            //return new ResourceWPF(res.Position, res.Name);
            System.Reflection.Assembly asm = System.Reflection.Assembly.LoadFrom("GidraSIM.GUI.Core.dll");
            Type type = asm.GetType(res.TypeRes, true, true);
            return (ResourceWPF)Activator.CreateInstance(type, res.Position, res.Name);
        }

        public static SaveResource ToSave(ResourceWPF res)
        {
            return new SaveResource()
            {
                TypeRes = ConvertTypeResToSave(res),
                Id = Guid.NewGuid(),
                Name = res.BlockName,
                Position = res.Position
            };
        }

        private static String ConvertTypeResToSave(ResourceWPF resourceWPF)
        {
            if (resourceWPF is CadResourceViewModel)
            {
                return (resourceWPF as CadResourceViewModel).GetType().ToString();
            }
            else if (resourceWPF is WorkerResourceViewModel)
            {
                return (resourceWPF as WorkerResourceViewModel).GetType().ToString();
            }
            else if (resourceWPF is TechincalSupportResourceViewModel)
            {
                return (resourceWPF as TechincalSupportResourceViewModel).GetType().ToString();
            }
            else if (resourceWPF is MethodolgicalSupportResourceViewModel)
            {
                return (resourceWPF as MethodolgicalSupportResourceViewModel).GetType().ToString(); ;
            }
            else
            {
                return resourceWPF.GetType().ToString();
            }
        }
    }

    public class SaveProcConnection
    {
        public Guid StartId { get; set; }
        public Guid EndId { get; set; }

        public Point RelativeStartPosition { get; set; }
        public Point RelativeEndPosition { get; set; }

        public int StartPort { get; set; }
        public int EndPort { get; set; }
    }

    public class SaveResConnection
    {
        public Guid StartID { get; set; }
        public Guid EndID { get; set; }

    }

    public class SaveBlock
    {
        public Point Position { get; set; }
    }

    public class SaveProcess
    {
        public String ProcessName { get; set; }
        public List<SaveProcedure> ProcedureList { get; set; }
        public List<SaveResource> ResourceList { get; set; }
        public List<SaveProcConnection> ProcedureConnectionList { get; set; }
        public List<SaveResConnection> ResourceConnectionList { get; set; }

        // Начальный и конечные блоки
        public SaveBlock StartElement { get; set; }
        public SaveBlock EndElement { get; set; }

        public SaveProcess()
        {
            ProcedureConnectionList = new List<SaveProcConnection>();
            ProcedureList = new List<SaveProcedure>();
            ResourceList = new List<SaveResource>();
            ResourceConnectionList = new List<SaveResConnection>();
        }
    }

    public class SaveProject
    {
        public List<SaveProcess> _processes;

        public SaveProject()
        {
            _processes = new List<SaveProcess>();
        }
    }

    public class ProjectSaver : IProjectSaver
    {
        private SaveProject save;

        public void SaveProjectExecute(TabControl tabcontrol, String path)
        {
            save = new Utility.SaveProject();
            foreach (var tabitem in tabcontrol.Items)
            {
                var tab = tabitem as TabItem;
                var drawArea = tab.Content as DrawArea;
                save._processes.Add(SaveProcessExecute(drawArea.Children, tab.Header.ToString()));
            }

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(SaveProject));
                ser.WriteObject(stream, save);
            }
        }
        private SaveProcess SaveProcessExecute(UIElementCollection elementCollection, String ProcessName)
        {
            SaveProcess temp = new SaveProcess(); // Сохранённыйй процесс

            Dictionary<ProcedureWPF, Guid> processes = new Dictionary<ProcedureWPF, Guid>(); // Список отмеченных процессов, которые уже обработали
            Dictionary<ResourceWPF, Guid> resources = new Dictionary<ResourceWPF, Guid>();

            // Проходим по всем элементам
            foreach (UIElement element in elementCollection)
            {
                // Если текущая связь - связь процессов
                if (element is ProcConnectionWPF)
                {
                    SaveProcConnection(element, processes, temp);
                } // Если текущий элемент - связь ресурсов
                else if (element is ResConnectionWPF)
                {
                    SaveResourceConnection(element, processes, resources, temp);
                } // Если текущий элемент - процедура
                else if (element is ProcedureWPF)
                {
                    SaveBlockProcedure(element as ProcedureWPF, processes, temp);
                } // Если элемент - ресурс
                else if (element is ResourceWPF)
                {
                    SaveBlockResourse(element as ResourceWPF, resources, temp);
                }
            }

            // Сохраняем начальный и конечные блоки
            temp.StartElement = new SaveBlock()
            {
                Position = (elementCollection[0] as StartBlockWPF).Position
            };
            temp.EndElement = new SaveBlock()
            {
                Position = (elementCollection[1] as EndBlockWPF).Position
            };

            temp.ProcessName = ProcessName;
            return temp;
        }
        // Сохранение связи процессов
        private void SaveProcConnection(UIElement element, Dictionary<ProcedureWPF, Guid> processes, SaveProcess saveProcess)
        {
            ProcConnectionWPF connect = element as ProcConnectionWPF; // Забираем связь

            // Достаём поля начала и конца связи
            BlockWPF Start = connect.StartBlock; // Начальо
            BlockWPF End = connect.EndBlock; // Конец

            // Начальная и конечная процедуры
            SaveProcedure StartProc = null;
            SaveProcedure EndProc = null;

            // Данное отношение может быть либо процедурой, либо блоком начала/кона (может быть подпроцессом)
            StartProc = CheckProcedureWPF(Start, processes, saveProcess);
            EndProc = CheckProcedureWPF(End, processes, saveProcess);

            // Сохраняем отношение
            SaveProcConnection saveProcConnection = new SaveProcConnection()
            {
                EndId = EndProc.Id,
                EndPort = connect.EndPort,
                StartPort = connect.StartPort,
                StartId = StartProc.Id,
                RelativeEndPosition = connect.RelateEnd,
                RelativeStartPosition = connect.RelateStart
            };
            

            saveProcess.ProcedureConnectionList.Add(saveProcConnection); // Добавляем в связи
        }
        // Парсит указанный блок на принадлежность к процедуре/начальному элементу или концу
        private SaveProcedure CheckProcedureWPF(BlockWPF Block, Dictionary<ProcedureWPF, Guid> processes, SaveProcess saveProcess)
        {
            SaveProcedure curProcedure = null; // Доставаемая процедура

            // Если текущий блок - процедура
            if (Block is ProcedureWPF)
            {
                curProcedure = SaveBlockProcedure(Block, processes, saveProcess); // Сохраняем блок процедуры

            } // Если начальный элемент - начальный/конечный блок
            else
            {
                curProcedure = new SaveProcedure() { Id = new Guid() };
            }

            return curProcedure;
        }
        // Сохранение связи ресурсов
        private void SaveResourceConnection(UIElement element, Dictionary<ProcedureWPF, Guid> processes, Dictionary<ResourceWPF, Guid> resources, SaveProcess saveProcess)
        {
            ResConnectionWPF resConnection = element as ResConnectionWPF; // Достаём связь
            BlockWPF Start = resConnection.StartBlock as BlockWPF; // Достаём начало связи
            BlockWPF End = resConnection.EndBlock as BlockWPF; // Достаём конец связи

            ProcedureWPF procedure = null;
            ResourceWPF resource = null;

            // Если первый блок процедура
            if (Start is ProcedureWPF)
            {
                procedure = Start as ProcedureWPF;
                resource = End as ResourceWPF;
            }
            else // Если второй блок процедура
            {
                procedure = End as ProcedureWPF;
                resource = Start as ResourceWPF;
            }

            SaveProcedure proc = SaveBlockProcedure(procedure, processes, saveProcess); // Сохраняем процедуру
            SaveResource res = SaveBlockResourse(resource, resources, saveProcess); // Сохраняем ресурс

            SaveResConnection resconnect = new SaveResConnection()
            {
                StartID = proc.Id,
                EndID = res.Id
            };
            saveProcess.ResourceConnectionList.Add(resconnect); // Сохраняем связь
        }

        // ----------------------------------- Сохранение отдельных блоков - начало -------------------------------------------------
        // Сохранение блока процедуры
        private SaveProcedure SaveBlockProcedure(BlockWPF Block, Dictionary<ProcedureWPF, Guid> processes, SaveProcess saveProcess)
        {
            SaveProcedure curProcedure = null; // Доставаемая процедура

            // Если данный блок ещё не видели
            if (!processes.ContainsKey(Block as ProcedureWPF))
            {
                curProcedure = SaveProcedure.ToSave(Block as ProcedureWPF); // Переводим элемент в форму сохранения
                processes.Add(Block as ProcedureWPF, curProcedure.Id); // Добавляем в сохранённые процедуры
                saveProcess.ProcedureList.Add(curProcedure); // Сохраняем
            }
            else // Если уже видели
            {
                curProcedure = saveProcess.ProcedureList.Where(x => x.Id.CompareTo(processes[Block as ProcedureWPF]) == 0).First(); // Достаём из словаря
            }

            return curProcedure;
        }
        // Сохранение блока ресурсов
        private SaveResource SaveBlockResourse(BlockWPF Block, Dictionary<ResourceWPF, Guid> resources, SaveProcess saveProcess)
        {
            SaveResource curResource = null;

            // Если данный блок ещё не видели
            if (!resources.ContainsKey(Block as ResourceWPF))
            {
                curResource = SaveResource.ToSave(Block as ResourceWPF); // Переводим в формусохранения
                resources.Add(Block as ResourceWPF, curResource.Id); // Добавляем в обработанные ресурсы
                saveProcess.ResourceList.Add(curResource); // Сохраняем
            }
            else // Просто достаём из словаря
            {
                curResource = saveProcess.ResourceList.Where(x => x.Id.CompareTo(resources[Block as ResourceWPF]) == 0).First(); 
            }

            return curResource;
        }
        // ---------------------------------- Сохранение отдельных блоков - конец ------------------------------------------------------

        public SaveProject LoadProjecExecute(String path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(SaveProject));
                save = (SaveProject)ser.ReadObject(stream);
            }
            return save;
        }
        public void LoadProcessExecute(SaveProcess process, DrawArea area)
        {
            // Создаём начальный и конечный блоки
            StartBlockWPF startBlock = new StartBlockWPF(process.StartElement.Position);
            EndBlockWPF endBlock = new EndBlockWPF(process.EndElement.Position);         

            // Кидаем их на поле
            area.Children.Add(startBlock);
            area.Children.Add(endBlock);

            // Помечаем, что они уже есть   
            area.IsHaveStartEnd = true;

            // Тут лежат уже обработанные блоки
            Dictionary<Guid, ProcedureWPF> worksavelistproc = new Dictionary<Guid, ProcedureWPF>();
            Dictionary<Guid, ResourceWPF> worksavelistres = new Dictionary<Guid, ResourceWPF>();

            // Проходим по сохранённым процедурам
            foreach (var proc in process.ProcedureList)
            {
                area.Children.Add(LoadProcedureBlock(proc, worksavelistproc));
            }

            // Проходим по всем ресурсам
            foreach (var res in process.ResourceList)
            {
                area.Children.Add(LoadResourceBlock(res, worksavelistres));
            }


            // Проходим по всем связям с процедурами
            foreach (SaveProcConnection connectproc in process.ProcedureConnectionList)
            {
                area.Children.Add(LoadProcConnection(connectproc, worksavelistproc, startBlock, endBlock));
            }

            // Проходим по всем связям с ресурсами
            foreach (SaveResConnection connectres in process.ResourceConnectionList)
            {
                area.Children.Add(LoadResConnection(connectres, worksavelistproc, worksavelistres));
            }


        }

        // Загрузка блока процедуры
        private ProcedureWPF LoadProcedureBlock(SaveProcedure Block, Dictionary<Guid, ProcedureWPF> worksavelist)
        {
            // Если текущий блок есть в сохранённых
            if (worksavelist.ContainsKey(Block.Id))
            {
                return worksavelist[Block.Id]; // Просто возвращаем его
            }
            else
            {
                ProcedureWPF curproc = SaveProcedure.ToNormal(Block); // Конвертируем в WPF
                worksavelist.Add(Block.Id, curproc); // Сохраняем в обработанные
                return curproc; // Возвращаем
            }
        }
        // Загрузка блока ресурсов
        private ResourceWPF LoadResourceBlock(SaveResource Block, Dictionary<Guid, ResourceWPF> worksavelist)
        {
            // Если текущий блок есть в сохранённых
            if (worksavelist.ContainsKey(Block.Id))
            {
                return worksavelist[Block.Id]; // Просто возвращаем его
            }
            else
            {
                ResourceWPF curproc = SaveResource.ToNormal(Block); // Конвертируем в WPF
                worksavelist.Add(Block.Id, curproc); // Сохраняем в обработанные
                return curproc; // Возвращаем
            }
        }
        // Загрузка связи с процедурами
        private ResConnectionWPF LoadResConnection(SaveResConnection connectres, Dictionary<Guid, ProcedureWPF> worksavelistproc, Dictionary<Guid, ResourceWPF> worksavelistres)
        {
            ProcedureWPF procedure = null;
            ResourceWPF resource = null;

            procedure = worksavelistproc[connectres.StartID];
            resource = worksavelistres[connectres.EndID];

            ResConnectionWPF connection = new ResConnectionWPF(procedure, resource);

            procedure.AddResPutConnection(connection);
            resource.AddResPutConnection(connection);
            connection.Refresh();
            return connection;
        }
        // Загрузка связи с процедурами
        private ProcConnectionWPF LoadProcConnection(SaveProcConnection connectproc, Dictionary<Guid, ProcedureWPF> worksavelistproc, StartBlockWPF startBlock, EndBlockWPF endBlock)
        {
            ProcedureWPF procStart = null;
            ProcedureWPF procEnd = null;

            // Если начальный элемент не null
            if (connectproc.StartId.CompareTo(new Guid()) != 0)
            {
                procStart = worksavelistproc[connectproc.StartId];
            }

            // Если конечный элемент не null
            if (connectproc.EndId.CompareTo(new Guid()) != 0)
            {
                procEnd = worksavelistproc[connectproc.EndId];
            }

            ProcConnectionWPF connection = null;

            // Если обе связи норм
            if (procStart != null && procEnd != null)
            {
                // Создаём связь
                connection = new ProcConnectionWPF(procStart, procEnd, connectproc.RelativeStartPosition, connectproc.RelativeEndPosition, connectproc.StartPort, connectproc.EndPort);

                // Проставляем связь процедурам
                procStart.AddOutPutConnection(connection);
                procEnd.AddInPutConnection(connection);
            }
            else
            {
                if (procStart == null)
                {
                    connection = new ProcConnectionWPF(startBlock, procEnd, connectproc.RelativeStartPosition, connectproc.RelativeEndPosition, connectproc.StartPort, connectproc.EndPort);

                    // Проставляем связь процедуре
                    procEnd.AddInPutConnection(connection);

                    // Проставляем связь блоку
                    startBlock.AddOutPutConnection(connection);
                }
                else
                {
                    if (procEnd == null)
                    {
                        connection = new ProcConnectionWPF(procStart, endBlock, connectproc.RelativeStartPosition, connectproc.RelativeEndPosition, connectproc.StartPort, connectproc.EndPort);

                        procStart.AddOutPutConnection(connection);
                        endBlock.AddInPutConnection(connection);
                    }
                }
            }

            return connection;
        }
    }
}