﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LadeskabClassLibrary
{
    public interface IChargeControl
    {
        bool IsConnected();
        void StartCharge();
        void StopCharge();
    }
}
