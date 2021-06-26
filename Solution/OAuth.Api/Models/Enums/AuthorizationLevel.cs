﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth.Api.Models.Enums
{
    public enum AuthorizationLevel : uint
    {
        Basic = 1,
        Details = 2,
        Manager = 3
    }
}
