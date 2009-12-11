/* DDL */ 
-- Content abstraction
CREATE TABLE cuyahoga_contentitem(
contentitemid bigint identity(1,1) NOT NULL CONSTRAINT PK_contentitem PRIMARY KEY,
globalid nvarchar(255) NOT NULL,
workflowstatus int NOT NULL,
title nvarchar(255) NOT NULL,
description nvarchar(255) NULL,
version int NOT NULL,
locale nvarchar(5) NULL,
syndicate bit NOT NULL DEFAULT 1,
createdat datetime NOT NULL,
modifiedat datetime NOT NULL,
publishedat datetime NULL,
publisheduntil datetime NULL,
createdby int NOT NULL,
modifiedby int NOT NULL,
publishedby int NULL,
sectionid int NOT NULL)
go

CREATE TABLE cuyahoga_contentitemrole(
contentitemroleid int identity(1,1) NOT NULL CONSTRAINT PK_contentitemrole PRIMARY KEY,
contentitemid int NOT NULL,
roleid int NOT NULL,
viewallowed bit NOT NULL,
editallowed bit NOT NULL)
go

CREATE UNIQUE INDEX IX_contentitemrole_roleid_contentitemid ON cuyahoga_contentitemrole (roleid,contentitemid)
go

CREATE TABLE cuyahoga_category(
categoryid int identity(1,1) NOT NULL CONSTRAINT PK_category PRIMARY KEY,
siteid int NOT NULL,
parentcategoryid int NULL,
path nvarchar(80) NOT NULL,
categoryname nvarchar(100) NOT NULL,
description nvarchar(255) NULL,
position int NOT NULL)
go

CREATE UNIQUE INDEX IX_category_path_siteid ON cuyahoga_category (path, siteid)
go

CREATE UNIQUE INDEX IX_category_categoryname_siteid ON cuyahoga_category (categoryname, siteid)
go

CREATE TABLE cuyahoga_categorycontentitem(
categorycontentitemid int identity(1,1) NOT NULL CONSTRAINT PK_categorycontentitem PRIMARY KEY,
categoryid int NOT NULL,
contentitemid bigint NOT NULL)
go

CREATE TABLE cuyahoga_comment(
commentid int identity(1,1) NOT NULL CONSTRAINT PK_comment PRIMARY KEY,
contentitemid bigint NOT NULL,
userid int NULL,
commentdatetime datetime NOT NULL,
[name] nvarchar(100) NULL,
website nvarchar(100) NULL,
commenttext nvarchar(2000) NOT NULL,
userip nvarchar(15) NULL)
go

CREATE TABLE cuyahoga_fileresource(
fileresourceid bigint NOT NULL CONSTRAINT PK_fileresource PRIMARY KEY,
filename nvarchar(255) NOT NULL,
physicalfilepath nvarchar(1000) NOT NULL,
length bigint NULL,
mimetype nvarchar(255) NULL,
downloadcount int NULL)
go

ALTER TABLE cuyahoga_contentitem
ADD CONSTRAINT FK_contentitem_user_createdby 
FOREIGN KEY (createdby) REFERENCES cuyahoga_user (userid)
go

ALTER TABLE cuyahoga_contentitem
ADD CONSTRAINT FK_contentitem_user_modifiedby 
FOREIGN KEY (modifiedby) REFERENCES cuyahoga_user (userid)
go

ALTER TABLE cuyahoga_contentitem
ADD CONSTRAINT FK_contentitem_user_publishedby 
FOREIGN KEY (publishedby) REFERENCES cuyahoga_user (userid)
go

ALTER TABLE cuyahoga_contentitem
ADD CONSTRAINT FK_contentitem_section_sectionid 
FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid)
go

ALTER TABLE cuyahoga_category
ADD CONSTRAINT FK_category_category_parentcategoryid 
FOREIGN KEY (parentcategoryid) REFERENCES cuyahoga_category (categoryid)
go

ALTER TABLE cuyahoga_category
ADD CONSTRAINT FK_category_site_siteid 
FOREIGN KEY (siteid) REFERENCES cuyahoga_site (siteid)
go

ALTER TABLE cuyahoga_categorycontentitem
ADD CONSTRAINT FK_categorycontentitem_contentitem_contentitemid
FOREIGN KEY (contentitemid) REFERENCES cuyahoga_contentitem (contentitemid)
go

