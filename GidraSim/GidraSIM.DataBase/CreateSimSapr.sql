--Создание базы ресурсов обратно совместимой с SQL server 2012
CREATE DATABASE SimSapr
GO
ALTER DATABASE SimSapr SET COMPATIBILITY_LEVEL = 100
GO

USE SimSapr
GO

CREATE SCHEMA Resources
GO

--Таблица информационного обеспечения
CREATE TABLE Resources.InformationSupports
(
    InformationSupportId SMALLINT PRIMARY KEY IDENTITY(1,1),
    MultiClientUse BIT NOT NULL DEFAULT 0,
    Type NVARCHAR(11) NOT NULL CHECK(Type in('Бумажный','Электронный')),
    Price SMALLMONEY NOT NULL CHECK(Price>=0)
);

--Таблица рабочих
CREATE TABLE Resources.Workers
(
    WorkerId SMALLINT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(20) NOT NULL CHECK(LEN(Name)>0),
    SalaryPerHour SMALLMONEY NOT NULL CHECK(SalaryPerHour between 100 and 2000),
);

--Таблица программного обеспечения
CREATE TABLE Resources.Softwares
(
    SoftwareId SMALLINT PRIMARY KEY IDENTITY(1,1),
    Type NVARCHAR(8) NOT NULL CHECK(Type in('ОС','Редактор','САПР')),
    Name NVARCHAR(50) NOT NULL UNIQUE CHECK(LEN(Name)>0),
    LicenseForm NVARCHAR(18) NOT NULL CHECK(LicenseForm in('Открытая','Бесплатная','Условно-бесплатная','Коммерческая')),
    LicenseStatus NVARCHAR(15) NOT NULL CHECK(LEN(LicenseStatus)>0) ,
    Price SMALLMONEY NOT NULL CHECK(Price>=0)
);

--Таблица квалификации работников
CREATE TABLE Resources.Qualifications
(
    WorkerId SMALLINT REFERENCES Resources.Workers(WorkerId),
    SoftwareId SMALLINT REFERENCES Resources.Softwares(SoftwareId),
    Experience NVARCHAR(24) NOT NULL CHECK(Experience in('Новичок','Уверенный пользователь','Продвинутый пользователь','Профессионал')) DEFAULT 'Новичок',
    PRIMARY KEY(WorkerId,SoftwareId)
);

--Таблица мониторы
CREATE TABLE Resources.Monitors
(
    MonitorId SMALLINT PRIMARY KEY IDENTITY(1,1),
    Diagonal TINYINT NOT NULL CHECK(Diagonal between 4 and 54),
    Price SMALLMONEY NOT NULL CHECK(Price>0)
);

--Таблица процессоров
CREATE TABLE Resources.CPU
(
    CPUId SMALLINT PRIMARY KEY IDENTITY(1,1),
    QuantityCore TINYINT NOT NULL CHECK(QuantityCore between 1 and 24) DEFAULT 4,
    Frequency SMALLINT NOT NULL CHECK(Frequency between 500 and 16000) DEFAULT 2700,
    --Mhz    
    Price SMALLMONEY NOT NULL CHECK(Price>0)
    --Rub
);

--Таблица видеокарт
CREATE TABLE Resources.GPU
(
    GPUId SMALLINT PRIMARY KEY IDENTITY(1,1),
    Frequency SMALLINT NOT NULL CHECK (Frequency between 100 and 2000) DEFAULT 750,
    --Mhz    
    Memory SMALLINT NOT NULL CHECK(Memory between 64  and 12288) DEFAULT 1024,
    --Mb
    Price SMALLMONEY NOT NULL CHECK(Price>0)
);

--Таблица носители
CREATE TABLE Resources.StorageDevices
(
    StorageDeviceId SMALLINT PRIMARY KEY IDENTITY(1,1),
    SpeedWrite SMALLINT NOT NULL CHECK(SpeedWrite>0),
    SpeedRead SMALLINT NOT NULL CHECK(SpeedRead >0),
    Size SMALLINT NOT NULL CHECK(Size between 64 and 1024),
    --Gb
    Price SMALLMONEY NOT NULL CHECK(Price>0)
);

