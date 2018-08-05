﻿using DevExpress.XtraEditors;
using System;
using System.ComponentModel;
using TQElectronic.Utils.DevExpress.Editors;
using TQElectronic.Utils.DevExpress.UserControls.Settings;

namespace TQElectronic.Utils.DevExpress.UserControls
{
    public partial class SeriesLineSettingsControl : XtraUserControl
    {
        private SeriesLineSettings _settings;

        [Browsable(false)]
        public SeriesLineSettings Settings
        {
            get
            {
                if (_settings == null || IsImmutable)
                    _settings = Activator.CreateInstance<SeriesLineSettings>();

                _settings.Color = colorEdit1.Color;
                _settings.Thickness = textEdit1.GetInt(1);

                return _settings;
            }
            set
            {
                _settings = value;

                if (_settings != null)
                {
                    colorEdit1.Color = _settings.Color;
                    textEdit1.EditValue = _settings.Thickness;
                }
            }
        }

        [Browsable(true)]
        public bool IsImmutable { get; set; }

        public SeriesLineSettingsControl()
        {
            InitializeComponent();
        }
    }
}