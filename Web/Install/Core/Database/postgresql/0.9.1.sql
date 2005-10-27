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
 * Version
 */
UPDATE cuyahoga_version SET major = 0, minor = 9, patch = 1 WHERE assembly = 'Cuyahoga.Core';