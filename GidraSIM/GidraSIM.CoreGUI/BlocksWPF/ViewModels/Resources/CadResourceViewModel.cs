﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GidraSIM.Core.Model.Resources;

namespace GidraSIM.GUI.Core.BlocksWPF.ViewModels.Resources
{
    public class CadResourceViewModel : ResourceWPF
    {
        public CadResource Model { get; set; }

        public CadResourceViewModel(Point position, string resourceName) : base(position, resourceName)
        {
            Model = new CadResource();
        }

        public int Count => Model.Count;
        public string Description => Model.Description;
    }
}
