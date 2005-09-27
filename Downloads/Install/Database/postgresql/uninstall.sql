/*
 *  Remove module definitions
 */
 
DELETE FROM cuyahoga_version WHERE assembly = 'Cuyahoga.Modules.Downloads';
 
DELETE FROM cuyahoga_modulesetting
WHERE moduletypeid in 
	(SELECT mt.moduletypeid FROM cuyahoga_moduletype mt WHERE mt.assemblyname = 'Cuyahoga.Modules.Downloads');

DELETE FROM cuyahoga_moduletype
WHERE assemblyname = 'Cuyahoga.Modules.Downloads';

/*
 *  Remove module specific tables
 */

DROP TABLE cm_filerole CASCADE;

DROP TABLE cm_file CASCADE;