CREATE PROCEDURE [dbo].[Location_GetAll]
AS
BEGIN
	SELECT 
		CONCAT([CountryName], '-', [CityName]) AS LocationName
	FROM dbo.[Weather]
END
