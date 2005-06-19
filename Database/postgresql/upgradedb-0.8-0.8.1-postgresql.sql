ALTER TABLE cm_file
	ADD COLUMN datepublished timestamp;
	
UPDATE cm_file
	SET datepublished = updatetimestamp;
	
ALTER TABLE cm_file
	ALTER COLUMN datepublished SET NOT NULL;
	
/*********************
Version updates
*********************/

UPDATE cuyahoga_version SET major = 0, minor = 8, patch = 1 WHERE assembly = 'Cuyahoga.Core';
UPDATE cuyahoga_version SET major = 0, minor = 8, patch = 1 WHERE assembly = 'Cuyahoga.Modules';
UPDATE cuyahoga_version SET major = 0, minor = 8, patch = 1 WHERE assembly = 'Cuyahoga.Modules.Downloads';