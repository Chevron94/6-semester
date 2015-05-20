using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace BindingApplication
{
    public static class StringExtension
    {
        public static bool IsEmpty(this string t)
        {
            return t.Trim() == "";
        }
    }
}
