CREATE TABLE Cuyahoga_User(
UserId int identity(1,1) NOT NULL CONSTRAINT PK_Cuyahoga_User1 PRIMARY KEY,
Username varchar(50) NOT NULL,
Password varchar(100) NOT NULL,
Firstname varchar(100) NULL,
Lastname varchar(100) NULL,
Email varchar(100) NOT NULL,
LastLogin datetime NULL,
LastIp varchar(40) NULL,
InsertTimestamp datetime DEFAULT current_timestamp NOT NULL,
UpdateTimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_Cuyahoga_User1 UNIQUE(UserId),
CONSTRAINT UC_Cuyahoga_User2 UNIQUE(Username))
go


CREATE TABLE Cuyahoga_Role(
RoleId int identity(1,1) NOT NULL CONSTRAINT PK_Cuyahoga_Role1 PRIMARY KEY,
Name varchar(50) NOT NULL,
PermissionLevel int DEFAULT 1 NOT NULL,
InsertTimestamp datetime DEFAULT current_timestamp NOT NULL,
UpdateTimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_Cuyahoga_Role1 UNIQUE(RoleId),
CONSTRAINT UC_Cuyahoga_Role2 UNIQUE(Name))
go


CREATE TABLE Cuyahoga_UserRole(
UserRoleId int identity(1,1) NOT NULL CONSTRAINT PK_Cuyahoga_UserRole1 PRIMARY KEY,
UserId int NOT NULL,
RoleId int NOT NULL,
InsertTimestamp datetime DEFAULT current_timestamp NOT NULL,
UpdateTimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_Cuyahoga_UserRole1 UNIQUE(UserRoleId))
go


CREATE TABLE Cuyahoga_Template(
TemplateId int identity(1,1) NOT NULL CONSTRAINT PK_Cuyahoga_Template1 PRIMARY KEY,
Name varchar(100) NOT NULL,
Path varchar(100) NOT NULL,
Css varchar(100) NOT NULL,
InsertTimestamp datetime DEFAULT current_timestamp NOT NULL,
UpdateTimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_Cuyahoga_Template1 UNIQUE(TemplateId))
go


CREATE TABLE Cuyahoga_ModuleType(
ModuleTypeId int identity(1,1) NOT NULL CONSTRAINT PK_Cuyahoga_ModuleType1 PRIMARY KEY,
Name varchar(100) NOT NULL,
AssemblyName varchar(100) NULL,
ClassName varchar(255) NOT NULL,
Path varchar(255) NOT NULL,
EditPath varchar(255) NULL,
InsertTimestamp datetime DEFAULT current_timestamp NOT NULL,
UpdateTimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_Cuyahoga_ModuleType1 UNIQUE(ClassName))
go


CREATE TABLE Cuyahoga_Site(
SiteId int identity(1,1) NOT NULL CONSTRAINT PK_Cuyahoga_Site1 PRIMARY KEY,
TemplateId int NULL,
Name varchar(100) NOT NULL,
HomeUrl varchar(100) NOT NULL,
DefaultCulture varchar(8) NOT NULL,
InsertTimestamp datetime DEFAULT current_timestamp NOT NULL,
UpdateTimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_Cuyahoga_Site1 UNIQUE(Name))
go


CREATE TABLE Cuyahoga_Node(
NodeId int identity(1,1) NOT NULL CONSTRAINT PK_Cuyahoga_Node1 PRIMARY KEY,
ParentNodeId int NULL,
TemplateId int NULL,
SiteId int NOT NULL,
Title varchar(255) NOT NULL,
ShortDescription varchar(255) NOT NULL,
Position int DEFAULT 0 NOT NULL,
Culture varchar(8) NOT NULL,
InsertTimestamp datetime DEFAULT current_timestamp NOT NULL,
UpdateTimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_Cuyahoga_Node1 UNIQUE(NodeId),
CONSTRAINT UC_Cuyahoga_Node2 UNIQUE(ShortDescription))
go


CREATE TABLE Cuyahoga_Section(
SectionId int identity(1,1) NOT NULL CONSTRAINT PK_Cuyahoga_Section1 PRIMARY KEY,
NodeId int NULL,
ModuleTypeId int NOT NULL,
Title varchar(100) NOT NULL,
ShowTitle bit DEFAULT 1 NOT NULL,
Placeholder varchar(100) NULL,
Position int DEFAULT 0 NOT NULL,
CacheDuration int NULL,
InsertTimestamp datetime DEFAULT current_timestamp NOT NULL,
UpdateTimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_Cuyahoga_Section1 UNIQUE(SectionId))
go


CREATE TABLE Cuyahoga_NodeRole(
NodeRoleId int identity(1,1) NOT NULL CONSTRAINT PK_Cuyahoga_NodeRole1 PRIMARY KEY,
NodeId int NOT NULL,
RoleId int NOT NULL,
ViewAllowed bit NOT NULL,
EditAllowed bit NOT NULL)
go

