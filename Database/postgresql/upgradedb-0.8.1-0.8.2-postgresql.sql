ALTER TABLE cuyahoga_node
	ADD COLUMN linkurl varchar(255) NULL;

ALTER TABLE cuyahoga_node
	ADD COLUMN linktarget int4 NULL;
	
/*********************
Version updates
*********************/

UPDATE cuyahoga_version SET major = 0, minor = 8, patch = 2 WHERE assembly = 'Cuyahoga.Core';
UPDATE cuyahoga_version SET major = 0, minor = 8, patch = 2 WHERE assembly = 'Cuyahoga.Modules';
UPDATE cuyahoga_version SET major = 0, minor = 8, patch = 2 WHERE assembly = 'Cuyahoga.Modules.Downloads';