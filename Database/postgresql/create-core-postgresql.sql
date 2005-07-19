
CREATE TABLE cuyahoga_user(
userid serial NOT NULL CONSTRAINT PK_cuyahoga_user1 PRIMARY KEY,
username varchar(50) NOT NULL CONSTRAINT UC_cuyahoga_user1 UNIQUE,
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
roleid serial NOT NULL CONSTRAINT PK_cuyahoga_role1 PRIMARY KEY,
name varchar(50) NOT NULL CONSTRAINT UC_cuyahoga_role1 UNIQUE,
permissionlevel int4 DEFAULT 1 NOT NULL,
inserttimestamp timestamp DEFAULT current_timestamp NOT NULL,
updatetimestamp timestamp DEFAULT current_timestamp NOT NULL);


CREATE TABLE cuyahoga_userrole(
userroleid serial NOT NULL CONSTRAINT PK_cuyahoga_userrole1 PRIMARY KEY,
userid int4 NOT NULL,
roleid int4 NOT NULL,
inserttimestamp timestamp DEFAULT current_timestamp NOT NULL,
updatetimestamp timestamp DEFAULT current_timestamp NOT NULL,
CONSTRAINT FK_cuyahoga_userrole_1 FOREIGN KEY (roleid) REFERENCES cuyahoga_role (roleid),
CONSTRAINT FK_cuyahoga_userrole_2 FOREIGN KEY (userid) REFERENCES cuyahoga_user (userid));


CREATE TABLE cuyahoga_template(
templateid serial NOT NULL CONSTRAINT PK_cuyahoga_template1 PRIMARY KEY,
name varchar(100) NOT NULL,
basepath varchar(100) NOT NULL,
templatecontrol varchar(50) NOT NULL,
css varchar(100) NOT NULL,
inserttimestamp timestamp DEFAULT current_timestamp NOT NULL,
updatetimestamp timestamp DEFAULT current_timestamp NOT NULL);


CREATE TABLE cuyahoga_moduletype(
moduletypeid serial NOT NULL CONSTRAINT PK_cuyahoga_moduletype1 PRIMARY KEY,
name varchar(100) NOT NULL,
assemblyname varchar(100),
classname varchar(255) NOT NULL CONSTRAINT UC_cuyahoga_moduletype1 UNIQUE,
path varchar(255) NOT NULL,
editpath varchar(255),
inserttimestamp timestamp DEFAULT current_timestamp NOT NULL,
updatetimestamp timestamp DEFAULT current_timestamp NOT NULL);


CREATE TABLE cuyahoga_modulesetting(
modulesettingid serial NOT NULL CONSTRAINT PK_cuyahoga_modulesetting1 PRIMARY KEY,
moduletypeid int4 NOT NULL,
name varchar(50) NOT NULL,
friendlyname varchar(50) NOT NULL,
settingdatatype varchar(100) NOT NULL,
iscustomtype bool NOT NULL,
isrequired bool NOT NULL,
CONSTRAINT FK_cuyahoga_modulesetting_1 FOREIGN KEY (moduletypeid) REFERENCES cuyahoga_moduletype (moduletypeid));

CREATE UNIQUE INDEX IDX_cuyahoga_modulesetting_1 ON cuyahoga_modulesetting (moduletypeid,name);

CREATE TABLE cuyahoga_site(
siteid serial NOT NULL CONSTRAINT PK_cuyahoga_site1 PRIMARY KEY,
templateid int4,
roleid int4 NOT NULL,
name varchar(100) NOT NULL CONSTRAINT UC_cuyahoga_site1 UNIQUE,
homeurl varchar(100) NOT NULL,
defaultculture varchar(8) NOT NULL,
defaultplaceholder varchar(100),
webmasteremail varchar(100) NOT NULL,
usefriendlyurls bool,
inserttimestamp timestamp DEFAULT current_timestamp NOT NULL,
updatetimestamp timestamp DEFAULT current_timestamp NOT NULL,
CONSTRAINT FK_cuyahoga_site_1 FOREIGN KEY (roleid) REFERENCES cuyahoga_role (roleid),
CONSTRAINT FK_cuyahoga_site_2 FOREIGN KEY (templateid) REFERENCES cuyahoga_template (templateid));


CREATE TABLE cuyahoga_node(
nodeid serial NOT NULL CONSTRAINT PK_cuyahoga_node1 PRIMARY KEY,
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
inserttimestamp timestamp DEFAULT current_timestamp NOT NULL,
updatetimestamp timestamp DEFAULT current_timestamp NOT NULL,
CONSTRAINT FK_cuyahoga_node_1 FOREIGN KEY (parentnodeid) REFERENCES cuyahoga_node (nodeid),
CONSTRAINT FK_cuyahoga_node_2 FOREIGN KEY (siteid) REFERENCES cuyahoga_site (siteid),
CONSTRAINT FK_cuyahoga_node_3 FOREIGN KEY (templateid) REFERENCES cuyahoga_template (templateid));

