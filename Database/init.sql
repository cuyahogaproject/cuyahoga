INSERT INTO Role (RoleId, Name)
VALUES (1, 'Adminstrator')

INSERT INTO CmsUser(CmsUserId, Username, Password, Email)
VALUES (1, 'admin', 'hyena', 'admin@cuyahoga')

INSERT INTO UserRole (CmsUserId, RoleId)
VALUES (1, 1)
