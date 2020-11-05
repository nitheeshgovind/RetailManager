CREATE PROCEDURE [dbo].[spProductGetById]
	@Id int
AS
BEGIN

	SET NOCOUNT ON

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
	WHERE
	[Id] = @Id

END
