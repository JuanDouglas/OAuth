using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OAuth.Api.Test
{
    [TestClass]
    public class LoginController
    {
        [TestMethod]
        public void TestCript()
        {
            const string password = "a0D_*792@";
            string hash = Controllers.LoginController.HashPassword(password);
            Assert.IsTrue(Controllers.LoginController.ValidPassword(password,hash),"Criptografy invalid");
        }

        [TestMethod]
        public void TestGenerateToken() {
            string token = Controllers.LoginController.GenerateToken(Controllers.LoginController.LargerTokenSize);
            Assert.IsNotNull(token);
            Assert.IsTrue(token.Length == Controllers.LoginController.LargerTokenSize);
        }
    }
}
