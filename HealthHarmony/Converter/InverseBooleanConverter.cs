﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace HealthHarmony.Converter;

public class InverseBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        !((bool)value);

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)=>
        throw new NotImplementedException();
}