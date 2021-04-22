USE [TaskDB]
GO

/****** Object:  Trigger [dbo].[Job_Delete]    Script Date: 22.04.2021 1:47:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[Job_Delete]
ON [dbo].[Job]
AFTER INSERT 
AS
DECLARE @SearchParentId int
SET @SearchParentId = (SELECT TOP 1 [ParentId] FROM deleted)
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

ALTER TABLE [dbo].[Job] ENABLE TRIGGER [Job_Delete]
GO

