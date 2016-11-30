using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using Hei10.Domain.Models.Stock;

namespace Hei10.Domain.Entityframework.ModelConfigurations
{
   public class StockRecordConfiguration : EntityTypeConfiguration<StockRecord>
    {
        public StockRecordConfiguration()
        {
            Property(m => m.OpenPrice).HasPrecision(10, 2);
            Property(m => m.FormPrice).HasPrecision(10, 2);
            Property(m => m.MaxPrice).HasPrecision(10, 2);
            Property(m => m.MinPrice).HasPrecision(10, 2);
            Property(m => m.MTR).HasPrecision(10, 2);
            Property(m => m.ATR).HasPrecision(10, 2);

        }
    }
}