CREATE UNIQUE INDEX IDX_cuyahoga_node_shortdescription_siteid ON cuyahoga_node (shortdescription,siteid);

CREATE TABLE cuyahoga_menu(
menuid serial NOT NULL CONSTRAINT PK_cuyahoga_menu1 PRIMARY KEY,
rootnodeid int4 NOT NULL,
name varchar(50) NOT NULL,
placeholder varchar(50) NOT NULL,
inserttimestamp date DEFAULT current_timestamp NOT NULL,
updatetimestamp date DEFAULT current_timestamp NOT NULL,
CONSTRAINT FK_cuyahoga_menu_1 FOREIGN KEY (rootnodeid) REFERENCES cuyahoga_node (nodeid));


CREATE TABLE cuyahoga_menunode(
menunodeid serial NOT NULL CONSTRAINT PK_cuyahoga_menunode1 PRIMARY KEY,
menuid int4 NOT NULL,
nodeid int4 NOT NULL,
position int4 NOT NULL,
CONSTRAINT FK_cuyahoga_menunode_1 FOREIGN KEY (menuid) REFERENCES cuyahoga_menu (menuid),
CONSTRAINT FK_cuyahoga_menunode_2 FOREIGN KEY (nodeid) REFERENCES cuyahoga_node (nodeid));


CREATE TABLE cuyahoga_sitealias(
sitealiasid serial NOT NULL CONSTRAINT PK_cuyahoga_sitealias1 PRIMARY KEY,
siteid int4 NOT NULL,
nodeid int4,
url varchar(100) NOT NULL,
inserttimestamp date DEFAULT current_timestamp NOT NULL,
updatetimestamp date DEFAULT current_timestamp NOT NULL,
CONSTRAINT FK_cuyahoga_sitealias_1 FOREIGN KEY (nodeid) REFERENCES cuyahoga_node (nodeid),
CONSTRAINT FK_cuyahoga_sitealias_2 FOREIGN KEY (siteid) REFERENCES cuyahoga_site (siteid));


CREATE TABLE cuyahoga_section(
sectionid serial NOT NULL CONSTRAINT PK_cuyahoga_section1 PRIMARY KEY,
nodeid int4,
moduletypeid int4 NOT NULL,
title varchar(100) NOT NULL,
showtitle bool DEFAULT true NOT NULL,
placeholder varchar(100),
position int4 DEFAULT 0 NOT NULL,
cacheduration int4,
inserttimestamp timestamp DEFAULT current_timestamp NOT NULL,
updatetimestamp timestamp DEFAULT current_timestamp NOT NULL,
CONSTRAINT FK_cuyahoga_section_1 FOREIGN KEY (moduletypeid) REFERENCES cuyahoga_moduletype (moduletypeid),
CONSTRAINT FK_cuyahoga_section_2 FOREIGN KEY (nodeid) REFERENCES cuyahoga_node (nodeid));


CREATE TABLE cuyahoga_sectionsetting(
sectionsettingid serial NOT NULL CONSTRAINT PK_cuyahoga_sectionsetting1 PRIMARY KEY,
sectionid int4 NOT NULL,
name varchar(50) NOT NULL,
value varchar(100),
CONSTRAINT FK_cuyahoga_sectionsetting_1 FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid));

CREATE UNIQUE INDEX IDX_cuyahoga_sectionsetting_1 ON cuyahoga_sectionsetting (sectionid,name);

CREATE TABLE cuyahoga_noderole(
noderoleid serial NOT NULL CONSTRAINT PK_cuyahoga_noderole1 PRIMARY KEY,
nodeid int4 NOT NULL,
roleid int4 NOT NULL,
viewallowed bool NOT NULL,
editallowed bool NOT NULL,
CONSTRAINT FK_cuyahoga_noderole_1 FOREIGN KEY (nodeid) REFERENCES cuyahoga_node (nodeid),
CONSTRAINT FK_cuyahoga_noderole_2 FOREIGN KEY (roleid) REFERENCES cuyahoga_role (roleid));

CREATE UNIQUE INDEX IDX_cuyahoga_noderole_1 ON cuyahoga_noderole (nodeid,roleid);

CREATE TABLE cuyahoga_sectionrole(
sectionroleid serial NOT NULL CONSTRAINT PK_cuyahoga_sectionrole1 PRIMARY KEY,
sectionid int4 NOT NULL,
roleid int4 NOT NULL,
viewallowed bool NOT NULL,
editallowed bool NOT NULL,
CONSTRAINT FK_cuyahoga_sectionrole_1 FOREIGN KEY (roleid) REFERENCES cuyahoga_role (roleid),
CONSTRAINT FK_cuyahoga_sectionrole_2 FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid));

CREATE UNIQUE INDEX IDX_cuyahoga_sectionrole_1 ON cuyahoga_sectionrole (roleid,sectionid);

CREATE TABLE cuyahoga_version(
versionid serial NOT NULL CONSTRAINT PK_cuyahoga_version PRIMARY KEY,
assembly varchar(255) NOT NULL,
major int NOT NULL,
minor int NOT NULL,
patch int NOT NULL);
