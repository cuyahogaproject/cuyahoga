ALTER TABLE cuyahoga_node
	ADD linkurl varchar(255) NULL

go

ALTER TABLE cuyahoga_node
	ADD linktarget int NULL

go
	
/*********************
Version updates
*********************/

UPDATE cuyahoga_version SET major = 0, minor = 8, patch = 2 WHERE assembly = 'Cuyahoga.Core'
UPDATE cuyahoga_version SET major = 0, minor = 8, patch = 2 WHERE assembly = 'Cuyahoga.Modules'
UPDATE cuyahoga_version SET major = 0, minor = 8, patch = 2 WHERE assembly = 'Cuyahoga.Modules.Downloads'

go