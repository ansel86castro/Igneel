IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Resources]') AND type in (N'U'))
DROP TABLE [Resources]
GO
CREATE TABLE [Resources] ( 
    [Id] int IDENTITY(1, 1) NOT NULL, 
    [Name] nvarchar(256) NULL, 
    [ResourceTypeId] int NOT NULL, 
    [FolderId] int NOT NULL, 
    [ContentId] int NOT NULL, 
    [CreateUserId] int NOT NULL, 
    [UpdateUserId] int NOT NULL, 
    [LastUpdateDate] datetime NOT NULL, 
    CONSTRAINT [PK_Resources] PRIMARY KEY ([Id])
)
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Folders]') AND type in (N'U'))
DROP TABLE [Folders]
GO
CREATE TABLE [Folders] ( 
    [FolderId] int IDENTITY(1, 1) NOT NULL, 
    [Name] varchar(50) NOT NULL, 
    [LastUpdatedDate] datetime NOT NULL, 
    [CreateUser] int NOT NULL, 
    [UpdateUser] int NOT NULL, 
    CONSTRAINT [PK_Folders] PRIMARY KEY ([FolderId])
)
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[AssetContents]') AND type in (N'U'))
DROP TABLE [AssetContents]
GO
CREATE TABLE [AssetContents] ( 
    [ContentId] int IDENTITY(1, 1) NOT NULL, 
    [Data] varbinary(max) NOT NULL, 
    CONSTRAINT [PK_AssetContents] PRIMARY KEY ([ContentId])
)
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ResourceType]') AND type in (N'U'))
DROP TABLE [ResourceType]
GO
CREATE TABLE [ResourceType] ( 
    [ResourceTypeId] int IDENTITY(1, 1) NOT NULL, 
    [Name] nvarchar(50) NOT NULL, 
    CONSTRAINT [PK_ResourceType] PRIMARY KEY ([ResourceTypeId])
)
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Users]') AND type in (N'U'))
DROP TABLE [Users]
GO
CREATE TABLE [Users] ( 
    [UserId] int IDENTITY(1, 1) NOT NULL, 
    [UserName] nvarchar(50) NOT NULL, 
    [Email] nvarchar(256) NULL, 
    [Password] nvarchar(256) NOT NULL, 
    [LastUpdateTime] datetime NOT NULL, 
    CONSTRAINT [PK_Users] PRIMARY KEY ([UserId])
)
GO
SET IDENTITY_INSERT [ResourceType] ON
GO
INSERT INTO [ResourceType] ([ResourceTypeId], [Name]) VALUES (1, 'TEXTURE')
INSERT INTO [ResourceType] ([ResourceTypeId], [Name]) VALUES (2, 'VISUAL_MESH')
INSERT INTO [ResourceType] ([ResourceTypeId], [Name]) VALUES (3, 'VISUAL_MATERIAL')
INSERT INTO [ResourceType] ([ResourceTypeId], [Name]) VALUES (4, 'FRAME')
INSERT INTO [ResourceType] ([ResourceTypeId], [Name]) VALUES (5, 'ANIMATION')
INSERT INTO [ResourceType] ([ResourceTypeId], [Name]) VALUES (6, 'PHYSIC_TRIANGLE_MESH')
INSERT INTO [ResourceType] ([ResourceTypeId], [Name]) VALUES (7, 'PHYSIC_HEIGHTFIELD')
INSERT INTO [ResourceType] ([ResourceTypeId], [Name]) VALUES (8, 'PHYSIC_ACTOR')
INSERT INTO [ResourceType] ([ResourceTypeId], [Name]) VALUES (9, 'PHYSIC_MATERIAL')
INSERT INTO [ResourceType] ([ResourceTypeId], [Name]) VALUES (10, 'PHYSIC_CLOTHE')
INSERT INTO [ResourceType] ([ResourceTypeId], [Name]) VALUES (11, 'PHYSIC_SOFT_BODY')
INSERT INTO [ResourceType] ([ResourceTypeId], [Name]) VALUES (12, 'VISUAL_HEIGHTFIELD')
INSERT INTO [ResourceType] ([ResourceTypeId], [Name]) VALUES (13, 'LIGHT')
INSERT INTO [ResourceType] ([ResourceTypeId], [Name]) VALUES (14, 'CAMERA')
INSERT INTO [ResourceType] ([ResourceTypeId], [Name]) VALUES (15, 'PARTICLE_SYSTEM')
INSERT INTO [ResourceType] ([ResourceTypeId], [Name]) VALUES (16, 'BILLBOARD')
INSERT INTO [ResourceType] ([ResourceTypeId], [Name]) VALUES (17, 'WATER')
INSERT INTO [ResourceType] ([ResourceTypeId], [Name]) VALUES (18, 'GRASS')
INSERT INTO [ResourceType] ([ResourceTypeId], [Name]) VALUES (19, 'FRAME_OBJECT')
INSERT INTO [ResourceType] ([ResourceTypeId], [Name]) VALUES (20, 'OBJECT')

