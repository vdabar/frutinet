﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frutinet.Common.Events
{
    public interface IAuthenticatedEvent : IEvent
    {
        Guid UserId { get; set; }
    }
}