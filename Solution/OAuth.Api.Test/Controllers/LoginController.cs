using Microsoft.VisualStudio.TestTools.UnitTesting;
using OAuth.Api.Controllers;

namespace OAuth.Api.Test.Controllers
{
    [TestClass]
    public class LoginControllerTest
    {
        [TestMethod]
        public void TestCript()
        {
            const string password = "a0D_*792@";
            string hash = LoginController.HashPassword(password);
            Assert.IsTrue(LoginController.ValidPassword(password, hash), "Criptografy invalid");
        }

        [TestMethod]
        public void TestGenerateToken()
        {
            string token = Api.Controllers.LoginController.GenerateToken(LoginController.LargerTokenSize);
            Assert.IsNotNull(token);
            Assert.IsTrue(token.Length == Api.Controllers.LoginController.LargerTokenSize);
        }

    }
}
