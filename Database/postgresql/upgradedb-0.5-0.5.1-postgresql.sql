INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (2, 'ALLOW_SYNDICATION', 'Allow syndication', 'System.Boolean', false, true);

INSERT INTO cuyahoga_sectionsetting (sectionid, name, value)
	SELECT sectionid, 'ALLOW_SYNDICATION', 'True'
	FROM cuyahoga_section s
		INNER JOIN cuyahoga_moduletype m on m.moduletypeid = s.moduletypeid
	WHERE m.Name = 'Articles';