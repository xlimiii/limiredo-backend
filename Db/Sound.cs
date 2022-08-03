using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace limiredo_backend.Db
{
    [Table("sound")]
    public class Sound
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("file")]
        public string File { get; set; }
        [Column("height")]
        public int Height { get; set; }
        [Column("source_id")]
        public int SourceId { get; set; }
    }
}
