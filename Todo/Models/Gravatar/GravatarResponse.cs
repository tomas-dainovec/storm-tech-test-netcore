using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Todo.Models.Gravatar
{
    public class GravatarResponse
    {
        public IEnumerable<GravatarProfile> Entry { get; set; }
    }
}
