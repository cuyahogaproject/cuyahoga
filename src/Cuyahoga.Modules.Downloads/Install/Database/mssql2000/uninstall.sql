/*
 *  Remove module definitions
 */
 
DELETE FROM cuyahoga_version WHERE assembly = 'Cuyahoga.Modules.Downloads'
go
 
DELETE cuyahoga_modulesetting 
FROM cuyahoga_modulesetting ms
	INNER JOIN cuyahoga_moduletype mt ON mt.moduletypeid = ms.moduletypeid AND mt.assemblyname = 'Cuyahoga.Modules.Downloads'
go

DELETE FROM cuyahoga_moduletype
WHERE assemblyname = 'Cuyahoga.Modules.Downloads'
go
