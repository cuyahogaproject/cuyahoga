

CREATE TABLE Cuyahoga_User(
UserId INT NOT NULL AUTO_INCREMENT,
Username VARCHAR(50) NOT NULL,
Password VARCHAR(100) NOT NULL,
Firstname VARCHAR(100),
Lastname VARCHAR(100),
Email VARCHAR(100) NOT NULL,
Website VARCHAR(100),
IsActive TINYINT,
LastLogin DATETIME,
LastIp VARCHAR(40),
InsertTimestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP()  NOT NULL,
UpdateTimestamp DATETIME  NOT NULL,
PRIMARY KEY (UserId),
UNIQUE UC_UserId (UserId),
UNIQUE UC_Username (Username));


CREATE TABLE Cuyahoga_Role(
RoleId INT NOT NULL AUTO_INCREMENT,
Name VARCHAR(50) NOT NULL,
PermissionLevel INT DEFAULT 1 NOT NULL,
InsertTimestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP()  NOT NULL,
UpdateTimestamp DATETIME  NOT NULL,
PRIMARY KEY (RoleId),
UNIQUE UC_RoleId (RoleId),
UNIQUE UC_Name (Name));


CREATE TABLE Cuyahoga_UserRole(
UserRoleId INT NOT NULL AUTO_INCREMENT,
UserId INT NOT NULL,
RoleId INT NOT NULL,
InsertTimestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP()  NOT NULL,
UpdateTimestamp DATETIME  NOT NULL,
FOREIGN KEY (RoleId) REFERENCES Cuyahoga_Role (RoleId),
FOREIGN KEY (UserId) REFERENCES Cuyahoga_User (UserId),
PRIMARY KEY (UserRoleId),
UNIQUE UC_UserRoleId (UserRoleId));


CREATE TABLE Cuyahoga_Template(
TemplateId INT NOT NULL AUTO_INCREMENT,
Name VARCHAR(100) NOT NULL,
Path VARCHAR(100) NOT NULL,
Css VARCHAR(100) NOT NULL,
InsertTimestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP()  NOT NULL,
UpdateTimestamp DATETIME  NOT NULL,
PRIMARY KEY (TemplateId),
UNIQUE UC_TemplateId (TemplateId));


CREATE TABLE Cuyahoga_ModuleType(
ModuleTypeId INT NOT NULL AUTO_INCREMENT,
Name VARCHAR(100) NOT NULL,
AssemblyName VARCHAR(100),
ClassName VARCHAR(255) NOT NULL,
Path VARCHAR(255) NOT NULL,
EditPath VARCHAR(255),
InsertTimestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP()  NOT NULL,
UpdateTimestamp DATETIME  NOT NULL,
PRIMARY KEY (ModuleTypeId),
UNIQUE UC_ClassName (ClassName));


CREATE TABLE Cuyahoga_Site(
SiteId INT NOT NULL AUTO_INCREMENT,
TemplateId INT,
RoleId INT NOT NULL,
Name VARCHAR(100) NOT NULL,
HomeUrl VARCHAR(100) NOT NULL,
DefaultCulture VARCHAR(8) NOT NULL,
DefaultPlaceholder VARCHAR(100),
WebmasterEmail VARCHAR(100) NOT NULL,
InsertTimestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP()  NOT NULL,
UpdateTimestamp DATETIME  NOT NULL,
FOREIGN KEY (RoleId) REFERENCES Cuyahoga_Role (RoleId),
FOREIGN KEY (TemplateId) REFERENCES Cuyahoga_Template (TemplateId),
PRIMARY KEY (SiteId),
UNIQUE UC_Name (Name));