--Таблица с печатающими устройствами
CREATE TABLE Resources.Printers
(
    PrinterId SMALLINT PRIMARY KEY IDENTITY(1,1),
    Type NVARCHAR(7) NOT NULL CHECK(Type in('Плоттер','Принтер','Сканер','МФУ')),
    Speed TINYINT NOT NULL CHECK(Speed>0),
    --страницы/минута
    Price SMALLMONEY NOT NULL CHECK(Price>0)
);

--Таблица с устройствам ввода
CREATE TABLE Resources.InputDevices
(
    InputDevicesId SMALLINT PRIMARY KEY IDENTITY(1,1),
    Type NVARCHAR(10) NOT NULL CHECK(Type in('Клавиатура','Мышка')),
    Price SMALLMONEY NOT NULL CHECK(Price>0)
);

--Таблица оперативной памяти
CREATE TABLE Resources.RAM
(
    RAMId SMALLINT PRIMARY KEY IDENTITY(1,1),
    Size TINYINT NOT NULL CHECK(Size between 1 and 64),
    --GB
    Frequency SMALLINT NOT NULL CHECK(Frequency between 200 and 3333),--Mhz
    Price SMALLMONEY NOT NULL CHECK(Price>0)
);

--Таблица техническое обеспечение
CREATE TABLE Resources.TechnicalSupports
(
    TechnicalSupportId SMALLINT PRIMARY KEY IDENTITY(1,1),
    CPUId SMALLINT REFERENCES Resources.CPU(CPUId) NOT NULL,
    RAMId SMALLINT REFERENCES Resources.RAM(RAMId) NOT NULL,
    GPUId SMALLINT REFERENCES Resources.GPU(GPUId) NOT NULL,
    StorageDeviceId SMALLINT REFERENCES Resources.StorageDevices(StorageDeviceId) NOT NULL,
    MonitorId SMALLINT REFERENCES Resources.Monitors(MonitorId) NULL,
);

--Таблица связи технического обеспечения с устройствами ввода
CREATE TABLE Resources.TechnicalSupportsInputDevices
(
    TechnicalSupportId SMALLINT REFERENCES Resources.TechnicalSupports(TechnicalSupportId),
    InputDevicesId SMALLINT REFERENCES Resources.InputDevices(InputDevicesId),
    PRIMARY KEY(TechnicalSupportId, InputDevicesId)
);

--Таблица связи технического обеспечения с печатающими устройствами
CREATE TABLE Resources.TechnicalSupportsPrinters
(
    TechnicalSupportId SMALLINT REFERENCES Resources.TechnicalSupports(TechnicalSupportId),
    PrinterId SMALLINT REFERENCES Resources.Printers(PrinterId),
    PRIMARY KEY(TechnicalSupportId, PrinterId)
);
GO

CREATE SCHEMA Processes
GO

--Таблица процессов
CREATE TABLE Processes.Processes
(
    ProcessId HIERARCHYID PRIMARY KEY CLUSTERED,
    Name NVARCHAR(MAX) NOT NULL CHECK(LEN(Name)>0),
    AllTime DATETIME2(0) NOT NULL DEFAULT '01/01/0001 00:00:00',
    Accidents DATETIME2(0) NOT NULL DEFAULT '01/01/0001 00:00:00',
    FullPrice MONEY NOT NULL CHECK(FullPrice>0)
);

-- Таблица процедур
CREATE TABLE Processes.Procedures
(
    ProcedureId INT PRIMARY KEY IDENTITY(1,1),
    ProcessId HIERARCHYID REFERENCES Processes.Processes(ProcessId) NULL,
    Name NVARCHAR(MAX) NOT NULL CHECK(LEN(Name)>0),
    --Type NVARCHAR(15) NOT NULL 
);

