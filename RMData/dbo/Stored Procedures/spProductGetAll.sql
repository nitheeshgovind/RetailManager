CREATE PROCEDURE [dbo].[spProductGetAll]	
AS
BEGIN

	SELECT
	[Id], 
    [ProductName], 
    [Description], 
	[RetailPrice], 
    [QuantityInStock],
    [CreateDate], 
    [LastModified],
	[IsTaxable]
	FROM
	[dbo].[Product]
	ORDER BY
	[ProductName]

END
