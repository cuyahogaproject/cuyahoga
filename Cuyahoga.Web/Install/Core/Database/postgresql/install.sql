CREATE TABLE cuyahoga_user(
userid serial NOT NULL CONSTRAINT PK_user PRIMARY KEY,
username varchar(50) NOT NULL CONSTRAINT UC_user_username UNIQUE,
password varchar(100) NOT NULL,
firstname varchar(100),
lastname varchar(100),
email varchar(100) NOT NULL,
website varchar(100),
timezone int4 DEFAULT 0 NOT NULL,
isactive bool,
lastlogin timestamp,
lastip varchar(40),
inserttimestamp timestamp DEFAULT current_timestamp NOT NULL,
updatetimestamp timestamp DEFAULT current_timestamp NOT NULL);


CREATE TABLE cuyahoga_role(
roleid serial NOT NULL CONSTRAINT PK_role PRIMARY KEY,
name varchar(50) NOT NULL CONSTRAINT UC_role_name UNIQUE,
permissionlevel int4 DEFAULT 1 NOT NULL,
inserttimestamp timestamp DEFAULT current_timestamp NOT NULL,
updatetimestamp timestamp DEFAULT current_timestamp NOT NULL);


CREATE TABLE cuyahoga_userrole(
userroleid serial NOT NULL CONSTRAINT PK_userrole PRIMARY KEY,
userid int4 NOT NULL,
roleid int4 NOT NULL,
CONSTRAINT FK_role_roleid FOREIGN KEY (roleid) REFERENCES cuyahoga_role (roleid),
CONSTRAINT FK_user_userid FOREIGN KEY (userid) REFERENCES cuyahoga_user (userid));


CREATE TABLE cuyahoga_template(
templateid serial NOT NULL CONSTRAINT PK_template PRIMARY KEY,
name varchar(100) NOT NULL,
basepath varchar(100) NOT NULL,
templatecontrol varchar(50) NOT NULL,
css varchar(100) NOT NULL,
inserttimestamp timestamp DEFAULT current_timestamp NOT NULL,
updatetimestamp timestamp DEFAULT current_timestamp NOT NULL);


CREATE TABLE cuyahoga_moduletype(
moduletypeid serial NOT NULL CONSTRAINT PK_moduletype PRIMARY KEY,
name varchar(100) NOT NULL,
assemblyname varchar(100),
classname varchar(255) NOT NULL CONSTRAINT UC_moduletype_classname UNIQUE,
path varchar(255) NOT NULL,
editpath varchar(255),
autoactivate bool NOT NULL DEFAULT true,
inserttimestamp timestamp DEFAULT current_timestamp NOT NULL,
updatetimestamp timestamp DEFAULT current_timestamp NOT NULL);


CREATE TABLE cuyahoga_modulesetting(
modulesettingid serial NOT NULL CONSTRAINT PK_modulesetting PRIMARY KEY,
moduletypeid int4 NOT NULL,
name varchar(50) NOT NULL,
friendlyname varchar(50) NOT NULL,
settingdatatype varchar(100) NOT NULL,
iscustomtype bool NOT NULL,
isrequired bool NOT NULL,
CONSTRAINT FK_modulesetting_moduletype_moduletypeid FOREIGN KEY (moduletypeid) REFERENCES cuyahoga_moduletype (moduletypeid));

CREATE UNIQUE INDEX IX_modulesetting_moduletypeid_name ON cuyahoga_modulesetting (moduletypeid,name);


CREATE TABLE cuyahoga_moduleservice(
moduleserviceid serial NOT NULL CONSTRAINT PK_moduleservice PRIMARY KEY,
moduletypeid int4 NOT NULL,
servicekey varchar(50) NOT NULL,
servicetype varchar(255) NOT NULL,
classtype varchar(255) NOT NULL,
lifestyle varchar(10),
CONSTRAINT FK_moduleservice_moduletype_moduletypeid FOREIGN KEY (moduletypeid) REFERENCES cuyahoga_moduletype (moduletypeid));

CREATE UNIQUE INDEX IX_moduleservice_moduletypeid_servicekey ON cuyahoga_moduleservice (moduletypeid,servicekey);


CREATE TABLE cuyahoga_site(
siteid serial NOT NULL CONSTRAINT PK_site PRIMARY KEY,
templateid int4,
roleid int4 NOT NULL,
name varchar(100) NOT NULL CONSTRAINT UC_site_name UNIQUE,
homeurl varchar(100) NOT NULL,
defaultculture varchar(8) NOT NULL,
defaultplaceholder varchar(100),
webmasteremail varchar(100) NOT NULL,
usefriendlyurls bool,
metakeywords varchar(500),
metadescription varchar(500),
inserttimestamp timestamp DEFAULT current_timestamp NOT NULL,
updatetimestamp timestamp DEFAULT current_timestamp NOT NULL,
CONSTRAINT FK_site_role_roleid FOREIGN KEY (roleid) REFERENCES cuyahoga_role (roleid),
CONSTRAINT FK_site_template_templateid FOREIGN KEY (templateid) REFERENCES cuyahoga_template (templateid));