--Таблица связи процевур и информационого обеспечения
CREATE TABLE Processes.ProceduresInformationSupports
(
    ProcedureId INT REFERENCES Processes.Procedures(ProcedureId),
    InformationSupportId SMALLINT REFERENCES Resources.InformationSupports(InformationSupportId),
    PRIMARY KEY(ProcedureId,InformationSupportId)
);

--Таблица связи процедур и програмнного обеспечения
CREATE TABLE Processes.ProceduresSoftware
(
    ProcedureId INT REFERENCES Processes.Procedures(ProcedureId),
    SoftwareId SMALLINT REFERENCES Resources.Softwares(SoftwareId),
    PRIMARY KEY(ProcedureId,SoftwareId)
);

--Таблица связи процедур и рабочих
CREATE TABLE Processes.ProceduresWorkers
(
    ProcedureId INT REFERENCES Processes.Procedures(ProcedureId),
    WorkerId SMALLINT REFERENCES Resources.Workers(WorkerId),
    PRIMARY KEY(ProcedureId,WorkerId)
);

--Таблица связи технического обеспечения с процедурами
CREATE TABLE Processes.ProceduresTechnicalSupports
(
    ProcedureId INT REFERENCES Processes.Procedures (ProcedureId),
    TechnicalSupportId SMALLINT REFERENCES Resources.TechnicalSupports(TechnicalSupportId),
    PRIMARY KEY(ProcedureId,TechnicalSupportId)
);
GO

CREATE PROCEDURE Resources.CPU_Create
    @QuantityCore TINYINT=4,
    @Frequency SMALLINT=2700,
    @Price SMALLMONEY
AS
BEGIN
    INSERT Resources.CPU
        (QuantityCore,Frequency,Price)
    VALUES
        (@QuantityCore, @Frequency, @Price);
    SELECT scope_identity()
END
GO

CREATE PROCEDURE Resources.CPU_Update
    @CPUId SMALLINT,
    @QuantityCore TINYINT,
    @Frequency SMALLINT,
    @Price SMALLMONEY
AS
BEGIN
    UPDATE Resources.CPU
    SET QuantityCore=@QuantityCore, Frequency=@Frequency,Price=@Price
    WHERE CPUId=@CPUId
END
GO

CREATE PROCEDURE Resources.CPU_Delete
    @CPUId SMALLINT
AS
DELETE Resources.CPU WHERE CPUId=@CPUId
GO

CREATE PROCEDURE Resources.CPU_Getall
AS
SELECT *
FROM Resources.CPU
GO

CREATE PROCEDURE Resources.CPU_Get
    @TechnicalSupportId SMALLINT
AS
BEGIN
    SELECT cpu.CPUId, cpu.QuantityCore, cpu.Frequency, cpu.Price
    FROM Resources.CPU AS cpu JOIN Resources.TechnicalSupports AS ts ON cpu.CPUId=ts.CPUId
    WHERE ts.TechnicalSupportId=@TechnicalSupportId
END
GO



CREATE PROCEDURE Resources.GPU_Create
    @Frequency SMALLINT=750,
    @Memory SMALLINT=1024,
    @Price SMALLMONEY
AS
BEGIN
    INSERT Resources.GPU
        (Frequency,Memory,Price)
    VALUES
        (@Frequency, @Memory, @Price);
    SELECT scope_identity()
END
GO

CREATE PROCEDURE Resources.GPU_Update
    @GPUId SMALLINT,
    @Frequency SMALLINT,
    @Memory SMALLINT,
    @Price SMALLMONEY
AS
BEGIN
    UPDATE Resources.GPU
    SET Frequency=@Frequency,Memory=@Memory,Price=@Price
    WHERE GPUId=@GPUId
END
GO

CREATE PROCEDURE Resources.GPU_Delete
    @GPUId SMALLINT
AS
DELETE Resources.GPU WHERE GPUId=@GPUId
GO

CREATE PROCEDURE Resources.GPU_Getall
AS
SELECT *
FROM Resources.GPU
GO

