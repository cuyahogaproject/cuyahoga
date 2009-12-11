

CREATE TABLE cm_statichtml(
statichtmlid serial NOT NULL CONSTRAINT PK_statichtml PRIMARY KEY,
sectionid int4 NOT NULL,
createdby int4 NOT NULL,
modifiedby int4,
title varchar(255),
content text NOT NULL,
inserttimestamp timestamp DEFAULT current_timestamp NOT NULL,
updatetimestamp timestamp DEFAULT current_timestamp NOT NULL,
CONSTRAINT FK_statichtml_section_sectionid FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid),
CONSTRAINT FK_statichtml_user_createdby FOREIGN KEY (createdby) REFERENCES cuyahoga_user (userid),
CONSTRAINT FK_statichtml_user_modifiedby FOREIGN KEY (modifiedby) REFERENCES cuyahoga_user (userid));

-- DATA --

INSERT INTO cuyahoga_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, autoactivate, inserttimestamp, updatetimestamp) VALUES (1, 'StaticHtml', 'Cuyahoga.Modules', 'Cuyahoga.Modules.StaticHtml.StaticHtmlModule', 'Modules/StaticHtml/StaticHtml.ascx', 'Modules/StaticHtml/EditHtml.aspx', true, '2004-10-02 14:36:28.324', '2004-10-02 14:36:28.324');
INSERT INTO cuyahoga_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, autoactivate, inserttimestamp, updatetimestamp) VALUES (3, 'User', 'Cuyahoga.Modules', 'Cuyahoga.Modules.User.UserModule', 'Modules/User/User.ascx', NULL, true, '2004-10-02 14:36:28.324', '2004-10-02 14:36:28.324');
INSERT INTO cuyahoga_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, autoactivate, inserttimestamp, updatetimestamp) VALUES (4, 'Search', 'Cuyahoga.Modules', 'Cuyahoga.Modules.Search.SearchModule', 'Modules/Search/Search.ascx', NULL, true, '2004-10-02 14:36:28.324', '2004-10-02 14:36:28.324');
INSERT INTO cuyahoga_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, autoactivate, inserttimestamp, updatetimestamp) VALUES (5, 'LanguageSwitcher', 'Cuyahoga.Modules', 'Cuyahoga.Modules.LanguageSwitcher.LanguageSwitcherModule', 'Modules/LanguageSwitcher/LanguageSwitcher.ascx', NULL, true, '2004-10-02 14:36:28.324', '2004-10-02 14:36:28.324');
INSERT INTO cuyahoga_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, autoactivate, inserttimestamp, updatetimestamp) VALUES (7, 'UserProfile', 'Cuyahoga.Modules', 'Cuyahoga.Modules.User.ProfileModule', 'Modules/User/EditProfile.ascx', NULL, true, '2005-10-20 14:36:28.324', '2005-10-20 14:36:28.324');
INSERT INTO cuyahoga_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, autoactivate, inserttimestamp, updatetimestamp) VALUES (8, 'SearchInput', 'Cuyahoga.Modules', 'Cuyahoga.Modules.Search.SearchInputModule', 'Modules/Search/SearchInput.ascx', NULL, true, '2005-10-20 14:36:28.324', '2005-10-20 14:36:28.324');
INSERT INTO cuyahoga_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, autoactivate, inserttimestamp, updatetimestamp) VALUES (9, 'Sitemap', 'Cuyahoga.Modules', 'Cuyahoga.Modules.Sitemap.SitemapModule', 'Modules/Sitemap/SitemapControl.ascx', NULL, true, '2005-10-20 14:36:28.324', '2005-10-20 14:36:28.324');


INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (3, 'SHOW_REGISTER', 'Show register link', 'System.Boolean', false, false);
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (3, 'SHOW_RESET_PASSWORD', 'Show reset password link', 'System.Boolean', false, false);
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (3, 'SHOW_EDIT_PROFILE', 'Show edit profile link', 'System.Boolean', false, false);
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (4, 'RESULTS_PER_PAGE', 'Results per page', 'System.Int32', false, true);
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (4, 'SHOW_INPUT_PANEL', 'Show search input box', 'System.Boolean', false, false);
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (5, 'DISPLAY_MODE', 'Display mode', 'Cuyahoga.Modules.LanguageSwitcher.DisplayMode', true, true);
INSERT INTO cuyahoga_modulesetting (moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (5, 'REDIRECT_TO_USER_LANGUAGE', 'Redirect user to browser language when possible?', 'System.Boolean', false, true);

INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Modules', 1, 5, 2);