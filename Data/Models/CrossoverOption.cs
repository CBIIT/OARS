using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("ProtocolCrossoverOption", Schema = "DMU")]
    public class CrossoverOption
    {
        [Column("Protocol_Crossover_Option_Id"), Key]
        public int CrossoverOptionId { get; set; }
        [Column("Protocol_Crossover_Option_Name")]
        public string CrossoverOptionName { get; set; }
    }
}
