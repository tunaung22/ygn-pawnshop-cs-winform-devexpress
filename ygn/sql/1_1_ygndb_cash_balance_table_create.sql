USE [ygndb]
GO

/****** Object:  Table [dbo].[cash_balance]    Script Date: 06/01/2014 09:49:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[cash_balance](
	[cash_balance_id] [int] IDENTITY(1,1) NOT NULL,
	[cash_balance_date] [date] NOT NULL,
	[opening_balance] [money] NOT NULL,
	[closing_balance] [money] NOT NULL,
	[description] [nvarchar](300) NULL,
	[modified_on] [datetime] NOT NULL,
 CONSTRAINT [PK_cash_balance_1] PRIMARY KEY CLUSTERED 
(
	[cash_balance_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


