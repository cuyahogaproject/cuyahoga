

CREATE TABLE cm_articlecategory(
articlecategoryid INT NOT NULL AUTO_INCREMENT,
title VARCHAR(100) NOT NULL,
summary VARCHAR(255),
syndicate TINYINT NOT NULL,
inserttimestamp TIMESTAMP NOT NULL,
updatetimestamp DATETIME NOT NULL,
PRIMARY KEY (articlecategoryid));


CREATE TABLE cm_feed(
feedid INT NOT NULL AUTO_INCREMENT,
sectionid INT NOT NULL,
url VARCHAR(255) NOT NULL,
title VARCHAR(100) NOT NULL,
pubdate DATETIME NOT NULL,
numberofitems INT NOT NULL,
inserttimestamp TIMESTAMP NOT NULL,
updatetimestamp DATETIME NOT NULL,
FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid),
PRIMARY KEY (feedid));


CREATE TABLE cm_feeditem(
feeditemid INT NOT NULL AUTO_INCREMENT,
feedid INT NOT NULL,
url VARCHAR(255) NOT NULL,
title VARCHAR(100) NOT NULL,
content TEXT,
pubdate DATETIME NOT NULL,
author VARCHAR(100),
FOREIGN KEY (feedid) REFERENCES cm_feed (feedid),
PRIMARY KEY (feeditemid));


CREATE TABLE cm_article(
articleid INT NOT NULL AUTO_INCREMENT,
sectionid INT NOT NULL,
createdby INT NOT NULL,
modifiedby INT,
articlecategoryid INT,
title VARCHAR(100) NOT NULL,
summary VARCHAR(255),
content TEXT NOT NULL,
syndicate TINYINT NOT NULL,
dateonline DATETIME NOT NULL,
dateoffline DATETIME NOT NULL,
inserttimestamp TIMESTAMP NOT NULL,
updatetimestamp DATETIME NOT NULL,
FOREIGN KEY (articlecategoryid) REFERENCES cm_articlecategory (articlecategoryid),
FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid),
FOREIGN KEY (createdby) REFERENCES cuyahoga_user (userid),
FOREIGN KEY (modifiedby) REFERENCES cuyahoga_user (userid),
PRIMARY KEY (articleid));


CREATE TABLE cm_articlecomment(
commentid INT NOT NULL AUTO_INCREMENT,
articleid INT NOT NULL,
userid INT,
name VARCHAR(100),
website VARCHAR(100),
commenttext TEXT NOT NULL,
userip VARCHAR(15),
inserttimestamp TIMESTAMP NOT NULL,
updatetimestamp DATETIME NOT NULL,
FOREIGN KEY (articleid) REFERENCES cm_article (articleid),
FOREIGN KEY (userid) REFERENCES cuyahoga_user (userid),
PRIMARY KEY (commentid));


CREATE TABLE cm_statichtml(
statichtmlid INT NOT NULL AUTO_INCREMENT,
sectionid INT NOT NULL,
createdby INT NOT NULL,
modifiedby INT,
title VARCHAR(255),
content TEXT NOT NULL,
inserttimestamp TIMESTAMP NOT NULL,
updatetimestamp DATETIME NOT NULL,
FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid),
FOREIGN KEY (createdby) REFERENCES cuyahoga_user (userid),
FOREIGN KEY (modifiedby) REFERENCES cuyahoga_user (userid),
PRIMARY KEY (statichtmlid));

