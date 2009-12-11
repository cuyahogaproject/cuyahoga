/*
 *  Remove module definitions
 */
 
DELETE FROM cuyahoga_version WHERE assembly = 'Cuyahoga.Modules.YetAnotherForum'
go
 
DELETE cuyahoga_modulesetting 
FROM cuyahoga_modulesetting ms
	INNER JOIN cuyahoga_moduletype mt ON mt.moduletypeid = ms.moduletypeid AND mt.assemblyname = 'Cuyahoga.Modules.YetAnotherForum'
go

DELETE FROM cuyahoga_moduletype
WHERE assemblyname = 'Cuyahoga.Modules.YetAnotherForum'
go