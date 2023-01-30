CREATE PROCEDURE [dbo].[Weather_GetByCityName]
	@CityNames NVARCHAR(500)
AS
BEGIN
	SELECT 
		[CityName]
		,[CountryName]
		,[LastUpdated]
		,[Temperature]
		,[TemperatureFeelsLike]
		,[WindSpeed]
		,[WindDirection]
		,[Precipitation]
		,[Humidity]
		,[CloudCoverage]
		,[UltravioletIndex]
	FROM dbo.[Weather]
	WHERE 
		[CityName] IN ( SELECT value FROM STRING_SPLIT(@CityNames, ',') )
END
