USE [ygndb]
GO

/****** Object:  Table [dbo].[cash_transaction]    Script Date: 03/03/2014 19:33:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[cash_transaction](
	[cash_transaction_id] [int] IDENTITY(1,1) NOT NULL,
	[transaction_date] [date] NOT NULL,
	[transaction_type] [tinyint] NOT NULL,
	[transaction_amount] [money] NOT NULL,
	[description] [nvarchar](300) NULL,
	[modified_on] [datetime] NOT NULL,
 CONSTRAINT [PK_cash_transaction] PRIMARY KEY CLUSTERED 
(
	[cash_transaction_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[cash_transaction] ADD  CONSTRAINT [DF_cash_transaction_description]  DEFAULT ('') FOR [description]
GO


