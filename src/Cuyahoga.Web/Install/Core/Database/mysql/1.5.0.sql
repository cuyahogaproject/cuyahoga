/*
 * DDL Changes
 */
CREATE TABLE cuyahoga_moduleservice(
moduleserviceid INT NOT NULL AUTO_INCREMENT,
moduletypeid INT NOT NULL,
servicekey VARCHAR(50) NOT NULL,
servicetype VARCHAR(255) NOT NULL,
classtype VARCHAR(255) NOT NULL,
lifestyle VARCHAR(10) NOT NULL,
FOREIGN KEY (moduletypeid) REFERENCES cuyahoga_moduletype (moduletypeid),
PRIMARY KEY (moduleserviceid),
UNIQUE IX_moduleservice_moduletypeid_servicekey (moduletypeid,servicekey));


/*
 * Version
 */
UPDATE cuyahoga_version SET major = 1, minor = 5, patch = 0 WHERE assembly = 'Cuyahoga.Core';
