using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using GidraSIM.Core.Model;
using GidraSIM.Core.Model.Procedures;
using GidraSIM.Core.Model.Resources;
using GidraSIM.GUI.Core.BlocksWPF;
using System.Runtime.Serialization;
using System.IO;
using System.Windows;

namespace GidraSIM.GUI.Utility
{
    public class ProjectSaver : IProjectSaver
    {
        private int SaveVersion = 0;

        private Type[] types;
        private Dictionary<Process, Guid> processSaved; // Содержит обработанные процессы
        Dictionary<Guid, Process> processWorked = new Dictionary<Guid, Process>();
        private TabControl tempTabControl;

        private SaveProject project;
        public ProjectSaver()
        {
            types = new Type[]
            {
                typeof(CadResource),
                typeof(WorkerResource),
                typeof(TechincalSupportResource),
                typeof(MethodolgicalSupportResource),
                typeof(AccidentsCollector),
                typeof(Accident),
                typeof(TokensCollector),
                typeof(ConnectionManager),
                typeof(AndBlock),
                typeof(DuplicateOutputsBlock),
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
            processSaved = new Dictionary<Process, Guid>();
        }

        // Если кто-то будет смотреть или редактировать этот код
        // Простите меня ;(


        public void SaveProjectExecute(TabControl testTabControl, string Path, int mainNumber)
        {
            tempTabControl = testTabControl;

            project = new SaveProject
            {
                SaveVersion = this.SaveVersion // Задаём версию сохранения
            };

            for (int i=0; i<tempTabControl.Items.Count; i++)
            //foreach (TabItem item in tempTabControl.Items)
            {
                TabItem item = tempTabControl.Items[i] as TabItem;

                if (!(processSaved.ContainsKey(item.Header as Process)))
                {
                    String name = (item.Header as Process).Description;

                    var drawArea = item.Content as DrawArea; // Достаём область рисования 
                    var savedProc = SaveProcessExecute(drawArea.Children, name);
                    project.ProcessList.Add(savedProc); // Сохраняем процессы
                    processSaved.Add(item.Header as Process, savedProc.ProcessId);

                    if (mainNumber == i) project.MainProcessID = savedProc.ProcessId;
                }
            }

            // Записываем информацию о проекте
            using (FileStream stream = new FileStream(Path, FileMode.Create))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(SaveProject), types);             
                ser.WriteObject(stream, project);
            }

        }