ALTER TABLE cuyahoga_categorycontentitem
ADD CONSTRAINT FK_categorycontentitem_category_categoryid
FOREIGN KEY (categoryid) REFERENCES cuyahoga_category (categoryid)
go

ALTER TABLE cuyahoga_comment
ADD CONSTRAINT FK_comment_contentitem_contentitemid
FOREIGN KEY (contentitemid) REFERENCES cuyahoga_contentitem (contentitemid)
go

ALTER TABLE cuyahoga_comment
ADD CONSTRAINT FK_comment_user_userid
FOREIGN KEY (userid) REFERENCES cuyahoga_user (userid)
go

ALTER TABLE cuyahoga_fileresource
ADD CONSTRAINT FK_fileresource_contentitem_fileresourceid 
FOREIGN KEY (fileresourceid) REFERENCES cuyahoga_contentitem (contentitemid)
go

-- Roles per site
ALTER TABLE cuyahoga_role
	ADD isglobal bit NULL DEFAULT 1
go

UPDATE cuyahoga_role
SET isglobal = 1
go

ALTER TABLE cuyahoga_role
	ALTER COLUMN isglobal bit NOT NULL
go 

CREATE TABLE cuyahoga_siterole(
siteid int NOT NULL,
roleid int NOT NULL,
CONSTRAINT PK_siterole PRIMARY KEY (siteid, roleid))
go

ALTER TABLE cuyahoga_siterole
	ADD CONSTRAINT FK_siterole_site_siteid 
		FOREIGN KEY (siteid) REFERENCES cuyahoga_site (siteid)
go

ALTER TABLE cuyahoga_siterole
	ADD CONSTRAINT FK_siterole_role_roleid 
		FOREIGN KEY (roleid) REFERENCES cuyahoga_role (roleid)
go

-- all existing roles will be linked to all available sites.
INSERT INTO cuyahoga_siterole(siteid, roleid)
SELECT cuyahoga_site.siteid, cuyahoga_role.roleid 
FROM cuyahoga_site, cuyahoga_role

go

-- Sections belong to a site
ALTER TABLE cuyahoga_section
	ADD siteid int NULL
go

-- sections that belong to a node inherit the siteid of the node
UPDATE cuyahoga_section
	SET siteid = n.siteid
FROM cuyahoga_section s, cuyahoga_node n
WHERE s.nodeid = n.nodeid

go

-- detached sections are moved to the site with the lowest id (usually 1, the originally created site)
UPDATE cuyahoga_section
	SET siteid = (SELECT MIN(siteid) FROM cuyahoga_site)
WHERE nodeid IS NULL

go

ALTER TABLE cuyahoga_section
	ALTER COLUMN siteid int NOT NULL
go 

ALTER TABLE cuyahoga_section
	ADD CONSTRAINT FK_section_site_siteid
		FOREIGN KEY(siteid) REFERENCES cuyahoga_site(siteid)
go

-- Template per site
ALTER TABLE cuyahoga_template
	ADD siteid int NULL
go

ALTER TABLE cuyahoga_template
	ADD CONSTRAINT FK_template_site_siteid
		FOREIGN KEY(siteid) REFERENCES cuyahoga_site(siteid)
go

-- Migrate templates to all existing sites
INSERT INTO cuyahoga_template([name], basepath, templatecontrol, css, siteid)
SELECT t.name, t.basepath, t.templatecontrol, t.css, s.siteid 
FROM cuyahoga_template t, cuyahoga_site s

go

-- Also link all sections that are connected to a template to the new templates
INSERT INTO cuyahoga_templatesection(templateid, sectionId, placeholder)
SELECT t2.templateid, ts.sectionid, ts.placeholder
FROM cuyahoga_template t1, cuyahoga_templatesection ts, cuyahoga_template t2
WHERE t1.[name] = t2.[name] 
	AND t1.templateid = ts.templateid 
	AND t2.siteid IS NOT NULL
	
go

-- Migrate templateid's of the nodes to link to the templates that belong to the site (match by template name)
UPDATE cuyahoga_node
	SET templateid = t2.templateid
FROM cuyahoga_node n, cuyahoga_template t1, cuyahoga_template t2
WHERE
	t1.templateid = n.templateid 
	AND t2.siteid = n.siteid
	AND t2.[name] = t1.[name]
	
