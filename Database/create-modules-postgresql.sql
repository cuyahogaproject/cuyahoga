
CREATE TABLE CM_ArticleCategory(
ArticleCategoryId serial NOT NULL CONSTRAINT UC_CM_ArticleCategory1 UNIQUE CONSTRAINT PK_CM_ArticleCategory1 PRIMARY KEY,
Title varchar(100) NOT NULL,
Summary varchar(255),
Syndicate bool NOT NULL,
InsertTimestamp timestamp DEFAULT current_timestamp NOT NULL,
UpdateTimestamp timestamp DEFAULT current_timestamp NOT NULL);


CREATE TABLE CM_Article(
ArticleId serial NOT NULL CONSTRAINT UC_CM_Article1 UNIQUE CONSTRAINT PK_CM_Article1 PRIMARY KEY,
SectionId int4 NOT NULL,
CreatedBy int4 NOT NULL,
ModifiedBy int4,
ArticleCategoryId int4,
Title varchar(100) NOT NULL,
Summary varchar(255),
Content text NOT NULL,
Syndicate bool NOT NULL,
DateOnline timestamp NOT NULL,
DateOffline timestamp NOT NULL,
InsertTimestamp timestamp DEFAULT current_timestamp NOT NULL,
UpdateTimestamp timestamp DEFAULT current_timestamp NOT NULL,
CONSTRAINT FK_CM_Article_1 FOREIGN KEY (ArticleCategoryId) REFERENCES CM_ArticleCategory (ArticleCategoryId),
CONSTRAINT FK_CM_Article_2 FOREIGN KEY (SectionId) REFERENCES Cuyahoga_Section (SectionId),
CONSTRAINT FK_CM_Article_3 FOREIGN KEY (CreatedBy) REFERENCES Cuyahoga_User (UserId),
CONSTRAINT FK_CM_Article_4 FOREIGN KEY (ModifiedBy) REFERENCES Cuyahoga_User (UserId));


CREATE TABLE CM_StaticHtml(
StaticHtmlId serial NOT NULL CONSTRAINT UC_CM_StaticHtml1 UNIQUE CONSTRAINT PK_CM_StaticHtml1 PRIMARY KEY,
SectionId int4 NOT NULL,
CreatedBy int4 NOT NULL,
ModifiedBy int4,
Title varchar(255),
Content text NOT NULL,
InsertTimestamp timestamp DEFAULT current_timestamp NOT NULL,
UpdateTimestamp timestamp DEFAULT current_timestamp NOT NULL,
CONSTRAINT FK_CM_StaticHtml_1 FOREIGN KEY (SectionId) REFERENCES Cuyahoga_Section (SectionId),
CONSTRAINT FK_CM_StaticHtml_2 FOREIGN KEY (CreatedBy) REFERENCES Cuyahoga_User (UserId),
CONSTRAINT FK_CM_StaticHtml_3 FOREIGN KEY (ModifiedBy) REFERENCES Cuyahoga_User (UserId));


CREATE TABLE CM_ArticleComment(
CommentId serial NOT NULL CONSTRAINT UC_CM_ArticleComment1 UNIQUE CONSTRAINT PK_CM_ArticleComment1 PRIMARY KEY,
ArticleId int4 NOT NULL,
UserId int4,
Name varchar(100),
Website varchar(100),
CommentText varchar(2000) NOT NULL,
UserIP varchar(15),
InsertTimestamp timestamp DEFAULT current_timestamp NOT NULL,
UpdateTimestamp timestamp DEFAULT current_timestamp NOT NULL,
CONSTRAINT FK_CM_ArticleComment_1 FOREIGN KEY (ArticleId) REFERENCES CM_Article (ArticleId),
CONSTRAINT FK_CM_ArticleComment_2 FOREIGN KEY (UserId) REFERENCES Cuyahoga_User (UserId));

