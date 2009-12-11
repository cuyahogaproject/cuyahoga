/*
 * DDL Changes
 */
CREATE TABLE cuyahoga_moduleservice(
moduleserviceid int identity(1,1) NOT NULL CONSTRAINT PK_moduleservice PRIMARY KEY,
moduletypeid int NOT NULL,
servicekey nvarchar(50) NOT NULL,
servicetype nvarchar(255) NOT NULL,
classtype nvarchar(255) NOT NULL,
lifestyle nvarchar(10) NULL)
go

CREATE UNIQUE INDEX IX_moduleservice_moduletypeid_servicekey ON cuyahoga_moduleservice (moduletypeid,servicekey)
go

ALTER TABLE cuyahoga_moduleservice
ADD CONSTRAINT FK_moduleservice_moduletype_moduletypeid
FOREIGN KEY (moduletypeid) REFERENCES cuyahoga_moduletype (moduletypeid)
go



/*
 * Version
 */
UPDATE cuyahoga_version SET major = 1, minor = 5, patch = 0 WHERE assembly = 'Cuyahoga.Core'
go