
CREATE TABLE cm_articlecategory(
articlecategoryid serial NOT NULL CONSTRAINT UC_cm_articlecategory1 UNIQUE CONSTRAINT PK_cm_articlecategory1 PRIMARY KEY,
title varchar(100) NOT NULL,
summary varchar(255),
syndicate bool NOT NULL,
inserttimestamp timestamp DEFAULT current_timestamp NOT NULL,
updatetimestamp timestamp DEFAULT current_timestamp NOT NULL);


CREATE TABLE cm_article(
articleid serial NOT NULL CONSTRAINT UC_cm_article1 UNIQUE CONSTRAINT PK_cm_article1 PRIMARY KEY,
sectionid int4 NOT NULL,
createdby int4 NOT NULL,
modifiedby int4,
articlecategoryid int4,
title varchar(100) NOT NULL,
summary varchar(255),
content text NOT NULL,
syndicate bool NOT NULL,
dateonline timestamp NOT NULL,
dateoffline timestamp NOT NULL,
inserttimestamp timestamp DEFAULT current_timestamp NOT NULL,
updatetimestamp timestamp DEFAULT current_timestamp NOT NULL,
CONSTRAINT FK_cm_article_1 FOREIGN KEY (articlecategoryid) REFERENCES cm_articlecategory (articlecategoryid),
CONSTRAINT FK_cm_article_2 FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid),
CONSTRAINT FK_cm_article_3 FOREIGN KEY (createdby) REFERENCES cuyahoga_user (userid),
CONSTRAINT FK_cm_article_4 FOREIGN KEY (modifiedby) REFERENCES cuyahoga_user (userid));


CREATE TABLE cm_articlecomment(
commentid serial NOT NULL CONSTRAINT UC_cm_articlecomment1 UNIQUE CONSTRAINT PK_cm_articlecomment1 PRIMARY KEY,
articleid int4 NOT NULL,
userid int4,
name varchar(100),
website varchar(100),
commenttext varchar(2000) NOT NULL,
userip varchar(15),
inserttimestamp timestamp DEFAULT current_timestamp NOT NULL,
updatetimestamp timestamp DEFAULT current_timestamp NOT NULL,
CONSTRAINT FK_cm_articlecomment_1 FOREIGN KEY (articleid) REFERENCES cm_article (articleid),
CONSTRAINT FK_cm_articlecomment_2 FOREIGN KEY (userid) REFERENCES cuyahoga_user (userid));


CREATE TABLE cm_statichtml(
statichtmlid serial NOT NULL CONSTRAINT UC_cm_statichtml1 UNIQUE CONSTRAINT PK_cm_statichtml1 PRIMARY KEY,
sectionid int4 NOT NULL,
createdby int4 NOT NULL,
modifiedby int4,
title varchar(255),
content text NOT NULL,
inserttimestamp timestamp DEFAULT current_timestamp NOT NULL,
updatetimestamp timestamp DEFAULT current_timestamp NOT NULL,
CONSTRAINT FK_cm_statichtml_1 FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid),
CONSTRAINT FK_cm_statichtml_2 FOREIGN KEY (createdby) REFERENCES cuyahoga_user (userid),
CONSTRAINT FK_cm_statichtml_3 FOREIGN KEY (modifiedby) REFERENCES cuyahoga_user (userid));

