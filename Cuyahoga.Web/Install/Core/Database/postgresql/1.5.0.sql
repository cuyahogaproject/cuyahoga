/*
 * DDL Changes
 */
CREATE TABLE cuyahoga_moduleservice(
moduleserviceid serial NOT NULL CONSTRAINT PK_moduleservice PRIMARY KEY,
moduletypeid int4 NOT NULL,
servicekey varchar(50) NOT NULL,
servicetype varchar(255) NOT NULL,
classtype varchar(255) NOT NULL,
lifestyle varchar(10),
CONSTRAINT FK_moduleservice_moduletype_moduletypeid FOREIGN KEY (moduletypeid) REFERENCES cuyahoga_moduletype (moduletypeid));

CREATE UNIQUE INDEX IX_modulesetting_moduletypeid_servicekey ON cuyahoga_modulesetting (moduletypeid,servicekey);


/*
 * Version
 */
UPDATE cuyahoga_version SET major = 1, minor = 5, patch = 0 WHERE assembly = 'Cuyahoga.Core';
