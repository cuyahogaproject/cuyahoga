CREATE TABLE cm_forums (
forumid serial NOT NULL CONSTRAINT PK_forums PRIMARY KEY,
updatetimestamp timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
inserttimestamp timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
categoryid int4,
name varchar(50),
description varchar(254),
sortorder int4,
lastposted timestamp,
lastpostid int4,
numtopics int4,
numposts int4,
allowguestpost int4,
lastpostusername varchar(50) NULL
);

CREATE TABLE cm_forumposts (
	postid serial NOT NULL CONSTRAINT PK_forumposts PRIMARY KEY,
	forumid int4,
	updatetimestamp timestamp,
	inserttimestamp timestamp,
	topic varchar(50),
	replytoid int4,
	userid int4,
	username varchar(50),
	ip varchar(15),
	message text NULL,
	views int4,
	replies int4,
	attachmentid int4
);

CREATE TABLE cm_forumcategory (
	categoryid serial NOT NULL CONSTRAINT PK_forumcategory PRIMARY KEY,
	siteid int4,
	updatetimestamp timestamp,
	inserttimestamp timestamp,
	boardid int4,
	name varchar(50),
	sortorder int4
);


CREATE TABLE cm_forumemoticon (
id serial NOT NULL CONSTRAINT PK_forumemoticon PRIMARY KEY,
textversion varchar(50),
imagename varchar(254),
updatetimestamp timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
inserttimestamp timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP
);


CREATE TABLE cm_forumtag (
id serial NOT NULL CONSTRAINT PK_forumtag PRIMARY KEY,
forumcodestart varchar(50),
forumcodeend varchar(50),
htmlcodestart varchar(50),
htmlcodeend varchar(50),
updatetimestamp timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
inserttimestamp timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE cm_forumuser (
id serial NOT NULL CONSTRAINT PK_forumuser PRIMARY KEY,
updatetimestamp timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
inserttimestamp timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
userid int4,
location varchar(50),
interest varchar(100),
homepage varchar(100),
msn varchar(50),
aimname varchar(50),
icqnumber varchar(50),
signature text,
gender int4,
timezone int4,
avartar varchar(254),
yahoomessenger varchar (50)
);


CREATE TABLE cm_forumfile (
id serial NOT NULL CONSTRAINT PK_forumfile PRIMARY KEY,
updatetimestamp timestamp,
inserttimestamp timestamp,
origfilename varchar (254),
forumfilename varchar (254),
filesize int4,
dlcount int4,
contenttype varchar (50)
);

/*
 *  Table data
 */

INSERT INTO cuyahoga_moduletype (name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) 
VALUES ('Forum', 'Cuyahoga.Modules.Forum', 'Cuyahoga.Modules.Forum.ForumModule', 'Modules/Forum/UserForum.ascx', 'Modules/Forum/AdminForum.aspx', '2005-02-11 14:36:28.324', '2004-02-11 14:36:28.324');

INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Modules.Forum', 1, 0, 0);

INSERT INTO cm_forumemoticon (textversion, imagename, inserttimestamp, updatetimestamp) 
VALUES ('8)', 'cool.gif', '2005-02-11 14:36:28.324', '2004-02-11 14:36:28.324');

INSERT INTO cm_forumemoticon (textversion, imagename, inserttimestamp, updatetimestamp) 
VALUES (':(', 'angry.gif', '2005-02-11 14:36:28.324', '2004-02-11 14:36:28.324');

INSERT INTO cm_forumemoticon (textversion, imagename, inserttimestamp, updatetimestamp) 
VALUES (':)', 'happy.gif', '2005-02-11 14:36:28.324', '2004-02-11 14:36:28.324');

INSERT INTO cm_forumemoticon (textversion, imagename, inserttimestamp, updatetimestamp) 
VALUES (';)', 'wink.gif', '2005-02-11 14:36:28.324', '2004-02-11 14:36:28.324');

INSERT INTO cm_forumtag (forumcodestart, forumcodeend,  htmlcodestart, htmlcodeend, inserttimestamp, updatetimestamp) 
VALUES ('[i]', '[/i]', '<em>', '</em>','2005-02-11 14:36:28.324', '2004-02-11 14:36:28.324');

INSERT INTO cm_forumtag (forumcodestart, forumcodeend,  htmlcodestart, htmlcodeend, inserttimestamp, updatetimestamp) 
VALUES ('[b]', '[/b]', '<strong>', '</strong>','2005-02-11 14:36:28.324', '2004-02-11 14:36:28.324');

INSERT INTO cm_forumtag (forumcodestart, forumcodeend,  htmlcodestart, htmlcodeend, inserttimestamp, updatetimestamp) 
VALUES ('[code]', '[/code]', '<code>', '</code>','2005-02-11 14:36:28.324', '2004-02-11 14:36:28.324');

INSERT INTO cm_forumtag (forumcodestart, forumcodeend,  htmlcodestart, htmlcodeend, inserttimestamp, updatetimestamp) 
VALUES ('[pre]', '[/pre]', '<pre>', '</pre>','2005-02-11 14:36:28.324', '2004-02-11 14:36:28.324');

INSERT INTO cm_forumtag (forumcodestart, forumcodeend,  htmlcodestart, htmlcodeend, inserttimestamp, updatetimestamp) 
VALUES ('[li]', '[/li]', '<li>', '</li>','2005-02-11 14:36:28.324', '2004-02-11 14:36:28.324');

