using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model
{
    public static class Extensions
    {
        public static IEnumerable<T> GetFlags<T>(this T e) where T: Enum
        {
            if (Convert.ToInt16(e) == 0)
                return null;
            return Enum.GetValues(e.GetType()).Cast<T>().Where(t => e.HasFlag(t));
        }
    }
}
