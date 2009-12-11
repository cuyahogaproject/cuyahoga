-- first set sequence values, otherwise the inserts will fail due to violation of the pk_constraint
SELECT setval('cuyahoga_moduletype_moduletypeid_seq', (SELECT max(moduletypeid) FROM cuyahoga_moduletype));
SELECT setval('cuyahoga_modulesetting_modulesettingid_seq', (SELECT max(modulesettingid) FROM cuyahoga_modulesetting));

/*
 *  SearchInput module
 */
INSERT INTO cuyahoga_moduletype (name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) 
VALUES ('SearchInput', 'Cuyahoga.Modules', 'Cuyahoga.Modules.Search.SearchInputModule', 'Modules/Search/SearchInput.ascx', NULL, '2005-10-20 14:36:28.324', '2005-10-20 14:36:28.324');

/*
 *  Search results settings
 */
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
	SELECT moduletypeid, 'RESULTS_PER_PAGE', 'Results per page', 'System.Int32', false, true
	FROM cuyahoga_moduletype 
	WHERE name = 'Search';

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
	SELECT moduletypeid, 'SHOW_INPUT_PANEL', 'Show search input box', 'System.Boolean', false, false
	FROM cuyahoga_moduletype 
	WHERE name = 'Search';

INSERT INTO cuyahoga_sectionsetting (sectionid, name, value)
	SELECT sectionid, 'RESULTS_PER_PAGE', '10'
	FROM cuyahoga_section s
		INNER JOIN cuyahoga_moduletype m on m.moduletypeid = s.moduletypeid
	WHERE m.Name = 'Search';
/*
 *  Version
 */
UPDATE cuyahoga_version SET major = 1, minor = 0, patch = 0 WHERE assembly = 'Cuyahoga.Modules';
