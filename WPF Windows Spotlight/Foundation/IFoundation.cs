﻿using System.ComponentModel;

namespace WPF_Windows_Spotlight.Foundation
{
    public interface IFoundation
    {
        void DoWork(object sender, DoWorkEventArgs e);
        void SetKeyword(string keyword);
    }
}