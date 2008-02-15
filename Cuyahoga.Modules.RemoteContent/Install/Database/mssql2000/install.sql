/*
 *  Table structure
 */

CREATE TABLE cm_feed(
feedid int identity(1,1) NOT NULL CONSTRAINT PK_feed PRIMARY KEY,
sectionid int NOT NULL,
url nvarchar(255) NOT NULL,
title nvarchar(100) NOT NULL,
pubdate datetime NOT NULL,
numberofitems int NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go


CREATE TABLE cm_feeditem(
feeditemid int identity(1,1) NOT NULL CONSTRAINT PK_feeditem PRIMARY KEY,
feedid int NOT NULL,
url nvarchar(255) NOT NULL,
title nvarchar(100) NOT NULL,
content ntext NULL,
pubdate datetime NOT NULL,
author nvarchar(100) NULL)
go


ALTER TABLE cm_feed
ADD CONSTRAINT FK_feed_section_sectionid
FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid)
go


ALTER TABLE cm_feeditem
ADD CONSTRAINT FK_feeditem_feed_feedid
FOREIGN KEY (feedid) REFERENCES cm_feed (feedid)
go


/*
 *  Table data
 */
SET DATEFORMAT ymd


DECLARE @moduletypeid int

INSERT INTO cuyahoga_moduletype ([name], assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) VALUES ('RemoteContent', 'Cuyahoga.Modules.RemoteContent', 'Cuyahoga.Modules.RemoteContent.RemoteContentModule', 'Modules/RemoteContent/RemoteContent.ascx', 'Modules/RemoteContent/AdminRemoteContent.aspx', '2005-04-08 14:36:28.324', '2004-04-08 14:36:28.324')

SELECT @moduletypeid = Scope_Identity()

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (@moduletypeid, 'CACHE_DURATION', 'Local database cache duration (min)', 'System.Int32', 0, 1)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (@moduletypeid, 'SHOW_CONTENTS', 'Show feed contents', 'System.Boolean', 0, 1)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (@moduletypeid, 'SHOW_DATES', 'Show dates', 'System.Boolean', 0, 1)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (@moduletypeid, 'BACKGROUND_REFRESH', 'Use background refreshing', 'System.Boolean', 0, 1)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (@moduletypeid, 'SHOW_SOURCES', 'Show feed sources', 'System.Boolean', 0, 1)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (@moduletypeid, 'SHOW_AUTHORS', 'Show authors', 'System.Boolean', 0, 1)

go

INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Modules.RemoteContent', 1, 5, 1)
go