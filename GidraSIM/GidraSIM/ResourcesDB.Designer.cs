﻿//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

[assembly: EdmSchemaAttribute()]
namespace GidraSIM
{
    #region Контексты
    
    /// <summary>
    /// Нет доступной документации по метаданным.
    /// </summary>
    public partial class ResourcesEntities : ObjectContext
    {
        #region Конструкторы
    
        /// <summary>
        /// Инициализирует новый объект ResourcesEntities, используя строку соединения из раздела "ResourcesEntities" файла конфигурации приложения.
        /// </summary>
        public ResourcesEntities() : base("name=ResourcesEntities", "ResourcesEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Инициализация нового объекта ResourcesEntities.
        /// </summary>
        public ResourcesEntities(string connectionString) : base(connectionString, "ResourcesEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Инициализация нового объекта ResourcesEntities.
        /// </summary>
        public ResourcesEntities(EntityConnection connection) : base(connection, "ResourcesEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        #endregion
    
        #region Разделяемые методы
    
        partial void OnContextCreated();
    
        #endregion
    
        #region Свойства ObjectSet
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        public ObjectSet<CAD_Systems> CAD_Systems
        {
            get
            {
                if ((_CAD_Systems == null))
                {
                    _CAD_Systems = base.CreateObjectSet<CAD_Systems>("CAD_Systems");
                }
                return _CAD_Systems;
            }
        }
        private ObjectSet<CAD_Systems> _CAD_Systems;
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        public ObjectSet<Methodological_Support> Methodological_Support
        {
            get
            {
                if ((_Methodological_Support == null))
                {
                    _Methodological_Support = base.CreateObjectSet<Methodological_Support>("Methodological_Support");
                }
                return _Methodological_Support;
            }
        }
        private ObjectSet<Methodological_Support> _Methodological_Support;
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        public ObjectSet<Technical_Support> Technical_Support
        {
            get
            {
                if ((_Technical_Support == null))
                {
                    _Technical_Support = base.CreateObjectSet<Technical_Support>("Technical_Support");
                }
                return _Technical_Support;
            }
        }
        private ObjectSet<Technical_Support> _Technical_Support;
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        public ObjectSet<Workers> Workers
        {
            get
            {
                if ((_Workers == null))
                {
                    _Workers = base.CreateObjectSet<Workers>("Workers");
                }
                return _Workers;
            }
        }
        private ObjectSet<Workers> _Workers;

        #endregion

        #region Методы AddTo
    
        /// <summary>
        /// Устаревший метод для добавления новых объектов в набор EntitySet CAD_Systems. Взамен можно использовать метод .Add связанного свойства ObjectSet&lt;T&gt;.
        /// </summary>
        public void AddToCAD_Systems(CAD_Systems cAD_Systems)
        {
            base.AddObject("CAD_Systems", cAD_Systems);
        }
    
        /// <summary>
        /// Устаревший метод для добавления новых объектов в набор EntitySet Methodological_Support. Взамен можно использовать метод .Add связанного свойства ObjectSet&lt;T&gt;.
        /// </summary>
        public void AddToMethodological_Support(Methodological_Support methodological_Support)
        {
            base.AddObject("Methodological_Support", methodological_Support);
        }
    
        /// <summary>
        /// Устаревший метод для добавления новых объектов в набор EntitySet Technical_Support. Взамен можно использовать метод .Add связанного свойства ObjectSet&lt;T&gt;.
        /// </summary>
        public void AddToTechnical_Support(Technical_Support technical_Support)
        {
            base.AddObject("Technical_Support", technical_Support);
        }
    
        /// <summary>
        /// Устаревший метод для добавления новых объектов в набор EntitySet Workers. Взамен можно использовать метод .Add связанного свойства ObjectSet&lt;T&gt;.
        /// </summary>
        public void AddToWorkers(Workers workers)
        {
            base.AddObject("Workers", workers);
        }

        #endregion

    }

    #endregion

    #region Сущности
    
    /// <summary>
    /// Нет доступной документации по метаданным.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="ResourcesModel", Name="CAD_Systems")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class CAD_Systems : EntityObject
    {
        #region Фабричный метод
    
        /// <summary>
        /// Создание нового объекта CAD_Systems.
        /// </summary>
        /// <param name="cad_id">Исходное значение свойства cad_id.</param>
        /// <param name="name">Исходное значение свойства Name.</param>
        /// <param name="license_form">Исходное значение свойства License_form.</param>
        /// <param name="license_status">Исходное значение свойства License_status.</param>
        public static CAD_Systems CreateCAD_Systems(global::System.Int32 cad_id, global::System.String name, global::System.String license_form, global::System.String license_status)
        {
            CAD_Systems cAD_Systems = new CAD_Systems();
            cAD_Systems.cad_id = cad_id;
            cAD_Systems.Name = name;
            cAD_Systems.License_form = license_form;
            cAD_Systems.License_status = license_status;
            return cAD_Systems;
        }

        #endregion

        #region Простые свойства
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 cad_id
        {
            get
            {
                return _cad_id;
            }
            set
            {
                if (_cad_id != value)
                {
                    Oncad_idChanging(value);
                    ReportPropertyChanging("cad_id");
                    _cad_id = StructuralObject.SetValidValue(value, "cad_id");
                    ReportPropertyChanged("cad_id");
                    Oncad_idChanged();
                }
            }
        }
        private global::System.Int32 _cad_id;
        partial void Oncad_idChanging(global::System.Int32 value);
        partial void Oncad_idChanged();
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                OnNameChanging(value);
                ReportPropertyChanging("Name");
                _Name = StructuralObject.SetValidValue(value, false, "Name");
                ReportPropertyChanged("Name");
                OnNameChanged();
            }
        }
        private global::System.String _Name;
        partial void OnNameChanging(global::System.String value);
        partial void OnNameChanged();
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String License_form
        {
            get
            {
                return _License_form;
            }
            set
            {
                OnLicense_formChanging(value);
                ReportPropertyChanging("License_form");
                _License_form = StructuralObject.SetValidValue(value, false, "License_form");
                ReportPropertyChanged("License_form");
                OnLicense_formChanged();
            }
        }
        private global::System.String _License_form;
        partial void OnLicense_formChanging(global::System.String value);
        partial void OnLicense_formChanged();
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String License_status
        {
            get
            {
                return _License_status;
            }
            set
            {
                OnLicense_statusChanging(value);
                ReportPropertyChanging("License_status");
                _License_status = StructuralObject.SetValidValue(value, false, "License_status");
                ReportPropertyChanged("License_status");
                OnLicense_statusChanged();
            }
        }
        private global::System.String _License_status;
        partial void OnLicense_statusChanging(global::System.String value);
        partial void OnLicense_statusChanged();

        #endregion

    }
    
