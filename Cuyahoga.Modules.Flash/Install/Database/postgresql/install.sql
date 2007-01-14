
CREATE TABLE cm_flashaltcontent(
flashaltcontentid serial NOT NULL CONSTRAINT PK_flashaltcontent PRIMARY KEY,
sectionid int4 NOT NULL,
createdby int4 NOT NULL,
modifiedby int4,
title varchar(255),
content text NOT NULL,
inserttimestamp timestamp DEFAULT current_timestamp NOT NULL,
updatetimestamp timestamp NOT NULL,
CONSTRAINT FK_flashaltcontent_section_sectionid FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid));


/*
 *  Table data
 */
-- first set sequence values, otherwise the inserts will fail due to violation of the pk_constraint
SELECT setval('cuyahoga_moduletype_moduletypeid_seq', (SELECT max(moduletypeid) FROM cuyahoga_moduletype));
SELECT setval('cuyahoga_modulesetting_modulesettingid_seq', (SELECT max(modulesettingid) FROM cuyahoga_modulesetting));

INSERT INTO cuyahoga_moduletype ( name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) VALUES ( 'Flash', 'Cuyahoga.Modules.Flash', 'Cuyahoga.Modules.Flash.FlashModule', 'Modules/Flash/Flash.ascx', 'Modules/Flash/EditFlash.aspx', '2005-05-15 14:36:28.324', '2004-05-15 14:36:28.324');

INSERT INTO cuyahoga_modulesetting ( moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES ( currval('cuyahoga_moduletype_moduletypeid_seq'), 'MOVIEALIGN', 'Movie Align', 'Cuyahoga.Modules.Flash.MovieAlign', 1, 0);
INSERT INTO cuyahoga_modulesetting ( moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES ( currval('cuyahoga_moduletype_moduletypeid_seq'), 'MOVIEBGCOLOR', 'Movie Background Color', 'System.String', 0, 0);
INSERT INTO cuyahoga_modulesetting ( moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES ( currval('cuyahoga_moduletype_moduletypeid_seq'), 'MOVIEHEIGHT', 'Movie Height', 'System.String', 0, 1);
INSERT INTO cuyahoga_modulesetting ( moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES ( currval('cuyahoga_moduletype_moduletypeid_seq'), 'MOVIEWIDTH', 'Movie Width', 'System.String', 0, 1);
INSERT INTO cuyahoga_modulesetting ( moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES ( currval('cuyahoga_moduletype_moduletypeid_seq'), 'MOVIENAME', 'Movie Name', 'System.String', 0, 1);
INSERT INTO cuyahoga_modulesetting ( moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES ( currval('cuyahoga_moduletype_moduletypeid_seq'), 'MOVIEQUALITY', 'Movie Quality', 'Cuyahoga.Modules.Flash.MovieQuality', 1, 1);
INSERT INTO cuyahoga_modulesetting ( moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES ( currval('cuyahoga_moduletype_moduletypeid_seq'), 'MOVIESCRIPTACCESS', 'Movie Script Access', 'Cuyahoga.Modules.Flash.MovieScriptAccess', 1, 1);
INSERT INTO cuyahoga_modulesetting ( moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES ( currval('cuyahoga_moduletype_moduletypeid_seq'), 'MAJORPLUGINVERSION', 'Major Plugin Version', 'System.Int32', 0, 1);
INSERT INTO cuyahoga_modulesetting ( moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES ( currval('cuyahoga_moduletype_moduletypeid_seq'), 'MAJORPLUGINVERSIONREVISION', 'Major Plugin Version Revision', 'System.Int32', 0, 1);
INSERT INTO cuyahoga_modulesetting ( moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES ( currval('cuyahoga_moduletype_moduletypeid_seq'), 'MINORPLUGINVERSION', 'Minor Plugin Version', 'System.Int32', 0, 1);
INSERT INTO cuyahoga_modulesetting ( moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES ( currval('cuyahoga_moduletype_moduletypeid_seq'), 'MINORPLUGINVERSIONREVISION', 'Minor Plugin Version Revision', 'System.Int32', 0, 0);
INSERT INTO cuyahoga_modulesetting ( moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES ( currval('cuyahoga_moduletype_moduletypeid_seq'), 'ALTERNATEDIVID', 'Use Alternate Div Id for Flash replacement', 'System.String', 0, 0);
INSERT INTO cuyahoga_modulesetting ( moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES ( currval('cuyahoga_moduletype_moduletypeid_seq'), 'MOVIEVARS', 'Set any flash vars (var:value;)', 'System.String', 0, 0);

INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Modules.Flash', 1, 5, 0);