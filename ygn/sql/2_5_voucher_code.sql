USE [ygndb]
GO

/****** Object:  Table [dbo].[voucher_code]    Script Date: 03/03/2014 19:34:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[voucher_code](
	[voucher_code_id] [nvarchar](20) NOT NULL,
	[voucher_code_name] [nvarchar](10) NOT NULL,
	[voucher_type] [tinyint] NOT NULL,
	[item_type] [tinyint] NOT NULL,
	[modified_on] [datetime] NOT NULL,
 CONSTRAINT [PK_voucher_code] PRIMARY KEY CLUSTERED 
(
	[voucher_code_id] ASC,
	[voucher_code_name] ASC,
	[voucher_type] ASC,
	[item_type] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'represent unique month+year character as id. Jan-2010 Feb-2010' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'voucher_code', @level2type=N'COLUMN',@level2name=N'voucher_code_id'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'voucher code for each month eg: A, AA, BC, ER, ...' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'voucher_code', @level2type=N'COLUMN',@level2name=N'voucher_code_name'
GO


