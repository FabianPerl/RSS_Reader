﻿using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Constants;
using Prism.Logging;

namespace ModuleEdit.ViewModels
{
    public class EditFeedFormUserControlViewModel : BindableBase
    {
        private readonly ILoggerFacade _logger = ProjectLogger.GetLogger;
        private string _message;
        public string Message
        {
            get { return _message; }
            set => SetProperty(ref _message, value);
        }

        public EditFeedFormUserControlViewModel()
        {
            Message = "View A from your Prism Module";
        }
    }
}
