ALTER TABLE cm_file
	ADD datepublished datetime NULL

go
	
UPDATE cm_file
	SET datepublished = updatetimestamp
	
go

ALTER TABLE cm_file
	ALTER COLUMN datepublished datetime NOT NULL
	
go

/*********************
Version updates
*********************/

UPDATE cuyahoga_version SET major = 0, minor = 8, patch = 1 WHERE assembly = 'Cuyahoga.Core'
UPDATE cuyahoga_version SET major = 0, minor = 8, patch = 1 WHERE assembly = 'Cuyahoga.Modules'
UPDATE cuyahoga_version SET major = 0, minor = 8, patch = 1 WHERE assembly = 'Cuyahoga.Modules.Downloads'

go