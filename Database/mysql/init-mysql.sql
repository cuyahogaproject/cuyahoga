--
-- TOC entry 1534 (class 0 OID 821791)
-- Dependencies: 1170
-- Data for Name: cuyahoga_moduletype; Type: TABLE DATA; Schema: public; Owner: tijn
--

INSERT INTO cuyahoga_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) VALUES (1, 'StaticHtml', 'Cuyahoga.Modules', 'Cuyahoga.Modules.StaticHtml.StaticHtmlModule', 'Modules/StaticHtml/StaticHtml.ascx', 'Modules/StaticHtml/EditHtml.aspx', '2004-10-02 14:36:28.324', '2004-10-02 14:36:28.324');
INSERT INTO cuyahoga_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) VALUES (2, 'Articles', 'Cuyahoga.Modules', 'Cuyahoga.Modules.Articles.ArticleModule', 'Modules/Articles/Articles.ascx', 'Modules/Articles/AdminArticles.aspx', '2004-10-02 14:36:28.324', '2004-10-02 14:36:28.324');
INSERT INTO cuyahoga_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) VALUES (3, 'User', 'Cuyahoga.Modules', 'Cuyahoga.Modules.User.UserModule', 'Modules/User/User.ascx', NULL, '2004-10-02 14:36:28.324', '2004-10-02 14:36:28.324');
INSERT INTO cuyahoga_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) VALUES (4, 'Search', 'Cuyahoga.Modules', 'Cuyahoga.Modules.Search.SearchModule', 'Modules/Search/Search.ascx', NULL, '2004-10-02 14:36:28.324', '2004-10-02 14:36:28.324');
INSERT INTO cuyahoga_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) VALUES (5, 'LanguageSwitcher', 'Cuyahoga.Modules', 'Cuyahoga.Modules.LanguageSwitcher.LanguageSwitcherModule', 'Modules/LanguageSwitcher/LanguageSwitcher.ascx', NULL, '2004-10-02 14:36:28.324', '2004-10-02 14:36:28.324');
INSERT INTO cuyahoga_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) VALUES (6, 'RemoteContent', 'Cuyahoga.Modules', 'Cuyahoga.Modules.RemoteContent.RemoteContentModule', 'Modules/RemoteContent/RemoteContent.ascx', 'Modules/RemoteContent/AdminRemoteContent.aspx', '2005-04-08 14:36:28.324', '2004-04-08 14:36:28.324');
INSERT INTO cuyahoga_moduletype (moduletypeid, name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) VALUES (7, 'Downloads', 'Cuyahoga.Modules.Downloads', 'Cuyahoga.Modules.Downloads.DownloadsModule', 'Modules/Downloads/Downloads.ascx', 'Modules/Downloads/EditDownloads.aspx', '2005-05-15 14:36:28.324', '2004-05-15 14:36:28.324');

--
-- TOC entry 1538 (class 0 OID 822807)
-- Dependencies: 1178
-- Data for Name: cuyahoga_modulesetting; Type: TABLE DATA; Schema: public; Owner: tijn
--

