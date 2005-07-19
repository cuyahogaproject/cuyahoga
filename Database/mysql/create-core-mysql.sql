

CREATE TABLE cuyahoga_user(
userid INT NOT NULL AUTO_INCREMENT,
username VARCHAR(50) NOT NULL,
password VARCHAR(100) NOT NULL,
firstname VARCHAR(100),
lastname VARCHAR(100),
email VARCHAR(100) NOT NULL,
website VARCHAR(100),
timezone INT DEFAULT 0 NOT NULL,
isactive TINYINT,
lastlogin DATETIME,
lastip VARCHAR(40),
inserttimestamp TIMESTAMP NOT NULL,
updatetimestamp DATETIME NOT NULL,
PRIMARY KEY (userid),
UNIQUE UC_username (username));


CREATE TABLE cuyahoga_role(
roleid INT NOT NULL AUTO_INCREMENT,
name VARCHAR(50) NOT NULL,
permissionlevel INT DEFAULT 1 NOT NULL,
inserttimestamp TIMESTAMP NOT NULL,
updatetimestamp DATETIME NOT NULL,
PRIMARY KEY (roleid),
UNIQUE UC_name (name));


CREATE TABLE cuyahoga_userrole(
userroleid INT NOT NULL AUTO_INCREMENT,
userid INT NOT NULL,
roleid INT NOT NULL,
inserttimestamp TIMESTAMP NOT NULL,
updatetimestamp DATETIME NOT NULL,
FOREIGN KEY (roleid) REFERENCES cuyahoga_role (roleid),
FOREIGN KEY (userid) REFERENCES cuyahoga_user (userid),
PRIMARY KEY (userroleid));


CREATE TABLE cuyahoga_template(
templateid INT NOT NULL AUTO_INCREMENT,
name VARCHAR(100) NOT NULL,
basepath VARCHAR(100) NOT NULL,
templatecontrol VARCHAR(50) NOT NULL,
css VARCHAR(100) NOT NULL,
inserttimestamp TIMESTAMP NOT NULL,
updatetimestamp DATETIME NOT NULL,
PRIMARY KEY (templateid));


CREATE TABLE cuyahoga_moduletype(
moduletypeid INT NOT NULL AUTO_INCREMENT,
name VARCHAR(100) NOT NULL,
assemblyname VARCHAR(100),
classname VARCHAR(255) NOT NULL,
path VARCHAR(255) NOT NULL,
editpath VARCHAR(255),
inserttimestamp TIMESTAMP NOT NULL,
updatetimestamp DATETIME NOT NULL,
PRIMARY KEY (moduletypeid),
UNIQUE UC_classname (classname));


CREATE TABLE cuyahoga_modulesetting(
modulesettingid INT NOT NULL AUTO_INCREMENT,
moduletypeid INT NOT NULL,
name VARCHAR(50) NOT NULL,
friendlyname VARCHAR(50) NOT NULL,
settingdatatype VARCHAR(100) NOT NULL,
iscustomtype TINYINT NOT NULL,
isrequired TINYINT NOT NULL,
FOREIGN KEY (moduletypeid) REFERENCES cuyahoga_moduletype (moduletypeid),
PRIMARY KEY (modulesettingid),
UNIQUE IDX_cuyahoga_modulesetting_1 (moduletypeid,name));


CREATE TABLE cuyahoga_site(
siteid INT NOT NULL AUTO_INCREMENT,
templateid INT,
roleid INT NOT NULL,
name VARCHAR(100) NOT NULL,
homeurl VARCHAR(100) NOT NULL,
defaultculture VARCHAR(8) NOT NULL,
defaultplaceholder VARCHAR(100),
webmasteremail VARCHAR(100) NOT NULL,
usefriendlyurls TINYINT,
inserttimestamp TIMESTAMP NOT NULL,
updatetimestamp DATETIME NOT NULL,
FOREIGN KEY (roleid) REFERENCES cuyahoga_role (roleid),
FOREIGN KEY (templateid) REFERENCES cuyahoga_template (templateid),
PRIMARY KEY (siteid),
UNIQUE UC_name (name));


