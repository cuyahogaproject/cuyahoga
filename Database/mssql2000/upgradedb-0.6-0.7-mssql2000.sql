/************************
 New tables
************************/

CREATE TABLE Cuyahoga_Menu(
MenuId int identity(1,1) NOT NULL CONSTRAINT PK_Cuyahoga_Menu1 PRIMARY KEY,
RootNodeId int NOT NULL,
Name varchar(50) NOT NULL,
Placeholder varchar(50) NOT NULL,
InsertTimestamp datetime DEFAULT current_timestamp NOT NULL,
UpdateTimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_Cuyahoga_Menu1 UNIQUE(MenuId))
go


CREATE TABLE Cuyahoga_MenuNode(
MenuNodeId int identity(1,1) NOT NULL CONSTRAINT PK_Cuyahoga_MenuNode1 PRIMARY KEY,
MenuId int NOT NULL,
NodeId int NOT NULL,
Position int NOT NULL,
CONSTRAINT UC_Cuyahoga_MenuNode1 UNIQUE(MenuNodeId))
go

ALTER TABLE Cuyahoga_Menu
ADD CONSTRAINT FK_Cuyahoga_Menu_1 
FOREIGN KEY (ParentNodeId) REFERENCES Cuyahoga_Node (NodeId)
go


ALTER TABLE Cuyahoga_MenuNode
ADD CONSTRAINT FK_Cuyahoga_MenuNode_1 
FOREIGN KEY (MenuId) REFERENCES Cuyahoga_Menu (MenuId)
go

ALTER TABLE Cuyahoga_MenuNode
ADD CONSTRAINT FK_Cuyahoga_MenuNode_2 
FOREIGN KEY (NodeId) REFERENCES Cuyahoga_Node (NodeId)
go


/************************
 Cuyahoga_Node changes
************************/
ALTER TABLE Cuyahoga_Node
	ADD	ShowInNavigation bit NULL
	
GO

UPDATE Cuyahoga_Node
	SET	ShowInNavigation = 1
	
GO

ALTER TABLE Cuyahoga_Node
	ALTER COLUMN ShowInNavigation bit NOT NULL
	
GO