using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsBanking
{
    /// <summary>
    /// GIVEN
    /// this class is used to capture data to be passed
    /// among the windows forms
    /// </summary>
    public class ConstructorData
    {
        public string ClientNumber { get; set; }
        public string AccountNumber { get; set; }
        public string Balance { get; set; }
        public string Name { get; set; }
        public string Type {get;set;}
     }
}
