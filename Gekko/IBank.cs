using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gekko
{
    public interface IBank
    {
        IVariable GetIVariable(string variable);

        void AddIVariable(string name, IVariable x);

        bool ContainsIVariable(string name);

        void RemoveIVariable(string name);

        string Message();
    }
}
