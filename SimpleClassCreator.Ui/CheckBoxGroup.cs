﻿using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;

namespace SimpleClassCreator.Ui
{
    public class CheckBoxGroup
    {
        private readonly List<CheckBox> _checkBoxes = new List<CheckBox>();

        public void Add(CheckBox checkBox) => _checkBoxes.Add(checkBox);

        public bool HasTickedCheckBox() => _checkBoxes.Any(x => x.IsChecked.GetValueOrDefault());
    }
}
