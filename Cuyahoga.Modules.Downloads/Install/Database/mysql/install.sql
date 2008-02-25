/*
 *  Table definition
 */

CREATE TABLE cm_file(
fileid INT NOT NULL AUTO_INCREMENT,
sectionid INT NOT NULL,
publisherid INT NOT NULL,
filepath VARCHAR(255) NOT NULL,
title VARCHAR(100),
filesize INT NOT NULL,
nrofdownloads INT NOT NULL,
contenttype VARCHAR(50) NOT NULL,
datepublished DATETIME NOT NULL,
inserttimestamp TIMESTAMP NOT NULL,
updatetimestamp DATETIME NOT NULL,
FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid),
FOREIGN KEY (publisherid) REFERENCES cuyahoga_user (userid),
PRIMARY KEY (fileid));


CREATE TABLE cm_filerole(
fileroleid INT NOT NULL AUTO_INCREMENT,
fileid INT NOT NULL,
roleid INT NOT NULL,
FOREIGN KEY (fileid) REFERENCES cm_file (fileid),
FOREIGN KEY (roleid) REFERENCES cuyahoga_role (roleid),
PRIMARY KEY (fileroleid));


/*
 *  Table data
 */

INSERT INTO cuyahoga_moduletype (name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp)
VALUES ('Downloads', 'Cuyahoga.Modules.Downloads', 'Cuyahoga.Modules.Downloads.DownloadsModule', 'Modules/Downloads/Downloads.ascx', 'Modules/Downloads/EditDownloads.aspx', '2005-05-15 14:36:28.324', '2004-05-15 14:36:28.324');

SELECT @moduletypeid := last_insert_id();

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired)
VALUES (@moduletypeid, 'SHOW_PUBLISHER', 'Show publisher', 'System.Boolean', 0, 1);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired)
VALUES (@moduletypeid, 'SHOW_DATE', 'Show file date', 'System.Boolean', 0, 1);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired)
VALUES (@moduletypeid, 'PHYSICAL_DIR', 'Physical directory (empty for App_Root/files)', 'System.String', 0, 0);

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'SHOW_NUMBER_OF_DOWNLOADS', 'Show number of downloads', 'System.Boolean', 0, 0);

INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Modules.Downloads', 1, 5, 2);

