/*
 *  Table data
 */
DECLARE @moduletypeid int

INSERT INTO cuyahoga_moduletype ([name], assemblyname, classname, path, editpath) 
VALUES ('YetAnotherForum', 'Cuyahoga.Modules.YetAnotherForum', 'Cuyahoga.Modules.YetAnotherForum.CuyahogaYafModule', 'Modules/YetAnotherForum/CuyahogaYaf.ascx', '')

SELECT @moduletypeid = Scope_Identity()

INSERT INTO cuyahoga_modulesetting (moduletypeid, [name], friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'BOARDID', 'Board ID', 'System.Int32', 0, 1)
INSERT INTO cuyahoga_modulesetting (moduletypeid, [name], friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'CATEGORYID', 'Category ID', 'System.Int32', 0, 0)

go

INSERT INTO cuyahoga_version (assembly, major, minor, patch) 
VALUES ('Cuyahoga.Modules.YetAnotherForum', 1, 5, 0)
go