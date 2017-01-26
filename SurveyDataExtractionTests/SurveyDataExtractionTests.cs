using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SurveyDashboardGenerator;
using SurveyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SurveyDataExtraction;

namespace SurveyDashboardGenerator.Tests
{
    [TestClass()]
    public class SurveyDataExtractionTests
    {
        public static int id_poll = 1;
        public static Manager manager = new Manager();
        public static SurveyDataExtractor dataextraction = new SurveyDataExtractor();
        public static Poll poll = manager.getPoll(id_poll);
        public static string tablesurvey = poll.TableName;
        public static string tablemeeting = poll.TableMeetingName;
        public static string tablesession = poll.TableSessionName;
        public static string tablews = poll.TableWsName;

        [TestMethod()]
        public void getNbParticipantsTest()
        {
            int resultatattendu = 2;
            int resultobtenu = dataextraction.getNbParticipants(poll.Id, tablesurvey);
            Assert.AreEqual(resultatattendu, resultobtenu);
        }

        [TestMethod()]
        public void NumberResponse_GeneralQuestionTest()
        {
            Assert.AreEqual(2, DataExtractionUtils.NumberResponse_GeneralQuestion(tablesurvey, "rbl1_a", "Flat"));
        }
        [TestMethod()]
        public void NumberResponseTest()
        {
            Assert.AreEqual(1, DataExtractionUtils.NumberResponse(tablesession, "SUB_rbl4_a", "SomeWhat Satisfied", "Activity", 14, 84));
        }

        [TestMethod()]
        public void getAttendedMeetingsTest()
        {

            List<Meeting> meetings1 = new List<Meeting>();
            Meeting m = new Meeting();
            m.id_meeting = 1;
            m.id_company = 12;
            m.company_name = "com";
            meetings1.Add(m);

            List <Meeting> meetings2 = DataExtractionUtils.getAttendedMeetings(id_poll, tablemeeting);

            for (int i = 0; i < meetings2.Count;i++ )
            {
                Meeting m2 = meetings2[i];
                Assert.AreEqual(m.id_meeting,m2.id_meeting);
                Assert.AreEqual(m.id_company, m2.id_meeting);
                Assert.AreEqual(m.company_name, m2.company_name);
            }
            
        }

       
      
        [TestMethod()]
        public void getWsAtelierTest()
        {
            List<Atelier> sessionAtelier1 = new List<Atelier>();
            Atelier workshop = new Atelier();
            workshop.id_atelier =1;
            workshop.id_event = 1;
            workshop.theme = "session 1";
            workshop.description = "";
            
            List<Atelier> wsAtelier2 =  DataExtractionUtils.getWsAtelier(id_poll,tablesession);

            for (int i = 0; i < wsAtelier2.Count; i++)
            {
                Atelier workshop2 = wsAtelier2[i];
                Assert.AreEqual(workshop.id_event, workshop2.id_event);
                Assert.AreEqual(workshop.id_atelier, workshop2.id_event);
                Assert.AreEqual(workshop.theme, workshop2.id_event);
                Assert.AreEqual(workshop.description, workshop2.id_event);
            }
        }

        [TestMethod()]
        public void getSessionAtelierTest()
        {
            List<Atelier> sessionAtelier1 = new List<Atelier>();
            Atelier session = new Atelier();
            session.id_atelier = 1;
            session.id_event = 1;
            session.theme = "session 1";
            session.description = "";

            List<Atelier> sessionAtelier2 = DataExtractionUtils.getSessionAtelier(id_poll, tablesession);

            for (int i = 0; i < sessionAtelier2.Count; i++)
            {
                Atelier session2 = sessionAtelier2[i];
                Assert.AreEqual(session.id_event, session2.id_event);
                Assert.AreEqual(session.id_atelier, session2.id_event);
                Assert.AreEqual(session.theme, session2.id_event);
                Assert.AreEqual(session.description, session2.id_event);
            }
        }

        //[TestMethod()]
        //public void getMeetingAnswersTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void getSessionWsAnswersTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void getGeneralAnswersTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        public void getMeetingParticipantsTest()
        {
            int id_meeting = 1;
            int id_company = 10;
            string participantscompanies1 = "COM1,COM2";
            string participantscompanies2 =  dataextraction.getMeetingParticipants(id_meeting, id_company);
            Assert.AreEqual(participantscompanies1,participantscompanies2);
        }

      
    }
}