using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gekko
{
    public enum EBankType
    {
        Normal,
        Map
    }

    public interface IBank
    {
        IVariable GetIVariable(string variable);

        void AddIVariable(string name, IVariable x);

        void AddIVariable(string name, IVariable x, bool isSimpleName);

        bool ContainsIVariable(string name);

        void RemoveIVariable(string name);

        EBankType BankType();

        string Message();
    }
}
