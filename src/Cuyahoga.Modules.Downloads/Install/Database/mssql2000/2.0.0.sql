-- Add a temporary column to contentitem to store the old fileid's
ALTER TABLE cuyahoga_contentitem
	ADD fileid int NULL
go

-- copy data to contentitem
INSERT INTO cuyahoga_contentitem(fileid, globalid, workflowstatus, title, syndicate, version, createdat, modifiedat, createdby, modifiedby, publishedat, sectionid)
SELECT fileid, newid(), 0, title = CASE WHEN (title IS NULL OR title = '') THEN filepath ELSE title END, 0, 1, inserttimestamp, updatetimestamp, publisherid, publisherid, datepublished, sectionid
FROM cm_file
go

-- copy data to fileresource
INSERT INTO cuyahoga_fileresource(fileresourceid, filename, physicalfilepath, length, mimetype, downloadcount)
SELECT ci.contentitemid, f.filepath, ISNULL(ss.value, '') + f.filepath, f.filesize, f.contenttype, f.nrofdownloads
FROM cm_file f 
	INNER JOIN cuyahoga_contentitem ci ON ci.fileid = f.fileid
	LEFT OUTER JOIN cuyahoga_sectionsetting ss ON ss.sectionid = s.sectionid && ss.[name] = 'PHYSICAL_DIR'
go

-- copy data to contentitemrole
INSERT INTO cuyahoga_contentitemrole(contentitemid, roleid, viewallowed, editallowed)
SELECT ci.contentitemid, fr.roleid, 1, 0
FROM cm_filerole fr
	INNER JOIN cm_file f ON fr.fileid = f.fileid
	INNER JOIN cuyahoga_contentitem ci on f.fileid = ci.fileid
go

-- clean up
ALTER TABLE cm_filerole
	DROP CONSTRAINT FK_filerole_cuyahoga_role_roleid
go

ALTER TABLE cm_filerole
	DROP CONSTRAINT FK_filerole_file_fileid
go

DROP TABLE cm_filerole
go

ALTER TABLE cm_file
	DROP CONSTRAINT FK_file_section_sectionid
go

ALTER TABLE cm_file
	DROP CONSTRAINT FK_file_cuyahoga_user_publisherid
go

DROP TABLE cm_file
go

ALTER TABLE cuyahoga_contentitem
	DROP COLUMN fileid
go

-- Change module admin url
UPDATE cuyahoga_moduletype
SET editpath = 'Modules/Downloads/ManageFiles'
WHERE [name] = 'Downloads'
go

-- Description for physical directory
UPDATE cuyahoga_modulesetting
SET friendlyname = 'Physical directory (leave empty for default)'
FROM cuyahoga_modulesetting ms 
	INNER JOIN cuyahoga_moduletype mt ON mt.moduletypeid = ms.moduletypeid
WHERE ms.[name] = 'PHYSICAL_DIR'
	AND mt.[name] = 'Downloads'
go

/*
 * Version
 */
UPDATE cuyahoga_version SET major = 2, minor = 0, patch = 0 WHERE assembly = 'Cuyahoga.Modules.Downloads'
go
