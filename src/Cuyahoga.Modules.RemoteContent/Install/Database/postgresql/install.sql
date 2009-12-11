/*
 *  Table definition
 */

CREATE TABLE cm_feed(
feedid serial NOT NULL CONSTRAINT PK_feed PRIMARY KEY,
sectionid int4 NOT NULL,
url varchar(255) NOT NULL,
title varchar(100) NOT NULL,
pubdate date NOT NULL,
numberofitems int4 NOT NULL,
inserttimestamp timestamp DEFAULT current_timestamp NOT NULL,
updatetimestamp timestamp DEFAULT current_timestamp NOT NULL,
CONSTRAINT FK_feed_section_sectionid FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid));


CREATE TABLE cm_feeditem(
feeditemid serial NOT NULL CONSTRAINT PK_feeditem PRIMARY KEY,
feedid int4 NOT NULL,
url varchar(255) NOT NULL,
title varchar(100) NOT NULL,
content text,
pubdate date NOT NULL,
author varchar(100),
CONSTRAINT FK_feeditem_feed_feedid FOREIGN KEY (feedid) REFERENCES cm_feed (feedid));


/*
 *  Table data
 */

-- first set sequence values, otherwise the inserts will fail due to violation of the pk_constraint
SELECT setval('cuyahoga_moduletype_moduletypeid_seq', (SELECT max(moduletypeid) FROM cuyahoga_moduletype));
SELECT setval('cuyahoga_modulesetting_modulesettingid_seq', (SELECT max(modulesettingid) FROM cuyahoga_modulesetting));

INSERT INTO cuyahoga_moduletype (name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) 
VALUES ('RemoteContent', 'Cuyahoga.Modules.RemoteContent', 'Cuyahoga.Modules.RemoteContent.RemoteContentModule', 'Modules/RemoteContent/RemoteContent.ascx', 'Modules/RemoteContent/AdminRemoteContent.aspx', '2005-04-08 14:36:28.324', '2004-04-08 14:36:28.324');

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (currval('cuyahoga_moduletype_moduletypeid_seq'), 'CACHE_DURATION', 'Local database cache duration (min)', 'System.Int32', false, true);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (currval('cuyahoga_moduletype_moduletypeid_seq'), 'SHOW_CONTENTS', 'Show feed contents', 'System.Boolean', false, true);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (currval('cuyahoga_moduletype_moduletypeid_seq'), 'SHOW_DATES', 'Show dates', 'System.Boolean', false, true);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (currval('cuyahoga_moduletype_moduletypeid_seq'), 'BACKGROUND_REFRESH', 'Use background refreshing', 'System.Boolean', false, true);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (currval('cuyahoga_moduletype_moduletypeid_seq'), 'SHOW_SOURCES', 'Show feed sources', 'System.Boolean', false, true);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (currval('cuyahoga_moduletype_moduletypeid_seq'), 'SHOW_AUTHORS', 'Show authors', 'System.Boolean', false, true);

INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Modules.RemoteContent', 1, 5, 1);

