SELECT TOP 15 * FROM dbo.BulkCopyTest ORDER BY BulkCopyTestId DESC


CREATE TABLE dbo.NumberCollection
(
	NumberCollectionId INT IDENTITY(1,1) NOT NULL,
	Number1 INT NOT NULL,
	Number2 INT NULL,
	Number3 INT NULL,
	Number4 INT NULL,
	Number5 INT NULL,
	Number6 INT NULL,
	Number7 INT NULL,
	Number8 INT NULL,
	Number9 INT NULL,
	Number10 INT NULL,
	CreatedOnUtc DATETIME2(7) NOT NULL CONSTRAINT [DF_dbo.NumberCollection_CreatedOnUtc] DEFAULT (GETUTCDATE()),
	CONSTRAINT [PK_dbo.NumberCollection_NumberCollectionId] PRIMARY KEY ([NumberCollectionId])
)

SELECT TOP 0 * FROM dbo.NumberCollection


SET FMTONLY ON; SELECT * FROM dbo.NumberCollection; SET FMTONLY OFF;

CREATE TABLE dbo.TypesTable
(
	 PrimaryKeyInt INT IDENTITY(1,1) NOT NULL PRIMARY KEY
	,DecimalCol DECIMAL(8, 2) NOT NULL
	,DateCol DATE NOT NULL
	,DateTime2Col DATETIME2(7) NOT NULL
	,AnsiString VARCHAR(255) NOT NULL
	,UnicodeString NVARCHAR(255) NOT NULL
)

CREATE TABLE dbo.TypesTable2
(
	 PrimaryKeyGuid UNIQUEIDENTIFIER NOT NULL PRIMARY KEY
	,BoolCol BIT NOT NULL
	,IntCol INT NOT NULL
	,DecimalCol DECIMAL(8, 2) NOT NULL
	,DateCol DATE NOT NULL
	,DateTime2Col DATETIME2(7) NOT NULL
	,AnsiString VARCHAR(255) NOT NULL
	,UnicodeString NVARCHAR(255) NOT NULL
)

SET FMTONLY ON; SELECT * FROM dbo.TypesTable; SET FMTONLY OFF;
SELECT * FROM dbo.TypesTable

SELECT * FROM sys.types
