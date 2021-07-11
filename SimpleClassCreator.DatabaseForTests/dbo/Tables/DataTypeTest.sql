CREATE TABLE [dbo].[DataTypeTest]
(
	[DataTypeTestId] INT IDENTITY(1,1) NOT NULL,
	[Short] SMALLINT NOT NULL,
	[Int] INT NOT NULL,
	[Long] BIGINT NOT NULL,
	CONSTRAINT [PK_dbo.DataTypeTest_DataTypeTestId] PRIMARY KEY ([DataTypeTestId])
)
