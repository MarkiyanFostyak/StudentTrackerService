﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace StudentTracker.Domain.IdentityEntities
{
    public class Role: IdentityRole<int, UserRole>
    {
    }
}