    /// <summary>
    /// Нет доступной документации по метаданным.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="ResourcesModel", Name="Methodological_Support")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Methodological_Support : EntityObject
    {
        #region Фабричный метод
    
        /// <summary>
        /// Создание нового объекта Methodological_Support.
        /// </summary>
        /// <param name="method_supp_id">Исходное значение свойства method_supp_id.</param>
        /// <param name="doc_type">Исходное значение свойства Doc_type.</param>
        /// <param name="multi_client_use">Исходное значение свойства Multi_client_use.</param>
        public static Methodological_Support CreateMethodological_Support(global::System.Int32 method_supp_id, global::System.String doc_type, global::System.String multi_client_use)
        {
            Methodological_Support methodological_Support = new Methodological_Support();
            methodological_Support.method_supp_id = method_supp_id;
            methodological_Support.Doc_type = doc_type;
            methodological_Support.Multi_client_use = multi_client_use;
            return methodological_Support;
        }

        #endregion

        #region Простые свойства
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 method_supp_id
        {
            get
            {
                return _method_supp_id;
            }
            set
            {
                if (_method_supp_id != value)
                {
                    Onmethod_supp_idChanging(value);
                    ReportPropertyChanging("method_supp_id");
                    _method_supp_id = StructuralObject.SetValidValue(value, "method_supp_id");
                    ReportPropertyChanged("method_supp_id");
                    Onmethod_supp_idChanged();
                }
            }
        }
        private global::System.Int32 _method_supp_id;
        partial void Onmethod_supp_idChanging(global::System.Int32 value);
        partial void Onmethod_supp_idChanged();
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Doc_type
        {
            get
            {
                return _Doc_type;
            }
            set
            {
                OnDoc_typeChanging(value);
                ReportPropertyChanging("Doc_type");
                _Doc_type = StructuralObject.SetValidValue(value, false, "Doc_type");
                ReportPropertyChanged("Doc_type");
                OnDoc_typeChanged();
            }
        }
        private global::System.String _Doc_type;
        partial void OnDoc_typeChanging(global::System.String value);
        partial void OnDoc_typeChanged();
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Multi_client_use
        {
            get
            {
                return _Multi_client_use;
            }
            set
            {
                OnMulti_client_useChanging(value);
                ReportPropertyChanging("Multi_client_use");
                _Multi_client_use = StructuralObject.SetValidValue(value, false, "Multi_client_use");
                ReportPropertyChanged("Multi_client_use");
                OnMulti_client_useChanged();
            }
        }
        private global::System.String _Multi_client_use;
        partial void OnMulti_client_useChanging(global::System.String value);
        partial void OnMulti_client_useChanged();

        #endregion

    }
    