        /// <summary>
        /// Загрузка проекта
        /// </summary>
        /// <param name="path"></param>
        /// <param name="testTabControl"></param>
        /// <param name="drawAreas"></param>
        /// <param name="processes"></param>
        /// <param name="mainprocess"></param>
        /// <returns></returns>
        public int LoadProjectExecute(string path, TabControl testTabControl, List<DrawArea> drawAreas, List<Process> processes, out Process mainprocess)
        {           
            SaveProject temp = null;

            // Считываем иформацию о проекте
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(SaveProject), types);
                temp = (ser.ReadObject(stream)) as SaveProject;
            }

            if (temp.SaveVersion < this.SaveVersion) throw new Exception("Сохранение несовместимо с текущей версией");

            mainprocess = null;
            int num = 0;

            // Очищаем информацию
            testTabControl.Items.Clear();
            drawAreas.Clear();
            processes.Clear();

            List<TabItem> tabitems = new List<TabItem>();

            // Проходим по каждому процессу
            for (int i = 0; i < temp.ProcessList.Count; i++)
            {         
                // Создаём процесс
                Process process = new Process() { Description = temp[i].ProcessName };
                processes.Add(process); // Добавляем в список процессов

                var tabItem = new TabItem() { Header = process };
                testTabControl.SelectedItem = tabItem;

                // Создаём область рисования
                var drawArea = new DrawArea()
                {
                    Processes = processes
                };
                drawAreas.Add(drawArea);
                tabItem.Content = drawArea;

                LoadProcess(temp[i], drawArea);
                processWorked.Add(temp[i].ProcessId, process);

                if (temp.MainProcessID == temp[i].ProcessId)
                {
                    mainprocess = process;
                    num = i;

                }

                //testTabControl.Items.Add(tabItem);
                tabitems.Add(tabItem);
                              
            }

            // А теперь добавляем, чтобы было в нормальном порядке
            tabitems.Reverse();

            foreach (var ch in tabitems)
            {
                testTabControl.Items.Add(ch);
            }
            //testTabControl.ItemsSource = tabitems;

            return num;
        }

        // <!--------------------------------- Сохранение - Начало ----------------------------->
        private SaveProcess SaveProcessExecute(UIElementCollection collection, String name)
        {
            SaveProcess temp = new SaveProcess
            {
                ProcessName = name
            };

            Dictionary<ProcedureWPF, Guid> ProcedureList = new Dictionary<ProcedureWPF, Guid>(); // Тут лежит список обработанных процедур
            Dictionary<ResourceWPF, Guid> ResourceList = new Dictionary<ResourceWPF, Guid>(); // Тут лежит список обработанных ресурсов 

            // Сохраняем информацию о начальном и конечном элементе
            temp.StartElement = (collection[0] as StartBlockWPF).Position;
            temp.EndElement = (collection[1] as EndBlockWPF).Position;

            temp.ProcessName = name;
            temp.ProcessId = Guid.NewGuid();

            List<UIElement> procedureConnections = new List<UIElement>(); // Список с соединениями процедур
            List<UIElement> resourceConnections = new List<UIElement>(); // Блок с соединениями ресурсов
            List<UIElement> procedures = new List<UIElement>(); // Список с процедурами
            List<UIElement> resourses = new List<UIElement>(); // Список с ресурсами

            // Проходим по всем элементам
            foreach (UIElement element in collection)
            {
                // Если элемент - связь процедур
                if (element is ProcConnectionWPF)
                {
                    procedureConnections.Add(element);
                }

                // Если элемент - связь ресурсов
                if (element is ResConnectionWPF)
                {
                    resourceConnections.Add(element);
                }

                // Если элемент - процедура
                if (element is ProcedureWPF)
                {
                    procedures.Add(element);
                }

                // Если элемент - рксурс
                if (element is ResourceWPF)
                {
                    resourses.Add(element);
                }
            }

            // Проходим по всем связям с процедурами
            foreach (var element in procedureConnections)
            {
                SaveProcConnection(element, ProcedureList, temp);
            }

            // Проходим по всем связям с ресурсами
            foreach (var element in resourceConnections)
            {
                SaveResourceConnection(element, ProcedureList, ResourceList, temp);
            }

            // Проходим по необработанным процедурам
            foreach (var element in procedures)
            {
                SaveBlockProcedure(element as ProcedureWPF, ProcedureList, temp);
            }

            // Проходимся по необработанным ресурсам
            foreach (var element in resourses)
            {
                SaveBlockResourse(element as ResourceWPF, ResourceList, temp);
            }


            return temp; // Возвращаем всю информацию о процессе
        }

        /// <summary>
        /// Производит сохранение информации о связи с процедурами
        /// </summary>
        /// <param name="element"></param>
        /// <param name="processes"></param>
        /// <param name="saveProcess"></param>
        private void SaveProcConnection(UIElement element, Dictionary<ProcedureWPF, Guid> procedures, SaveProcess saveProcess)
        {
            var connection = element as ProcConnectionWPF; // Достаём информацию о связи

            BlockWPF Start = connection.StartBlock; // Информация о начальном блоке в связи
            BlockWPF End = connection.EndBlock; // Информация о конечном блоке в связи

            var StartProcedure = SaveBlockProcedure(Start, procedures, saveProcess); // Сохраняем информацию о начальной процедуре
            var EndProcedure = SaveBlockProcedure(End, procedures, saveProcess); // Сохраняем информацию о конецной процедуре

            // Формируем информацию о связи
            SaveProcedureConnection connect = new SaveProcedureConnection()
            {
                StartID = StartProcedure.Id,
                EndID = EndProcedure.Id,
                StartPort = connection.StartPort,
                EndPort = connection.EndPort,
                RelativeStartPosition = connection.RelateStart,
                RelativeEndPosition = connection.RelateEnd
            };

            // Сохраняем информацию
            saveProcess.ProcedureConnectonList.Add(connect);
        }

        /// <summary>
        /// Производит сохранение информации о связи с ресурсами
        /// </summary>
        /// <param name="element"></param>
        /// <param name="processes"></param>
        /// <param name="resources"></param>
        /// <param name="saveProcess"></param>
        private void SaveResourceConnection(UIElement element, Dictionary<ProcedureWPF, Guid> procedures, Dictionary<ResourceWPF, Guid> resources, SaveProcess saveProcess)
        {
            var connection = element as ResConnectionWPF; // Достаём информацию о связи

            // Информация о начальных и конечных блоках
            BlockWPF Start = connection.StartBlock;
            BlockWPF End = connection.EndBlock;

            SaveProcedure procedure = null;
            SaveResource resource = null;

            // Если начальный элемент - процедура
            if (Start is ProcedureWPF)
            {
                procedure = SaveBlockProcedure(Start, procedures, saveProcess);
                resource = SaveBlockResourse(End, resources, saveProcess);
            }
            else
            {
                procedure = SaveBlockProcedure(End, procedures, saveProcess);
                resource = SaveBlockResourse(Start, resources, saveProcess);
            }

            // Формируем информацию
            SaveResourceConnection resConnection = new Utility.SaveResourceConnection()
            {
                StartID = procedure.Id,
                EndID = resource.Id
            };

            // Сохраняем
            saveProcess.ResourceConnectionList.Add(resConnection);
        }

        /// <summary>
        /// Производит сохранение информации о блоке процедуры
        /// </summary>
        /// <param name="Block"></param>
        /// <param name="processes"></param>
        /// <param name="saveProcess"></param>
        /// <returns></returns>
        private SaveProcedure SaveBlockProcedure(BlockWPF Block, Dictionary<ProcedureWPF, Guid> procedures, SaveProcess saveProcess)
        {
            SaveProcedure temp = null;

            // Если обрабатываемый блок - начальный или конечный элемент
            if (Block is StartBlockWPF || Block is EndBlockWPF)
            {
                temp = new SaveProcedure() { Id = new Guid(), Model = null, Position = Block.Position };
            }
            else
            {
                if (!procedures.ContainsKey(Block as ProcedureWPF))
                {
                    var info = (Block as ProcedureWPF).BlockModel;
                        bool IsProcess = info is Process;
                    Guid childId = new Guid();

                    // Проверяем, не является ли содержимое подпроцессом
                    if (IsProcess)
                    {
                        // Если является

                        // Проверяем, сохранен ли он
                        if (!processSaved.ContainsKey(info as Process))
                        {
                            // Если не сохранён, быстро сохраняем, передавая родительский блок
                            SaveProcess childProc = ExtrSaveProcess((info as Process).Description);
                            childId = childProc.ProcessId;
                            processSaved.Add(info as Process, childId);
                            project.ProcessList.Add(childProc);
                            //processSaved.Add(info as Process, childId);
                        }
                        else // Сохранён
                        {
                            // Находим процесс
                            childId = processSaved[info as Process]; // Достаём его ID
                        }
                    }

                    temp = SaveProcedure.ToSave(Block as ProcedureWPF, IsProcess, childId);
                    procedures.Add(Block as ProcedureWPF, temp.Id);
                    saveProcess.ProcedureList.Add(temp);
                }
                else temp = saveProcess.ProcedureList.Where(x=>x.Id.CompareTo(procedures[Block as ProcedureWPF])==0).First();
            }

            return temp;
        }

        /// <summary>
        /// Производит сохранение о блоке ресурса
        /// </summary>
        /// <param name="Block"></param>
        /// <param name="resources"></param>
        /// <param name="saveProcess"></param>
        /// <returns></returns>
        private SaveResource SaveBlockResourse(BlockWPF Block, Dictionary<ResourceWPF, Guid> resources, SaveProcess saveProcess)
        {
            SaveResource temp = null;

            // Если обрабатываемый блок - начальный или конечный элемент
            if (Block is StartBlockWPF || Block is EndBlockWPF)
            {
                temp = new SaveResource() { Id = new Guid(), Model = null, Position = Block.Position };
            }
            else
            {
                // Проверяем видели ли элемент текущий
                if (!resources.ContainsKey(Block as ResourceWPF))
                {
                    temp = SaveResource.ToSave(Block as ResourceWPF);
                    resources.Add(Block as ResourceWPF, temp.Id); // Добавляем в обработанные блоки
                    saveProcess.ResourceList.Add(temp); // Добавляем в сохранённые
                }
                else temp = saveProcess.ResourceList.Where(x => x.Id.CompareTo(resources[Block as ResourceWPF]) == 0).First(); // Возвращаем блок из списка
            }

            return temp;
        }

        /// <summary>
        /// Внеплановое сохранение процесса
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private SaveProcess ExtrSaveProcess(String name)
        {
            foreach (TabItem tab in tempTabControl.Items)
            {
                if ((tab.Header as Process).Description == name) return SaveProcessExecute((tab.Content as DrawArea).Children, name);
            }

            return null;
        }

        // <!--------------------------------- Сохранение - Конец ----------------------------->

        // <!--------------------------------- Загрузка - Начало ----------------------------->
        public void LoadProcess(SaveProcess process, DrawArea area)
        {
            // Создаём начальный и конечный блоки
            StartBlockWPF startBlock = new StartBlockWPF(process.StartElement);
            EndBlockWPF endBlock = new EndBlockWPF(process.EndElement);

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
            foreach (SaveProcedureConnection connectproc in process.ProcedureConnectonList)
            {
                area.Children.Add(LoadProcConnection(connectproc, worksavelistproc, startBlock, endBlock));
            }

            // Проходим по всем связям с ресурсами
            foreach (SaveResourceConnection connectres in process.ResourceConnectionList)
            {
                area.Children.Add(LoadResConnection(connectres, worksavelistproc, worksavelistres));
            }
        }

        // Загрузка блока процедуры
        private ProcedureWPF LoadProcedureBlock(SaveProcedure Block, Dictionary<Guid, ProcedureWPF> worksavelist)
        {
            if (worksavelist.ContainsKey(Block.Id))
            {
                return worksavelist[Block.Id];
            }
            else
            {
                ProcedureWPF curProc;
                if (Block.IsProcess)
                {
                    Block.Model = processWorked[Block.ChildBlockID];
                    curProc = new SubProcessWPF(Block.Position, Block.Model as Process);
                }
                else
                {
                    curProc = SaveProcedure.ToNormal(Block);

                }

                worksavelist.Add(Block.Id, curProc);

                return curProc;
            }
        }

        // Загрузка блока ресурсов
        private ResourceWPF LoadResourceBlock(SaveResource Block, Dictionary<Guid, ResourceWPF> worksavelist)
        {
            if (worksavelist.ContainsKey(Block.Id))
            {
                return worksavelist[Block.Id];
            }
            else
            {
                ResourceWPF curRes = SaveResource.ToNormal(Block);
                worksavelist.Add(Block.Id, curRes);
                return curRes;
            }
        }

        // Загрузка связи с ресурсами
        private ResConnectionWPF LoadResConnection(SaveResourceConnection connectres, Dictionary<Guid, ProcedureWPF> worksavelistproc, Dictionary<Guid, ResourceWPF> worksavelistres)
        {
            ProcedureWPF proc = worksavelistproc[connectres.StartID];
            ResourceWPF res = worksavelistres[connectres.EndID];

            ResConnectionWPF connection = new ResConnectionWPF(proc, res);
            proc.AddResPutConnection(connection);
            res.AddResPutConnection(connection);

            return connection;
        }

        // Загрузка связи с процедурами
        private ProcConnectionWPF LoadProcConnection(SaveProcedureConnection connectproc, Dictionary<Guid, ProcedureWPF> worksavelistproc, StartBlockWPF startBlock, EndBlockWPF endBlock)
        {
            ProcedureWPF procStart = null;
            ProcedureWPF procEnd = null;

            if (connectproc.StartID.CompareTo(new Guid()) != 0)
            {
                procStart = worksavelistproc[connectproc.StartID];
            }

            if (connectproc.EndID.CompareTo(new Guid()) != 0)
            {
                procEnd = worksavelistproc[connectproc.EndID];
            }

            ProcConnectionWPF connection = null;

            if (procStart != null && procEnd != null)
            {
                connection = new ProcConnectionWPF(procStart, procEnd, connectproc.RelativeStartPosition, connectproc.RelativeEndPosition, connectproc.StartPort, connectproc.EndPort);

                procStart.AddOutPutConnection(connection);
                procEnd.AddInPutConnection(connection);
            }
            else
            {
                if (procStart == null)
                {
                    connection = new ProcConnectionWPF(startBlock, procEnd, connectproc.RelativeStartPosition, connectproc.RelativeEndPosition, connectproc.StartPort, connectproc.EndPort);
                    procEnd.AddInPutConnection(connection);
                    startBlock.AddOutPutConnection(connection);
                }

                if (procEnd == null)
                {
                    connection = new ProcConnectionWPF(procStart, endBlock, connectproc.RelativeStartPosition, connectproc.RelativeEndPosition, connectproc.StartPort, connectproc.EndPort);
                    procStart.AddOutPutConnection(connection);
                    endBlock.AddInPutConnection(connection);
                }
            }

            return connection;
        }

        // <!--------------------------------- Загрузка - Конец ----------------------------->
    }

    /// --------------------------------------- Сохраняемые блоки

    /// <summary>
    /// Сохраняет информацию о процедуре WPF
    /// </summary>
    [DataContract(IsReference = true, Name = "BlockProcedure")]
    public class SaveProcedure
    {
        // ID блока
        [DataMember(Name = "ProcedureID")] public Guid Id { get; set; }

        // Флаг, указывающий на то, является ли блок подпроцессом
        [DataMember(EmitDefaultValue = false)] public bool IsProcess { get; set; }
        [DataMember(EmitDefaultValue = false)] public Guid ChildBlockID { get; set; }

        // Положение блока
        [DataMember(EmitDefaultValue = false)] public Point Position { get; set; }

        // "Содержимое" блока
        [DataMember(Name = "Content")] public IBlock Model { get; set; }  

        /// <summary>
        /// Переводит блок WPF в форму для сохранения
        /// </summary>
        /// <param name="procedure"></param>
        /// <returns></returns>
        public static SaveProcedure ToSave(ProcedureWPF procedure, bool IsProc = false, Guid child = new Guid())
        {
            return new SaveProcedure()
            {
                Id = Guid.NewGuid(),
                IsProcess = IsProc,
                ChildBlockID = child,
                Model = procedure.BlockModel,
                Position = procedure.Position
            };
        }

        /// <summary>
        /// Переводит форму для сохранения в форму блока
        /// </summary>
        /// <param name="procedure"></param>
        /// <returns></returns>
        public static ProcedureWPF ToNormal(SaveProcedure procedure)
        {
            return new ProcedureWPF(procedure.Position, procedure.Model);
        }
    }

    /// <summary>
    /// Сохраняет информацию о ресурсе WPF
    /// </summary>
    [DataContract(IsReference = true, Name = "BlockResource")]
    public class SaveResource
    {
        [DataMember(Name = "ResourceID")] public Guid Id { get; set; }

        [DataMember(EmitDefaultValue = false)] public Point Position { get; set; }

        [DataMember(Name = "Content")] public AbstractResource Model { get; set; }

        public static SaveResource ToSave(ResourceWPF resource)
        {
            return new SaveResource()
            {
                Id = Guid.NewGuid(),
                Position = resource.Position,
                Model = resource.ResourceModel
            };
        }
        public static ResourceWPF ToNormal(SaveResource resource)
        {
            return new ResourceWPF(resource.Position, resource.Model);
        }
    }

    /// <summary>
    /// Сохраняет информацию о связи между процедурами WPF
    /// </summary>
    [DataContract(IsReference = true, Name = "ProcedureConnection")]
    public class SaveProcedureConnection
    {
        [DataMember(EmitDefaultValue = false)]
        public Guid StartID { get; set; } // ID стартового блока 
        [DataMember(EmitDefaultValue = false)]
        public Guid EndID { get; set; } // ID конечного блока

        [DataMember(EmitDefaultValue = false)]
        public Point RelativeStartPosition { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public Point RelativeEndPosition { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int StartPort { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public int EndPort { get; set; }
    }

    /// <summary>
    /// Сохраняет информацию о связи с ресурсами WPF
    /// </summary>
    [DataContract(IsReference = true, Name = "ResourceConnection")]
    public class SaveResourceConnection
    {
        [DataMember(EmitDefaultValue = false)]
        public Guid StartID { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public Guid EndID { get; set; }
    }

    /// <summary>
    /// Сохраняет информацию о процессе
    /// </summary>
    [DataContract(IsReference = true, Name = "ProcessInfo")]
    public class SaveProcess
    {
        [DataMember(EmitDefaultValue = false)] public Guid ProcessId { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public String ProcessName { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public List<SaveProcedure> ProcedureList { get; set; }  // Сохраняет список процедур 
        [DataMember(EmitDefaultValue = false)]
        public List<SaveResource> ResourceList { get; set; } // Сохраняет список ресурсов
        [DataMember(EmitDefaultValue = false)]
        public List<SaveResourceConnection> ResourceConnectionList { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public List<SaveProcedureConnection> ProcedureConnectonList { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Point StartElement { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public Point EndElement { get; set; }

        public SaveProcess()
        {
            ProcedureList = new List<SaveProcedure>();
            ResourceList = new List<SaveResource>();
            ResourceConnectionList = new List<SaveResourceConnection>();
            ProcedureConnectonList = new List<SaveProcedureConnection>();
        }
    }

    [DataContract(IsReference = true)]
    public class SaveProject
    {
        [DataMember]
        public int SaveVersion { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public Guid MainProcessID { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public List<SaveProcess> ProcessList { get; set; }

        public SaveProcess this[int index] => ProcessList[index];

        public SaveProject()
        {
            ProcessList = new List<SaveProcess>();
        }
    }
}