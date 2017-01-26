use meta_survey;
declare @event_id int = 3;
declare @id_poll int;
declare @event_plalning_block int;
declare @event_satisfaction_block int;
declare @meeting_block int;
declare @session_block int;
declare @ws_block int;

declare @q1 int;
declare @q2 int;
declare @q3 int;
declare @q4 int;
declare @mgq int;
declare @sgq int;
declare @wsgq int;

declare @mq1 int;
declare @mq2 int;

declare @sq1 int;
declare @sq2 int;

declare @wsq1 int;
declare @wsq2 int;
-- insert poll
EXEC @id_poll = [SELP100] '2016-11-06', '2017-11-06', 'Test Survey', 'Survey for tests', @event_id;
-- insert blocks
EXEC @event_plalning_block = [SELP101] 'Event planing';
EXEC @event_plalning_block = [SELP101] 'Event statisfaction';
-- insert general question questions 
EXEC @q1 = [SELP102] @id_poll, 'How convenient or inconvenient is the date and time of the event for you?', '', 1, NULL, 1, '1.a', @event_plalning_block, 4, 1;
EXEC @q2 = [SELP102] @id_poll, 'How convenient or inconvenient is the location of the event for you?', '', 2, NULL, 1, '1.b', @event_plalning_block, 3, 1;
EXEC @q3 = [SELP102] @id_poll, 'Overall, how would you rate the test event?', '', 3, NULL, 2, '2.a', @event_satisfaction_block, 5, 1;
EXEC @q4 = [SELP102] @id_poll, 'Do you have any other comments, questions, or concerns?', '', 4, NULL, NULL, '2.b', @event_satisfaction_block, 5, 0;
-- insert choice
EXEC [SELP103] @q1, 'Very convenient', 'Very convenient', 1;
EXEC [SELP103] @q1, 'Convenient', 'Convenient', 2;
EXEC [SELP103] @q1, 'Neither convenient nor inconvenient', 'Neither convenient nor inconvenient', 3;
EXEC [SELP103] @q1, 'Inconvenient', 'Inconvenient', 4;
EXEC [SELP103] @q1, 'Very inconvenient', 'Very inconvenient', 5;

EXEC [SELP103] @q2, 'Very convenient', 'Very convenient', 1;
EXEC [SELP103] @q2, 'Convenient', 'Convenient', 2;
EXEC [SELP103] @q2, 'Neither convenient nor inconvenient', 'Neither convenient nor inconvenient', 3;
EXEC [SELP103] @q2, 'Inconvenient', 'Inconvenient', 4;
EXEC [SELP103] @q2, 'Very inconvenient', 'Very inconvenient', 5;

EXEC [SELP103] @q3, 'Excellent', 'Excellent', 1;
EXEC [SELP103] @q3, 'Very good', 'Very good', 2;
EXEC [SELP103] @q3, 'Good', 'Good', 3;
EXEC [SELP103] @q3, 'Fair', 'Fair', 4;
EXEC [SELP103] @q3, 'Poor', 'Poor', 5;

EXEC @meeting_block = [dbo].[SELP101] @label = N'Meeting Block'
EXEC @session_block = [dbo].[SELP101] @label = N'Session Block'
EXEC @ws_block = [dbo].[SELP101] @label = N'Workshop Block'

EXEC @mgq = [dbo].[SELP102] @id_poll = @id_poll, @label = N'Please rate the meeting you attended', @description = N'', @order = 4, @max_size = NULL, @nb_choice = NULL, @number = N'3', @id_block = @meeting_block, @id_choice_type = NULL, @mandatory = 0, @page_number = 1, @id_question_type = 3, @id_parent = -1;
EXEC @sgq= [dbo].[SELP102] @id_poll = @id_poll, @label = N'Please rate the sessions you attended', @description = N'', @order = 5, @max_size = NULL, @nb_choice = NULL, @number = N'4', @id_block = @session_block, @id_choice_type = NULL, @mandatory = 0, @page_number = 1, @id_question_type = 2, @id_parent = -1;
EXEC @wsgq = [dbo].[SELP102] @id_poll = @id_poll, @label = N'Please rate the workshop you attended', @description = N'', @order = 6, @max_size = NULL, @nb_choice = NULL, @number = N'5', @id_block = @ws_block, @id_choice_type = NULL, @mandatory = 0, @page_number = 1, @id_question_type = 4, @id_parent = -1;

