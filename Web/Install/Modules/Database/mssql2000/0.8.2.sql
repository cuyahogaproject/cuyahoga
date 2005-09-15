
CREATE TABLE cm_articlecategory(
articlecategoryid int identity(1,1) NOT NULL CONSTRAINT PK_cm_articlecategory1 PRIMARY KEY,
title varchar(100) NOT NULL,
summary varchar(255) NULL,
syndicate bit NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go


CREATE TABLE cm_feed(
feedid int identity(1,1) NOT NULL CONSTRAINT PK_cm_feed1 PRIMARY KEY,
sectionid int NOT NULL,
url varchar(255) NOT NULL,
title varchar(100) NOT NULL,
pubdate datetime NOT NULL,
numberofitems int NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go


CREATE TABLE cm_feeditem(
feeditemid int identity(1,1) NOT NULL CONSTRAINT PK_cm_feeditem1 PRIMARY KEY,
feedid int NOT NULL,
url varchar(255) NOT NULL,
title varchar(100) NOT NULL,
content text NULL,
pubdate datetime NOT NULL,
author varchar(100) NULL)
go


CREATE TABLE cm_article(
articleid int identity(1,1) NOT NULL CONSTRAINT PK_cm_article1 PRIMARY KEY,
sectionid int NOT NULL,
createdby int NOT NULL,
modifiedby int NULL,
articlecategoryid int NULL,
title varchar(100) NOT NULL,
summary varchar(255) NULL,
content text NOT NULL,
syndicate bit NOT NULL,
dateonline datetime NOT NULL,
dateoffline datetime NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go


CREATE TABLE cm_articlecomment(
commentid int identity(1,1) NOT NULL CONSTRAINT PK_cm_articlecomment1 PRIMARY KEY,
articleid int NOT NULL,
userid int NULL,
name varchar(100) NULL,
website varchar(100) NULL,
commenttext varchar(2000) NOT NULL,
userip varchar(15) NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go


CREATE TABLE cm_statichtml(
statichtmlid int identity(1,1) NOT NULL CONSTRAINT PK_cm_statichtml1 PRIMARY KEY,
sectionid int NOT NULL,
createdby int NOT NULL,
modifiedby int NULL,
title varchar(255) NULL,
content text NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go





ALTER TABLE cm_feed
ADD CONSTRAINT FK_cm_feed_1 
FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid)
go


ALTER TABLE cm_feeditem
ADD CONSTRAINT FK_cm_feeditem_1 
FOREIGN KEY (feedid) REFERENCES cm_feed (feedid)
go


ALTER TABLE cm_article
ADD CONSTRAINT FK_cm_article_1 
FOREIGN KEY (articlecategoryid) REFERENCES cm_articlecategory (articlecategoryid)
go

ALTER TABLE cm_article
ADD CONSTRAINT FK_cm_article_2 
FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid)
go

ALTER TABLE cm_article
ADD CONSTRAINT FK_cm_article_3 
FOREIGN KEY (createdby) REFERENCES cuyahoga_user (userid)
go

ALTER TABLE cm_article
ADD CONSTRAINT FK_cm_article_4 
FOREIGN KEY (modifiedby) REFERENCES cuyahoga_user (userid)
go


ALTER TABLE cm_articlecomment
ADD CONSTRAINT FK_cm_articlecomment_1 
FOREIGN KEY (articleid) REFERENCES cm_article (articleid)
go

ALTER TABLE cm_articlecomment
ADD CONSTRAINT FK_cm_articlecomment_2 
FOREIGN KEY (userid) REFERENCES cuyahoga_user (userid)
go


ALTER TABLE cm_statichtml
ADD CONSTRAINT FK_cm_statichtml_1 
FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid)
go

ALTER TABLE cm_statichtml
ADD CONSTRAINT FK_cm_statichtml_2 
FOREIGN KEY (createdby) REFERENCES cuyahoga_user (userid)
go

ALTER TABLE cm_statichtml
ADD CONSTRAINT FK_cm_statichtml_3 
FOREIGN KEY (modifiedby) REFERENCES cuyahoga_user (userid)
go

-- DATA

SET IDENTITY_INSERT cuyahoga_moduletype ON

GO

INSERT INTO cuyahoga_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) VALUES (1, 'StaticHtml', 'Cuyahoga.Modules', 'Cuyahoga.Modules.StaticHtml.StaticHtmlModule', 'Modules/StaticHtml/StaticHtml.ascx', 'Modules/StaticHtml/EditHtml.aspx', '2004-10-02 14:36:28.324', '2004-10-02 14:36:28.324')
INSERT INTO cuyahoga_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) VALUES (2, 'Articles', 'Cuyahoga.Modules', 'Cuyahoga.Modules.Articles.ArticleModule', 'Modules/Articles/Articles.ascx', 'Modules/Articles/AdminArticles.aspx', '2004-10-02 14:36:28.324', '2004-10-02 14:36:28.324')
INSERT INTO cuyahoga_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) VALUES (3, 'User', 'Cuyahoga.Modules', 'Cuyahoga.Modules.User.UserModule', 'Modules/User/User.ascx', NULL, '2004-10-02 14:36:28.324', '2004-10-02 14:36:28.324')
INSERT INTO cuyahoga_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) VALUES (4, 'Search', 'Cuyahoga.Modules', 'Cuyahoga.Modules.Search.SearchModule', 'Modules/Search/Search.ascx', NULL, '2004-10-02 14:36:28.324', '2004-10-02 14:36:28.324')
INSERT INTO cuyahoga_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) VALUES (5, 'LanguageSwitcher', 'Cuyahoga.Modules', 'Cuyahoga.Modules.LanguageSwitcher.LanguageSwitcherModule', 'Modules/LanguageSwitcher/LanguageSwitcher.ascx', NULL, '2004-10-02 14:36:28.324', '2004-10-02 14:36:28.324')
INSERT INTO cuyahoga_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) VALUES (6, 'RemoteContent', 'Cuyahoga.Modules', 'Cuyahoga.Modules.RemoteContent.RemoteContentModule', 'Modules/RemoteContent/RemoteContent.ascx', 'Modules/RemoteContent/AdminRemoteContent.aspx', '2005-04-08 14:36:28.324', '2004-04-08 14:36:28.324')