CREATE TABLE cuyahoga_node(
nodeid serial NOT NULL CONSTRAINT PK_node PRIMARY KEY,
parentnodeid int4,
templateid int4,
siteid int4 NOT NULL,
title varchar(255) NOT NULL,
shortdescription varchar(255) NOT NULL,
position int4 DEFAULT 0 NOT NULL,
culture varchar(8) NOT NULL,
showinnavigation bool NOT NULL,
linkurl varchar(255),
linktarget int4,
metakeywords varchar(500),
metadescription varchar(500),
inserttimestamp timestamp DEFAULT current_timestamp NOT NULL,
updatetimestamp timestamp DEFAULT current_timestamp NOT NULL,
CONSTRAINT FK_node_node_parentnodeid FOREIGN KEY (parentnodeid) REFERENCES cuyahoga_node (nodeid),
CONSTRAINT FK_node_site_siteid FOREIGN KEY (siteid) REFERENCES cuyahoga_site (siteid),
CONSTRAINT FK_node_template_templateid FOREIGN KEY (templateid) REFERENCES cuyahoga_template (templateid));

CREATE UNIQUE INDEX IX_node_shortdescription_siteid ON cuyahoga_node (shortdescription,siteid);

CREATE TABLE cuyahoga_menu(
menuid serial NOT NULL CONSTRAINT PK_menu PRIMARY KEY,
rootnodeid int4 NOT NULL,
name varchar(50) NOT NULL,
placeholder varchar(50) NOT NULL,
inserttimestamp date DEFAULT current_timestamp NOT NULL,
updatetimestamp date DEFAULT current_timestamp NOT NULL,
CONSTRAINT FK_menu_node_nodeid FOREIGN KEY (rootnodeid) REFERENCES cuyahoga_node (nodeid));


CREATE TABLE cuyahoga_menunode(
menunodeid serial NOT NULL CONSTRAINT PK_menunode PRIMARY KEY,
menuid int4 NOT NULL,
nodeid int4 NOT NULL,
position int4 NOT NULL,
CONSTRAINT FK_menunode_menu_menuid FOREIGN KEY (menuid) REFERENCES cuyahoga_menu (menuid),
CONSTRAINT FK_menunode_node_nodeid FOREIGN KEY (nodeid) REFERENCES cuyahoga_node (nodeid));


CREATE TABLE cuyahoga_sitealias(
sitealiasid serial NOT NULL CONSTRAINT PK_sitealias PRIMARY KEY,
siteid int4 NOT NULL,
nodeid int4,
url varchar(100) NOT NULL,
inserttimestamp date DEFAULT current_timestamp NOT NULL,
updatetimestamp date DEFAULT current_timestamp NOT NULL,
CONSTRAINT FK_sitealias_node_nodeid FOREIGN KEY (nodeid) REFERENCES cuyahoga_node (nodeid),
CONSTRAINT FK_sitealias_site_siteid FOREIGN KEY (siteid) REFERENCES cuyahoga_site (siteid));


CREATE TABLE cuyahoga_section(
sectionid serial NOT NULL CONSTRAINT PK_section PRIMARY KEY,
nodeid int4,
moduletypeid int4 NOT NULL,
title varchar(100) NOT NULL,
showtitle bool DEFAULT true NOT NULL,
placeholder varchar(100),
position int4 DEFAULT 0 NOT NULL,
cacheduration int4,
inserttimestamp timestamp DEFAULT current_timestamp NOT NULL,
updatetimestamp timestamp DEFAULT current_timestamp NOT NULL,
CONSTRAINT FK_section_moduletype_moduletypeid FOREIGN KEY (moduletypeid) REFERENCES cuyahoga_moduletype (moduletypeid),
CONSTRAINT FK_section_node_nodeid FOREIGN KEY (nodeid) REFERENCES cuyahoga_node (nodeid));


CREATE TABLE cuyahoga_sectionsetting(
sectionsettingid serial NOT NULL CONSTRAINT PK_sectionsetting PRIMARY KEY,
sectionid int4 NOT NULL,
name varchar(50) NOT NULL,
value varchar(100),
CONSTRAINT FK_sectionsetting_section_sectionid FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid));

CREATE UNIQUE INDEX IX_sectionsetting_sectionid_name ON cuyahoga_sectionsetting (sectionid,name);


CREATE TABLE cuyahoga_sectionconnection(
sectionconnectionid serial NOT NULL CONSTRAINT PK_sectionconnection PRIMARY KEY,
sectionidfrom int4 NOT NULL,
sectionidto int4 NOT NULL,
actionname varchar(50) NOT NULL,
CONSTRAINT FK_sectionconnection_section_sectionidfrom FOREIGN KEY (sectionidfrom) REFERENCES cuyahoga_section (sectionid),
CONSTRAINT FK_sectionconnection_section_sectionidto FOREIGN KEY (sectionidto) REFERENCES cuyahoga_section (sectionid));

