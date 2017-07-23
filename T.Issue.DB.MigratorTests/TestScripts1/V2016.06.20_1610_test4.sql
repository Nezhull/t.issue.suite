create table test2 (
	[Id] bigint identity(1,20),
	[{{parameters_test_key_1}}] int NULL,
	[{{parameters_test_key_2}}] Int NOT NULL,
	CONSTRAINT [PK_TEST2] PRIMARY KEY ([Id] ASC),
)
GO