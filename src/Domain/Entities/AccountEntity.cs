using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public enum AccountType
{
    Default,
    Saving,
}

public class AccountEntity:BaseEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public AccountType Type { get; set; }
    public decimal Balance { get; set; }
}
