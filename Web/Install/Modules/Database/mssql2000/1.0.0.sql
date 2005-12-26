/*
 *  SearchInput module
 */
INSERT INTO cuyahoga_moduletype ([name], assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) 
VALUES ('SearchInput', 'Cuyahoga.Modules', 'Cuyahoga.Modules.Search.SearchInputModule', 'Modules/Search/SearchInput.ascx', NULL, '2005-10-20 14:36:28.324', '2005-10-20 14:36:28.324')
go

/*
 *  Search results settings
 */
DECLARE @moduletypeid int

SELECT @moduletypeid = moduletypeid FROM cuyahoga_moduletype WHERE [name] = 'Search'

INSERT INTO cuyahoga_modulesetting (moduletypeid, [name], friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'RESULTS_PER_PAGE', 'Results per page', 'System.Int32', 0, 1)

INSERT INTO cuyahoga_modulesetting (moduletypeid, [name], friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'SHOW_INPUT_PANEL', 'Show search input box', 'System.Boolean', 0, 0)
GO

INSERT INTO cuyahoga_sectionsetting (sectionid, name, value)
	SELECT sectionid, 'RESULTS_PER_PAGE', '10'
	FROM cuyahoga_section s
		INNER JOIN cuyahoga_moduletype m on m.moduletypeid = s.moduletypeid
	WHERE m.Name = 'Search'
GO

/*
 *  Version
 */
UPDATE cuyahoga_version SET major = 1, minor = 0, patch = 0 WHERE assembly = 'Cuyahoga.Modules'
go