ALTER TABLE cuyahoga_node
	ADD COLUMN linkurl VARCHAR(255) NULL;

ALTER TABLE cuyahoga_node
	ADD COLUMN linktarget INT NULL;
	
/*********************
Version updates
*********************/

UPDATE cuyahoga_version SET major = 0, minor = 8, patch = 2 WHERE assembly = 'Cuyahoga.Core';
UPDATE cuyahoga_version SET major = 0, minor = 8, patch = 2 WHERE assembly = 'Cuyahoga.Modules';
UPDATE cuyahoga_version SET major = 0, minor = 8, patch = 2 WHERE assembly = 'Cuyahoga.Modules.Downloads';
