INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
SELECT mt.moduletypeid, 'SHOW_NUMBER_OF_DOWNLOADS', 'Show number of downloads', 'System.Boolean', 0, 0
FROM cuyahoga_moduletype mt
WHERE mt.assemblyname = 'Cuyahoga.Modules.Downloads'
go

UPDATE cuyahoga_version SET major = 0, minor = 9, patch = 0 WHERE assembly = 'Cuyahoga.Modules.Downloads'
go