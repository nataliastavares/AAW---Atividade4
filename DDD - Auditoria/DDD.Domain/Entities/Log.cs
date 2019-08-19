using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Domain.Entities
{
    class Log : BaseEntity
    {
        public int idMovimento{ get; set; }
        public int IdContaCorrente { get; set; }
        public DateTime dataAlteracao { get; set; }
        public double valorMovimentado { get; set; }
        public char tipoOperacao { get; set; }
    }
}
