CREATE TABLE Cuyahoga_User(
UserId serial NOT NULL UNIQUE PRIMARY KEY,
Username varchar(50) NOT NULL UNIQUE,
Password varchar(100) NOT NULL,
Firstname varchar(100),
Lastname varchar(100),
Email varchar(100) NOT NULL,
LastLogin timestamp,
LastIp varchar(40),
InsertTimestamp timestamp DEFAULT current_timestamp NOT NULL,
UpdateTimestamp timestamp DEFAULT current_timestamp(3) NOT NULL);


CREATE TABLE Cuyahoga_Role(
RoleId serial NOT NULL UNIQUE PRIMARY KEY,
Name varchar(50) NOT NULL UNIQUE,
PermissionLevel int4 DEFAULT 1 NOT NULL,
InsertTimestamp timestamp DEFAULT current_timestamp NOT NULL,
UpdateTimestamp timestamp DEFAULT current_timestamp(3) NOT NULL);


CREATE TABLE Cuyahoga_UserRole(
UserRoleId serial NOT NULL UNIQUE PRIMARY KEY,
UserId int4 NOT NULL,
RoleId int4 NOT NULL,
InsertTimestamp timestamp DEFAULT current_timestamp NOT NULL,
UpdateTimestamp timestamp DEFAULT current_timestamp(3) NOT NULL,
FOREIGN KEY (RoleId) REFERENCES Cuyahoga_Role (RoleId),
FOREIGN KEY (UserId) REFERENCES Cuyahoga_User (UserId));


CREATE TABLE Cuyahoga_Culture(
Culture varchar(8) NOT NULL PRIMARY KEY,
NeutralCulture varchar(2) NOT NULL,
Description varchar(50) NOT NULL);


CREATE TABLE Cuyahoga_Template(
TemplateId serial NOT NULL UNIQUE PRIMARY KEY,
Name varchar(100) NOT NULL,
Path varchar(100) NOT NULL,
InsertTimestamp timestamp DEFAULT current_timestamp NOT NULL,
UpdateTimestamp timestamp DEFAULT current_timestamp(3) NOT NULL);


CREATE TABLE Cuyahoga_Node(
NodeId serial NOT NULL UNIQUE PRIMARY KEY,
ParentNodeId int4,
TemplateId int4,
Culture varchar(8) NULL,
Title varchar(255) NOT NULL,
ShortDescription varchar(255) NOT NULL UNIQUE,
Position int4 DEFAULT 0 NOT NULL,
InsertTimestamp timestamp DEFAULT current_timestamp NOT NULL,
UpdateTimestamp timestamp DEFAULT current_timestamp(3) NOT NULL,
FOREIGN KEY (Culture) REFERENCES Cuyahoga_Culture (Culture),
FOREIGN KEY (ParentNodeId) REFERENCES Cuyahoga_Node (NodeId),
FOREIGN KEY (TemplateId) REFERENCES Cuyahoga_Template (TemplateId));


CREATE TABLE Cuyahoga_ModuleType(
ModuleTypeId serial NOT NULL PRIMARY KEY,
Name varchar(100) NOT NULL,
AssemblyName varchar(100),
ClassName varchar(255) NOT NULL UNIQUE,
Path varchar(255) NOT NULL,
EditPath varchar(255),
InsertTimestamp timestamp DEFAULT current_timestamp NOT NULL,
UpdateTimestamp timestamp DEFAULT current_timestamp(3) NOT NULL);


CREATE TABLE Cuyahoga_Section(
SectionId serial NOT NULL UNIQUE PRIMARY KEY,
NodeId int4,
ModuleTypeId int4 NOT NULL,
Title varchar(100) NOT NULL,
ShowTitle bool DEFAULT 1 NOT NULL,
Placeholder varchar(100),
Position int4 DEFAULT 0 NOT NULL,
CacheDuration int4,
InsertTimestamp timestamp DEFAULT current_timestamp NOT NULL,
UpdateTimestamp timestamp DEFAULT current_timestamp(3) NOT NULL,
FOREIGN KEY (ModuleTypeId) REFERENCES Cuyahoga_ModuleType (ModuleTypeId),
FOREIGN KEY (NodeId) REFERENCES Cuyahoga_Node (NodeId));


CREATE TABLE Cuyahoga_NodeRole(
NodeRoleId serial NOT NULL PRIMARY KEY,
NodeId int4 NOT NULL,
RoleId int4 NOT NULL,
ViewAllowed bool NOT NULL,
EditAllowed bool NOT NULL,
FOREIGN KEY (NodeId) REFERENCES Cuyahoga_Node (NodeId),
FOREIGN KEY (RoleId) REFERENCES Cuyahoga_Role (RoleId));

CREATE UNIQUE INDEX IDX_Cuyahoga_NodeRole_1 ON Cuyahoga_NodeRole (NodeId,RoleId);

CREATE TABLE Cuyahoga_SectionRole(
SectionRoleId serial NOT NULL PRIMARY KEY,
SectionId int4 NOT NULL,
RoleId int4 NOT NULL,
ViewAllowed bool NOT NULL,
EditAllowed bool NOT NULL,
FOREIGN KEY (RoleId) REFERENCES Cuyahoga_Role (RoleId),
FOREIGN KEY (SectionId) REFERENCES Cuyahoga_Section (SectionId));

CREATE UNIQUE INDEX IDX_Cuyahoga_SectionRole_1 ON Cuyahoga_SectionRole (RoleId,SectionId);

CREATE TABLE Cuyahoga_ModuleSetting(
ModuleSettingId serial NOT NULL PRIMARY KEY,
ModuleTypeId int4 NOT NULL,
Name varchar(50) NOT NULL,
FriendlyName varchar(50) NOT NULL,
SettingDataType varchar(100) NOT NULL,
IsCustomType bool NOT NULL,
FOREIGN KEY (ModuleTypeId) REFERENCES Cuyahoga_ModuleType (ModuleTypeId));

CREATE UNIQUE INDEX IDX_Cuyahoga_ModuleSetting_1 ON Cuyahoga_ModuleSetting (ModuleTypeId,Name);

CREATE TABLE Cuyahoga_SectionSetting(
SectionSettingId serial NOT NULL PRIMARY KEY,
SectionId int4 NOT NULL,
Name varchar(50) NOT NULL,
Value varchar(100),
FOREIGN KEY (SectionId) REFERENCES Cuyahoga_Section (SectionId));

CREATE UNIQUE INDEX IDX_Cuyahoga_SectionSetting_1 ON Cuyahoga_SectionSetting (SectionId,Name);
