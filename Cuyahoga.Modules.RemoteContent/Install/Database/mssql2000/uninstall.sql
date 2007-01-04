/*
 *  Remove module definitions
 */
 
DELETE FROM cuyahoga_version WHERE assembly = 'Cuyahoga.Modules.RemoteContent'
go
 
DELETE cuyahoga_modulesetting 
FROM cuyahoga_modulesetting ms
	INNER JOIN cuyahoga_moduletype mt ON mt.moduletypeid = ms.moduletypeid AND mt.assemblyname = 'Cuyahoga.Modules.RemoteContent'
go

DELETE FROM cuyahoga_moduletype
WHERE assemblyname = 'Cuyahoga.Modules.RemoteContent'
go

/*
 *  Remove module specific tables
 */

DROP TABLE cm_feeditem
go

DROP TABLE cm_feed
go