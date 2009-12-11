-- Migrate Articles to ContentItem
-- Move categories to cuyahoga_category table
-- Create a temp table and copy the categories that need to be moved to that table. We need this for proper position generation.
CREATE TABLE #tempcategory(id int identity(1,1), categoryname nvarchar(100))

DECLARE @currentMaxPosition int
SELECT @currentMaxPosition = MAX([position]) FROM cuyahoga_category WHERE parentcategoryid IS NULL 

INSERT INTO #tempcategory(categoryname)
SELECT ac.title
FROM cm_articlecategory ac
WHERE NOT EXISTS (SELECT 1 FROM cuyahoga_category cc WHERE cc.categoryname = ac.title)

INSERT INTO cuyahoga_category(siteid, [position], categoryname, description, path)
SELECT siteid, @currentMaxPosition + tc.id, title, summary, '.' + right('0000' + convert(nvarchar,(@currentMaxPosition + tc.id)), 4)
FROM cm_articlecategory ac
	INNER JOIN #tempcategory tc ON tc.categoryname = ac.title

DROP TABLE #tempcategory
go

-- Add a temporary column to contentitem to store the old articleid's
ALTER TABLE cuyahoga_contentitem
	ADD articleid int NULL
go

-- copy data to contentitem
INSERT INTO cuyahoga_contentitem(articleid, globalid, workflowstatus, title, description, syndicate, version, createdat, modifiedat, createdby, modifiedby, publishedat, publisheduntil, sectionid)
SELECT articleid, newid(), 0, title, summary, syndicate, 1, inserttimestamp, updatetimestamp, createdby, modifiedby, dateonline, dateoffline, sectionid
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

-- update references to categories
INSERT INTO cuyahoga_categorycontentitem(categoryid, contentitemid)
SELECT cc.categoryid, ci.contentitemid
FROM cuyahoga_contentitem ci
	INNER JOIN cm_article a ON a.articleid = ci.articleid
	INNER JOIN cm_articlecategory ac ON ac.articlecategoryid = a.articlecategoryid
	INNER JOIN cuyahoga_category cc ON cc.categoryname = ac.title
go
	
-- move the comments
INSERT INTO cuyahoga_comment(contentitemid, userid, commentdatetime, [name], website, commenttext, userip)
SELECT ci.contentitemid, ac.userid, ac.updatetimestamp, ac.[name], ac.website, ac.commenttext, ac.userip
FROM cuyahoga_contentitem ci
	INNER JOIN cm_articlecomment ac ON ac.articleid = ci.articleid
go

-- we can now get rid of the temporary column and clean the old cm_article, cm_ariclecategory and cm_articlecomment tables

-- Drop old article comments
ALTER TABLE cm_articlecomment
	DROP CONSTRAINT FK_articlecomment_article_articleid
go

ALTER TABLE cm_articlecomment
	DROP CONSTRAINT FK_articlecomment_user_userid
go

DROP TABLE cm_articlecomment
go

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

-- Drop old article categories
ALTER TABLE cm_articlecategory
	DROP CONSTRAINT FK_articlecategory_site_siteid
go

DROP TABLE cm_articlecategory
go

-- Remove IArticelDao from moduleservice table
DELETE FROM cuyahoga_moduleservice
WHERE servicekey = 'articles.articledao';
go

-- Change module admin url
UPDATE cuyahoga_moduletype
SET editpath = 'Modules/Articles/ManageArticles'
WHERE [name] = 'Articles'
go

/*
 * Version
 */
UPDATE cuyahoga_version SET major = 2, minor = 0, patch = 0 WHERE assembly = 'Cuyahoga.Modules.Articles'
go