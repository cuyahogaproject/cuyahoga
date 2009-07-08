
CREATE TABLE cm_statichtml(
contentitemid bigint NOT NULL CONSTRAINT PK_statichtml PRIMARY KEY,
content ntext NOT NULL)
go

ALTER TABLE cm_statichtml
	ADD CONSTRAINT FK_statichtml_contentitem_contentitemid
		FOREIGN KEY (contentitemid) REFERENCES cuyahoga_contentitem (contentitemid)
go

-- DATA
SET IDENTITY_INSERT cuyahoga_moduletype ON

GO

INSERT INTO cuyahoga_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, autoactivate) 
VALUES (1, 'StaticHtml', 'Cuyahoga.Modules', 'Cuyahoga.Modules.StaticHtml.StaticHtmlModule', 'Modules/StaticHtml/StaticHtml.ascx', 'Modules/StaticHtml/EditHtml.aspx', 1)
INSERT INTO cuyahoga_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, autoactivate) 
VALUES (3, 'User', 'Cuyahoga.Modules', 'Cuyahoga.Modules.User.UserModule', 'Modules/User/User.ascx', NULL, 1)
INSERT INTO cuyahoga_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, autoactivate) 
VALUES (4, 'Search', 'Cuyahoga.Modules', 'Cuyahoga.Modules.Search.SearchModule', 'Modules/Search/Search.ascx', NULL, 1)
INSERT INTO cuyahoga_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, autoactivate) 
VALUES (5, 'LanguageSwitcher', 'Cuyahoga.Modules', 'Cuyahoga.Modules.LanguageSwitcher.LanguageSwitcherModule', 'Modules/LanguageSwitcher/LanguageSwitcher.ascx', NULL, 1)
INSERT INTO cuyahoga_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, autoactivate) 
VALUES (7, 'UserProfile', 'Cuyahoga.Modules', 'Cuyahoga.Modules.User.ProfileModule', 'Modules/User/EditProfile.ascx', NULL, 1)
INSERT INTO cuyahoga_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, autoactivate) 
VALUES (8, 'SearchInput', 'Cuyahoga.Modules', 'Cuyahoga.Modules.Search.SearchInputModule', 'Modules/Search/SearchInput.ascx', NULL, 1)
INSERT INTO cuyahoga_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, autoactivate) 
VALUES (9, 'Sitemap', 'Cuyahoga.Modules', 'Cuyahoga.Modules.Sitemap.SitemapModule', 'Modules/Sitemap/SitemapControl.ascx', NULL, 1)
INSERT INTO cuyahoga_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, autoactivate) 
VALUES (10, 'Categories', 'Cuyahoga.Modules', 'Cuyahoga.Modules.Categories.CategoryModule', 'Modules/Categories/CategoryControl.ascx', NULL, 1)

GO

SET IDENTITY_INSERT cuyahoga_moduletype OFF

GO

INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (3, 'SHOW_REGISTER', 'Show register link', 'System.Boolean', 0, 0)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (3, 'SHOW_RESET_PASSWORD', 'Show reset password link', 'System.Boolean', 0, 0)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (3, 'SHOW_EDIT_PROFILE', 'Show edit profile link', 'System.Boolean', 0, 0)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (4, 'RESULTS_PER_PAGE', 'Results per page', 'System.Int32', 0, 1)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (4, 'SHOW_INPUT_PANEL', 'Show search input box', 'System.Boolean', 0, 0)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (5, 'DISPLAY_MODE', 'Display mode', 'Cuyahoga.Modules.LanguageSwitcher.DisplayMode', 1, 1)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (5, 'REDIRECT_TO_USER_LANGUAGE', 'Redirect user to browser language when possible?', 'System.Boolean', 0, 1)
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) 
VALUES (10, 'ROOT_CATEGORY_KEY', 'Root category key', 'System.String', 0, 1)
GO

INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Modules', 2, 0, 0)

GO
