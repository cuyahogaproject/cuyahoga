
ALTER TABLE CM_ArticleComment
DROP CONSTRAINT FK_CM_ArticleComment_1 
go

ALTER TABLE CM_ArticleComment
DROP CONSTRAINT FK_CM_ArticleComment_2 
go

ALTER TABLE CM_Article
DROP CONSTRAINT FK_CM_Article_1 
go

ALTER TABLE CM_Article
DROP CONSTRAINT FK_CM_Article_2 
go

ALTER TABLE CM_Article
DROP CONSTRAINT FK_CM_Article_3 
go

ALTER TABLE CM_Article
DROP CONSTRAINT FK_CM_Article_4 
go

ALTER TABLE CM_StaticHtml
DROP CONSTRAINT FK_CM_StaticHtml_1 
go

ALTER TABLE CM_StaticHtml
DROP CONSTRAINT FK_CM_StaticHtml_2 
go

ALTER TABLE CM_StaticHtml
DROP CONSTRAINT FK_CM_StaticHtml_3 
go


DROP TABLE CM_ArticleComment
go


DROP TABLE CM_Article
go


DROP TABLE CM_ArticleCategory
go


DROP TABLE CM_StaticHtml
go
