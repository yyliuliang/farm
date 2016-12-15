USE [master]
GO
/****** Object:  Database [farm]    Script Date: 2016/12/15 10:45:48 ******/
CREATE DATABASE [farm]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'farm', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\farm.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'farm_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\farm_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [farm] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [farm].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [farm] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [farm] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [farm] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [farm] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [farm] SET ARITHABORT OFF 
GO
ALTER DATABASE [farm] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [farm] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [farm] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [farm] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [farm] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [farm] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [farm] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [farm] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [farm] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [farm] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [farm] SET  DISABLE_BROKER 
GO
ALTER DATABASE [farm] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [farm] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [farm] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [farm] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [farm] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [farm] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [farm] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [farm] SET RECOVERY FULL 
GO
ALTER DATABASE [farm] SET  MULTI_USER 
GO
ALTER DATABASE [farm] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [farm] SET DB_CHAINING OFF 
GO
ALTER DATABASE [farm] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [farm] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'farm', N'ON'
GO
USE [farm]
GO
/****** Object:  Table [dbo].[Entrust]    Script Date: 2016/12/15 10:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Entrust](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TargetId] [int] NULL,
	[UserId] [int] NULL,
	[ProductId] [int] NULL,
	[Price] [decimal](18, 5) NULL,
	[Count] [int] NULL,
	[DealedCount] [int] NULL,
	[DealedAmount] [decimal](18, 5) NULL,
	[IsBuy] [bit] NULL,
	[Status] [int] NULL,
	[Cancelled] [bit] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_MarketDelegate] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[KeyValueConfig]    Script Date: 2016/12/15 10:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[KeyValueConfig](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Key] [varchar](200) NULL,
	[Name] [varchar](200) NULL,
	[Value] [nvarchar](max) NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_KeyValueConfig] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Market]    Script Date: 2016/12/15 10:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Market](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[CurrentPrice] [decimal](18, 5) NULL,
	[PrevDayPrice] [decimal](18, 5) NULL,
	[OpenPrice] [decimal](18, 5) NULL,
	[ClosePrice] [decimal](18, 5) NULL,
	[TopPrice] [decimal](18, 5) NULL,
	[BottomPrice] [decimal](18, 5) NULL,
	[Volume] [int] NULL,
	[Date] [date] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_ProductMarket] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[News]    Script Date: 2016/12/15 10:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[News](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CategoryId] [int] NULL,
	[Title] [varchar](200) NULL,
	[Source] [varchar](200) NULL,
	[Author] [varchar](50) NULL,
	[ReadCount] [int] NOT NULL,
	[PreviewContent] [nvarchar](max) NULL,
	[Content] [nvarchar](max) NULL,
	[CreateTime] [datetime] NOT NULL,
	[Creator] [int] NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_News] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[NewsCategory]    Script Date: 2016/12/15 10:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[NewsCategory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[Title] [nvarchar](200) NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_NewsCategory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Product]    Script Date: 2016/12/15 10:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Product](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [varchar](50) NULL,
	[ProductCode] [varchar](50) NULL,
	[PriceLimit] [decimal](18, 6) NULL,
	[ImageUrl] [varchar](200) NULL,
	[Description] [varchar](max) NULL,
	[CreateTime] [datetime] NOT NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProductRebirth]    Script Date: 2016/12/15 10:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductRebirth](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NULL,
	[UserId] [int] NULL,
	[RebirthCount] [int] NULL,
	[SeedCount] [int] NULL,
	[ChargeFee] [decimal](18, 5) NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_ProductRebirth] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Role]    Script Date: 2016/12/15 10:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Role](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [varchar](50) NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Sms]    Script Date: 2016/12/15 10:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sms](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Phone] [varchar](50) NULL,
	[Code] [varchar](50) NULL,
	[Category] [varchar](50) NULL,
	[Message] [varchar](500) NULL,
	[Sender] [int] NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Sms] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SysLog]    Script Date: 2016/12/15 10:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Log] [nvarchar](max) NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_SysLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Transaction]    Script Date: 2016/12/15 10:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transaction](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TransactionId] [uniqueidentifier] NOT NULL,
	[ProductId] [int] NULL,
	[UserId] [int] NULL,
	[IsBuy] [bit] NULL,
	[Count] [int] NULL,
	[ChargeFee] [decimal](18, 5) NULL,
	[Price] [decimal](18, 5) NULL,
	[Date] [date] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_MarketTransaction] PRIMARY KEY CLUSTERED 
(
	[TransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User]    Script Date: 2016/12/15 10:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(60000,1) NOT NULL,
	[UserGuid] [uniqueidentifier] NOT NULL,
	[UserName] [varchar](50) NULL,
	[DisplayName] [nvarchar](200) NULL,
	[Phone] [varchar](50) NULL,
	[Password] [varchar](50) NULL,
	[Avatar] [varchar](200) NULL,
	[TotalScore] [decimal](18, 5) NOT NULL,
	[FrozenScore] [decimal](18, 5) NOT NULL,
	[LastLoginIP] [varchar](50) NULL,
	[LastLoginTime] [datetime] NULL,
	[IdNum] [varchar](50) NULL,
	[SmsGiveSwitch] [bit] NOT NULL,
	[RefUserId] [int] NULL,
	[RefUserPath] [varchar](500) NULL,
	[CreateTime] [datetime] NOT NULL,
	[Deleted] [bit] NOT NULL,
	[IsAdmin] [bit] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserBankAccount]    Script Date: 2016/12/15 10:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserBankAccount](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[Bank] [varchar](50) NULL,
	[AccountName] [varchar](50) NULL,
	[AccountNum] [varchar](50) NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_UserBankAccount] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserBorrow]    Script Date: 2016/12/15 10:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserBorrow](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[ProductId] [int] NULL,
	[BorrowCount] [int] NULL,
	[DailyInterest] [decimal](18, 5) NULL,
	[Bail] [decimal](18, 5) NULL,
	[Status] [int] NOT NULL,
	[ReturnTime] [datetime] NULL,
	[Deadline] [datetime] NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_UserBorrow] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserGive]    Script Date: 2016/12/15 10:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserGive](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[ReceiverId] [int] NULL,
	[ProductId] [int] NULL,
	[Count] [int] NULL,
	[ChargeFee] [decimal](18, 5) NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_UserGive] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserProduct]    Script Date: 2016/12/15 10:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProduct](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[ProductId] [int] NULL,
	[TotalCount] [int] NULL,
	[FrozenCount] [int] NULL,
	[UpdateTime] [datetime] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_UserProduct] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserScore]    Script Date: 2016/12/15 10:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserScore](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[TypeId] [int] NULL,
	[Num] [varchar](50) NULL,
	[UserPath] [varchar](max) NULL,
	[Score] [decimal](18, 5) NULL,
	[ChargeFee] [decimal](18, 5) NULL,
	[Status] [int] NULL,
	[Description] [varchar](500) NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_UserScore] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserWithdraw]    Script Date: 2016/12/15 10:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserWithdraw](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[Bank] [varchar](50) NULL,
	[AccountNum] [varchar](50) NULL,
	[AccountName] [varchar](50) NULL,
	[Amount] [decimal](18, 5) NULL,
	[ChargeFee] [decimal](18, 5) NULL,
	[Status] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_UserWithdraw] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[Entrust] ADD  CONSTRAINT [DF_MarketDelegate_Cancelled]  DEFAULT ((0)) FOR [Cancelled]
GO
ALTER TABLE [dbo].[Entrust] ADD  CONSTRAINT [DF_MarketDelegate_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[KeyValueConfig] ADD  CONSTRAINT [DF_KeyValueConfig_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[Market] ADD  CONSTRAINT [DF_ProductMarket_Date]  DEFAULT (getdate()) FOR [Date]
GO
ALTER TABLE [dbo].[Market] ADD  CONSTRAINT [DF_ProductMarket_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[News] ADD  CONSTRAINT [DF_News_ReadCount]  DEFAULT ((0)) FOR [ReadCount]
GO
ALTER TABLE [dbo].[News] ADD  CONSTRAINT [DF_News_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[News] ADD  CONSTRAINT [DF_News_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[NewsCategory] ADD  CONSTRAINT [DF_NewsCategory_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[ProductRebirth] ADD  CONSTRAINT [DF_ProductRebirth_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[Role] ADD  CONSTRAINT [DF_Role_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[Sms] ADD  CONSTRAINT [DF_Sms_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[SysLog] ADD  CONSTRAINT [DF_SysLog_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[Transaction] ADD  CONSTRAINT [DF_MarketTransaction_Date]  DEFAULT (getdate()) FOR [Date]
GO
ALTER TABLE [dbo].[Transaction] ADD  CONSTRAINT [DF_MarketTransaction_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_AvailableScore]  DEFAULT ((0)) FOR [TotalScore]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_FrozenScore]  DEFAULT ((0)) FOR [FrozenScore]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_SmsGiveSwitch]  DEFAULT ((0)) FOR [SmsGiveSwitch]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_RefUserId]  DEFAULT ((0)) FOR [RefUserId]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_IsAdmin]  DEFAULT ((0)) FOR [IsAdmin]
GO
ALTER TABLE [dbo].[UserBankAccount] ADD  CONSTRAINT [DF_UserBankAccount_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[UserBorrow] ADD  CONSTRAINT [DF_UserBorrow_Status]  DEFAULT ((0)) FOR [Status]
GO
ALTER TABLE [dbo].[UserBorrow] ADD  CONSTRAINT [DF_UserBorrow_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[UserGive] ADD  CONSTRAINT [DF_UserGive_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[UserProduct] ADD  CONSTRAINT [DF_UserProduct_UpdateTime]  DEFAULT (getdate()) FOR [UpdateTime]
GO
ALTER TABLE [dbo].[UserProduct] ADD  CONSTRAINT [DF_UserProduct_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[UserScore] ADD  CONSTRAINT [DF_UserScore_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[UserWithdraw] ADD  CONSTRAINT [DF_UserWithdraw_Status]  DEFAULT ((0)) FOR [Status]
GO
ALTER TABLE [dbo].[UserWithdraw] ADD  CONSTRAINT [DF_UserWithdraw_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
USE [master]
GO
ALTER DATABASE [farm] SET  READ_WRITE 
GO
