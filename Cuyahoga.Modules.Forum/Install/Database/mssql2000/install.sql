CREATE TABLE cm_forums (
	forumid int IDENTITY (1, 1) NOT NULL CONSTRAINT PK_forum PRIMARY KEY,
	updatetimestamp datetime NULL,
	inserttimestamp datetime NULL,
	categoryid int NULL,
	name nvarchar (50) NULL,
	description nchar (254) NULL,
	sortorder int NULL,
	lastposted datetime NULL,
	lastpostid int NULL,
	numtopics int NULL,
	numposts int NULL,
	allowguestpost int NULL,
	lastpostusername nvarchar (50) 
)
go

CREATE TABLE cm_forumposts (
	postid int IDENTITY (1, 1) NOT NULL CONSTRAINT PK_forumposts PRIMARY KEY,
	forumid int NULL,
	updatetimestamp datetime NULL,
	inserttimestamp datetime NULL,
	topic nvarchar (50) NULL,
	replytoid int NULL,
	userid int NULL,
	username nvarchar (50) NULL,
	ip nvarchar (15) NULL,
	message ntext NULL,
	views int NULL,
	replies int NULL,
	attachmentid int NULL
)

CREATE TABLE cm_forumcategory (
	categoryid int IDENTITY (1, 1) NOT NULL CONSTRAINT PK_forumcategory PRIMARY KEY,
	siteid int NULL,
	updatetimestamp datetime NULL,
	inserttimestamp datetime NULL,
	boardid int NULL,
	name nvarchar (50)NULL,
	sortorder int NULL 
)

CREATE TABLE cm_forumemoticon (
	id int IDENTITY (1, 1) NOT NULL CONSTRAINT PK_forumemoticon PRIMARY KEY,
	textversion nvarchar (50) NULL,
	imagename nvarchar (254) NULL,
	updatetimestamp datetime NULL,
	inserttimestamp datetime NULL
)
go

CREATE TABLE cm_forumtag (
	id int IDENTITY (1, 1) NOT NULL CONSTRAINT PK_forumtag PRIMARY KEY,
	forumcodestart nvarchar (50) NULL,
	forumcodeend nvarchar (50) NULL,
	htmlcodestart nvarchar (50) NULL,
	htmlcodeend nvarchar (50) NULL,
	updatetimestamp datetime NULL,
	inserttimestamp datetime NULL
)
go

CREATE TABLE cm_forumuser (
	id int IDENTITY (1, 1) NOT NULL CONSTRAINT PK_cm_forumuser PRIMARY KEY,
	updatetimestamp datetime NULL ,
	inserttimestamp datetime NULL ,
	userid int NULL ,
	location nvarchar (50) NULL ,
	occupation nvarchar (50) NULL ,
	interest nvarchar (100) NULL ,
	homepage nvarchar (100) NULL ,
	msn nvarchar (50) NULL ,
	aimname nvarchar (50) NULL ,
	icqnumber nvarchar (50) NULL ,
	signature ntext NULL ,
	gender int NULL ,
	timezone int NULL ,
	avartar nvarchar (254),
	yahoomessenger nvarchar (50) NULL ,
)
go

CREATE TABLE cm_forumfile (
	id int IDENTITY (1, 1) NOT NULL CONSTRAINT PK_cm_forumfile PRIMARY KEY,
	updatetimestamp datetime NULL ,
	inserttimestamp datetime NULL ,
	origfilename nvarchar (254),
	forumfilename nvarchar (254),
	filesize int NULL,
	dlcount int NULL,
	contenttype nvarchar (50)
)
go

/*
 *  Table data
 */
SET DATEFORMAT ymd

DECLARE @moduletypeid int

INSERT INTO cuyahoga_moduletype (name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) VALUES ('Forum', 'Cuyahoga.Modules.Forum', 'Cuyahoga.Modules.Forum.ForumModule', 'Modules/Forum/UserForum.ascx', 'Modules/Forum/AdminForum.aspx', '2005-02-11 14:36:28.324', '2004-02-11 14:36:28.324')

SELECT @moduletypeid = Scope_Identity()

INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Modules.Forum', 1, 0, 0)
go
INSERT INTO cm_forumemoticon (textversion, imagename,  inserttimestamp, updatetimestamp) VALUES ('8)', 'cool.gif', '2005-02-11 14:36:28.324', '2004-02-11 14:36:28.324')
go
INSERT INTO cm_forumemoticon (textversion, imagename,  inserttimestamp, updatetimestamp) VALUES (':(', 'angry.gif', '2005-02-11 14:36:28.324', '2004-02-11 14:36:28.324')
go
INSERT INTO cm_forumemoticon (textversion, imagename,  inserttimestamp, updatetimestamp) VALUES (':)', 'happy.gif', '2005-02-11 14:36:28.324', '2004-02-11 14:36:28.324')
go
INSERT INTO cm_forumemoticon (textversion, imagename,  inserttimestamp, updatetimestamp) VALUES (';)', 'wink.gif', '2005-02-11 14:36:28.324', '2004-02-11 14:36:28.324')
go
INSERT INTO cm_forumtag (forumcodestart, forumcodeend,  htmlcodestart, htmlcodeend, inserttimestamp, updatetimestamp) VALUES ('[i]', '[/i]', '<em>', '</em>','2005-02-11 14:36:28.324', '2004-02-11 14:36:28.324')
go
INSERT INTO cm_forumtag (forumcodestart, forumcodeend,  htmlcodestart, htmlcodeend, inserttimestamp, updatetimestamp) VALUES ('[b]', '[/b]', '<strong>', '</strong>','2005-02-11 14:36:28.324', '2004-02-11 14:36:28.324')
go
INSERT INTO cm_forumtag (forumcodestart, forumcodeend,  htmlcodestart, htmlcodeend, inserttimestamp, updatetimestamp) VALUES ('[code]', '[/code]', '<code>', '</code>','2005-02-11 14:36:28.324', '2004-02-11 14:36:28.324')
go
INSERT INTO cm_forumtag (forumcodestart, forumcodeend,  htmlcodestart, htmlcodeend, inserttimestamp, updatetimestamp) VALUES ('[pre]', '[/pre]', '<pre>', '</pre>','2005-02-11 14:36:28.324', '2004-02-11 14:36:28.324')
go
INSERT INTO cm_forumtag (forumcodestart, forumcodeend,  htmlcodestart, htmlcodeend, inserttimestamp, updatetimestamp) VALUES ('[li]', '[/li]', '<li>', '</li>','2005-02-11 14:36:28.324', '2004-02-11 14:36:28.324')
go
