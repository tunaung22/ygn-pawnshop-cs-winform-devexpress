USE [ygndb]
GO

/****** Object:  Table [dbo].[transaction_type]    Script Date: 03/03/2014 19:36:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[transaction_type](
	[transaction_type_id] [int] IDENTITY(1,1) NOT NULL,
	[transaction_type_name] [nvarchar](100) NOT NULL,
	[description] [nvarchar](300) NULL,
	[is_active] [bit] NOT NULL,
	[modified_on] [datetime] NOT NULL,
 CONSTRAINT [PK_transaction_type] PRIMARY KEY CLUSTERED 
(
	[transaction_type_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[transaction_type] ADD  CONSTRAINT [DF_transaction_type_description]  DEFAULT ('') FOR [description]
GO


