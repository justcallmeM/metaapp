CREATE TABLE [dbo].[Weather]
(
	[Id] INT NOT NULL IDENTITY(1,1), 
    [CityName] NVARCHAR(100) NOT NULL,
	[CountryName] NVARCHAR(100) NOT NULL,
	[LastUpdated] NVARCHAR(100) NOT NULL,
	[Temperature] BIGINT NOT NULL,
	[TemperatureFeelsLike] BIGINT NOT NULL,
	[WindSpeed] BIGINT NOT NULL,
	[WindDirection] NVARCHAR(10) NOT NULL,
	[Precipitation] BIGINT NOT NULL,
	[Humidity] INT NOT NULL,
	[CloudCoverage] INT NOT NULL,
	[UltravioletIndex] BIGINT NOT NULL,

	CONSTRAINT PK_Weather_Id PRIMARY KEY (Id)
)
