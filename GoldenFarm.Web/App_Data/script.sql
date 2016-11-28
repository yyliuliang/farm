USE [master]
GO
/****** Object:  Database [farm]    Script Date: 2016/11/28 13:46:43 ******/
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
/****** Object:  Table [dbo].[KeyValueConfig]    Script Date: 2016/11/28 13:46:43 ******/
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
/****** Object:  Table [dbo].[Market]    Script Date: 2016/11/28 13:46:43 ******/
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
/****** Object:  Table [dbo].[MarketEntrust]    Script Date: 2016/11/28 13:46:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MarketEntrust](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[ProductId] [int] NULL,
	[Price] [decimal](18, 5) NULL,
	[Count] [int] NULL,
	[buy] [bit] NULL,
	[Status] [int] NULL,
	[Cancelled] [bit] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_MarketDelegate] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MarketTransaction]    Script Date: 2016/11/28 13:46:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MarketTransaction](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TransactionId] [uniqueidentifier] NOT NULL,
	[ProductId] [int] NULL,
	[BuyerId] [int] NULL,
	[SellerId] [int] NULL,
	[Volume] [int] NULL,
	[Price] [decimal](18, 5) NULL,
	[Date] [date] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_MarketTransaction] PRIMARY KEY CLUSTERED 
(
	[TransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[News]    Script Date: 2016/11/28 13:46:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[News](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](200) NULL,
	[Source] [varchar](200) NULL,
	[Author] [varchar](50) NULL,
	[ReadCount] [int] NOT NULL,
	[Content] [nvarchar](max) NULL,
	[CreateTime] [datetime] NOT NULL,
	[Creator] [int] NULL,
 CONSTRAINT [PK_News] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Product]    Script Date: 2016/11/28 13:46:43 ******/
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
/****** Object:  Table [dbo].[Role]    Script Date: 2016/11/28 13:46:43 ******/
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
/****** Object:  Table [dbo].[User]    Script Date: 2016/11/28 13:46:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[DisplayName] [nvarchar](200) NULL,
	[Password] [varchar](50) NULL,
	[Avatar] [varchar](200) NULL,
	[LastLoginIP] [varchar](50) NULL,
	[LastLoginTime] [datetime] NULL,
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
ALTER TABLE [dbo].[KeyValueConfig] ADD  CONSTRAINT [DF_KeyValueConfig_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[Market] ADD  CONSTRAINT [DF_ProductMarket_Date]  DEFAULT (getdate()) FOR [Date]
GO
ALTER TABLE [dbo].[Market] ADD  CONSTRAINT [DF_ProductMarket_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[MarketEntrust] ADD  CONSTRAINT [DF_MarketDelegate_Cancelled]  DEFAULT ((0)) FOR [Cancelled]
GO
ALTER TABLE [dbo].[MarketEntrust] ADD  CONSTRAINT [DF_MarketDelegate_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[MarketTransaction] ADD  CONSTRAINT [DF_MarketTransaction_Date]  DEFAULT (getdate()) FOR [Date]
GO
ALTER TABLE [dbo].[MarketTransaction] ADD  CONSTRAINT [DF_MarketTransaction_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[News] ADD  CONSTRAINT [DF_News_ReadCount]  DEFAULT ((0)) FOR [ReadCount]
GO
ALTER TABLE [dbo].[News] ADD  CONSTRAINT [DF_News_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[Role] ADD  CONSTRAINT [DF_Role_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
USE [master]
GO
ALTER DATABASE [farm] SET  READ_WRITE 
GO
