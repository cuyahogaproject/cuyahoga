/*
 *  Remove module definitions
 */
 
DELETE FROM cuyahoga_version WHERE assembly = 'Cuyahoga.Modules.Articles'
go
 
DELETE cuyahoga_modulesetting 
FROM cuyahoga_modulesetting ms
	INNER JOIN cuyahoga_moduletype mt ON mt.moduletypeid = ms.moduletypeid AND mt.assemblyname = 'Cuyahoga.Modules.Articles'
go

DELETE cuyahoga_moduleservice
FROM cuyahoga_moduleservice ms
	INNER JOIN cuyahoga_moduletype mt ON mt.moduletypeid = ms.moduletypeid AND mt.assemblyname = 'Cuyahoga.Modules.Articles'
go


DELETE FROM cuyahoga_moduletype
WHERE assemblyname = 'Cuyahoga.Modules.Articles'
go

/*
 *  Remove module specific tables
 */

DROP TABLE cm_articlecomment
go

DROP TABLE cm_article
go

DROP TABLE cm_articlecategory
go