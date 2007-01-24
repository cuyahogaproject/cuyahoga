/*
 *  Remove module definitions
 */
 
DELETE FROM cuyahoga_version WHERE assembly = 'Cuyahoga.Modules.Articles';
 
DELETE FROM cuyahoga_modulesetting
WHERE moduletypeid IN
	(SELECT mt.moduletypeid FROM cuyahoga_moduletype mt WHERE mt.assemblyname = 'Cuyahoga.Modules.Articles');
	
DELETE FROM cuyahoga_moduleservice
WHERE moduletypeid IN
	(SELECT mt.moduletypeid FROM cuyahoga_moduletype mt WHERE mt.assemblyname = 'Cuyahoga.Modules.Articles');

DELETE FROM cuyahoga_moduletype
WHERE assemblyname = 'Cuyahoga.Modules.Articles';

/*
 *  Remove module specific tables
 */

DROP TABLE cm_articlecomment;

DROP TABLE cm_article;

DROP TABLE cm_articlecategory;
