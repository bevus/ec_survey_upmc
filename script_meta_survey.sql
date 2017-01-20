USE [meta_survey]
GO
/****** Object:  StoredProcedure [dbo].[get_hased_id]    Script Date: 1/20/2017 9:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[get_hased_id]
	@hash nvarchar(60),
	@poll_id int
as
begin
	select PERI_person_id as person_id from POLL_PERSON_ID, PERSON where
	PERI_person_id = PER_id_person and
	PERI_id = @hash and
	PERI_poll_id = @poll_id
end
GO
/****** Object:  StoredProcedure [dbo].[get_rep_person]    Script Date: 1/20/2017 9:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[get_rep_person]
	@poll_id int,
	@person_id int,
	@table nvarchar(255),
	@activity_id int,
	@question_type nvarchar(50),
	@column_name nvarchar(255)
as
BEGIN
	declare @query nvarchar(2000);

	SET @query = 'SELECT ' + @column_name + ' FROM ' + @table + ' WHERE ';
	IF @question_type = 'General'
	BEGIN
		SET @query = @query + 'id_poll = ' + convert(nvarchar(50), @poll_id) + ' and id_person = ' + convert(nvarchar(50), @person_id)
	END

	IF @question_type = 'Meeting'
	BEGIN
		SET @query = @query + 'SUM_id_poll = ' + convert(nvarchar(50), @poll_id) + ' and SUM_id_meeting = ' + convert(nvarchar(50), @activity_id) + ' and SUM_id_person = ' + convert(nvarchar(50), @person_id);
	END

	IF @question_type = 'Activity'
	BEGIN
		SET @query = @query + 'SUB_id_poll = ' + convert(nvarchar(50), @poll_id) + ' and SUB_id_atelier = ' + convert(nvarchar(50), @activity_id) + ' and SUB_id_person = ' + convert(nvarchar(50), @person_id);
	END

	IF @question_type = 'Workshop'
	BEGIN
		SET @query = @query + 'SUB_id_poll = ' + convert(nvarchar(50), @poll_id) + ' and SUB_id_atelier = ' + convert(nvarchar(50), @activity_id) + ' and SUB_id_person = ' + convert(nvarchar(50), @person_id);
	END
	PRINT @query;
	EXEC(@query);
END
GO
/****** Object:  StoredProcedure [dbo].[i_generated_poll]    Script Date: 1/20/2017 9:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[i_generated_poll]
	@poll_id int,
	@path nvarchar(60),
	@arg nvarchar(20)
as
insert into  dbo.POLL_GENERATED_SURVEY(
	GEN_poll_id,
	GEN_path,
	GEN_arg
) values (
	@poll_id,
	@path,
	@arg
);
GO
/****** Object:  StoredProcedure [dbo].[i_poll_survey]    Script Date: 1/20/2017 9:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[i_poll_survey]
	@poll_id int,
	@person_id int,
	@saved_date date
as
insert into POLL_SURVEY (
	SUR_id_poll,
	SUR_id_person,
	SUR_saved,
	SUR_saved_date
) values (
	@poll_id,
	@person_id,
	1,
	@saved_date
);
GO
/****** Object:  StoredProcedure [dbo].[sel_blocks]    Script Date: 1/20/2017 9:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[sel_blocks]
	@poll_id int
as
	select distinct POLL_BLOCK.* from POLL, POLL_QUESTION, POLL_BLOCK where POLL.POL_id_poll = POLL_QUESTION.QUE_id_poll and POLL_QUESTION.QUE_id_block = POLL_BLOCK.POLB_id_block and POLL.POL_id_poll = @poll_id;

/*****************************/


GO
/****** Object:  StoredProcedure [dbo].[sel_choices]    Script Date: 1/20/2017 9:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sel_choices]
	@question_id int
as
select 
CHO_id_choice as choice_id
,CHO_id_question as question_id
,CHO_choice_label as choice_label
,CHO_order as choice_order
,CHO_choice_value as choice_value
from dbo.POLL_CHOICE where CHO_id_question = @question_id;

/******************************/


