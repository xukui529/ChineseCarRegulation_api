using System;
using NFine.Domain.Entity.CustomCommon;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.CustomCommon
{
    public class SS_AppSettingsMap : EntityTypeConfiguration<SS_AppSettingsEntity>
    {
        public SS_AppSettingsMap()
        {
            this.ToTable("SS_AppSettings");
            this.HasKey(t => t.ID);
        }
    }
}
