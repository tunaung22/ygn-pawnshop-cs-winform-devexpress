USE ygndb;
GO

BEGIN TRANSACTION;
GO

INSERT INTO dbo.voucher_code(
								voucher_code_id,
								voucher_code_name,
								voucher_type,
								item_type,
								modified_on
								)	
       SELECT
				voucher_code_id,
				custom_voucher_code,
				voucher_type,
				item_type,
				modified_on
				
				
       FROM dbo.voucher_code_old;

GO

COMMIT TRANSACTION;
GO