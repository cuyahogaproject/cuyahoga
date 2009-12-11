/*
 *  Table definition
 */

CREATE TABLE cm_articlecategory(
articlecategoryid serial NOT NULL CONSTRAINT PK_articlecategory PRIMARY KEY,
siteid int4 NOT NULL,
title varchar(100) NOT NULL,
summary varchar(255),
syndicate bool NOT NULL,
inserttimestamp timestamp DEFAULT current_timestamp NOT NULL,
updatetimestamp timestamp DEFAULT current_timestamp NOT NULL,
CONSTRAINT FK_articlecategory_site_siteid FOREIGN KEY (siteid) REFERENCES cuyahoga_site (siteid));

CREATE TABLE cm_article(
articleid serial NOT NULL CONSTRAINT PK_article PRIMARY KEY,
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
CONSTRAINT FK_article_articlecategory_articlecategoryid FOREIGN KEY (articlecategoryid) REFERENCES cm_articlecategory (articlecategoryid),
CONSTRAINT FK_article_section_sectionid FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid),
CONSTRAINT FK_article_user_createdby FOREIGN KEY (createdby) REFERENCES cuyahoga_user (userid),
CONSTRAINT FK_article_user_modifiedby FOREIGN KEY (modifiedby) REFERENCES cuyahoga_user (userid));


CREATE TABLE cm_articlecomment(
commentid serial NOT NULL CONSTRAINT PK_articlecomment PRIMARY KEY,
articleid int4 NOT NULL,
userid int4,
name varchar(100),
website varchar(100),
commenttext varchar(2000) NOT NULL,
userip varchar(15),
inserttimestamp timestamp DEFAULT current_timestamp NOT NULL,
updatetimestamp timestamp DEFAULT current_timestamp NOT NULL,
CONSTRAINT FK_articlecomment_article_articleid FOREIGN KEY (articleid) REFERENCES cm_article (articleid),
CONSTRAINT FK_articlecomment_user_userid FOREIGN KEY (userid) REFERENCES cuyahoga_user (userid));


/*
 *  Table data
 */

-- first set sequence values, otherwise the inserts will fail due to violation of the pk_constraint
SELECT setval('cuyahoga_moduletype_moduletypeid_seq', (SELECT max(moduletypeid) FROM cuyahoga_moduletype));
SELECT setval('cuyahoga_modulesetting_modulesettingid_seq', (SELECT max(modulesettingid) FROM cuyahoga_modulesetting));

INSERT INTO cuyahoga_moduletype (name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) 
VALUES ('Articles', 'Cuyahoga.Modules.Articles', 'Cuyahoga.Modules.Articles.ArticleModule', 'Modules/Articles/Articles.ascx', 'Modules/Articles/AdminArticles.aspx', '2004-10-02 14:36:28.324', '2004-10-02 14:36:28.324');

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (currval('cuyahoga_moduletype_moduletypeid_seq'), 'ALLOW_COMMENTS', 'Allow comments', 'System.Boolean', false, true);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (currval('cuyahoga_moduletype_moduletypeid_seq'), 'NUMBER_OF_ARTICLES_IN_LIST', 'Number of articles to display', 'System.Int16', false, true);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (currval('cuyahoga_moduletype_moduletypeid_seq'), 'DISPLAY_TYPE', 'Display type', 'Cuyahoga.Modules.Articles.DisplayType', true, true);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (currval('cuyahoga_moduletype_moduletypeid_seq'), 'ALLOW_ANONYMOUS_COMMENTS', 'Allow anonymous comments', 'System.Boolean', false, true);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (currval('cuyahoga_moduletype_moduletypeid_seq'), 'ALLOW_SYNDICATION', 'Allow syndication', 'System.Boolean', false, true);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (currval('cuyahoga_moduletype_moduletypeid_seq'), 'SHOW_ARCHIVE', 'Show link to archived articles', 'System.Boolean', false, true);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (currval('cuyahoga_moduletype_moduletypeid_seq'), 'SHOW_DATETIME', 'Show publish date and time', 'System.Boolean', false, true);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (currval('cuyahoga_moduletype_moduletypeid_seq'), 'SHOW_CATEGORY', 'Show category', 'System.Boolean', false, true);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (currval('cuyahoga_moduletype_moduletypeid_seq'), 'SHOW_AUTHOR', 'Show author', 'System.Boolean', false, true);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (currval('cuyahoga_moduletype_moduletypeid_seq'), 'SORT_BY', 'Sort by', 'Cuyahoga.Modules.Articles.SortBy', true, true);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (currval('cuyahoga_moduletype_moduletypeid_seq'), 'SORT_DIRECTION', 'Sort direction', 'Cuyahoga.Modules.Articles.SortDirection', true, true);

INSERT INTO cuyahoga_moduleservice (moduletypeid, servicekey, servicetype, classtype) 
VALUES (currval('cuyahoga_moduletype_moduletypeid_seq'), 'articles.articledao', 'Cuyahoga.Modules.Articles.DataAccess.IArticleDao, Cuyahoga.Modules.Articles', 'Cuyahoga.Modules.Articles.DataAccess.ArticleDao, Cuyahoga.Modules.Articles');


INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Modules.Articles', 1, 5, 2);

