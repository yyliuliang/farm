USE [master]
GO
/****** Object:  Database [farm]    Script Date: 2016/12/9 10:01:05 ******/
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
/****** Object:  Table [dbo].[Entrust]    Script Date: 2016/12/9 10:01:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Entrust](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[ProductId] [int] NULL,
	[Price] [decimal](18, 5) NULL,
	[Count] [int] NULL,
	[DealedCount] [int] NULL,
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
/****** Object:  Table [dbo].[KeyValueConfig]    Script Date: 2016/12/9 10:01:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[KeyValueConfig](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Key] [varchar](200) NULL,
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
/****** Object:  Table [dbo].[Market]    Script Date: 2016/12/9 10:01:06 ******/
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
/****** Object:  Table [dbo].[News]    Script Date: 2016/12/9 10:01:06 ******/
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
/****** Object:  Table [dbo].[NewsCategory]    Script Date: 2016/12/9 10:01:06 ******/
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
/****** Object:  Table [dbo].[Product]    Script Date: 2016/12/9 10:01:06 ******/
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
/****** Object:  Table [dbo].[ProductRebirth]    Script Date: 2016/12/9 10:01:06 ******/
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
/****** Object:  Table [dbo].[Role]    Script Date: 2016/12/9 10:01:06 ******/
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
/****** Object:  Table [dbo].[Sms]    Script Date: 2016/12/9 10:01:06 ******/
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
/****** Object:  Table [dbo].[SysLog]    Script Date: 2016/12/9 10:01:06 ******/
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
/****** Object:  Table [dbo].[Transaction]    Script Date: 2016/12/9 10:01:06 ******/
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
/****** Object:  Table [dbo].[User]    Script Date: 2016/12/9 10:01:06 ******/
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
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserBankAccount]    Script Date: 2016/12/9 10:01:06 ******/
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
/****** Object:  Table [dbo].[UserBorrow]    Script Date: 2016/12/9 10:01:06 ******/
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
/****** Object:  Table [dbo].[UserGive]    Script Date: 2016/12/9 10:01:06 ******/
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
/****** Object:  Table [dbo].[UserProduct]    Script Date: 2016/12/9 10:01:06 ******/
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
/****** Object:  Table [dbo].[UserScore]    Script Date: 2016/12/9 10:01:06 ******/
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
/****** Object:  Table [dbo].[UserWithdraw]    Script Date: 2016/12/9 10:01:06 ******/
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
SET IDENTITY_INSERT [dbo].[News] ON 

INSERT [dbo].[News] ([Id], [CategoryId], [Title], [Source], [Author], [ReadCount], [PreviewContent], [Content], [CreateTime], [Creator], [Deleted]) VALUES (1, 1, N'通知公告1', N'test', N'test', 4, N'test', N'testtest', CAST(0x0000A6CF00ADD3AB AS DateTime), 1, 0)
INSERT [dbo].[News] ([Id], [CategoryId], [Title], [Source], [Author], [ReadCount], [PreviewContent], [Content], [CreateTime], [Creator], [Deleted]) VALUES (2, 2, N'行业新闻1', N'test', N'test', 20, N'test', N'testtest', CAST(0x0000A6CF00C19BFA AS DateTime), 1, 0)
SET IDENTITY_INSERT [dbo].[News] OFF
SET IDENTITY_INSERT [dbo].[NewsCategory] ON 

INSERT [dbo].[NewsCategory] ([Id], [Name], [Title], [CreateTime]) VALUES (1, N'Notice', N'通知公告', CAST(0x0000A6CF00ABBB4B AS DateTime))
INSERT [dbo].[NewsCategory] ([Id], [Name], [Title], [CreateTime]) VALUES (2, N'Infomation', N'行业新闻', CAST(0x0000A6CF00ABCA28 AS DateTime))
SET IDENTITY_INSERT [dbo].[NewsCategory] OFF
SET IDENTITY_INSERT [dbo].[Product] ON 

