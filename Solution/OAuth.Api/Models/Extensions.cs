using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
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

        public static void AddResponseHeader(this IApplicationBuilder app, string name, StringValues values) {

        }
    }
}
