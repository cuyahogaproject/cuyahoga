DECLARE @moduletypeid int

INSERT INTO cuyahoga_moduletype (name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) VALUES ('RemoteContent', 'Cuyahoga.Modules', 'Cuyahoga.Modules.RemoteContent.RemoteContentModule', 'Modules/RemoteContent/FeedDisplay.ascx', NULL, '2004-10-02 14:36:28.324', '2004-10-02 14:36:28.324')

SELECT @moduletypeid = @@IDENTITY

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (@moduletypeid, 'FEED_URL', 'Feed url', 'System.String', 0, 1)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (@moduletypeid, 'FEED_TYPE', 'Feed type', 'Cuyahoga.Modules.RemoteContent.FeedType', 1, 1)

GO
