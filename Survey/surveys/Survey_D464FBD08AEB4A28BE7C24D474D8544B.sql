CREATE PROCEDURE [dbo].[selD464FBD08AEB4A28BE7C24D474D8544B]
(
	@person_id int
)
AS
	SELECT * 
 	FROM Poll_SURVEY_D464FBD08AEB4A28BE7C24D474D8544B
 	WHERE id_person=@person_id 
GO

CREATE PROCEDURE [dbo].[ID464FBD08AEB4A28BE7C24D474D8544B]
(
	@person_id int,
	@rbl1_a  nvarchar(255),
	@rbl2_a  nvarchar(255),
	@rbl1_b  nvarchar(255),
	@rbl2_b  nvarchar(255),
	@area1_c  nvarchar(2000)
)
AS
if not exists(SELECT id_person FROM Poll_SURVEY_D464FBD08AEB4A28BE7C24D474D8544B	WHERE id_person = @person_id )
BEGIN
	INSERT INTO Poll_SURVEY_D464FBD08AEB4A28BE7C24D474D8544B (id_person) VALUES(@person_id)
end
	UPDATE  [dbo].[Poll_SURVEY_D464FBD08AEB4A28BE7C24D474D8544B]
SET
	[rbl1_a] = @rbl1_a,
	[rbl2_a] = @rbl2_a,
	[rbl1_b] = @rbl1_b,
	[rbl2_b] = @rbl2_b,
	[area1_c] = @area1_c
WHERE id_person = @person_id 
GO

CREATE PROCEDURE [dbo].[ISD464FBD08AEB4A28BE7C24D474D8544B]
(
	@person_id int,
	@clicked BIT,
	@clicked_date DATETIME,
	@saved BIT,
	@saved_date DATETIME,
	@comment VARCHAR(4000),
	@modified_date DATETIME,
	@page VARCHAR(4000)
)
AS
    if not exists(SELECT SUR_id_person FROM Poll_SURVEY WHERE SUR_id_person = @person_id )
BEGIN
	INSERT INTO Poll_SURVEY(SUR_id_person) VALUES(@person_id)
end
	UPDATE  [dbo].[Poll_SURVEY]
    SET
        [SUR_clicked] =  @clicked,
        [SUR_clicked_date] =  @clicked_date, 
        [SUR_saved] = @saved, 
        [SUR_saved_date] = @saved_date,
        [SUR_comments] = @comment,
        [SUR_date_mod] = @modified_date,
        [SUR_page] = @page
    WHERE SUR_id_person = @person_id 
GO