GO

SET IDENTITY_INSERT cuyahoga_moduletype OFF

GO

SET IDENTITY_INSERT cuyahoga_modulesetting ON

GO

INSERT INTO cuyahoga_modulesetting (modulesettingid, moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (1, 2, 'ALLOW_COMMENTS', 'Allow comments', 'System.Boolean', 0, 1)
INSERT INTO cuyahoga_modulesetting (modulesettingid, moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (2, 2, 'NUMBER_OF_ARTICLES_IN_LIST', 'Number of articles to display', 'System.Int16', 0, 1)
INSERT INTO cuyahoga_modulesetting (modulesettingid, moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (3, 2, 'DISPLAY_TYPE', 'Display type', 'Cuyahoga.Modules.Articles.DisplayType', 1, 1)
INSERT INTO cuyahoga_modulesetting (modulesettingid, moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (4, 2, 'ALLOW_ANONYMOUS_COMMENTS', 'Allow anonymous comments', 'System.Boolean', 0, 1)
INSERT INTO cuyahoga_modulesetting (modulesettingid, moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (5, 2, 'ALLOW_SYNDICATION', 'Allow syndication', 'System.Boolean', 0, 1)
INSERT INTO cuyahoga_modulesetting (modulesettingid, moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (6, 6, 'CACHE_DURATION', 'Local database cache duration (min)', 'System.Int32', 0, 1)
INSERT INTO cuyahoga_modulesetting (modulesettingid, moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (7, 6, 'SHOW_CONTENTS', 'Show feed contents', 'System.Boolean', 0, 1)
INSERT INTO cuyahoga_modulesetting (modulesettingid, moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (8, 6, 'SHOW_DATES', 'Show dates', 'System.Boolean', 0, 1)
INSERT INTO cuyahoga_modulesetting (modulesettingid, moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (9, 6, 'BACKGROUND_REFRESH', 'Use background refreshing', 'System.Boolean', 0, 1)
INSERT INTO cuyahoga_modulesetting (modulesettingid, moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (10, 6, 'SHOW_SOURCES', 'Show feed sources', 'System.Boolean', 0, 1)
INSERT INTO cuyahoga_modulesetting (modulesettingid, moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (11, 6, 'SHOW_AUTHORS', 'Show authors', 'System.Boolean', 0, 1)

GO

SET IDENTITY_INSERT cuyahoga_modulesetting OFF

GO

INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Modules', 0, 8, 2)

GO