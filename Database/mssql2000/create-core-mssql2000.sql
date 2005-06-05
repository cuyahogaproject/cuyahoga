
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
CONSTRAINT UC_cuyahoga_user1 UNIQUE(userid),
CONSTRAINT UC_cuyahoga_user2 UNIQUE(username))
go


CREATE TABLE cuyahoga_role(
roleid int identity(1,1) NOT NULL CONSTRAINT PK_cuyahoga_role1 PRIMARY KEY,
name varchar(50) NOT NULL,
permissionlevel int DEFAULT 1 NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_cuyahoga_role1 UNIQUE(roleid),
CONSTRAINT UC_cuyahoga_role2 UNIQUE(name))
go


CREATE TABLE cuyahoga_userrole(
userroleid int identity(1,1) NOT NULL CONSTRAINT PK_cuyahoga_userrole1 PRIMARY KEY,
userid int NOT NULL,
roleid int NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_cuyahoga_userrole1 UNIQUE(userroleid))
go


CREATE TABLE cuyahoga_template(
templateid int identity(1,1) NOT NULL CONSTRAINT PK_cuyahoga_template1 PRIMARY KEY,
name varchar(100) NOT NULL,
basepath varchar(100) NOT NULL,
templatecontrol varchar(50) NOT NULL,
css varchar(100) NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_cuyahoga_template1 UNIQUE(templateid))
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
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_cuyahoga_node1 UNIQUE(nodeid),
CONSTRAINT UC_cuyahoga_node2 UNIQUE(shortdescription))
go


CREATE TABLE cuyahoga_menu(
menuid int identity(1,1) NOT NULL CONSTRAINT PK_cuyahoga_menu1 PRIMARY KEY,
rootnodeid int NOT NULL,
name varchar(50) NOT NULL,
placeholder varchar(50) NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_cuyahoga_menu1 UNIQUE(menuid))
go


CREATE TABLE cuyahoga_menunode(
menunodeid int identity(1,1) NOT NULL CONSTRAINT PK_cuyahoga_menunode1 PRIMARY KEY,
menuid int NOT NULL,
nodeid int NOT NULL,
position int NOT NULL,
CONSTRAINT UC_cuyahoga_menunode1 UNIQUE(menunodeid))
go


CREATE TABLE cuyahoga_sitealias(
sitealiasid int identity(1,1) NOT NULL CONSTRAINT PK_cuyahoga_sitealias1 PRIMARY KEY,
siteid int NOT NULL,
nodeid int NULL,
url varchar(100) NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_cuyahoga_sitealias1 UNIQUE(sitealiasid))
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
updatetimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_cuyahoga_section1 UNIQUE(sectionid))
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

