CREATE PROCEDURE [dbo].[spSaleLookup]
	@CashierId NVARCHAR(128),
	@SaleDate DATETIME2
AS
BEGIN

	SET NOCOUNT ON

	SELECT
	[Id]
	FROm
	[dbo].[Sale]
	WHERE
	[CashierId] = @CashierId
	AND
	[SaleDate] = @SaleDate

END
