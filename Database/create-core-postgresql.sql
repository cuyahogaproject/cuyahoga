
CREATE TABLE Cuyahoga_User(
UserId serial NOT NULL CONSTRAINT UC_Cuyahoga_User1 UNIQUE CONSTRAINT PK_Cuyahoga_User1 PRIMARY KEY,
Username varchar(50) NOT NULL CONSTRAINT UC_Cuyahoga_User2 UNIQUE,
Password varchar(100) NOT NULL,
Firstname varchar(100),
Lastname varchar(100),
Email varchar(100) NOT NULL,
Website varchar(100),
IsActive bool,
LastLogin timestamp,
LastIp varchar(40),
InsertTimestamp timestamp DEFAULT current_timestamp NOT NULL,
UpdateTimestamp timestamp DEFAULT current_timestamp NOT NULL);


CREATE TABLE Cuyahoga_Role(
RoleId serial NOT NULL CONSTRAINT UC_Cuyahoga_Role1 UNIQUE CONSTRAINT PK_Cuyahoga_Role1 PRIMARY KEY,
Name varchar(50) NOT NULL CONSTRAINT UC_Cuyahoga_Role2 UNIQUE,
PermissionLevel int4 DEFAULT 1 NOT NULL,
InsertTimestamp timestamp DEFAULT current_timestamp NOT NULL,
UpdateTimestamp timestamp DEFAULT current_timestamp NOT NULL);


CREATE TABLE Cuyahoga_UserRole(
UserRoleId serial NOT NULL CONSTRAINT UC_Cuyahoga_UserRole1 UNIQUE CONSTRAINT PK_Cuyahoga_UserRole1 PRIMARY KEY,
UserId int4 NOT NULL,
RoleId int4 NOT NULL,
InsertTimestamp timestamp DEFAULT current_timestamp NOT NULL,
UpdateTimestamp timestamp DEFAULT current_timestamp NOT NULL,
CONSTRAINT FK_Cuyahoga_UserRole_1 FOREIGN KEY (RoleId) REFERENCES Cuyahoga_Role (RoleId),
CONSTRAINT FK_Cuyahoga_UserRole_2 FOREIGN KEY (UserId) REFERENCES Cuyahoga_User (UserId));


CREATE TABLE Cuyahoga_Template(
TemplateId serial NOT NULL CONSTRAINT UC_Cuyahoga_Template1 UNIQUE CONSTRAINT PK_Cuyahoga_Template1 PRIMARY KEY,
Name varchar(100) NOT NULL,
Path varchar(100) NOT NULL,
Css varchar(100) NOT NULL,
InsertTimestamp timestamp DEFAULT current_timestamp NOT NULL,
UpdateTimestamp timestamp DEFAULT current_timestamp NOT NULL);


CREATE TABLE Cuyahoga_ModuleType(
ModuleTypeId serial NOT NULL CONSTRAINT PK_Cuyahoga_ModuleType1 PRIMARY KEY,
Name varchar(100) NOT NULL,
AssemblyName varchar(100),
ClassName varchar(255) NOT NULL CONSTRAINT UC_Cuyahoga_ModuleType1 UNIQUE,
Path varchar(255) NOT NULL,
EditPath varchar(255),
InsertTimestamp timestamp DEFAULT current_timestamp NOT NULL,
UpdateTimestamp timestamp DEFAULT current_timestamp NOT NULL);


CREATE TABLE Cuyahoga_Site(
SiteId serial NOT NULL CONSTRAINT PK_Cuyahoga_Site1 PRIMARY KEY,
TemplateId int4,
RoleId int4 NOT NULL,
Name varchar(100) NOT NULL CONSTRAINT UC_Cuyahoga_Site1 UNIQUE,
HomeUrl varchar(100) NOT NULL,
DefaultCulture varchar(8) NOT NULL,
DefaultPlaceholder varchar(100),
WebmasterEmail varchar(100) NOT NULL,
InsertTimestamp timestamp DEFAULT current_timestamp NOT NULL,
UpdateTimestamp timestamp DEFAULT current_timestamp NOT NULL,
CONSTRAINT FK_Cuyahoga_Site_1 FOREIGN KEY (RoleId) REFERENCES Cuyahoga_Role (RoleId),
CONSTRAINT FK_Cuyahoga_Site_2 FOREIGN KEY (TemplateId) REFERENCES Cuyahoga_Template (TemplateId));


