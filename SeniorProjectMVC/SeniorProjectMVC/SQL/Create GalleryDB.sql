Drop table dbo.Favorites
Drop table dbo.Images
Drop table dbo.Users
Drop Procedure dbo.GetAllUsers
Drop Procedure dbo.GetAllImages
Drop Procedure GetImagesPaginated
Drop Procedure GetSingleImage
Drop Procedure SearchImages
Drop Procedure SearchImagesPaginated
Drop Procedure AddImage
Drop Procedure ValidateLogin
Drop Procedure LoginAttempt
Drop Procedure GetUserImages
Drop Procedure GetUsername
Drop Procedure AddUser
Drop Procedure DeleteImage
Drop View UserFavorites
Drop Procedure ToggleFavorite
Drop Procedure FavoriteCheck
Drop Procedure GetUserFavorites
Drop Procedure EditImage
Drop Procedure GetUserImagesPaginated
Drop Procedure GetUserFavoritesPaginated


Create Table dbo.Users(
	UserID int IDENTITY(1,1) NOT NULL,
	Email varchar(50) NOT NULL,
	UserName varchar(50) NOT NULL,
	PasswordHash BINARY(64) NOT NULL,
	Salt UNIQUEIDENTIFIER Not Null,
PRIMARY KEY CLUSTERED
(
UserID ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF,
ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

Create Table dbo.Images(
	ImageID int IDENTITY(1,1) NOT NULL,
	OwnerID int NOT NULL,
	Name varchar(50) NOT NULL,
	Extension varchar(10) NOT NULL,
	ImageSize bigint NOT NULL,
	Description varchar(250) NOT NULL,
	FOREIGN KEY (OwnerID) REFERENCES dbo.Users (UserID) ON DELETE CASCADE ON UPDATE CASCADE,
PRIMARY KEY CLUSTERED
(
ImageID ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF,
ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

Create Table dbo.Favorites(
	FavID int IDENTITY(1,1) NOT NULL,
	UserID int NOT NULL,
	ImageID int NOT NULL,
	Favorited TINYINT NOT NULL,
PRIMARY KEY CLUSTERED
(
FavID ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF,
ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

Create View UserFavorites As
Select F.UserID, U.UserName, F.ImageID, I.Name, F.Favorited From dbo.Favorites F
Join Users U on U.UserID = F.UserID
Join Images I on I.ImageID = F.ImageID
Go


Declare @Salt UniqueIdentifier=NEWID()

Insert into dbo.Users values('asporter09@gmail.com', 'Admin', HASHBYTES('SHA2_512', 'AdminPass'+CAST(@Salt As Nvarchar(36))), @Salt);
GO

Declare @Salt UniqueIdentifier=NEWID()

Insert into dbo.Users values('email.com', 'User1', HASHBYTES('SHA2_512', 'UserPass'+CAST(@Salt As Nvarchar(36))), @Salt);
GO

Insert into dbo.Images values(1, 'Arch', '.jpg', 0, 'Navajo Arch')
Insert into dbo.Images values(1, 'Bright Clouds', '.jpg', 0, 'Gold clouds on a sunny day')
Insert into dbo.Images values(1, 'Bright Sky', '.jpg', 0, 'The sky at dawn')
Insert into dbo.Images values(1, 'Clear Waters', '.jpg', 0, 'Clear waters at the beach')
Insert into dbo.Images values(1, 'Cloudy Sky', '.jpg', 0, 'The suns rays hidden by the clouds')
Insert into dbo.Images values(1, 'Foggy Forest', '.jpg', 0, 'Fog fills the forest')
Insert into dbo.Images values(1, 'Jellyfish', '.jpg', 0, 'Jellyfish in the deep blue')
Insert into dbo.Images values(1, 'Lake Dock', '.jpg', 0, 'Dock by a calm lake')
Insert into dbo.Images values(1, 'Mountain View', '.jpg', 0, 'Snowy mountaintop')
Insert into dbo.Images values(2, 'Pink Sky', '.jpg', 0, 'Rainbow at dawn')
Insert into dbo.Images values(2, 'Pink Sunset', '.jpg', 0, 'Sunset below the trees')
Insert into dbo.Images values(2, 'Blue Butterflies', '.jpg', 0, 'Fantasy butterflies')
Insert into dbo.Images values(2, 'Autumn Tree', '.jpg', 0, 'Orange trees shed leaves')
Insert into dbo.Images values(1, 'Sunset Tree', '.jpg', 0, 'The sun sets under the blazing sky')
Insert into dbo.Images values(1, 'Blue Flowers', '.jpg', 0, 'Blue flowers blossom')
Insert into dbo.Images values(1, 'Desert Dunes', '.jpg', 0, 'Daylight on the desert dunes')
Insert into dbo.Images values(2, 'Cloudy Mountains', '.jpg', 0, 'Mountains hidden by the clouds')
Insert into dbo.Images values(2, 'Purple Flowers', '.jpg', 0, 'Default Image')
Insert into dbo.Images values(1, 'Butterfly', '.jpg', 0, 'Butterfly close-up')
Insert into dbo.Images values(1, 'Sunset Lake', '.jpg', 0, 'Lake at sunset')
Insert into dbo.Images values(2, 'Chick', '.jpg', 0, 'Cute chick')
Insert into dbo.Images values(1, 'Sunflowers', '.jpg', 0, 'Sunny sunflowers')
Insert into dbo.Images values(2, 'Sunlit Shade', '.jpg', 0, 'Default Image')



Insert into dbo.Favorites values(1, 10, 1)
Insert into dbo.Favorites values(2, 3, 1)
Go

Create Procedure dbo.GetAllUsers
As
Begin
	Set Nocount On

	Select UserID, UserName, Email From dbo.Users
End
Go

Create Procedure dbo.GetAllImages
As
Begin
	Set Nocount On

	Select * From dbo.Images
	Order By ImageID Desc
End
Go

Create Procedure dbo.GetImagesPaginated
	@Page int
As
Begin
	Set Nocount On
	DECLARE @PerPage AS INT
	Set @PerPage = 20

	Select * From dbo.Images
	Order By ImageID Desc
	OFFSET (@Page - 1) * @PerPage Rows
	Fetch Next @PerPage Rows Only
End
Go

Create Procedure dbo.SearchImages
	@input varchar(50)
As
Begin
	Set Nocount On
	
	Declare @Match varchar(54)
	Select @Match = Concat('%', @input, '%'); 
	
	Select * From dbo.Images Where Name Like @Match
	Order By ImageID Desc
End
Go

Create Procedure dbo.SearchImagesPaginated
	@input varchar(50), @Page int
As
Begin
	Set Nocount On
	DECLARE @PerPage AS INT
	Set @PerPage = 20
	Declare @Match varchar(54)
	Select @Match = Concat('%', @input, '%'); 

	Select * From dbo.Images 
	Where Name Like @Match OR Description Like @Match
	Order By ImageID Desc
	OFFSET (@Page - 1) * @PerPage Rows
	Fetch Next @PerPage Rows Only
End
Go

Create Procedure dbo.GetSingleImage
	@ID int
As
Begin
	Set Nocount On
	Declare @Count int

	Select @Count = Count(ImageID) From Images Where ImageID = @ID
	If @Count = 0
    BEGIN
		Select 0 As ImageID, 0 As OwnerID, 'None' As Name, 'None' As Extension, 0 As ImageSize
		Return
    END
	
	Select I.ImageID, I.OwnerID, I.Name, I.Extension, I.ImageSize, 
	I.Description, U.UserName As OwnerUsername From Images I
	Join Users U on U.UserID = I.OwnerID
	Where ImageID = @ID
	
End
Go

Create Procedure dbo.AddImage
	@OwnerID int, @Name varchar(50), @Extension varchar(10), @ImageSize bigint, @Description varchar(250)
As
Begin
	Set Nocount On
	
	Begin Transaction
	Begin Try
		Insert into dbo.Images values(@OwnerID, @Name, @Extension, @ImageSize, @Description)
	End Try
	Begin Catch
		Rollback Transaction
		Select 1 As MessageID, 'Error' As Message
		Return
	End Catch
	
	Commit Transaction

	Declare @ID int
	Select @ID = (Select Top 1 ImageID From dbo.Images Order BY ImageID Desc)
	Select @ID As ReturnID, 'Success' As Message
End
Go

Create Procedure dbo.ValidateLogin
	@ID int,
	@Password varchar(50)
As
Begin
	Set Nocount On

	Declare @userID int
	Set @userID=(Select UserID From Users Where UserID=@ID And PasswordHash=HASHBYTES('SHA2_512', @Password+CAST(Salt As Nvarchar(36))))

	If(@userID Is Null)
	Begin
		Select 0 As ReturnID, 'Invalid' As Message
		Return
	End
	Else
	Begin
		Select @userID As ReturnID, 'Valid' As Message
		Return
	End
End
Go

Create Procedure dbo.LoginAttempt
	@Username Nvarchar(50),
    @Password Nvarchar(50)
As
Begin
	Set Nocount On
	
	Declare @userID int

	IF Exists (Select Top 1 UserID From Users Where Username=@Username)
	Begin
		Set @userID=(Select UserID From Users Where Username=@Username AND PasswordHash=HASHBYTES('SHA2_512', @Password+CAST(Salt As Nvarchar(36))))

		If(@userID Is Null)
		Begin
			Select 0 As ReturnID, 'Incorrect password' As Message
			Return;
		End
		Else
		Begin
			Select @userID As ReturnID, 'Success' As Message
			Return;
		End
    End
    Else
		Select 0 As ReturnID, 'Invalid Login' As Message
		Return;
End
Go

Create Procedure dbo.GetUserImages
	@ID int
As
Begin
	Set Nocount On

	Select * From dbo.Images Where OwnerID = @ID
	Order By ImageID Desc
End
Go

Create Procedure dbo.GetUserImagesPaginated
	@ID int, @Page int
As
Begin
	Set Nocount On
	DECLARE @PerPage AS INT
	Set @PerPage = 20

	Select * From dbo.Images Where OwnerID = @ID
	Order By ImageID Desc
	OFFSET (@Page - 1) * @PerPage Rows
	Fetch Next @PerPage Rows Only
End
Go

Create Procedure dbo.GetUsername
	@ID int
As
Begin
	Set Nocount On
	
	Declare @userName varchar(50)

	IF Exists (Select Top 1 Username From Users Where UserID = @ID)
	Begin
		Set @userName = (Select Top 1 Username From Users Where UserID = @ID)

		Select @ID As ReturnID, @userName As Message
	End
	Else
	Begin
		Select 0 As ReturnID, 'Invalid ID' As Message
	End
End
Go

Create Procedure dbo.AddUser
	@UserName varchar(50), @Email varchar(50), @Password varchar(50)
As
Begin
	Set Nocount On
	
	Begin Transaction
	Begin Try
		Declare @Salt UniqueIdentifier=NEWID()

		Insert into dbo.Users values(@Email, @UserName, HASHBYTES('SHA2_512', @Password+CAST(@Salt As Nvarchar(36))), @Salt);
	End Try
	Begin Catch
		Rollback Transaction
		Select 0 As MessageID, 'Error' As Message
		Return
	End Catch
	
	Commit Transaction

	Declare @ID int
	Set @ID = (Select Top 1 UserID From dbo.Users Order BY UserID Desc)
	Select @ID As ReturnID, 'Success' As Message
End
Go

Create Procedure dbo.DeleteImage
	@UserID int, @ImageID int
As
Begin
	Set Nocount On

	IF Exists (Select Top 1 ImageID From Images Where ImageID = @ImageID)
	Begin
		IF Exists (Select Top 1 ImageID From Images Where ImageID = @ImageID And OwnerID = @UserID)
		Begin
			Select I.ImageID, I.OwnerID, I.Name, I.Extension, I.ImageSize, 
			'Success' As Description, U.UserName As OwnerUsername From Images I
			Join Users U on U.UserID = I.OwnerID
			Where ImageID = @ImageID

			Delete From Images Where ImageID = @ImageID
		End
		Else
		Begin
			Select 0 As ImageID, I.OwnerID, I.Name, I.Extension, I.ImageSize, 
			'Access Denied' As Description, U.UserName As OwnerUsername From Images I
			Join Users U on U.UserID = I.OwnerID
			Where ImageID = @ImageID
		End
	End
	Else
	Begin
		Select 0 As ImageID, 0 As OwnerID, '' As Name, '' As Extension, 0 As ImageSize, 
		'Invalid ID' As Description, '' As OwnerUsername
	End
End
Go

Create Procedure dbo.ToggleFavorite
	@UserID int, @ImageID int
As
Begin
	Set Nocount On

	IF Exists (Select Top 1 Favorited From Favorites Where ImageID = @ImageID And UserID = @UserID)
	Begin
		Declare @Fav TinyInt
		Set @Fav = (Select Top 1 Favorited From Favorites Where ImageID = @ImageID And UserID = @UserID)

		If @Fav = 1
		Begin
			Delete From dbo.Favorites Where ImageID = @ImageID And UserID = @UserID

			Select 0 As ReturnID, 'Success - UnFavorited' As Message
		End
		Else
		Begin
			Update dbo.Favorites Set Favorited = 1 Where ImageID = @ImageID And UserID = @UserID

			Select 1 As ReturnID, 'Success - Favorited' As Message
		End
	End
	Else
	Begin
		Insert into dbo.Favorites values(@UserID, @ImageID, 1)

		Select 1 As ReturnID, 'Success - Added' As Message
	End
End
Go

Create Procedure dbo.FavoriteCheck
	@UserID int, @ImageID int
As
Begin
	Set Nocount On

	IF Exists (Select Top 1 Favorited From Favorites Where ImageID = @ImageID And UserID = @UserID)
	Begin
		Declare @Fav TinyInt
		Set @Fav = (Select Top 1 Favorited From Favorites Where ImageID = @ImageID And UserID = @UserID)

		If @Fav = 1
		Begin
			Select 1 As ReturnID, 'Favorited' As Message
		End
		Else
		Begin
			Select 0 As ReturnID, 'UnFavorited' As Message
		End
	End
	Else
	Begin
		Select 0 As ReturnID, 'No Favorite' As Message
	End
End
Go

Create Procedure dbo.GetUserFavorites
	@ID int
As
Begin
	Set Nocount On

	Select I.ImageID, I.OwnerID, I.Name, I.Extension, I.ImageSize, I.Description From dbo.Images I
	Join Favorites F ON F.ImageID = I.ImageID
	WHERE F.Favorited = 1 AND F.UserID = @ID AND F.ImageID = I.ImageID
	Order By F.FavID Desc
End
Go

Create Procedure dbo.GetUserFavoritesPaginated
	@ID int, @Page int
As
Begin
	Set Nocount On
	DECLARE @PerPage AS INT
	Set @PerPage = 20

	Select I.ImageID, I.OwnerID, I.Name, I.Extension, I.ImageSize, I.Description From dbo.Images I
	Join Favorites F ON F.ImageID = I.ImageID
	WHERE F.Favorited = 1 AND F.UserID = @ID AND F.ImageID = I.ImageID
	Order By F.FavID Desc
	OFFSET (@Page - 1) * @PerPage Rows
	Fetch Next @PerPage Rows Only
End
Go

Create Procedure dbo.EditImage
	@UserID int, @ImageID int, @Name varchar(50), @Description varchar(250)
As
Begin
	Set Nocount On

	IF Exists (Select Top 1 ImageID From Images Where ImageID = @ImageID)
	Begin
		IF Exists (Select Top 1 ImageID From Images Where ImageID = @ImageID And OwnerID = @UserID)
		Begin
			Update Images
			Set Name = @Name, Description = @Description
			Where ImageID = @ImageID

			Select 1 As ReturnID, 'Success' As Message
		End
		Else
		Begin
			Select 0 As ReturnID, 'Access Denied' As Message
		End
	End
	Else
	Begin
		Select 0 As ReturnID, 'Invalid ID' As Message
	End
End
Go