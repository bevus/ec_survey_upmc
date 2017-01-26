using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SurveyModel;

namespace DataAccess.Tests
{
    [TestClass()]
    public class ManagerTests
    {
        [TestMethod()]
        public void GetPollQuestionCountTest()
        {
            var manager = new Manager();
            var p = manager.getPoll(8);
            Assert.AreEqual(7, p.Questions.Count);
        }
        [TestMethod()]
        public void GetPollGeneralQuestionCount()
        {
            var manager = new Manager();
            var p = manager.getPoll(8);
            var gqc = 0;
            foreach (var q in p.Questions)
            {
                if (q.Category.Equals(QuestionType.General))
                {
                    gqc++;
                }
            }
            Assert.AreEqual(4, gqc);
        }
        [TestMethod()]
        public void GetPollMeetingQuestionCount()
        {
            var manager = new Manager();
            var p = manager.getPoll(8);
            var mqc = 0;
            foreach (var q in p.Questions)
            {
                if (q.Category.Equals(QuestionType.Meeting))
                {
                    mqc++;
                }
            }
            Assert.AreEqual(1, mqc);
        }
        [TestMethod()]
        public void GetPollSessionQuestionCount()
        {
            var manager = new Manager();
            var p = manager.getPoll(8);
            var sqc = 0;
            foreach (var q in p.Questions)
            {
                if (q.Category.Equals(QuestionType.Session))
                {
                    sqc++;
                }
            }
            Assert.AreEqual(1, sqc);
        }
        [TestMethod()]
        public void GetPollWsQuestionCount()
        {
            var manager = new Manager();
            var p = manager.getPoll(8);
            var wsqc = 0;
            foreach (var q in p.Questions)
            {
                if (q.Category.Equals(QuestionType.Workshop))
                {
                    wsqc++;
                }
            }
            Assert.AreEqual(1, wsqc);
        }

        [TestMethod()]
        public void GetPollBlockCount()
        {
            var manager = new Manager();
            var p = manager.getPoll(8);
            Assert.AreEqual(5, p.Blocks.Count);
        }

        [TestMethod()]
        public void GetPollDataTest()
        {
            
        }
    }
}