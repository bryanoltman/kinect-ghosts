using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghosts
{
    public partial class GhostDataContext
    {
        [Function(Name = "NEWID", IsComposable = true)]
        public Guid Random()
        { // to prove not used by our C# code... 
            throw new NotImplementedException();
        }
    }
}
