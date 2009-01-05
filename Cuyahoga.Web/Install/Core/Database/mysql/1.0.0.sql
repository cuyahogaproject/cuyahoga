/*
 * DDL changes
 */

/* 
 * MySQL 5.0 doesn't add the defaults anymore with required fields, which resulted in an error while adding roles to users.
 * -> make the updatetimestamp nullable.
 * The timestamps in the userrole table should be removed anyway someday because they don't make that much sense.
 */
ALTER TABLE cuyahoga_userrole
	MODIFY updatetimestamp DATETIME;

/*
 * Version
 */
UPDATE cuyahoga_version SET major = 1, minor = 0, patch = 0 WHERE assembly = 'Cuyahoga.Core';