CREATE UNIQUE INDEX IDX_Cuyahoga_NodeRole_1 ON Cuyahoga_NodeRole (NodeId,RoleId)
go

CREATE TABLE Cuyahoga_SectionRole(
SectionRoleId int identity(1,1) NOT NULL CONSTRAINT PK_Cuyahoga_SectionRole1 PRIMARY KEY,
SectionId int NOT NULL,
RoleId int NOT NULL,
ViewAllowed bit NOT NULL,
EditAllowed bit NOT NULL)
go

CREATE UNIQUE INDEX IDX_Cuyahoga_SectionRole_1 ON Cuyahoga_SectionRole (RoleId,SectionId)
go

CREATE TABLE Cuyahoga_ModuleSetting(
ModuleSettingId int identity(1,1) NOT NULL CONSTRAINT PK_Cuyahoga_ModuleSetting1 PRIMARY KEY,
ModuleTypeId int NOT NULL,
Name varchar(50) NOT NULL,
FriendlyName varchar(50) NOT NULL,
SettingDataType varchar(100) NOT NULL,
IsCustomType bit NOT NULL,
IsRequired bit NOT NULL)
go

CREATE UNIQUE INDEX IDX_Cuyahoga_ModuleSetting_1 ON Cuyahoga_ModuleSetting (ModuleTypeId,Name)
go

CREATE TABLE Cuyahoga_SectionSetting(
SectionSettingId int identity(1,1) NOT NULL CONSTRAINT PK_Cuyahoga_SectionSetting1 PRIMARY KEY,
SectionId int NOT NULL,
Name varchar(50) NOT NULL,
Value varchar(100) NULL)
go

CREATE UNIQUE INDEX IDX_Cuyahoga_SectionSetting_1 ON Cuyahoga_SectionSetting (SectionId,Name)
go





ALTER TABLE Cuyahoga_UserRole
ADD CONSTRAINT FK_Cuyahoga_UserRole_1 
FOREIGN KEY (RoleId) REFERENCES Cuyahoga_Role (RoleId)
go

ALTER TABLE Cuyahoga_UserRole
ADD CONSTRAINT FK_Cuyahoga_UserRole_2 
FOREIGN KEY (UserId) REFERENCES Cuyahoga_User (UserId)
go




ALTER TABLE Cuyahoga_Site
ADD CONSTRAINT FK_Cuyahoga_Site_1 
FOREIGN KEY (TemplateId) REFERENCES Cuyahoga_Template (TemplateId)
go


ALTER TABLE Cuyahoga_Node
ADD CONSTRAINT FK_Cuyahoga_Node_1 
FOREIGN KEY (ParentNodeId) REFERENCES Cuyahoga_Node (NodeId)
go

ALTER TABLE Cuyahoga_Node
ADD CONSTRAINT FK_Cuyahoga_Node_2 
FOREIGN KEY (SiteId) REFERENCES Cuyahoga_Site (SiteId)
go

ALTER TABLE Cuyahoga_Node
ADD CONSTRAINT FK_Cuyahoga_Node_3 
FOREIGN KEY (TemplateId) REFERENCES Cuyahoga_Template (TemplateId)
go


ALTER TABLE Cuyahoga_Section
ADD CONSTRAINT FK_Cuyahoga_Section_1 
FOREIGN KEY (ModuleTypeId) REFERENCES Cuyahoga_ModuleType (ModuleTypeId)
go

ALTER TABLE Cuyahoga_Section
ADD CONSTRAINT FK_Cuyahoga_Section_2 
FOREIGN KEY (NodeId) REFERENCES Cuyahoga_Node (NodeId)
go


ALTER TABLE Cuyahoga_NodeRole
ADD CONSTRAINT FK_Cuyahoga_NodeRole_1 
FOREIGN KEY (NodeId) REFERENCES Cuyahoga_Node (NodeId)
go

ALTER TABLE Cuyahoga_NodeRole
ADD CONSTRAINT FK_Cuyahoga_NodeRole_2 
FOREIGN KEY (RoleId) REFERENCES Cuyahoga_Role (RoleId)
go


ALTER TABLE Cuyahoga_SectionRole
ADD CONSTRAINT FK_Cuyahoga_SectionRole_1 
FOREIGN KEY (RoleId) REFERENCES Cuyahoga_Role (RoleId)
go

ALTER TABLE Cuyahoga_SectionRole
ADD CONSTRAINT FK_Cuyahoga_SectionRole_2 
FOREIGN KEY (SectionId) REFERENCES Cuyahoga_Section (SectionId)
go


ALTER TABLE Cuyahoga_ModuleSetting
ADD CONSTRAINT FK_Cuyahoga_ModuleSetting_1 
FOREIGN KEY (ModuleTypeId) REFERENCES Cuyahoga_ModuleType (ModuleTypeId)
go


ALTER TABLE Cuyahoga_SectionSetting
ADD CONSTRAINT FK_Cuyahoga_SectionSetting_1 
FOREIGN KEY (SectionId) REFERENCES Cuyahoga_Section (SectionId)
go

