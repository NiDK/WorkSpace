using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PwC.C4.ConnectionPool
{
    internal interface IConnectionPool
    {
        void EnterLimit();

        void ReleaseLimit();
        
        XConnection GetConnection();
        
        void ReleaseConnection(XConnection connection);
        
        void ClearConnections();
        
        void Dispose();
    }
}
