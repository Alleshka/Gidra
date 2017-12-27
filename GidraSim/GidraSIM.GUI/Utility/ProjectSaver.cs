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
    public class SaveProcedures
    {
        public Guid Id { get; set; }
        public String ProcessName { get; set; } // Имя процесса 
        public Point Position { get; set; } // Позиция блока 
        public int InputCount { get; set; } // Количество входом
        public int OutputCount { get; set; } // Количество выходов

        public static SaveProcedures ToSave(ProcedureWPF proc)
        {
            return new SaveProcedures()
            {
                InputCount = proc.InputCount,
                OutputCount = proc.OutputCount,
                Position = proc.Position,
                ProcessName = proc.BlockName,
                Id = Guid.NewGuid()
            };
        }

        public static ProcedureWPF ToNormal(SaveProcedures proc)
        {
            return new ProcedureWPF(proc.Position, proc.ProcessName, proc.InputCount, proc.OutputCount);
        }
    }

    /// <summary>
    ///  Класс сериализуюзий ресурсы
    /// </summary>
    public class SaveResources
    {
        public Guid Id { get; set; }
        public Point Position { get; set; }
        public String Name { get; set; }

        public static ResourceWPF ToNormal(SaveResources res)
        {
            return new ResourceWPF(res.Position, res.Name);
        }

        public static SaveResources ToSave(ResourceWPF res)
        {
            return new SaveResources()
            {
                Id = Guid.NewGuid(),
                Name = res.Name,
                Position = res.Position
            };
        }
    }

    public class SaveProcConnection
    {
        public Guid StartId { get; set; }
        public Guid EndId { get; set; }

        public Point relativeStartPosition { get; set; }
        public Point relativeEndPosition { get; set; }

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
        public String _name { get; set; }
        public List<SaveProcedures> _procedures { get; set; }
        public List<SaveResources> _resources { get; set; }
        public List<SaveProcConnection> _procConnection { get; set; }
        public List<SaveResConnection> _resConnection { get; set; }

        // Начальный и конечные блоки
        public SaveBlock _Start { get; set; }
        public SaveBlock _End { get; set; }

        public SaveProcess()
        {
            _procConnection = new List<SaveProcConnection>();
            _procedures = new List<SaveProcedures>();
            _resources = new List<SaveResources>();
            _resConnection = new List<SaveResConnection>();
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

    public class ProjectSaver
    {
        private SaveProject save;

        public void ActSaveProject(TabControl tabcontrol, String path)
        {
            save = new Utility.SaveProject();
            foreach (var tabitem in tabcontrol.Items)
            {
                var tab = tabitem as TabItem;
                var drawArea = tab.Content as DrawArea;
                save._processes.Add(ActSaveProcess(drawArea.Children, tab.Header.ToString()));
            }

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(SaveProject));
                ser.WriteObject(stream, save);
            }
        }

        public SaveProcess ActSaveProcess(UIElementCollection elementCollection, String ProcessName)
        {
            SaveProcess temp = new SaveProcess();
            Dictionary<ProcedureWPF, Guid> processes = new Dictionary<ProcedureWPF, Guid>(); // Список отмеченных процессов
            UIElement[] mdds = new UIElement[elementCollection.Count];
            elementCollection.CopyTo(mdds, 0);

            // Проходим по всем элементам
            foreach (var element in elementCollection)
            {
                // Смотрим только на связи

                // Если текущая связь - связь ресурсов
                if (element is ProcConnectionWPF)
                {
                    ProcConnectionWPF connect = element as ProcConnectionWPF; // Забираем связь

                    BlockWPF Start = connect.StartBlock; // Начальо
                    BlockWPF End = connect.EndBlock; // Конец

                    SaveProcedures StartProc = null;
                    SaveProcedures EndProc = null;

                    // Если не начальный и не конечный блок
                    //if ((!(Start is StartBlockWPF)) && (!(End is EndBlockWPF)))
                    //{

                    // Если процедура
                    if (Start is ProcedureWPF)
                    {
                        // Если данный блок ещё не видели
                        if (!processes.ContainsKey(Start as ProcedureWPF))
                        {
                            StartProc = SaveProcedures.ToSave(Start as ProcedureWPF);
                            processes.Add(Start as ProcedureWPF, StartProc.Id);
                            temp._procedures.Add(StartProc);
                        }
                        else
                        {
                            StartProc = temp._procedures.Where(x => x.Id.CompareTo(processes[Start as ProcedureWPF]) == 0).First(); // Достаём из словар
                        }
                    }
                    else if (Start is StartBlockWPF)
                    {
                        StartProc = new SaveProcedures() { Id = new Guid() };
                        temp._Start = new SaveBlock()
                        {
                            Position = Start.Position
                        };
                    }

                    if (End is ProcedureWPF)
                    {

                        if (!processes.ContainsKey(End as ProcedureWPF))
                        {
                            EndProc = SaveProcedures.ToSave(End as ProcedureWPF);
                            processes.Add(End as ProcedureWPF, EndProc.Id);
                            temp._procedures.Add(EndProc);
                        }
                        else
                        {
                            EndProc = temp._procedures.Where(x => x.Id.CompareTo(processes[End as ProcedureWPF]) == 0).First(); // Достаём из словар
                        }
                    }

                    else if (End is EndBlockWPF)
                    {
                        EndProc = new SaveProcedures() { Id = new Guid() };
                        temp._End = new SaveBlock()
                        {
                            Position = End.Position
                        };
                    }

                    //}

                    // Сохраняем отношение
                    SaveProcConnection saveProc = new SaveProcConnection()
                    {
                        EndId = EndProc.Id,
                        EndPort = connect.EndPort,
                        StartPort = connect.StartPort,
                        StartId = StartProc.Id,
                        relativeEndPosition = connect.RelateEnd,
                        relativeStartPosition = connect.RelateStart
                    };
                    temp._procConnection.Add(saveProc); // Добавляем в связи
                }
            }

            return temp;
        }

        public SaveProject ActLoadProject(String path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(SaveProject));
                save = (SaveProject)ser.ReadObject(stream);
            }
            return save;
        }

        public void ActLoadProcess(SaveProcess process, ref DrawArea area)
        {
            StartBlockWPF startBlock = new StartBlockWPF(process._Start.Position);
            EndBlockWPF endBlock = new EndBlockWPF(process._End.Position);         

            area.Children.Add(startBlock);
            area.Children.Add(endBlock);

            // Тут лежат уже обработанные блоки
            Dictionary<Guid, ProcedureWPF> worksavelist = new Dictionary<Guid, ProcedureWPF>();
            ProcedureWPF Start = null;
            ProcedureWPF End = null;

            foreach (var t in process._procConnection)
            {
                // Тут не обрабатываем начальный и конечный блоки
                if (t.StartId.CompareTo(new Guid()) != 0 && (t.EndId.CompareTo(new Guid()) != 0))
                {
                    // Достаём блоки у связи
                    // Проверяем, есть ли стартовый блок в сохранённых
                    if (!worksavelist.ContainsKey(t.StartId))
                    {
                        Start = SaveProcedures.ToNormal(process._procedures.Where(x => x.Id.CompareTo(t.StartId) == 0).First());
                        worksavelist.Add(t.StartId, Start);
                        area.Children.Add(Start);
                    }
                    else Start = worksavelist[t.StartId];

                    if (!worksavelist.ContainsKey(t.EndId))
                    {
                        End = SaveProcedures.ToNormal(process._procedures.Where(x => x.Id.CompareTo(t.EndId) == 0).First());
                        worksavelist.Add(t.EndId, End);
                        area.Children.Add(End);
                    }
                    else End = worksavelist[t.EndId];

                    ProcConnectionWPF proc = new ProcConnectionWPF(Start, End, t.relativeStartPosition, t.relativeEndPosition, t.StartPort, t.EndPort);

                    // Проставлем связи
                    Start.AddInPutConnection(proc);
                    End.AddOutPutConnection(proc);
                    area.Children.Add(proc);
                }

                // Если у связи старт - старотвый блок
                if (t.StartId.CompareTo(new Guid()) == 0)
                {
                    if (!worksavelist.ContainsKey(t.EndId))
                    {
                        End = SaveProcedures.ToNormal(process._procedures.Where(x => x.Id.CompareTo(t.EndId) == 0).First());
                        worksavelist.Add(t.EndId, End);
                        area.Children.Add(End);
                    }
                    else End = worksavelist[t.EndId];

                    ProcConnectionWPF proc = new ProcConnectionWPF(startBlock, End, t.relativeStartPosition, t.relativeEndPosition, t.StartPort, t.EndPort);

                    startBlock.AddOutPutConnection(proc);
                    End.AddInPutConnection(proc);
                    area.Children.Add(proc);
                }

                // Если конец - конечный блок
                if (t.EndId.CompareTo(new Guid()) == 0)
                {
                    if (!worksavelist.ContainsKey(t.StartId))
                    {
                        Start = SaveProcedures.ToNormal(process._procedures.Where(x => x.Id.CompareTo(t.StartId) == 0).First());
                        worksavelist.Add(t.StartId, End);
                        area.Children.Add(Start);
                    }
                    else Start = worksavelist[t.StartId];

                    ProcConnectionWPF proc = new ProcConnectionWPF(Start, endBlock, t.relativeStartPosition, t.relativeEndPosition, t.StartPort, t.EndPort);

                    startBlock.AddOutPutConnection(proc);
                    End.AddInPutConnection(proc);
                    endBlock.AddInPutConnection(proc);
                    startBlock.AddOutPutConnection(proc);
                    area.Children.Add(proc);
                }
            }
        }
    }
}