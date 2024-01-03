using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public enum TransactionType
{
    Fee,
    Tops,
    Send,
    Received,
}


public class TransactionEntity
{
    public DateTime Created { get; set; }
    public Guid Id { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public Guid AccountId { get; set; }
}
