using System;

namespace NFine.Domain.Entity.CustomCommon
{
    public class SS_AppSettingsEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string Description { get; set; }
    }
}