    /// <summary>
    /// Нет доступной документации по метаданным.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="ResourcesModel", Name="Technical_Support")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Technical_Support : EntityObject
    {
        #region Фабричный метод
    
        /// <summary>
        /// Создание нового объекта Technical_Support.
        /// </summary>
        /// <param name="tech_supp_id">Исходное значение свойства tech_supp_id.</param>
        /// <param name="name">Исходное значение свойства Name.</param>
        /// <param name="processor">Исходное значение свойства Processor.</param>
        /// <param name="processor_Memory">Исходное значение свойства Processor_Memory.</param>
        /// <param name="diagonal">Исходное значение свойства Diagonal.</param>
        /// <param name="video_card_Memory">Исходное значение свойства Video_card_Memory.</param>
        public static Technical_Support CreateTechnical_Support(global::System.Int32 tech_supp_id, global::System.String name, global::System.String processor, global::System.String processor_Memory, global::System.String diagonal, global::System.String video_card_Memory)
        {
            Technical_Support technical_Support = new Technical_Support();
            technical_Support.tech_supp_id = tech_supp_id;
            technical_Support.Name = name;
            technical_Support.Processor = processor;
            technical_Support.Processor_Memory = processor_Memory;
            technical_Support.Diagonal = diagonal;
            technical_Support.Video_card_Memory = video_card_Memory;
            return technical_Support;
        }

        #endregion

        #region Простые свойства
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 tech_supp_id
        {
            get
            {
                return _tech_supp_id;
            }
            set
            {
                if (_tech_supp_id != value)
                {
                    Ontech_supp_idChanging(value);
                    ReportPropertyChanging("tech_supp_id");
                    _tech_supp_id = StructuralObject.SetValidValue(value, "tech_supp_id");
                    ReportPropertyChanged("tech_supp_id");
                    Ontech_supp_idChanged();
                }
            }
        }
        private global::System.Int32 _tech_supp_id;
        partial void Ontech_supp_idChanging(global::System.Int32 value);
        partial void Ontech_supp_idChanged();
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                OnNameChanging(value);
                ReportPropertyChanging("Name");
                _Name = StructuralObject.SetValidValue(value, false, "Name");
                ReportPropertyChanged("Name");
                OnNameChanged();
            }
        }
        private global::System.String _Name;
        partial void OnNameChanging(global::System.String value);
        partial void OnNameChanged();
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Processor
        {
            get
            {
                return _Processor;
            }
            set
            {
                OnProcessorChanging(value);
                ReportPropertyChanging("Processor");
                _Processor = StructuralObject.SetValidValue(value, false, "Processor");
                ReportPropertyChanged("Processor");
                OnProcessorChanged();
            }
        }
        private global::System.String _Processor;
        partial void OnProcessorChanging(global::System.String value);
        partial void OnProcessorChanged();
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Processor_Memory
        {
            get
            {
                return _Processor_Memory;
            }
            set
            {
                OnProcessor_MemoryChanging(value);
                ReportPropertyChanging("Processor_Memory");
                _Processor_Memory = StructuralObject.SetValidValue(value, false, "Processor_Memory");
                ReportPropertyChanged("Processor_Memory");
                OnProcessor_MemoryChanged();
            }
        }
        private global::System.String _Processor_Memory;
        partial void OnProcessor_MemoryChanging(global::System.String value);
        partial void OnProcessor_MemoryChanged();
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Diagonal
        {
            get
            {
                return _Diagonal;
            }
            set
            {
                OnDiagonalChanging(value);
                ReportPropertyChanging("Diagonal");
                _Diagonal = StructuralObject.SetValidValue(value, false, "Diagonal");
                ReportPropertyChanged("Diagonal");
                OnDiagonalChanged();
            }
        }
        private global::System.String _Diagonal;
        partial void OnDiagonalChanging(global::System.String value);
        partial void OnDiagonalChanged();
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Video_card_Memory
        {
            get
            {
                return _Video_card_Memory;
            }
            set
            {
                OnVideo_card_MemoryChanging(value);
                ReportPropertyChanging("Video_card_Memory");
                _Video_card_Memory = StructuralObject.SetValidValue(value, false, "Video_card_Memory");
                ReportPropertyChanged("Video_card_Memory");
                OnVideo_card_MemoryChanged();
            }
        }
        private global::System.String _Video_card_Memory;
        partial void OnVideo_card_MemoryChanging(global::System.String value);
        partial void OnVideo_card_MemoryChanged();

