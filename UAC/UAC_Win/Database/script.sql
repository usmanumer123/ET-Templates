USE [master]
GO
/****** Object:  Database [CFL_CV_Practice]    Script Date: 6/4/2024 12:32:20 PM ******/
CREATE DATABASE [CFL_CV_Practice]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CFL_CV', FILENAME = N'E:\Visual Studio\EmergeTech Templates\database\CFL_CV.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'CFL_CV_log', FILENAME = N'E:\Visual Studio\EmergeTech Templates\database\CFL_CV_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [CFL_CV_Practice] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CFL_CV_Practice].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CFL_CV_Practice] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CFL_CV_Practice] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CFL_CV_Practice] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CFL_CV_Practice] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CFL_CV_Practice] SET ARITHABORT OFF 
GO
ALTER DATABASE [CFL_CV_Practice] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CFL_CV_Practice] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CFL_CV_Practice] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CFL_CV_Practice] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CFL_CV_Practice] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CFL_CV_Practice] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CFL_CV_Practice] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CFL_CV_Practice] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CFL_CV_Practice] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CFL_CV_Practice] SET  DISABLE_BROKER 
GO
ALTER DATABASE [CFL_CV_Practice] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CFL_CV_Practice] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CFL_CV_Practice] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CFL_CV_Practice] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CFL_CV_Practice] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CFL_CV_Practice] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CFL_CV_Practice] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CFL_CV_Practice] SET RECOVERY FULL 
GO
ALTER DATABASE [CFL_CV_Practice] SET  MULTI_USER 
GO
ALTER DATABASE [CFL_CV_Practice] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CFL_CV_Practice] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CFL_CV_Practice] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CFL_CV_Practice] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [CFL_CV_Practice] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [CFL_CV_Practice] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [CFL_CV_Practice] SET QUERY_STORE = OFF
GO
USE [CFL_CV_Practice]
GO
/****** Object:  Table [dbo].[UserProfile]    Script Date: 6/4/2024 12:32:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProfile](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](100) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[RollsID] [int] NOT NULL,
	[Password] [varchar](255) NOT NULL,
	[IsEnabled] [bit] NOT NULL,
 CONSTRAINT [PK_UserProfile] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRolls]    Script Date: 6/4/2024 12:32:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRolls](
	[RollsId] [int] IDENTITY(1,1) NOT NULL,
	[RollsDesc] [varchar](100) NOT NULL,
	[CreatedBy] [int] NOT NULL,
 CONSTRAINT [PK_UserRolls] PRIMARY KEY CLUSTERED 
(
	[RollsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRollsPermission]    Script Date: 6/4/2024 12:32:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRollsPermission](
	[PermissionId] [int] IDENTITY(1,1) NOT NULL,
	[Module] [varchar](100) NOT NULL,
	[Permission] [nvarchar](50) NOT NULL,
	[RollsId] [int] NOT NULL,
	[IsEnable] [bit] NOT NULL,
	[CreatedBy] [int] NOT NULL,
 CONSTRAINT [PK_UserRollsPermission] PRIMARY KEY CLUSTERED 
(
	[PermissionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UserProfile] ADD  CONSTRAINT [DF_UserProfile_IsEnabled]  DEFAULT ((1)) FOR [IsEnabled]
GO
ALTER TABLE [dbo].[UserRollsPermission] ADD  CONSTRAINT [DF_UserRollsPermission_IsEnable]  DEFAULT ((1)) FOR [IsEnable]
GO
ALTER TABLE [dbo].[UserProfile]  WITH CHECK ADD  CONSTRAINT [FK_UserProfile_UserProfile] FOREIGN KEY([RollsID])
REFERENCES [dbo].[UserRolls] ([RollsId])
GO
ALTER TABLE [dbo].[UserProfile] CHECK CONSTRAINT [FK_UserProfile_UserProfile]
GO
ALTER TABLE [dbo].[UserRollsPermission]  WITH CHECK ADD  CONSTRAINT [FK_UserRollsPermission_UserRolls] FOREIGN KEY([RollsId])
REFERENCES [dbo].[UserRolls] ([RollsId])
GO
ALTER TABLE [dbo].[UserRollsPermission] CHECK CONSTRAINT [FK_UserRollsPermission_UserRolls]
GO
/****** Object:  StoredProcedure [dbo].[DeleteUserAndPermissions]    Script Date: 6/4/2024 12:32:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteUserAndPermissions]
    @RollsId INT
AS
BEGIN
    -- First, delete from UserProfile where the RollsId is referenced
    DELETE FROM UserProfile WHERE RollsId = @RollsId;

    -- Then, delete from UserRollsPermission
    DELETE FROM UserRollsPermission WHERE RollsId = @RollsId;

    -- Finally, delete from UserRolls
    DELETE FROM UserRolls WHERE RollsId = @RollsId;
END
GO
USE [master]
GO
ALTER DATABASE [CFL_CV_Practice] SET  READ_WRITE 
GO
