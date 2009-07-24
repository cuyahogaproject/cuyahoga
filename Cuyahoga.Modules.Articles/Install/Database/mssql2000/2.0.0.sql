-- Migrate Articles to ContentItem
-- Move categories to cuyahoga_category table
-- MBO, 20090724: we're stuck here.
INSERT INTO cuyahoga_category(siteid, [position], categoryname, description)
SELECT siteid, (SELECT MAX([position]) FROM cuyahoga_category WHERE parentcategoryid IS NULL), title, summary
FROM cm_articlecategory ac
WHERE NOT EXISTS (SELECT 1 FROM cuyahoga_category cc WHERE cc.categoryname = ac.title)
go

-- Add a temporary column to contentitem to store the old statichtmlid's
ALTER TABLE cuyahoga_contentitem
	ADD articleid int NULL
go

-- copy data to contentitem
INSERT INTO cuyahoga_contentitem(articleid, globalid, workflowstatus, title, description, syndicate, version, createdat, modifiedat, createdby, modifiedby, publishedat, publisheduntil, sectionid)
SELECT articleid, newid(), 0, title, summary, syndicate, 1, inserttimestamp, updatetimestamp, createdby, modifiedby, dateonline, dateoffline, sectionid)
FROM cm_article
go

-- add new contentitemid column
ALTER TABLE cm_article
	ADD contentitemid bigint NULL
go
	
-- update contentitemid's
UPDATE cm_article
	SET contentitemid = ci.contentitemid
FROM cm_article a, cuyahoga_contentitem ci
WHERE a.articleid = ci.articleid
go

-- we can now get rid of the temporary column and clean the old cm_article, cm_ariclecategory and cm_articlecomment tables
ALTER TABLE cuyahoga_contentitem
	DROP COLUMN articleid
go

ALTER TABLE cm_article
	ADD CONSTRAINT FK_article_contentitem_contentitemid
		FOREIGN KEY (contentitemid) REFERENCES cuyahoga_contentitem (contentitemid)
go

ALTER TABLE cm_article
	ALTER COLUMN contentitemid bigint NOT NULL
go

ALTER TABLE cm_article
	DROP CONSTRAINT PK_article
go

ALTER TABLE cm_article
	ADD CONSTRAINT PK_article PRIMARY KEY (contentitemid)
go

ALTER TABLE cm_article
	DROP CONSTRAINT FK_article_section_sectionid
go

ALTER TABLE cm_article
	DROP CONSTRAINT FK_article_articlecategory_articlecategoryid
go

ALTER TABLE cm_article
	DROP CONSTRAINT FK_article_user_createdby
go

ALTER TABLE cm_article
	DROP CONSTRAINT FK_article_user_modifiedby
go

ALTER TABLE cm_article
	DROP COLUMN articleid
go

ALTER TABLE cm_article
	DROP COLUMN title
go

ALTER TABLE cm_article
	DROP COLUMN summary
go

ALTER TABLE cm_article
	DROP COLUMN syndicate
go

ALTER TABLE cm_article
	DROP COLUMN dateonline
go

ALTER TABLE cm_article
	DROP COLUMN dateoffline
go

ALTER TABLE cm_article
	DROP COLUMN sectionid
go

ALTER TABLE cm_article
	DROP COLUMN articlecategoryid
go

ALTER TABLE cm_article
	DROP COLUMN createdby
go

ALTER TABLE cm_article
	DROP COLUMN modifiedby
go

-- We don't drop the inserttimestamp and updatetimestamp columns because they have defaults and are 
-- very hard to drop because of their generated names

/*
 * Version
 */
UPDATE cuyahoga_version SET major = 2, minor = 0, patch = 0 WHERE assembly = 'Cuyahoga.Modules.Articles'
go