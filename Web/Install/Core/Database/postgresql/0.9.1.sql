/*
 * Module Connections
 */
CREATE TABLE cuyahoga_sectionconnection(
sectionconnectionid serial NOT NULL CONSTRAINT PK_sectionconnection PRIMARY KEY,
sectionidfrom int4 NOT NULL,
sectionidto int4 NOT NULL,
actionname varchar(50) NOT NULL,
CONSTRAINT FK_sectionconnection_section_sectionidfrom FOREIGN KEY (sectionidfrom) REFERENCES cuyahoga_section (sectionid),
CONSTRAINT FK_sectionconnection_section_sectionidto FOREIGN KEY (sectionidto) REFERENCES cuyahoga_section (sectionid));

CREATE UNIQUE INDEX IX_sectionconnection_sectionidfrom_actionname ON cuyahoga_sectionconnection (sectionidfrom, actionname);

/*
 * Template sections
 */
CREATE TABLE cuyahoga_templatesection(
templatesectionid serial NOT NULL CONSTRAINT PK_templatesection PRIMARY KEY,
templateid int4 NOT NULL,
sectionid int4 NOT NULL,
placeholder varchar(100) NOT NULL,
CONSTRAINT FK_templatesection_template_templateid FOREIGN KEY (templateid) REFERENCES cuyahoga_template (templateid),
CONSTRAINT FK_templatesection_section_sectionid FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid));

CREATE UNIQUE INDEX IX_templatesection_templateidid_placeholder ON cuyahoga_templatesection (templateid, placeholder);

/*
 * Version
 */
UPDATE cuyahoga_version SET major = 0, minor = 9, patch = 1 WHERE assembly = 'Cuyahoga.Core';