CREATE TABLE cuyahoga_contentitem(
contentitemid bigint identity(1,1) NOT NULL CONSTRAINT PK_contentitem PRIMARY KEY,
globalid nvarchar(255) NOT NULL,
workflowstatus int NOT NULL,
title nvarchar(255) NOT NULL,
description nvarchar(255) NULL,
version int NOT NULL,
locale nvarchar(5) NULL,
createdat datetime NOT NULL,
modifiedat datetime NOT NULL,
publishedat datetime NULL,
publisheduntil datetime NULL,
createdby int NOT NULL,
modifiedby int NOT NULL,
publishedby int NULL,
urlformat nvarchar(255) NOT NULL,
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
path nvarchar(80) NOT NULL,
categorykey nvarchar(10) NOT NULL,
categoryname nvarchar(100) NOT NULL,
description nvarchar(255) NULL,
parentcategoryid int NULL,
CONSTRAINT UC_category_catagorykey UNIQUE(categorykey))
go


CREATE TABLE cuyahoga_categorycontentitem(
categorycontentitemid int identity(1,1) NOT NULL CONSTRAINT PK_categorycontentitem PRIMARY KEY,
categoryid int NOT NULL,
contentitemid bigint NOT NULL)
go


CREATE TABLE cuyahoga_fileresource(
fileresourceid bigint NOT NULL CONSTRAINT PK_fileresource PRIMARY KEY,
physicalpath nvarchar(255) NOT NULL,
length bigint NULL,
mimetype nvarchar(255) NULL,
filename nvarchar(255) NULL,
extension nvarchar(10) NULL,
downloadcount int NULL)
go


CREATE TABLE cuyahoga_fileresourceuserattributes(
fileresourceuserattributesid int identity(1,1) NOT NULL CONSTRAINT PK_fileresourceuserattributes PRIMARY KEY,
fileresourceid bigint NOT NULL,
attributekey nvarchar(50) NOT NULL,
attributevalue nvarchar(255) NOT NULL)
go


CREATE TABLE cuyahoga_fileresourcerole(
fileresourceroleid int identity(1,1) NOT NULL CONSTRAINT PK_fileresourcerole PRIMARY KEY,
fileresourceid bigint NOT NULL,
roleid int NOT NULL)
go


CREATE TABLE cuyahoga_user(
userid int identity(1,1) NOT NULL CONSTRAINT PK_user PRIMARY KEY,
username nvarchar(50) NOT NULL,
password nvarchar(100) NOT NULL,
firstname nvarchar(100) NULL,
lastname nvarchar(100) NULL,
email nvarchar(100) NOT NULL,
website nvarchar(100) NULL,
timezone int DEFAULT 0 NOT NULL,
isactive bit NULL,
lastlogin datetime NULL,
lastip nvarchar(40) NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_user_username UNIQUE(username))
go

CREATE TABLE cuyahoga_role(
roleid int identity(1,1) NOT NULL CONSTRAINT PK_role PRIMARY KEY,
name nvarchar(50) NOT NULL,
permissionlevel int DEFAULT 1 NOT NULL,
isglobal bit NOT NULL DEFAULT 1,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_role_name UNIQUE(name))
go

CREATE TABLE cuyahoga_userrole(
userroleid int identity(1,1) NOT NULL CONSTRAINT PK_userrole PRIMARY KEY,
userid int NOT NULL,
roleid int NOT NULL)
go

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