CREATE PROCEDURE Resources.GPU_Get
    @TechnicalSupportId SMALLINT
AS
BEGIN
    SELECT gpu.GPUId, gpu.Frequency, gpu.Memory, gpu.Price
    FROM Resources.GPU AS gpu JOIN Resources.TechnicalSupports AS ts ON gpu.GPUId=ts.GPUId
    WHERE ts.TechnicalSupportId=@TechnicalSupportId
END
GO



CREATE PROCEDURE Resources.InformationSupports_Create
    @MultiClientUse BIT=0,
    @Type NVARCHAR(11),
    @Price SMALLMONEY
AS
BEGIN
    INSERT Resources.InformationSupports
        (MultiClientUse,Type,Price)
    VALUES
        (@MultiClientUse, @Type, @Price)
    SELECT scope_identity()
END
GO

CREATE PROCEDURE Resources.InformationSupports_Update
    @InformationSupportId SMALLINT,
    @MultiClientUse BIT,
    @Type NVARCHAR(11),
    @Price SMALLMONEY
AS
BEGIN
    UPDATE Resources.InformationSupports
    SET MultiClientUse=@MultiClientUse,Type=@Type,Price=@Price
    WHERE InformationSupportId=@InformationSupportId
END
GO


CREATE PROCEDURE Resources.InformationSupports_Delete
    @InformationSupportId SMALLINT
AS
BEGIN
    DELETE Resources.InformationSupports WHERE InformationSupportId=@InformationSupportId
END
GO

CREATE PROCEDURE Resources.InformationSupports_Getall
AS
BEGIN
    SELECT *
    FROM Resources.InformationSupports
END
GO

CREATE PROCEDURE Resources.InformationSupports_Get
    @ProcedureId INT
AS
BEGIN
    SELECT RIS.InformationSupportId, RIS.MultiClientUse, RIS.Type, RIS.Price
    FROM Resources.InformationSupports AS RIS JOIN Processes.ProceduresInformationSupports AS PPIS ON RIS.InformationSupportId=PPIS.InformationSupportId
    WHERE PPIS.ProcedureId=@ProcedureId
END
GO



CREATE PROCEDURE Resources.Softwares_Create
    @Type NVARCHAR(8),
    @Name NVARCHAR(50),
    @LicenseForm NVARCHAR(18),
    @LicenseStatus NVARCHAR(15),
    @Price SMALLMONEY
AS
BEGIN
    INSERT Resources.Softwares
        (Type,Name,LicenseForm,LicenseStatus,Price)
    VALUES
        (@Type, @Name, @LicenseForm, @LicenseStatus, @Price)
    SELECT scope_identity()
END
GO

CREATE PROCEDURE Resources.Softwares_Update
    @SoftwareId SMALLINT,
    @Type NVARCHAR(8),
    @Name NVARCHAR(50),
    @LicenseForm NVARCHAR(18),
    @LicenseStatus NVARCHAR(15),
    @Price SMALLMONEY
AS
BEGIN
    UPDATE Resources.Softwares
    SET Type=@Type, Name=@Name,LicenseForm=@LicenseForm,LicenseStatus=@LicenseStatus,Price=@Price
    WHERE SoftwareId=@SoftwareId
END
GO

CREATE PROCEDURE Resources.Softwares_Delete
    @SoftwareId SMALLINT
AS
BEGIN
    DELETE Resources.Softwares WHERE SoftwareId=@SoftwareId
END
GO

CREATE PROCEDURE Resources.Softwares_Getall
AS
BEGIN
    SELECT *
    FROM Resources.Softwares
END
GO

CREATE PROCEDURE Resources.Softwares_Get
    @ProcedureId INT
AS
BEGIN
    SELECT RS.SoftwareId, RS.Type, RS.Name, RS.LicenseForm, RS.LicenseStatus, RS.Price
    FROM Resources.Softwares AS RS JOIN Processes.ProceduresSoftware AS PPS ON RS.SoftwareId=PPS.SoftwareId
    WHERE PPS.ProcedureId=@ProcedureId