GO
/****** Object:  StoredProcedure [dbo].[SEL_empty_survey]    Script Date: 1/20/2017 9:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Maïssa BAKER - SEAL - Event Catalyst
-- Create date: 2016-11-06
-- Description:	Create a possible choice for a question
-- =============================================
CREATE PROCEDURE [dbo].[SEL_empty_survey]
AS
BEGIN

	delete POLL
	delete POLL_BLOCK
	delete POLL_CHOICE
	delete POLL_MEETING_CHOICE
	delete POLL_MEETING_QUESTION
	delete POLL_QUESTION
	delete POLL_SESSION_CHOICE
	delete POLL_SESSION_QUESTION
	delete POLL_WS_CHOICE
	delete POLL_WS_QUESTION

	DBCC CHECKIDENT ('POLL', RESEED, 0);
	DBCC CHECKIDENT ('POLL_BLOCK', RESEED, 0);
	DBCC CHECKIDENT ('POLL_CHOICE', RESEED, 0);
	DBCC CHECKIDENT ('POLL_MEETING_CHOICE', RESEED, 0);
	DBCC CHECKIDENT ('POLL_MEETING_QUESTION', RESEED, 0);	
	DBCC CHECKIDENT ('POLL_QUESTION', RESEED, 0);
	DBCC CHECKIDENT ('POLL_SESSION_CHOICE', RESEED, 0);
	DBCC CHECKIDENT ('POLL_SESSION_QUESTION', RESEED, 0);
	DBCC CHECKIDENT ('POLL_WS_CHOICE', RESEED, 0);
	DBCC CHECKIDENT ('POLL_WS_QUESTION', RESEED, 0);

END


GO
/****** Object:  StoredProcedure [dbo].[SEL_generate_survey]    Script Date: 1/20/2017 9:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Maïssa BAKER - SeAL - Event Catalyst
-- Create date: 2016-11-06
-- Description:	Create all Table for one survey
-- =============================================
CREATE PROCEDURE [dbo].[SEL_generate_survey]
	@id_poll int
AS
BEGIN
	declare @MySurvey nvarchar(4000)
	declare @MyMeetingSurvey nvarchar(4000)
	declare @MyWsSurvey nvarchar(4000)
	declare @MySessionSurvey nvarchar(4000)

	declare @TableSurvey nvarchar(255)
	declare @TableMeetingSurvey nvarchar(255)
	declare @TableWsSurvey nvarchar(255)
	declare @TableSessionSurvey nvarchar(255)

	DECLARE @column nvarchar(50)
	DECLARE @max_size INT

	select @TableSurvey = POL_table_name, @TableMeetingSurvey = POL_table_meeting_name, @TableWsSurvey = POL_table_ws_name, @TableSessionSurvey = POL_table_session_name
	from POLL
	where POL_id_poll = @id_poll

	IF OBJECT_ID (N'POLL_SURVEY', N'U') IS NULL
	BEGIN
		-- create table poll survey
		CREATE TABLE [dbo].[POLL_SURVEY](
			[SUR_id_poll] [int] NOT NULL,
			[SUR_id_person] [int] NOT NULL,
			[SUR_clicked] [bit] NULL,
			[SUR_clicked_date] [datetime] NULL,
			[SUR_saved] [bit] NULL,
			[SUR_saved_date] [datetime] NULL,
			[SUR_comments] [nvarchar](4000) NULL,
			[SUR_date_mod] [datetime] NULL,
			[SUR_page] [nchar](255) NULL,
		 CONSTRAINT [PK_POLL_SURVEY] PRIMARY KEY CLUSTERED 
		(
			[SUR_id_poll] ASC,
			[SUR_id_person] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]
	END

	IF OBJECT_ID (@TableSurvey, N'U') IS NULL and exists (select QUE_id_question from POLL_QUESTION where QUE_id_poll = @id_poll)
	BEGIN
		SET @MySurvey = 'CREATE TABLE [dbo].['+ @TableSurvey+ ']([id_person] [int] NOT NULL, [id_poll] [int] NOT NULL '
		
		DECLARE MyCursor CURSOR FOR
		select QUE_column, isnull(QUE_max_size, 255) from POLL_QUESTION where QUE_column IS NOT NULL and QUE_id_poll = @id_poll order by QUE_page_number, QUE_id_block, QUE_order, QUE_id_question

		--open cursor and etch and init variable
		OPEN MyCursor
		FETCH NEXT FROM MyCursor INTO @column, @max_size
		WHILE @@FETCH_STATUS = 0
		BEGIN
			-- Do Something
			SET @MySurvey = @MySurvey + ', [' + @column+ '] [nvarchar](' + convert(varchar(50), @max_size) + ') NULL'

			-- Fetch and init variable
			FETCH NEXT FROM MyCursor INTO @column, @max_size
		END
		-- Close cursor
		CLOSE MyCursor
		-- Deallocate
		DEALLOCATE MyCursor

		SET @MySurvey = @MySurvey + ', 
		CONSTRAINT [PK_'+ @TableSurvey + '] PRIMARY KEY CLUSTERED ([id_person] ASC, [id_poll] ASC)) ON [PRIMARY]'

		SET @MySurvey = @MySurvey + '
		ALTER TABLE [dbo].['+ @TableSurvey + '] ADD CONSTRAINT [DF_'+ @TableSurvey + '_id_poll] DEFAULT (('+ CONVERT(varchar(50), @id_poll) +')) FOR [id_poll] '
		EXEC (@MySurvey)
	END
	IF OBJECT_ID (@TableMeetingSurvey, N'U') IS NULL and exists (select MQUE_id_meeting_question from POLL_MEETING_QUESTION join POLL_QUESTION on MQUE_id_question = QUE_id_question where QUE_id_poll = @id_poll)
	BEGIN
		SET @MyMeetingSurvey = 'CREATE TABLE [dbo].['+ @TableMeetingSurvey+ ']([SUM_id_survey] [int] IDENTITY(1,1) NOT NULL, [SUM_id_poll] [int] NOT NULL, [SUM_id_meeting][int] NOT NULL, [SUM_id_person] [int] NOT NULL, [SUM_id_company] [int] NOT NULL, [SUM_date_mod] [datetime] NULL '
		
		DECLARE MyCursor CURSOR FOR
		select MQUE_column, isnull(MQUE_max_size, 255) from POLL_MEETING_QUESTION join POLL_QUESTION on MQUE_id_question = QUE_id_question where QUE_id_poll = @id_poll order by QUE_page_number, QUE_id_block, QUE_order, MQUE_order

		--open cursor and etch and init variable
		OPEN MyCursor
		FETCH NEXT FROM MyCursor INTO @column, @max_size
		WHILE @@FETCH_STATUS = 0
		BEGIN
			-- Do Something
			SET @MyMeetingSurvey = @MyMeetingSurvey + ', [SUM_' + @column+ '] [nvarchar](' + convert(varchar(50), @max_size) + ') NULL'

			-- Fetch and init variable
			FETCH NEXT FROM MyCursor INTO @column, @max_size
		END
		-- Close cursor
		CLOSE MyCursor
		-- Deallocate
		DEALLOCATE MyCursor

		SET @MyMeetingSurvey = @MyMeetingSurvey + ', 
		CONSTRAINT [PK_'+ @TableMeetingSurvey + '] PRIMARY KEY CLUSTERED (SUM_id_survey ASC, SUM_id_poll, SUM_id_meeting, SUM_id_person, SUM_id_company)) ON [PRIMARY]'

		SET @MyMeetingSurvey = @MyMeetingSurvey + '
		ALTER TABLE [dbo].['+ @TableMeetingSurvey + '] ADD CONSTRAINT [DF_'+ @TableMeetingSurvey + '_SUM_id_poll] DEFAULT (('+ CONVERT(varchar(50), @id_poll) +')) FOR [SUM_id_poll] '

		EXEC (@MyMeetingSurvey)
	END

	IF OBJECT_ID (@TableWsSurvey, N'U') IS NULL and exists (select WSQUE_id_ws_question from POLL_WS_QUESTION join POLL_QUESTION on WSQUE_id_question = QUE_id_question where QUE_id_poll = @id_poll)
	BEGIN
		SET @MyWsSurvey = 'CREATE TABLE [dbo].['+ @TableWsSurvey+ ']([SUB_id_survey] [int] IDENTITY(1,1) NOT NULL, [SUB_id_poll] [int] NOT NULL,[SUB_id_person] [int] NOT NULL, [SUB_id_atelier] [int] NOT NULL, [SUB_attended] [bit] NULL, [SUB_date_mod] [datetime] NULL '

		DECLARE MyCursor CURSOR FOR
		select WSQUE_column, isnull(WSQUE_max_size, 255) from POLL_WS_QUESTION join POLL_QUESTION on WSQUE_id_question = QUE_id_question where QUE_id_poll = @id_poll order by QUE_page_number, QUE_id_block, QUE_order, WSQUE_order 


		--open cursor and etch and init variable
		OPEN MyCursor
		FETCH NEXT FROM MyCursor INTO @column, @max_size
		WHILE @@FETCH_STATUS = 0
		BEGIN
			-- Do Something
			SET @MyWsSurvey = @MyWsSurvey + ', [SUB_' + @column+ '] [nvarchar](' + convert(varchar(50), @max_size) + ') NULL'

			-- Fetch and init variable
			FETCH NEXT FROM MyCursor INTO @column, @max_size
		END
		-- Close cursor
		CLOSE MyCursor
		-- Deallocate
		DEALLOCATE MyCursor

		SET @MyWsSurvey = @MyWsSurvey + ', 
		CONSTRAINT [PK_'+ @TableWsSurvey + '] PRIMARY KEY CLUSTERED (SUB_id_survey ASC, SUB_id_poll, SUB_id_person, SUB_id_atelier)) ON [PRIMARY]'

		SET @MyWsSurvey = @MyWsSurvey + '
		ALTER TABLE [dbo].['+ @TableWsSurvey + '] ADD CONSTRAINT [DF_'+ @TableWsSurvey + '_SUB_id_poll] DEFAULT (('+ CONVERT(varchar(50), @id_poll) +')) FOR [SUB_id_poll] '

		SET @MyWsSurvey = @MyWsSurvey + '
		ALTER TABLE [dbo].['+ @TableWsSurvey + '] ADD CONSTRAINT [DF_'+ @TableWsSurvey + '_SUB_attended] DEFAULT ((0)) FOR [SUB_attended] '

		EXEC (@MyWsSurvey)
	END

	IF OBJECT_ID (@TableSessionSurvey, N'U') IS NULL and exists (select SQUE_id_session_question from POLL_SESSION_QUESTION  join POLL_QUESTION on SQUE_id_question = QUE_id_question where QUE_id_poll = @id_poll)
	BEGIN
		SET @MySessionSurvey = 'CREATE TABLE [dbo].['+ @TableSessionSurvey+ ']([SUB_id_survey] [int] IDENTITY(1,1) NOT NULL, [SUB_id_poll] [int] NOT NULL,[SUB_id_person] [int] NOT NULL, [SUB_id_atelier] [int] NOT NULL, [SUB_attended] [bit] NULL, [SUB_date_mod] [datetime] NULL '

		DECLARE MyCursor CURSOR FOR
		select SQUE_column, isnull(SQUE_max_size, 255) from POLL_SESSION_QUESTION  join POLL_QUESTION on SQUE_id_question = QUE_id_question where QUE_id_poll = @id_poll order by QUE_page_number, QUE_id_block, QUE_order, SQUE_order 

		--open cursor and etch and init variable
		OPEN MyCursor
		FETCH NEXT FROM MyCursor INTO @column, @max_size
		WHILE @@FETCH_STATUS = 0
		BEGIN
			-- Do Something
			SET @MySessionSurvey = @MySessionSurvey + ', [SUB_' + @column+ '] [nvarchar](' + convert(varchar(50), @max_size) + ') NULL'

			-- Fetch and init variable
			FETCH NEXT FROM MyCursor INTO @column, @max_size
		END
		-- Close cursor
		CLOSE MyCursor
		-- Deallocate
		DEALLOCATE MyCursor

		SET @MySessionSurvey = @MySessionSurvey + ', 
		CONSTRAINT [PK_'+ @TableSessionSurvey + '] PRIMARY KEY CLUSTERED (SUB_id_survey ASC, SUB_id_poll, SUB_id_person, SUB_id_atelier)) ON [PRIMARY]'

		SET @MySessionSurvey = @MySessionSurvey + '
		ALTER TABLE [dbo].['+ @TableSessionSurvey + '] ADD CONSTRAINT [DF_'+ @TableSessionSurvey + '_SUB_id_poll] DEFAULT (('+ CONVERT(varchar(50), @id_poll) +')) FOR [SUB_id_poll] '

		SET @MySessionSurvey = @MySessionSurvey + '
		ALTER TABLE [dbo].['+ @TableSessionSurvey + '] ADD CONSTRAINT [DF_'+ @TableSessionSurvey + '_SUB_attended] DEFAULT ((0)) FOR [SUB_attended] '

		EXEC (@MySessionSurvey)
	END
END


GO
/****** Object:  StoredProcedure [dbo].[sel_list_poll]    Script Date: 1/20/2017 9:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[sel_list_poll]
as
begin
	select * from POLL;
end
GO
/****** Object:  StoredProcedure [dbo].[sel_meetings]    Script Date: 1/20/2017 9:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sel_meetings]
	@id_person int,
	@id_event int
as
	select * from MEETING, TIMEFRAME, LOCATION, COMPANY where 
	MEETING.MEE_id_timeframe = TIMEFRAME.TIM_id_timeframe and
	MEETING.MEE_id_location = LOCATION.LOC_id_location and
	MEETING.MEE_id_company = COMPANY.COM_id_company and
	MEE_id_person = @id_person and MEE_id_event = @id_event;

GO
/****** Object:  StoredProcedure [dbo].[sel_person]    Script Date: 1/20/2017 9:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[sel_person]
	@person_id int
as
	select count(*) as c from PERSON where PER_id_person = @person_id;
GO
/****** Object:  StoredProcedure [dbo].[sel_poll]    Script Date: 1/20/2017 9:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[sel_poll] 
    @poll_id int
AS    
    SELECT * 
    FROM dbo.POLL  
    WHERE POL_id_poll = @poll_id;  

/********************************/


