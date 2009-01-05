
CREATE TABLE cuyahoga_user(
userid int identity(1,1) NOT NULL CONSTRAINT PK_cuyahoga_user1 PRIMARY KEY,
username varchar(50) NOT NULL,
password varchar(100) NOT NULL,
firstname varchar(100) NULL,
lastname varchar(100) NULL,
email varchar(100) NOT NULL,
website varchar(100) NULL,
timezone int DEFAULT 0 NOT NULL,
isactive bit NULL,
lastlogin datetime NULL,
lastip varchar(40) NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_cuyahoga_user1 UNIQUE(username))
go


CREATE TABLE cuyahoga_role(
roleid int identity(1,1) NOT NULL CONSTRAINT PK_cuyahoga_role1 PRIMARY KEY,
name varchar(50) NOT NULL,
permissionlevel int DEFAULT 1 NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_cuyahoga_role1 UNIQUE(name))
go


CREATE TABLE cuyahoga_userrole(
userroleid int identity(1,1) NOT NULL CONSTRAINT PK_cuyahoga_userrole1 PRIMARY KEY,
userid int NOT NULL,
roleid int NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go


CREATE TABLE cuyahoga_template(
templateid int identity(1,1) NOT NULL CONSTRAINT PK_cuyahoga_template1 PRIMARY KEY,
name varchar(100) NOT NULL,
basepath varchar(100) NOT NULL,
templatecontrol varchar(50) NOT NULL,
css varchar(100) NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go


CREATE TABLE cuyahoga_moduletype(
moduletypeid int identity(1,1) NOT NULL CONSTRAINT PK_cuyahoga_moduletype1 PRIMARY KEY,
name varchar(100) NOT NULL,
assemblyname varchar(100) NULL,
classname varchar(255) NOT NULL,
path varchar(255) NOT NULL,
editpath varchar(255) NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_cuyahoga_moduletype1 UNIQUE(classname))
go


CREATE TABLE cuyahoga_modulesetting(
modulesettingid int identity(1,1) NOT NULL CONSTRAINT PK_cuyahoga_modulesetting1 PRIMARY KEY,
moduletypeid int NOT NULL,
name varchar(50) NOT NULL,
friendlyname varchar(50) NOT NULL,
settingdatatype varchar(100) NOT NULL,
iscustomtype bit NOT NULL,
isrequired bit NOT NULL)
go

CREATE UNIQUE INDEX IDX_cuyahoga_modulesetting_1 ON cuyahoga_modulesetting (moduletypeid,name)
go

CREATE TABLE cuyahoga_site(
siteid int identity(1,1) NOT NULL CONSTRAINT PK_cuyahoga_site1 PRIMARY KEY,
templateid int NULL,
roleid int NOT NULL,
name varchar(100) NOT NULL,
homeurl varchar(100) NOT NULL,
defaultculture varchar(8) NOT NULL,
defaultplaceholder varchar(100) NULL,
webmasteremail varchar(100) NOT NULL,
usefriendlyurls bit NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_cuyahoga_site1 UNIQUE(name))
go


CREATE TABLE cuyahoga_node(
nodeid int identity(1,1) NOT NULL CONSTRAINT PK_cuyahoga_node1 PRIMARY KEY,
parentnodeid int NULL,
templateid int NULL,
siteid int NOT NULL,
title varchar(255) NOT NULL,
shortdescription varchar(255) NOT NULL,
position int DEFAULT 0 NOT NULL,
culture varchar(8) NOT NULL,
showinnavigation bit NOT NULL,
linkurl varchar(255) NULL,
linktarget int NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go

CREATE UNIQUE INDEX IDX_cuyahoga_node_shortdescription_siteid ON cuyahoga_node (shortdescription,siteid)
go

CREATE TABLE cuyahoga_menu(
menuid int identity(1,1) NOT NULL CONSTRAINT PK_cuyahoga_menu1 PRIMARY KEY,
rootnodeid int NOT NULL,
name varchar(50) NOT NULL,
placeholder varchar(50) NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go


CREATE TABLE cuyahoga_menunode(
menunodeid int identity(1,1) NOT NULL CONSTRAINT PK_cuyahoga_menunode1 PRIMARY KEY,
menuid int NOT NULL,
nodeid int NOT NULL,
position int NOT NULL)
go


CREATE TABLE cuyahoga_sitealias(
sitealiasid int identity(1,1) NOT NULL CONSTRAINT PK_cuyahoga_sitealias1 PRIMARY KEY,
siteid int NOT NULL,
nodeid int NULL,
url varchar(100) NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go


CREATE TABLE cuyahoga_section(
sectionid int identity(1,1) NOT NULL CONSTRAINT PK_cuyahoga_section1 PRIMARY KEY,
nodeid int NULL,
moduletypeid int NOT NULL,
title varchar(100) NOT NULL,
showtitle bit DEFAULT 1 NOT NULL,
placeholder varchar(100) NULL,
position int DEFAULT 0 NOT NULL,
cacheduration int NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go


CREATE TABLE cuyahoga_sectionsetting(
sectionsettingid int identity(1,1) NOT NULL CONSTRAINT PK_cuyahoga_sectionsetting1 PRIMARY KEY,
sectionid int NOT NULL,
name varchar(50) NOT NULL,
value varchar(100) NULL)
go

CREATE UNIQUE INDEX IDX_cuyahoga_sectionsetting_1 ON cuyahoga_sectionsetting (sectionid,name)
go

CREATE TABLE cuyahoga_noderole(
noderoleid int identity(1,1) NOT NULL CONSTRAINT PK_cuyahoga_noderole1 PRIMARY KEY,
nodeid int NOT NULL,
roleid int NOT NULL,
viewallowed bit NOT NULL,
editallowed bit NOT NULL)
go

CREATE UNIQUE INDEX IDX_cuyahoga_noderole_1 ON cuyahoga_noderole (nodeid,roleid)
go

CREATE TABLE cuyahoga_sectionrole(
sectionroleid int identity(1,1) NOT NULL CONSTRAINT PK_cuyahoga_sectionrole1 PRIMARY KEY,
sectionid int NOT NULL,
roleid int NOT NULL,
viewallowed bit NOT NULL,
editallowed bit NOT NULL)
go

CREATE UNIQUE INDEX IDX_cuyahoga_sectionrole_1 ON cuyahoga_sectionrole (roleid,sectionid)
go

CREATE TABLE cuyahoga_version(
versionid int identity(1,1) NOT NULL CONSTRAINT PK_cuyahoga_version PRIMARY KEY,
assembly varchar(255) NOT NULL,
major int NOT NULL,
minor int NOT NULL,
patch int NOT NULL)

go



ALTER TABLE cuyahoga_userrole
ADD CONSTRAINT FK_cuyahoga_userrole_1 
FOREIGN KEY (roleid) REFERENCES cuyahoga_role (roleid)
go

ALTER TABLE cuyahoga_userrole
ADD CONSTRAINT FK_cuyahoga_userrole_2 
FOREIGN KEY (userid) REFERENCES cuyahoga_user (userid)
go




ALTER TABLE cuyahoga_modulesetting
ADD CONSTRAINT FK_cuyahoga_modulesetting_1 
FOREIGN KEY (moduletypeid) REFERENCES cuyahoga_moduletype (moduletypeid)
go


ALTER TABLE cuyahoga_site
ADD CONSTRAINT FK_cuyahoga_site_1 
FOREIGN KEY (roleid) REFERENCES cuyahoga_role (roleid)
go

ALTER TABLE cuyahoga_site
ADD CONSTRAINT FK_cuyahoga_site_2 
FOREIGN KEY (templateid) REFERENCES cuyahoga_template (templateid)
go


ALTER TABLE cuyahoga_node
ADD CONSTRAINT FK_cuyahoga_node_1 
FOREIGN KEY (parentnodeid) REFERENCES cuyahoga_node (nodeid)
go

ALTER TABLE cuyahoga_node
ADD CONSTRAINT FK_cuyahoga_node_2 
FOREIGN KEY (siteid) REFERENCES cuyahoga_site (siteid)
go

ALTER TABLE cuyahoga_node
ADD CONSTRAINT FK_cuyahoga_node_3 
FOREIGN KEY (templateid) REFERENCES cuyahoga_template (templateid)
go


ALTER TABLE cuyahoga_menu
ADD CONSTRAINT FK_cuyahoga_menu_1 
FOREIGN KEY (rootnodeid) REFERENCES cuyahoga_node (nodeid)
go


ALTER TABLE cuyahoga_menunode
ADD CONSTRAINT FK_cuyahoga_menunode_1 
FOREIGN KEY (menuid) REFERENCES cuyahoga_menu (menuid)
go

ALTER TABLE cuyahoga_menunode
ADD CONSTRAINT FK_cuyahoga_menunode_2 
FOREIGN KEY (nodeid) REFERENCES cuyahoga_node (nodeid)
go


ALTER TABLE cuyahoga_sitealias
ADD CONSTRAINT FK_cuyahoga_sitealias_1 
FOREIGN KEY (nodeid) REFERENCES cuyahoga_node (nodeid)
go

ALTER TABLE cuyahoga_sitealias
ADD CONSTRAINT FK_cuyahoga_sitealias_2 
FOREIGN KEY (siteid) REFERENCES cuyahoga_site (siteid)
go


ALTER TABLE cuyahoga_section
ADD CONSTRAINT FK_cuyahoga_section_1 
FOREIGN KEY (moduletypeid) REFERENCES cuyahoga_moduletype (moduletypeid)
go

ALTER TABLE cuyahoga_section
ADD CONSTRAINT FK_cuyahoga_section_2 
FOREIGN KEY (nodeid) REFERENCES cuyahoga_node (nodeid)
go


ALTER TABLE cuyahoga_sectionsetting
ADD CONSTRAINT FK_cuyahoga_sectionsetting_1 
FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid)
go


ALTER TABLE cuyahoga_noderole
ADD CONSTRAINT FK_cuyahoga_noderole_1 
FOREIGN KEY (nodeid) REFERENCES cuyahoga_node (nodeid)
go

ALTER TABLE cuyahoga_noderole
ADD CONSTRAINT FK_cuyahoga_noderole_2 
FOREIGN KEY (roleid) REFERENCES cuyahoga_role (roleid)
go


ALTER TABLE cuyahoga_sectionrole
ADD CONSTRAINT FK_cuyahoga_sectionrole_1 
FOREIGN KEY (roleid) REFERENCES cuyahoga_role (roleid)
go

ALTER TABLE cuyahoga_sectionrole
ADD CONSTRAINT FK_cuyahoga_sectionrole_2 
FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid)
go

-- DATA


SET IDENTITY_INSERT cuyahoga_role ON

GO

INSERT INTO cuyahoga_role (roleid, name, inserttimestamp, updatetimestamp, permissionlevel) VALUES (3, 'Authenticated user', '2004-01-04 16:34:50.271', '2004-06-25 00:59:02.822', 2)
INSERT INTO cuyahoga_role (roleid, name, inserttimestamp, updatetimestamp, permissionlevel) VALUES (2, 'Editor', '2004-01-04 16:34:25.669', '2004-06-25 00:59:08.256', 6)
INSERT INTO cuyahoga_role (roleid, name, inserttimestamp, updatetimestamp, permissionlevel) VALUES (1, 'Administrator', '2004-01-04 16:33:42.255', '2004-09-19 17:08:47.248', 14)
INSERT INTO cuyahoga_role (roleid, name, inserttimestamp, updatetimestamp, permissionlevel) VALUES (4, 'Anonymous user', '2004-01-04 16:35:10.766', '2004-07-16 21:18:09.017', 1)

GO

SET IDENTITY_INSERT cuyahoga_role OFF

GO

SET IDENTITY_INSERT cuyahoga_template ON

GO

INSERT INTO cuyahoga_template (templateid, [name], basepath, templatecontrol, css, inserttimestamp, updatetimestamp) VALUES (1, 'Cuyahoga Home', 'Templates/Classic', 'CuyahogaHome.ascx', 'red.css', '2004-01-26 21:52:52.365', '2004-01-26 21:52:52.365')
INSERT INTO cuyahoga_template (templateid, [name], basepath, templatecontrol, css, inserttimestamp, updatetimestamp) VALUES (2, 'Cuyahoga Standard', 'Templates/Classic', 'CuyahogaStandard.ascx', 'red.css', '2004-01-26 21:52:52.365', '2004-01-26 21:52:52.365')
INSERT INTO cuyahoga_template (templateid, [name], basepath, templatecontrol, css, inserttimestamp, updatetimestamp) VALUES (3, 'Cuyahoga New', 'Templates/Default', 'CuyahogaNew.ascx', 'red-new.css', '2004-01-26 21:52:52.365', '2004-01-26 21:52:52.365')
INSERT INTO cuyahoga_template (templateid, [name], basepath, templatecontrol, css, inserttimestamp, updatetimestamp) VALUES (4, 'Another Red', 'Templates/AnotherRed', 'Cuyahoga.ascx', 'red.css', '2004-01-26 21:52:52.365', '2004-01-26 21:52:52.365')

GO

SET IDENTITY_INSERT cuyahoga_template OFF

GO


INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Core', 0, 8, 2)
GO