END
GO




CREATE PROCEDURE Resources.Monitors_Create
    @Diagonal TINYINT,
    @Price SMALLMONEY
AS
BEGIN
    INSERT Resources.Monitors
        (Diagonal,Price)
    VALUES(@Diagonal, @Price)
    SELECT scope_identity()
END
GO

CREATE PROCEDURE Resources.Monitors_Update
    @MonitorId SMALLINT,
    @Diagonal TINYINT,
    @Price SMALLMONEY
AS
BEGIN
    UPDATE Resources.Monitors
    SET Diagonal=@Diagonal,Price=@Price
    WHERE MonitorId=@MonitorId
END
GO

CREATE PROCEDURE Resources.Monitors_Delete
    @MonitorId SMALLINT
AS
BEGIN
    DELETE Resources.Monitors WHERE MonitorId=@MonitorId
END
GO

CREATE PROCEDURE Resources.Monitors_Getall
AS
BEGIN
    select *
    FROM Resources.Monitors
END
GO

CREATE PROCEDURE Resources.Monitors_Get
    @TechnicalSupportId SMALLINT
AS
BEGIN
    SELECT RM.MonitorId, RM.Diagonal, RM.Price
    FROM Resources.Monitors AS RM JOIN Resources.TechnicalSupports AS RTS ON RM.MonitorId=RTS.MonitorId
    WHERE RTS.TechnicalSupportId=@TechnicalSupportId
END
GO



CREATE PROCEDURE Resources.StorageDevices_Create
    @SpeedWrite SMALLINT,
    @SpeedRead SMALLINT,
    @Size SMALLINT,
    @Price SMALLMONEY
AS
BEGIN
    INSERT Resources.StorageDevices
        (SpeedWrite,SpeedRead,Size,Price)
    VALUES
        (@SpeedWrite, @SpeedRead, @Size, @Price)
    SELECT scope_identity()
END
GO

CREATE PROCEDURE Resources.StorageDevices_Update
    @StorageDeviceId SMALLINT,
    @SpeedWrite SMALLINT,
    @SpeedRead SMALLINT,
    @Size SMALLINT,
    @Price SMALLMONEY
AS
BEGIN
    UPDATE Resources.StorageDevices
    SET SpeedWrite=@SpeedWrite,SpeedRead=@SpeedRead,Size=@Size,Price=@Price
    WHERE StorageDeviceId=@StorageDeviceId
END
GO

CREATE PROCEDURE Resources.StorageDevices_Delete
    @StorageDeviceId SMALLINT
AS
BEGIN
    DELETE Resources.StorageDevices WHERE StorageDeviceId=@StorageDeviceId
END
GO

CREATE PROCEDURE Resources.StorageDevices_Getall
AS
BEGIN
    SELECT *
    FROM Resources.StorageDevices
END
GO

CREATE PROCEDURE Resources.StorageDevices_Get
    @TechnicalSupportId SMALLINT
AS
BEGIN
    SELECT RSD.StorageDeviceId, RSD.SpeedWrite, RSD.SpeedRead, RSD.Size, RSD.Price
    FROM Resources.StorageDevices AS RSD JOIN Resources.TechnicalSupports AS RTS ON RSD.StorageDeviceId=RTS.StorageDeviceId
    WHERE RTS.TechnicalSupportId=@TechnicalSupportId
END 
GO




CREATE PROCEDURE Resources.Printers_Create
    @Type NVARCHAR(7),
    @Speed TINYINT,
    @Price SMALLMONEY
AS
BEGIN
    INSERT Resources.Printers
        (Type,Speed,Price)
    VALUES
        (@Type, @Speed, @Price)
    SELECT scope_identity()
END
GO

CREATE PROCEDURE Resources.Printers_Update
    @PrinterId SMALLINT,
    @Type NVARCHAR(7),
    @Speed TINYINT,
    @Price SMALLMONEY
