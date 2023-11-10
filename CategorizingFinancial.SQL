/****** Object:  Table [dbo].[Fatura]    Script Date: 10/11/2023 10:50:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE #FinancialCategories (
	[Category] [varchar](20) PRIMARY KEY NOT NULL,
	[StartMarketValue] [decimal] NOT NULL,
	[EndMarketValue] [decimal] NOT NULL)
GO

CREATE TABLE #FinancialInstruments (
	[Type] [varchar](20) NOT NULL,
	[MarketValue] [decimal] NOT NULL)
GO

INSERT INTO #FinancialCategories VALUES ('Low Value',0,1000000);
INSERT INTO #FinancialCategories VALUES ('Medium Value', 1000001, 5000000);
INSERT INTO #FinancialCategories VALUES ('High Value', 5000001, 9999999999);

INSERT INTO #FinancialInstruments VALUES ('Stock', -1);
INSERT INTO #FinancialInstruments VALUES ('Stock', 800000);
INSERT INTO #FinancialInstruments VALUES ('Bond', 1500000);
INSERT INTO #FinancialInstruments VALUES ('Stock', 6000000);
INSERT INTO #FinancialInstruments VALUES ('Stock', 300000);


SELECT CASE WHEN cat.Category IS NULL THEN 'Unknow' ELSE cat.Category END FROM #FinancialInstruments ins LEFT JOIN
#FinancialCategories cat ON ins.MarketValue >= cat.StartMarketValue AND ins.MarketValue <= cat.EndMarketValue;

DROP TABLE #FinancialCategories
DROP TABLE #FinancialInstruments