USE [ygndb]
GO

/****** Object:  Table [dbo].[item_type]    Script Date: 03/03/2014 19:33:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[item_type](
	[item_type_id] [int] NOT NULL,
	[item_type_name] [nvarchar](100) NOT NULL,
	[description] [nvarchar](300) NULL,
	[is_active] [bit] NOT NULL,
	[modified_on] [datetime] NOT NULL,
 CONSTRAINT [PK_item_type] PRIMARY KEY CLUSTERED 
(
	[item_type_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[item_type] ADD  CONSTRAINT [DF_item_type_description]  DEFAULT ('') FOR [description]
GO


