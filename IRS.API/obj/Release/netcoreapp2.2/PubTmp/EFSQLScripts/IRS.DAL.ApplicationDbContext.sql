IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [Code] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE TABLE [Countries] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(255) NOT NULL,
        [Description] nvarchar(255) NULL,
        [Protected] bit NOT NULL,
        [Deleted] bit NOT NULL,
        [Code1] nvarchar(max) NULL,
        [Code2] nvarchar(max) NULL,
        [Flag] nvarchar(max) NULL,
        CONSTRAINT [PK_Countries] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE TABLE [IncidenceStatuses] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(255) NOT NULL,
        [Description] nvarchar(255) NULL,
        [Protected] bit NOT NULL,
        [Deleted] bit NOT NULL,
        CONSTRAINT [PK_IncidenceStatuses] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE TABLE [IncidenceTypes] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(255) NOT NULL,
        [Description] nvarchar(255) NULL,
        [Protected] bit NOT NULL,
        [Deleted] bit NOT NULL,
        CONSTRAINT [PK_IncidenceTypes] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE TABLE [SystemSettings] (
        [Id] uniqueidentifier NOT NULL,
        [BrandCssStyle] nvarchar(max) NULL,
        [PreferredLanguage] nvarchar(max) NULL,
        [BrandTitle] nvarchar(max) NULL,
        [BrandLogo] nvarchar(max) NULL,
        [BrandIcon] nvarchar(max) NULL,
        [Email1] nvarchar(max) NULL,
        [Email2] nvarchar(max) NULL,
        [Phone1] nvarchar(max) NULL,
        [Phone2] nvarchar(max) NULL,
        [UseSsl] bit NOT NULL,
        [HostName] nvarchar(max) NULL,
        [Port] nvarchar(max) NULL,
        [MaxFileSize] nvarchar(max) NULL,
        [AcceptedFileTypes] nvarchar(max) NULL,
        [PageSize] int NOT NULL,
        CONSTRAINT [PK_SystemSettings] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] uniqueidentifier NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE TABLE [States] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(255) NOT NULL,
        [Description] nvarchar(255) NULL,
        [Protected] bit NOT NULL,
        [Deleted] bit NOT NULL,
        [Code] nvarchar(max) NULL,
        [CountryId] uniqueidentifier NULL,
        CONSTRAINT [PK_States] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_States_Countries_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [Countries] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE TABLE [Cities] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(255) NOT NULL,
        [Description] nvarchar(255) NULL,
        [Protected] bit NOT NULL,
        [Deleted] bit NOT NULL,
        [Code] nvarchar(max) NULL,
        [StateId] uniqueidentifier NULL,
        CONSTRAINT [PK_Cities] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Cities_States_StateId] FOREIGN KEY ([StateId]) REFERENCES [States] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE TABLE [Areas] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(255) NOT NULL,
        [Description] nvarchar(255) NULL,
        [Protected] bit NOT NULL,
        [Deleted] bit NOT NULL,
        [Code] nvarchar(max) NULL,
        [CityId] uniqueidentifier NULL,
        [StateId] uniqueidentifier NULL,
        CONSTRAINT [PK_Areas] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Areas_Cities_CityId] FOREIGN KEY ([CityId]) REFERENCES [Cities] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Areas_States_StateId] FOREIGN KEY ([StateId]) REFERENCES [States] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE TABLE [AspNetUsers] (
        [Id] uniqueidentifier NOT NULL,
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
        [FirstName] nvarchar(max) NULL,
        [MiddleName] nvarchar(max) NULL,
        [LastName] nvarchar(max) NULL,
        [JobTitle] nvarchar(max) NULL,
        [Email1] nvarchar(max) NULL,
        [Email2] nvarchar(max) NULL,
        [Phone1] nvarchar(max) NULL,
        [Phone2] nvarchar(max) NULL,
        [StaffNo] nvarchar(max) NULL,
        [DateOfBirth] datetime2 NOT NULL,
        [KnownAs] nvarchar(max) NULL,
        [LastActive] datetime2 NOT NULL,
        [Introduction] nvarchar(max) NULL,
        [AreaOfOriginId] uniqueidentifier NULL,
        [CityOfOriginId] uniqueidentifier NULL,
        [StateOfOriginId] uniqueidentifier NULL,
        [CountryOfOriginId] uniqueidentifier NULL,
        [DepartmentId] uniqueidentifier NULL,
        [Gender] int NOT NULL,
        [PreferredContactMethod] int NOT NULL,
        [PhotoUrl] nvarchar(max) NULL,
        [Deleted] bit NOT NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUsers_Areas_AreaOfOriginId] FOREIGN KEY ([AreaOfOriginId]) REFERENCES [Areas] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_AspNetUsers_Cities_CityOfOriginId] FOREIGN KEY ([CityOfOriginId]) REFERENCES [Cities] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_AspNetUsers_Countries_CountryOfOriginId] FOREIGN KEY ([CountryOfOriginId]) REFERENCES [Countries] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_AspNetUsers_States_StateOfOriginId] FOREIGN KEY ([StateOfOriginId]) REFERENCES [States] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] uniqueidentifier NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] uniqueidentifier NOT NULL,
        [RoleId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] uniqueidentifier NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE TABLE [AuditTrails] (
        [Id] uniqueidentifier NOT NULL,
        [EventAction] nvarchar(max) NULL,
        [Controller] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [DateHappened] datetime2 NOT NULL,
        [ClientIP] nvarchar(max) NULL,
        [ClientUserAgent] nvarchar(max) NULL,
        [RecordData] nvarchar(max) NULL,
        [UserId] uniqueidentifier NULL,
        CONSTRAINT [PK_AuditTrails] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AuditTrails_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE TABLE [Organizations] (
        [Id] uniqueidentifier NOT NULL,
        [CompanyName] nvarchar(max) NULL,
        [RegistrationNo] nvarchar(max) NULL,
        [BusinessCategory] nvarchar(max) NULL,
        [Code] nvarchar(max) NULL,
        [ContactFirstName] nvarchar(max) NULL,
        [ContactMiddleName] nvarchar(max) NULL,
        [ContactLastName] nvarchar(max) NULL,
        [Phone1] nvarchar(max) NULL,
        [Phone2] nvarchar(max) NULL,
        [OfficeAddress] nvarchar(max) NULL,
        [BrandLogo] nvarchar(max) NULL,
        [DateofEst] datetime2 NOT NULL,
        [Comment] nvarchar(max) NULL,
        [DateCreated] datetime2 NOT NULL,
        [CreatedByUserId] uniqueidentifier NOT NULL,
        [DateEdited] datetime2 NULL,
        [EditedByUserId] uniqueidentifier NULL,
        [AreaId] uniqueidentifier NULL,
        [CityId] uniqueidentifier NULL,
        [StateId] uniqueidentifier NULL,
        [CountryId] uniqueidentifier NULL,
        [EnableBranding] bit NOT NULL,
        [BrandCssStyle] nvarchar(max) NULL,
        [PreferredLanguage] nvarchar(max) NULL,
        [BrandTitle] nvarchar(max) NULL,
        [BrandIcon] nvarchar(max) NULL,
        [Email1] nvarchar(max) NULL,
        [Email2] nvarchar(max) NULL,
        [UseSsl] bit NOT NULL,
        [HostName] nvarchar(max) NULL,
        [Port] nvarchar(max) NULL,
        [MaxFileSize] nvarchar(max) NULL,
        [AcceptedFileTypes] nvarchar(max) NULL,
        [PageSize] int NOT NULL,
        CONSTRAINT [PK_Organizations] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Organizations_Areas_AreaId] FOREIGN KEY ([AreaId]) REFERENCES [Areas] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Organizations_Cities_CityId] FOREIGN KEY ([CityId]) REFERENCES [Cities] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Organizations_Countries_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [Countries] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Organizations_AspNetUsers_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Organizations_AspNetUsers_EditedByUserId] FOREIGN KEY ([EditedByUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Organizations_States_StateId] FOREIGN KEY ([StateId]) REFERENCES [States] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE TABLE [Incidences] (
        [Id] uniqueidentifier NOT NULL,
        [Code] nvarchar(200) NULL,
        [Title] nvarchar(100) NOT NULL,
        [Description] nvarchar(255) NULL,
        [Comment] nvarchar(1024) NULL,
        [RouteCause] nvarchar(255) NULL,
        [AreaId] uniqueidentifier NULL,
        [CityId] uniqueidentifier NULL,
        [StateId] uniqueidentifier NULL,
        [CountryId] uniqueidentifier NULL,
        [ReportedIncidenceLatitude] float NOT NULL,
        [ReportedIncidenceLongitude] float NOT NULL,
        [Suggestion] nvarchar(255) NULL,
        [AssignerId] uniqueidentifier NULL,
        [AssigneeId] uniqueidentifier NULL,
        [DateCreated] datetime2 NULL,
        [CreatedByUserId] uniqueidentifier NULL,
        [DateEdited] datetime2 NULL,
        [EditedByUserId] uniqueidentifier NULL,
        [OrganizationId] uniqueidentifier NULL,
        [ReporterName] nvarchar(50) NULL,
        [ReporterEmail] nvarchar(50) NULL,
        [ReporterFeedbackComment] nvarchar(1024) NULL,
        [ReporterFeedbackRating] int NULL,
        [IncidenceTypeId] uniqueidentifier NOT NULL,
        [IncidenceStatusId] uniqueidentifier NOT NULL,
        [ResolutionDate] datetime2 NULL,
        [SystemSettingId] uniqueidentifier NULL,
        CONSTRAINT [PK_Incidences] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Incidences_Areas_AreaId] FOREIGN KEY ([AreaId]) REFERENCES [Areas] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Incidences_AspNetUsers_AssigneeId] FOREIGN KEY ([AssigneeId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Incidences_AspNetUsers_AssignerId] FOREIGN KEY ([AssignerId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Incidences_Cities_CityId] FOREIGN KEY ([CityId]) REFERENCES [Cities] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Incidences_Countries_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [Countries] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Incidences_IncidenceStatuses_IncidenceStatusId] FOREIGN KEY ([IncidenceStatusId]) REFERENCES [IncidenceStatuses] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Incidences_IncidenceTypes_IncidenceTypeId] FOREIGN KEY ([IncidenceTypeId]) REFERENCES [IncidenceTypes] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Incidences_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [Organizations] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Incidences_States_StateId] FOREIGN KEY ([StateId]) REFERENCES [States] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Incidences_SystemSettings_SystemSettingId] FOREIGN KEY ([SystemSettingId]) REFERENCES [SystemSettings] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE TABLE [OrganizationDepartments] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NULL,
        [OrganizationId] uniqueidentifier NULL,
        [SystemSettingId] uniqueidentifier NULL,
        CONSTRAINT [PK_OrganizationDepartments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_OrganizationDepartments_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [Organizations] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_OrganizationDepartments_SystemSettings_SystemSettingId] FOREIGN KEY ([SystemSettingId]) REFERENCES [SystemSettings] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE TABLE [Photos] (
        [Id] uniqueidentifier NOT NULL,
        [FileName] nvarchar(255) NOT NULL,
        [Url] nvarchar(512) NULL,
        [IncidenceId] uniqueidentifier NULL,
        CONSTRAINT [PK_Photos] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Photos_Incidences_IncidenceId] FOREIGN KEY ([IncidenceId]) REFERENCES [Incidences] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE TABLE [Videos] (
        [Id] uniqueidentifier NOT NULL,
        [FileName] nvarchar(255) NOT NULL,
        [Url] nvarchar(512) NULL,
        [IncidenceId] uniqueidentifier NULL,
        CONSTRAINT [PK_Videos] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Videos_Incidences_IncidenceId] FOREIGN KEY ([IncidenceId]) REFERENCES [Incidences] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE TABLE [UserDeployment] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NULL,
        [AreaOfDeploymentId] uniqueidentifier NULL,
        [CityOfDeploymentId] uniqueidentifier NULL,
        [StateOfDeploymentId] uniqueidentifier NULL,
        [CountryOfDeploymentId] uniqueidentifier NULL,
        [DepartmentId] uniqueidentifier NULL,
        [DateOfDeployment] datetime2 NOT NULL,
        [DateOfSignOff] datetime2 NOT NULL,
        [CurrentDeployment] bit NOT NULL,
        CONSTRAINT [PK_UserDeployment] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_UserDeployment_Areas_AreaOfDeploymentId] FOREIGN KEY ([AreaOfDeploymentId]) REFERENCES [Areas] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_UserDeployment_Cities_CityOfDeploymentId] FOREIGN KEY ([CityOfDeploymentId]) REFERENCES [Cities] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_UserDeployment_Countries_CountryOfDeploymentId] FOREIGN KEY ([CountryOfDeploymentId]) REFERENCES [Countries] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_UserDeployment_OrganizationDepartments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [OrganizationDepartments] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_UserDeployment_States_StateOfDeploymentId] FOREIGN KEY ([StateOfDeploymentId]) REFERENCES [States] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_UserDeployment_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_Areas_CityId] ON [Areas] ([CityId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_Areas_StateId] ON [Areas] ([StateId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_AspNetUsers_AreaOfOriginId] ON [AspNetUsers] ([AreaOfOriginId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_AspNetUsers_CityOfOriginId] ON [AspNetUsers] ([CityOfOriginId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_AspNetUsers_CountryOfOriginId] ON [AspNetUsers] ([CountryOfOriginId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_AspNetUsers_DepartmentId] ON [AspNetUsers] ([DepartmentId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_AspNetUsers_StateOfOriginId] ON [AspNetUsers] ([StateOfOriginId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_AuditTrails_UserId] ON [AuditTrails] ([UserId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_Cities_StateId] ON [Cities] ([StateId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_Incidences_AreaId] ON [Incidences] ([AreaId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_Incidences_AssigneeId] ON [Incidences] ([AssigneeId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_Incidences_AssignerId] ON [Incidences] ([AssignerId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_Incidences_CityId] ON [Incidences] ([CityId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_Incidences_CountryId] ON [Incidences] ([CountryId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_Incidences_IncidenceStatusId] ON [Incidences] ([IncidenceStatusId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_Incidences_IncidenceTypeId] ON [Incidences] ([IncidenceTypeId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_Incidences_OrganizationId] ON [Incidences] ([OrganizationId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_Incidences_StateId] ON [Incidences] ([StateId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_Incidences_SystemSettingId] ON [Incidences] ([SystemSettingId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_OrganizationDepartments_OrganizationId] ON [OrganizationDepartments] ([OrganizationId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_OrganizationDepartments_SystemSettingId] ON [OrganizationDepartments] ([SystemSettingId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_Organizations_AreaId] ON [Organizations] ([AreaId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_Organizations_CityId] ON [Organizations] ([CityId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_Organizations_CountryId] ON [Organizations] ([CountryId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_Organizations_CreatedByUserId] ON [Organizations] ([CreatedByUserId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_Organizations_EditedByUserId] ON [Organizations] ([EditedByUserId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_Organizations_StateId] ON [Organizations] ([StateId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_Photos_IncidenceId] ON [Photos] ([IncidenceId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_States_CountryId] ON [States] ([CountryId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_UserDeployment_AreaOfDeploymentId] ON [UserDeployment] ([AreaOfDeploymentId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_UserDeployment_CityOfDeploymentId] ON [UserDeployment] ([CityOfDeploymentId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_UserDeployment_CountryOfDeploymentId] ON [UserDeployment] ([CountryOfDeploymentId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_UserDeployment_DepartmentId] ON [UserDeployment] ([DepartmentId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_UserDeployment_StateOfDeploymentId] ON [UserDeployment] ([StateOfDeploymentId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_UserDeployment_UserId] ON [UserDeployment] ([UserId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    CREATE INDEX [IX_Videos_IncidenceId] ON [Videos] ([IncidenceId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    ALTER TABLE [AspNetUsers] ADD CONSTRAINT [FK_AspNetUsers_OrganizationDepartments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [OrganizationDepartments] ([Id]) ON DELETE NO ACTION;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    insert into Countries(Id, Code1, Code2, [Name], Protected, Deleted) values ('A164359A-585C-4EB2-A25A-205975A3AD30', 'NGA', 'NGA', 'Nigeria', 1, 0)

    insert into States(Id, Code, [Name], CountryId, Protected, Deleted) values ('35881A6A-2047-4DF2-80C6-97A62A3F545B', 'ABI', 'Abia', 'A164359A-585C-4EB2-A25A-205975A3AD30', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('92520E46-8516-4917-AE1A-8561CF9B9FCC', 'ABS', 'Aba South', '35881A6A-2047-4DF2-80C6-97A62A3F545B', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('2413CC85-F5CA-4E50-8E82-B866ED5D3BAA', 'ARO', 'Arochukwu', '35881A6A-2047-4DF2-80C6-97A62A3F545B', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('46D4E709-8670-47D1-B18D-92F5444A144C', 'BEN', 'Bende', '35881A6A-2047-4DF2-80C6-97A62A3F545B', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('46A3586D-1633-4869-BA29-DA4169E0EE5C', 'IKW', 'Ikwuano', '35881A6A-2047-4DF2-80C6-97A62A3F545B', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('D8EC0344-C08F-4D17-B343-380ADA6171BB', 'INN', 'Isiala Ngwa North', '35881A6A-2047-4DF2-80C6-97A62A3F545B', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('4C14050F-39C0-45C3-821F-C7EB3E66A8AF', 'INS', 'Isiala Ngwa South', '35881A6A-2047-4DF2-80C6-97A62A3F545B', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('CF10DA3B-4057-481E-AADC-1C30EAEED238', 'ISU', 'Isuikwuato', '35881A6A-2047-4DF2-80C6-97A62A3F545B', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('B996F314-3334-4BC4-8B5C-666FEDB581FD', 'OBI', 'Obi Ngwa', '35881A6A-2047-4DF2-80C6-97A62A3F545B', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('7FA0EBBB-EACE-47D5-BA97-6F73C8DAB20B', 'OHA', 'Ohafia', '35881A6A-2047-4DF2-80C6-97A62A3F545B', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('66F75FBB-9717-4C69-84CF-0DEF1092B51C', 'OSI', 'Osisioma', '35881A6A-2047-4DF2-80C6-97A62A3F545B', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('B8F0C833-A9F7-47E5-AE0F-D4D0B3590C64', 'UKE', 'Ukwa East', '35881A6A-2047-4DF2-80C6-97A62A3F545B', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('7B8F31D6-25BD-4CD7-8A32-37C4B501F5E3', 'UKW', 'Ukwa West', '35881A6A-2047-4DF2-80C6-97A62A3F545B', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('D677998A-FA4C-4D67-8419-9CA3E86B2296', 'UMN', 'Umuahia North', '35881A6A-2047-4DF2-80C6-97A62A3F545B', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('76701A72-F6F7-4EB0-B479-69F9334358F5', 'UMS', 'Umuahia South', '35881A6A-2047-4DF2-80C6-97A62A3F545B', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('61FEEF70-23C5-4A29-A245-D06223DE5715', 'UMU', 'Umu Nneochi', '35881A6A-2047-4DF2-80C6-97A62A3F545B', 1, 0)


    insert into States(Id, Code, [Name], CountryId, Protected, Deleted) values ('ACB46224-A719-4B54-9145-5CD8097A9A95', 'ADA', 'Adamawa', 'A164359A-585C-4EB2-A25A-205975A3AD30', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('587D96D9-EC98-47B9-BDF6-00B478188D03', 'FUF', 'Fufure', 'ACB46224-A719-4B54-9145-5CD8097A9A95', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('A819F0D4-26D5-4046-9B81-4E6D97532705', 'GAN', 'Ganye', 'ACB46224-A719-4B54-9145-5CD8097A9A95', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('D37698AD-53AC-4A1D-9D72-8E8C8029FEC3', 'GAY', 'Gayuk', 'ACB46224-A719-4B54-9145-5CD8097A9A95', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('51AEB060-801F-4313-A2CC-3C985268B5F9', 'GOM', 'Gombi', 'ACB46224-A719-4B54-9145-5CD8097A9A95', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('492F0E91-E17B-4477-AFE3-A4A8B6AACDB8', 'GRI', 'Grie', 'ACB46224-A719-4B54-9145-5CD8097A9A95', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('2E328C07-19D9-4818-A944-D037A7CC5D67', 'HON', 'Hong', 'ACB46224-A719-4B54-9145-5CD8097A9A95', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('F44E1311-AD1F-4950-9F34-D8B67FC89AD2', 'JAD', 'Jada', 'ACB46224-A719-4B54-9145-5CD8097A9A95', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('22EF4397-560C-4DCB-B37A-A39C1D1B6923', 'LAM', 'Lamurde', 'ACB46224-A719-4B54-9145-5CD8097A9A95', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('201D24D3-49A1-477B-BD66-E9BC485CBF9E', 'MAD', 'Madagali', 'ACB46224-A719-4B54-9145-5CD8097A9A95', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('23718E41-C752-444C-A364-F9B4B11ED23E', 'MAI', 'Maiha', 'ACB46224-A719-4B54-9145-5CD8097A9A95', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('639AD09B-552E-4547-B67C-B8E269347209', 'MAY', 'Mayo Belwa', 'ACB46224-A719-4B54-9145-5CD8097A9A95', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('B738D9C3-538F-4F2A-B7AF-CBA53FBB3B60', 'MIC', 'Michika', 'ACB46224-A719-4B54-9145-5CD8097A9A95', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('5392A308-F7F9-4FA9-A578-5A8135AD6A47', 'MUB', 'Mubi North', 'ACB46224-A719-4B54-9145-5CD8097A9A95', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('F64D4347-116A-4274-A2F7-1191E4994C7D', 'MUS', 'Mubi South', 'ACB46224-A719-4B54-9145-5CD8097A9A95', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('B429145A-4A53-400E-95B1-79C8D7CA164B', 'NUM', 'Numan', 'ACB46224-A719-4B54-9145-5CD8097A9A95', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('D5EBE51A-1E34-4BE3-89A5-89257790D0CE', 'SHE', 'Shelleng', 'ACB46224-A719-4B54-9145-5CD8097A9A95', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('897B1E69-EF13-4609-9BE6-A5C7791C07D3', 'SON', 'Song', 'ACB46224-A719-4B54-9145-5CD8097A9A95', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('2FCCD23B-D913-47AB-B21C-E0B95C104FDA', 'TOU', 'Toungo', 'ACB46224-A719-4B54-9145-5CD8097A9A95', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('ED3E282A-E0E8-44FF-A406-201BBAC75B3D', 'YON', 'Yola North', 'ACB46224-A719-4B54-9145-5CD8097A9A95', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('1FDB5DFA-7A8F-48A5-9702-F54B6BA7510E', 'YOS', 'Yola South', 'ACB46224-A719-4B54-9145-5CD8097A9A95', 1, 0)


    insert into States(Id, Code, [Name], CountryId, Protected, Deleted) values ('7412E075-BE9B-4586-B301-3E76A7B051A5', 'AKW', 'Akwa Ibom', 'A164359A-585C-4EB2-A25A-205975A3AD30', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('011E1C16-2C78-4642-90CB-7F6E58353B6C', 'EAS', 'Eastern Obolo', '7412E075-BE9B-4586-B301-3E76A7B051A5', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('60EB6673-FE1A-435A-94A6-540D134D1CDF', 'EKE', 'Eket', '7412E075-BE9B-4586-B301-3E76A7B051A5', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('435DEB9A-9A75-43D9-AC1C-55D22C79900F', 'ESI', 'Esit Eket', '7412E075-BE9B-4586-B301-3E76A7B051A5', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('FEBBC1F0-94AD-4A1E-A9DE-0930FF42B8A2', 'ESS', 'Essien Udim', '7412E075-BE9B-4586-B301-3E76A7B051A5', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('8C05CCDA-036F-4D50-BE88-ADFCDD242942', 'ETI', 'Etim Ekpo', '7412E075-BE9B-4586-B301-3E76A7B051A5', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('EDBD16E8-DF15-4AF6-A7A7-18C29003D331', 'ETN', 'Etinan', '7412E075-BE9B-4586-B301-3E76A7B051A5', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('01338DB9-8AFC-4438-81F5-7A122D6C8070', 'IBE', 'Ibeno', '7412E075-BE9B-4586-B301-3E76A7B051A5', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('FF1161DE-3F38-4F0D-9591-E62786947D53', 'IBS', 'Ibesikpo Asutan', '7412E075-BE9B-4586-B301-3E76A7B051A5', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('81EF0ABF-84FB-444E-ACD1-C438DCF83F4F', 'IBO', 'Ibiono-Ibom', '7412E075-BE9B-4586-B301-3E76A7B051A5', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('67D3A008-FD64-4602-8E5A-498317A29243', 'IKA', 'Ika', '7412E075-BE9B-4586-B301-3E76A7B051A5', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('ABF2D0BD-8B71-4818-838F-4C6C9CFCFE74', 'IKO', 'Ikono', '7412E075-BE9B-4586-B301-3E76A7B051A5', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('2E573999-61CB-4909-BD3A-228F252F50CC', 'IKB', 'Ikot Abasi', '7412E075-BE9B-4586-B301-3E76A7B051A5', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('844D514D-A50F-4524-95D9-C8B2465D6830', 'IKE', 'Ikot Ekpene', '7412E075-BE9B-4586-B301-3E76A7B051A5', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('5780EB97-AE17-453D-8ED2-D67D5239E9A8', 'INI', 'Ini', '7412E075-BE9B-4586-B301-3E76A7B051A5', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('BFB53377-506E-4136-8B31-2E155BB65191', 'MBO', 'Mbo', '7412E075-BE9B-4586-B301-3E76A7B051A5', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('AE656BCC-4136-4678-A9DA-88BDD885D730', 'MKP', 'Mkpat-Enin', '7412E075-BE9B-4586-B301-3E76A7B051A5', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('6E639602-AB1D-4931-9876-549657B53E93', 'NSA', 'Nsit-Atai', '7412E075-BE9B-4586-B301-3E76A7B051A5', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('7E5BEE1A-D43A-49FE-8DC6-C0AEFA333B7B', 'NSI', 'Nsit-Ibom', '7412E075-BE9B-4586-B301-3E76A7B051A5', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('75556A75-BC7F-44F4-8D98-D7E2A903A4F9', 'NSU', 'Nsit-Ubium', '7412E075-BE9B-4586-B301-3E76A7B051A5', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('5C89A917-25EE-4E17-A027-C33F5B05D484', 'OBO', 'Obot Akara', '7412E075-BE9B-4586-B301-3E76A7B051A5', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('16AB2CE2-4AC5-4E30-A981-BDC662C4C75F', 'OKO', 'Okobo', '7412E075-BE9B-4586-B301-3E76A7B051A5', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('4E1E1DE9-54C9-47C2-A198-491791575641', 'ONN', 'Onna', '7412E075-BE9B-4586-B301-3E76A7B051A5', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('A1C48684-C4F2-4B05-A7F4-8A4498AB6D99', 'ORO', 'Oron', '7412E075-BE9B-4586-B301-3E76A7B051A5', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('85DBB879-5893-421F-8973-6C7A329AB402', 'ORU', 'Oruk Anam', '7412E075-BE9B-4586-B301-3E76A7B051A5', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('2FFA795A-784D-4E7C-A0EA-E1D0BC9862FC', 'UDU', 'Udung-Uko', '7412E075-BE9B-4586-B301-3E76A7B051A5', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('A9672202-2C13-476A-ABA3-615F454C7873', 'UKA', 'Ukanafun', '7412E075-BE9B-4586-B301-3E76A7B051A5', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('1847F7F2-713A-4945-9B47-BF4FD2DAFC72', 'URU', 'Uruan', '7412E075-BE9B-4586-B301-3E76A7B051A5', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('BDFE3B7E-3EB4-427D-8BD6-DCC0C8D473E2', 'URO', 'Urue-Offong/Oruko', '7412E075-BE9B-4586-B301-3E76A7B051A5', 1, 0)
    		insert into Areas(Id, Code, [Name], StateId, Protected, Deleted) values ('8F1CD849-3747-400E-80BD-C37C3D1A07EE', 'UYO', 'Uyo', '7412E075-BE9B-4586-B301-3E76A7B051A5', 1, 0)

    insert into States(Id, Code, [Name], CountryId, Protected, Deleted) values ('910BFD00-024E-438E-88F3-221348D56250', 'LAG', 'Lagos', 'A164359A-585C-4EB2-A25A-205975A3AD30', 1, 0)
    	insert into Cities(Id, Code, [Name], StateId, Protected, Deleted) values ('078D0FBF-B3EC-42E1-AA9D-C2DE808E9F85', 'LAC', 'Lagos', '910BFD00-024E-438E-88F3-221348D56250', 1, 0)

    insert into IncidenceStatuses(Id, [Name], [Description], Protected, Deleted) values ('33a33655-4eea-44f8-b89c-132c37ec8cd2', 'New', 'New', 1, 0)
    insert into IncidenceStatuses(Id, [Name], [Description], Protected, Deleted) values ('a64769a0-ce38-4414-afdb-c6bfac9056a1', 'Open', 'Open', 1, 0)
    insert into IncidenceStatuses(Id, [Name], [Description], Protected, Deleted) values ('0839bc87-7d23-431d-86c4-3ca1e4f190ef', 'Closed', 'Closed', 1, 0)
    insert into IncidenceStatuses(Id, [Name], [Description], Protected, Deleted) values ('f59fe8e6-646a-4b04-7fbc-08d702eeea0d', 'Re-Opened', 'Re-Opened', 1, 0)
    insert into IncidenceStatuses(Id, [Name], [Description], Protected, Deleted) values ('53986162-1dae-45f7-8771-08d702ea4b5c', 'Under Review', 'Under Review', 1, 0)
    insert into IncidenceStatuses(Id, [Name], [Description], Protected, Deleted) values ('A12FAFB4-FA09-4188-8A54-10551FBE05C0', 'Resolved', 'Resolved', 1, 0)
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190620175321_InitialCreate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190620175321_InitialCreate', N'2.2.6-servicing-10079');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190625072112_DepartmentExtension')
BEGIN
    ALTER TABLE [OrganizationDepartments] ADD [Code] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190625072112_DepartmentExtension')
BEGIN
    ALTER TABLE [OrganizationDepartments] ADD [Description] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190625072112_DepartmentExtension')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190625072112_DepartmentExtension', N'2.2.6-servicing-10079');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190625132903_AddChangeTrackingInModels')
BEGIN
    ALTER TABLE [UserDeployment] ADD [CreatedByUserId] uniqueidentifier NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190625132903_AddChangeTrackingInModels')
BEGIN
    ALTER TABLE [UserDeployment] ADD [DateCreated] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190625132903_AddChangeTrackingInModels')
BEGIN
    ALTER TABLE [UserDeployment] ADD [DateEdited] datetime2 NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190625132903_AddChangeTrackingInModels')
BEGIN
    ALTER TABLE [UserDeployment] ADD [Deleted] bit NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190625132903_AddChangeTrackingInModels')
BEGIN
    ALTER TABLE [UserDeployment] ADD [EditedByUserId] uniqueidentifier NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190625132903_AddChangeTrackingInModels')
BEGIN
    ALTER TABLE [UserDeployment] ADD [Protected] bit NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190625132903_AddChangeTrackingInModels')
BEGIN
    ALTER TABLE [Organizations] ADD [Deleted] bit NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190625132903_AddChangeTrackingInModels')
BEGIN
    ALTER TABLE [Organizations] ADD [Protected] bit NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190625132903_AddChangeTrackingInModels')
BEGIN
    ALTER TABLE [OrganizationDepartments] ADD [Deleted] bit NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190625132903_AddChangeTrackingInModels')
BEGIN
    ALTER TABLE [OrganizationDepartments] ADD [Protected] bit NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190625132903_AddChangeTrackingInModels')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Incidences]') AND [c].[name] = N'DateCreated');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Incidences] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [Incidences] ALTER COLUMN [DateCreated] datetime2 NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190625132903_AddChangeTrackingInModels')
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Incidences]') AND [c].[name] = N'CreatedByUserId');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Incidences] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [Incidences] ALTER COLUMN [CreatedByUserId] uniqueidentifier NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190625132903_AddChangeTrackingInModels')
BEGIN
    ALTER TABLE [Incidences] ADD [Deleted] bit NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190625132903_AddChangeTrackingInModels')
BEGIN
    ALTER TABLE [Incidences] ADD [Protected] bit NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190625132903_AddChangeTrackingInModels')
BEGIN
    ALTER TABLE [AspNetUsers] ADD [CreatedByUserId] uniqueidentifier NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190625132903_AddChangeTrackingInModels')
BEGIN
    ALTER TABLE [AspNetUsers] ADD [DateCreated] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190625132903_AddChangeTrackingInModels')
BEGIN
    ALTER TABLE [AspNetUsers] ADD [DateEdited] datetime2 NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190625132903_AddChangeTrackingInModels')
BEGIN
    ALTER TABLE [AspNetUsers] ADD [EditedByUserId] uniqueidentifier NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190625132903_AddChangeTrackingInModels')
BEGIN
    ALTER TABLE [AspNetUsers] ADD [Protected] bit NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190625132903_AddChangeTrackingInModels')
BEGIN
    CREATE INDEX [IX_UserDeployment_CreatedByUserId] ON [UserDeployment] ([CreatedByUserId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190625132903_AddChangeTrackingInModels')
BEGIN
    CREATE INDEX [IX_UserDeployment_EditedByUserId] ON [UserDeployment] ([EditedByUserId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190625132903_AddChangeTrackingInModels')
BEGIN
    CREATE INDEX [IX_Incidences_CreatedByUserId] ON [Incidences] ([CreatedByUserId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190625132903_AddChangeTrackingInModels')
BEGIN
    CREATE INDEX [IX_Incidences_EditedByUserId] ON [Incidences] ([EditedByUserId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190625132903_AddChangeTrackingInModels')
BEGIN
    ALTER TABLE [Incidences] ADD CONSTRAINT [FK_Incidences_AspNetUsers_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190625132903_AddChangeTrackingInModels')
BEGIN
    ALTER TABLE [Incidences] ADD CONSTRAINT [FK_Incidences_AspNetUsers_EditedByUserId] FOREIGN KEY ([EditedByUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190625132903_AddChangeTrackingInModels')
BEGIN
    ALTER TABLE [UserDeployment] ADD CONSTRAINT [FK_UserDeployment_AspNetUsers_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190625132903_AddChangeTrackingInModels')
BEGIN
    ALTER TABLE [UserDeployment] ADD CONSTRAINT [FK_UserDeployment_AspNetUsers_EditedByUserId] FOREIGN KEY ([EditedByUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190625132903_AddChangeTrackingInModels')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190625132903_AddChangeTrackingInModels', N'2.2.6-servicing-10079');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190716124304_OrganizationUpdate')
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Organizations]') AND [c].[name] = N'DateofEst');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Organizations] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [Organizations] ALTER COLUMN [DateofEst] datetime2 NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190716124304_OrganizationUpdate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190716124304_OrganizationUpdate', N'2.2.6-servicing-10079');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190716131200_OrganizationUpdate2')
BEGIN
    Update Organizations set MaxFileSize=0 where MaxFileSize<0 or MaxFileSize is NULL
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190716131200_OrganizationUpdate2')
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Organizations]') AND [c].[name] = N'MaxFileSize');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Organizations] DROP CONSTRAINT [' + @var3 + '];');
    ALTER TABLE [Organizations] ALTER COLUMN [MaxFileSize] int NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190716131200_OrganizationUpdate2')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190716131200_OrganizationUpdate2', N'2.2.6-servicing-10079');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190718072446_UserModelUpdate')
BEGIN
    DECLARE @var4 sysname;
    SELECT @var4 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'LastActive');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var4 + '];');
    ALTER TABLE [AspNetUsers] ALTER COLUMN [LastActive] datetime2 NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190718072446_UserModelUpdate')
BEGIN
    DECLARE @var5 sysname;
    SELECT @var5 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'DateOfBirth');
    IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var5 + '];');
    ALTER TABLE [AspNetUsers] ALTER COLUMN [DateOfBirth] datetime2 NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190718072446_UserModelUpdate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190718072446_UserModelUpdate', N'2.2.6-servicing-10079');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190718144014_UserDeploymentUpdate')
BEGIN
    DECLARE @var6 sysname;
    SELECT @var6 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserDeployment]') AND [c].[name] = N'DateOfSignOff');
    IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [UserDeployment] DROP CONSTRAINT [' + @var6 + '];');
    ALTER TABLE [UserDeployment] ALTER COLUMN [DateOfSignOff] datetime2 NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190718144014_UserDeploymentUpdate')
BEGIN
    ALTER TABLE [UserDeployment] ADD [OrganizationId] uniqueidentifier NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190718144014_UserDeploymentUpdate')
BEGIN
    ALTER TABLE [AspNetUsers] ADD [OrganizationId] uniqueidentifier NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190718144014_UserDeploymentUpdate')
BEGIN
    CREATE INDEX [IX_UserDeployment_OrganizationId] ON [UserDeployment] ([OrganizationId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190718144014_UserDeploymentUpdate')
BEGIN
    CREATE INDEX [IX_AspNetUsers_OrganizationId] ON [AspNetUsers] ([OrganizationId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190718144014_UserDeploymentUpdate')
BEGIN
    ALTER TABLE [AspNetUsers] ADD CONSTRAINT [FK_AspNetUsers_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [Organizations] ([Id]) ON DELETE NO ACTION;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190718144014_UserDeploymentUpdate')
BEGIN
    ALTER TABLE [UserDeployment] ADD CONSTRAINT [FK_UserDeployment_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [Organizations] ([Id]) ON DELETE NO ACTION;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190718144014_UserDeploymentUpdate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190718144014_UserDeploymentUpdate', N'2.2.6-servicing-10079');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190802132657_IncidenceOrganizationUpdate')
BEGIN
    ALTER TABLE [Incidences] ADD [DepartmentId] uniqueidentifier NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190802132657_IncidenceOrganizationUpdate')
BEGIN
    CREATE INDEX [IX_Incidences_DepartmentId] ON [Incidences] ([DepartmentId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190802132657_IncidenceOrganizationUpdate')
BEGIN
    ALTER TABLE [Incidences] ADD CONSTRAINT [FK_Incidences_OrganizationDepartments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [OrganizationDepartments] ([Id]) ON DELETE NO ACTION;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190802132657_IncidenceOrganizationUpdate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190802132657_IncidenceOrganizationUpdate', N'2.2.6-servicing-10079');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190805203852_EmployeeListDropDownField')
BEGIN
    ALTER TABLE [AspNetUsers] ADD [toStringField] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190805203852_EmployeeListDropDownField')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190805203852_EmployeeListDropDownField', N'2.2.6-servicing-10079');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190806184938_IncidenceFieldsUpdate')
BEGIN
    ALTER TABLE [Incidences] DROP CONSTRAINT [FK_Incidences_IncidenceStatuses_IncidenceStatusId];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190806184938_IncidenceFieldsUpdate')
BEGIN
    ALTER TABLE [Incidences] DROP CONSTRAINT [FK_Incidences_IncidenceTypes_IncidenceTypeId];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190806184938_IncidenceFieldsUpdate')
BEGIN
    DECLARE @var7 sysname;
    SELECT @var7 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Incidences]') AND [c].[name] = N'IncidenceTypeId');
    IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Incidences] DROP CONSTRAINT [' + @var7 + '];');
    ALTER TABLE [Incidences] ALTER COLUMN [IncidenceTypeId] uniqueidentifier NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190806184938_IncidenceFieldsUpdate')
BEGIN
    DECLARE @var8 sysname;
    SELECT @var8 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Incidences]') AND [c].[name] = N'IncidenceStatusId');
    IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [Incidences] DROP CONSTRAINT [' + @var8 + '];');
    ALTER TABLE [Incidences] ALTER COLUMN [IncidenceStatusId] uniqueidentifier NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190806184938_IncidenceFieldsUpdate')
BEGIN
    ALTER TABLE [Incidences] ADD CONSTRAINT [FK_Incidences_IncidenceStatuses_IncidenceStatusId] FOREIGN KEY ([IncidenceStatusId]) REFERENCES [IncidenceStatuses] ([Id]) ON DELETE NO ACTION;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190806184938_IncidenceFieldsUpdate')
BEGIN
    ALTER TABLE [Incidences] ADD CONSTRAINT [FK_Incidences_IncidenceTypes_IncidenceTypeId] FOREIGN KEY ([IncidenceTypeId]) REFERENCES [IncidenceTypes] ([Id]) ON DELETE NO ACTION;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190806184938_IncidenceFieldsUpdate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190806184938_IncidenceFieldsUpdate', N'2.2.6-servicing-10079');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190808192801_FileUploadChannels')
BEGIN
    EXEC sp_rename N'[Photos].[Url]', N'Uri', N'COLUMN';
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190808192801_FileUploadChannels')
BEGIN
    DECLARE @var9 sysname;
    SELECT @var9 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SystemSettings]') AND [c].[name] = N'PageSize');
    IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [SystemSettings] DROP CONSTRAINT [' + @var9 + '];');
    ALTER TABLE [SystemSettings] ALTER COLUMN [PageSize] int NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190808192801_FileUploadChannels')
BEGIN
    DECLARE @var10 sysname;
    SELECT @var10 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SystemSettings]') AND [c].[name] = N'MaxFileSize');
    IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [SystemSettings] DROP CONSTRAINT [' + @var10 + '];');
    ALTER TABLE [SystemSettings] ALTER COLUMN [MaxFileSize] real NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190808192801_FileUploadChannels')
BEGIN
    ALTER TABLE [Photos] ADD [DateEdited] datetime2 NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190808192801_FileUploadChannels')
BEGIN
    ALTER TABLE [Photos] ADD [DateUploaded] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190808192801_FileUploadChannels')
BEGIN
    ALTER TABLE [Photos] ADD [FileUploadChannel] int NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190808192801_FileUploadChannels')
BEGIN
    DECLARE @var11 sysname;
    SELECT @var11 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Organizations]') AND [c].[name] = N'PageSize');
    IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [Organizations] DROP CONSTRAINT [' + @var11 + '];');
    ALTER TABLE [Organizations] ALTER COLUMN [PageSize] int NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190808192801_FileUploadChannels')
BEGIN
    DECLARE @var12 sysname;
    SELECT @var12 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Organizations]') AND [c].[name] = N'MaxFileSize');
    IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [Organizations] DROP CONSTRAINT [' + @var12 + '];');
    ALTER TABLE [Organizations] ALTER COLUMN [MaxFileSize] real NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190808192801_FileUploadChannels')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190808192801_FileUploadChannels', N'2.2.6-servicing-10079');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190817134437_MediaUpdate')
BEGIN
    EXEC sp_rename N'[Photos].[Uri]', N'RemoteUri', N'COLUMN';
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190817134437_MediaUpdate')
BEGIN
    ALTER TABLE [Photos] ADD [Description] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190817134437_MediaUpdate')
BEGIN
    ALTER TABLE [Photos] ADD [IsVideo] bit NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190817134437_MediaUpdate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190817134437_MediaUpdate', N'2.2.6-servicing-10079');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190818110909_MediaGalleryUpdate')
BEGIN
    DROP TABLE [Photos];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190818110909_MediaGalleryUpdate')
BEGIN
    DROP TABLE [Videos];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190818110909_MediaGalleryUpdate')
BEGIN
    EXEC sp_rename N'[SystemSettings].[MaxFileSize]', N'MaxVideoFileSize', N'COLUMN';
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190818110909_MediaGalleryUpdate')
BEGIN
    EXEC sp_rename N'[SystemSettings].[AcceptedFileTypes]', N'AcceptedVideoFileTypes', N'COLUMN';
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190818110909_MediaGalleryUpdate')
BEGIN
    EXEC sp_rename N'[Organizations].[MaxFileSize]', N'MaxVideoFileSize', N'COLUMN';
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190818110909_MediaGalleryUpdate')
BEGIN
    EXEC sp_rename N'[Organizations].[AcceptedFileTypes]', N'AcceptedVideoFileTypes', N'COLUMN';
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190818110909_MediaGalleryUpdate')
BEGIN
    ALTER TABLE [SystemSettings] ADD [AcceptedImageFileTypes] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190818110909_MediaGalleryUpdate')
BEGIN
    ALTER TABLE [SystemSettings] ADD [MaxImageFileSize] real NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190818110909_MediaGalleryUpdate')
BEGIN
    ALTER TABLE [Organizations] ADD [AcceptedImageFileTypes] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190818110909_MediaGalleryUpdate')
BEGIN
    ALTER TABLE [Organizations] ADD [MaxImageFileSize] real NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190818110909_MediaGalleryUpdate')
BEGIN
    CREATE TABLE [Gallery] (
        [Id] uniqueidentifier NOT NULL,
        [FileName] nvarchar(255) NOT NULL,
        [RemoteUri] nvarchar(512) NULL,
        [DateUploaded] datetime2 NOT NULL,
        [DateEdited] datetime2 NULL,
        [IncidenceId] uniqueidentifier NULL,
        [FileUploadChannel] int NULL,
        [Description] nvarchar(max) NULL,
        [IsVideo] bit NOT NULL,
        CONSTRAINT [PK_Gallery] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Gallery_Incidences_IncidenceId] FOREIGN KEY ([IncidenceId]) REFERENCES [Incidences] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190818110909_MediaGalleryUpdate')
BEGIN
    CREATE INDEX [IX_Gallery_IncidenceId] ON [Gallery] ([IncidenceId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190818110909_MediaGalleryUpdate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190818110909_MediaGalleryUpdate', N'2.2.6-servicing-10079');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190821193916_NotificationBySMSUpdate')
BEGIN
    ALTER TABLE [SystemSettings] ADD [EmailSenderName] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190821193916_NotificationBySMSUpdate')
BEGIN
    ALTER TABLE [SystemSettings] ADD [EmailSenderPassword] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190821193916_NotificationBySMSUpdate')
BEGIN
    ALTER TABLE [SystemSettings] ADD [EmailSendersEmail] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190821193916_NotificationBySMSUpdate')
BEGIN
    ALTER TABLE [SystemSettings] ADD [SendSMS] bit NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190821193916_NotificationBySMSUpdate')
BEGIN
    ALTER TABLE [SystemSettings] ADD [SmsServiceProvider] int NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190821193916_NotificationBySMSUpdate')
BEGIN
    ALTER TABLE [Organizations] ADD [EmailSenderName] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190821193916_NotificationBySMSUpdate')
BEGIN
    ALTER TABLE [Organizations] ADD [EmailSenderPassword] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190821193916_NotificationBySMSUpdate')
BEGIN
    ALTER TABLE [Organizations] ADD [EmailSendersEmail] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190821193916_NotificationBySMSUpdate')
BEGIN
    ALTER TABLE [Organizations] ADD [SendSMS] bit NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190821193916_NotificationBySMSUpdate')
BEGIN
    ALTER TABLE [Organizations] ADD [SmsServiceProvider] int NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190821193916_NotificationBySMSUpdate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190821193916_NotificationBySMSUpdate', N'2.2.6-servicing-10079');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190822184822_UserDeploymentDatesIssue')
BEGIN
    DECLARE @var13 sysname;
    SELECT @var13 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserDeployment]') AND [c].[name] = N'DateOfDeployment');
    IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [UserDeployment] DROP CONSTRAINT [' + @var13 + '];');
    ALTER TABLE [UserDeployment] ALTER COLUMN [DateOfDeployment] datetime2 NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190822184822_UserDeploymentDatesIssue')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190822184822_UserDeploymentDatesIssue', N'2.2.6-servicing-10079');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190828114255_IncidenceTypeDepartmentMapping')
BEGIN
    CREATE TABLE [IncidenceTypeDepartmentMappings] (
        [Id] uniqueidentifier NOT NULL,
        [IncidenceTypeId] uniqueidentifier NULL,
        [DepartmentId] uniqueidentifier NULL,
        CONSTRAINT [PK_IncidenceTypeDepartmentMappings] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_IncidenceTypeDepartmentMappings_OrganizationDepartments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [OrganizationDepartments] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_IncidenceTypeDepartmentMappings_IncidenceTypes_IncidenceTypeId] FOREIGN KEY ([IncidenceTypeId]) REFERENCES [IncidenceTypes] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190828114255_IncidenceTypeDepartmentMapping')
BEGIN
    CREATE INDEX [IX_IncidenceTypeDepartmentMappings_DepartmentId] ON [IncidenceTypeDepartmentMappings] ([DepartmentId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190828114255_IncidenceTypeDepartmentMapping')
BEGIN
    CREATE INDEX [IX_IncidenceTypeDepartmentMappings_IncidenceTypeId] ON [IncidenceTypeDepartmentMappings] ([IncidenceTypeId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190828114255_IncidenceTypeDepartmentMapping')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190828114255_IncidenceTypeDepartmentMapping', N'2.2.6-servicing-10079');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190828122758_DepartmentNotificationUpdate')
BEGIN
    ALTER TABLE [OrganizationDepartments] ADD [AcceptedImageFileTypes] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190828122758_DepartmentNotificationUpdate')
BEGIN
    ALTER TABLE [OrganizationDepartments] ADD [AcceptedVideoFileTypes] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190828122758_DepartmentNotificationUpdate')
BEGIN
    ALTER TABLE [OrganizationDepartments] ADD [Email1] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190828122758_DepartmentNotificationUpdate')
BEGIN
    ALTER TABLE [OrganizationDepartments] ADD [Email2] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190828122758_DepartmentNotificationUpdate')
BEGIN
    ALTER TABLE [OrganizationDepartments] ADD [EmailRecipientAddresses] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190828122758_DepartmentNotificationUpdate')
BEGIN
    ALTER TABLE [OrganizationDepartments] ADD [EmailSenderName] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190828122758_DepartmentNotificationUpdate')
BEGIN
    ALTER TABLE [OrganizationDepartments] ADD [EmailSenderPassword] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190828122758_DepartmentNotificationUpdate')
BEGIN
    ALTER TABLE [OrganizationDepartments] ADD [EmailSendersEmail] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190828122758_DepartmentNotificationUpdate')
BEGIN
    ALTER TABLE [OrganizationDepartments] ADD [HostName] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190828122758_DepartmentNotificationUpdate')
BEGIN
    ALTER TABLE [OrganizationDepartments] ADD [MaxImageFileSize] real NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190828122758_DepartmentNotificationUpdate')
BEGIN
    ALTER TABLE [OrganizationDepartments] ADD [MaxVideoFileSize] real NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190828122758_DepartmentNotificationUpdate')
BEGIN
    ALTER TABLE [OrganizationDepartments] ADD [PageSize] int NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190828122758_DepartmentNotificationUpdate')
BEGIN
    ALTER TABLE [OrganizationDepartments] ADD [Port] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190828122758_DepartmentNotificationUpdate')
BEGIN
    ALTER TABLE [OrganizationDepartments] ADD [SMSRecipientNumbers] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190828122758_DepartmentNotificationUpdate')
BEGIN
    ALTER TABLE [OrganizationDepartments] ADD [SendSMS] bit NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190828122758_DepartmentNotificationUpdate')
BEGIN
    ALTER TABLE [OrganizationDepartments] ADD [SmsServiceProvider] int NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190828122758_DepartmentNotificationUpdate')
BEGIN
    ALTER TABLE [OrganizationDepartments] ADD [UseSsl] bit NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190828122758_DepartmentNotificationUpdate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190828122758_DepartmentNotificationUpdate', N'2.2.6-servicing-10079');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190830112838_SystemSettingsOrganizationNotificationUpdate')
BEGIN
    ALTER TABLE [SystemSettings] ADD [EmailRecipientAddresses] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190830112838_SystemSettingsOrganizationNotificationUpdate')
BEGIN
    ALTER TABLE [SystemSettings] ADD [SMSRecipientNumbers] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190830112838_SystemSettingsOrganizationNotificationUpdate')
BEGIN
    ALTER TABLE [SystemSettings] ADD [SMSSenderName] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190830112838_SystemSettingsOrganizationNotificationUpdate')
BEGIN
    ALTER TABLE [Organizations] ADD [EmailRecipientAddresses] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190830112838_SystemSettingsOrganizationNotificationUpdate')
BEGIN
    ALTER TABLE [Organizations] ADD [SMSRecipientNumbers] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190830112838_SystemSettingsOrganizationNotificationUpdate')
BEGIN
    ALTER TABLE [Organizations] ADD [SMSSenderName] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190830112838_SystemSettingsOrganizationNotificationUpdate')
BEGIN
    ALTER TABLE [OrganizationDepartments] ADD [SMSSenderName] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190830112838_SystemSettingsOrganizationNotificationUpdate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190830112838_SystemSettingsOrganizationNotificationUpdate', N'2.2.6-servicing-10079');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190901104916_EmailNotificationPortChange')
BEGIN
    DECLARE @var14 sysname;
    SELECT @var14 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SystemSettings]') AND [c].[name] = N'Port');
    IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [SystemSettings] DROP CONSTRAINT [' + @var14 + '];');
    ALTER TABLE [SystemSettings] ALTER COLUMN [Port] int NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190901104916_EmailNotificationPortChange')
BEGIN
    DECLARE @var15 sysname;
    SELECT @var15 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Organizations]') AND [c].[name] = N'Port');
    IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [Organizations] DROP CONSTRAINT [' + @var15 + '];');
    ALTER TABLE [Organizations] ALTER COLUMN [Port] int NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190901104916_EmailNotificationPortChange')
BEGIN
    DECLARE @var16 sysname;
    SELECT @var16 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[OrganizationDepartments]') AND [c].[name] = N'Port');
    IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [OrganizationDepartments] DROP CONSTRAINT [' + @var16 + '];');
    ALTER TABLE [OrganizationDepartments] ALTER COLUMN [Port] int NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190901104916_EmailNotificationPortChange')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190901104916_EmailNotificationPortChange', N'2.2.6-servicing-10079');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190903154643_OrganizationMultitenancy')
BEGIN
    ALTER TABLE [Incidences] DROP CONSTRAINT [FK_Incidences_OrganizationDepartments_DepartmentId];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190903154643_OrganizationMultitenancy')
BEGIN
    EXEC sp_rename N'[Incidences].[DepartmentId]', N'AssignedOrganizationId', N'COLUMN';
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190903154643_OrganizationMultitenancy')
BEGIN
    EXEC sp_rename N'[Incidences].[IX_Incidences_DepartmentId]', N'IX_Incidences_AssignedOrganizationId', N'INDEX';
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190903154643_OrganizationMultitenancy')
BEGIN
    ALTER TABLE [UserDeployment] ADD [UserOrganizationId] uniqueidentifier NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190903154643_OrganizationMultitenancy')
BEGIN
    ALTER TABLE [IncidenceTypes] ADD [OrganizationId] uniqueidentifier NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190903154643_OrganizationMultitenancy')
BEGIN
    ALTER TABLE [IncidenceTypeDepartmentMappings] ADD [OrganizationId] uniqueidentifier NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190903154643_OrganizationMultitenancy')
BEGIN
    ALTER TABLE [IncidenceStatuses] ADD [OrganizationId] uniqueidentifier NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190903154643_OrganizationMultitenancy')
BEGIN
    ALTER TABLE [Incidences] ADD [AssignedDepartmentId] uniqueidentifier NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190903154643_OrganizationMultitenancy')
BEGIN
    ALTER TABLE [Gallery] ADD [OrganizationId] uniqueidentifier NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190903154643_OrganizationMultitenancy')
BEGIN
    ALTER TABLE [AuditTrails] ADD [OrganizationId] uniqueidentifier NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190903154643_OrganizationMultitenancy')
BEGIN
    CREATE INDEX [IX_UserDeployment_UserOrganizationId] ON [UserDeployment] ([UserOrganizationId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190903154643_OrganizationMultitenancy')
BEGIN
    CREATE INDEX [IX_IncidenceTypes_OrganizationId] ON [IncidenceTypes] ([OrganizationId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190903154643_OrganizationMultitenancy')
BEGIN
    CREATE INDEX [IX_IncidenceTypeDepartmentMappings_OrganizationId] ON [IncidenceTypeDepartmentMappings] ([OrganizationId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190903154643_OrganizationMultitenancy')
BEGIN
    CREATE INDEX [IX_IncidenceStatuses_OrganizationId] ON [IncidenceStatuses] ([OrganizationId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190903154643_OrganizationMultitenancy')
BEGIN
    CREATE INDEX [IX_Incidences_AssignedDepartmentId] ON [Incidences] ([AssignedDepartmentId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190903154643_OrganizationMultitenancy')
BEGIN
    CREATE INDEX [IX_Gallery_OrganizationId] ON [Gallery] ([OrganizationId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190903154643_OrganizationMultitenancy')
BEGIN
    CREATE INDEX [IX_AuditTrails_OrganizationId] ON [AuditTrails] ([OrganizationId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190903154643_OrganizationMultitenancy')
BEGIN
    ALTER TABLE [AuditTrails] ADD CONSTRAINT [FK_AuditTrails_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [Organizations] ([Id]) ON DELETE NO ACTION;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190903154643_OrganizationMultitenancy')
BEGIN
    ALTER TABLE [Gallery] ADD CONSTRAINT [FK_Gallery_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [Organizations] ([Id]) ON DELETE NO ACTION;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190903154643_OrganizationMultitenancy')
BEGIN
    ALTER TABLE [Incidences] ADD CONSTRAINT [FK_Incidences_OrganizationDepartments_AssignedDepartmentId] FOREIGN KEY ([AssignedDepartmentId]) REFERENCES [OrganizationDepartments] ([Id]) ON DELETE NO ACTION;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190903154643_OrganizationMultitenancy')
BEGIN
    ALTER TABLE [Incidences] ADD CONSTRAINT [FK_Incidences_Organizations_AssignedOrganizationId] FOREIGN KEY ([AssignedOrganizationId]) REFERENCES [Organizations] ([Id]) ON DELETE NO ACTION;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190903154643_OrganizationMultitenancy')
BEGIN
    ALTER TABLE [IncidenceStatuses] ADD CONSTRAINT [FK_IncidenceStatuses_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [Organizations] ([Id]) ON DELETE NO ACTION;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190903154643_OrganizationMultitenancy')
BEGIN
    ALTER TABLE [IncidenceTypeDepartmentMappings] ADD CONSTRAINT [FK_IncidenceTypeDepartmentMappings_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [Organizations] ([Id]) ON DELETE NO ACTION;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190903154643_OrganizationMultitenancy')
BEGIN
    ALTER TABLE [IncidenceTypes] ADD CONSTRAINT [FK_IncidenceTypes_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [Organizations] ([Id]) ON DELETE NO ACTION;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190903154643_OrganizationMultitenancy')
BEGIN
    ALTER TABLE [UserDeployment] ADD CONSTRAINT [FK_UserDeployment_Organizations_UserOrganizationId] FOREIGN KEY ([UserOrganizationId]) REFERENCES [Organizations] ([Id]) ON DELETE NO ACTION;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190903154643_OrganizationMultitenancy')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190903154643_OrganizationMultitenancy', N'2.2.6-servicing-10079');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190905181542_IncidenceFieldUpdate')
BEGIN
    ALTER TABLE [Incidences] ADD [address] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190905181542_IncidenceFieldUpdate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190905181542_IncidenceFieldUpdate', N'2.2.6-servicing-10079');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190911143249_IncidenceStatusUpdate')
BEGIN
    ALTER TABLE [IncidenceStatuses] DROP CONSTRAINT [FK_IncidenceStatuses_Organizations_OrganizationId];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190911143249_IncidenceStatusUpdate')
BEGIN
    DROP INDEX [IX_IncidenceStatuses_OrganizationId] ON [IncidenceStatuses];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190911143249_IncidenceStatusUpdate')
BEGIN
    DECLARE @var17 sysname;
    SELECT @var17 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[IncidenceStatuses]') AND [c].[name] = N'OrganizationId');
    IF @var17 IS NOT NULL EXEC(N'ALTER TABLE [IncidenceStatuses] DROP CONSTRAINT [' + @var17 + '];');
    ALTER TABLE [IncidenceStatuses] DROP COLUMN [OrganizationId];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190911143249_IncidenceStatusUpdate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190911143249_IncidenceStatusUpdate', N'2.2.6-servicing-10079');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190912192859_EmailSettingsUpdate')
BEGIN
    ALTER TABLE [Organizations] ADD [ActivateEmailSenderSettings] bit NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190912192859_EmailSettingsUpdate')
BEGIN
    ALTER TABLE [Organizations] ADD [ActivateFileSettings] bit NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190912192859_EmailSettingsUpdate')
BEGIN
    ALTER TABLE [OrganizationDepartments] ADD [ActivateEmailSenderSettings] bit NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190912192859_EmailSettingsUpdate')
BEGIN
    ALTER TABLE [OrganizationDepartments] ADD [ActivateFileSettings] bit NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190912192859_EmailSettingsUpdate')
BEGIN
    ALTER TABLE [OrganizationDepartments] ADD [Phone1] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190912192859_EmailSettingsUpdate')
BEGIN
    ALTER TABLE [OrganizationDepartments] ADD [Phone2] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190912192859_EmailSettingsUpdate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190912192859_EmailSettingsUpdate', N'2.2.6-servicing-10079');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190914193533_UserDeploymentDepartmentUpdate')
BEGIN
    ALTER TABLE [AspNetUsers] DROP CONSTRAINT [FK_AspNetUsers_OrganizationDepartments_DepartmentId];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190914193533_UserDeploymentDepartmentUpdate')
BEGIN
    ALTER TABLE [UserDeployment] DROP CONSTRAINT [FK_UserDeployment_Organizations_OrganizationId];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190914193533_UserDeploymentDepartmentUpdate')
BEGIN
    ALTER TABLE [UserDeployment] DROP CONSTRAINT [FK_UserDeployment_Organizations_UserOrganizationId];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190914193533_UserDeploymentDepartmentUpdate')
BEGIN
    DROP INDEX [IX_UserDeployment_OrganizationId] ON [UserDeployment];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190914193533_UserDeploymentDepartmentUpdate')
BEGIN
    DROP INDEX [IX_UserDeployment_UserOrganizationId] ON [UserDeployment];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190914193533_UserDeploymentDepartmentUpdate')
BEGIN
    DECLARE @var18 sysname;
    SELECT @var18 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserDeployment]') AND [c].[name] = N'OrganizationId');
    IF @var18 IS NOT NULL EXEC(N'ALTER TABLE [UserDeployment] DROP CONSTRAINT [' + @var18 + '];');
    ALTER TABLE [UserDeployment] DROP COLUMN [OrganizationId];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190914193533_UserDeploymentDepartmentUpdate')
BEGIN
    DECLARE @var19 sysname;
    SELECT @var19 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserDeployment]') AND [c].[name] = N'UserOrganizationId');
    IF @var19 IS NOT NULL EXEC(N'ALTER TABLE [UserDeployment] DROP CONSTRAINT [' + @var19 + '];');
    ALTER TABLE [UserDeployment] DROP COLUMN [UserOrganizationId];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190914193533_UserDeploymentDepartmentUpdate')
BEGIN
    EXEC sp_rename N'[AspNetUsers].[DepartmentId]', N'OrganizationDepartmentId', N'COLUMN';
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190914193533_UserDeploymentDepartmentUpdate')
BEGIN
    EXEC sp_rename N'[AspNetUsers].[IX_AspNetUsers_DepartmentId]', N'IX_AspNetUsers_OrganizationDepartmentId', N'INDEX';
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190914193533_UserDeploymentDepartmentUpdate')
BEGIN
    ALTER TABLE [AspNetUsers] ADD CONSTRAINT [FK_AspNetUsers_OrganizationDepartments_OrganizationDepartmentId] FOREIGN KEY ([OrganizationDepartmentId]) REFERENCES [OrganizationDepartments] ([Id]) ON DELETE NO ACTION;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190914193533_UserDeploymentDepartmentUpdate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190914193533_UserDeploymentDepartmentUpdate', N'2.2.6-servicing-10079');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190917092557_MediaUserProfilePhoto')
BEGIN
    ALTER TABLE [Gallery] ADD [UserId] uniqueidentifier NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190917092557_MediaUserProfilePhoto')
BEGIN
    CREATE INDEX [IX_Gallery_UserId] ON [Gallery] ([UserId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190917092557_MediaUserProfilePhoto')
BEGIN
    ALTER TABLE [Gallery] ADD CONSTRAINT [FK_Gallery_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190917092557_MediaUserProfilePhoto')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190917092557_MediaUserProfilePhoto', N'2.2.6-servicing-10079');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190917115315_SystemSettingsCleanUp')
BEGIN
    ALTER TABLE [Incidences] DROP CONSTRAINT [FK_Incidences_SystemSettings_SystemSettingId];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190917115315_SystemSettingsCleanUp')
BEGIN
    ALTER TABLE [OrganizationDepartments] DROP CONSTRAINT [FK_OrganizationDepartments_SystemSettings_SystemSettingId];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190917115315_SystemSettingsCleanUp')
BEGIN
    DROP INDEX [IX_OrganizationDepartments_SystemSettingId] ON [OrganizationDepartments];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190917115315_SystemSettingsCleanUp')
BEGIN
    DROP INDEX [IX_Incidences_SystemSettingId] ON [Incidences];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190917115315_SystemSettingsCleanUp')
BEGIN
    DECLARE @var20 sysname;
    SELECT @var20 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[OrganizationDepartments]') AND [c].[name] = N'SystemSettingId');
    IF @var20 IS NOT NULL EXEC(N'ALTER TABLE [OrganizationDepartments] DROP CONSTRAINT [' + @var20 + '];');
    ALTER TABLE [OrganizationDepartments] DROP COLUMN [SystemSettingId];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190917115315_SystemSettingsCleanUp')
BEGIN
    DECLARE @var21 sysname;
    SELECT @var21 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Incidences]') AND [c].[name] = N'SystemSettingId');
    IF @var21 IS NOT NULL EXEC(N'ALTER TABLE [Incidences] DROP CONSTRAINT [' + @var21 + '];');
    ALTER TABLE [Incidences] DROP COLUMN [SystemSettingId];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190917115315_SystemSettingsCleanUp')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190917115315_SystemSettingsCleanUp', N'2.2.6-servicing-10079');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191005134608_UserUpdate')
BEGIN
    ALTER TABLE [AspNetUsers] DROP CONSTRAINT [FK_AspNetUsers_OrganizationDepartments_OrganizationDepartmentId];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191005134608_UserUpdate')
BEGIN
    EXEC sp_rename N'[AspNetUsers].[OrganizationDepartmentId]', N'DepartmentId', N'COLUMN';
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191005134608_UserUpdate')
BEGIN
    EXEC sp_rename N'[AspNetUsers].[IX_AspNetUsers_OrganizationDepartmentId]', N'IX_AspNetUsers_DepartmentId', N'INDEX';
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191005134608_UserUpdate')
BEGIN
    ALTER TABLE [AspNetUsers] ADD CONSTRAINT [FK_AspNetUsers_OrganizationDepartments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [OrganizationDepartments] ([Id]) ON DELETE NO ACTION;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191005134608_UserUpdate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20191005134608_UserUpdate', N'2.2.6-servicing-10079');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191023094057_IncidenceReporterFieldUpdate')
BEGIN
    EXEC sp_rename N'[Incidences].[ReporterFeedbackComment]', N'ReporterFirstResponderAction', N'COLUMN';
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191023094057_IncidenceReporterFieldUpdate')
BEGIN
    ALTER TABLE [Incidences] ADD [ManagerFeedbackRating] int NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191023094057_IncidenceReporterFieldUpdate')
BEGIN
    ALTER TABLE [Incidences] ADD [ReporterDepartmentId] uniqueidentifier NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191023094057_IncidenceReporterFieldUpdate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20191023094057_IncidenceReporterFieldUpdate', N'2.2.6-servicing-10079');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191031125632_IncidenceRequiredFieldUpdate')
BEGIN
    DECLARE @var22 sysname;
    SELECT @var22 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Incidences]') AND [c].[name] = N'Title');
    IF @var22 IS NOT NULL EXEC(N'ALTER TABLE [Incidences] DROP CONSTRAINT [' + @var22 + '];');
    ALTER TABLE [Incidences] ALTER COLUMN [Title] nvarchar(100) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191031125632_IncidenceRequiredFieldUpdate')
BEGIN
    DECLARE @var23 sysname;
    SELECT @var23 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Incidences]') AND [c].[name] = N'Description');
    IF @var23 IS NOT NULL EXEC(N'ALTER TABLE [Incidences] DROP CONSTRAINT [' + @var23 + '];');
    ALTER TABLE [Incidences] ALTER COLUMN [Description] nvarchar(255) NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191031125632_IncidenceRequiredFieldUpdate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20191031125632_IncidenceRequiredFieldUpdate', N'2.2.6-servicing-10079');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191101082540_HazardFeature')
BEGIN
    ALTER TABLE [Organizations] ADD [HazardDefaultDepartmentId] uniqueidentifier NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191101082540_HazardFeature')
BEGIN
    ALTER TABLE [Gallery] ADD [HazardId] uniqueidentifier NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191101082540_HazardFeature')
BEGIN
    CREATE TABLE [Hazards] (
        [Id] uniqueidentifier NOT NULL,
        [Code] nvarchar(200) NULL,
        [Title] nvarchar(100) NULL,
        [Description] nvarchar(255) NOT NULL,
        [Comment] nvarchar(1024) NULL,
        [RouteCause] nvarchar(255) NULL,
        [AreaId] uniqueidentifier NULL,
        [CityId] uniqueidentifier NULL,
        [StateId] uniqueidentifier NULL,
        [CountryId] uniqueidentifier NULL,
        [address] nvarchar(max) NULL,
        [ReportedLatitude] float NOT NULL,
        [ReportedLongitude] float NOT NULL,
        [Suggestion] nvarchar(255) NULL,
        [AssignerId] uniqueidentifier NULL,
        [AssigneeId] uniqueidentifier NULL,
        [DateCreated] datetime2 NOT NULL,
        [CreatedByUserId] uniqueidentifier NOT NULL,
        [DateEdited] datetime2 NULL,
        [EditedByUserId] uniqueidentifier NULL,
        [AssignedOrganizationId] uniqueidentifier NULL,
        [AssignedDepartmentId] uniqueidentifier NULL,
        [ReporterName] nvarchar(50) NULL,
        [ReporterEmail] nvarchar(50) NULL,
        [ReporterFirstResponderAction] nvarchar(1024) NULL,
        [ReporterFeedbackRating] int NULL,
        [ManagerFeedbackRating] int NULL,
        [ReporterDepartmentId] uniqueidentifier NULL,
        [IncidenceStatusId] uniqueidentifier NULL,
        [ResolutionDate] datetime2 NULL,
        [Protected] bit NOT NULL,
        [Deleted] bit NOT NULL,
        [OrganizationId] uniqueidentifier NULL,
        [IsRectified] bit NOT NULL,
        CONSTRAINT [PK_Hazards] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Hazards_Areas_AreaId] FOREIGN KEY ([AreaId]) REFERENCES [Areas] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Hazards_OrganizationDepartments_AssignedDepartmentId] FOREIGN KEY ([AssignedDepartmentId]) REFERENCES [OrganizationDepartments] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Hazards_Organizations_AssignedOrganizationId] FOREIGN KEY ([AssignedOrganizationId]) REFERENCES [Organizations] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Hazards_AspNetUsers_AssigneeId] FOREIGN KEY ([AssigneeId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Hazards_AspNetUsers_AssignerId] FOREIGN KEY ([AssignerId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Hazards_Cities_CityId] FOREIGN KEY ([CityId]) REFERENCES [Cities] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Hazards_Countries_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [Countries] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Hazards_AspNetUsers_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Hazards_AspNetUsers_EditedByUserId] FOREIGN KEY ([EditedByUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Hazards_IncidenceStatuses_IncidenceStatusId] FOREIGN KEY ([IncidenceStatusId]) REFERENCES [IncidenceStatuses] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Hazards_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [Organizations] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Hazards_States_StateId] FOREIGN KEY ([StateId]) REFERENCES [States] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191101082540_HazardFeature')
BEGIN
    CREATE INDEX [IX_Gallery_HazardId] ON [Gallery] ([HazardId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191101082540_HazardFeature')
BEGIN
    CREATE INDEX [IX_Hazards_AreaId] ON [Hazards] ([AreaId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191101082540_HazardFeature')
BEGIN
    CREATE INDEX [IX_Hazards_AssignedDepartmentId] ON [Hazards] ([AssignedDepartmentId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191101082540_HazardFeature')
BEGIN
    CREATE INDEX [IX_Hazards_AssignedOrganizationId] ON [Hazards] ([AssignedOrganizationId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191101082540_HazardFeature')
BEGIN
    CREATE INDEX [IX_Hazards_AssigneeId] ON [Hazards] ([AssigneeId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191101082540_HazardFeature')
BEGIN
    CREATE INDEX [IX_Hazards_AssignerId] ON [Hazards] ([AssignerId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191101082540_HazardFeature')
BEGIN
    CREATE INDEX [IX_Hazards_CityId] ON [Hazards] ([CityId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191101082540_HazardFeature')
BEGIN
    CREATE INDEX [IX_Hazards_CountryId] ON [Hazards] ([CountryId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191101082540_HazardFeature')
BEGIN
    CREATE INDEX [IX_Hazards_CreatedByUserId] ON [Hazards] ([CreatedByUserId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191101082540_HazardFeature')
BEGIN
    CREATE INDEX [IX_Hazards_EditedByUserId] ON [Hazards] ([EditedByUserId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191101082540_HazardFeature')
BEGIN
    CREATE INDEX [IX_Hazards_IncidenceStatusId] ON [Hazards] ([IncidenceStatusId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191101082540_HazardFeature')
BEGIN
    CREATE INDEX [IX_Hazards_OrganizationId] ON [Hazards] ([OrganizationId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191101082540_HazardFeature')
BEGIN
    CREATE INDEX [IX_Hazards_StateId] ON [Hazards] ([StateId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191101082540_HazardFeature')
BEGIN
    ALTER TABLE [Gallery] ADD CONSTRAINT [FK_Gallery_Hazards_HazardId] FOREIGN KEY ([HazardId]) REFERENCES [Hazards] ([Id]) ON DELETE NO ACTION;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191101082540_HazardFeature')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20191101082540_HazardFeature', N'2.2.6-servicing-10079');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191104194209_EventLocationUpdate')
BEGIN
    DECLARE @var24 sysname;
    SELECT @var24 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Hazards]') AND [c].[name] = N'IsRectified');
    IF @var24 IS NOT NULL EXEC(N'ALTER TABLE [Hazards] DROP CONSTRAINT [' + @var24 + '];');
    ALTER TABLE [Hazards] DROP COLUMN [IsRectified];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191104194209_EventLocationUpdate')
BEGIN
    EXEC sp_rename N'[Incidences].[address]', N'Address', N'COLUMN';
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191104194209_EventLocationUpdate')
BEGIN
    EXEC sp_rename N'[Hazards].[address]', N'Address', N'COLUMN';
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191104194209_EventLocationUpdate')
BEGIN
    DECLARE @var25 sysname;
    SELECT @var25 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Incidences]') AND [c].[name] = N'Address');
    IF @var25 IS NOT NULL EXEC(N'ALTER TABLE [Incidences] DROP CONSTRAINT [' + @var25 + '];');
    ALTER TABLE [Incidences] ALTER COLUMN [Address] nvarchar(max) NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191104194209_EventLocationUpdate')
BEGIN
    DECLARE @var26 sysname;
    SELECT @var26 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Incidences]') AND [c].[name] = N'ReportedIncidenceLongitude');
    IF @var26 IS NOT NULL EXEC(N'ALTER TABLE [Incidences] DROP CONSTRAINT [' + @var26 + '];');
    ALTER TABLE [Incidences] ALTER COLUMN [ReportedIncidenceLongitude] float NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191104194209_EventLocationUpdate')
BEGIN
    DECLARE @var27 sysname;
    SELECT @var27 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Incidences]') AND [c].[name] = N'ReportedIncidenceLatitude');
    IF @var27 IS NOT NULL EXEC(N'ALTER TABLE [Incidences] DROP CONSTRAINT [' + @var27 + '];');
    ALTER TABLE [Incidences] ALTER COLUMN [ReportedIncidenceLatitude] float NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191104194209_EventLocationUpdate')
BEGIN
    DECLARE @var28 sysname;
    SELECT @var28 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Hazards]') AND [c].[name] = N'Address');
    IF @var28 IS NOT NULL EXEC(N'ALTER TABLE [Hazards] DROP CONSTRAINT [' + @var28 + '];');
    ALTER TABLE [Hazards] ALTER COLUMN [Address] nvarchar(max) NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191104194209_EventLocationUpdate')
BEGIN
    DECLARE @var29 sysname;
    SELECT @var29 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Hazards]') AND [c].[name] = N'ReportedLongitude');
    IF @var29 IS NOT NULL EXEC(N'ALTER TABLE [Hazards] DROP CONSTRAINT [' + @var29 + '];');
    ALTER TABLE [Hazards] ALTER COLUMN [ReportedLongitude] float NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191104194209_EventLocationUpdate')
BEGIN
    DECLARE @var30 sysname;
    SELECT @var30 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Hazards]') AND [c].[name] = N'ReportedLatitude');
    IF @var30 IS NOT NULL EXEC(N'ALTER TABLE [Hazards] DROP CONSTRAINT [' + @var30 + '];');
    ALTER TABLE [Hazards] ALTER COLUMN [ReportedLatitude] float NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191104194209_EventLocationUpdate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20191104194209_EventLocationUpdate', N'2.2.6-servicing-10079');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191114173703_MobileAppUserLoginUpdate')
BEGIN
    ALTER TABLE [AspNetUsers] ADD [IsActive] bit NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191114173703_MobileAppUserLoginUpdate')
BEGIN
    ALTER TABLE [AspNetUsers] ADD [MobileAppLoginPattern] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191114173703_MobileAppUserLoginUpdate')
BEGIN
    update AspNetUsers set IsActive = 1
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191114173703_MobileAppUserLoginUpdate')
BEGIN
    update AspNetUsers set MobileAppLoginPattern = ''
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20191114173703_MobileAppUserLoginUpdate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20191114173703_MobileAppUserLoginUpdate', N'2.2.6-servicing-10079');
END;

GO

