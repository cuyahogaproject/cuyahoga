/*
 *  Remove module definitions
 */
 
DELETE FROM cuyahoga_version WHERE assembly = 'Cuyahoga.Modules.RemoteContent';
 
DELETE FROM cuyahoga_modulesetting
WHERE moduletypeid IN
	(SELECT mt.moduletypeid FROM cuyahoga_moduletype mt WHERE mt.assemblyname = 'Cuyahoga.Modules.RemoteContent');

DELETE FROM cuyahoga_moduletype
WHERE assemblyname = 'Cuyahoga.Modules.RemoteContent';

/*
 *  Remove module specific tables
 */

DROP TABLE cm_feeditem;

DROP TABLE cm_feed;