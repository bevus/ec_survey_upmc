using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SurveyDashboardGenerator;
using SurveyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyDashboardGenerator.Tests
{
    [TestClass()]
    public class DashboardUtilsTests
    {
        DashboardUtils utils = new DashboardUtils();
        public static int id_poll = 1;
        public static Manager manager = new Manager();
        public static Poll poll = manager.getPoll(id_poll);
        public static string tablesurvey = poll.TableName;
        public static string tablemeeting = poll.TableMeetingName;
        public static string tablesession = poll.TableSessionName;
        public static string tablews = poll.TableWsName;


        [TestMethod()]
        public void GetGeneralResponseTest()
        {
            Question q = new Question();
            utils.GetGeneralResponse(q, tablesurvey);
        }

        [TestMethod()]
        public void GetQesDataResponseTest()
        {
            Question q = new Question();
            utils.GetQesDataResponse(q,tablesurvey);
        }

        [TestMethod()]
        public void NumberResponseGeneralTest()
        {
            string nomC = "";
            string lable = "";
            utils.NumberResponseGeneral(nomC, lable, tablesurvey);
        }

        [TestMethod()]
        public void NumberResponseMultipleChoiceTest()
        {
            string nomC = "";
            string lable = "";
            utils.NumberResponseMultipleChoice(nomC,lable,tablesurvey);
        }

        [TestMethod()]
        public void GetWSsubQuestionTest()
        {
            Question q = new Question();
            utils.GetWSsubQuestion(id_poll, q, tablesurvey);
        }

        [TestMethod()]
        public void GetWSsubQResponseTest()
        {
           
        }

        [TestMethod()]
        public void NumberResponseWSTest()
        {
            
        }

        [TestMethod()]
        public void Get_list_ws_atelierTest()
        {
            
        }

        [TestMethod()]
        public void GetSessionsubQuestionTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetSessionsubQResponseTest()
        {
            
        }

        [TestMethod()]
        public void NumberResponseSessionTest()
        {
            
        }

        [TestMethod()]
        public void Get_list_session_atelierTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetMeetingsubQuestionTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetQMeetingesDataResponseTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void NumberResponseMeetingTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void NumberResponseMultipleChoiceMeetingTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetMeetingDataResponseTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void getPieContenerTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void getCheckBoxContenerTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void getWSQContenerTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetGeneralQuestionContenerTest()
        {
            Assert.Fail();
        }
    }
}