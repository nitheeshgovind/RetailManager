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
    [LastModified]
	FROM
	[dbo].[Product]
	ORDER BY
	[ProductName]

END
