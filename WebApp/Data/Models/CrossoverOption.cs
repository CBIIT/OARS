using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OARS.Data.Models
{
    [Table("PROTOCOLCROSSOVEROPTION", Schema = "DMU")]
    public class CrossoverOption
    {
        [Column("PROTOCOL_CROSSOVER_OPTION_ID"), Key]
        public int CrossoverOptionId { get; set; }
        [Column("PROTOCOL_CROSSOVER_OPTION_NAME")]
        public string CrossoverOptionName { get; set; }
    }
}


