﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Asset.Model
{
    public interface IPlateAction
    {
        Unit reference { get; set; }
        void Handle();
        void Prepare();
    }
}
