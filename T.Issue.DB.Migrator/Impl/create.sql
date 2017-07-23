
CREATE TABLE [${schema.version.table}] (
	[Type] [char](1) NOT NULL,
	[Version] [nvarchar](255) NOT NULL,
	[Script] [nvarchar](max) NOT NULL,
	[Executed] [datetime] NOT NULL,
	[Hash] [char](32) NOT NULL,
	CONSTRAINT [PK_Schema_Version] PRIMARY KEY CLUSTERED ([Version] ASC)
)
