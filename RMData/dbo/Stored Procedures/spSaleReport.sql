CREATE PROCEDURE [dbo].[spSaleReport]
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT
		[S].[SaleDate],
		[S].[SubTotal],
		[S].[Tax],
		[S].[Total],
		[U].[FirstName],
		[U].[LastName],
		[U].[EmailAddress]
	FROM
		[dbo].[Sale] AS [S]
	INNER JOIN
		[dbo].[User] AS [U]
	ON
		[S].[CashierId] = [U].[Id]

END
