DELETE FROM cuyahoga_version WHERE assembly = 'Cuyahoga.Modules.Forum';


DELETE FROM cuyahoga_modulesetting
WHERE moduletypeid IN (SELECT mt.moduletypeid FROM cuyahoga_moduletype mt WHERE mt.assemblyname = 'Cuyahoga.Modules.Forum');

DELETE FROM cuyahoga_moduletype
WHERE assemblyname = 'Cuyahoga.Modules.Forum';


DROP TABLE cm_forums;
DROP TABLE cm_forumposts;
DROP TABLE cm_forumcategory;
DROP TABLE cm_forumemoticon;
DROP TABLE cm_forumtag;
DROP TABLE cm_forumuser;
DROP TABLE cm_forumfile;