GO
SET IDENTITY_INSERT [ResourceType] OFF
GO
SET IDENTITY_INSERT [Users] ON
GO
INSERT INTO [Users] ([UserId], [UserName], [Email], [Password], [LastUpdateTime]) VALUES (1, 'Admin', NULL, 'Admin', '2015-08-08T00:00:00.000')

GO
SET IDENTITY_INSERT [Users] OFF
GO
SET IDENTITY_INSERT [AssetContents] ON
GO
SET IDENTITY_INSERT [AssetContents] OFF
GO
SET IDENTITY_INSERT [Resources] ON
GO
SET IDENTITY_INSERT [Resources] OFF
GO
SET IDENTITY_INSERT [Folders] ON
GO
INSERT INTO [Folders] ([FolderId], [Name], [LastUpdatedDate], [CreateUser], [UpdateUser]) VALUES (1, 'Textures', '2015-08-08T00:00:00.000', 1, 1)
INSERT INTO [Folders] ([FolderId], [Name], [LastUpdatedDate], [CreateUser], [UpdateUser]) VALUES (2, 'Meshes', '2015-08-08T00:00:00.000', 1, 1)
INSERT INTO [Folders] ([FolderId], [Name], [LastUpdatedDate], [CreateUser], [UpdateUser]) VALUES (3, 'Frames', '2015-08-08T00:00:00.000', 1, 1)
INSERT INTO [Folders] ([FolderId], [Name], [LastUpdatedDate], [CreateUser], [UpdateUser]) VALUES (4, 'Animations', '2015-08-08T00:00:00.000', 1, 1)
INSERT INTO [Folders] ([FolderId], [Name], [LastUpdatedDate], [CreateUser], [UpdateUser]) VALUES (5, 'Objects', '2015-08-08T00:00:00.000', 1, 1)

