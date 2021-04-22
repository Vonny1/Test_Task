USE [TaskDB]
GO

/****** Object:  Table [dbo].[Job]    Script Date: 22.04.2021 1:51:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Job](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ParentId] [int] NULL,
	[Name] [text] NULL,
	[Description] [text] NULL,
	[PlannedTimeMin] [int] NULL,
	[ActualTimeMin] [int] NULL,
	[Asignee] [text] NULL,
	[RegDate] [date] NULL,
	[CompleteDate] [date] NULL,
	[StatusId] [int] NULL,
	[PlannedTimeSum] [int] NULL,
	[ActualTimeSum] [int] NULL,
 CONSTRAINT [PK_Job] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Job]  WITH CHECK ADD  CONSTRAINT [FK_Job_Job] FOREIGN KEY([ParentId])
REFERENCES [dbo].[Job] ([Id])
GO

ALTER TABLE [dbo].[Job] CHECK CONSTRAINT [FK_Job_Job]
GO

ALTER TABLE [dbo].[Job]  WITH CHECK ADD  CONSTRAINT [FK_Job_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([Id])
GO

ALTER TABLE [dbo].[Job] CHECK CONSTRAINT [FK_Job_Status]
GO

