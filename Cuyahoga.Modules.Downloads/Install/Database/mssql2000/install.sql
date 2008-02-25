/*
 *  Table structure
 */
 
CREATE TABLE cm_file(
fileid int identity(1,1) NOT NULL CONSTRAINT PK_file PRIMARY KEY,
sectionid int NOT NULL,
publisherid int NOT NULL,
filepath nvarchar(255) NOT NULL,
title nvarchar(100) NULL,
filesize int NOT NULL,
nrofdownloads int NOT NULL,
contenttype nvarchar(50) NOT NULL,
datepublished datetime NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime NOT NULL)
go

CREATE TABLE cm_filerole(
fileroleid int identity(1,1) NOT NULL CONSTRAINT PK_filerole PRIMARY KEY,
fileid int NOT NULL,
roleid int NOT NULL)
go


ALTER TABLE cm_file
ADD CONSTRAINT FK_file_section_sectionid
FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid)
go

ALTER TABLE cm_file
ADD CONSTRAINT FK_file_cuyahoga_user_publisherid
FOREIGN KEY (publisherid) REFERENCES cuyahoga_user (userid)
go


ALTER TABLE cm_filerole
ADD CONSTRAINT FK_filerole_file_fileid
FOREIGN KEY (fileid) REFERENCES cm_file (fileid)
go

ALTER TABLE cm_filerole
ADD CONSTRAINT FK_filerole_cuyahoga_role_roleid
FOREIGN KEY (roleid) REFERENCES cuyahoga_role (roleid)
go

/*
 *  Table data
 */
SET DATEFORMAT ymd


DECLARE @moduletypeid int

INSERT INTO cuyahoga_moduletype (name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) VALUES ('Downloads', 'Cuyahoga.Modules.Downloads', 'Cuyahoga.Modules.Downloads.DownloadsModule', 'Modules/Downloads/Downloads.ascx', 'Modules/Downloads/EditDownloads.aspx', '2005-05-15 14:36:28.324', '2004-05-15 14:36:28.324')

SELECT @moduletypeid = Scope_Identity()

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (@moduletypeid, 'SHOW_PUBLISHER', 'Show publisher', 'System.Boolean', 0, 1)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (@moduletypeid, 'SHOW_DATE', 'Show file date', 'System.Boolean', 0, 1)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (@moduletypeid, 'PHYSICAL_DIR', 'Physical directory (empty for App_Root/files)', 'System.String', 0, 0)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (@moduletypeid, 'SHOW_NUMBER_OF_DOWNLOADS', 'Show number of downloads', 'System.Boolean', 0, 0)
go

INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Modules.Downloads', 1, 5, 2)
go