CREATE TABLE Cuyahoga_Node(
NodeId INT NOT NULL AUTO_INCREMENT,
ParentNodeId INT,
TemplateId INT,
SiteId INT NOT NULL,
Title VARCHAR(255) NOT NULL,
ShortDescription VARCHAR(255) NOT NULL,
Position INT DEFAULT 0 NOT NULL,
Culture VARCHAR(8) NOT NULL,
InsertTimestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP()  NOT NULL,
UpdateTimestamp DATETIME  NOT NULL,
FOREIGN KEY (ParentNodeId) REFERENCES Cuyahoga_Node (NodeId),
FOREIGN KEY (SiteId) REFERENCES Cuyahoga_Site (SiteId),
FOREIGN KEY (TemplateId) REFERENCES Cuyahoga_Template (TemplateId),
PRIMARY KEY (NodeId),
UNIQUE UC_NodeId (NodeId),
UNIQUE UC_ShortDescription (ShortDescription));


CREATE TABLE Cuyahoga_Section(
SectionId INT NOT NULL AUTO_INCREMENT,
NodeId INT,
ModuleTypeId INT NOT NULL,
Title VARCHAR(100) NOT NULL,
ShowTitle TINYINT DEFAULT 1 NOT NULL,
Placeholder VARCHAR(100),
Position INT DEFAULT 0 NOT NULL,
CacheDuration INT,
InsertTimestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP()  NOT NULL,
UpdateTimestamp DATETIME  NOT NULL,
FOREIGN KEY (ModuleTypeId) REFERENCES Cuyahoga_ModuleType (ModuleTypeId),
FOREIGN KEY (NodeId) REFERENCES Cuyahoga_Node (NodeId),
PRIMARY KEY (SectionId),
UNIQUE UC_SectionId (SectionId));


CREATE TABLE Cuyahoga_NodeRole(
NodeRoleId INT NOT NULL AUTO_INCREMENT,
NodeId INT NOT NULL,
RoleId INT NOT NULL,
ViewAllowed TINYINT NOT NULL,
EditAllowed TINYINT NOT NULL,
FOREIGN KEY (NodeId) REFERENCES Cuyahoga_Node (NodeId),
FOREIGN KEY (RoleId) REFERENCES Cuyahoga_Role (RoleId),
PRIMARY KEY (NodeRoleId),
UNIQUE IDX_Cuyahoga_NodeRole_1 (NodeId,RoleId));


CREATE TABLE Cuyahoga_SectionRole(
SectionRoleId INT NOT NULL AUTO_INCREMENT,
SectionId INT NOT NULL,
RoleId INT NOT NULL,
ViewAllowed TINYINT NOT NULL,
EditAllowed TINYINT NOT NULL,
FOREIGN KEY (RoleId) REFERENCES Cuyahoga_Role (RoleId),
FOREIGN KEY (SectionId) REFERENCES Cuyahoga_Section (SectionId),
PRIMARY KEY (SectionRoleId),
UNIQUE IDX_Cuyahoga_SectionRole_1 (RoleId,SectionId));


CREATE TABLE Cuyahoga_ModuleSetting(
ModuleSettingId INT NOT NULL AUTO_INCREMENT,
ModuleTypeId INT NOT NULL,
Name VARCHAR(50) NOT NULL,
FriendlyName VARCHAR(50) NOT NULL,
SettingDataType VARCHAR(100) NOT NULL,
IsCustomType TINYINT NOT NULL,
IsRequired TINYINT NOT NULL,
FOREIGN KEY (ModuleTypeId) REFERENCES Cuyahoga_ModuleType (ModuleTypeId),
PRIMARY KEY (ModuleSettingId),
UNIQUE IDX_Cuyahoga_ModuleSetting_1 (ModuleTypeId,Name));


CREATE TABLE Cuyahoga_SectionSetting(
SectionSettingId INT NOT NULL AUTO_INCREMENT,
SectionId INT NOT NULL,
Name VARCHAR(50) NOT NULL,
Value VARCHAR(100),
FOREIGN KEY (SectionId) REFERENCES Cuyahoga_Section (SectionId),
PRIMARY KEY (SectionSettingId),
UNIQUE IDX_Cuyahoga_SectionSetting_1 (SectionId,Name));

