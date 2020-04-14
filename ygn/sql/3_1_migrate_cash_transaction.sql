USE ygndb;
GO

BEGIN TRANSACTION;
GO

INSERT INTO dbo.cash_transaction(
									transaction_date,
									transaction_type,
									transaction_amount,
									description,
									modified_on

								)	
       SELECT
			transaction_date,
			transaction_type,
			transaction_amount,
			description,
			modified_on	
       FROM dbo.cash_transaction_old;

GO

COMMIT TRANSACTION;
GO