CREATE TABLE cuyahoga_template(
templateid int identity(1,1) NOT NULL CONSTRAINT PK_template PRIMARY KEY,
siteid int NULL,
name nvarchar(100) NOT NULL,
basepath nvarchar(100) NOT NULL,
templatecontrol nvarchar(50) NOT NULL,
css nvarchar(100) NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go


CREATE TABLE cuyahoga_moduletype(
moduletypeid int identity(1,1) NOT NULL CONSTRAINT PK_moduletype PRIMARY KEY,
name nvarchar(100) NOT NULL,
assemblyname nvarchar(100) NULL,
classname nvarchar(255) NOT NULL,
path nvarchar(255) NOT NULL,
editpath nvarchar(255) NULL,
autoactivate bit NOT NULL DEFAULT 1,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_moduletype_classname UNIQUE(classname))
go


CREATE TABLE cuyahoga_modulesetting(
modulesettingid int identity(1,1) NOT NULL CONSTRAINT PK_modulesetting PRIMARY KEY,
moduletypeid int NOT NULL,
name nvarchar(50) NOT NULL,
friendlyname nvarchar(50) NOT NULL,
settingdatatype nvarchar(100) NOT NULL,
iscustomtype bit NOT NULL,
isrequired bit NOT NULL)
go

CREATE UNIQUE INDEX IX_modulesetting_moduletypeid_name ON cuyahoga_modulesetting (moduletypeid,name)
go

CREATE TABLE cuyahoga_moduleservice(
moduleserviceid int identity(1,1) NOT NULL CONSTRAINT PK_moduleservice PRIMARY KEY,
moduletypeid int NOT NULL,
servicekey nvarchar(50) NOT NULL,
servicetype nvarchar(255) NOT NULL,
classtype nvarchar(255) NOT NULL,
lifestyle nvarchar(10) NULL)
go

CREATE UNIQUE INDEX IX_moduleservice_moduletypeid_servicekey ON cuyahoga_moduleservice (moduletypeid,servicekey)
go

CREATE TABLE cuyahoga_site(
siteid int identity(1,1) NOT NULL CONSTRAINT PK_site PRIMARY KEY,
templateid int NULL,
roleid int NOT NULL,
name nvarchar(100) NOT NULL,
homeurl nvarchar(100) NOT NULL,
defaultculture nvarchar(8) NOT NULL,
defaultplaceholder nvarchar(100) NULL,
webmasteremail nvarchar(100) NOT NULL,
usefriendlyurls bit NULL,
metakeywords nvarchar(500) NULL,
metadescription nvarchar(500) NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_site_name UNIQUE(name))
go

CREATE TABLE cuyahoga_siterole(
siteid int NOT NULL,
roleid int NOT NULL,
CONSTRAINT PK_siterole PRIMARY KEY (siteid, roleid))
go

CREATE TABLE cuyahoga_node(
nodeid int identity(1,1) NOT NULL CONSTRAINT PK_node PRIMARY KEY,
parentnodeid int NULL,
templateid int NULL,
siteid int NOT NULL,
title nvarchar(255) NOT NULL,
shortdescription nvarchar(255) NOT NULL,
position int DEFAULT 0 NOT NULL,
culture nvarchar(8) NOT NULL,
showinnavigation bit NOT NULL,
linkurl nvarchar(255) NULL,
linktarget int NULL,
metakeywords nvarchar(500) NULL,
metadescription nvarchar(500) NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go

CREATE UNIQUE INDEX IX_node_shortdescription_siteid ON cuyahoga_node (shortdescription,siteid)
go

CREATE TABLE cuyahoga_menu(
menuid int identity(1,1) NOT NULL CONSTRAINT PK_menu PRIMARY KEY,
rootnodeid int NOT NULL,
name nvarchar(50) NOT NULL,
placeholder nvarchar(50) NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go


CREATE TABLE cuyahoga_menunode(
menunodeid int identity(1,1) NOT NULL CONSTRAINT PK_menunode PRIMARY KEY,
menuid int NOT NULL,
nodeid int NOT NULL,
position int NOT NULL)
go


CREATE TABLE cuyahoga_sitealias(
sitealiasid int identity(1,1) NOT NULL CONSTRAINT PK_sitealias PRIMARY KEY,
siteid int NOT NULL,
nodeid int NULL,
url nvarchar(100) NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go


CREATE TABLE cuyahoga_section(
sectionid int identity(1,1) NOT NULL CONSTRAINT PK_section PRIMARY KEY,
nodeid int NULL,
moduletypeid int NOT NULL,
title nvarchar(100) NOT NULL,
showtitle bit DEFAULT 1 NOT NULL,
placeholder nvarchar(100) NULL,
position int DEFAULT 0 NOT NULL,
cacheduration int NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go


CREATE TABLE cuyahoga_sectionsetting(
sectionsettingid int identity(1,1) NOT NULL CONSTRAINT PK_sectionsetting PRIMARY KEY,
sectionid int NOT NULL,
name nvarchar(50) NOT NULL,
value nvarchar(100) NULL)
go

CREATE UNIQUE INDEX IX_sectionsetting_sectionid_name ON cuyahoga_sectionsetting (sectionid,name)
go

CREATE TABLE cuyahoga_sectionconnection(
sectionconnectionid int identity(1,1) NOT NULL CONSTRAINT PK_sectionconnection PRIMARY KEY,
sectionidfrom int NOT NULL,
sectionidto int NOT NULL,
actionname nvarchar(50) NOT NULL)
go

CREATE UNIQUE INDEX IX_sectionconnection_sectionidfrom_actionname ON cuyahoga_sectionconnection (sectionidfrom, actionname)
go

CREATE TABLE cuyahoga_templatesection(
templatesectionid int identity(1,1) NOT NULL CONSTRAINT PK_templatesection PRIMARY KEY,
templateid int NOT NULL,
sectionid int NOT NULL,
placeholder nvarchar(100) NOT NULL)
go

CREATE UNIQUE INDEX IX_templatesection_templateidid_placeholder ON cuyahoga_templatesection (templateid, placeholder)
go

CREATE TABLE cuyahoga_noderole(
noderoleid int identity(1,1) NOT NULL CONSTRAINT PK_noderole PRIMARY KEY,
nodeid int NOT NULL,
roleid int NOT NULL,
viewallowed bit NOT NULL,
editallowed bit NOT NULL)
go

CREATE UNIQUE INDEX IX_noderole_nodeid_roleid ON cuyahoga_noderole (nodeid,roleid)
go

CREATE TABLE cuyahoga_sectionrole(
sectionroleid int identity(1,1) NOT NULL CONSTRAINT PK_sectionrole PRIMARY KEY,
sectionid int NOT NULL,
roleid int NOT NULL,
viewallowed bit NOT NULL,
editallowed bit NOT NULL)
go

CREATE UNIQUE INDEX IX_sectionrole_roleid_sectionid ON cuyahoga_sectionrole (roleid,sectionid)
go


CREATE TABLE cuyahoga_version(
versionid int identity(1,1) NOT NULL CONSTRAINT PK_version PRIMARY KEY,
assembly nvarchar(255) NOT NULL,
major int NOT NULL,
minor int NOT NULL,
patch int NOT NULL)

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

ALTER TABLE cuyahoga_categorycontentitem
ADD CONSTRAINT FK_categorycontentitem_contentitem_contentitemid
FOREIGN KEY (contentitemid) REFERENCES cuyahoga_contentitem (contentitemid)
go

ALTER TABLE cuyahoga_categorycontentitem
ADD CONSTRAINT FK_categorycontentitem_category_categoryid
FOREIGN KEY (categoryid) REFERENCES cuyahoga_category (categoryid)
go

ALTER TABLE cuyahoga_fileresource
ADD CONSTRAINT FK_fileresource_contentitem_fileresourceid 
FOREIGN KEY (fileresourceid) REFERENCES cuyahoga_contentitem (contentitemid)
go

ALTER TABLE cuyahoga_fileresourceuserattributes
ADD CONSTRAINT FK_fileresourceuserattributes_fileresource_fileresourceid 
FOREIGN KEY (fileresourceid) REFERENCES cuyahoga_fileresource (fileresourceid)
go

ALTER TABLE cuyahoga_fileresourcerole
ADD CONSTRAINT FK_fileresourcerole_fileresource_fileresourceid 
FOREIGN KEY (fileresourceid) REFERENCES cuyahoga_fileresource (fileresourceid)
go

ALTER TABLE cuyahoga_fileresourcerole
ADD CONSTRAINT FK_fileresourcerole_role_roleid 
FOREIGN KEY (roleid) REFERENCES cuyahoga_role (roleid)
go


ALTER TABLE cuyahoga_userrole
ADD CONSTRAINT FK_userrole_role_roleid
FOREIGN KEY (roleid) REFERENCES cuyahoga_role (roleid)
go

ALTER TABLE cuyahoga_userrole
ADD CONSTRAINT FK_user_userid
FOREIGN KEY (userid) REFERENCES cuyahoga_user (userid)
go

ALTER TABLE cuyahoga_roleright
	ADD CONSTRAINT FK_roleright_role_roleid 
		FOREIGN KEY (roleid) REFERENCES cuyahoga_role (roleid)
go

ALTER TABLE cuyahoga_roleright
	ADD CONSTRAINT FK_roleright_right_rightid 
		FOREIGN KEY (rightid) REFERENCES cuyahoga_right (rightid)
go

ALTER TABLE cuyahoga_modulesetting
ADD CONSTRAINT FK_modulesetting_moduletype_moduletypeid
FOREIGN KEY (moduletypeid) REFERENCES cuyahoga_moduletype (moduletypeid)
go


ALTER TABLE cuyahoga_moduleservice
ADD CONSTRAINT FK_moduleservice_moduletype_moduletypeid
FOREIGN KEY (moduletypeid) REFERENCES cuyahoga_moduletype (moduletypeid)
go

ALTER TABLE cuyahoga_site
ADD CONSTRAINT FK_site_role_roleid
FOREIGN KEY (roleid) REFERENCES cuyahoga_role (roleid)
go

ALTER TABLE cuyahoga_site
ADD CONSTRAINT FK_site_template_templateid
FOREIGN KEY (templateid) REFERENCES cuyahoga_template (templateid)
go

ALTER TABLE cuyahoga_siterole
	ADD CONSTRAINT FK_siterole_site_siteid 
		FOREIGN KEY (siteid) REFERENCES cuyahoga_site (siteid)
go

ALTER TABLE cuyahoga_siterole
	ADD CONSTRAINT FK_siterole_role_roleid 
		FOREIGN KEY (roleid) REFERENCES cuyahoga_role (roleid)
go

ALTER TABLE cuyahoga_node
ADD CONSTRAINT FK_node_node_parentnodeid 
FOREIGN KEY (parentnodeid) REFERENCES cuyahoga_node (nodeid)
go

ALTER TABLE cuyahoga_node
ADD CONSTRAINT FK_node_site_siteid
FOREIGN KEY (siteid) REFERENCES cuyahoga_site (siteid)
go

ALTER TABLE cuyahoga_node
ADD CONSTRAINT FK_node_template_templateid
FOREIGN KEY (templateid) REFERENCES cuyahoga_template (templateid)
go


ALTER TABLE cuyahoga_menu
ADD CONSTRAINT FK_menu_node_rootnodeid
FOREIGN KEY (rootnodeid) REFERENCES cuyahoga_node (nodeid)
go


ALTER TABLE cuyahoga_menunode
ADD CONSTRAINT FK_menunode_menu_menuid
FOREIGN KEY (menuid) REFERENCES cuyahoga_menu (menuid)
go

ALTER TABLE cuyahoga_menunode
ADD CONSTRAINT FK_menunode_node_nodeid
FOREIGN KEY (nodeid) REFERENCES cuyahoga_node (nodeid)
go


ALTER TABLE cuyahoga_sitealias
ADD CONSTRAINT FK_sitealias_node_nodeid
FOREIGN KEY (nodeid) REFERENCES cuyahoga_node (nodeid)
go

ALTER TABLE cuyahoga_sitealias
ADD CONSTRAINT FK_sitealias_site_siteid
FOREIGN KEY (siteid) REFERENCES cuyahoga_site (siteid)
go


ALTER TABLE cuyahoga_section
ADD CONSTRAINT FK_section_moduletype_moduletypeid
FOREIGN KEY (moduletypeid) REFERENCES cuyahoga_moduletype (moduletypeid)
go

ALTER TABLE cuyahoga_section
ADD CONSTRAINT FK_section_node_nodeid
FOREIGN KEY (nodeid) REFERENCES cuyahoga_node (nodeid)
go


ALTER TABLE cuyahoga_sectionsetting
ADD CONSTRAINT FK_sectionsetting_section_sectionid
FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid)
go

ALTER TABLE cuyahoga_sectionconnection
ADD CONSTRAINT FK_sectionconnection_section_sectionidfrom
FOREIGN KEY (sectionidfrom) REFERENCES cuyahoga_section (sectionid)
go

ALTER TABLE cuyahoga_sectionconnection
ADD CONSTRAINT FK_sectionconnection_section_sectionidto
FOREIGN KEY (sectionidto) REFERENCES cuyahoga_section (sectionid)
go

ALTER TABLE cuyahoga_template
ADD CONSTRAINT FK_template_site_siteid
FOREIGN KEY(siteid) REFERENCES cuyahoga_site(siteid)
go

ALTER TABLE cuyahoga_templatesection
ADD CONSTRAINT FK_templatesection_template_templateid
FOREIGN KEY (templateid) REFERENCES cuyahoga_template (templateid)
go

ALTER TABLE cuyahoga_templatesection
ADD CONSTRAINT FK_templatesection_section_sectionid
FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid)
go

ALTER TABLE cuyahoga_noderole
ADD CONSTRAINT FK_noderole_node_nodeid
FOREIGN KEY (nodeid) REFERENCES cuyahoga_node (nodeid)
go

ALTER TABLE cuyahoga_noderole
ADD CONSTRAINT FK_noderole_role_roleid
FOREIGN KEY (roleid) REFERENCES cuyahoga_role (roleid)
go


ALTER TABLE cuyahoga_sectionrole
ADD CONSTRAINT FK_sectionrole_role_roleid
FOREIGN KEY (roleid) REFERENCES cuyahoga_role (roleid)
go

ALTER TABLE cuyahoga_sectionrole
ADD CONSTRAINT FK_sectionrole_section_sectionid
FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid)
go

-- DATA
SET DATEFORMAT ymd


SET IDENTITY_INSERT cuyahoga_role ON

GO

INSERT INTO cuyahoga_role (roleid, name, inserttimestamp, updatetimestamp, permissionlevel, isglobal) VALUES (3, 'Authenticated user', '2004-01-04 16:34:50.271', '2004-06-25 00:59:02.822', 2, 1)
INSERT INTO cuyahoga_role (roleid, name, inserttimestamp, updatetimestamp, permissionlevel, isglobal) VALUES (2, 'Editor', '2004-01-04 16:34:25.669', '2004-06-25 00:59:08.256', 6, 1)
INSERT INTO cuyahoga_role (roleid, name, inserttimestamp, updatetimestamp, permissionlevel, isglobal) VALUES (1, 'Administrator', '2004-01-04 16:33:42.255', '2004-09-19 17:08:47.248', 14, 1)
INSERT INTO cuyahoga_role (roleid, name, inserttimestamp, updatetimestamp, permissionlevel, isglobal) VALUES (4, 'Anonymous user', '2004-01-04 16:35:10.766', '2004-07-16 21:18:09.017', 1, 1)

GO

SET IDENTITY_INSERT cuyahoga_role OFF

GO

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
INSERT INTO cuyahoga_right (rightid, name, description) VALUES (10, 'Global Permissions', 'Manage permissions that are shared across sites')
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

SET IDENTITY_INSERT cuyahoga_template ON

GO

INSERT INTO cuyahoga_template (templateid, [name], basepath, templatecontrol, css, inserttimestamp, updatetimestamp) VALUES (1, 'Cuyahoga Home', 'Templates/Classic', 'CuyahogaHome.ascx', 'red.css', '2004-01-26 21:52:52.365', '2004-01-26 21:52:52.365')
INSERT INTO cuyahoga_template (templateid, [name], basepath, templatecontrol, css, inserttimestamp, updatetimestamp) VALUES (2, 'Cuyahoga Standard', 'Templates/Classic', 'CuyahogaStandard.ascx', 'red.css', '2004-01-26 21:52:52.365', '2004-01-26 21:52:52.365')
INSERT INTO cuyahoga_template (templateid, [name], basepath, templatecontrol, css, inserttimestamp, updatetimestamp) VALUES (3, 'Cuyahoga New', 'Templates/Default', 'CuyahogaNew.ascx', 'red-new.css', '2004-01-26 21:52:52.365', '2004-01-26 21:52:52.365')
INSERT INTO cuyahoga_template (templateid, [name], basepath, templatecontrol, css, inserttimestamp, updatetimestamp) VALUES (4, 'Another Red', 'Templates/AnotherRed', 'Cuyahoga.ascx', 'red.css', '2004-01-26 21:52:52.365', '2004-01-26 21:52:52.365')

GO

SET IDENTITY_INSERT cuyahoga_template OFF

GO


INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Core', 2, 0, 0)
GO
