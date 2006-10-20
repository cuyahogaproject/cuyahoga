
DELETE FROM cuyahoga_version WHERE assembly = 'Cuyahoga.Modules.Forum';

DELETE FROM cuyahoga_modulesetting
WHERE moduletypeid IN
	(SELECT mt.moduletypeid FROM cuyahoga_moduletype mt WHERE mt.assemblyname = 'Cuyahoga.Modules.Forum');

DELETE FROM cuyahoga_moduletype
WHERE assemblyname = 'Cuyahoga.Modules.Forum';

DROP TABLE cm_forums CASCADE;
DROP TABLE cm_forumposts CASCADE;
DROP TABLE cm_forumcategory CASCADE;
DROP TABLE cm_forumemoticon CASCADE;
DROP TABLE cm_forumtag CASCADE;
DROP TABLE cm_forumuser CASCADE;
DROP TABLE cm_forumfile CASCADE;