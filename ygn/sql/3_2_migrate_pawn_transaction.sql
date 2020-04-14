USE ygndb;
GO

BEGIN TRANSACTION;
GO

INSERT INTO dbo.pawn_transaction(
								invoice_number,
								voucher_type,
								item_type,
								customer_name,
								customer_nrc,
								customer_address,
								item_name,
								item_weight,
								en_amount,
								description,
								pawn_date,
								receive_date,
								is_received,
								interest_rate,
								user_name,
								modified_on
								)	
       SELECT
				voucher_no,
				voucher_type,
				item_type,
				customer_name,
				customer_nrc,
				customer_address,
				item_name,
				item_weight,
				en_amount,
				description,
				pawn_date,
				receive_date,
				is_received,
				interest_rate,
				user_name,
				modified_on
				
				
       FROM dbo.pawn_transaction_old;

GO

COMMIT TRANSACTION;
GO