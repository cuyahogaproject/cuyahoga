
CREATE TABLE cm_flashaltcontent(
	flashaltcontentid int identity(1,1) NOT NULL CONSTRAINT PK_cm_flashaltcontent PRIMARY KEY,
	sectionid int NOT NULL,
	createdby int NOT NULL,
	modifiedby int NULL,
	title varchar(255) NULL,	
	content text NOT NULL,
	inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
	updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go


ALTER TABLE cm_flashaltcontent
	ADD CONSTRAINT FK_flashaltcontent_section_sectionid
		FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid)
go

/*
 *  Table data
 */

DECLARE @moduletypeid int

INSERT INTO cuyahoga_moduletype ([name], assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) VALUES ('Flash', 'Cuyahoga.Modules.Flash', 'Cuyahoga.Modules.Flash.FlashModule', 'Modules/Flash/Flash.ascx', 'Modules/Flash/EditFlash.aspx', '2005-05-15 14:36:28.324', '2004-05-15 14:36:28.324')

SELECT @moduletypeid = Scope_Identity()

INSERT INTO cuyahoga_modulesetting ( moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES ( @moduletypeid, 'MOVIEALIGN', 'Movie Align', 'Cuyahoga.Modules.Flash.MovieAlign', 1, 0)
INSERT INTO cuyahoga_modulesetting ( moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES ( @moduletypeid, 'MOVIEBGCOLOR', 'Movie Background Color', 'System.String', 0, 0)
INSERT INTO cuyahoga_modulesetting ( moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES ( @moduletypeid, 'MOVIEHEIGHT', 'Movie Height', 'System.String', 0, 1)
INSERT INTO cuyahoga_modulesetting ( moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES ( @moduletypeid, 'MOVIEWIDTH', 'Movie Width', 'System.String', 0, 1)
INSERT INTO cuyahoga_modulesetting ( moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES ( @moduletypeid, 'MOVIENAME', 'Movie Name', 'System.String', 0, 1)
INSERT INTO cuyahoga_modulesetting ( moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES ( @moduletypeid, 'MOVIEQUALITY', 'Movie Quality', 'Cuyahoga.Modules.Flash.MovieQuality', 1, 1)
INSERT INTO cuyahoga_modulesetting ( moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES ( @moduletypeid, 'MOVIESCRIPTACCESS', 'Movie Script Access', 'Cuyahoga.Modules.Flash.MovieScriptAccess', 1, 1)
INSERT INTO cuyahoga_modulesetting ( moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES ( @moduletypeid, 'MAJORPLUGINVERSION', 'Major Plugin Version', 'System.Int32', 0, 1)
INSERT INTO cuyahoga_modulesetting ( moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES ( @moduletypeid, 'MAJORPLUGINVERSIONREVISION', 'Major Plugin Version Revision', 'System.Int32', 0, 1)
INSERT INTO cuyahoga_modulesetting ( moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES ( @moduletypeid, 'MINORPLUGINVERSION', 'Minor Plugin Version', 'System.Int32', 0, 1)
INSERT INTO cuyahoga_modulesetting ( moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES ( @moduletypeid, 'MINORPLUGINVERSIONREVISION', 'Minor Plugin Version Revision', 'System.Int32', 0, 0)
INSERT INTO cuyahoga_modulesetting ( moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES ( @moduletypeid, 'ALTERNATEDIVID', 'Use Alternate Div Id for Flash replacement', 'System.String', 0, 0)
GO

INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Modules.Flash', 1, 0, 0);
go