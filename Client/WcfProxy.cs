using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class WcfProxy : IDisposable
    {
        // Veza sa serverom
        // Implementirati IDisposable tako da se konekcija sigurno zatvori čak i ako pukne prenos.
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
