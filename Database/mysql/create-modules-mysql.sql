

CREATE TABLE cm_articlecategory(
articlecategoryid INT NOT NULL AUTO_INCREMENT,
title VARCHAR(100) NOT NULL,
summary VARCHAR(255),
syndicate TINYINT NOT NULL,
inserttimestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP()  NOT NULL,
updatetimestamp DATETIME  NOT NULL,
PRIMARY KEY (articlecategoryid),
UNIQUE UC_articlecategoryid (articlecategoryid));


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
inserttimestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP()  NOT NULL,
updatetimestamp DATETIME  NOT NULL,
FOREIGN KEY (articlecategoryid) REFERENCES cm_articlecategory (articlecategoryid),
FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid),
FOREIGN KEY (createdby) REFERENCES cuyahoga_user (userid),
FOREIGN KEY (modifiedby) REFERENCES cuyahoga_user (userid),
PRIMARY KEY (articleid),
UNIQUE UC_articleid (articleid));


CREATE TABLE cm_articlecomment(
commentid INT NOT NULL AUTO_INCREMENT,
articleid INT NOT NULL,
userid INT,
name VARCHAR(100),
website VARCHAR(100),
commenttext VARCHAR(2000) NOT NULL,
userip VARCHAR(15),
inserttimestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP()  NOT NULL,
updatetimestamp DATETIME  NOT NULL,
FOREIGN KEY (articleid) REFERENCES cm_article (articleid),
FOREIGN KEY (userid) REFERENCES cuyahoga_user (userid),
PRIMARY KEY (commentid),
UNIQUE UC_commentid (commentid));


CREATE TABLE cm_statichtml(
statichtmlid INT NOT NULL AUTO_INCREMENT,
sectionid INT NOT NULL,
createdby INT NOT NULL,
modifiedby INT,
title VARCHAR(255),
content TEXT NOT NULL,
inserttimestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP()  NOT NULL,
updatetimestamp DATETIME  NOT NULL,
FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid),
FOREIGN KEY (createdby) REFERENCES cuyahoga_user (userid),
FOREIGN KEY (modifiedby) REFERENCES cuyahoga_user (userid),
PRIMARY KEY (statichtmlid),
UNIQUE UC_statichtmlid (statichtmlid));

