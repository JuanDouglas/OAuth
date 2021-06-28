using OAuth.Dal.Models;
using System;

namespace OAuth.Api.Models.Result
{
    public class FirstStep
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string Token { get; set; }
        public bool Valid { get; set; }
        public string IPAdress { get; set; }

        public FirstStep()
        {
        }
        public FirstStep(LoginFirstStep loginFirstStep)
        {
            Date = loginFirstStep.Date;
            Token = loginFirstStep.Token;
            Valid = loginFirstStep.Valid;
            IPAdress = loginFirstStep.Ipadress;
            ID = loginFirstStep.Id;
        }
    }
}