INSERT [dbo].[Product] ([Id], [ProductName], [ProductCode], [PriceLimit], [ImageUrl], [Description], [CreateTime], [Deleted]) VALUES (800001, N'种子', N'800001', NULL, N'/content/images/800001.png', NULL, CAST(0x0000A6C5010B5E93 AS DateTime), 0)
INSERT [dbo].[Product] ([Id], [ProductName], [ProductCode], [PriceLimit], [ImageUrl], [Description], [CreateTime], [Deleted]) VALUES (800002, N'萝卜', N'800002', NULL, N'/content/images/800002.png', NULL, CAST(0x0000A6C5010B9517 AS DateTime), 0)
INSERT [dbo].[Product] ([Id], [ProductName], [ProductCode], [PriceLimit], [ImageUrl], [Description], [CreateTime], [Deleted]) VALUES (800003, N'苹果', N'800003', NULL, N'/content/images/800003.png', NULL, CAST(0x0000A6C5010BBF49 AS DateTime), 0)
INSERT [dbo].[Product] ([Id], [ProductName], [ProductCode], [PriceLimit], [ImageUrl], [Description], [CreateTime], [Deleted]) VALUES (800004, N'辣椒', N'800004', NULL, N'/content/images/800004.png', NULL, CAST(0x0000A6C5010BCE36 AS DateTime), 0)
INSERT [dbo].[Product] ([Id], [ProductName], [ProductCode], [PriceLimit], [ImageUrl], [Description], [CreateTime], [Deleted]) VALUES (800005, N'西瓜', N'800005', NULL, N'/content/images/800005.png', NULL, CAST(0x0000A6C5010BD7C3 AS DateTime), 0)
INSERT [dbo].[Product] ([Id], [ProductName], [ProductCode], [PriceLimit], [ImageUrl], [Description], [CreateTime], [Deleted]) VALUES (800006, N'南瓜', N'800006', NULL, N'/content/images/800006.png', NULL, CAST(0x0000A6C5010BDD8F AS DateTime), 0)
INSERT [dbo].[Product] ([Id], [ProductName], [ProductCode], [PriceLimit], [ImageUrl], [Description], [CreateTime], [Deleted]) VALUES (800007, N'草莓', N'800007', NULL, N'/content/images/800007.png', NULL, CAST(0x0000A6C5010BE976 AS DateTime), 0)
INSERT [dbo].[Product] ([Id], [ProductName], [ProductCode], [PriceLimit], [ImageUrl], [Description], [CreateTime], [Deleted]) VALUES (800008, N'玫瑰', N'800008', NULL, N'/content/images/800008.png', NULL, CAST(0x0000A6C5010BF00C AS DateTime), 0)
SET IDENTITY_INSERT [dbo].[Product] OFF
SET IDENTITY_INSERT [dbo].[Sms] ON 

