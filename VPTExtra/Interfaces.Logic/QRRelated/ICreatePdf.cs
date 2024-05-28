using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Logic.QRRelated
{
    public interface ICreatePdf
    {
        public byte[] CreateTicketPdf(int userId, int eventId);
    }
}
