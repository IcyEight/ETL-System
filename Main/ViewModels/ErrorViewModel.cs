﻿using System;
namespace Main.ViewModels
{
    public class ErrorViewModel
    {
        public ErrorViewModel(){}
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}