GO
/****** Object:  StoredProcedure [dbo].[sel_poll_survey]    Script Date: 1/20/2017 9:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[sel_poll_survey]
	@poll_id int,
	@person_id int
as
select count(SUR_id_person) as c from POLL_SURVEY where SUR_id_person = @person_id and SUR_id_poll = @poll_id;
GO
/****** Object:  StoredProcedure [dbo].[sel_questions]    Script Date: 1/20/2017 9:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sel_questions]
	@poll_id int
as
select 
 QUE_id_question as question_id
,QUE_label as question_label
,QUE_description as question_description
,QUE_order as question_order
,QUE_max_size as question_max_size
,QUE_nb_choice as question_nb_choice
,QUE_mandatory as question_mandatory
,QUE_number as question_number
,QUE_id_choice_type as question_id_choice_type
,QUE_column as question_column
,QUE_id_parent 
,QUE_id_poll
,QUE_id_block
,QUE_id_question_type
,dbo.POLL_QUESTION_TYPE.QUET_question_type
,dbo.POLL_CHOICE_TYPE.CHOT_choice_type
,dbo.POLL_CHOICE_TYPE.CHOT_prefix 
from
dbo.POLL_QUESTION JOIN dbo.POLL_QUESTION_TYPE
ON dbo.POLL_QUESTION.QUE_id_question_type = dbo.POLL_QUESTION_TYPE.QUET_id
LEFT JOIN dbo.POLL_CHOICE_TYPE ON 
dbo.POLL_QUESTION.QUE_id_choice_type = dbo.POLL_CHOICE_TYPE.CHOT_id
and QUE_id_poll = @poll_id order by POLL_QUESTION.QUE_order;



GO
/****** Object:  StoredProcedure [dbo].[sel_sessions]    Script Date: 1/20/2017 9:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*********************/

