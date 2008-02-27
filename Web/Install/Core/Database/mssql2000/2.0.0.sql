/* DDL */ 
CREATE TABLE cuyahoga_right(
rightid int identity(1,1) NOT NULL CONSTRAINT PK_right PRIMARY KEY,
name nvarchar(50) NOT NULL,
description nvarchar(255) NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_right_name UNIQUE(name))
go

CREATE TABLE cuyahoga_roleright(
roleid int NOT NULL,
rightid int NOT NULL,
CONSTRAINT PK_roleright PRIMARY KEY (roleid, rightid))
go

ALTER TABLE cuyahoga_roleright
	ADD CONSTRAINT FK_roleright_role_roleid 
		FOREIGN KEY (roleid) REFERENCES cuyahoga_role (roleid)
go

ALTER TABLE cuyahoga_roleright
	ADD CONSTRAINT FK_roleright_right_rightid 
		FOREIGN KEY (rightid) REFERENCES cuyahoga_right (rightid)
go

/* DATA */
SET IDENTITY_INSERT cuyahoga_right ON

GO

INSERT INTO cuyahoga_right (rightid, name, description) VALUES (1, 'Anonymous', 'Legacy right, migrated from AccessLevel.Anonymous')
INSERT INTO cuyahoga_right (rightid, name, description) VALUES (2, 'Authenticated', 'Legacy right, migrated from AccessLevel.Authenticated')
INSERT INTO cuyahoga_right (rightid, name, description) VALUES (3, 'Editor', 'Legacy right, migrated from AccessLevel.Editor')
INSERT INTO cuyahoga_right (rightid, name, description) VALUES (4, 'Administrator', 'Legacy right, migrated from AccessLevel.Administrator')

GO

SET IDENTITY_INSERT cuyahoga_right OFF

GO

INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (1, 1)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (1, 2)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (1, 3)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (1, 4)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (2, 1)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (2, 2)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (2, 3)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (3, 1)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (3, 2)
INSERT INTO cuyahoga_roleright(roleid, rightid) VALUES (4, 1)

GO
