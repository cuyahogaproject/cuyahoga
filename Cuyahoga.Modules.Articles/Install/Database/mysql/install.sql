/*
 *  Table definition
 */
CREATE TABLE cm_articlecategory(
articlecategoryid INT NOT NULL AUTO_INCREMENT,
siteid INT NOT NULL,
title VARCHAR(100) NOT NULL,
summary VARCHAR(255),
syndicate TINYINT NOT NULL,
inserttimestamp TIMESTAMP NOT NULL,
updatetimestamp DATETIME NOT NULL,
FOREIGN KEY (siteid) REFERENCES cuyahoga_site (siteid),
PRIMARY KEY (articlecategoryid));

CREATE TABLE cm_article(
articleid INT NOT NULL AUTO_INCREMENT,
sectionid INT NOT NULL,
createdby INT NOT NULL,
modifiedby INT,
articlecategoryid INT,
title VARCHAR(100) NOT NULL,
summary VARCHAR(255),
content MEDIUMTEXT NOT NULL,
syndicate TINYINT NOT NULL,
dateonline DATETIME NOT NULL,
dateoffline DATETIME NOT NULL,
inserttimestamp TIMESTAMP NOT NULL,
updatetimestamp DATETIME NOT NULL,
FOREIGN KEY (articlecategoryid) REFERENCES cm_articlecategory (articlecategoryid),
FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid),
FOREIGN KEY (createdby) REFERENCES cuyahoga_user (userid),
FOREIGN KEY (modifiedby) REFERENCES cuyahoga_user (userid),
PRIMARY KEY (articleid));


CREATE TABLE cm_articlecomment(
commentid INT NOT NULL AUTO_INCREMENT,
articleid INT NOT NULL,
userid INT,
name VARCHAR(100),
website VARCHAR(100),
commenttext TEXT NOT NULL,
userip VARCHAR(15),
inserttimestamp TIMESTAMP NOT NULL,
updatetimestamp DATETIME NOT NULL,
FOREIGN KEY (articleid) REFERENCES cm_article (articleid),
FOREIGN KEY (userid) REFERENCES cuyahoga_user (userid),
PRIMARY KEY (commentid));



/*
 *  Table data
 */
INSERT INTO cuyahoga_moduletype (name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp)
VALUES ('Articles', 'Cuyahoga.Modules.Articles', 'Cuyahoga.Modules.Articles.ArticleModule', 'Modules/Articles/Articles.ascx', 'Modules/Articles/AdminArticles.aspx', '2005-05-15 14:36:28.324', '2004-05-15 14:36:28.324');

SELECT @moduletypeid := last_insert_id();

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'ALLOW_COMMENTS', 'Allow comments', 'System.Boolean', 0, 1);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'NUMBER_OF_ARTICLES_IN_LIST', 'Number of articles to display', 'System.Int16', 0, 1);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'DISPLAY_TYPE', 'Display type', 'Cuyahoga.Modules.Articles.DisplayType', 1, 1);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'ALLOW_ANONYMOUS_COMMENTS', 'Allow anonymous comments', 'System.Boolean', 0, 1);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'ALLOW_SYNDICATION', 'Allow syndication', 'System.Boolean', 0, 1);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'SHOW_ARCHIVE', 'Show link to archived articles', 'System.Boolean', 0, 1);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'SHOW_DATETIME', 'Show publish date and time', 'System.Boolean', 0, 1);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'SHOW_CATEGORY', 'Show category', 'System.Boolean', 0, 1);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'SHOW_AUTHOR', 'Show author', 'System.Boolean', 0, 1);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'SORT_BY', 'Sort by', 'Cuyahoga.Modules.Articles.SortBy', 1, 1);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'SORT_DIRECTION', 'Sort direction', 'Cuyahoga.Modules.Articles.SortDirection', 1, 1);

INSERT INTO cuyahoga_moduleservice (moduletypeid, servicekey, servicetype, classtype) 
VALUES (@moduletypeid, 'articles.articledao', 'Cuyahoga.Modules.Articles.DataAccess.IArticleDao, Cuyahoga.Modules.Articles', 'Cuyahoga.Modules.Articles.DataAccess.ArticleDao, Cuyahoga.Modules.Articles');


INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Modules.Articles', 1, 5, 2);

