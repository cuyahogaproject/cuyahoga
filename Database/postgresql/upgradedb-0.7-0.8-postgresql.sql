/************************
New tables
************************/

CREATE TABLE cm_feed(
feedid serial NOT NULL CONSTRAINT PK_cm_feed1 PRIMARY KEY,
sectionid int4 NOT NULL,
url varchar(255) NOT NULL,
title varchar(100) NOT NULL,
pubdate date NOT NULL,
numberofitems int4 NOT NULL,
inserttimestamp timestamp DEFAULT current_timestamp NOT NULL,
updatetimestamp timestamp DEFAULT current_timestamp NOT NULL,
CONSTRAINT FK_cm_feed_1 FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid));


CREATE TABLE cm_feeditem(
feeditemid serial NOT NULL CONSTRAINT PK_cm_feeditem1 PRIMARY KEY,
feedid int4 NOT NULL,
url varchar(255) NOT NULL,
title varchar(100) NOT NULL,
content text,
pubdate date NOT NULL,
author varchar(100),
CONSTRAINT FK_cm_feeditem_1 FOREIGN KEY (feedid) REFERENCES cm_feed (feedid));


CREATE TABLE cm_file(
fileid serial NOT NULL CONSTRAINT PK_cm_file1 PRIMARY KEY,
sectionid int4 NOT NULL,
publisherid int4 NOT NULL,
filepath varchar(255) NOT NULL,
title varchar(100),
filesize int4 NOT NULL,
nrofdownloads int4 NOT NULL,
contenttype varchar(50) NOT NULL,
inserttimestamp timestamp DEFAULT current_timestamp NOT NULL,
updatetimestamp timestamp NOT NULL,
CONSTRAINT FK_cm_file_1 FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid),
CONSTRAINT FK_cm_file_2 FOREIGN KEY (publisherid) REFERENCES cuyahoga_user (userid));


CREATE TABLE cm_filerole(
fileroleid serial NOT NULL CONSTRAINT PK_cm_filerole1 PRIMARY KEY,
fileid int4 NOT NULL,
roleid int4 NOT NULL,
CONSTRAINT FK_cm_filerole_1 FOREIGN KEY (fileid) REFERENCES cm_file (fileid),
CONSTRAINT FK_cm_filerole_2 FOREIGN KEY (roleid) REFERENCES cuyahoga_role (roleid));

CREATE TABLE cuyahoga_version(
versionid serial NOT NULL CONSTRAINT PK_cuyahoga_version PRIMARY KEY,
assembly varchar(255) NOT NULL,
major int NOT NULL,
minor int NOT NULL,
patch int NOT NULL);

/************************
Misc. DDL changes
************************/
ALTER TABLE cuyahoga_user
	ADD COLUMN timezone int NULL;

UPDATE cuyahoga_user SET timezone = 0;

ALTER TABLE cuyahoga_user
	ALTER COLUMN timezone SET NOT NULL;

ALTER TABLE cuyahoga_site
	ADD COLUMN usefriendlyurls bool NULL;

ALTER TABLE cuyahoga_node
	DROP CONSTRAINT uc_cuyahoga_node2;
		
CREATE UNIQUE INDEX IDX_cuyahoga_node_shortdescription_siteid ON cuyahoga_node (shortdescription,siteid);

/************************
Modules parameters
************************/

-- first set sequence values, otherwise the inserts will fail due to violation of the pk_constraint
SELECT setval('cuyahoga_moduletype_moduletypeid_seq', (SELECT max(moduletypeid) FROM cuyahoga_moduletype));
SELECT setval('cuyahoga_modulesetting_modulesettingid_seq', (SELECT max(modulesettingid) FROM cuyahoga_modulesetting));

-- RemoteContent
INSERT INTO cuyahoga_moduletype (name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) VALUES ('RemoteContent', 'Cuyahoga.Modules', 'Cuyahoga.Modules.RemoteContent.RemoteContentModule', 'Modules/RemoteContent/RemoteContent.ascx', 'Modules/RemoteContent/AdminRemoteContent.aspx', '2005-04-08 14:36:28.324', '2004-04-08 14:36:28.324');

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (currval('cuyahoga_moduletype_moduletypeid_seq'), 'CACHE_DURATION', 'Local database cache duration (min)', 'System.Int32', false, true);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (currval('cuyahoga_moduletype_moduletypeid_seq'), 'BACKGROUND_REFRESH', 'Use background refreshing', 'System.Boolean', false, true);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (currval('cuyahoga_moduletype_moduletypeid_seq'), 'SHOW_CONTENTS', 'Show feed contents', 'System.Boolean', false, true);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (currval('cuyahoga_moduletype_moduletypeid_seq'), 'SHOW_DATES', 'Show dates', 'System.Boolean', false, true);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (currval('cuyahoga_moduletype_moduletypeid_seq'), 'SHOW_SOURCES', 'Show feed sources', 'System.Boolean', false, true);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (currval('cuyahoga_moduletype_moduletypeid_seq'), 'SHOW_AUTHORS', 'Show authors', 'System.Boolean', false, true);

-- Downloads
INSERT INTO cuyahoga_moduletype (name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) VALUES ('Downloads', 'Cuyahoga.Modules.Downloads', 'Cuyahoga.Modules.Downloads.DownloadsModule', 'Modules/Downloads/Downloads.ascx', 'Modules/Downloads/EditDownloads.aspx', '2005-05-15 14:36:28.324', '2004-05-15 14:36:28.324');

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (currval('cuyahoga_moduletype_moduletypeid_seq'), 'SHOW_PUBLISHER', 'Show publisher', 'System.Boolean', false, true);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (currval('cuyahoga_moduletype_moduletypeid_seq'), 'SHOW_DATE', 'Show file date', 'System.Boolean', false, true);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (currval('cuyahoga_moduletype_moduletypeid_seq'), 'PHYSICAL_DIR', 'Physical directory (empty for App_Root/files)', 'System.String', false, false);

/*********************
Version updates
*********************/

INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Core', 0, 8, 0);
INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Modules', 0, 8, 0);
INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Modules.Downloads', 0, 8, 0);
