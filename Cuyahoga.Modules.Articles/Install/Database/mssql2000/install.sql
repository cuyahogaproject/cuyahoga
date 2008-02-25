/*
 *  Table structure
 */
CREATE TABLE cm_articlecategory(
articlecategoryid int identity(1,1) NOT NULL CONSTRAINT PK_articlecategory PRIMARY KEY,
siteid int NOT NULL,
title nvarchar(100) NOT NULL,
summary nvarchar(255) NULL,
syndicate bit NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
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

ALTER TABLE cm_articlecategory
ADD CONSTRAINT FK_articlecategory_site_siteid
FOREIGN KEY (siteid) REFERENCES cuyahoga_site (siteid)
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



/*
 *  Table data
 */
SET DATEFORMAT ymd


DECLARE @moduletypeid int

INSERT INTO cuyahoga_moduletype ([name], assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) VALUES ('Articles', 'Cuyahoga.Modules.Articles', 'Cuyahoga.Modules.Articles.ArticleModule', 'Modules/Articles/Articles.ascx', 'Modules/Articles/AdminArticles.aspx', '2004-10-02 14:36:28.324', '2004-10-02 14:36:28.324')

SELECT @moduletypeid = Scope_Identity()

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (@moduletypeid, 'ALLOW_COMMENTS', 'Allow comments', 'System.Boolean', 0, 1)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (@moduletypeid, 'NUMBER_OF_ARTICLES_IN_LIST', 'Number of articles to display', 'System.Int16', 0, 1)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (@moduletypeid, 'DISPLAY_TYPE', 'Display type', 'Cuyahoga.Modules.Articles.DisplayType', 1, 1)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (@moduletypeid, 'ALLOW_ANONYMOUS_COMMENTS', 'Allow anonymous comments', 'System.Boolean', 0, 1)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (@moduletypeid, 'ALLOW_SYNDICATION', 'Allow syndication', 'System.Boolean', 0, 1)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (@moduletypeid, 'SHOW_ARCHIVE', 'Show link to archived articles', 'System.Boolean', 0, 1)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (@moduletypeid, 'SHOW_CATEGORY', 'Show category', 'System.Boolean', 0, 1)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (@moduletypeid, 'SHOW_DATETIME', 'Show publish date and time', 'System.Boolean', 0, 1)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (@moduletypeid, 'SHOW_AUTHOR', 'Show author', 'System.Boolean', 0, 1)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (@moduletypeid, 'SORT_BY', 'Sort by', 'Cuyahoga.Modules.Articles.SortBy', 1, 1)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (@moduletypeid, 'SORT_DIRECTION', 'Sort direction', 'Cuyahoga.Modules.Articles.SortDirection', 1, 1)

INSERT INTO cuyahoga_moduleservice (moduletypeid, servicekey, servicetype, classtype) 
VALUES (@moduletypeid, 'articles.articledao', 'Cuyahoga.Modules.Articles.DataAccess.IArticleDao, Cuyahoga.Modules.Articles', 'Cuyahoga.Modules.Articles.DataAccess.ArticleDao, Cuyahoga.Modules.Articles')

go

INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Modules.Articles', 1, 5, 2)
go