CREATE TABLE cuyahoga_node(
nodeid INT NOT NULL AUTO_INCREMENT,
parentnodeid INT,
templateid INT,
siteid INT NOT NULL,
title VARCHAR(255) NOT NULL,
shortdescription VARCHAR(255) NOT NULL,
position INT DEFAULT 0 NOT NULL,
culture VARCHAR(8) NOT NULL,
showinnavigation TINYINT NOT NULL,
linkurl VARCHAR(255),
linktarget INT,
inserttimestamp TIMESTAMP NOT NULL,
updatetimestamp DATETIME NOT NULL,
FOREIGN KEY (parentnodeid) REFERENCES cuyahoga_node (nodeid),
FOREIGN KEY (siteid) REFERENCES cuyahoga_site (siteid),
FOREIGN KEY (templateid) REFERENCES cuyahoga_template (templateid),
PRIMARY KEY (nodeid),
UNIQUE IDX_cuyahoga_node_shortdescription_siteid (shortdescription,siteid));


CREATE TABLE cuyahoga_menu(
menuid INT NOT NULL AUTO_INCREMENT,
rootnodeid INT NOT NULL,
name VARCHAR(50) NOT NULL,
placeholder VARCHAR(50) NOT NULL,
inserttimestamp TIMESTAMP NOT NULL,
updatetimestamp DATETIME NOT NULL,
FOREIGN KEY (rootnodeid) REFERENCES cuyahoga_node (nodeid),
PRIMARY KEY (menuid));


CREATE TABLE cuyahoga_menunode(
menunodeid INT NOT NULL AUTO_INCREMENT,
menuid INT NOT NULL,
nodeid INT NOT NULL,
position INT NOT NULL,
FOREIGN KEY (menuid) REFERENCES cuyahoga_menu (menuid),
FOREIGN KEY (nodeid) REFERENCES cuyahoga_node (nodeid),
PRIMARY KEY (menunodeid));


CREATE TABLE cuyahoga_sitealias(
sitealiasid INT NOT NULL AUTO_INCREMENT,
siteid INT NOT NULL,
nodeid INT,
url VARCHAR(100) NOT NULL,
inserttimestamp TIMESTAMP NOT NULL,
updatetimestamp DATETIME NOT NULL,
FOREIGN KEY (nodeid) REFERENCES cuyahoga_node (nodeid),
FOREIGN KEY (siteid) REFERENCES cuyahoga_site (siteid),
PRIMARY KEY (sitealiasid));


CREATE TABLE cuyahoga_section(
sectionid INT NOT NULL AUTO_INCREMENT,
nodeid INT,
moduletypeid INT NOT NULL,
title VARCHAR(100) NOT NULL,
showtitle TINYINT DEFAULT 1 NOT NULL,
placeholder VARCHAR(100),
position INT DEFAULT 0 NOT NULL,
cacheduration INT,
inserttimestamp TIMESTAMP NOT NULL,
updatetimestamp DATETIME NOT NULL,
FOREIGN KEY (moduletypeid) REFERENCES cuyahoga_moduletype (moduletypeid),
FOREIGN KEY (nodeid) REFERENCES cuyahoga_node (nodeid),
PRIMARY KEY (sectionid));


CREATE TABLE cuyahoga_sectionsetting(
sectionsettingid INT NOT NULL AUTO_INCREMENT,
sectionid INT NOT NULL,
name VARCHAR(50) NOT NULL,
value VARCHAR(100),
FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid),
PRIMARY KEY (sectionsettingid),
UNIQUE IDX_cuyahoga_sectionsetting_1 (sectionid,name));


CREATE TABLE cuyahoga_noderole(
noderoleid INT NOT NULL AUTO_INCREMENT,
nodeid INT NOT NULL,
roleid INT NOT NULL,
viewallowed TINYINT NOT NULL,
editallowed TINYINT NOT NULL,
FOREIGN KEY (nodeid) REFERENCES cuyahoga_node (nodeid),
FOREIGN KEY (roleid) REFERENCES cuyahoga_role (roleid),
PRIMARY KEY (noderoleid),
UNIQUE IDX_cuyahoga_noderole_1 (nodeid,roleid));


CREATE TABLE cuyahoga_sectionrole(
sectionroleid INT NOT NULL AUTO_INCREMENT,
sectionid INT NOT NULL,
roleid INT NOT NULL,
viewallowed TINYINT NOT NULL,
editallowed TINYINT NOT NULL,
FOREIGN KEY (roleid) REFERENCES cuyahoga_role (roleid),
FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid),
PRIMARY KEY (sectionroleid),
UNIQUE IDX_cuyahoga_sectionrole_1 (roleid,sectionid));

CREATE TABLE cuyahoga_version(
versionid INT NOT NULL AUTO_INCREMENT,
assembly VARCHAR(255) NOT NULL,
major INT NOT NULL,
minor INT NOT NULL,
patch INT NOT NULL,
PRIMARY KEY (versionid));