        #endregion

    }
    
    /// <summary>
    /// Нет доступной документации по метаданным.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="ResourcesModel", Name="Workers")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Workers : EntityObject
    {
        #region Фабричный метод
    
        /// <summary>
        /// Создание нового объекта Workers.
        /// </summary>
        /// <param name="worker_id">Исходное значение свойства worker_id.</param>
        /// <param name="fIO">Исходное значение свойства FIO.</param>
        /// <param name="position">Исходное значение свойства Position.</param>
        public static Workers CreateWorkers(global::System.Int32 worker_id, global::System.String fIO, global::System.String position)
        {
            Workers workers = new Workers();
            workers.worker_id = worker_id;
            workers.FIO = fIO;
            workers.Position = position;
            return workers;
        }

        #endregion

        #region Простые свойства
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 worker_id
        {
            get
            {
                return _worker_id;
            }
            set
            {
                if (_worker_id != value)
                {
                    Onworker_idChanging(value);
                    ReportPropertyChanging("worker_id");
                    _worker_id = StructuralObject.SetValidValue(value, "worker_id");
                    ReportPropertyChanged("worker_id");
                    Onworker_idChanged();
                }
            }
        }
        private global::System.Int32 _worker_id;
        partial void Onworker_idChanging(global::System.Int32 value);
        partial void Onworker_idChanged();
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String FIO
        {
            get
            {
                return _FIO;
            }
            set
            {
                OnFIOChanging(value);
                ReportPropertyChanging("FIO");
                _FIO = StructuralObject.SetValidValue(value, false, "FIO");
                ReportPropertyChanged("FIO");
                OnFIOChanged();
            }
        }
        private global::System.String _FIO;
        partial void OnFIOChanging(global::System.String value);
        partial void OnFIOChanged();
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Position
        {
            get
            {
                return _Position;
            }
            set
            {
                OnPositionChanging(value);
                ReportPropertyChanging("Position");
                _Position = StructuralObject.SetValidValue(value, false, "Position");
                ReportPropertyChanged("Position");
                OnPositionChanged();
            }
        }
        private global::System.String _Position;
        partial void OnPositionChanging(global::System.String value);
        partial void OnPositionChanged();
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Qualification
        {
            get
            {
                return _Qualification;
            }
            set
            {
                OnQualificationChanging(value);
                ReportPropertyChanging("Qualification");
                _Qualification = StructuralObject.SetValidValue(value, true, "Qualification");
                ReportPropertyChanged("Qualification");
                OnQualificationChanged();
            }
        }
        private global::System.String _Qualification;
        partial void OnQualificationChanging(global::System.String value);
        partial void OnQualificationChanged();

        #endregion

    }

    #endregion

}
