
CREATE TABLE cm_articlecategory(
articlecategoryid int identity(1,1) NOT NULL CONSTRAINT PK_cm_articlecategory1 PRIMARY KEY,
title varchar(100) NOT NULL,
summary varchar(255) NULL,
syndicate bit NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go


CREATE TABLE cm_feed(
feedid int identity(1,1) NOT NULL CONSTRAINT PK_cm_feed1 PRIMARY KEY,
sectionid int NOT NULL,
url varchar(255) NOT NULL,
title varchar(100) NOT NULL,
pubdate datetime NOT NULL,
numberofitems int NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go


CREATE TABLE cm_feeditem(
feeditemid int identity(1,1) NOT NULL CONSTRAINT PK_cm_feeditem1 PRIMARY KEY,
feedid int NOT NULL,
url varchar(255) NOT NULL,
title varchar(100) NOT NULL,
content text NULL,
pubdate datetime NOT NULL,
author varchar(100) NULL)
go


CREATE TABLE cm_article(
articleid int identity(1,1) NOT NULL CONSTRAINT PK_cm_article1 PRIMARY KEY,
sectionid int NOT NULL,
createdby int NOT NULL,
modifiedby int NULL,
articlecategoryid int NULL,
title varchar(100) NOT NULL,
summary varchar(255) NULL,
content text NOT NULL,
syndicate bit NOT NULL,
dateonline datetime NOT NULL,
dateoffline datetime NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go


CREATE TABLE cm_articlecomment(
commentid int identity(1,1) NOT NULL CONSTRAINT PK_cm_articlecomment1 PRIMARY KEY,
articleid int NOT NULL,
userid int NULL,
name varchar(100) NULL,
website varchar(100) NULL,
commenttext varchar(2000) NOT NULL,
userip varchar(15) NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go


CREATE TABLE cm_statichtml(
statichtmlid int identity(1,1) NOT NULL CONSTRAINT PK_cm_statichtml1 PRIMARY KEY,
sectionid int NOT NULL,
createdby int NOT NULL,
modifiedby int NULL,
title varchar(255) NULL,
content text NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime DEFAULT current_timestamp NOT NULL)
go





ALTER TABLE cm_feed
ADD CONSTRAINT FK_cm_feed_1 
FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid)
go


ALTER TABLE cm_feeditem
ADD CONSTRAINT FK_cm_feeditem_1 
FOREIGN KEY (feedid) REFERENCES cm_feed (feedid)
go


ALTER TABLE cm_article
ADD CONSTRAINT FK_cm_article_1 
FOREIGN KEY (articlecategoryid) REFERENCES cm_articlecategory (articlecategoryid)
go

ALTER TABLE cm_article
ADD CONSTRAINT FK_cm_article_2 
FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid)
go

ALTER TABLE cm_article
ADD CONSTRAINT FK_cm_article_3 
FOREIGN KEY (createdby) REFERENCES cuyahoga_user (userid)
go

ALTER TABLE cm_article
ADD CONSTRAINT FK_cm_article_4 
FOREIGN KEY (modifiedby) REFERENCES cuyahoga_user (userid)
go


ALTER TABLE cm_articlecomment
ADD CONSTRAINT FK_cm_articlecomment_1 
FOREIGN KEY (articleid) REFERENCES cm_article (articleid)
go

ALTER TABLE cm_articlecomment
ADD CONSTRAINT FK_cm_articlecomment_2 
FOREIGN KEY (userid) REFERENCES cuyahoga_user (userid)
go


ALTER TABLE cm_statichtml
ADD CONSTRAINT FK_cm_statichtml_1 
FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid)
go

ALTER TABLE cm_statichtml
ADD CONSTRAINT FK_cm_statichtml_2 
FOREIGN KEY (createdby) REFERENCES cuyahoga_user (userid)
go

ALTER TABLE cm_statichtml
ADD CONSTRAINT FK_cm_statichtml_3 
FOREIGN KEY (modifiedby) REFERENCES cuyahoga_user (userid)
go