CREATE procedure [dbo].[sel_sessions]
	@id_person int,
	@id_event int
as
	select * from SELECTION_SESSION JOIN ATELIER_SESSION 
	ON SELECTION_SESSION.id_atelier = ATELIER_SESSION.id_atelier
	where id_person = @id_person and id_event = @id_event and attended = 1;

GO
/****** Object:  StoredProcedure [dbo].[sel_sub_questions]    Script Date: 1/20/2017 9:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sel_sub_questions]
	@question_id int,
	@question_type nvarchar(50)
as
begin
	declare @query nvarchar(4000);

	SET @query = 'select ';
	IF @question_type = 'Activity'
	BEGIN
	  SET @query = @query + 
	  'SQUE_id_session_question as question_id 
      ,SQUE_id_question as question_parent_id
      ,SQUE_label as question_label
      ,SQUE_description as question_description
      ,SQUE_order as question_order
      ,SQUE_max_size as question_max_size
      ,SQUE_nb_choice as question_nb_choice
      ,SQUE_mandatory as question_mandatory
      ,SQUE_number as question_number
      ,SQUE_id_choice_type as question_id_choice_type
      ,SQUE_column as question_column';
	END

	IF @question_type = 'Workshop'
	BEGIN
	  SET @query = @query +
	  'WSQUE_id_ws_question as question_id 
      ,WSQUE_id_question as question_parent_id
      ,WSQUE_label as question_label
      ,WSQUE_description as question_description
      ,WSQUE_order as question_order
      ,WSQUE_max_size as question_max_size
      ,WSQUE_nb_choice as question_nb_choice
      ,WSQUE_mandatory as question_mandatory
      ,WSQUE_number as question_number
      ,WSQUE_id_choice_type as question_id_choice_type
      ,WSQUE_column as question_column';
	END

	IF @question_type = 'Meeting'
	BEGIN
	   SET @query = @query +
	  'MQUE_id_meeting_question as question_id 
      ,MQUE_id_question as question_parent_id
      ,MQUE_label as question_label
      ,MQUE_description as question_description
      ,MQUE_order as question_order
      ,MQUE_max_size as question_max_size
      ,MQUE_nb_choice as question_nb_choice
      ,MQUE_mandatory as question_mandatory
      ,MQUE_number as question_number
      ,MQUE_id_choice_type as question_id_choice_type
      ,MQUE_column as question_column';
	END

	SET @query = @query + ', POLL_CHOICE_TYPE.* from ';

	IF @question_type = 'Activity'
	BEGIN
		SET @query = @query + 'POLL_SESSION_QUESTION 
		JOIN POLL_CHOICE_TYPE ON SQUE_id_choice_type = POLL_CHOICE_TYPE.CHOT_id
		WHERE SQUE_id_question = ' + convert(nvarchar(50), @question_id) + ' order by SQUE_order';
	END
	IF @question_type = 'Workshop'
	BEGIN
		SET @query = @query + 'POLL_WS_QUESTION 
		JOIN POLL_CHOICE_TYPE ON WSQUE_id_choice_type = POLL_CHOICE_TYPE.CHOT_id
		WHERE WSQUE_id_question = ' + convert(nvarchar(50), @question_id) + ' order by WSQUE_order';
	END
	IF @question_type = 'Meeting'
	BEGIN
		SET @query = @query + 'POLL_MEETING_QUESTION JOIN POLL_CHOICE_TYPE 
		ON MQUE_id_choice_type = POLL_CHOICE_TYPE.CHOT_id
		WHERE MQUE_id_question = ' + convert(nvarchar(50), @question_id) + ' order by MQUE_order';
	END
	PRINT @query;
	EXEC(@query);
	PRINT @query;
END



GO
/****** Object:  StoredProcedure [dbo].[sel_sub_questions_choices]    Script Date: 1/20/2017 9:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sel_sub_questions_choices]
	@question_id int,
	@question_type nvarchar(50)
as
begin
	declare @query nvarchar(4000);

	SET @query = 'select ';
	IF @question_type = 'Activity'
	BEGIN
	  SET @query = @query + 
	  'SCHO_id_session_choice as choice_id
      ,SCHO_id_session_question as question_id
      ,SCHO_choice_label choice_label
      ,SCHO_order choice_order
      ,SCHO_choice_value choice_value';
	END

	IF @question_type = 'Workshop'
	BEGIN
	  SET @query = @query +
	  'WSCHO_id_ws_choice as choice_id
      ,WSCHO_id_ws_question as question_id
      ,WSCHO_choice_label choice_label
      ,WSCHO_order choice_order
      ,WSCHO_choice_value choice_value';
	END

	IF @question_type = 'Meeting'
	BEGIN
	   SET @query = @query +
	  'MCHO_id_meeting_choice as choice_id
      ,MCHO_id_meeting_question as question_id
      ,MCHO_choice_label choice_label
      ,MCHO_order choice_order
      ,MCHO_choice_value choice_value';
	END

	SET @query = @query + ' from ';

		IF @question_type = 'Activity'
	BEGIN
		SET @query = @query + 'POLL_SESSION_CHOICE 
		WHERE SCHO_id_session_question = ' + convert(nvarchar(50), @question_id) + ' order by SCHO_order';
	END
	IF @question_type = 'Workshop'
	BEGIN
		SET @query = @query + 'POLL_WS_CHOICE 
		WHERE WSCHO_id_ws_question = ' + convert(nvarchar(50), @question_id) + ' order by WSCHO_order';
	END
	IF @question_type = 'Meeting'
	BEGIN
		SET @query = @query + 'POLL_MEETING_CHOICE 
		WHERE MCHO_id_meeting_question = ' + convert(nvarchar(50), @question_id) + ' order by MCHO_order';
	END
	PRINT @query;
	EXEC(@query);
END
GO
/****** Object:  StoredProcedure [dbo].[sel_workshops]    Script Date: 1/20/2017 9:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/********************/
create procedure [dbo].[sel_workshops]
	@id_person int,
	@id_event int
