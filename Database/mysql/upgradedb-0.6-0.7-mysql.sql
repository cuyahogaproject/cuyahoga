CREATE TABLE cuyahoga_menu(
menuid INT NOT NULL AUTO_INCREMENT,
rootnodeid INT NOT NULL,
name VARCHAR(50) NOT NULL,
placeholder VARCHAR(50) NOT NULL,
inserttimestamp TIMESTAMP NOT NULL,
updatetimestamp DATETIME  NOT NULL,
FOREIGN KEY (rootnodeid) REFERENCES cuyahoga_node (nodeid),
PRIMARY KEY (menuid),
UNIQUE UC_menuid (menuid));


CREATE TABLE cuyahoga_menunode(
menunodeid INT NOT NULL AUTO_INCREMENT,
menuid INT NOT NULL,
nodeid INT NOT NULL,
position INT NOT NULL,
FOREIGN KEY (menuid) REFERENCES cuyahoga_menu (menuid),
FOREIGN KEY (nodeid) REFERENCES cuyahoga_node (nodeid),
PRIMARY KEY (menunodeid),
UNIQUE UC_menunodeid (menunodeid));

CREATE TABLE cuyahoga_sitealias(
sitealiasid INT NOT NULL AUTO_INCREMENT,
siteid INT NOT NULL,
nodeid INT,
url VARCHAR(100) NOT NULL,
inserttimestamp TIMESTAMP NOT NULL,
updatetimestamp DATETIME  NOT NULL,
FOREIGN KEY (nodeid) REFERENCES cuyahoga_node (nodeid),
FOREIGN KEY (siteid) REFERENCES cuyahoga_site (siteid),
PRIMARY KEY (sitealiasid),
UNIQUE UC_sitealiasid (sitealiasid));

/************************
 Cuyahoga_Node changes
************************/
ALTER TABLE cuyahoga_node
	ADD COLUMN showinnavigation TINYINT;

UPDATE cuyahoga_node
	SET	showinnavigation = 1;
	
ALTER TABLE cuyahoga_node
	CHANGE COLUMN showinnavigation showinnavigation TINYINT NOT NULL;

/************************
 Cuyahoga_Template changes
************************/
ALTER TABLE cuyahoga_template
	CHANGE COLUMN path basepath VARCHAR(100) NOT NULL;

ALTER TABLE cuyahoga_template
	ADD	 COLUMN templatecontrol VARCHAR(50);
	
ALTER TABLE cuyahoga_template
	CHANGE COLUMN css css VARCHAR(100);
