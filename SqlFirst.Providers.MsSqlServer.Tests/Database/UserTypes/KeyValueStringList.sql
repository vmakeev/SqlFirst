USE SqlFirstTestDb
GO

CREATE TYPE dbo.KeyValueStringList AS TABLE
(
[Key] nvarchar(50) not null,
[Value] nvarchar(50) not null,
PRIMARY KEY([Key])
)