INSERT INTO cuyahoga_modulesetting (modulesettingid, moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (1, 2, 'ALLOW_COMMENTS', 'Allow comments', 'System.Boolean', 0, 1);
INSERT INTO cuyahoga_modulesetting (modulesettingid, moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (2, 2, 'NUMBER_OF_ARTICLES_IN_LIST', 'Number of articles to display', 'System.Int16', 0, 1);
INSERT INTO cuyahoga_modulesetting (modulesettingid, moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (3, 2, 'DISPLAY_TYPE', 'Display type', 'Cuyahoga.Modules.Articles.DisplayType', 1, 1);
INSERT INTO cuyahoga_modulesetting (modulesettingid, moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (4, 2, 'ALLOW_ANONYMOUS_COMMENTS', 'Allow anonymous comments', 'System.Boolean', 0, 1);
INSERT INTO cuyahoga_modulesetting (modulesettingid, moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (5, 2, 'ALLOW_SYNDICATION', 'Allow syndication', 'System.Boolean', 0, 1);
INSERT INTO cuyahoga_modulesetting (modulesettingid, moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (6, 6, 'CACHE_DURATION', 'Local database cache duration (min)', 'System.Int32', 0, 1);
INSERT INTO cuyahoga_modulesetting (modulesettingid, moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (7, 6, 'SHOW_CONTENTS', 'Show feed contents', 'System.Boolean', 0, 1);
INSERT INTO cuyahoga_modulesetting (modulesettingid, moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (8, 6, 'SHOW_DATES', 'Show dates', 'System.Boolean', 0, 1);
INSERT INTO cuyahoga_modulesetting (modulesettingid, moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (9, 6, 'BACKGROUND_REFRESH', 'Use background refreshing', 'System.Boolean', 0, 1);
INSERT INTO cuyahoga_modulesetting (modulesettingid, moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (10, 6, 'SHOW_SOURCES', 'Show feed sources', 'System.Boolean', 0, 1);
INSERT INTO cuyahoga_modulesetting (modulesettingid, moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (11, 6, 'SHOW_AUTHORS', 'Show authors', 'System.Boolean', 0, 1);
INSERT INTO cuyahoga_modulesetting (modulesettingid, moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (12, 7, 'SHOW_PUBLISHER', 'Show publisher', 'System.Boolean', 0, 1);
INSERT INTO cuyahoga_modulesetting (modulesettingid, moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (13, 7, 'SHOW_DATE', 'Show file date', 'System.Boolean', 0, 1);
INSERT INTO cuyahoga_modulesetting (modulesettingid, moduletypeid, name, friendlyname, settingdatatype, iscustomtype, isrequired) VALUES (14, 7, 'PHYSICAL_DIR', 'Physical directory (empty for App_Root/files)', 'System.String', 0, 0);

-- TOC entry 1525 (class 0 OID 17156)
-- Dependencies: 1152
-- Data for Name: cuyahoga_role; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO cuyahoga_role (roleid, name, inserttimestamp, updatetimestamp, permissionlevel) VALUES (3, 'Authenticated user', '2004-01-04 16:34:50.271', '2004-06-25 00:59:02.822', 2);
INSERT INTO cuyahoga_role (roleid, name, inserttimestamp, updatetimestamp, permissionlevel) VALUES (2, 'Editor', '2004-01-04 16:34:25.669', '2004-06-25 00:59:08.256', 6);
INSERT INTO cuyahoga_role (roleid, name, inserttimestamp, updatetimestamp, permissionlevel) VALUES (1, 'Administrator', '2004-01-04 16:33:42.255', '2004-09-19 17:08:47.248', 14);
INSERT INTO cuyahoga_role (roleid, name, inserttimestamp, updatetimestamp, permissionlevel) VALUES (4, 'Anonymous user', '2004-01-04 16:35:10.766', '2004-07-16 21:18:09.017', 1);


--
-- TOC entry 1527 (class 0 OID 17170)
-- Dependencies: 1156
-- Data for Name: cuyahoga_template; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO cuyahoga_template (templateid, name, basepath, templatecontrol, css, inserttimestamp, updatetimestamp) VALUES (1, 'Cuyahoga Home', 'Templates/Classic', 'CuyahogaHome.ascx', 'red.css', '2004-01-26 21:52:52.365', '2004-01-26 21:52:52.365');
INSERT INTO cuyahoga_template (templateid, name, basepath, templatecontrol, css, inserttimestamp, updatetimestamp) VALUES (2, 'Cuyahoga Standard', 'Templates/Classic', 'CuyahogaStandard.ascx', 'red.css', '2004-01-26 21:52:52.365', '2004-01-26 21:52:52.365');
INSERT INTO cuyahoga_template (templateid, name, basepath, templatecontrol, css, inserttimestamp, updatetimestamp) VALUES (3, 'Cuyahoga New', 'Templates/Default', 'CuyahogaNew.ascx', 'red-new.css', '2004-01-26 21:52:52.365', '2004-01-26 21:52:52.365');
INSERT INTO cuyahoga_template (templateid, name, basepath, templatecontrol, css, inserttimestamp, updatetimestamp) VALUES (4, 'Another Red', 'Templates/AnotherRed', 'Cuyahoga.ascx', 'red.css', '2004-01-26 21:52:52.365', '2004-01-26 21:52:52.365');
--
-- TOC entry 1524 (class 0 OID 17149)
-- Dependencies: 1150
-- Data for Name: cuyahoga_user; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO cuyahoga_user (userid, username, `password`, firstname, lastname, email, isactive, inserttimestamp, updatetimestamp, lastlogin, lastip) VALUES (1, 'admin', 'ba213b8c28962d5b00140bdc076796c6', '', '', 'admin@cuyahoga.org', 1, '2004-01-04 16:32:35.099', '2004-11-09 22:48:47.359', '2004-11-09 22:48:47', '127.0.0.1');


--
-- TOC entry 1526 (class 0 OID 17163)
-- Dependencies: 1154
-- Data for Name: cuyahoga_userrole; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO cuyahoga_userrole (userroleid, userid, roleid, inserttimestamp, updatetimestamp) VALUES (1, 1, 1, '2004-09-09 23:30:34.465', '2004-09-09 23:30:34.465');

INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Core', 0, 8, 2);
INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Modules', 0, 8, 2);
INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Modules.Downloads', 0, 8, 2);