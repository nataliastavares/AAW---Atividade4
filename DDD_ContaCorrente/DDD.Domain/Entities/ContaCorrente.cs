using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Domain.Entities
{
    public class ContaCorrente : BaseEntity
    {
        public int idContaCorrente { get; set; }
        public double saldo { get; set; }
        public double limiteCredito { get; set; }
    }
}
