/*
 *  Table structure
 *  From Cuyahoga 2.0, the Downloads module uses the core contentitem and fileresource tables.
 */

/*
 *  Table data
 */
DECLARE @moduletypeid int

INSERT INTO cuyahoga_moduletype (name, assemblyname, classname, path, editpath) 
VALUES ('Downloads', 'Cuyahoga.Modules.Downloads', 'Cuyahoga.Modules.Downloads.DownloadsModule', 'Modules/Downloads/Downloads.ascx', 'Modules/Downloads/ManageFiles')

SELECT @moduletypeid = Scope_Identity()

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'SHOW_PUBLISHER', 'Show publisher', 'System.Boolean', 0, 1)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'SHOW_DATE', 'Show file date', 'System.Boolean', 0, 1)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'PHYSICAL_DIR', 'Physical directory (leave empty for default)', 'System.String', 0, 0)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'SHOW_NUMBER_OF_DOWNLOADS', 'Show number of downloads', 'System.Boolean', 0, 0)
go

INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Modules.Downloads', 2, 0, 0)
go