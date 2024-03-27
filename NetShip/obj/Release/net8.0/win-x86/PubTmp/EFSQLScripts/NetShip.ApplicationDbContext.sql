IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    IF SCHEMA_ID(N'MasterBase') IS NULL EXEC(N'CREATE SCHEMA [MasterBase];');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE TABLE [MasterBase].[Roles] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE TABLE [MasterBase].[Users] (
        [Id] nvarchar(450) NOT NULL,
        [FirstName] nvarchar(max) NULL,
        [LastName] nvarchar(max) NULL,
        [DisableTutorial] bit NULL,
        [ReferralLink] nvarchar(max) NULL,
        [ReferredBy] nvarchar(max) NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE TABLE [MasterBase].[RoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_RoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RoleClaims_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [MasterBase].[Roles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE TABLE [MasterBase].[Brands] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NULL,
        [UrlNormalizedName] nvarchar(max) NULL,
        [Logo] nvarchar(max) NULL,
        [Slogan] nvarchar(max) NULL,
        [Instagram] nvarchar(max) NULL,
        [Facebook] nvarchar(max) NULL,
        [Website] nvarchar(max) NULL,
        [CatalogsBackground] nvarchar(max) NULL,
        [UserId] nvarchar(450) NOT NULL,
        [ApplicationUserId] nvarchar(450) NULL,
        CONSTRAINT [PK_Brands] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Brands_Users_ApplicationUserId] FOREIGN KEY ([ApplicationUserId]) REFERENCES [MasterBase].[Users] ([Id]),
        CONSTRAINT [FK_Brands_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [MasterBase].[Users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE TABLE [MasterBase].[UserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_UserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_UserClaims_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [MasterBase].[Users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE TABLE [MasterBase].[UserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_UserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_UserLogins_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [MasterBase].[Users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE TABLE [MasterBase].[UserRoles] (
        [UserId] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_UserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_UserRoles_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [MasterBase].[Roles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_UserRoles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [MasterBase].[Users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE TABLE [MasterBase].[UserTokens] (
        [UserId] nvarchar(450) NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_UserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_UserTokens_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [MasterBase].[Users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE TABLE [MasterBase].[Branches] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [UrlNormalizedName] nvarchar(max) NOT NULL,
        [NormalizedDigitalMenu] nvarchar(max) NULL,
        [DigitalMenuLink] nvarchar(max) NULL,
        [QrCodePath] nvarchar(max) NULL,
        [BrandId] uniqueidentifier NOT NULL,
        [DigitalMenuJson] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Branches] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Branches_Brands_BrandId] FOREIGN KEY ([BrandId]) REFERENCES [MasterBase].[Brands] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE TABLE [MasterBase].[Platforms] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NULL,
        [Alias] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        [IsScheduleActive] bit NOT NULL,
        [Sort] int NOT NULL,
        [Icon] nvarchar(max) NULL,
        [BrandId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Platforms] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Platforms_Brands_BrandId] FOREIGN KEY ([BrandId]) REFERENCES [MasterBase].[Brands] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE TABLE [MasterBase].[Catalogs] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [IsActive] bit NOT NULL,
        [IsScheduleActive] bit NOT NULL,
        [Sort] int NOT NULL,
        [BranchId] uniqueidentifier NOT NULL,
        [Icon] nvarchar(max) NULL,
        CONSTRAINT [PK_Catalogs] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Catalogs_Branches_BranchId] FOREIGN KEY ([BranchId]) REFERENCES [MasterBase].[Branches] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE TABLE [MasterBase].[Categories] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        [IsScheduleActive] bit NOT NULL,
        [Sort] int NOT NULL,
        [CatalogId] uniqueidentifier NOT NULL,
        [Icon] nvarchar(max) NULL,
        CONSTRAINT [PK_Categories] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Categories_Catalogs_CatalogId] FOREIGN KEY ([CatalogId]) REFERENCES [MasterBase].[Catalogs] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE TABLE [MasterBase].[Items] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NULL,
        [Alias] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        [IsScheduleActive] bit NOT NULL,
        [Sort] int NOT NULL,
        [CategoryId] uniqueidentifier NOT NULL,
        [Icon] nvarchar(max) NULL,
        [Price] decimal(18,2) NULL,
        CONSTRAINT [PK_Items] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Items_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [MasterBase].[Categories] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE TABLE [MasterBase].[Products] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NULL,
        [Alias] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        [IsScheduleActive] bit NOT NULL,
        [Sort] int NOT NULL,
        [CategoryId] uniqueidentifier NOT NULL,
        [Icon] nvarchar(max) NULL,
        CONSTRAINT [PK_Products] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Products_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [MasterBase].[Categories] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE TABLE [MasterBase].[PricePerItemPerPlatforms] (
        [ItemId] uniqueidentifier NOT NULL,
        [PlatformId] uniqueidentifier NOT NULL,
        [Id] uniqueidentifier NOT NULL,
        [Price] float NULL,
        [IsActive] bit NOT NULL,
        [PlatformId1] uniqueidentifier NULL,
        CONSTRAINT [PK_PricePerItemPerPlatforms] PRIMARY KEY ([PlatformId], [ItemId]),
        CONSTRAINT [FK_PricePerItemPerPlatforms_Items_ItemId] FOREIGN KEY ([ItemId]) REFERENCES [MasterBase].[Items] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_PricePerItemPerPlatforms_Platforms_PlatformId] FOREIGN KEY ([PlatformId]) REFERENCES [MasterBase].[Platforms] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_PricePerItemPerPlatforms_Platforms_PlatformId1] FOREIGN KEY ([PlatformId1]) REFERENCES [MasterBase].[Platforms] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE TABLE [MasterBase].[ModifiersGroups] (
        [Id] uniqueidentifier NOT NULL,
        [ProductId] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        [IsScheduleActive] bit NOT NULL,
        [Sort] int NOT NULL,
        [Alias] nvarchar(max) NULL,
        [Icon] nvarchar(max) NULL,
        [MinModifiers] int NOT NULL,
        [MaxModifiers] int NOT NULL,
        CONSTRAINT [PK_ModifiersGroups] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ModifiersGroups_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [MasterBase].[Products] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE TABLE [MasterBase].[Modifiers] (
        [Id] uniqueidentifier NOT NULL,
        [ModifiersGroupId] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NULL,
        [Alias] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        [IsScheduleActive] bit NOT NULL,
        [Sort] int NOT NULL,
        [Icon] nvarchar(max) NULL,
        [MinModifier] int NOT NULL,
        [MaxModifier] int NOT NULL,
        [Price] decimal(18,2) NULL,
        CONSTRAINT [PK_Modifiers] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Modifiers_ModifiersGroups_ModifiersGroupId] FOREIGN KEY ([ModifiersGroupId]) REFERENCES [MasterBase].[ModifiersGroups] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE TABLE [MasterBase].[PricePerModifierPerPlatforms] (
        [ModifierId] uniqueidentifier NOT NULL,
        [PlatformId] uniqueidentifier NOT NULL,
        [Id] uniqueidentifier NOT NULL,
        [Price] float NULL,
        [IsActive] bit NOT NULL,
        [PlatformId1] uniqueidentifier NULL,
        CONSTRAINT [PK_PricePerModifierPerPlatforms] PRIMARY KEY ([PlatformId], [ModifierId]),
        CONSTRAINT [FK_PricePerModifierPerPlatforms_Modifiers_ModifierId] FOREIGN KEY ([ModifierId]) REFERENCES [MasterBase].[Modifiers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_PricePerModifierPerPlatforms_Platforms_PlatformId] FOREIGN KEY ([PlatformId]) REFERENCES [MasterBase].[Platforms] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_PricePerModifierPerPlatforms_Platforms_PlatformId1] FOREIGN KEY ([PlatformId1]) REFERENCES [MasterBase].[Platforms] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE INDEX [IX_Branches_BrandId] ON [MasterBase].[Branches] ([BrandId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE INDEX [IX_Brands_ApplicationUserId] ON [MasterBase].[Brands] ([ApplicationUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Brands_UserId] ON [MasterBase].[Brands] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE INDEX [IX_Catalogs_BranchId] ON [MasterBase].[Catalogs] ([BranchId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE INDEX [IX_Categories_CatalogId] ON [MasterBase].[Categories] ([CatalogId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE INDEX [IX_Items_CategoryId] ON [MasterBase].[Items] ([CategoryId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE INDEX [IX_Modifiers_ModifiersGroupId] ON [MasterBase].[Modifiers] ([ModifiersGroupId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE INDEX [IX_ModifiersGroups_ProductId] ON [MasterBase].[ModifiersGroups] ([ProductId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE INDEX [IX_Platforms_BrandId] ON [MasterBase].[Platforms] ([BrandId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE INDEX [IX_PricePerItemPerPlatforms_ItemId] ON [MasterBase].[PricePerItemPerPlatforms] ([ItemId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE INDEX [IX_PricePerItemPerPlatforms_PlatformId1] ON [MasterBase].[PricePerItemPerPlatforms] ([PlatformId1]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE INDEX [IX_PricePerModifierPerPlatforms_ModifierId] ON [MasterBase].[PricePerModifierPerPlatforms] ([ModifierId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE INDEX [IX_PricePerModifierPerPlatforms_PlatformId1] ON [MasterBase].[PricePerModifierPerPlatforms] ([PlatformId1]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE INDEX [IX_Products_CategoryId] ON [MasterBase].[Products] ([CategoryId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE INDEX [IX_RoleClaims_RoleId] ON [MasterBase].[RoleClaims] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [MasterBase].[Roles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE INDEX [IX_UserClaims_UserId] ON [MasterBase].[UserClaims] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE INDEX [IX_UserLogins_UserId] ON [MasterBase].[UserLogins] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE INDEX [IX_UserRoles_RoleId] ON [MasterBase].[UserRoles] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    CREATE INDEX [EmailIndex] ON [MasterBase].[Users] ([NormalizedEmail]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [MasterBase].[Users] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240323195711_initDB'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240323195711_initDB', N'8.0.3');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240324061049_CatalogsInitit'
)
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MasterBase].[Catalogs]') AND [c].[name] = N'Name');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [MasterBase].[Catalogs] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [MasterBase].[Catalogs] ALTER COLUMN [Name] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240324061049_CatalogsInitit'
)
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MasterBase].[Catalogs]') AND [c].[name] = N'Description');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [MasterBase].[Catalogs] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [MasterBase].[Catalogs] ALTER COLUMN [Description] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240324061049_CatalogsInitit'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240324061049_CatalogsInitit', N'8.0.3');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240327213347_localDB'
)
BEGIN
    ALTER TABLE [MasterBase].[Products] ADD [Price] decimal(18,2) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240327213347_localDB'
)
BEGIN
    ALTER TABLE [MasterBase].[ModifiersGroups] ADD [showPrices] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240327213347_localDB'
)
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MasterBase].[Categories]') AND [c].[name] = N'Icon');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [MasterBase].[Categories] DROP CONSTRAINT [' + @var2 + '];');
    EXEC(N'UPDATE [MasterBase].[Categories] SET [Icon] = N'''' WHERE [Icon] IS NULL');
    ALTER TABLE [MasterBase].[Categories] ALTER COLUMN [Icon] nvarchar(max) NOT NULL;
    ALTER TABLE [MasterBase].[Categories] ADD DEFAULT N'' FOR [Icon];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240327213347_localDB'
)
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MasterBase].[Categories]') AND [c].[name] = N'Description');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [MasterBase].[Categories] DROP CONSTRAINT [' + @var3 + '];');
    EXEC(N'UPDATE [MasterBase].[Categories] SET [Description] = N'''' WHERE [Description] IS NULL');
    ALTER TABLE [MasterBase].[Categories] ALTER COLUMN [Description] nvarchar(max) NOT NULL;
    ALTER TABLE [MasterBase].[Categories] ADD DEFAULT N'' FOR [Description];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240327213347_localDB'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240327213347_localDB', N'8.0.3');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240327213946_localDB2'
)
BEGIN
    EXEC sp_rename N'[MasterBase].[ModifiersGroups].[showPrices]', N'isSelectable', N'COLUMN';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240327213946_localDB2'
)
BEGIN
    DECLARE @var4 sysname;
    SELECT @var4 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MasterBase].[Products]') AND [c].[name] = N'Price');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [MasterBase].[Products] DROP CONSTRAINT [' + @var4 + '];');
    EXEC(N'UPDATE [MasterBase].[Products] SET [Price] = 0.0 WHERE [Price] IS NULL');
    ALTER TABLE [MasterBase].[Products] ALTER COLUMN [Price] decimal(18,2) NOT NULL;
    ALTER TABLE [MasterBase].[Products] ADD DEFAULT 0.0 FOR [Price];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240327213946_localDB2'
)
BEGIN
    DECLARE @var5 sysname;
    SELECT @var5 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MasterBase].[Products]') AND [c].[name] = N'Name');
    IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [MasterBase].[Products] DROP CONSTRAINT [' + @var5 + '];');
    EXEC(N'UPDATE [MasterBase].[Products] SET [Name] = N'''' WHERE [Name] IS NULL');
    ALTER TABLE [MasterBase].[Products] ALTER COLUMN [Name] nvarchar(max) NOT NULL;
    ALTER TABLE [MasterBase].[Products] ADD DEFAULT N'' FOR [Name];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240327213946_localDB2'
)
BEGIN
    DECLARE @var6 sysname;
    SELECT @var6 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MasterBase].[Products]') AND [c].[name] = N'Icon');
    IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [MasterBase].[Products] DROP CONSTRAINT [' + @var6 + '];');
    EXEC(N'UPDATE [MasterBase].[Products] SET [Icon] = N'''' WHERE [Icon] IS NULL');
    ALTER TABLE [MasterBase].[Products] ALTER COLUMN [Icon] nvarchar(max) NOT NULL;
    ALTER TABLE [MasterBase].[Products] ADD DEFAULT N'' FOR [Icon];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240327213946_localDB2'
)
BEGIN
    DECLARE @var7 sysname;
    SELECT @var7 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MasterBase].[Products]') AND [c].[name] = N'Description');
    IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [MasterBase].[Products] DROP CONSTRAINT [' + @var7 + '];');
    EXEC(N'UPDATE [MasterBase].[Products] SET [Description] = N'''' WHERE [Description] IS NULL');
    ALTER TABLE [MasterBase].[Products] ALTER COLUMN [Description] nvarchar(max) NOT NULL;
    ALTER TABLE [MasterBase].[Products] ADD DEFAULT N'' FOR [Description];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240327213946_localDB2'
)
BEGIN
    DECLARE @var8 sysname;
    SELECT @var8 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MasterBase].[Products]') AND [c].[name] = N'Alias');
    IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [MasterBase].[Products] DROP CONSTRAINT [' + @var8 + '];');
    EXEC(N'UPDATE [MasterBase].[Products] SET [Alias] = N'''' WHERE [Alias] IS NULL');
    ALTER TABLE [MasterBase].[Products] ALTER COLUMN [Alias] nvarchar(max) NOT NULL;
    ALTER TABLE [MasterBase].[Products] ADD DEFAULT N'' FOR [Alias];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240327213946_localDB2'
)
BEGIN
    DECLARE @var9 sysname;
    SELECT @var9 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MasterBase].[ModifiersGroups]') AND [c].[name] = N'Name');
    IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [MasterBase].[ModifiersGroups] DROP CONSTRAINT [' + @var9 + '];');
    EXEC(N'UPDATE [MasterBase].[ModifiersGroups] SET [Name] = N'''' WHERE [Name] IS NULL');
    ALTER TABLE [MasterBase].[ModifiersGroups] ALTER COLUMN [Name] nvarchar(max) NOT NULL;
    ALTER TABLE [MasterBase].[ModifiersGroups] ADD DEFAULT N'' FOR [Name];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240327213946_localDB2'
)
BEGIN
    DECLARE @var10 sysname;
    SELECT @var10 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MasterBase].[ModifiersGroups]') AND [c].[name] = N'Icon');
    IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [MasterBase].[ModifiersGroups] DROP CONSTRAINT [' + @var10 + '];');
    EXEC(N'UPDATE [MasterBase].[ModifiersGroups] SET [Icon] = N'''' WHERE [Icon] IS NULL');
    ALTER TABLE [MasterBase].[ModifiersGroups] ALTER COLUMN [Icon] nvarchar(max) NOT NULL;
    ALTER TABLE [MasterBase].[ModifiersGroups] ADD DEFAULT N'' FOR [Icon];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240327213946_localDB2'
)
BEGIN
    DECLARE @var11 sysname;
    SELECT @var11 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MasterBase].[ModifiersGroups]') AND [c].[name] = N'Description');
    IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [MasterBase].[ModifiersGroups] DROP CONSTRAINT [' + @var11 + '];');
    EXEC(N'UPDATE [MasterBase].[ModifiersGroups] SET [Description] = N'''' WHERE [Description] IS NULL');
    ALTER TABLE [MasterBase].[ModifiersGroups] ALTER COLUMN [Description] nvarchar(max) NOT NULL;
    ALTER TABLE [MasterBase].[ModifiersGroups] ADD DEFAULT N'' FOR [Description];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240327213946_localDB2'
)
BEGIN
    DECLARE @var12 sysname;
    SELECT @var12 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MasterBase].[ModifiersGroups]') AND [c].[name] = N'Alias');
    IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [MasterBase].[ModifiersGroups] DROP CONSTRAINT [' + @var12 + '];');
    EXEC(N'UPDATE [MasterBase].[ModifiersGroups] SET [Alias] = N'''' WHERE [Alias] IS NULL');
    ALTER TABLE [MasterBase].[ModifiersGroups] ALTER COLUMN [Alias] nvarchar(max) NOT NULL;
    ALTER TABLE [MasterBase].[ModifiersGroups] ADD DEFAULT N'' FOR [Alias];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240327213946_localDB2'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240327213946_localDB2', N'8.0.3');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240327215809_localDB3'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240327215809_localDB3', N'8.0.3');
END;
GO

COMMIT;
GO

