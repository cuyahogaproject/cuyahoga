/************************
 Cuyahoga_Site changes
************************/
ALTER TABLE Cuyahoga_Site
	ADD	RoleId int NULL
	,	DefaultPlaceholder varchar(100) NULL
	,	WebmasterEmail varchar(100) NULL
	
GO

-- defaults
UPDATE Cuyahoga_Site
	SET	RoleId = (SELECT TOP 1 RoleId FROM Cuyahoga_Role WHERE PermissionLevel = 1) 
	,	WebmasterEmail = 'webmaster@localhost'
	
GO

ALTER TABLE Cuyahoga_Site
	ALTER COLUMN RoleId int NOT NULL
	
GO

ALTER TABLE Cuyahoga_Site
	ALTER COLUMN WebmasterEmail varchar(100) NOT NULL
	
GO

ALTER TABLE Cuyahoga_Site
	ADD CONSTRAINT FK_Cuyahoga_Site_Cuyahoga_Role
		FOREIGN KEY (RoleId) REFERENCES Cuyahoga_Role (RoleId)

GO

/************************
 Cuyahoga_User changes
************************/

ALTER TABLE Cuyahoga_User
	ADD	Website varchar(100) NULL
	,	IsActive bit NULL
	
GO

UPDATE TABLE Cuyahoga_User
	SET IsActive = 1
	
GO