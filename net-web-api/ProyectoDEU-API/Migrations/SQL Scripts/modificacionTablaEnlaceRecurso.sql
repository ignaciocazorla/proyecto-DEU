USE [Proyecto-DEU]
GO

ALTER TABLE [dbo].[EnlaceRecurso] DROP CONSTRAINT [FK_EnlaceRecurso_Recurso]
GO

/****** Object:  Table [dbo].[EnlaceRecurso]    Script Date: 3/8/2022 12:59:07 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EnlaceRecurso]') AND type in (N'U'))
DROP TABLE [dbo].[EnlaceRecurso]
GO

/****** Object:  Table [dbo].[EnlaceRecurso]    Script Date: 3/8/2022 12:59:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EnlaceRecurso](
	[Id] [uniqueidentifier] NOT NULL,
	[IdRecurso] [uniqueidentifier] NOT NULL,
	[Url] [nvarchar](max) NOT NULL,
	[Nombre] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_EnlaceRecurso] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[EnlaceRecurso]  WITH CHECK ADD  CONSTRAINT [FK_EnlaceRecurso_Recurso] FOREIGN KEY([IdRecurso])
REFERENCES [dbo].[Recurso] ([Id])
GO

ALTER TABLE [dbo].[EnlaceRecurso] CHECK CONSTRAINT [FK_EnlaceRecurso_Recurso]
GO


