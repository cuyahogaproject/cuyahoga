/*
 * Module Connections
 */
CREATE TABLE cuyahoga_sectionconnection(
sectionconnectionid int identity(1,1) NOT NULL CONSTRAINT PK_sectionconnection PRIMARY KEY,
sectionidfrom int NOT NULL,
sectionidto int NOT NULL,
actionname nvarchar(50) NOT NULL)
go

CREATE UNIQUE INDEX IX_sectionconnection_sectionidfrom_actionname ON cuyahoga_sectionconnection (sectionidfrom, actionname)
go

ALTER TABLE cuyahoga_sectionconnection
ADD CONSTRAINT FK_sectionconnection_section_sectionidfrom
FOREIGN KEY (sectionidfrom) REFERENCES cuyahoga_section (sectionid)
go

ALTER TABLE cuyahoga_sectionconnection
ADD CONSTRAINT FK_sectionconnection_section_sectionidto
FOREIGN KEY (sectionidto) REFERENCES cuyahoga_section (sectionid)
go

/*
 * Version
 */
UPDATE cuyahoga_version SET major = 0, minor = 9, patch = 1 WHERE assembly = 'Cuyahoga.Core'
go