go 
	
-- Rights 
CREATE TABLE cuyahoga_right(
rightid int identity(1,1) NOT NULL CONSTRAINT PK_right PRIMARY KEY,
name nvarchar(50) NOT NULL,
description nvarchar(255) NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_right_name UNIQUE(name))
go

CREATE TABLE cuyahoga_roleright(
roleid int NOT NULL,
rightid int NOT NULL,
CONSTRAINT PK_roleright PRIMARY KEY (roleid, rightid))
go

ALTER TABLE cuyahoga_roleright
	ADD CONSTRAINT FK_roleright_role_roleid 
		FOREIGN KEY (roleid) REFERENCES cuyahoga_role (roleid)
go

ALTER TABLE cuyahoga_roleright
	ADD CONSTRAINT FK_roleright_right_rightid 
		FOREIGN KEY (rightid) REFERENCES cuyahoga_right (rightid)
go

/* DATA */
SET IDENTITY_INSERT cuyahoga_right ON

GO

INSERT INTO cuyahoga_right (rightid, name, description) VALUES (1, 'Anonymous', 'Legacy right, migrated from AccessLevel.Anonymous')
INSERT INTO cuyahoga_right (rightid, name, description) VALUES (2, 'Authenticated', 'Legacy right, migrated from AccessLevel.Authenticated')
INSERT INTO cuyahoga_right (rightid, name, description) VALUES (3, 'Editor', 'Legacy right, migrated from AccessLevel.Editor')
INSERT INTO cuyahoga_right (rightid, name, description) VALUES (4, 'Administrator', 'Legacy right, migrated from AccessLevel.Administrator')

INSERT INTO cuyahoga_right (rightid, name, description) VALUES (5, 'Manage Pages', 'Create, edit, move and delete pages')
INSERT INTO cuyahoga_right (rightid, name, description) VALUES (6, 'Manage Files', 'Manage files')
INSERT INTO cuyahoga_right (rightid, name, description) VALUES (7, 'Manage Users', 'Manage users and roles')
INSERT INTO cuyahoga_right (rightid, name, description) VALUES (8, 'Manage Site', 'Manage site properties')
INSERT INTO cuyahoga_right (rightid, name, description) VALUES (9, 'Manage Server', 'Manage server properties')
INSERT INTO cuyahoga_right (rightid, name, description) VALUES (10, 'Global Permissions', 'Manage permissions that are shared between sites')
INSERT INTO cuyahoga_right (rightid, name, description) VALUES (11, 'Access Admin', 'Access site administration')
INSERT INTO cuyahoga_right (rightid, name, description) VALUES (12, 'Create Site', 'Create a new site')
INSERT INTO cuyahoga_right (rightid, name, description) VALUES (13, 'Manage Templates', 'Manage templates')
INSERT INTO cuyahoga_right (rightid, name, description) VALUES (14, 'Access Root Data Folder', 'Access root data folder')
INSERT INTO cuyahoga_right (rightid, name, description) VALUES (15, 'Create Directory', 'Create a new directory')
INSERT INTO cuyahoga_right (rightid, name, description) VALUES (16, 'Copy Files', 'Copy files and folders')
INSERT INTO cuyahoga_right (rightid, name, description) VALUES (17, 'Move Files', 'Move files and folders')
INSERT INTO cuyahoga_right (rightid, name, description) VALUES (18, 'Delete Files', 'Delete files and folders')

GO

SET IDENTITY_INSERT cuyahoga_right OFF

GO

INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (1, 1)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (1, 2)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (1, 3)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (1, 4)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (1, 5)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (1, 6)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (1, 7)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (1, 8)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (1, 9)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (1, 10)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (1, 11)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (1, 12)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (1, 13)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (1, 14)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (1, 15)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (1, 16)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (1, 17)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (1, 18)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (2, 1)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (2, 2)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (2, 3)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (2, 5)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (2, 6)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (2, 11)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (2, 15)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (2, 16)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (2, 17)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (3, 1)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (3, 2)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (4, 1)

GO

/*
 * Version
 */
UPDATE cuyahoga_version SET major = 2, minor = 0, patch = 0 WHERE assembly = 'Cuyahoga.Core'
go