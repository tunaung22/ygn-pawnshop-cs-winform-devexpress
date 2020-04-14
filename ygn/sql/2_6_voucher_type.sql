USE [ygndb]
GO

/****** Object:  Table [dbo].[voucher_type]    Script Date: 03/03/2014 19:34:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[voucher_type](
	[voucher_type_id] [int] NOT NULL,
	[voucher_type_name] [nvarchar](100) NOT NULL,
	[description] [nvarchar](300) NULL,
	[is_active] [bit] NOT NULL,
	[modified_on] [datetime] NOT NULL,
 CONSTRAINT [PK_voucher_type] PRIMARY KEY CLUSTERED 
(
	[voucher_type_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[voucher_type] ADD  CONSTRAINT [DF_voucher_type_description]  DEFAULT ('') FOR [description]
GO


