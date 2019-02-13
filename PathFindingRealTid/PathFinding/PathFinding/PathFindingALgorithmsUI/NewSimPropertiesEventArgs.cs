using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFindingALgorithmsUI
{
    public class NewSimPropertiesEventArgs : EventArgs
    {
        public Algorithms selected;        
        public string size, speed;
        public NewSimPropertiesEventArgs(string _size, string _speed, Algorithms _selected)
        {
            size = _size;
            speed = _speed;
            selected = _selected;
        }
    }
}
