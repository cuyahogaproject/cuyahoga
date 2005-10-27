/*
 * Module Connections
 */
CREATE TABLE cuyahoga_sectionconnection(
sectionconnectionid INT NOT NULL AUTO_INCREMENT,
sectionidfrom INT NOT NULL,
sectionidto INT NOT NULL,
actionname VARCHAR(50) NOT NULL
FOREIGN KEY (sectionidfrom) REFERENCES cuyahoga_section (sectionid),
FOREIGN KEY (sectionidto) REFERENCES cuyahoga_section (sectionid),
PRIMARY KEY (sectionconnectionid),
UNIQUE IX_sectionconnection_sectionidfrom_actionname(sectionidfrom, actionname));

/*
 * Version
 */
UPDATE cuyahoga_version SET major = 0, minor = 9, patch = 1 WHERE assembly = 'Cuyahoga.Core';