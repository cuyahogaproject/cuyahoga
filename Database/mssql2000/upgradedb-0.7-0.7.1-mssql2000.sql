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

ALTER TABLE cm_feed
ADD CONSTRAINT FK_cm_feed_1 
FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid)
go


ALTER TABLE cm_feeditem
ADD CONSTRAINT FK_cm_feeditem_1 
FOREIGN KEY (feedid) REFERENCES cm_feed (feedid)
go