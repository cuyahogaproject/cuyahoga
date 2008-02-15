/*
 *  Table definition
 */
CREATE TABLE cm_feed(
feedid INT NOT NULL AUTO_INCREMENT,
sectionid INT NOT NULL,
url VARCHAR(255) NOT NULL,
title VARCHAR(100) NOT NULL,
pubdate DATETIME NOT NULL,
numberofitems INT NOT NULL,
inserttimestamp TIMESTAMP NOT NULL,
updatetimestamp DATETIME NOT NULL,
FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid),
PRIMARY KEY (feedid));


CREATE TABLE cm_feeditem(
feeditemid INT NOT NULL AUTO_INCREMENT,
feedid INT NOT NULL,
url VARCHAR(255) NOT NULL,
title VARCHAR(100) NOT NULL,
content MEDIUMTEXT,
pubdate DATETIME NOT NULL,
author VARCHAR(100),
FOREIGN KEY (feedid) REFERENCES cm_feed (feedid),
PRIMARY KEY (feeditemid));

/*
 *  Table data
 */
INSERT INTO cuyahoga_moduletype (name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) 
VALUES ('RemoteContent', 'Cuyahoga.Modules.RemoteContent', 'Cuyahoga.Modules.RemoteContent.RemoteContentModule', 'Modules/RemoteContent/RemoteContent.ascx', 'Modules/RemoteContent/AdminRemoteContent.aspx', '2005-04-08 14:36:28.324', '2004-04-08 14:36:28.324');

SELECT @moduletypeid := last_insert_id();

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (@moduletypeid, 'CACHE_DURATION', 'Local database cache duration (min)', 'System.Int32', 0, 1);
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (@moduletypeid, 'SHOW_CONTENTS', 'Show feed contents', 'System.Boolean', 0, 1);
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (@moduletypeid, 'SHOW_DATES', 'Show dates', 'System.Boolean', 0, 1);
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (@moduletypeid, 'BACKGROUND_REFRESH', 'Use background refreshing', 'System.Boolean', 0, 1);
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (@moduletypeid, 'SHOW_SOURCES', 'Show feed sources', 'System.Boolean', 0, 1);
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (@moduletypeid, 'SHOW_AUTHORS', 'Show authors', 'System.Boolean', 0, 1);

INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Modules.RemoteContent', 1, 5, 1);

