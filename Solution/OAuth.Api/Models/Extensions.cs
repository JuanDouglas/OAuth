using Microsoft.EntityFrameworkCore;
using OAuth.Api.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 
namespace OAuth.Api.Models
{
    public static class Extensions
    {
        public static void SaveChangesFailed(this ApiController apiController, object sender, SaveChangesEventArgs args)
        {
     
        }
    }
}
