/************************
 Cuyahoga_Site changes
************************/
ALTER TABLE cuyahoga_site ADD COLUMN roleid int4;
ALTER TABLE cuyahoga_site ADD COLUMN defaultplaceholder varchar(100);
ALTER TABLE cuyahoga_site ADD COLUMN webmasteremail varchar(100);

-- defaults
UPDATE cuyahoga_site
	SET	roleid = (SELECT roleid FROM cuyahoga_role WHERE permissionlevel = 1 LIMIT 1) 
	,	WebmasterEmail = 'webmaster@localhost';

ALTER TABLE cuyahoga_site ALTER COLUMN roleid SET NOT NULL;
	
ALTER TABLE cuyahoga_site ALTER COLUMN webmasteremail SET NOT NULL;

ALTER TABLE cuyahoga_site
	ADD CONSTRAINT FK_Cuyahoga_Site_Cuyahoga_Role
		FOREIGN KEY (roleid) REFERENCES cuyahoga_role (roleid);

/************************
 Cuyahoga_User changes
************************/

ALTER TABLE cuyahoga_user ADD COLUMN website varchar(100) NULL;
ALTER TABLE cuyahoga_user ADD COLUMN isactive bool NULL;

UPDATE cuyahoga_user SET isactive = true;