as
	select * from SELECTION_WS JOIN ATELIER_WS 
	ON SELECTION_WS.id_atelier = ATELIER_WS.id_atelier
	where id_person = @id_person and id_event = @id_event and attended = 1;

GO
/****** Object:  StoredProcedure [dbo].[SELP100]    Script Date: 1/20/2017 9:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Maïssa BAKER - SEAL - Event Catalyst
-- Create date: 2016-11-06
-- Description:	Create a pool survey
-- =============================================
CREATE PROCEDURE [dbo].[SELP100]
(
    @start_date datetime,
    @end_date datetime,
    @name nvarchar(255),
    @description nvarchar(255),
	@id_event int = 1
)
AS
BEGIN
	declare @id_pool int = -1

	declare @external_id varchar(50)
	declare @table_prefix varchar(100)
	set @external_id = REPLACE(newid(), '-', '')
	set @table_prefix = 'POLL_SURVEY_'

	INSERT INTO [dbo].[POLL] ([POL_id_event] ,[POL_start_date] ,[POL_end_date] ,[POL_name] ,[POL_description] ,[POL_external_id] ,[POL_table_name], [POL_table_meeting_name], [POL_table_ws_name], [POL_table_session_name])
    VALUES (@id_event, @start_date, @end_date, @name, @description, @external_id, @table_prefix + @external_id, @table_prefix + 'MEETING_' + @external_id, @table_prefix + 'WS_' + @external_id, @table_prefix + 'SESSION_' + @external_id)

	select @id_pool = @@IDENTITY
	
	select @id_pool as id_poll

	return @id_pool
END


GO
/****** Object:  StoredProcedure [dbo].[SELP101]    Script Date: 1/20/2017 9:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Maïssa BAKER - SEAL - Event Catalyst
-- Create date: 2016-11-06
-- Description:	Create a block
-- =============================================
CREATE PROCEDURE [dbo].[SELP101] 
	@label nvarchar(255)
AS
BEGIN
	declare @id_block int = -1

	INSERT INTO [dbo].[POLL_BLOCK] ([POLB_label])
    VALUES (@label)

	select @id_block = @@IDENTITY

	select @id_block as id_block

	return @id_block
END


GO
/****** Object:  StoredProcedure [dbo].[SELP102]    Script Date: 1/20/2017 9:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Maïssa BAKER - SEAL - Event Catalyst
-- Create date: 2016-11-06
-- Description:	Create a question
-- =============================================
CREATE PROCEDURE [dbo].[SELP102] 
(
	@id_poll int,
    @label nvarchar(255),
    @description nvarchar(1000),
    @order int,
    @max_size int,
    @nb_choice int,
    @number nvarchar(50),
    @id_block int,
    @id_choice_type int = 1,
    @mandatory bit = 0,
    @page_number int = 1,
    @id_question_type int = 1,
    @id_parent int = -1
)
AS
BEGIN
	declare @id_question int = -1
	declare @column nvarchar(50)
	select @column = RTRIM(LTRIM(CHOT_prefix)) + REPLACE(@number, '.', '_') from POLL_CHOICE_TYPE where CHOT_id = @id_choice_type

	INSERT INTO [dbo].[POLL_QUESTION]
([QUE_id_poll],[QUE_label],[QUE_description],[QUE_order],[QUE_max_size],[QUE_nb_choice],[QUE_mandatory],[QUE_number],[QUE_page_number],[QUE_id_block],[QUE_id_choice_type],[QUE_id_question_type],[QUE_id_parent],[QUE_column])
VALUES(@id_poll,@label,@description,@order,@max_size,@nb_choice,@mandatory,@number,@page_number,@id_block,@id_choice_type,@id_question_type,@id_parent,@column)

	select @id_question = @@IDENTITY
	select @id_question as id_question
	return @id_question
END


GO
/****** Object:  StoredProcedure [dbo].[SELP103]    Script Date: 1/20/2017 9:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Maïssa BAKER - SEAL - Event Catalyst
-- Create date: 2016-11-06
-- Description:	Create a possible choice for a question
-- =============================================
CREATE PROCEDURE [dbo].[SELP103]
(
	@id_question int,
    @label nvarchar(255),
    @value nvarchar(255),
    @order int
)
AS
BEGIN
	declare @id_choice int = -1

	INSERT INTO[dbo].[POLL_CHOICE] ([CHO_id_question],[CHO_choice_label],[CHO_order],[CHO_choice_value])
	VALUES (@id_question, @label, @order, @value)

	select @id_choice = @@IDENTITY

	return @id_choice
END


GO
/****** Object:  StoredProcedure [dbo].[SELP104]    Script Date: 1/20/2017 9:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Maïssa BAKER - SEAL - Event Catalyst
-- Create date: 2016-11-06
-- Description:	Create a question
-- =============================================
CREATE PROCEDURE [dbo].[SELP104] 
(
	@id_question int,
    @label nvarchar(255),
    @description nvarchar(1000),
    @order int,
    @max_size int,
    @nb_choice int,
    @number nvarchar(50),
    @id_choice_type int = 1,
    @mandatory bit = 0
)
AS
BEGIN
	declare @id_sub_question int = -1

	declare @column nvarchar(50)
	select @column = RTRIM(LTRIM(CHOT_prefix)) + REPLACE(@number, '.', '_') from POLL_CHOICE_TYPE where CHOT_id = @id_choice_type

	INSERT INTO [dbo].[POLL_MEETING_QUESTION]
([MQUE_id_question],[MQUE_label],[MQUE_description],[MQUE_order],[MQUE_max_size],[MQUE_nb_choice],[MQUE_mandatory],[MQUE_number],[MQUE_id_choice_type],[MQUE_column])
VALUES(@id_question,@label,@description,@order,@max_size,@nb_choice,@mandatory,@number,@id_choice_type,@column)

	select @id_sub_question = @@IDENTITY
	select @id_sub_question as id_question
	return @id_sub_question
END


GO
/****** Object:  StoredProcedure [dbo].[SELP105]    Script Date: 1/20/2017 9:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Maïssa BAKER - SEAL - Event Catalyst
-- Create date: 2016-11-06
-- Description:	Create a possible choice for a question
-- =============================================
CREATE PROCEDURE [dbo].[SELP105]
(
	@id_question int,
    @label nvarchar(255),
    @value nvarchar(255),
    @order int
)
AS
BEGIN
	declare @id_choice int = -1

	INSERT INTO[dbo].[POLL_MEETING_CHOICE] ([MCHO_id_meeting_question],[MCHO_choice_label],[MCHO_order],[MCHO_choice_value])
	VALUES (@id_question, @label, @order, @value)

	select @id_choice = @@IDENTITY

	return @id_choice
END

GO
/****** Object:  StoredProcedure [dbo].[SELP106]    Script Date: 1/20/2017 9:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Maïssa BAKER - SEAL - Event Catalyst
-- Create date: 2016-11-06
-- Description:	Create a question
-- =============================================
CREATE PROCEDURE [dbo].[SELP106] 
(
	@id_question int,
    @label nvarchar(255),
    @description nvarchar(1000),
    @order int,
    @max_size int,
    @nb_choice int,
    @number nvarchar(50),
    @id_choice_type int = 1,
    @mandatory bit = 0
)
AS
BEGIN
	declare @id_sub_question int = -1
	declare @column nvarchar(50)
	select @column = RTRIM(LTRIM(CHOT_prefix)) + REPLACE(@number, '.', '_') from POLL_CHOICE_TYPE where CHOT_id = @id_choice_type

	INSERT INTO [dbo].[POLL_SESSION_QUESTION]
([SQUE_id_question],[SQUE_label],[SQUE_description],[SQUE_order],[SQUE_max_size],[SQUE_nb_choice],[SQUE_mandatory],[SQUE_number],[SQUE_id_choice_type],[SQUE_column])
VALUES(@id_question,@label,@description,@order,@max_size,@nb_choice,@mandatory,@number,@id_choice_type,@column)
	
	select @id_sub_question = @@IDENTITY
	select @id_sub_question as id_question
	return @id_sub_question
END


GO
/****** Object:  StoredProcedure [dbo].[SELP107]    Script Date: 1/20/2017 9:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Maïssa BAKER - SEAL - Event Catalyst
-- Create date: 2016-11-06
-- Description:	Create a possible choice for a question
-- =============================================
CREATE PROCEDURE [dbo].[SELP107]
(
	@id_question int,
    @label nvarchar(255),
    @value nvarchar(255),
    @order int
)
AS
BEGIN
	declare @id_choice int = -1

	INSERT INTO[dbo].[POLL_SESSION_CHOICE] ([SCHO_id_session_question],[SCHO_choice_label],[SCHO_order],[SCHO_choice_value])
	VALUES (@id_question, @label, @order, @value)

	select @id_choice = @@IDENTITY

	return @id_choice
END

GO
/****** Object:  StoredProcedure [dbo].[SELP108]    Script Date: 1/20/2017 9:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Maïssa BAKER - SEAL - Event Catalyst
-- Create date: 2016-11-06
-- Description:	Create a question
-- =============================================
CREATE PROCEDURE [dbo].[SELP108] 
(
	@id_question int,
    @label nvarchar(255),
    @description nvarchar(1000),
    @order int,
    @max_size int,
    @nb_choice int,
    @number nvarchar(50),
    @id_choice_type int = 1,
    @mandatory bit = 0
)
AS
BEGIN
	declare @id_sub_question int = -1
	declare @column nvarchar(50)
	select @column = RTRIM(LTRIM(CHOT_prefix)) + REPLACE(@number, '.', '_') from POLL_CHOICE_TYPE where CHOT_id = @id_choice_type

	INSERT INTO [dbo].[POLL_WS_QUESTION]
([WSQUE_id_question],[WSQUE_label],[WSQUE_description],[WSQUE_order],[WSQUE_max_size],[WSQUE_nb_choice],[WSQUE_mandatory],[WSQUE_number],[WSQUE_id_choice_type],[WSQUE_column])
VALUES(@id_question,@label,@description,@order,@max_size,@nb_choice,@mandatory,@number,@id_choice_type,@column)
	
	select @id_sub_question = @@IDENTITY
	select @id_sub_question as id_question
	return @id_sub_question
END

GO
/****** Object:  StoredProcedure [dbo].[SELP109]    Script Date: 1/20/2017 9:58:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Maïssa BAKER - SEAL - Event Catalyst
-- Create date: 2016-11-06
-- Description:	Create a possible choice for a question
-- =============================================
CREATE PROCEDURE [dbo].[SELP109]
(
	@id_question int,
    @label nvarchar(255),
    @value nvarchar(255),
    @order int
)
AS
BEGIN
	declare @id_choice int = -1

	INSERT INTO[dbo].[POLL_WS_CHOICE] ([WSCHO_id_ws_question],[WSCHO_choice_label],[WSCHO_order],[WSCHO_choice_value])
	VALUES (@id_question, @label, @order, @value)

	select @id_choice = @@IDENTITY

	return @id_choice
END

GO
