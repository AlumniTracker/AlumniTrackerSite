//using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static AlumniTrackerSite.Data.Security;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        //public ILogger _log { get; set; }
        //public void Init()
        //{
        //    _log = new ILogger<AlumniTrackerSite.AlumniUsersController>();
        //}
        //[TestMethod]
        //public void TestSecurity_MaliciousInput()
        //{
        //    string[] BadInputs = { "1:1", "DROP TABLE", "DROP DATABASE", "<i>tofu</i>" };
        //    foreach (string BadInput in BadInputs)
        //    {
        //        Assert.IsFalse(GeneralInput(BadInput));
        //    }
        //}
        //[TestMethod]
        //public void TestSecurity_FalseEmailInput()
        //{
        //    string[] BadEmails = { "Aiden", "1", ",", ".';[]", "mgrant@hotmail.", "@hotmail.com", "mgrant@.com", "mgrant@hotmail.c" };
        //    foreach (string BadEmail in BadEmails)
        //    {
        //        Assert.IsFalse(EmailInput(BadEmail));
        //    }
        //}
        [TestMethod]
        public void TestSecurity_MaliciousInput()
        {
            string[] BadInputs = { "1:1", "DROP TABLE", "DROP DATABASE", "<i>tofu</i>" };
            foreach (string BadInput in BadInputs)
            {
                //Assert.IsFalse(GeneralInput(BadInput));
            }
        }
        [TestMethod]
        public void TestSecurity_FalseEmailInput()
        {
            string[] BadEmails = { "Aiden", "1", ",", ".';[]", "mgrant@hotmail.", "@hotmail.com", "mgrant@.com", "mgrant@hotmail.c" };
            foreach (string BadEmail in BadEmails)
            {
                //Assert.IsFalse(EmailInput(BadEmail));
            }
        }
        [TestMethod]
        public void TestSecurity_TrueEmailInput()
        {
            string[] GoodEmails = { "mgrant@hotmail.com", "consuelo26@yahoo.com",
                "hegmann.julio@yahoo.com", "maeve48@yahoo.com", "bhermiston@yahoo.com",
                "tlowe@yahoo.com", "vokuneva@gmail.com", "considine.elinore@kunze.com",
                "fbrown@mcclure.com", "thand@hotmail.com", "deckow.chloe@hotmail.com",
                "icarroll@gmail.com", "xoreilly@hotmail.com", "wwhite@hegmann.com",
                "kilback.maxwell@boehm.com", "efadel@gmail.com", "rwest@gmail.com",
                "harley.romaguera@gmail.com", "mariana49@yahoo.com", "mikel89@yahoo.com",
                "wstamm@oberbrunner.com", "hermann.samara@yahoo.com", "karine01@yahoo.com",
                "antwon.ratke@wiza.com", "jolson@yahoo.com", "kobe.smith@hotmail.com",
                "tyrell17@lakin.com", "morris02@hotmail.com", "jjast@kohler.com",
                "zswift@medhurst.com", "rjacobson@hotmail.com", "elnora.thompson@hotmail.com",
                "hill.gardner@lehner.com", "odie37@hotmail.com", "nelle92@gmail.com",
                "quigley.angel@gmail.com", "connelly.vivien@rippin.com", "delphia03@nader.com",
                "yolson@yahoo.com", "ykautzer@skiles.biz", "yasmine.macejkovic@jones.com",
                "lynch.dedrick@ondricka.com", "destiny.wolff@yahoo.com", "marielle.schuster@gmail.com",
                "aiyana.koss@heidenreich.com", "cicero.sawayn@gmail.com", "tremayne.hartmann@mueller.org",
                "marlon.conroy@okuneva.com", "bauch.louie@gmail.com", "saige.kreiger@yahoo.com",
                 };
            foreach (string GoodEmail in GoodEmails)
            {
                //Assert.IsTrue(EmailInput(GoodEmail));
            }
        }
        [TestMethod]
        public void Test()
        {

        }
    }
}