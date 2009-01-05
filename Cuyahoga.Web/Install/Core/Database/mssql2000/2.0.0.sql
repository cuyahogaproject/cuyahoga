/* DDL */ 

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
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (2, 1)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (2, 2)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (2, 3)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (2, 5)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (2, 6)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (2, 11)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (3, 1)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (3, 2)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (4, 1)

GO

/*
 * Version
 */
UPDATE cuyahoga_version SET major = 2, minor = 0, patch = 0 WHERE assembly = 'Cuyahoga.Core'
go