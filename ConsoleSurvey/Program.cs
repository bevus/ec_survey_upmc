using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using DataAccess;
using SurveyModel;
using Widgets;

namespace ConsoleSurvey
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var manager = new Manager();
        }

        private static void TestGetHashedId(Poll poll)
        {
            Console.WriteLine(Manager.GetHashedId("cad61ca1c4fbad3be96e27e0b1ecd5de3b4d5c25", poll.Id));
            Console.WriteLine(Manager.GetHashedId("8d25b4c973c5dafa021036664b080a79e0bb69a0", poll.Id));
            Console.WriteLine(Manager.GetHashedId("8d25b4c973c5dafa021036664b080a79e0bb69b0", poll.Id));
        }

        public void TestGetRep(Poll poll, Manager manager)
        {
            foreach (var q in poll.Questions)
            {
                if (q.Category == QuestionType.General)
                {
                    Console.WriteLine(manager.getAnswer(poll.Id, poll.TableName, q.Category,
                        QuestionTypeActionFactory.getActionByName(q.Category).getAnswerComlunName(q), 1076));
                }
                else if (q.Category == QuestionType.Meeting)
                {
                    Console.WriteLine();
                    foreach (var m in poll.Meetings)
                    {
                        Console.WriteLine("Messtings");
                        foreach (var sq in q.SubQuestions)
                        {
                            Console.WriteLine(manager.getAnswer(poll.Id, poll.TableMeetingName, sq.Category,
                                QuestionTypeActionFactory.getActionByName(q.Category).getAnswerComlunName(sq), 1076, m.id_meeting));
                        }
                    }
                }
                else if (q.Category == QuestionType.Session)
                {
                    foreach (var s in poll.Sessions)
                    {
                        Console.WriteLine("Sessions");
                        foreach (var sq in q.SubQuestions)
                        {
                            Console.WriteLine(manager.getAnswer(poll.Id, poll.TableSessionName, sq.Category,
                                QuestionTypeActionFactory.getActionByName(q.Category).getAnswerComlunName(sq), 1076, s.id_atelier));
                        }
                    }
                }
                else if (q.Category == QuestionType.Workshop)
                {
                    foreach (var w in poll.Workshops)
                    {
                        Console.WriteLine("Workshops");
                        foreach (var sq in q.SubQuestions)
                        {
                            Console.WriteLine(manager.getAnswer(poll.Id, poll.TableWsName, sq.Category,
                                QuestionTypeActionFactory.getActionByName(q.Category).getAnswerComlunName(sq), 1076, w.id_atelier));
                        }
                    }
                }
            }

        }
        static string Hash(string input)
        {
            using (var sha = new SHA1CryptoServiceProvider())
            {
                var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}
