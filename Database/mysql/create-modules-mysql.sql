

CREATE TABLE CM_ArticleCategory(
ArticleCategoryId INT NOT NULL AUTO_INCREMENT,
Title VARCHAR(100) NOT NULL,
Summary VARCHAR(255),
Syndicate TINYINT NOT NULL,
InsertTimestamp TIMESTAMP DEFAULT current_timestamp NOT NULL,
UpdateTimestamp DATETIME NOT NULL,
PRIMARY KEY (ArticleCategoryId),
UNIQUE UC_ArticleCategoryId (ArticleCategoryId));


CREATE TABLE CM_Article(
ArticleId INT NOT NULL AUTO_INCREMENT,
SectionId INT NOT NULL,
CreatedBy INT NOT NULL,
ModifiedBy INT,
ArticleCategoryId INT,
Title VARCHAR(100) NOT NULL,
Summary VARCHAR(255),
Content TEXT NOT NULL,
Syndicate TINYINT NOT NULL,
DateOnline DATETIME NOT NULL,
DateOffline DATETIME NOT NULL,
InsertTimestamp TIMESTAMP DEFAULT current_timestamp NOT NULL,
UpdateTimestamp DATETIME NOT NULL,
FOREIGN KEY (ArticleCategoryId) REFERENCES CM_ArticleCategory (ArticleCategoryId),
FOREIGN KEY (SectionId) REFERENCES Cuyahoga_Section (SectionId),
FOREIGN KEY (CreatedBy) REFERENCES Cuyahoga_User (UserId),
FOREIGN KEY (ModifiedBy) REFERENCES Cuyahoga_User (UserId),
PRIMARY KEY (ArticleId),
UNIQUE UC_ArticleId (ArticleId));


CREATE TABLE CM_StaticHtml(
StaticHtmlId INT NOT NULL AUTO_INCREMENT,
SectionId INT NOT NULL,
CreatedBy INT NOT NULL,
ModifiedBy INT,
Title VARCHAR(255),
Content TEXT NOT NULL,
InsertTimestamp TIMESTAMP DEFAULT current_timestamp NOT NULL,
UpdateTimestamp DATETIME NOT NULL,
FOREIGN KEY (SectionId) REFERENCES Cuyahoga_Section (SectionId),
FOREIGN KEY (CreatedBy) REFERENCES Cuyahoga_User (UserId),
FOREIGN KEY (ModifiedBy) REFERENCES Cuyahoga_User (UserId),
PRIMARY KEY (StaticHtmlId),
UNIQUE UC_StaticHtmlId (StaticHtmlId));


CREATE TABLE CM_ArticleComment(
CommentId INT NOT NULL AUTO_INCREMENT,
ArticleId INT NOT NULL,
UserId INT,
Name VARCHAR(100),
Website VARCHAR(100),
CommentText VARCHAR(2000) NOT NULL,
UserIP VARCHAR(15),
InsertTimestamp TIMESTAMP DEFAULT current_timestamp NOT NULL,
UpdateTimestamp DATETIME NOT NULL,
FOREIGN KEY (ArticleId) REFERENCES CM_Article (ArticleId),
FOREIGN KEY (UserId) REFERENCES Cuyahoga_User (UserId),
PRIMARY KEY (CommentId),
UNIQUE UC_CommentId (CommentId));

