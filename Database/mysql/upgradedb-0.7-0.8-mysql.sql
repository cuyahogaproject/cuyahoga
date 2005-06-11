/************************
New tables
************************/

CREATE TABLE cm_feed(
feedid INT NOT NULL AUTO_INCREMENT,
sectionid INT NOT NULL,
url VARCHAR(255) NOT NULL,
title VARCHAR(100) NOT NULL,
pubdate DATETIME NOT NULL,
numberofitems INT NOT NULL,
inserttimestamp TIMESTAMP NOT NULL,
updatetimestamp DATETIME NOT NULL,
FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid),
PRIMARY KEY (feedid));


CREATE TABLE cm_feeditem(
feeditemid INT NOT NULL AUTO_INCREMENT,
feedid INT NOT NULL,
url VARCHAR(255) NOT NULL,
title VARCHAR(100) NOT NULL,
content TEXT,
pubdate DATETIME NOT NULL,
author VARCHAR(100),
FOREIGN KEY (feedid) REFERENCES cm_feed (feedid),
PRIMARY KEY (feeditemid));

CREATE TABLE cm_file(
fileid INT NOT NULL AUTO_INCREMENT,
sectionid INT NOT NULL,
publisherid INT NOT NULL,
filepath VARCHAR(255) NOT NULL,
title VARCHAR(100),
filesize INT NOT NULL,
nrofdownloads INT NOT NULL,
contenttype VARCHAR(50) NOT NULL,
inserttimestamp TIMESTAMP NOT NULL,
updatetimestamp DATETIME NOT NULL,
FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid),
FOREIGN KEY (publisherid) REFERENCES cuyahoga_user (userid),
PRIMARY KEY (fileid));


CREATE TABLE cm_filerole(
fileroleid INT NOT NULL AUTO_INCREMENT,
fileid INT NOT NULL,
roleid INT NOT NULL,
FOREIGN KEY (fileid) REFERENCES cm_file (fileid),
FOREIGN KEY (roleid) REFERENCES cuyahoga_role (roleid),
PRIMARY KEY (fileroleid));

CREATE TABLE cuyahoga_version(
versionid INT NOT NULL AUTO_INCREMENT,
assembly VARCHAR(255) NOT NULL,
major INT NOT NULL,
minor INT NOT NULL,
patch INT NOT NULL,
PRIMARY KEY (versionid));

/************************
Misc. DDL changes
************************/
ALTER TABLE cuyahoga_user
	ADD COLUMN timezone int NULL;

UPDATE cuyahoga_user SET timezone = 0;

ALTER TABLE cuyahoga_user
	CHANGE COLUMN timezone timezone INT NOT NULL;

ALTER TABLE cuyahoga_site
	ADD COLUMN usefriendlyurls TINYINT NULL;

ALTER TABLE cuyahoga_node
	DROP index UC_shortdescription;
		
ALTER TABLE cuyahoga_node
	ADD UNIQUE IDX_cuyahoga_node_shortdescription_siteid (shortdescription,siteid);

/************************
Modules parameters
************************/

-- RemoteContent
INSERT INTO cuyahoga_moduletype (name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) VALUES ('RemoteContent', 'Cuyahoga.Modules', 'Cuyahoga.Modules.RemoteContent.RemoteContentModule', 'Modules/RemoteContent/RemoteContent.ascx', 'Modules/RemoteContent/AdminRemoteContent.aspx', '2005-04-08 14:36:28.324', '2004-04-08 14:36:28.324');

SELECT @moduletypeid := last_insert_id();

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'CACHE_DURATION', 'Local database cache duration (min)', 'System.Int32', 0, 1);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'BACKGROUND_REFRESH', 'Use background refreshing', 'System.Boolean', 0, 1);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'SHOW_CONTENTS', 'Show feed contents', 'System.Boolean', 0, 1);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'SHOW_DATES', 'Show dates', 'System.Boolean', 0, 1);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'SHOW_SOURCES', 'Show feed sources', 'System.Boolean', 0, 1);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'SHOW_AUTHORS', 'Show authors', 'System.Boolean', 0, 1);

-- Downloads
INSERT INTO cuyahoga_moduletype (name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) VALUES ('Downloads', 'Cuyahoga.Modules.Downloads', 'Cuyahoga.Modules.Downloads.DownloadsModule', 'Modules/Downloads/Downloads.ascx', 'Modules/Downloads/EditDownloads.aspx', '2005-05-15 14:36:28.324', '2004-05-15 14:36:28.324');

SELECT @moduletypeid := last_insert_id();

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'SHOW_PUBLISHER', 'Show publisher', 'System.Boolean', 0, 1);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'SHOW_DATE', 'Show file date', 'System.Boolean', 0, 1);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'PHYSICAL_DIR', 'Physical directory (empty for App_Root/files)', 'System.String', 0, 0);

/*********************
Version updates
*********************/

INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Core', 0, 8, 0);
INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Modules', 0, 8, 0);
INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Modules.Downloads', 0, 8, 0);
