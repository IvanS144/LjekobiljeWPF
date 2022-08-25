using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ljekobilje
{
    public partial class CooperantBankAccount
    {
        public override string ToString()
        {
            return Account.Bank + " " + Account.AccountNumber;
        }
    }

    public partial class Station
    {
        public override string ToString()
        {
            return "["+ StationId +"]" + " " + Adress + ", " + City;
        }
    }

    public partial class Purchasespaid
    {
        public bool NotPaid { get { return (Status > 0) ? false : true; } }
    }

    public partial class Cooperant
    {
        public override string ToString()
        {
            return "[" + CooperantId + "]" + " " + FirstName + " " + LastName;
        }
    }

    public partial class Plant
    {
        public override string ToString()
        {
            return "[" + PlantId + "]" + " " + PlantName;
        }
    }
}
