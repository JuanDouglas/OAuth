CREATE TABLE [Image](
    [ID] INTEGER IDENTITY PRIMARY KEY NOT NULL,
    [FileName] VARCHAR(500) NOT NULL,
    [FileType] INT NOT NULL
);

CREATE TABLE [CompanyCategory](
   [ID] INTEGER IDENTITY PRIMARY KEY NOT NULL,
);

CREATE TABLE [CompanyAccount](

);

CREATE TABLE [Account](
    [ID] INTEGER IDENTITY PRIMARY KEY NOT NULL,
    [Key] VARCHAR(300) UNIQUE NOT NULL,
    [Name] VARCHAR(250) NOT NULL,
    [UserName] VARCHAR(500) UNIQUE NOT NULL,
    [Password] VARCHAR(100) NOT NULL,
    [Email] VARCHAR(500) UNIQUE NOT NULL,
    [PhoneNumber] VARCHAR(32) NOT NULL,
    [IsCompany] BIT NOT NULL,
    [AcceptTermsDate] DATETIME2 NOT NULL,
    [Valid] BIT NOT NULL,
    [CreateDate] DATETIME2 NOT NULL,
    [ZipCode] VARCHAR(30) NOT NULL,
    [ProfileImageID] INTEGER NOT NULL
    FOREIGN KEY ([ProfileImageID]) REFERENCES [Image]([ID])
);

CREATE TABLE [Company] (
    [ID] INTEGER PRIMARY KEY NOT NULL,
    [Name] VARCHAR(500) NOT NULL,
    [CNPJ] VARCHAR(100) UNIQUE NOT NULL,
    [Icon] INTEGER NOT NULL,
    FOREIGN KEY ([Icon]) REFERENCES [Image]([ID])
);

CREATE TABLE [Personal](
    [ID] INTEGER IDENTITY PRIMARY KEY NOT NULL,
    [Account] INTEGER UNIQUE NOT NULL,
    FOREIGN KEY ([Account]) REFERENCES [Account]([ID])
);

CREATE TABLE [Application](
    [ID] INTEGER IDENTITY PRIMARY KEY NOT NULL, 
    [Name] VARCHAR(250) NOT NULL,
    [Key] VARCHAR(300) UNIQUE NOT NULL,
    [AuthorizeRedirect] VARCHAR(500) NOT NULL,
    [LoginRedirect] VARCHAR(500) NOT NULL,
    [Site] VARCHAR(200) NOT NULL,
    [Owner] INTEGER NOT NULL,
    [Icon] INTEGER NOT NULL,
    [PrivateKey] VARCHAR(300) NOT NULL,
    FOREIGN KEY ([Owner]) REFERENCES [Account]([ID]),
    FOREIGN KEY ([Icon]) REFERENCES [Image]([ID])
);

CREATE TABLE [IP] (
    [ID] INTEGER IDENTITY PRIMARY KEY NOT NULL,
    [Adress] VARCHAR(89) UNIQUE NOT NULL,
    [Confiance] INT NOT NULL,
    [AlreadyBeenBanned] BIT NOT NULL
);

CREATE TABLE [LoginFirstStep](
    [ID] INTEGER IDENTITY PRIMARY KEY NOT NULL, 
    [Date] DATETIME2 NOT NULL,
    [Account] INTEGER NOT NULL,
    [Token] VARCHAR(300) NOT NULL,
    [Valid] BIT NOT NULL,
    [IPAdress] VARCHAR(89) NOT NULL,
    FOREIGN KEY ([IPAdress]) REFERENCES [IP]([Adress]),
    FOREIGN KEY ([Account]) REFERENCES [Account]([ID])
);

CREATE TABLE [Authentication] (
    [ID] INTEGER IDENTITY PRIMARY KEY NOT NULL,
    [User-Agent] VARCHAR(300) NOT NULL,
    [IPAdress] VARCHAR(89) NOT NULL,
    [Token] VARCHAR(150) UNIQUE NOT NULL,
    [LoginFirstStep] INTEGER NOT NULL,
    [Date] DATETIME2 NOT NULL,
    [IsValid] BIT NOT NULL DEFAULT 0,
    FOREIGN KEY ([LoginFirstStep]) REFERENCES [LoginFirstStep]([ID]),
    FOREIGN KEY ([IPAdress]) REFERENCES [IP]([Adress])
);

