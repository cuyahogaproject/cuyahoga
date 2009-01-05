/*
 *  Profile module
 */
INSERT INTO cuyahoga_moduletype ([name], assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) 
VALUES ('UserProfile', 'Cuyahoga.Modules', 'Cuyahoga.Modules.User.ProfileModule', 'Modules/User/EditProfile.ascx', NULL, '2005-10-20 14:36:28.324', '2005-10-20 14:36:28.324')
go

/*
 *  Version
 */
UPDATE cuyahoga_version SET major = 0, minor = 9, patch = 1 WHERE assembly = 'Cuyahoga.Modules'
go