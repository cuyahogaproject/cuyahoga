CREATE TABLE cm_forums (
forumid INT NOT NULL AUTO_INCREMENT,
updatetimestamp DATETIME NOT NULL,
inserttimestamp TIMESTAMP NOT NULL,
categoryid INT NULL,
name VARCHAR(50) NULL,
description VARCHAR(254) NULL,
sortorder INT NULL,
lastposted DATETIME NULL,
lastpostid INT NULL,
numtopics INT NULL,
numposts INT NULL,
allowguestpost INT NULL,
lastpostusername VARCHAR(50) NULL,
PRIMARY KEY (forumid)
);

CREATE TABLE cm_forumposts (
	postid INT NOT NULL AUTO_INCREMENT,
	forumid INT NULL,
	updatetimestamp DATETIME NULL,
	inserttimestamp DATETIME NULL,
	topic VARCHAR(50) NULL,
	replytoid INT NULL,
	userid INT NULL,
	username VARCHAR(50) NULL,
	ip VARCHAR(15) NULL,
	message TEXT NULL,
	views INT NULL,
	replies INT NULL,
	attachmentid INT NULL,
	PRIMARY KEY (postid)
);

CREATE TABLE cm_forumcategory (
	categoryid INT NOT NULL AUTO_INCREMENT,
	siteid INT NULL,
	updatetimestamp DATETIME NULL,
	inserttimestamp DATETIME NULL,
	boardid INT NULL,
	name VARCHAR(50) NULL,
	sortorder INT NULL,
	PRIMARY KEY (categoryid)
);


CREATE TABLE cm_forumemoticon (
id INT NOT NULL AUTO_INCREMENT,
textversion VARCHAR(50) NULL,
imagename VARCHAR(254) NULL,
updatetimestamp DATETIME NOT NULL,
inserttimestamp TIMESTAMP NOT NULL,
PRIMARY KEY (id)
);


CREATE TABLE cm_forumtag (
id INT NOT NULL AUTO_INCREMENT,
forumcodestart VARCHAR(50) NULL,
forumcodeend VARCHAR(50) NULL,
htmlcodestart VARCHAR(50) NULL,
htmlcodeend VARCHAR(50) NULL,
updatetimestamp DATETIME NOT NULL,
inserttimestamp TIMESTAMP NOT NULL,
PRIMARY KEY (id)
);

CREATE TABLE cm_forumuser (
id INT NOT NULL AUTO_INCREMENT,
updatetimestamp DATETIME NOT NULL,
inserttimestamp TIMESTAMP NOT NULL,
userid INT NULL,
location VARCHAR(50) NULL,
interest VARCHAR(100) NULL,
homepage VARCHAR(100) NULL,
msn VARCHAR(50) NULL,
aimname VARCHAR(50) NULL,
icqnumber VARCHAR(50) NULL,
signature TEXT NULL,
gender INT NULL,
timezone INT NULL,
avartar VARCHAR (254),
yahoomessenger VARCHAR (50),
PRIMARY KEY (id)
);


CREATE TABLE cm_forumfile (
id INT NOT NULL AUTO_INCREMENT,
updatetimestamp DATETIME NULL,
inserttimestamp TIMESTAMP NULL,
origfilename VARCHAR (254),
forumfilename VARCHAR (254),
filesize INT NULL,
dlcount INT NULL,
contenttype VARCHAR (50),
PRIMARY KEY (id)
);

/*
 *  Table data
 */

INSERT INTO cuyahoga_moduletype (name, assemblyname, classname, path, editpath, inserttimestamp, updatetimestamp) 
VALUES ('Forum', 'Cuyahoga.Modules.Forum', 'Cuyahoga.Modules.Forum.ForumModule', 'Modules/Forum/UserForum.ascx', 'Modules/Forum/AdminForum.aspx', '2005-02-11 14:36:28.324', '2004-02-11 14:36:28.324');

SELECT @moduletypeid := last_insert_id();

INSERT INTO cuyahoga_version (assembly, major, minor, patch) VALUES ('Cuyahoga.Modules.Forum', 1, 0, 0);

INSERT INTO cm_forumemoticon (textversion, imagename,updatetimestamp,inserttimestamp)
VALUES ('8)', 'cool.gif', '2005-02-11 14:36:28.324', '2004-02-11 14:36:28.324');

INSERT INTO cm_forumemoticon (textversion, imagename,updatetimestamp,inserttimestamp)
VALUES (':(', 'angry.gif', '2005-02-11 14:36:28.324', '2004-02-11 14:36:28.324');

INSERT INTO cm_forumemoticon (textversion, imagename,updatetimestamp,inserttimestamp)
VALUES (':)', 'happy.gif', '2005-02-11 14:36:28.324', '2004-02-11 14:36:28.324');

INSERT INTO cm_forumemoticon (textversion, imagename,updatetimestamp,inserttimestamp)
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
