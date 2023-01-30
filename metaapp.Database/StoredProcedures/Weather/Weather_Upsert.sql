CREATE PROCEDURE [dbo].[Weather_Upsert]
    @CityName NVARCHAR(100) 
	,@CountryName NVARCHAR(100)
	,@LastUpdated NVARCHAR(100)
	,@Temperature BIGINT
	,@TemperatureFeelsLike BIGINT
	,@WindSpeed BIGINT
	,@WindDirection NVARCHAR(10)
	,@Precipitation BIGINT
	,@Humidity INT
	,@CloudCoverage INT
	,@UltravioletIndex BIGINT
AS
BEGIN
	IF NOT EXISTS ( SELECT 1 FROM dbo.[Weather] WHERE CityName = @CityName )
		INSERT INTO dbo.[Weather]
			(
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
			)
		VALUES 
			(
				@CityName
				,@CountryName
				,@LastUpdated
				,@Temperature
				,@TemperatureFeelsLike
				,@WindSpeed
				,@WindDirection
				,@Precipitation
				,@Humidity
				,@CloudCoverage
				,@UltravioletIndex
			)
	ELSE
		UPDATE dbo.[Weather]
		SET 
			[CityName] = @CityName
			,[CountryName] = @CountryName
			,[LastUpdated] = @LastUpdated
			,[Temperature] = @Temperature
			,[TemperatureFeelsLike] = @TemperatureFeelsLike
			,[WindSpeed] = @WindSpeed
			,[WindDirection] = @WindDirection
			,[Precipitation] = @Precipitation
			,[Humidity] = @Humidity
			,[CloudCoverage] = @CloudCoverage
			,[UltravioletIndex] = @UltravioletIndex
		WHERE 
			[CityName] = @CityName
END