GO
SET IDENTITY_INSERT [Folders] OFF
GO
ALTER TABLE [Resources] ADD CONSTRAINT [FK_Resources_FolderId] FOREIGN KEY ([FolderId]) REFERENCES [Folders]([FolderId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO
ALTER TABLE [Resources] ADD CONSTRAINT [FK_Resources_ContentId] FOREIGN KEY ([ContentId]) REFERENCES [AssetContents]([ContentId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO
ALTER TABLE [Resources] ADD CONSTRAINT [FK_Resources_ResourceTypeId] FOREIGN KEY ([ResourceTypeId]) REFERENCES [ResourceType]([ResourceTypeId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO
ALTER TABLE [Resources] ADD CONSTRAINT [FK_Resources_CreateUserId] FOREIGN KEY ([CreateUserId]) REFERENCES [Users]([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO
ALTER TABLE [Resources] ADD CONSTRAINT [FK_Resources_UpdateUserId] FOREIGN KEY ([UpdateUserId]) REFERENCES [Users]([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO
ALTER TABLE [Folders] ADD CONSTRAINT [FK_Folders_CreateUser] FOREIGN KEY ([CreateUser]) REFERENCES [Users]([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO
ALTER TABLE [Folders] ADD CONSTRAINT [FK_Folders_UpdateUser] FOREIGN KEY ([UpdateUser]) REFERENCES [Users]([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sp_helpdiagrams]') AND type in (N'P', N'PC'))
DROP PROCEDURE [sp_helpdiagrams]
GO

	CREATE PROCEDURE dbo.sp_helpdiagrams
	(
		@diagramname sysname = NULL,
		@owner_id int = NULL
	)
	WITH EXECUTE AS N'dbo'
	AS
	BEGIN
		DECLARE @user sysname
		DECLARE @dboLogin bit
		EXECUTE AS CALLER;
			SET @user = USER_NAME();
			SET @dboLogin = CONVERT(bit,IS_MEMBER('db_owner'));
		REVERT;
		SELECT
			[Database] = DB_NAME(),
			[Name] = name,
			[ID] = diagram_id,
			[Owner] = USER_NAME(principal_id),
			[OwnerID] = principal_id
		FROM
			sysdiagrams
		WHERE
			(@dboLogin = 1 OR USER_NAME(principal_id) = @user) AND
			(@diagramname IS NULL OR name = @diagramname) AND
			(@owner_id IS NULL OR principal_id = @owner_id)
		ORDER BY
			4, 5, 1
	END
	
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sp_helpdiagramdefinition]') AND type in (N'P', N'PC'))
DROP PROCEDURE [sp_helpdiagramdefinition]
GO

	CREATE PROCEDURE dbo.sp_helpdiagramdefinition
	(
		@diagramname 	sysname,
		@owner_id	int	= null 		
	)
	WITH EXECUTE AS N'dbo'
	AS
	BEGIN
		set nocount on

		declare @theId 		int
		declare @IsDbo 		int
		declare @DiagId		int
		declare @UIDFound	int
	
		if(@diagramname is null)
		begin
			RAISERROR (N'E_INVALIDARG', 16, 1);
			return -1
		end
	
		execute as caller;
		select @theId = DATABASE_PRINCIPAL_ID();
		select @IsDbo = IS_MEMBER(N'db_owner');
		if(@owner_id is null)
			select @owner_id = @theId;
		revert; 
	
		select @DiagId = diagram_id, @UIDFound = principal_id from dbo.sysdiagrams where principal_id = @owner_id and name = @diagramname;
		if(@DiagId IS NULL or (@IsDbo = 0 and @UIDFound <> @theId ))
		begin
			RAISERROR ('Diagram does not exist or you do not have permission.', 16, 1);
			return -3
		end

		select version, definition FROM dbo.sysdiagrams where diagram_id = @DiagId ; 
		return 0
	END
	
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sp_creatediagram]') AND type in (N'P', N'PC'))
DROP PROCEDURE [sp_creatediagram]
GO

	CREATE PROCEDURE dbo.sp_creatediagram
	(
		@diagramname 	sysname,
		@owner_id		int	= null, 	
		@version 		int,
		@definition 	varbinary(max)
	)
	WITH EXECUTE AS 'dbo'
	AS
	BEGIN
		set nocount on
	
		declare @theId int
		declare @retval int
		declare @IsDbo	int
		declare @userName sysname
		if(@version is null or @diagramname is null)
		begin
			RAISERROR (N'E_INVALIDARG', 16, 1);
			return -1
		end
	
		execute as caller;
		select @theId = DATABASE_PRINCIPAL_ID(); 
		select @IsDbo = IS_MEMBER(N'db_owner');
		revert; 
		
		if @owner_id is null
		begin
			select @owner_id = @theId;
		end
		else
		begin
			if @theId <> @owner_id
			begin
				if @IsDbo = 0
				begin
					RAISERROR (N'E_INVALIDARG', 16, 1);
					return -1
				end
				select @theId = @owner_id
			end
		end
		-- next 2 line only for test, will be removed after define name unique
		if EXISTS(select diagram_id from dbo.sysdiagrams where principal_id = @theId and name = @diagramname)
		begin
			RAISERROR ('The name is already used.', 16, 1);
			return -2
		end
	
		insert into dbo.sysdiagrams(name, principal_id , version, definition)
				VALUES(@diagramname, @theId, @version, @definition) ;
		
		select @retval = @@IDENTITY 
		return @retval
	END
	
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sp_renamediagram]') AND type in (N'P', N'PC'))
DROP PROCEDURE [sp_renamediagram]
GO

	CREATE PROCEDURE dbo.sp_renamediagram
	(
		@diagramname 		sysname,
		@owner_id		int	= null,
		@new_diagramname	sysname
	
	)
	WITH EXECUTE AS 'dbo'
	AS
	BEGIN
		set nocount on
		declare @theId 			int
		declare @IsDbo 			int
		
		declare @UIDFound 		int
		declare @DiagId			int
		declare @DiagIdTarg		int
		declare @u_name			sysname
		if((@diagramname is null) or (@new_diagramname is null))
		begin
			RAISERROR ('Invalid value', 16, 1);
			return -1
		end
	
		EXECUTE AS CALLER;
		select @theId = DATABASE_PRINCIPAL_ID();
		select @IsDbo = IS_MEMBER(N'db_owner'); 
		if(@owner_id is null)
			select @owner_id = @theId;
		REVERT;
	
		select @u_name = USER_NAME(@owner_id)
	
		select @DiagId = diagram_id, @UIDFound = principal_id from dbo.sysdiagrams where principal_id = @owner_id and name = @diagramname 
		if(@DiagId IS NULL or (@IsDbo = 0 and @UIDFound <> @theId))
		begin
			RAISERROR ('Diagram does not exist or you do not have permission.', 16, 1)
			return -3
		end
	
		-- if((@u_name is not null) and (@new_diagramname = @diagramname))	-- nothing will change
		--	return 0;
	
		if(@u_name is null)
			select @DiagIdTarg = diagram_id from dbo.sysdiagrams where principal_id = @theId and name = @new_diagramname
		else
			select @DiagIdTarg = diagram_id from dbo.sysdiagrams where principal_id = @owner_id and name = @new_diagramname
	
		if((@DiagIdTarg is not null) and  @DiagId <> @DiagIdTarg)
		begin
			RAISERROR ('The name is already used.', 16, 1);
			return -2
		end		
	
		if(@u_name is null)
			update dbo.sysdiagrams set [name] = @new_diagramname, principal_id = @theId where diagram_id = @DiagId
		else
			update dbo.sysdiagrams set [name] = @new_diagramname where diagram_id = @DiagId
		return 0
	END
	
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sp_alterdiagram]') AND type in (N'P', N'PC'))
DROP PROCEDURE [sp_alterdiagram]
GO

	CREATE PROCEDURE dbo.sp_alterdiagram
	(
		@diagramname 	sysname,
		@owner_id	int	= null,
		@version 	int,
		@definition 	varbinary(max)
	)
	WITH EXECUTE AS 'dbo'
	AS
	BEGIN
		set nocount on
	
		declare @theId 			int
		declare @retval 		int
		declare @IsDbo 			int
		
		declare @UIDFound 		int
		declare @DiagId			int
		declare @ShouldChangeUID	int
	
		if(@diagramname is null)
		begin
			RAISERROR ('Invalid ARG', 16, 1)
			return -1
		end
	
		execute as caller;
		select @theId = DATABASE_PRINCIPAL_ID();	 
		select @IsDbo = IS_MEMBER(N'db_owner'); 
		if(@owner_id is null)
			select @owner_id = @theId;
		revert;
	
		select @ShouldChangeUID = 0
		select @DiagId = diagram_id, @UIDFound = principal_id from dbo.sysdiagrams where principal_id = @owner_id and name = @diagramname 
		
		if(@DiagId IS NULL or (@IsDbo = 0 and @theId <> @UIDFound))
		begin
			RAISERROR ('Diagram does not exist or you do not have permission.', 16, 1);
			return -3
		end
	
		if(@IsDbo <> 0)
		begin
			if(@UIDFound is null or USER_NAME(@UIDFound) is null) -- invalid principal_id
			begin
				select @ShouldChangeUID = 1 ;
			end
		end

		-- update dds data			
		update dbo.sysdiagrams set definition = @definition where diagram_id = @DiagId ;

		-- change owner
		if(@ShouldChangeUID = 1)
			update dbo.sysdiagrams set principal_id = @theId where diagram_id = @DiagId ;

		-- update dds version
		if(@version is not null)
			update dbo.sysdiagrams set version = @version where diagram_id = @DiagId ;

		return 0
	END
	
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sp_dropdiagram]') AND type in (N'P', N'PC'))
DROP PROCEDURE [sp_dropdiagram]
GO

	CREATE PROCEDURE dbo.sp_dropdiagram
	(
		@diagramname 	sysname,
		@owner_id	int	= null
	)
	WITH EXECUTE AS 'dbo'
	AS
	BEGIN
		set nocount on
		declare @theId 			int
		declare @IsDbo 			int
		
		declare @UIDFound 		int
		declare @DiagId			int
	
		if(@diagramname is null)
		begin
			RAISERROR ('Invalid value', 16, 1);
			return -1
		end
	
		EXECUTE AS CALLER;
		select @theId = DATABASE_PRINCIPAL_ID();
		select @IsDbo = IS_MEMBER(N'db_owner'); 
		if(@owner_id is null)
			select @owner_id = @theId;
		REVERT; 
		
		select @DiagId = diagram_id, @UIDFound = principal_id from dbo.sysdiagrams where principal_id = @owner_id and name = @diagramname 
		if(@DiagId IS NULL or (@IsDbo = 0 and @UIDFound <> @theId))
		begin
			RAISERROR ('Diagram does not exist or you do not have permission.', 16, 1)
			return -3
		end
	
		delete from dbo.sysdiagrams where diagram_id = @DiagId;
	
		return 0;
	END
	
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sp_upgraddiagrams]') AND type in (N'P', N'PC'))
DROP PROCEDURE [sp_upgraddiagrams]
GO

	CREATE PROCEDURE dbo.sp_upgraddiagrams
	AS
	BEGIN
		IF OBJECT_ID(N'dbo.sysdiagrams') IS NOT NULL
			return 0;
	
		CREATE TABLE dbo.sysdiagrams
		(
			name sysname NOT NULL,
			principal_id int NOT NULL,	-- we may change it to varbinary(85)
			diagram_id int PRIMARY KEY IDENTITY,
			version int,
	
			definition varbinary(max)
			CONSTRAINT UK_principal_name UNIQUE
			(
				principal_id,
				name
			)
		);


		/* Add this if we need to have some form of extended properties for diagrams */
		/*
		IF OBJECT_ID(N'dbo.sysdiagram_properties') IS NULL
		BEGIN
			CREATE TABLE dbo.sysdiagram_properties
			(
				diagram_id int,
				name sysname,
				value varbinary(max) NOT NULL
			)
		END
		*/

		IF OBJECT_ID(N'dbo.dtproperties') IS NOT NULL
		begin
			insert into dbo.sysdiagrams
			(
				[name],
				[principal_id],
				[version],
				[definition]
			)
			select	 
				convert(sysname, dgnm.[uvalue]),
				DATABASE_PRINCIPAL_ID(N'dbo'),			-- will change to the sid of sa
				0,							-- zero for old format, dgdef.[version],
				dgdef.[lvalue]
			from dbo.[dtproperties] dgnm
				inner join dbo.[dtproperties] dggd on dggd.[property] = 'DtgSchemaGUID' and dggd.[objectid] = dgnm.[objectid]	
				inner join dbo.[dtproperties] dgdef on dgdef.[property] = 'DtgSchemaDATA' and dgdef.[objectid] = dgnm.[objectid]
				
			where dgnm.[property] = 'DtgSchemaNAME' and dggd.[uvalue] like N'_EA3E6268-D998-11CE-9454-00AA00A3F36E_' 
			return 2;
		end
		return 1;
	END
	
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[fn_diagramobjects]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [fn_diagramobjects]
GO

	CREATE FUNCTION dbo.fn_diagramobjects() 
	RETURNS int
	WITH EXECUTE AS N'dbo'
	AS
	BEGIN
		declare @id_upgraddiagrams		int
		declare @id_sysdiagrams			int
		declare @id_helpdiagrams		int
		declare @id_helpdiagramdefinition	int
		declare @id_creatediagram	int
		declare @id_renamediagram	int
		declare @id_alterdiagram 	int 
		declare @id_dropdiagram		int
		declare @InstalledObjects	int

		select @InstalledObjects = 0

		select 	@id_upgraddiagrams = object_id(N'dbo.sp_upgraddiagrams'),
			@id_sysdiagrams = object_id(N'dbo.sysdiagrams'),
			@id_helpdiagrams = object_id(N'dbo.sp_helpdiagrams'),
			@id_helpdiagramdefinition = object_id(N'dbo.sp_helpdiagramdefinition'),
			@id_creatediagram = object_id(N'dbo.sp_creatediagram'),
			@id_renamediagram = object_id(N'dbo.sp_renamediagram'),
			@id_alterdiagram = object_id(N'dbo.sp_alterdiagram'), 
			@id_dropdiagram = object_id(N'dbo.sp_dropdiagram')

		if @id_upgraddiagrams is not null
			select @InstalledObjects = @InstalledObjects + 1
		if @id_sysdiagrams is not null
			select @InstalledObjects = @InstalledObjects + 2
		if @id_helpdiagrams is not null
			select @InstalledObjects = @InstalledObjects + 4
		if @id_helpdiagramdefinition is not null
			select @InstalledObjects = @InstalledObjects + 8
		if @id_creatediagram is not null
			select @InstalledObjects = @InstalledObjects + 16
		if @id_renamediagram is not null
			select @InstalledObjects = @InstalledObjects + 32
		if @id_alterdiagram  is not null
			select @InstalledObjects = @InstalledObjects + 64
		if @id_dropdiagram is not null
			select @InstalledObjects = @InstalledObjects + 128
		
		return @InstalledObjects 
	END
	