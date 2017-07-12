using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PwC.C4.ConnectionPool
{
    internal interface IConnection
    {
        bool Expired { get; }

        bool Alive { get; }

        Exception LastError { get; set; }

        void Open();

        void Close();

        void Dispose();
    }
}
