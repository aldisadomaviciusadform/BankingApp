using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.Accounts;

public class AccountAdd
{
    public Guid UserId { get; set; }
    public AccountType Type { get; set; }
}
