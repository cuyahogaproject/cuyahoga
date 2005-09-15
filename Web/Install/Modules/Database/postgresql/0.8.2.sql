
CREATE TABLE cm_articlecategory(
articlecategoryid serial NOT NULL CONSTRAINT PK_cm_articlecategory1 PRIMARY KEY,
title varchar(100) NOT NULL,
summary varchar(255),
syndicate bool NOT NULL,
inserttimestamp timestamp DEFAULT current_timestamp NOT NULL,
updatetimestamp timestamp DEFAULT current_timestamp NOT NULL);


CREATE TABLE cm_feed(
feedid serial NOT NULL CONSTRAINT PK_cm_feed1 PRIMARY KEY,
sectionid int4 NOT NULL,
url varchar(255) NOT NULL,
title varchar(100) NOT NULL,
pubdate date NOT NULL,
numberofitems int4 NOT NULL,
inserttimestamp timestamp DEFAULT current_timestamp NOT NULL,
updatetimestamp timestamp DEFAULT current_timestamp NOT NULL,
CONSTRAINT FK_cm_feed_1 FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid));


CREATE TABLE cm_feeditem(
feeditemid serial NOT NULL CONSTRAINT PK_cm_feeditem1 PRIMARY KEY,
feedid int4 NOT NULL,
url varchar(255) NOT NULL,
title varchar(100) NOT NULL,
content text,
pubdate date NOT NULL,
author varchar(100),
CONSTRAINT FK_cm_feeditem_1 FOREIGN KEY (feedid) REFERENCES cm_feed (feedid));


CREATE TABLE cm_article(
articleid serial NOT NULL CONSTRAINT PK_cm_article1 PRIMARY KEY,
sectionid int4 NOT NULL,
createdby int4 NOT NULL,
modifiedby int4,
articlecategoryid int4,
title varchar(100) NOT NULL,
summary varchar(255),
content text NOT NULL,
syndicate bool NOT NULL,
dateonline timestamp NOT NULL,
dateoffline timestamp NOT NULL,
inserttimestamp timestamp DEFAULT current_timestamp NOT NULL,
updatetimestamp timestamp DEFAULT current_timestamp NOT NULL,
CONSTRAINT FK_cm_article_1 FOREIGN KEY (articlecategoryid) REFERENCES cm_articlecategory (articlecategoryid),
CONSTRAINT FK_cm_article_2 FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid),
CONSTRAINT FK_cm_article_3 FOREIGN KEY (createdby) REFERENCES cuyahoga_user (userid),
CONSTRAINT FK_cm_article_4 FOREIGN KEY (modifiedby) REFERENCES cuyahoga_user (userid));


CREATE TABLE cm_articlecomment(
commentid serial NOT NULL CONSTRAINT PK_cm_articlecomment1 PRIMARY KEY,
articleid int4 NOT NULL,
userid int4,
name varchar(100),
website varchar(100),
commenttext varchar(2000) NOT NULL,
userip varchar(15),
inserttimestamp timestamp DEFAULT current_timestamp NOT NULL,
updatetimestamp timestamp DEFAULT current_timestamp NOT NULL,
CONSTRAINT FK_cm_articlecomment_1 FOREIGN KEY (articleid) REFERENCES cm_article (articleid),
CONSTRAINT FK_cm_articlecomment_2 FOREIGN KEY (userid) REFERENCES cuyahoga_user (userid));


CREATE TABLE cm_statichtml(
statichtmlid serial NOT NULL CONSTRAINT PK_cm_statichtml1 PRIMARY KEY,
sectionid int4 NOT NULL,
createdby int4 NOT NULL,
modifiedby int4,
title varchar(255),
content text NOT NULL,
inserttimestamp timestamp DEFAULT current_timestamp NOT NULL,
updatetimestamp timestamp DEFAULT current_timestamp NOT NULL,
CONSTRAINT FK_cm_statichtml_1 FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid),
CONSTRAINT FK_cm_statichtml_2 FOREIGN KEY (createdby) REFERENCES cuyahoga_user (userid),
CONSTRAINT FK_cm_statichtml_3 FOREIGN KEY (modifiedby) REFERENCES cuyahoga_user (userid));

-- DATA --

