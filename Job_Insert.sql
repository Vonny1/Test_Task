USE [TaskDB]
GO

/****** Object:  Trigger [dbo].[Job_Insert]    Script Date: 22.04.2021 1:47:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[Job_Insert]
ON [dbo].[Job]
AFTER INSERT 
AS


DECLARE @SearchId int
DECLARE @SearchParentId int
SET @SearchId = (SELECT TOP 1 [Id] FROM INSERTED)
SET @SearchParentId = (SELECT TOP 1 [ParentId] FROM INSERTED)

UPDATE Job
SET PlannedTimeSum = PlannedTimeMin WHERE Job.Id = @SearchId

CREATE TABLE #JobWithId
(
Id INT,
ParentId INT,
PlannedTimeSum INT,
)
INSERT INTO #JobWithId SELECT Job.Id, Job.ParentId, Job.PlannedTimeSum FROM  Job WHERE Job.ParentId = @SearchParentId
DECLARE @SumOfTime int
SET @SumOfTime = (SELECT SUM(PlannedTimeSum) FROM #JobWithId)
UPDATE Job
SET PlannedTimeSum = @SumOfTime + PlannedTimeMin WHERE Job.Id = @SearchParentId;
GO

ALTER TABLE [dbo].[Job] ENABLE TRIGGER [Job_Insert]
GO

