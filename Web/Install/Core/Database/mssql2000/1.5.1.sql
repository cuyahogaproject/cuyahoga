/*
 * DDL Changes
 */
ALTER TABLE cuyahoga_moduletype
	ADD autoactivate bit NULL DEFAULT 1
go

UPDATE cuyahoga_moduletype
SET autoactivate = 1
go

ALTER TABLE cuyahoga_moduletype
	ALTER COLUMN autoactivate bit NOT NULL
go  


/*
 * Version
 */
UPDATE cuyahoga_version SET major = 1, minor = 5, patch = 1 WHERE assembly = 'Cuyahoga.Core'
go