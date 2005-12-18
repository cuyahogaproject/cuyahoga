/*
 *  DDL changes.
 */
ALTER TABLE cm_statichtml 
	MODIFY COLUMN content MEDIUMTEXT NOT NULL;


ALTER TABLE cm_feeditem
	MODIFY COLUMN content MEDIUMTEXT;


ALTER TABLE cm_article
	MODIFY COLUMN content MEDIUMTEXT NOT NULL;

/*
 *  Version
 */
UPDATE cuyahoga_version SET major = 1, minor = 0, patch = 0 WHERE assembly = 'Cuyahoga.Modules';