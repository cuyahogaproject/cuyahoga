/************************
New tables
************************/

CREATE TABLE cm_feed(
feedid int identity(1,1) NOT NULL CONSTRAINT PK_cm_feed1 PRIMARY KEY,
sectionid int NOT NULL,
url varchar(255) NOT NULL,
title varchar(100) NOT NULL,
pubdate datetime NOT NULL,
numberofitems int NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go


CREATE TABLE cm_feeditem(
feeditemid int identity(1,1) NOT NULL CONSTRAINT PK_cm_feeditem1 PRIMARY KEY,
feedid int NOT NULL,
url varchar(255) NOT NULL,
title varchar(100) NOT NULL,
content text NULL,
pubdate datetime NOT NULL,
author varchar(100) NULL)
go

CREATE TABLE cm_file(
fileid int identity(1,1) NOT NULL CONSTRAINT PK_cm_file1 PRIMARY KEY,
sectionid int NOT NULL,
publisherid int NOT NULL,
filepath varchar(255) NOT NULL,
title varchar(100) NULL,
filesize int NOT NULL,
nrofdownloads int NOT NULL,
contenttype varchar(255) NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime NOT NULL)
go

CREATE TABLE cm_filerole(
fileroleid int identity(1,1) NOT NULL CONSTRAINT PK_cm_filerole1 PRIMARY KEY,
fileid int NOT NULL,
roleid int NOT NULL)
go

CREATE TABLE cuyahoga_version(
versionid int identity(1,1) NOT NULL CONSTRAINT PK_cuyahoga_version PRIMARY KEY,
assembly varchar(255) NOT NULL,
major int NOT NULL,
minor int NOT NULL,
patch int NOT NULL)

go

ALTER TABLE cm_feed
ADD CONSTRAINT FK_cm_feed_1 
FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid)
go


ALTER TABLE cm_feeditem
ADD CONSTRAINT FK_cm_feeditem_1 
FOREIGN KEY (feedid) REFERENCES cm_feed (feedid)
go

ALTER TABLE cm_file
ADD CONSTRAINT FK_cm_file_1 
FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid)
go

ALTER TABLE cm_file
ADD CONSTRAINT FK_cm_file_2 
FOREIGN KEY (publisherid) REFERENCES cuyahoga_user (userid)
go

ALTER TABLE cm_filerole
ADD CONSTRAINT FK_cm_filerole_1 
FOREIGN KEY (fileid) REFERENCES cm_file (fileid)
go

ALTER TABLE cm_filerole
ADD CONSTRAINT FK_cm_filerole_2 
FOREIGN KEY (roleid) REFERENCES cuyahoga_role (roleid)
go

/************************
Misc. DDL changes
************************/
ALTER TABLE cuyahoga_user
	ADD timezone int NOT NULL DEFAULT 0
go

ALTER TABLE cuyahoga_site
	ADD usefriendlyurls bit NULL

IF EXISTS (SELECT 1 FROM sysobjects WHERE xtype = 'UQ' AND name = 'UC_cuyahoga_node2')
	ALTER TABLE cuyahoga_node
		DROP CONSTRAINT UC_cuyahoga_node2
		
go
	
CREATE UNIQUE INDEX IDX_cuyahoga_node_shortdescription_siteid ON cuyahoga_node (shortdescription,siteid)
go


/************************
Modules parameters
************************/

DECLARE @moduletypeid int

-- RemoteContent
INSERT INTO cuyahoga_moduletype (name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) VALUES ('RemoteContent', 'Cuyahoga.Modules', 'Cuyahoga.Modules.RemoteContent.RemoteContentModule', 'Modules/RemoteContent/RemoteContent.ascx', 'Modules/RemoteContent/AdminRemoteContent.aspx', '2005-04-08 14:36:28.324', '2004-04-08 14:36:28.324')

SELECT @moduletypeid = Scope_Identity()

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'CACHE_DURATION', 'Local database cache duration (min)', 'System.Int32', 0, 1)

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'BACKGROUND_REFRESH', 'Use background refreshing', 'System.Boolean', 0, 1)

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'SHOW_CONTENTS', 'Show feed contents', 'System.Boolean', 0, 1)

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'SHOW_DATES', 'Show dates', 'System.Boolean', 0, 1)

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'SHOW_SOURCES', 'Show feed sources', 'System.Boolean', 0, 1)

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'SHOW_AUTHORS', 'Show authors', 'System.Boolean', 0, 1)

-- Downloads
INSERT INTO cuyahoga_moduletype (name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) VALUES ('Downloads', 'Cuyahoga.Modules.Downloads', 'Cuyahoga.Modules.Downloads.DownloadsModule', 'Modules/Downloads/Downloads.ascx', 'Modules/Downloads/EditDownloads.aspx', '2005-05-15 14:36:28.324', '2004-05-15 14:36:28.324')

SELECT @moduletypeid = Scope_Identity()

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'SHOW_PUBLISHER', 'Show publisher', 'System.Boolean', 0, 1)

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'SHOW_DATE', 'Show file date', 'System.Boolean', 0, 1)

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'PHYSICAL_DIR', 'Physical directory (empty for App_Root/files)', 'System.String', 0, 0)


go

/*********************
Version updates
*********************/

IF EXISTS (SELECT 1 FROM cuyahoga_version WHERE assembly = 'Cuyahoga.Core')
	UPDATE cuyahoga_version SET major = 0, minor = 8, patch = 0 WHERE assembly = 'Cuyahoga.Core'
ELSE
	INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Core', 0, 8, 0)
	
IF EXISTS (SELECT 1 FROM cuyahoga_version WHERE assembly = 'Cuyahoga.Modules')
	UPDATE cuyahoga_version SET major = 0, minor = 8, patch = 0 WHERE assembly = 'Cuyahoga.Modules'
ELSE
	INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Modules', 0, 8, 0)
	
IF EXISTS (SELECT 1 FROM cuyahoga_version WHERE assembly = 'Cuyahoga.Modules.Downloads')
	UPDATE cuyahoga_version SET major = 0, minor = 8, patch = 0 WHERE assembly = 'Cuyahoga.Modules.Downloads'
ELSE
	INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Modules.Downloads', 0, 8, 0)
	
go