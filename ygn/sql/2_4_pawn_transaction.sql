USE [ygndb]
GO

/****** Object:  Table [dbo].[pawn_transaction]    Script Date: 03/03/2014 19:33:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[pawn_transaction](
	[invoice_number] [nvarchar](50) NOT NULL,
	[voucher_type] [smallint] NOT NULL,
	[item_type] [smallint] NOT NULL,
	[customer_name] [nvarchar](100) NOT NULL,
	[customer_nrc] [nvarchar](100) NULL,
	[customer_address] [nvarchar](300) NULL,
	[item_name] [nvarchar](300) NOT NULL,
	[item_weight] [nvarchar](50) NOT NULL,
	[en_amount] [money] NOT NULL,
	[description] [nvarchar](300) NULL,
	[pawn_date] [datetime] NOT NULL,
	[receive_date] [datetime] NOT NULL,
	[is_received] [bit] NOT NULL,
	[interest_rate] [numeric](4, 2) NOT NULL,
	[user_name] [nvarchar](30) NOT NULL,
	[modified_on] [datetime] NOT NULL,
 CONSTRAINT [PK_pawn_transaction] PRIMARY KEY CLUSTERED 
(
	[invoice_number] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'voucher number eg: 2013A0001,2013AA0001,....' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'pawn_transaction', @level2type=N'COLUMN',@level2name=N'invoice_number'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'represent voucher_type_id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'pawn_transaction', @level2type=N'COLUMN',@level2name=N'voucher_type'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'represent item_type_id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'pawn_transaction', @level2type=N'COLUMN',@level2name=N'item_type'
GO

ALTER TABLE [dbo].[pawn_transaction] ADD  CONSTRAINT [DF_pawn_transaction_customer_nrc]  DEFAULT ('') FOR [customer_nrc]
GO

ALTER TABLE [dbo].[pawn_transaction] ADD  CONSTRAINT [DF_pawn_transaction_customer_address]  DEFAULT ('') FOR [customer_address]
GO

ALTER TABLE [dbo].[pawn_transaction] ADD  CONSTRAINT [DF_pawn_transaction_description]  DEFAULT ('') FOR [description]
GO

ALTER TABLE [dbo].[pawn_transaction] ADD  CONSTRAINT [DF_pawn_transaction_interest_rate]  DEFAULT ((0.00)) FOR [interest_rate]
GO


