﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueliche
{
    interface ICollidable
    {
        bool OnCollision(Player player);
    }
}