CREATE TABLE Cuyahoga_Node(
NodeId serial NOT NULL CONSTRAINT UC_Cuyahoga_Node1 UNIQUE CONSTRAINT PK_Cuyahoga_Node1 PRIMARY KEY,
ParentNodeId int4,
TemplateId int4,
SiteId int4 NOT NULL,
Title varchar(255) NOT NULL,
ShortDescription varchar(255) NOT NULL CONSTRAINT UC_Cuyahoga_Node2 UNIQUE,
Position int4 DEFAULT 0 NOT NULL,
Culture varchar(8) NOT NULL,
InsertTimestamp timestamp DEFAULT current_timestamp NOT NULL,
UpdateTimestamp timestamp DEFAULT current_timestamp NOT NULL,
CONSTRAINT FK_Cuyahoga_Node_1 FOREIGN KEY (ParentNodeId) REFERENCES Cuyahoga_Node (NodeId),
CONSTRAINT FK_Cuyahoga_Node_2 FOREIGN KEY (SiteId) REFERENCES Cuyahoga_Site (SiteId),
CONSTRAINT FK_Cuyahoga_Node_3 FOREIGN KEY (TemplateId) REFERENCES Cuyahoga_Template (TemplateId));


CREATE TABLE Cuyahoga_Section(
SectionId serial NOT NULL CONSTRAINT UC_Cuyahoga_Section1 UNIQUE CONSTRAINT PK_Cuyahoga_Section1 PRIMARY KEY,
NodeId int4,
ModuleTypeId int4 NOT NULL,
Title varchar(100) NOT NULL,
ShowTitle bool DEFAULT 1 NOT NULL,
Placeholder varchar(100),
Position int4 DEFAULT 0 NOT NULL,
CacheDuration int4,
InsertTimestamp timestamp DEFAULT current_timestamp NOT NULL,
UpdateTimestamp timestamp DEFAULT current_timestamp NOT NULL,
CONSTRAINT FK_Cuyahoga_Section_1 FOREIGN KEY (ModuleTypeId) REFERENCES Cuyahoga_ModuleType (ModuleTypeId),
CONSTRAINT FK_Cuyahoga_Section_2 FOREIGN KEY (NodeId) REFERENCES Cuyahoga_Node (NodeId));


CREATE TABLE Cuyahoga_NodeRole(
NodeRoleId serial NOT NULL CONSTRAINT PK_Cuyahoga_NodeRole1 PRIMARY KEY,
NodeId int4 NOT NULL,
RoleId int4 NOT NULL,
ViewAllowed bool NOT NULL,
EditAllowed bool NOT NULL,
CONSTRAINT FK_Cuyahoga_NodeRole_1 FOREIGN KEY (NodeId) REFERENCES Cuyahoga_Node (NodeId),
CONSTRAINT FK_Cuyahoga_NodeRole_2 FOREIGN KEY (RoleId) REFERENCES Cuyahoga_Role (RoleId));

CREATE UNIQUE INDEX IDX_Cuyahoga_NodeRole_1 ON Cuyahoga_NodeRole (NodeId,RoleId);

CREATE TABLE Cuyahoga_SectionRole(
SectionRoleId serial NOT NULL CONSTRAINT PK_Cuyahoga_SectionRole1 PRIMARY KEY,
SectionId int4 NOT NULL,
RoleId int4 NOT NULL,
ViewAllowed bool NOT NULL,
EditAllowed bool NOT NULL,
CONSTRAINT FK_Cuyahoga_SectionRole_1 FOREIGN KEY (RoleId) REFERENCES Cuyahoga_Role (RoleId),
CONSTRAINT FK_Cuyahoga_SectionRole_2 FOREIGN KEY (SectionId) REFERENCES Cuyahoga_Section (SectionId));

CREATE UNIQUE INDEX IDX_Cuyahoga_SectionRole_1 ON Cuyahoga_SectionRole (RoleId,SectionId);

CREATE TABLE Cuyahoga_ModuleSetting(
ModuleSettingId serial NOT NULL CONSTRAINT PK_Cuyahoga_ModuleSetting1 PRIMARY KEY,
ModuleTypeId int4 NOT NULL,
Name varchar(50) NOT NULL,
FriendlyName varchar(50) NOT NULL,
SettingDataType varchar(100) NOT NULL,
IsCustomType bool NOT NULL,
IsRequired bool NOT NULL,
CONSTRAINT FK_Cuyahoga_ModuleSetting_1 FOREIGN KEY (ModuleTypeId) REFERENCES Cuyahoga_ModuleType (ModuleTypeId));

CREATE UNIQUE INDEX IDX_Cuyahoga_ModuleSetting_1 ON Cuyahoga_ModuleSetting (ModuleTypeId,Name);

CREATE TABLE Cuyahoga_SectionSetting(
SectionSettingId serial NOT NULL CONSTRAINT PK_Cuyahoga_SectionSetting1 PRIMARY KEY,
SectionId int4 NOT NULL,
Name varchar(50) NOT NULL,
Value varchar(100),
CONSTRAINT FK_Cuyahoga_SectionSetting_1 FOREIGN KEY (SectionId) REFERENCES Cuyahoga_Section (SectionId));

CREATE UNIQUE INDEX IDX_Cuyahoga_SectionSetting_1 ON Cuyahoga_SectionSetting (SectionId,Name);
