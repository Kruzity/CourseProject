﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CourseProjectServerApplication.Model
{
    public interface IAdminCredentialRepository
    {
        int AuthenticateAdmin(NetworkCredential adminCredential);
    }
}
