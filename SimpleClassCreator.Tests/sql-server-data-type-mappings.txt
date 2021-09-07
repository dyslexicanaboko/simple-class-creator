#https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-data-type-mappings
# Purposely not supported: FILESTREAM, image, ntext, text, timestamp, sql_variant
# You should not be using these types

bigint	long	BigInt	GetSqlInt64	Int64	GetInt64
binary	Byte[]	VarBinary	GetSqlBinary	Binary	GetBytes
bit	bool	Bit	GetSqlBoolean	Boolean	GetBoolean
char	String	GetSqlString	AnsiStringFixedLength
date	DateTime	Date	GetSqlDateTime	Date	GetDateTime
datetime	DateTime	DateTime	GetSqlDateTime	DateTime	GetDateTime
datetime2	DateTime	DateTime2	None	DateTime2	GetDateTime
datetimeoffset	DateTimeOffset	DateTimeOffset	none	DateTimeOffset	GetDateTimeOffset
decimal	Decimal	Decimal	GetSqlDecimal	Decimal	GetDecimal
float	Double	Float	GetSqlDouble	Double	GetDouble
int	int	Int	GetSqlInt32	Int32	GetInt32
money	Decimal	Money	GetSqlMoney	Decimal	GetDecimal
nchar	String	NChar	GetSqlString	StringFixedLength	GetString
numeric	Decimal	Decimal	GetSqlDecimal	Decimal	GetDecimal
nvarchar	String	NVarChar	GetSqlString	String	GetString
real	float	Real	GetSqlSingle	Single	GetFloat
rowversion	Byte[]	Timestamp	GetSqlBinary	Binary	GetBytes
smalldatetime	DateTime	DateTime	GetSqlDateTime	DateTime	GetDateTime
smallint	short	SmallInt	GetSqlInt16	Int16	GetInt16
smallmoney	Decimal	SmallMoney	GetSqlMoney	Decimal	GetDecimal
time	TimeSpan	Time	none	Time	GetDateTime
tinyint	Byte	TinyInt	GetSqlByte	Byte	GetByte
uniqueidentifier	Guid	UniqueIdentifier	GetSqlGuid	Guid	GetGuid
varbinary	Byte[]	VarBinary	GetSqlBinary	Binary	GetBytes
varchar	String	VarChar	GetSqlString	AnsiString, String	GetString
xml	Xml	Xml	GetSqlXml	Xml	none
