DELETE FROM cuyahoga_version WHERE assembly = 'Cuyahoga.Modules.Forum'
go
 
DELETE cuyahoga_modulesetting 
FROM cuyahoga_modulesetting ms
	INNER JOIN cuyahoga_moduletype mt ON mt.moduletypeid = ms.moduletypeid AND mt.assemblyname = 'Cuyahoga.Modules.Forum'
go
 
DELETE FROM cuyahoga_moduletype
WHERE assemblyname = 'Cuyahoga.Modules.Forum'
go

DROP TABLE cm_forums
go

DROP TABLE cm_forumposts
go

DROP TABLE cm_forumcategory
go

DROP TABLE cm_forumemoticon
go

DROP TABLE cm_forumtag
go

DROP TABLE cm_forumuser
go

DROP TABLE cm_forumfile
go
