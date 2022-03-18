using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBComputing.util
{
    class ListOption : IListableOption
    {
        public static implicit operator ListOption(LocString name) => new ListOption(name);
        public static implicit operator ListOption(string name) => new ListOption(name);

        public static bool operator ==(ListOption one, ListOption two) => one.Equals(two);

        public static bool operator !=(ListOption one, ListOption two) => !one.Equals(two);

        public string GetProperName()
        {
            return name;
        }

        public override string ToString()
        {
            return name;
        }

        private readonly string name;

        public ListOption(string name)
        {
            this.name = name ?? throw new ArgumentNullException("name");
        }

        public override bool Equals(object obj)
        {
            return obj is ListOption other && other.name == name;
        }

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }

    }
}
