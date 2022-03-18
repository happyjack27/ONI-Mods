using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBComputing.SideScreens
{
    class ListOption<T> : IListableOption
    {
        public string name;
        public T value;
        public ListOption(T value, string name)
        {
            this.name = name;
            this.value = value;
        }
        public string GetProperName()
        {
            return name;
        }
    }
}
