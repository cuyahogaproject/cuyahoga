/************************
 New tables
************************/

CREATE TABLE cuyahoga_menu(
menuid int identity(1,1) NOT NULL CONSTRAINT PK_cuyahoga_menu1 PRIMARY KEY,
rootnodeid int NOT NULL,
name varchar(50) NOT NULL,
placeholder varchar(50) NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_cuyahoga_menu1 UNIQUE(menuid))
go


CREATE TABLE cuyahoga_menunode(
menunodeid int identity(1,1) NOT NULL CONSTRAINT PK_cuyahoga_menunode1 PRIMARY KEY,
menuid int NOT NULL,
nodeid int NOT NULL,
position int NOT NULL,
CONSTRAINT UC_cuyahoga_menunode1 UNIQUE(menunodeid))
go

CREATE TABLE cuyahoga_sitealias(
sitealiasid int identity(1,1) NOT NULL CONSTRAINT PK_cuyahoga_sitealias1 PRIMARY KEY,
siteid int NOT NULL,
nodeid int NULL,
url varchar(100) NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_cuyahoga_sitealias1 UNIQUE(sitealiasid))
go

ALTER TABLE cuyahoga_menu
ADD CONSTRAINT FK_cuyahoga_menu_1 
FOREIGN KEY (rootnodeid) REFERENCES cuyahoga_node (nodeid)
go


ALTER TABLE cuyahoga_menunode
ADD CONSTRAINT FK_cuyahoga_menunode_1 
FOREIGN KEY (menuid) REFERENCES cuyahoga_menu (menuid)
go

ALTER TABLE cuyahoga_menunode
ADD CONSTRAINT FK_cuyahoga_menunode_2 
FOREIGN KEY (nodeid) REFERENCES cuyahoga_node (nodeid)
go

ALTER TABLE cuyahoga_sitealias
ADD CONSTRAINT FK_cuyahoga_sitealias_1 
FOREIGN KEY (nodeid) REFERENCES cuyahoga_node (nodeid)
go

ALTER TABLE cuyahoga_sitealias
ADD CONSTRAINT FK_cuyahoga_sitealias_2 
FOREIGN KEY (siteid) REFERENCES cuyahoga_site (siteid)
go


/************************
 Cuyahoga_Node changes
************************/
ALTER TABLE cuyahoga_node
	ADD	showinnavigation bit NULL
	
GO

UPDATE cuyahoga_node
	SET	showinnavigation = 1
	
GO

ALTER TABLE cuyahoga_node
	ALTER COLUMN showinnavigation bit NOT NULL
	
GO

/************************
 Cuyahoga_Template changes
************************/
EXEC sp_rename 'cuyahoga_template.[path]', 'basepath', 'COLUMN' 

GO

ALTER TABLE cuyahoga_template
	ADD	templatecontrol varchar(50) NULL
	
GO

ALTER TABLE cuyahoga_template
	ALTER COLUMN css varchar(100) NULL
	
GO