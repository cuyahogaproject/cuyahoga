/*
 *  DDL Changes
 */
ALTER TABLE cuyahoga_moduletype
	ADD COLUMN autoactivate bool DEFAULT true;

UPDATE cuyahoga_moduletype
	SET autoactivate = true;

ALTER TABLE cuyahoga_moduletype
	ALTER COLUMN autoactivate SET NOT NULL;

		
/*
 *  Version
 */
UPDATE cuyahoga_version SET major = 1, minor = 5, patch = 1 WHERE assembly = 'Cuyahoga.Core';