using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Client.Android.Models.Results
{
    public class ValidLoginResult
    {
        public DateTime ValidationDate { get; set; }
        public DateTime LoginDate { get; set; }
        public bool IsValid { get; set; }

        public ValidLoginResult() { 
        
        }
        public override string ToString()
        {
            return $"Validation Date: {ValidationDate}\nLogin Date: {LoginDate}\nIs Valid: {IsValid}";
        }
    }
}
