CREATE TABLE CM_StaticHtml(
StaticHtmlId int identity(1,1) NOT NULL CONSTRAINT PK_CM_StaticHtml1 PRIMARY KEY,
SectionId int NOT NULL,
CreatedBy int NOT NULL,
ModifiedBy int NULL,
Title varchar(255) NULL,
Content text NOT NULL,
InsertTimestamp datetime DEFAULT current_timestamp NOT NULL,
UpdateTimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_CM_StaticHtml1 UNIQUE(StaticHtmlId))
go


CREATE TABLE CM_ArticleCategory(
ArticleCategoryId int identity(1,1) NOT NULL CONSTRAINT PK_CM_ArticleCategory1 PRIMARY KEY,
Title varchar(100) NOT NULL,
Summary varchar(255) NULL,
Syndicate bit NOT NULL,
InsertTimestamp datetime DEFAULT current_timestamp NOT NULL,
UpdateTimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_CM_ArticleCategory1 UNIQUE(ArticleCategoryId))
go


CREATE TABLE CM_Article(
ArticleId int identity(1,1) NOT NULL CONSTRAINT PK_CM_Article1 PRIMARY KEY,
SectionId int NOT NULL,
CreatedBy int NOT NULL,
ModifiedBy int NULL,
ArticleCategoryId int NULL,
Title varchar(100) NOT NULL,
Summary varchar(255) NULL,
Content text NOT NULL,
Syndicate bit NOT NULL,
DateOnline datetime NOT NULL,
DateOffline datetime NOT NULL,
InsertTimestamp datetime DEFAULT current_timestamp NOT NULL,
UpdateTimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_CM_Article1 UNIQUE(ArticleId))
go


CREATE TABLE CM_ArticleComment(
CommentId int identity(1,1) NOT NULL CONSTRAINT PK_CM_ArticleComment1 PRIMARY KEY,
ArticleId int NOT NULL,
UserId int NULL,
Name varchar(100) NULL,
Website varchar(100) NULL,
CommentText varchar(2000) NOT NULL,
UserIP varchar(15) NULL,
InsertTimestamp datetime DEFAULT current_timestamp NOT NULL,
UpdateTimestamp datetime DEFAULT current_timestamp NOT NULL,
CONSTRAINT UC_CM_ArticleComment1 UNIQUE(CommentId))
go




ALTER TABLE CM_StaticHtml
ADD CONSTRAINT FK_CM_StaticHtml_1 
FOREIGN KEY (SectionId) REFERENCES Cuyahoga_Section (SectionId)
go

ALTER TABLE CM_StaticHtml
ADD CONSTRAINT FK_CM_StaticHtml_2 
FOREIGN KEY (CreatedBy) REFERENCES Cuyahoga_User (UserId)
go

ALTER TABLE CM_StaticHtml
ADD CONSTRAINT FK_CM_StaticHtml_3 
FOREIGN KEY (ModifiedBy) REFERENCES Cuyahoga_User (UserId)
go



ALTER TABLE CM_Article
ADD CONSTRAINT FK_CM_Article_1 
FOREIGN KEY (ArticleCategoryId) REFERENCES CM_ArticleCategory (ArticleCategoryId)
go

ALTER TABLE CM_Article
ADD CONSTRAINT FK_CM_Article_2 
FOREIGN KEY (SectionId) REFERENCES Cuyahoga_Section (SectionId)
go

ALTER TABLE CM_Article
ADD CONSTRAINT FK_CM_Article_3 
FOREIGN KEY (CreatedBy) REFERENCES Cuyahoga_User (UserId)
go

ALTER TABLE CM_Article
ADD CONSTRAINT FK_CM_Article_4 
FOREIGN KEY (ModifiedBy) REFERENCES Cuyahoga_User (UserId)
go


ALTER TABLE CM_ArticleComment
ADD CONSTRAINT FK_CM_ArticleComment_1 
FOREIGN KEY (ArticleId) REFERENCES CM_Article (ArticleId)
go

ALTER TABLE CM_ArticleComment
ADD CONSTRAINT FK_CM_ArticleComment_2 
FOREIGN KEY (UserId) REFERENCES Cuyahoga_User (UserId)
go

