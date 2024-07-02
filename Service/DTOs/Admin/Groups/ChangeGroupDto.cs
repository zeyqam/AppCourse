using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Admin.Groups
{
    public class ChangeGroupDto
    {
        public int OldGroupId { get; set; }
        public int NewGroupId { get; set; }
    }
}
