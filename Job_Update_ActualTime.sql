USE [TaskDB]
GO

/****** Object:  Trigger [dbo].[Job_Update_ActualTime]    Script Date: 22.04.2021 1:47:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[Job_Update_ActualTime]
ON [dbo].[Job]
AFTER UPDATE
AS


DECLARE @SearchId int
DECLARE @SearchParentId int
SET @SearchId = (SELECT TOP 1 [Id] FROM DELETED)
SET @SearchParentId = (SELECT TOP 1 [ParentId] FROM deleted)

UPDATE Job
SET ActualTimeSum = ActualTimeMin WHERE Job.Id = @SearchId

CREATE TABLE #JobWithId2
(
Id INT,
ParentId INT,
ActualTimeSum INT
)
INSERT INTO #JobWithId2 SELECT Job.Id, Job.ParentId, Job.ActualTimeSum FROM  Job WHERE Job.ParentId = @SearchParentId
DECLARE @SumOfTime int
SET @SumOfTime = (SELECT SUM(ActualTimeSum) FROM #JobWithId2)
UPDATE Job
SET ActualTimeSum = @SumOfTime + ActualTimeMin WHERE Job.Id = @SearchParentId;
GO

ALTER TABLE [dbo].[Job] ENABLE TRIGGER [Job_Update_ActualTime]
GO

