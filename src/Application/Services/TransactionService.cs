using Application.Dto.Transactions;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using System.Data;
using System.Threading.Tasks;


namespace Application.Services;

public class TransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;

    public TransactionService(ITransactionRepository transactionRepository, IAccountRepository accountRepository)
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
    }

    public async Task<IEnumerable<Transaction>> Get()
    {
        List<Transaction> transactions = [];
        IEnumerable<TransactionEntity> transactionsEntities = await _transactionRepository.Get();

        if (!transactionsEntities.Any())
            return [];

        transactions = transactionsEntities.Select(o => new Transaction()
        {
            Id = o.Id,
            Type = o.Type,
            Amount = o.Amount,
            Created = o.Created,
            AccountId = o.AccountId,
        }).ToList();

        return transactions;
    }

    public async Task<IEnumerable<Transaction>> GetUsersTransactions(Guid id)
    {
        List<Transaction> transactions = [];
        IEnumerable<TransactionEntity> transactionsEntities = await _transactionRepository.GetUsersTransactions(id);

        if (!transactionsEntities.Any())
            return [];

        transactions = transactionsEntities.Select(o => new Transaction()
        {
            Id = o.Id,
            Type = o.Type,
            Amount = o.Amount,
            Created = o.Created,
            AccountId = o.AccountId,
        }).ToList();

        return transactions;
    }

    public async Task<IEnumerable<Transaction>> GetAccountsTransactions(Guid id)
    {
        List<Transaction> transactions = [];
        IEnumerable<TransactionEntity> transactionsEntities = await _transactionRepository.GetAccountsTransactions(id);

        if (!transactionsEntities.Any())
            return [];

        transactions = transactionsEntities.Select(o => new Transaction()
        {
            Id = o.Id,
            Type = o.Type,
            Amount = o.Amount,
            Created = o.Created,
            AccountId = o.AccountId,
        }).ToList();

        return transactions;
    }

    public async Task<Transaction> Get(Guid id)
    {
        TransactionEntity transactionEntity = await _transactionRepository.Get(id) ?? throw new NotFoundException("Transaction not found in DB");

        Transaction transaction = new()
        {
            Id = transactionEntity.Id,
            Type = transactionEntity.Type,
            Amount = transactionEntity.Amount,
            Created = transactionEntity.Created,
            AccountId = transactionEntity.AccountId,
        };

        return transaction;
    }

    public async Task CreateTransaction(Guid senderId, TransactionSend transaction)
    {
        IDbTransaction dbTransaction = _transactionRepository.StartTransaction();

        if (transaction.Amount <= 0.01m)
            throw new ArgumentException("Send amount cant be less or equal zero");

        Task.WaitAll
        (
            SendTransaction(senderId, transaction.Amount, dbTransaction),
            ReceiveTransaction(transaction.ReceiverAccount, transaction.Amount, dbTransaction),
            ServiceFeeTransaction(senderId, dbTransaction)
        );

        try
        {
            _transactionRepository.EndTransaction(dbTransaction);
        }
        catch (Exception)
        {
            dbTransaction.Rollback();
            throw;
        } 
    }

    const decimal TransactionFee = 1;
    private async Task SendTransaction(Guid accountId, decimal amount, IDbTransaction dbTransaction)
    {
        AccountEntity accountSender = await _accountRepository.Get(accountId) ?? throw new NotFoundException("Account not found in DB");
        if (accountSender.Balance < (TransactionFee + amount))
            throw new ArgumentException("Insufficient balance");

        accountSender.Balance -= (TransactionFee + amount);
        await _accountRepository.UpdateAmount(accountSender, dbTransaction);
        TransactionEntity transactionSend = new()
        {
            Type = TransactionType.Send,
            Amount = -amount,
            AccountId = accountId,
        };

        Task.WaitAll
        (
            _transactionRepository.Add(transactionSend, dbTransaction),
            _accountRepository.UpdateAmount(accountSender, dbTransaction)
        );
    }

    private async Task ReceiveTransaction(Guid accountId, decimal amount, IDbTransaction dbTransaction)
    {
        AccountEntity accountReceiver = await _accountRepository.Get(accountId) ?? throw new NotFoundException("Account not found in DB");

        accountReceiver.Balance += amount;
        await _accountRepository.UpdateAmount(accountReceiver, dbTransaction);

        TransactionEntity transactionReceive = new()
        {
            Type = TransactionType.Received,
            Amount = amount,
            AccountId = accountId,
        };

        Task.WaitAll
        (
            _accountRepository.UpdateAmount(accountReceiver, dbTransaction),
            _transactionRepository.Add(transactionReceive, dbTransaction)
        );
    }

    private async Task ServiceFeeTransaction(Guid accountId, IDbTransaction dbTransaction)
    {
        TransactionEntity transactionServiceFee = new()
        {
            Type = TransactionType.Fee,
            Amount = TransactionFee,
            AccountId = accountId,
        };

        await _transactionRepository.Add(transactionServiceFee, dbTransaction);
    }

    public async Task TopUpTransaction(Guid accountId, TopUp topUp)
    {
        if (topUp.Amount <= 0.01m)
            throw new ArgumentException("Top up amount cant be less or equal zero");

        AccountEntity account = await _accountRepository.Get(accountId) ?? throw new NotFoundException("Account not found in DB");

        IDbTransaction dbTransaction = _accountRepository.StartTransaction();

        account.Balance += topUp.Amount;
        await _accountRepository.UpdateAmount(account, dbTransaction);

        TransactionEntity transaction = new()
        {
            Type = TransactionType.Tops,
            Amount = topUp.Amount,
            AccountId = accountId,
        };

        await _transactionRepository.Add(transaction, dbTransaction);

        try
        {
            _accountRepository.EndTransaction(dbTransaction);
        }
        catch (Exception)
        {
            dbTransaction.Rollback();
            throw;
        }
    }
}