CREATE TABLE [Authorization](
    [ID] INTEGER IDENTITY PRIMARY KEY NOT NULL, 
    [Authentication] INTEGER NOT NULL,
    [Application] INTEGER NOT NULL,
    [Key] VARCHAR(300) NOT NULL,
    [Active] BIT NOT NULL,
    [Level] INT NOT NULL,
    [Date] DATETIME2 NOT NULL,
    FOREIGN KEY ([Authentication]) REFERENCES [Authentication]([ID]),
    FOREIGN KEY ([Application]) REFERENCES [Application]([ID])
);

CREATE TABLE [ApplicationAuthentication] (
    [ID] INTEGER IDENTITY PRIMARY KEY NOT NULL,
    [User-Agent] VARCHAR(300) NOT NULL,
    [IPAdress] VARCHAR(89) NOT NULL,
    [Token] VARCHAR(300) UNIQUE NOT NULL,
    [Date] DATETIME2 NOT NULL,
    [Application] INTEGER NOT NULL,
    [Authorization] INTEGER  NOT NULL,
    [Authentication] INTEGER NOT NUlL,
    [Active] BIT NOT NULL,
    FOREIGN KEY ([IPAdress]) REFERENCES [IP]([Adress]),
    FOREIGN KEY ([Application]) REFERENCES [Application]([ID]),
    FOREIGN KEY ([Authorization]) REFERENCES [Authorization]([ID]),
    FOREIGN KEY ([Authentication]) REFERENCES [Authentication]([ID])
);

CREATE TABLE [FailAttemp](
    [ID] INTEGER IDENTITY PRIMARY KEY NOT NULL, 
    [Date] DATETIME2 NOT NULL,
    [IPAdress] VARCHAR(89) NOT NULL,
    [AttempType] INT NOT NULL,
    FOREIGN KEY ([IPAdress]) REFERENCES [IP]([Adress])
);

CREATE TABLE [AccountConfirmation](
    [ID] INTEGER IDENTITY PRIMARY KEY NOT NULL,
    [Date] DATETIME2 NOT NULL,
    [Key] VARCHAR(150) NOT NULL,
    [Account] INTEGER NOT NULL,
    FOREIGN KEY ([Account]) REFERENCES [Account]([ID])
);

/* INSERT IMAGES */
INSERT INTO [Image]([FileName],[FileType]) VALUES ('default_profile_image.png',1);
INSERT INTO [Image]([FileName],[FileType]) VALUES ('default_application_icon.png',2);
INSERT INTO [Image]([FileName],[FileType]) VALUES ('default_company_icon.png',3);

/* INSERT USERS */
INSERT INTO [Account] ([UserName],[Password],[Key],[ZipCode],[Email],[PhoneNumber],[IsCompany],[AcceptTermsDate],[Valid],[CreateDate],[ProfileImageID]) 
VALUES ('JuanDouglas','$2a$10$vl2ajEUyNi7/pGsMqy1N1.h65XLywq.FRwiX43t.58f2Ou1fYgSSu','pr1v@t3K31H4Sh','71881-663','juandouglas2004@gmail.com','+55 (61) 9 9260-6441',1,GETDATE(),1,GETDATE(),1);

/* INSERT APPLICATIONS */
INSERT INTO [Application] ([Name],[Owner],[Key],[Icon],[PrivateKey],[Site],[LoginRedirect],[AuthorizeRedirect]) 
VALUES('Stock-Manager-Api',1,'Nexus-Stock-Manager',2,'Pr1v4T3K31H@sh3d','https://nexus-stock-manager.azurewebsites.net/','https://nexus-stock-manager.azurewebsites.net/api/OAuth/Login?authorization_token={authorization-token}&account_id={account-id}&login_token={login-token}','https://nexus-stock-manager.azurewebsites.net/api/OAuth/Authorize?authorization_token={authorization-token}&account_id={account-id}');
 
SELECT * FROM [Image];
SELECT * FROM [Account];
SELECT * FROM [Application];