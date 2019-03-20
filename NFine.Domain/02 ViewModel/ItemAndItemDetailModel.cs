using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.ViewModel
{
    public class ItemAndItemDetailModel
    {
        public string Id { set; get; }
        public string ItemName { set; get; }
        public string ItemEnName { get; set; }
        public string ItemCode { set; get; }


        public string ItemId { set; get; }
        public string EnCode { set; get; } 
    }
}
