
CREATE TABLE cm_articlecategory(
articlecategoryid int identity(1,1) NOT NULL CONSTRAINT PK_articlecategory PRIMARY KEY,
title nvarchar(100) NOT NULL,
summary nvarchar(255) NULL,
syndicate bit NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go


CREATE TABLE cm_feed(
feedid int identity(1,1) NOT NULL CONSTRAINT PK_feed PRIMARY KEY,
sectionid int NOT NULL,
url nvarchar(255) NOT NULL,
title nvarchar(100) NOT NULL,
pubdate datetime NOT NULL,
numberofitems int NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go


CREATE TABLE cm_feeditem(
feeditemid int identity(1,1) NOT NULL CONSTRAINT PK_feeditem PRIMARY KEY,
feedid int NOT NULL,
url nvarchar(255) NOT NULL,
title nvarchar(100) NOT NULL,
content ntext NULL,
pubdate datetime NOT NULL,
author nvarchar(100) NULL)
go


CREATE TABLE cm_article(
articleid int identity(1,1) NOT NULL CONSTRAINT PK_article PRIMARY KEY,
sectionid int NOT NULL,
createdby int NOT NULL,
modifiedby int NULL,
articlecategoryid int NULL,
title nvarchar(100) NOT NULL,
summary nvarchar(255) NULL,
content ntext NOT NULL,
syndicate bit NOT NULL,
dateonline datetime NOT NULL,
dateoffline datetime NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go


CREATE TABLE cm_articlecomment(
commentid int identity(1,1) NOT NULL CONSTRAINT PK_articlecomment PRIMARY KEY,
articleid int NOT NULL,
userid int NULL,
name nvarchar(100) NULL,
website nvarchar(100) NULL,
commenttext nvarchar(2000) NOT NULL,
userip nvarchar(15) NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go


CREATE TABLE cm_statichtml(
statichtmlid int identity(1,1) NOT NULL CONSTRAINT PK_statichtml PRIMARY KEY,
sectionid int NOT NULL,
createdby int NOT NULL,
modifiedby int NULL,
title nvarchar(255) NULL,
content text NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go




ALTER TABLE cm_feed
ADD CONSTRAINT FK_feed_section_sectionid
FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid)
go


ALTER TABLE cm_feeditem
ADD CONSTRAINT FK_feeditem_feed_feedid
FOREIGN KEY (feedid) REFERENCES cm_feed (feedid)
go


ALTER TABLE cm_article
ADD CONSTRAINT FK_article_articlecategory_articlecategoryid
FOREIGN KEY (articlecategoryid) REFERENCES cm_articlecategory (articlecategoryid)
go

ALTER TABLE cm_article
ADD CONSTRAINT FK_article_section_sectionid
FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid)
go

ALTER TABLE cm_article
ADD CONSTRAINT FK_article_user_createdby
FOREIGN KEY (createdby) REFERENCES cuyahoga_user (userid)
go

ALTER TABLE cm_article
ADD CONSTRAINT FK_article_user_modifiedby
FOREIGN KEY (modifiedby) REFERENCES cuyahoga_user (userid)
go


ALTER TABLE cm_articlecomment
ADD CONSTRAINT FK_articlecomment_article_articleid
FOREIGN KEY (articleid) REFERENCES cm_article (articleid)
go

ALTER TABLE cm_articlecomment
ADD CONSTRAINT FK_articlecomment_user_userid
FOREIGN KEY (userid) REFERENCES cuyahoga_user (userid)
go


ALTER TABLE cm_statichtml
ADD CONSTRAINT FK_statichtml_section_sectionid
FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid)
go

ALTER TABLE cm_statichtml
ADD CONSTRAINT FK_statichtml_user_createdby
FOREIGN KEY (createdby) REFERENCES cuyahoga_user (userid)
go

ALTER TABLE cm_statichtml
ADD CONSTRAINT FK_statichtml_user_modifiedby
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

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (2, 'ALLOW_COMMENTS', 'Allow comments', 'System.Boolean', 0, 1)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (2, 'NUMBER_OF_ARTICLES_IN_LIST', 'Number of articles to display', 'System.Int16', 0, 1)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (2, 'DISPLAY_TYPE', 'Display type', 'Cuyahoga.Modules.Articles.DisplayType', 1, 1)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (2, 'ALLOW_ANONYMOUS_COMMENTS', 'Allow anonymous comments', 'System.Boolean', 0, 1)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (2, 'ALLOW_SYNDICATION', 'Allow syndication', 'System.Boolean', 0, 1)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (2, 'SORT_BY', 'Sort by', 'Cuyahoga.Modules.Articles.SortBy', 1, 1)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (2, 'SORT_DIRECTION', 'Sort direction', 'Cuyahoga.Modules.Articles.SortDirection', 1, 1)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (6, 'CACHE_DURATION', 'Local database cache duration (min)', 'System.Int32', 0, 1)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (6, 'SHOW_CONTENTS', 'Show feed contents', 'System.Boolean', 0, 1)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (6, 'SHOW_DATES', 'Show dates', 'System.Boolean', 0, 1)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (6, 'BACKGROUND_REFRESH', 'Use background refreshing', 'System.Boolean', 0, 1)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (6, 'SHOW_SOURCES', 'Show feed sources', 'System.Boolean', 0, 1)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (6, 'SHOW_AUTHORS', 'Show authors', 'System.Boolean', 0, 1)

GO

INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Modules', 0, 9, 0)

GO