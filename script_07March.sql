USE [localDB]
GO
/****** Object:  Table [dbo].[PostalLookup]    Script Date: 2021/03/07 16:21:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PostalLookup](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PostalCode] [nvarchar](4) NULL,
	[TaxCalculationType] [nvarchar](50) NULL,
	[TaxCalculationDescriptor] [nvarchar](2) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RateLookup]    Script Date: 2021/03/07 16:21:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RateLookup](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Rate] [nvarchar](4) NULL,
	[FromLimit] [decimal](18, 2) NULL,
	[ToLimit] [decimal](18, 2) NULL,
	[RateCalcVal] [decimal](18, 2) NULL,
 CONSTRAINT [PK_RateLookup] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TaxCalculationsResult]    Script Date: 2021/03/07 16:21:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaxCalculationsResult](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PostalCode] [nvarchar](4) NULL,
	[AnnualIncome] [decimal](18, 2) NULL,
	[DateSubmitted] [datetime] NULL,
	[CalculatedTax] [decimal](18, 2) NULL,
	[CalculationType] [nvarchar](50) NULL,
 CONSTRAINT [PK_TaxCalcualtionsResult] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[PostalLookup] ON 

INSERT [dbo].[PostalLookup] ([Id], [PostalCode], [TaxCalculationType], [TaxCalculationDescriptor]) VALUES (1, N'7441', N'Progressive', N'P')
INSERT [dbo].[PostalLookup] ([Id], [PostalCode], [TaxCalculationType], [TaxCalculationDescriptor]) VALUES (2, N'A100', N'Flat Value', N'FV')
INSERT [dbo].[PostalLookup] ([Id], [PostalCode], [TaxCalculationType], [TaxCalculationDescriptor]) VALUES (3, N'7000', N'Flat Rate', N'FR')
INSERT [dbo].[PostalLookup] ([Id], [PostalCode], [TaxCalculationType], [TaxCalculationDescriptor]) VALUES (4, N'1000', N'Progressive', N'P')
SET IDENTITY_INSERT [dbo].[PostalLookup] OFF
GO
SET IDENTITY_INSERT [dbo].[RateLookup] ON 

INSERT [dbo].[RateLookup] ([Id], [Rate], [FromLimit], [ToLimit], [RateCalcVal]) VALUES (1, N'10%', CAST(0.00 AS Decimal(18, 2)), CAST(8350.00 AS Decimal(18, 2)), CAST(0.10 AS Decimal(18, 2)))
INSERT [dbo].[RateLookup] ([Id], [Rate], [FromLimit], [ToLimit], [RateCalcVal]) VALUES (2, N'15%', CAST(8351.00 AS Decimal(18, 2)), CAST(33950.00 AS Decimal(18, 2)), CAST(0.15 AS Decimal(18, 2)))
INSERT [dbo].[RateLookup] ([Id], [Rate], [FromLimit], [ToLimit], [RateCalcVal]) VALUES (3, N'25%', CAST(33951.00 AS Decimal(18, 2)), CAST(82250.00 AS Decimal(18, 2)), CAST(0.25 AS Decimal(18, 2)))
INSERT [dbo].[RateLookup] ([Id], [Rate], [FromLimit], [ToLimit], [RateCalcVal]) VALUES (4, N'28%', CAST(82251.00 AS Decimal(18, 2)), CAST(171550.00 AS Decimal(18, 2)), CAST(0.28 AS Decimal(18, 2)))
INSERT [dbo].[RateLookup] ([Id], [Rate], [FromLimit], [ToLimit], [RateCalcVal]) VALUES (5, N'33%', CAST(171551.00 AS Decimal(18, 2)), CAST(372950.00 AS Decimal(18, 2)), CAST(0.33 AS Decimal(18, 2)))
INSERT [dbo].[RateLookup] ([Id], [Rate], [FromLimit], [ToLimit], [RateCalcVal]) VALUES (6, N'35%', CAST(372951.00 AS Decimal(18, 2)), NULL, CAST(0.35 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[RateLookup] OFF
GO
SET IDENTITY_INSERT [dbo].[TaxCalculationsResult] ON 

INSERT [dbo].[TaxCalculationsResult] ([Id], [PostalCode], [AnnualIncome], [DateSubmitted], [CalculatedTax], [CalculationType]) VALUES (1, N'1000', CAST(0.00 AS Decimal(18, 2)), CAST(N'2021-03-07T13:04:46.710' AS DateTime), CAST(0.00 AS Decimal(18, 2)), N'Progressive')
INSERT [dbo].[TaxCalculationsResult] ([Id], [PostalCode], [AnnualIncome], [DateSubmitted], [CalculatedTax], [CalculationType]) VALUES (2, N'1000', CAST(1.00 AS Decimal(18, 2)), CAST(N'2021-03-07T13:06:28.223' AS DateTime), CAST(0.10 AS Decimal(18, 2)), N'Progressive')
INSERT [dbo].[TaxCalculationsResult] ([Id], [PostalCode], [AnnualIncome], [DateSubmitted], [CalculatedTax], [CalculationType]) VALUES (3, N'A100', CAST(1.00 AS Decimal(18, 2)), CAST(N'2021-03-07T13:06:40.117' AS DateTime), CAST(0.05 AS Decimal(18, 2)), N'Flat Value')
INSERT [dbo].[TaxCalculationsResult] ([Id], [PostalCode], [AnnualIncome], [DateSubmitted], [CalculatedTax], [CalculationType]) VALUES (4, N'1000', CAST(890765.00 AS Decimal(18, 2)), CAST(N'2021-03-07T13:25:08.547' AS DateTime), CAST(289451.25 AS Decimal(18, 2)), N'Progressive')
INSERT [dbo].[TaxCalculationsResult] ([Id], [PostalCode], [AnnualIncome], [DateSubmitted], [CalculatedTax], [CalculationType]) VALUES (5, N'1000', CAST(123456.00 AS Decimal(18, 2)), CAST(N'2021-03-07T13:35:17.003' AS DateTime), CAST(-59035.22 AS Decimal(18, 2)), N'Progressive')
INSERT [dbo].[TaxCalculationsResult] ([Id], [PostalCode], [AnnualIncome], [DateSubmitted], [CalculatedTax], [CalculationType]) VALUES (6, N'1000', CAST(123456.00 AS Decimal(18, 2)), CAST(N'2021-03-07T14:03:59.550' AS DateTime), CAST(28287.68 AS Decimal(18, 2)), N'Progressive')
INSERT [dbo].[TaxCalculationsResult] ([Id], [PostalCode], [AnnualIncome], [DateSubmitted], [CalculatedTax], [CalculationType]) VALUES (7, N'1000', CAST(400000.00 AS Decimal(18, 2)), CAST(N'2021-03-07T14:30:38.200' AS DateTime), CAST(117683.50 AS Decimal(18, 2)), N'Progressive')
INSERT [dbo].[TaxCalculationsResult] ([Id], [PostalCode], [AnnualIncome], [DateSubmitted], [CalculatedTax], [CalculationType]) VALUES (8, N'1000', CAST(400001.00 AS Decimal(18, 2)), CAST(N'2021-03-07T15:11:00.880' AS DateTime), CAST(117683.85 AS Decimal(18, 2)), N'Progressive')
INSERT [dbo].[TaxCalculationsResult] ([Id], [PostalCode], [AnnualIncome], [DateSubmitted], [CalculatedTax], [CalculationType]) VALUES (9, N'7000', CAST(400000.00 AS Decimal(18, 2)), CAST(N'2021-03-07T15:39:20.350' AS DateTime), CAST(70000.00 AS Decimal(18, 2)), N'Flat Rate')
INSERT [dbo].[TaxCalculationsResult] ([Id], [PostalCode], [AnnualIncome], [DateSubmitted], [CalculatedTax], [CalculationType]) VALUES (10, N'1000', CAST(234000.00 AS Decimal(18, 2)), CAST(N'2021-03-07T16:08:42.747' AS DateTime), CAST(62362.50 AS Decimal(18, 2)), N'Progressive')
SET IDENTITY_INSERT [dbo].[TaxCalculationsResult] OFF
GO
