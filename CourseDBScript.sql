create database RequestProcessingApplicationDB;
go
USE [RequestProcessingApplicationDB]
GO
/****** Object:  UserDefinedFunction [dbo].[IsUserExist]    Script Date: 20.11.2023 17:22:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[IsUserExist](@login nchar(30), @password nchar(30), @type nchar(30))
returns int
as
begin
	declare @id int;
	if(@type='user')
		set @id = (select Id from UsersCredentials where Login=@login and Password=@password);
	else if(@type='admin')
		set @id = (select Id from AdminsCredentials where Login=@login and Password=@password);
	
	if(@id is NULL) 
		set @id=-1;
	return @id;
end
GO
/****** Object:  Table [dbo].[Answers]    Script Date: 20.11.2023 17:22:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Answers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoutedRequestId] [int] NOT NULL,
	[Message] [nchar](300) NOT NULL,
	[Date] [datetime] NOT NULL,
	[IsReaded] [bit] NOT NULL,
 CONSTRAINT [PK_Answers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[RequestAnswers]    Script Date: 20.11.2023 17:22:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[RequestAnswers](@RequestId int) 
returns table 
as
return (select Id, Message, Date, IsReaded from Answers where RoutedRequestId=@RequestId)
GO
/****** Object:  Table [dbo].[Requests]    Script Date: 20.11.2023 17:22:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Requests](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Title] [nchar](100) NOT NULL,
	[Message] [nchar](300) NOT NULL,
	[State] [nchar](30) NOT NULL,
	[RequestedDate] [datetime] NOT NULL,
	[DoneDate] [datetime] NULL,
 CONSTRAINT [PK__Requests__3214EC07CB9A668A] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[GetRequestsAndUnreadMessageCountByUser]    Script Date: 20.11.2023 17:22:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetRequestsAndUnreadMessageCountByUser](@UserId INT)
RETURNS TABLE
AS
RETURN
(
    SELECT
        r.*,
        COALESCE(umc.UnreadMessageCount, 0) AS UnreadMessageCount
    FROM
        Requests r
    LEFT JOIN
        (SELECT
            RoutedRequestId,
            COUNT(CASE WHEN IsReaded = 0 THEN 1 END) AS UnreadMessageCount
        FROM
            Answers
        GROUP BY
            RoutedRequestId) umc ON r.Id = umc.RoutedRequestId
    WHERE
        r.UserId = @UserId
);
GO
/****** Object:  UserDefinedFunction [dbo].[UserRequests]    Script Date: 20.11.2023 17:22:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[UserRequests](@RequestedUserId int) 
returns table 
as
return (select * from Requests where UserId=@RequestedUserId)
GO
/****** Object:  Table [dbo].[AdminsCredentials]    Script Date: 20.11.2023 17:22:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdminsCredentials](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Login] [nchar](30) NOT NULL,
	[Password] [nchar](30) NOT NULL,
 CONSTRAINT [AdminId_UK] UNIQUE NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AdminLogin_UK] UNIQUE NONCLUSTERED 
(
	[Login] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsersCredentials]    Script Date: 20.11.2023 17:22:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsersCredentials](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Login] [nchar](30) NOT NULL,
	[Password] [nchar](30) NOT NULL,
 CONSTRAINT [UserID_UK] UNIQUE NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UserLogin_UK] UNIQUE NONCLUSTERED 
(
	[Login] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Answers]  WITH CHECK ADD  CONSTRAINT [FK_Answers_RequestId] FOREIGN KEY([RoutedRequestId])
REFERENCES [dbo].[Requests] ([Id])
GO
ALTER TABLE [dbo].[Answers] CHECK CONSTRAINT [FK_Answers_RequestId]
GO
ALTER TABLE [dbo].[Requests]  WITH CHECK ADD  CONSTRAINT [FK__Requests__UserId__3E52440B] FOREIGN KEY([UserId])
REFERENCES [dbo].[UsersCredentials] ([Id])
GO
ALTER TABLE [dbo].[Requests] CHECK CONSTRAINT [FK__Requests__UserId__3E52440B]
GO
ALTER TABLE [dbo].[Requests]  WITH CHECK ADD  CONSTRAINT [check_State] CHECK  (([State]='Done' OR [State]='Viewed' OR [State]='Processing' OR [State]='Unviewed'))
GO
ALTER TABLE [dbo].[Requests] CHECK CONSTRAINT [check_State]
GO

insert into UsersCredentials values('test'                          ,'test'                          )
,('Maxlyn'                        ,'AaBcD1'                        )

insert into Requests values(1                             ,'Запрос о подтверждении бронирования                                                                 ','Привет! Подтвердите, пожалуйста, мою бронь на указанные даты. Необходима информация о номере и условиях проживания.                                                                                                                                                                                         ','Done                          ','Nov  6 2023  2:01AM           ','Nov 17 2023 10:39PM           ')
,(1                             ,'Вопрос о доступности парковки                                                                       ','Планирую приезд на машине. Есть ли у вас парковка для гостей, и нужно ли резервировать место?                                                                                                                                                                                                               ','Done                          ','Nov  6 2023  2:03AM           ','Nov 20 2023  7:33PM           ')
,(1                             ,'Запрос о раннем заезде                                                                              ','Мой рейс рано утром. Возможен ли ранний заезд, и есть ли дополнительные затраты?                                                                                                                                                                                                                            ','Done                          ','Nov  6 2023  2:03AM           ','Nov 20 2023  7:38PM           ')
,(1                             ,'Вопрос о политике отмены                                                                            ','Каковы условия отмены бронирования? Возможно ли отменить бронь бесплатно?                                                                                                                                                                                                                                   ','Done                          ','Nov  6 2023  2:03AM           ','Nov 17 2023 10:40PM           ')
,(1                             ,'Запрос о дополнительных услугах                                                                     ','Предоставляете ли дополнительные услуги, такие как завтрак, трансфер или экскурсии?                                                                                                                                                                                                                         ','Done                          ','Nov  6 2023  2:03AM           ','Nov 20 2023  7:34PM           ')
,(1                             ,'Вопрос о Wi-Fi соединении                                                                           ','Какова скорость Wi-Fi? Будет ли стабильное соединение?                                                                                                                                                                                                                                                      ','Done                          ','Nov  6 2023  2:04AM           ','Nov 18 2023  7:50PM           ')
,(1                             ,'Запрос о дополнительных кроватях                                                                    ','Можно ли добавить дополнительные кровати для детей в номер?                                                                                                                                                                                                                                                 ','Done                          ','Nov  6 2023  2:04AM           ','Nov 19 2023  6:28PM           ')
,(1                             ,'Вопрос о ресторанах и достопримечательностях                                                        ','Порекомендуйте, пожалуйста, ближайшие рестораны и достопримечательности.                                                                                                                                                                                                                                    ','Processing                    ','Nov  6 2023  2:04AM           ','Jan  1 1900 12:00AM           ')
,(1                             ,'Запрос о услугах для деловых путешественников                                                       ','Есть ли у вас услуги для деловых поездок, такие как конференц-залы?                                                                                                                                                                                                                                         ','Done                          ','Nov  6 2023  2:04AM           ','Nov 19 2023  6:28PM           ')
,(1                             ,'Вопрос об условиях с питомцами                                                                      ','Каковы правила проживания с домашними животными?                                                                                                                                                                                                                                                            ','Processing                    ','Nov  6 2023  2:04AM           ','Jan  1 1900 12:00AM           ')
,(1                             ,'Помощь с Бронированием                                                                              ','Планирую посещение вашего отеля и хотел бы уточнить несколько вопросов перед бронированием. Можете ли вы подсказать доступность номеров на 22.01.23, предоставить информацию о ценах и возможных акциях?                                                                                             ','Done                          ','Nov 19 2023  5:46PM           ','Nov 20 2023  7:38PM           ')

insert into AdminsCredentials values('admin'                         ,'admin'                         )
,('vasya'                         ,'123'                           )
,('petya'                         ,'123'                           )


insert into Answers values(1                            ,'Предоставляет всё в полной мере'                                                                                                                                                                                                                                                                             ,'Nov 20 2023  7:34PM'           ,1)
,(1                            ,'Возможен'                                                                                                                                                                                                                                                                                                    ,'Nov 20 2023  7:38PM'           ,0)
,(2                            ,'возможен'                                                                                                                                                                                                                                                                                                    ,'Nov 20 2023  7:38PM'           ,0)
,(3                            ,'хорошо'                                                                                                                                                                                                                                                                                                      ,'Nov 20 2023  7:38PM'           ,0)