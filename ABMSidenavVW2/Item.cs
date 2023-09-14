using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMSidenavVW2
{
    public class Item
    {
        public int ItemNumero { get; set; }
        public int ItemPadre { get; set; }
        public string Nombre { get; set; }

        public Item(int itemNumero, int itemPadre, string nombre)
        {
            ItemNumero = itemNumero;
            ItemPadre = itemPadre;
            Nombre = nombre;
        }
    }
}
