
CREATE TABLE cm_file(
fileid serial NOT NULL CONSTRAINT PK_cm_file1 PRIMARY KEY,
sectionid int4 NOT NULL,
publisherid int4 NOT NULL,
filepath varchar(255) NOT NULL,
title varchar(100),
filesize int4 NOT NULL,
nrofdownloads int4 NOT NULL,
contenttype varchar(50) NOT NULL,
datepublished timestamp NOT NULL,
inserttimestamp timestamp DEFAULT current_timestamp NOT NULL,
updatetimestamp timestamp NOT NULL,
CONSTRAINT FK_cm_file_1 FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid),
CONSTRAINT FK_cm_file_2 FOREIGN KEY (publisherid) REFERENCES cuyahoga_user (userid));


CREATE TABLE cm_filerole(
fileroleid serial NOT NULL CONSTRAINT PK_cm_filerole1 PRIMARY KEY,
fileid int4 NOT NULL,
roleid int4 NOT NULL,
CONSTRAINT FK_cm_filerole_1 FOREIGN KEY (fileid) REFERENCES cm_file (fileid),
CONSTRAINT FK_cm_filerole_2 FOREIGN KEY (roleid) REFERENCES cuyahoga_role (roleid));

