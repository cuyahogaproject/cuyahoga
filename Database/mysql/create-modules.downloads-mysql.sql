

CREATE TABLE cm_file(
fileid INT NOT NULL AUTO_INCREMENT,
sectionid INT NOT NULL,
publisherid INT NOT NULL,
filepath VARCHAR(255) NOT NULL,
title VARCHAR(100),
filesize INT NOT NULL,
nrofdownloads INT NOT NULL,
contenttype VARCHAR(50) NOT NULL,
datepublished DATETIME NOT NULL,
inserttimestamp TIMESTAMP NOT NULL,
updatetimestamp DATETIME NOT NULL,
FOREIGN KEY (sectionid) REFERENCES cuyahoga_section (sectionid),
FOREIGN KEY (publisherid) REFERENCES cuyahoga_user (userid),
PRIMARY KEY (fileid));


CREATE TABLE cm_filerole(
fileroleid INT NOT NULL AUTO_INCREMENT,
fileid INT NOT NULL,
roleid INT NOT NULL,
FOREIGN KEY (fileid) REFERENCES cm_file (fileid),
FOREIGN KEY (roleid) REFERENCES cuyahoga_role (roleid),
PRIMARY KEY (fileroleid));

