USE [ygndb]
GO

/****** Object:  Table [dbo].[gold_transaction]    Script Date: 03/03/2014 19:33:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[gold_transaction](
	[gold_transaction_id] [int] IDENTITY(1,1) NOT NULL,
	[transaction_date] [date] NOT NULL,
	[transaction_type] [tinyint] NOT NULL,
	[customer_name] [nvarchar](50) NULL,
	[customer_nrc] [nvarchar](50) NULL,
	[customer_address] [nvarchar](50) NULL,
	[item_name] [nvarchar](50) NOT NULL,
	[item_weight] [nvarchar](50) NULL,
	[en_amount] [money] NOT NULL,
	[description] [nvarchar](300) NULL,
	[modified_on] [datetime] NOT NULL,
 CONSTRAINT [PK_gold_transaction_1] PRIMARY KEY CLUSTERED 
(
	[gold_transaction_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


