using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using BusinessLogic;
using NUnit.Framework;
using Web.Controllers;

namespace Unit_Tests
{
    [TestFixture]
    public class AccountControllerTests
    {
        [Test(Description = "GetMembershipCreateStatusResultText method returns duplicate email error text")]
        public void GetMembershipCreateStatusResultTextMethodReturnsDuplicateEmailErrorText()
        {
            var controller = new AccountController(new DataManager(null,null,null,null,null));
            string result = controller.GetMembershipCreateStatusResultText(MembershipCreateStatus.DuplicateEmail);
            Assert.AreEqual("Пользователь с таким email адресом уже существует", result);
        }

        [Test(Description = "GetMembershipCreateStatusResultText method returns duplicate username error text")]
        public void GetMembershipCreateStatusResultTextMethodReturnsDuplicateUserNameErrorText()
        {
            var controller = new AccountController(new DataManager(null, null, null, null, null));
            string result = controller.GetMembershipCreateStatusResultText(MembershipCreateStatus.DuplicateUserName);
            Assert.AreEqual("Пользователь с таким именем уже существует", result);
        }

        [Test(Description = "GetMembershipCreateStatusResultText method returns duplicate email error text")]
        public void GetMembershipCreateStatusResultTextMethodReturnsOtherErrorText()
        {
            var controller = new AccountController(new DataManager(null, null, null, null, null));
            string result = controller.GetMembershipCreateStatusResultText(MembershipCreateStatus.InvalidPassword);
            Assert.AreEqual("Неизвестная ошибка", result);
        }
    }
}
