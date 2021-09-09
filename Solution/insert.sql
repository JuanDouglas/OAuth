USE [OAuth];
GO

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