CREATE UNIQUE INDEX IX_sectionconnection_sectionidfrom_actionname ON cuyahoga_sectionconnection (sectionidfrom, actionname);

CREATE TABLE cuyahoga_templatesection(
templatesectionid serial NOT NULL CONSTRAINT PK_templatesection PRIMARY KEY,
templateid int4 NOT NULL,
sectionid int4 NOT NULL,
placeholder varchar(100) NOT NULL,
CONSTRAINT FK_templatesection_template_templateid FOREIGN KEY (templateid) REFERENCES cuyahoga_template (templateid),
CONSTRAINT FK_templatesection_section_sectionid FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid));

CREATE UNIQUE INDEX IX_templatesection_templateidid_placeholder ON cuyahoga_templatesection (templateid, placeholder);

CREATE TABLE cuyahoga_noderole(
noderoleid serial NOT NULL CONSTRAINT PK_noderole PRIMARY KEY,
nodeid int4 NOT NULL,
roleid int4 NOT NULL,
viewallowed bool NOT NULL,
editallowed bool NOT NULL,
CONSTRAINT FK_noderole_node_nodeid FOREIGN KEY (nodeid) REFERENCES cuyahoga_node (nodeid),
CONSTRAINT FK_noderole_role_roleid FOREIGN KEY (roleid) REFERENCES cuyahoga_role (roleid));

CREATE UNIQUE INDEX IX_noderole_nodeid_roleid ON cuyahoga_noderole (nodeid,roleid);

CREATE TABLE cuyahoga_sectionrole(
sectionroleid serial NOT NULL CONSTRAINT PK_sectionrole PRIMARY KEY,
sectionid int4 NOT NULL,
roleid int4 NOT NULL,
viewallowed bool NOT NULL,
editallowed bool NOT NULL,
CONSTRAINT FK_sectionrole_role_roleid FOREIGN KEY (roleid) REFERENCES cuyahoga_role (roleid),
CONSTRAINT FK_sectionrole_section_sectionid FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid));

CREATE UNIQUE INDEX IX_sectionrole_roleid_sectionid ON cuyahoga_sectionrole (roleid,sectionid);

CREATE TABLE cuyahoga_version(
versionid serial NOT NULL CONSTRAINT PK_version PRIMARY KEY,
assembly varchar(255) NOT NULL,
major int NOT NULL,
minor int NOT NULL,
patch int NOT NULL);


-- DATA --

INSERT INTO cuyahoga_role (roleid, name, inserttimestamp, updatetimestamp, permissionlevel) VALUES (3, 'Authenticated user', '2004-01-04 16:34:50.271', '2004-06-25 00:59:02.822', 2);
INSERT INTO cuyahoga_role (roleid, name, inserttimestamp, updatetimestamp, permissionlevel) VALUES (2, 'Editor', '2004-01-04 16:34:25.669', '2004-06-25 00:59:08.256', 6);
INSERT INTO cuyahoga_role (roleid, name, inserttimestamp, updatetimestamp, permissionlevel) VALUES (1, 'Administrator', '2004-01-04 16:33:42.255', '2004-09-19 17:08:47.248', 14);
INSERT INTO cuyahoga_role (roleid, name, inserttimestamp, updatetimestamp, permissionlevel) VALUES (4, 'Anonymous user', '2004-01-04 16:35:10.766', '2004-07-16 21:18:09.017', 1);

INSERT INTO cuyahoga_template (templateid, name, basepath, templatecontrol, css, inserttimestamp, updatetimestamp) VALUES (1, 'Cuyahoga Home', 'Templates/Classic', 'CuyahogaHome.ascx', 'red.css', '2004-01-26 21:52:52.365', '2004-01-26 21:52:52.365');
INSERT INTO cuyahoga_template (templateid, name, basepath, templatecontrol, css, inserttimestamp, updatetimestamp) VALUES (2, 'Cuyahoga Standard', 'Templates/Classic', 'CuyahogaStandard.ascx', 'red.css', '2004-01-26 21:52:52.365', '2004-01-26 21:52:52.365');
INSERT INTO cuyahoga_template (templateid, name, basepath, templatecontrol, css, inserttimestamp, updatetimestamp) VALUES (3, 'Cuyahoga New', 'Templates/Default', 'CuyahogaNew.ascx', 'red-new.css', '2004-01-26 21:52:52.365', '2004-01-26 21:52:52.365');
INSERT INTO cuyahoga_template (templateid, name, basepath, templatecontrol, css, inserttimestamp, updatetimestamp) VALUES (4, 'Another Red', 'Templates/AnotherRed', 'Cuyahoga.ascx', 'red.css', '2004-01-26 21:52:52.365', '2004-01-26 21:52:52.365');

INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Core', 1, 5, 2);
