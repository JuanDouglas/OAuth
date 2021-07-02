using OAuth.Dal.Models;
using System;

namespace OAuth.Api.Models.Result
{
    public class FirstStep
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string Key { get; set; }
        public bool Valid { get; set; }

        public FirstStep()
        {
        }
        public FirstStep(LoginFirstStep loginFirstStep)
        {
            Date = loginFirstStep.Date;
            Key = loginFirstStep.Token;
            Valid = loginFirstStep.Valid;
            ID = loginFirstStep.Id;
        }
    }
}
