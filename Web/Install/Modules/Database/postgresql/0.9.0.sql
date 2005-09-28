-- first set sequence values, otherwise the inserts will fail due to violation of the pk_constraint
SELECT setval('cuyahoga_modulesetting_modulesettingid_seq', (SELECT max(modulesettingid) FROM cuyahoga_modulesetting));

/*
 *  Article sort order settings
 */
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
	SELECT moduletypeid, 'SORT_BY', 'Sort by', 'Cuyahoga.Modules.Articles.SortBy', true, true 
	FROM cuyahoga_moduletype 
	WHERE name = 'Articles';

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
	SELECT moduletypeid, 'SORT_DIRECTION', 'Sort direction', 'Cuyahoga.Modules.Articles.SortDirection', true, true 
	FROM cuyahoga_moduletype 
	WHERE name = 'Articles';

INSERT INTO cuyahoga_sectionsetting (sectionid, name, value)
	SELECT sectionid, 'SORT_BY', 'DateOnline'
	FROM cuyahoga_section s
		INNER JOIN cuyahoga_moduletype m ON m.moduletypeid = s.moduletypeid
	WHERE m.name = 'Articles';

INSERT INTO cuyahoga_sectionsetting (sectionid, name, value)
	SELECT sectionid, 'SORT_DIRECTION', 'DESC'
	FROM cuyahoga_section s
		INNER JOIN cuyahoga_moduletype m ON m.moduletypeid = s.moduletypeid
	WHERE m.name = 'Articles';
	
/*
 *  Login control (user) settings
 */
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
	SELECT moduletypeid, 'SHOW_REGISTER', 'Show register link', 'System.Boolean', false, false 
	FROM cuyahoga_moduletype 
	WHERE name = 'User';

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
	SELECT moduletypeid, 'SHOW_RESET_PASSWORD', 'Show reset password link', 'System.Boolean', false, false 
	FROM cuyahoga_moduletype 
	WHERE name = 'User';

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
	SELECT moduletypeid, 'SHOW_EDIT_PROFILE', 'Show edit profile link', 'System.Boolean', false, false 
	FROM cuyahoga_moduletype 
	WHERE name = 'User';

/*
 *  Version
 */
UPDATE cuyahoga_version SET major = 0, minor = 9, patch = 0 WHERE assembly = 'Cuyahoga.Modules';