AS
BEGIN
    UPDATE Resources.Printers
    SET Type=@Type,Speed=@Speed,Price=@Price
    WHERE PrinterId=@PrinterId
END
GO

CREATE PROCEDURE Resources.Printers_Delete
    @PrinterId SMALLINT
AS
BEGIN
    DELETE Resources.Printers WHERE PrinterId=@PrinterId
END
GO

CREATE PROCEDURE Resources.Printers_Getall
AS
BEGIN
    SELECT *
    FROM Resources.Printers
END
GO

CREATE PROCEDURE Resources.Printers_Get
    @TechnicalSupportId SMALLINT
AS
BEGIN
    SELECT RP.PrinterId, RP.Type, RP.Speed, RP.Price
    FROM Resources.Printers AS RP JOIN Resources.TechnicalSupportsPrinters AS RTSP ON RP.PrinterId=RTSP.PrinterId
    WHERE RTSP.TechnicalSupportId=@TechnicalSupportId
END
GO





CREATE PROCEDURE Resources.InputDevices_Create
    @Type NVARCHAR(10),
    @Price SMALLMONEY
AS
BEGIN
    INSERT Resources.InputDevices
        (Type,Price)
    VALUES
        (@Type, @Price)
    SELECT scope_identity()
END
GO

CREATE PROCEDURE Resources.InputDevices_Update
    @InputDevicesId SMALLINT,
    @Type NVARCHAR(10),
    @Price SMALLMONEY
AS
BEGIN
    UPDATE Resources.InputDevices
    SET Type=@Type,Price=@Price
    WHERE InputDevicesId=@InputDevicesId
END
GO

CREATE PROCEDURE Resources.InputDevices_Delete
    @InputDevicesId SMALLINT
AS
BEGIN
    DELETE Resources.InputDevices WHERE InputDevicesId=@InputDevicesId
END
GO


CREATE PROCEDURE Resources.InputDevices_Getall
AS
BEGIN
    SELECT *
    FROM Resources.InputDevices
END
GO

CREATE PROCEDURE Resources.InputDevices_Get
    @TechnicalSupportId SMALLINT
AS
BEGIN
    SELECT RID.InputDevicesId, RID.Type, RID.Price
    FROM Resources.InputDevices AS RID JOIN Resources.TechnicalSupportsInputDevices AS RTSID ON RID.InputDevicesId=RTSID.InputDevicesId
    WHERE RTSID.TechnicalSupportId=@TechnicalSupportId
END
GO



CREATE PROCEDURE Resources.RAM_Create
    @Size TINYINT,
    @Frequency SMALLINT,
    @Price SMALLMONEY
AS
BEGIN
    INSERT Resources.RAM
        (Size,Frequency,Price)
    VALUES(@Size, @Frequency, @Price)
    SELECT scope_identity()
END
GO

CREATE PROCEDURE Resources.RAM_Update
    @RAMId SMALLINT,
    @Size TINYINT,
    @Frequency SMALLINT,
    @Price SMALLMONEY
AS
BEGIN
    UPDATE Resources.RAM
    SET Size=@Size,Frequency=@Frequency,Price=@Price
    WHERE RAMId=@RAMId
END
GO

CREATE PROCEDURE Resources.RAM_Delete
    @RAMId SMALLINT
AS
BEGIN
    DELETE Resources.RAM WHERE RAMId=@RAMId
END
GO

CREATE PROCEDURE Resources.RAM_Getall
AS
BEGIN
    SELECT *
    FROM Resources.RAM
END
GO

CREATE PROCEDURE Resources.RAM_Get
    @TechnicalSupportId SMALLINT
AS
BEGIN
    SELECT RR.RAMId, RR.Size, RR.Frequency, RR.Price
    FROM Resources.RAM AS RR JOIN Resources.TechnicalSupports AS RTS ON RR.RAMId=RTS.RAMId
    WHERE RTS.TechnicalSupportId=@TechnicalSupportId
END
GO

