/*
 *  Table definition
 */

CREATE TABLE cm_file(
fileid serial NOT NULL CONSTRAINT PK_file PRIMARY KEY,
sectionid int4 NOT NULL,
publisherid int4 NOT NULL,
filepath varchar(255) NOT NULL,
title varchar(100),
filesize int4 NOT NULL,
nrofdownloads int4 NOT NULL,
contenttype varchar(50) NOT NULL,
datepublished timestamp NOT NULL,
inserttimestamp timestamp DEFAULT current_timestamp NOT NULL,
updatetimestamp timestamp NOT NULL,
CONSTRAINT FK_file_section_sectionid FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid),
CONSTRAINT FK_file_user_publisherid FOREIGN KEY (publisherid) REFERENCES cuyahoga_user (userid));


CREATE TABLE cm_filerole(
fileroleid serial NOT NULL CONSTRAINT PK_filerole PRIMARY KEY,
fileid int4 NOT NULL,
roleid int4 NOT NULL,
CONSTRAINT FK_filerole_file_fileid FOREIGN KEY (fileid) REFERENCES cm_file (fileid),
CONSTRAINT FK_filerole_role_roleid FOREIGN KEY (roleid) REFERENCES cuyahoga_role (roleid));

/*
 *  Table data
 */

-- first set sequence values, otherwise the inserts will fail due to violation of the pk_constraint
SELECT setval('cuyahoga_moduletype_moduletypeid_seq', (SELECT max(moduletypeid) FROM cuyahoga_moduletype));
SELECT setval('cuyahoga_modulesetting_modulesettingid_seq', (SELECT max(modulesettingid) FROM cuyahoga_modulesetting));

INSERT INTO cuyahoga_moduletype (name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp)
VALUES ('Downloads', 'Cuyahoga.Modules.Downloads', 'Cuyahoga.Modules.Downloads.DownloadsModule', 'Modules/Downloads/Downloads.ascx', 'Modules/Downloads/EditDownloads.aspx', '2005-05-15 14:36:28.324', '2004-05-15 14:36:28.324');

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired)
VALUES (currval('cuyahoga_moduletype_moduletypeid_seq'), 'SHOW_PUBLISHER', 'Show publisher', 'System.Boolean', false, true);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired)
VALUES (currval('cuyahoga_moduletype_moduletypeid_seq'), 'SHOW_DATE', 'Show file date', 'System.Boolean', false, true);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired)
VALUES (currval('cuyahoga_moduletype_moduletypeid_seq'), 'PHYSICAL_DIR', 'Physical directory (empty for App_Root/files)', 'System.String', false, false);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (currval('cuyahoga_moduletype_moduletypeid_seq'), 'SHOW_NUMBER_OF_DOWNLOADS', 'Show number of downloads', 'System.Boolean', false, false);

INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Modules.Downloads', 1, 5, 2);

