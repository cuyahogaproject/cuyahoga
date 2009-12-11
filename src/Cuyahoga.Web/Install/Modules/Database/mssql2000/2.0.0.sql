-- Migrate StaticHtml to ContentItem
-- First add a temporary column to contentitem to store the old statichtmlid's
ALTER TABLE cuyahoga_contentitem
	ADD statichtmlid int NULL
go

-- copy data to contentitem
INSERT INTO cuyahoga_contentitem(statichtmlid, globalid, workflowstatus, title, version, createdat, modifiedat, createdby, modifiedby, sectionid)
SELECT statichtmlid, newid(), 0, title, 1, inserttimestamp, updatetimestamp, createdby, modifiedby, sectionid
FROM cm_statichtml
go

-- add new contentitemid column
ALTER TABLE cm_statichtml
	ADD contentitemid bigint NULL
go
	
-- update contentitemid's
UPDATE cm_statichtml
	SET contentitemid = ci.contentitemid
FROM cm_statichtml sh, cuyahoga_contentitem ci
WHERE sh.statichtmlid = ci.statichtmlid
go

-- we can now get rid of the temporary column and clean the old cm_statichtml table
ALTER TABLE cuyahoga_contentitem
	DROP COLUMN statichtmlid
go

ALTER TABLE cm_statichtml
	ADD CONSTRAINT FK_statichtml_contentitem_contentitemid
		FOREIGN KEY (contentitemid) REFERENCES cuyahoga_contentitem (contentitemid)
go

ALTER TABLE cm_statichtml
	ALTER COLUMN contentitemid bigint NOT NULL
go

ALTER TABLE cm_statichtml
	DROP CONSTRAINT PK_statichtml
go

ALTER TABLE cm_statichtml
	ADD CONSTRAINT PK_statichtml PRIMARY KEY (contentitemid)
go

ALTER TABLE cm_statichtml
	DROP CONSTRAINT FK_statichtml_section_sectionid
go

ALTER TABLE cm_statichtml
	DROP CONSTRAINT FK_statichtml_user_createdby
go

ALTER TABLE cm_statichtml
	DROP CONSTRAINT FK_statichtml_user_modifiedby
go

ALTER TABLE cm_statichtml
	DROP COLUMN statichtmlid
go

ALTER TABLE cm_statichtml
	DROP COLUMN title
go

ALTER TABLE cm_statichtml
	DROP COLUMN sectionid
go

ALTER TABLE cm_statichtml
	DROP COLUMN createdby
go

ALTER TABLE cm_statichtml
	DROP COLUMN modifiedby
go

-- We don't drop the inserttimestamp and updatetimestamp columns because they have defaults and are 
-- very hard to drop because of their generated names

-- Change module admin url
UPDATE cuyahoga_moduletype
SET editpath = 'Modules/StaticHtml/ManageContent/Edit'
WHERE [name] = 'StaticHtml'
go

/*
 * Version
 */
UPDATE cuyahoga_version SET major = 2, minor = 0, patch = 0 WHERE assembly = 'Cuyahoga.Modules'
go