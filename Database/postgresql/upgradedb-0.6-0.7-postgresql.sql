CREATE TABLE cuyahoga_menu(
menuid serial NOT NULL CONSTRAINT UC_cuyahoga_menu1 UNIQUE CONSTRAINT PK_cuyahoga_menu1 PRIMARY KEY,
rootnodeid int4 NOT NULL,
name varchar(50) NOT NULL,
placeholder varchar(50) NOT NULL,
inserttimestamp date DEFAULT current_timestamp NOT NULL,
updatetimestamp date DEFAULT current_timestamp NOT NULL,
CONSTRAINT FK_cuyahoga_menu_1 FOREIGN KEY (rootnodeid) REFERENCES cuyahoga_node (nodeid));

CREATE TABLE cuyahoga_menunode(
menunodeid serial NOT NULL CONSTRAINT UC_cuyahoga_menunode1 UNIQUE CONSTRAINT PK_cuyahoga_menunode1 PRIMARY KEY,
menuid int4 NOT NULL,
nodeid int4 NOT NULL,
position int4 NOT NULL,
CONSTRAINT FK_cuyahoga_menunode_1 FOREIGN KEY (menuid) REFERENCES cuyahoga_menu (menuid),
CONSTRAINT FK_cuyahoga_menunode_2 FOREIGN KEY (nodeid) REFERENCES cuyahoga_node (nodeid));

CREATE TABLE cuyahoga_sitealias(
sitealiasid serial NOT NULL CONSTRAINT UC_cuyahoga_sitealias1 UNIQUE CONSTRAINT PK_cuyahoga_sitealias1 PRIMARY KEY,
siteid int4 NOT NULL,
nodeid int4,
url varchar(100) NOT NULL,
inserttimestamp date DEFAULT current_timestamp NOT NULL,
updatetimestamp date DEFAULT current_timestamp NOT NULL,
CONSTRAINT FK_cuyahoga_sitealias_1 FOREIGN KEY (nodeid) REFERENCES cuyahoga_node (nodeid),
CONSTRAINT FK_cuyahoga_sitealias_2 FOREIGN KEY (siteid) REFERENCES cuyahoga_site (siteid));

/************************
 Cuyahoga_Node changes
************************/
ALTER TABLE cuyahoga_node
	ADD COLUMN showinnavigation bool;

UPDATE cuyahoga_node
	SET	showinnavigation = true;
	
ALTER TABLE cuyahoga_node
	ALTER COLUMN showinnavigation SET NOT NULL;

/************************
 Cuyahoga_Template changes
************************/
ALTER TABLE cuyahoga_template
	RENAME COLUMN path TO basepath;

ALTER TABLE cuyahoga_template
	ADD	 COLUMN templatecontrol varchar(50);
	
ALTER TABLE cuyahoga_template
	ALTER COLUMN css DROP NOT NULL;
