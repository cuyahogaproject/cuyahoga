/*
 *  Sitemap module
 */
INSERT INTO cuyahoga_moduletype ([name], assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) 
VALUES ('Sitemap', 'Cuyahoga.Modules', 'Cuyahoga.Modules.Sitemap.SitemapModule', 'Modules/Sitemap/SitemapControl.ascx', NULL, '2005-10-20 14:36:28.324', '2005-10-20 14:36:28.324')
go

/*
 *  Language switcher settings
 */
DECLARE @moduletypeid int

SELECT @moduletypeid = moduletypeid FROM cuyahoga_moduletype WHERE [name] = 'LanguageSwitcher'

INSERT INTO cuyahoga_modulesetting (moduletypeid, [name], friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'DISPLAY_MODE', 'Display mode', 'Cuyahoga.Modules.LanguageSwitcher.DisplayMode', 1, 1)

INSERT INTO cuyahoga_modulesetting (moduletypeid, [name], friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'REDIRECT_TO_USER_LANGUAGE', 'Redirect user to browser language when possible?', 'System.Boolean', 0, 1)
GO

INSERT INTO cuyahoga_sectionsetting (sectionid, name, value)
	SELECT sectionid, 'DISPLAY_MODE', 'DropDown'
	FROM cuyahoga_section s
		INNER JOIN cuyahoga_moduletype m on m.moduletypeid = s.moduletypeid
	WHERE m.Name = 'LanguageSwitcher'
GO

INSERT INTO cuyahoga_sectionsetting (sectionid, name, value)
	SELECT sectionid, 'REDIRECT_TO_USER_LANGUAGE', 'False'
	FROM cuyahoga_section s
		INNER JOIN cuyahoga_moduletype m on m.moduletypeid = s.moduletypeid
	WHERE m.Name = 'LanguageSwitcher'
GO

/*
 *  Move Articles and RemoteContent modules to their own assemblies
 */
UPDATE cuyahoga_moduletype SET assemblyname = 'Cuyahoga.Modules.Articles' WHERE [name] = 'Articles'
INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Modules.Articles', 1, 5, 0)

GO

UPDATE cuyahoga_moduletype SET assemblyname = 'Cuyahoga.Modules.RemoteContent' WHERE [name] = 'RemoteContent'
INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Modules.RemoteContent', 1, 5, 0)

GO

/*
 *  Add new settings to Articles module and add the IArticleDao module service
 */
DECLARE @moduletypeid int

SELECT @moduletypeid = moduletypeid FROM cuyahoga_moduletype WHERE [name] = 'Articles'

INSERT INTO cuyahoga_modulesetting (moduletypeid, [name], friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'SHOW_ARCHIVE', 'Show link to archived articles', 'System.Boolean', 0, 1)

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'SHOW_CATEGORY', 'Show category', 'System.Boolean', 0, 1)

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'SHOW_DATETIME', 'Show publish date and time', 'System.Boolean', 0, 1)

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (@moduletypeid, 'SHOW_AUTHOR', 'Show author', 'System.Boolean', 0, 1)

INSERT INTO cuyahoga_moduleservice (moduletypeid, servicekey, servicetype, classtype) 
VALUES (@moduletypeid, 'articles.articledao', 'Cuyahoga.Modules.Articles.DataAccess.IArticleDao, Cuyahoga.Modules.Articles', 'Cuyahoga.Modules.Articles.DataAccess.ArticleDao, Cuyahoga.Modules.Articles')
GO

INSERT INTO cuyahoga_sectionsetting (sectionid, name, value)
	SELECT sectionid, 'SHOW_ARCHIVE', 'False'
	FROM cuyahoga_section s
		INNER JOIN cuyahoga_moduletype m on m.moduletypeid = s.moduletypeid
	WHERE m.Name = 'Articles'
GO

INSERT INTO cuyahoga_sectionsetting (sectionid, name, value)
	SELECT sectionid, 'SHOW_CATEGORY', 'True'
	FROM cuyahoga_section s
		INNER JOIN cuyahoga_moduletype m on m.moduletypeid = s.moduletypeid
	WHERE m.Name = 'Articles'
GO

INSERT INTO cuyahoga_sectionsetting (sectionid, name, value)
	SELECT sectionid, 'SHOW_DATETIME', 'True'
	FROM cuyahoga_section s
		INNER JOIN cuyahoga_moduletype m on m.moduletypeid = s.moduletypeid
	WHERE m.Name = 'Articles'
GO

INSERT INTO cuyahoga_sectionsetting (sectionid, name, value)
	SELECT sectionid, 'SHOW_AUTHOR', 'True'
	FROM cuyahoga_section s
		INNER JOIN cuyahoga_moduletype m on m.moduletypeid = s.moduletypeid
	WHERE m.Name = 'Articles'
GO


/*
 *  Add siteid FK to ArticleCategory
 */
ALTER TABLE cm_articlecategory
	ADD siteid int NULL
go

-- by default, all categories are linked to the first site 
UPDATE cm_articlecategory
	SET siteid = (SELECT MIN(siteid) FROM cuyahoga_site)
go

ALTER TABLE cm_articlecategory
	ALTER COLUMN siteid int NOT NULL
go

ALTER TABLE cm_articlecategory
	ADD CONSTRAINT FK_articlecategory_site_siteid
		FOREIGN KEY (siteid) REFERENCES cuyahoga_site (siteid)
go 
 
/*
 *  Version
 */
UPDATE cuyahoga_version SET major = 1, minor = 5, patch = 0 WHERE assembly = 'Cuyahoga.Modules'
go