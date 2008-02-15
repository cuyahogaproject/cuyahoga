/*
 * DDL Changes
 */
ALTER TABLE cuyahoga_moduletype
	ADD COLUMN autoactivate TINYINT DEFAULT 1;

UPDATE cuyahoga_moduletype
	SET autoactivate = 1;

ALTER TABLE cuyahoga_moduletype
	CHANGE COLUMN autoactivate autoactivate TINYINT NOT NULL DEFAULT 1;
 
/*
 *  Version
 */
UPDATE cuyahoga_version SET major = 1, minor = 5, patch = 1 WHERE assembly = 'Cuyahoga.Core';