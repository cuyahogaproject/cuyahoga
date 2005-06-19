
CREATE TABLE cm_file(
fileid int identity(1,1) NOT NULL CONSTRAINT PK_cm_file1 PRIMARY KEY,
sectionid int NOT NULL,
publisherid int NOT NULL,
filepath varchar(255) NOT NULL,
title varchar(100) NULL,
filesize int NOT NULL,
nrofdownloads int NOT NULL,
contenttype varchar(50) NOT NULL,
datepublished datetime NOT NULL,
inserttimestamp datetime DEFAULT current_timestamp NOT NULL,
updatetimestamp datetime NOT NULL)
go


CREATE TABLE cm_filerole(
fileroleid int identity(1,1) NOT NULL CONSTRAINT PK_cm_filerole1 PRIMARY KEY,
fileid int NOT NULL,
roleid int NOT NULL)
go




ALTER TABLE cm_file
ADD CONSTRAINT FK_cm_file_1 
FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid)
go

ALTER TABLE cm_file
ADD CONSTRAINT FK_cm_file_2 
FOREIGN KEY (publisherid) REFERENCES cuyahoga_user (userid)
go


ALTER TABLE cm_filerole
ADD CONSTRAINT FK_cm_filerole_1 
FOREIGN KEY (fileid) REFERENCES cm_file (fileid)
go

ALTER TABLE cm_filerole
ADD CONSTRAINT FK_cm_filerole_2 
FOREIGN KEY (roleid) REFERENCES cuyahoga_role (roleid)
go

