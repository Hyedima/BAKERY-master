USE [codeicon_GobblesBakery]
GO

/****** Object:  Table [dbo].[production]    Script Date: 12/23/2023 9:29:23 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[production](
	[id] [varchar](200) NOT NULL,
	[name] [varchar](200) NULL,
	[decription] [varchar](200) NULL,
	[stockid] [varchar](200) NULL,
	[qty_bags] [int] NULL,
	[qty_items] [int] NULL,
	[insertdate] [datetime] NULL,
	[insertuser] [varchar](200) NULL,
 CONSTRAINT [PK_production] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[production] ADD  CONSTRAINT [DF_production_qty_bags]  DEFAULT ((0)) FOR [qty_bags]
GO

ALTER TABLE [dbo].[production] ADD  CONSTRAINT [DF_production_qty_items]  DEFAULT ((0)) FOR [qty_items]
GO

ALTER TABLE [dbo].[production]  WITH CHECK ADD  CONSTRAINT [FK_production_stocks] FOREIGN KEY([stockid])
REFERENCES [dbo].[stocks] ([id])
GO

ALTER TABLE [dbo].[production] CHECK CONSTRAINT [FK_production_stocks]
GO

ALTER TABLE [dbo].[production]  WITH CHECK ADD  CONSTRAINT [FK_production_useraccounts] FOREIGN KEY([insertuser])
REFERENCES [dbo].[useraccounts] ([id])
GO

ALTER TABLE [dbo].[production] CHECK CONSTRAINT [FK_production_useraccounts]
GO