declare @id_one_question_1 int
declare @id_one_question_2 int
EXEC @id_one_question_1 = [dbo].[SELP104] @id_question = @mgq, @label = N'How was the meeting', @description = NULL, @order = 1, @max_size = NULL, @nb_choice = 1, @number = N'3.a', @id_choice_type = 4, @mandatory = 1
EXEC @id_one_question_2 = [dbo].[SELP104] @id_question = @mgq, @label = N'Any comments?', @description = NULL, @order = 2, @max_size = 2000, @nb_choice = NULL, @number = N'3.b', @id_choice_type = 2, @mandatory = 0

-- ajouter des réponses aux questions
EXEC [dbo].[SELP105] @id_question = @id_one_question_1, @label = N'Very Satisfied', @value = N'Very Satisfied', @order = 1
EXEC [dbo].[SELP105] @id_question = @id_one_question_1, @label = N'Somewhat Satisfied', @value = N'Somewhat Satisfied', @order = 2
EXEC [dbo].[SELP105] @id_question = @id_one_question_1, @label = N'Somewhat Dissatisfied', @value = N'Somewhat Dissatisfied', @order = 3
EXEC [dbo].[SELP105] @id_question = @id_one_question_1, @label = N'Very Dissatisfied', @value = N'Very Dissatisfied', @order = 4
EXEC [dbo].[SELP105] @id_question = @id_one_question_1, @label = N'Don’t Know', @value = N'Don’t Know', @order = 5
EXEC [dbo].[SELP105] @id_question = @id_one_question_1, @label = N'Not Applicable', @value = N'Not Applicable', @order = 6

-- ajouter des nouvelles pour les session
declare @id_as_question_1 int
declare @id_as_question_2 int
EXEC @id_as_question_1 = [dbo].[SELP106] @id_question = @sgq, @label = N'How was the session', @description = NULL, @order = 1, @max_size = NULL, @nb_choice = 1, @number = N'4.a', @id_choice_type = 4, @mandatory = 1
EXEC @id_as_question_2 = [dbo].[SELP106] @id_question = @sgq, @label = N'Any comments?', @description = NULL, @order = 2, @max_size = 2000, @nb_choice = NULL, @number = N'4.b', @id_choice_type = 2, @mandatory = 0

-- ajouter des réponses aux questions
EXEC [dbo].[SELP107] @id_question = @id_as_question_1, @label = N'Very Satisfied', @value = N'Very Satisfied', @order = 1
EXEC [dbo].[SELP107] @id_question = @id_as_question_1, @label = N'Somewhat Satisfied', @value = N'Somewhat Satisfied', @order = 2
EXEC [dbo].[SELP107] @id_question = @id_as_question_1, @label = N'Somewhat Dissatisfied', @value = N'Somewhat Dissatisfied', @order = 3
EXEC [dbo].[SELP107] @id_question = @id_as_question_1, @label = N'Very Dissatisfied', @value = N'Very Dissatisfied', @order = 4
EXEC [dbo].[SELP107] @id_question = @id_as_question_1, @label = N'Don’t Know', @value = N'Don’t Know', @order = 5
EXEC [dbo].[SELP107] @id_question = @id_as_question_1, @label = N'Not Applicable', @value = N'Not Applicable', @order = 6

-- ajouter des nouvelles pour les workshop
declare @id_ws_question_1 int
declare @id_ws_question_2 int
EXEC @id_ws_question_1 = [dbo].[SELP108] @id_question = @wsgq, @label = N'How was the workshop', @description = NULL, @order = 1, @max_size = NULL, @nb_choice = 1, @number = N'5.a', @id_choice_type = 4, @mandatory = 1
EXEC @id_ws_question_2 = [dbo].[SELP108] @id_question = @wsgq, @label = N'Any comments?', @description = NULL, @order = 2, @max_size = 2000, @nb_choice = NULL, @number = N'5.b', @id_choice_type = 2, @mandatory = 0

-- ajouter des réponses aux questions
EXEC [dbo].[SELP109] @id_question = @id_ws_question_1, @label = N'Very Satisfied', @value = N'Very Satisfied', @order = 1;
EXEC [dbo].[SELP109] @id_question = @id_ws_question_1, @label = N'Somewhat Satisfied', @value = N'Somewhat Satisfied', @order = 2;
EXEC [dbo].[SELP109] @id_question = @id_ws_question_1, @label = N'Somewhat Dissatisfied', @value = N'Somewhat Dissatisfied', @order = 3;
EXEC [dbo].[SELP109] @id_question = @id_ws_question_1, @label = N'Very Dissatisfied', @value = N'Very Dissatisfied', @order = 4;
EXEC [dbo].[SELP109] @id_question = @id_ws_question_1, @label = N'Don’t Know', @value = N'Don’t Know', @order = 5;
EXEC [dbo].[SELP109] @id_question = @id_ws_question_1, @label = N'Not Applicable', @value = N'Not Applicable', @order = 6;
