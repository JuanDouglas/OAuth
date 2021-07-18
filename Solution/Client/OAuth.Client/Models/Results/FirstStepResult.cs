using System;

namespace OAuth.Client.Models.Results
{
    /// <summary>
    /// Login First Step Api Result 
    /// </summary>
    public class FirstStepResult
    {
        /// <summary>
        /// First Step Identification
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Obtain First Step Date
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// First step key.
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// FIrst Step is valid.
        /// </summary>
        public bool Valid { get; set; }


        public FirstStepResult()
        {
        }
    }
}