INSERT [dbo].[Sms] ([Id], [Phone], [Code], [Category], [Message], [Sender], [CreateTime]) VALUES (1, N'18618477035', N' 2', NULL, N'尊敬的用户：您本次的验证码为  2，如非本人操作，请勿转告他人。', 1, CAST(0x0000A6D600B2457C AS DateTime))
INSERT [dbo].[Sms] ([Id], [Phone], [Code], [Category], [Message], [Sender], [CreateTime]) VALUES (2, N'18618477035', N'551155', NULL, N'尊敬的用户：您本次的验证码为 551155，如非本人操作，请勿转告他人。', 1, CAST(0x0000A6D600B3D1DB AS DateTime))
INSERT [dbo].[Sms] ([Id], [Phone], [Code], [Category], [Message], [Sender], [CreateTime]) VALUES (3, N'18618477035', N'207446', NULL, N'尊敬的用户：您本次的验证码为 207446，如非本人操作，请勿转告他人。', 1, CAST(0x0000A6D600B3EFF8 AS DateTime))
INSERT [dbo].[Sms] ([Id], [Phone], [Code], [Category], [Message], [Sender], [CreateTime]) VALUES (4, N'18618477035', N'378645', NULL, N'尊敬的用户：您本次的验证码为 378645，如非本人操作，请勿转告他人。', 1, CAST(0x0000A6D600B925D6 AS DateTime))
INSERT [dbo].[Sms] ([Id], [Phone], [Code], [Category], [Message], [Sender], [CreateTime]) VALUES (5, N'18618477035', N'105453', NULL, N'尊敬的用户：您本次的验证码为 105453，如非本人操作，请勿转告他人。', 1, CAST(0x0000A6D600BE1D01 AS DateTime))
INSERT [dbo].[Sms] ([Id], [Phone], [Code], [Category], [Message], [Sender], [CreateTime]) VALUES (6, N'18618477035', N'007148', NULL, N'尊敬的用户：您本次的验证码为 007148，如非本人操作，请勿转告他人。', 1, CAST(0x0000A6D600C02C66 AS DateTime))
INSERT [dbo].[Sms] ([Id], [Phone], [Code], [Category], [Message], [Sender], [CreateTime]) VALUES (7, N'18618477035', N'328360', N'Common', N'尊敬的用户：您本次的验证码为 328360，如非本人操作，请勿转告他人。', 1, CAST(0x0000A6D600C6DA63 AS DateTime))
INSERT [dbo].[Sms] ([Id], [Phone], [Code], [Category], [Message], [Sender], [CreateTime]) VALUES (8, N'18618477035', N'717635', N'Common', N'尊敬的用户：您本次的验证码为 717635，如非本人操作，请勿转告他人。', 1, CAST(0x0000A6D600C8642E AS DateTime))
SET IDENTITY_INSERT [dbo].[Sms] OFF
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([Id], [UserGuid], [UserName], [DisplayName], [Phone], [Password], [Avatar], [TotalScore], [FrozenScore], [LastLoginIP], [LastLoginTime], [IdNum], [SmsGiveSwitch], [RefUserId], [RefUserPath], [CreateTime], [Deleted]) VALUES (1, N'ef7c314b-c53e-4c6a-8cc1-4f14cbfa317d', N'admin', N'测试测试', N'18618477035', N'E10ADC3949BA59ABBE56E057F20F883E', N'', CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), N'::1', CAST(0x0000A6D700A49811 AS DateTime), N'123456110000000000', 0, 60002, NULL, CAST(0x0000A6CE00998F06 AS DateTime), 0)
INSERT [dbo].[User] ([Id], [UserGuid], [UserName], [DisplayName], [Phone], [Password], [Avatar], [TotalScore], [FrozenScore], [LastLoginIP], [LastLoginTime], [IdNum], [SmsGiveSwitch], [RefUserId], [RefUserPath], [CreateTime], [Deleted]) VALUES (2, N'7d2afb82-dafa-4e2f-be28-08c09213c2c2', N'13777777777', NULL, N'13777777777', N'E10ADC3949BA59ABBE56E057F20F883E', NULL, CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), N'::1', CAST(0x0000A6CE00BCA9DA AS DateTime), NULL, 0, NULL, NULL, CAST(0x0000A6CE00BCA9DA AS DateTime), 0)
INSERT [dbo].[User] ([Id], [UserGuid], [UserName], [DisplayName], [Phone], [Password], [Avatar], [TotalScore], [FrozenScore], [LastLoginIP], [LastLoginTime], [IdNum], [SmsGiveSwitch], [RefUserId], [RefUserPath], [CreateTime], [Deleted]) VALUES (3, N'3057d726-b78f-40a4-be09-1841c357c1fe', N'13111111111', NULL, N'13111111111', N'E10ADC3949BA59ABBE56E057F20F883E', NULL, CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), N'::1', CAST(0x0000A6CE00BD0487 AS DateTime), NULL, 0, NULL, NULL, CAST(0x0000A6CE00BD0487 AS DateTime), 0)
INSERT [dbo].[User] ([Id], [UserGuid], [UserName], [DisplayName], [Phone], [Password], [Avatar], [TotalScore], [FrozenScore], [LastLoginIP], [LastLoginTime], [IdNum], [SmsGiveSwitch], [RefUserId], [RefUserPath], [CreateTime], [Deleted]) VALUES (60002, N'e96f195d-677e-4a1e-a2fd-c496a0c13100', N'13222222222', NULL, N'13222222222', N'aa', NULL, CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), NULL, NULL, NULL, 0, 0, NULL, CAST(0x0000A6D500996041 AS DateTime), 0)
INSERT [dbo].[User] ([Id], [UserGuid], [UserName], [DisplayName], [Phone], [Password], [Avatar], [TotalScore], [FrozenScore], [LastLoginIP], [LastLoginTime], [IdNum], [SmsGiveSwitch], [RefUserId], [RefUserPath], [CreateTime], [Deleted]) VALUES (60003, N'61126e86-f558-46de-a92f-6e04ad99bf2c', N'13333333333', NULL, N'13333333333', N'E10ADC3949BA59ABBE56E057F20F883E', NULL, CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), N'::1', CAST(0x0000A6D600E5C3A0 AS DateTime), NULL, 0, 60003, N'60003;', CAST(0x0000A6D600E5C3A0 AS DateTime), 0)
INSERT [dbo].[User] ([Id], [UserGuid], [UserName], [DisplayName], [Phone], [Password], [Avatar], [TotalScore], [FrozenScore], [LastLoginIP], [LastLoginTime], [IdNum], [SmsGiveSwitch], [RefUserId], [RefUserPath], [CreateTime], [Deleted]) VALUES (60004, N'e0b1759f-e523-410b-9f28-f2471b0a22be', N'13444444444', N'测试', N'13444444444', N'E10ADC3949BA59ABBE56E057F20F883E', NULL, CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), N'::1', CAST(0x0000A6D600F053C6 AS DateTime), N'430602198301131555', 0, 60004, N'60004;', CAST(0x0000A6D600E92E90 AS DateTime), 0)
INSERT [dbo].[User] ([Id], [UserGuid], [UserName], [DisplayName], [Phone], [Password], [Avatar], [TotalScore], [FrozenScore], [LastLoginIP], [LastLoginTime], [IdNum], [SmsGiveSwitch], [RefUserId], [RefUserPath], [CreateTime], [Deleted]) VALUES (60005, N'cc630de0-cdb3-4874-9741-7edf6a787feb', N'13555555555', N'测试5', N'13555555555', N'E10ADC3949BA59ABBE56E057F20F883E', N'/Content/portrait/60005.jpg', CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), N'::1', CAST(0x0000A6D600F2AABB AS DateTime), N'12334234234234234', 0, 60004, NULL, CAST(0x0000A6D600F2AAAF AS DateTime), 0)
SET IDENTITY_INSERT [dbo].[User] OFF
SET IDENTITY_INSERT [dbo].[UserBankAccount] ON 

INSERT [dbo].[UserBankAccount] ([Id], [UserId], [Bank], [AccountName], [AccountNum], [CreateTime]) VALUES (1, 1, N'民生银行', N'测试测试', N'hhhhhhhhhhhhhhhhhhha', CAST(0x0000A6D300EE6B2E AS DateTime))
INSERT [dbo].[UserBankAccount] ([Id], [UserId], [Bank], [AccountName], [AccountNum], [CreateTime]) VALUES (3, 60004, N'中国邮政储蓄银行', N'测试', N'123123123123', CAST(0x0000A6D600EBDB64 AS DateTime))
INSERT [dbo].[UserBankAccount] ([Id], [UserId], [Bank], [AccountName], [AccountNum], [CreateTime]) VALUES (4, 3, N'中国邮政储蓄银行', N'测试', N'asdfasdfsad', CAST(0x0000A6D600EC20AE AS DateTime))
INSERT [dbo].[UserBankAccount] ([Id], [UserId], [Bank], [AccountName], [AccountNum], [CreateTime]) VALUES (5, 60005, N'兴业银行', N'测试5', N'12312312312', CAST(0x0000A6D600F2CADF AS DateTime))
SET IDENTITY_INSERT [dbo].[UserBankAccount] OFF
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