INSERT INTO cuyahoga_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) VALUES (1, 'StaticHtml', 'Cuyahoga.Modules', 'Cuyahoga.Modules.StaticHtml.StaticHtmlModule', 'Modules/StaticHtml/StaticHtml.ascx', 'Modules/StaticHtml/EditHtml.aspx', '2004-10-02 14:36:28.324', '2004-10-02 14:36:28.324');
INSERT INTO cuyahoga_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) VALUES (2, 'Articles', 'Cuyahoga.Modules', 'Cuyahoga.Modules.Articles.ArticleModule', 'Modules/Articles/Articles.ascx', 'Modules/Articles/AdminArticles.aspx', '2004-10-02 14:36:28.324', '2004-10-02 14:36:28.324');
INSERT INTO cuyahoga_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) VALUES (3, 'User', 'Cuyahoga.Modules', 'Cuyahoga.Modules.User.UserModule', 'Modules/User/User.ascx', NULL, '2004-10-02 14:36:28.324', '2004-10-02 14:36:28.324');
INSERT INTO cuyahoga_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) VALUES (4, 'Search', 'Cuyahoga.Modules', 'Cuyahoga.Modules.Search.SearchModule', 'Modules/Search/Search.ascx', NULL, '2004-10-02 14:36:28.324', '2004-10-02 14:36:28.324');
INSERT INTO cuyahoga_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) VALUES (5, 'LanguageSwitcher', 'Cuyahoga.Modules', 'Cuyahoga.Modules.LanguageSwitcher.LanguageSwitcherModule', 'Modules/LanguageSwitcher/LanguageSwitcher.ascx', NULL, '2004-10-02 14:36:28.324', '2004-10-02 14:36:28.324');
INSERT INTO cuyahoga_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) VALUES (6, 'RemoteContent', 'Cuyahoga.Modules', 'Cuyahoga.Modules.RemoteContent.RemoteContentModule', 'Modules/RemoteContent/RemoteContent.ascx', 'Modules/RemoteContent/AdminRemoteContent.aspx', '2005-04-08 14:36:28.324', '2004-04-08 14:36:28.324');

INSERT INTO cuyahoga_modulesetting (modulesettingid, moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (1, 2, 'ALLOW_COMMENTS', 'Allow comments', 'System.Boolean', false, true);
INSERT INTO cuyahoga_modulesetting (modulesettingid, moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (2, 2, 'NUMBER_OF_ARTICLES_IN_LIST', 'Number of articles to display', 'System.Int16', false, true);
INSERT INTO cuyahoga_modulesetting (modulesettingid, moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (3, 2, 'DISPLAY_TYPE', 'Display type', 'Cuyahoga.Modules.Articles.DisplayType', true, true);
INSERT INTO cuyahoga_modulesetting (modulesettingid, moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (4, 2, 'ALLOW_ANONYMOUS_COMMENTS', 'Allow anonymous comments', 'System.Boolean', false, true);
INSERT INTO cuyahoga_modulesetting (modulesettingid, moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (5, 2, 'ALLOW_SYNDICATION', 'Allow syndication', 'System.Boolean', false, true);
INSERT INTO cuyahoga_modulesetting (modulesettingid, moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (6, 6, 'CACHE_DURATION', 'Local database cache duration (min)', 'System.Int32', false, true);
INSERT INTO cuyahoga_modulesetting (modulesettingid, moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (7, 6, 'SHOW_CONTENTS', 'Show feed contents', 'System.Boolean', false, true);
INSERT INTO cuyahoga_modulesetting (modulesettingid, moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (8, 6, 'SHOW_DATES', 'Show dates', 'System.Boolean', false, true);
INSERT INTO cuyahoga_modulesetting (modulesettingid, moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (9, 6, 'BACKGROUND_REFRESH', 'Use background refreshing', 'System.Boolean', false, true);
INSERT INTO cuyahoga_modulesetting (modulesettingid, moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (10, 6, 'SHOW_SOURCES', 'Show feed sources', 'System.Boolean', false, true);
INSERT INTO cuyahoga_modulesetting (modulesettingid, moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (11, 6, 'SHOW_AUTHORS', 'Show authors', 'System.Boolean', false, true);

INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Modules', 0, 8, 2);