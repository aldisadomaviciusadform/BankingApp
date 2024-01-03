using Application.Dto.Accounts;
using Application.Dto.Transactions;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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

        Transaction transaction = new ()
        {
            Id = transactionEntity.Id,
            Type = transactionEntity.Type,
            Amount = transactionEntity.Amount,
            Created = transactionEntity.Created,
            AccountId = transactionEntity.AccountId,
        };

        return transaction;
    }

    const decimal TransactionFee = 1;

    public async Task CreateTransaction(Guid senderId, TransactionSend transaction)
    {
        IDbTransaction dbTransaction = _transactionRepository.StartTransaction();

        if (transaction.Amount <= 0.01m)
            throw new ArgumentException("Send amount cant be less or equal zero");

        AccountEntity accountSender = await _accountRepository.Get(senderId) ?? throw new NotFoundException("Account not found in DB");
        AccountEntity accountReceiver = await _accountRepository.Get(transaction.ReceiverAccount) ?? throw new NotFoundException("Account not found in DB");

        if (accountSender.Balance < (TransactionFee + transaction.Amount))
            throw new ArgumentException("Insufficient balance");

        accountSender.Balance -= (TransactionFee + transaction.Amount);
        await _accountRepository.UpdateAmount(accountSender, dbTransaction);
        TransactionEntity transactionSend = new()
        {
            Type = TransactionType.Send,
            Amount = -transaction.Amount,
            AccountId = senderId,
        };

        await _accountRepository.UpdateAmount(accountReceiver, dbTransaction);
        await _transactionRepository.Add(transactionSend, dbTransaction);

        accountReceiver.Balance += transaction.Amount;
        await _accountRepository.UpdateAmount(accountSender, dbTransaction);
        TransactionEntity transactionReceive = new()
        {
            Type = TransactionType.Received,
            Amount = transaction.Amount,
            AccountId = transaction.ReceiverAccount,
        };

        await _accountRepository.UpdateAmount(accountReceiver, dbTransaction);
        await _transactionRepository.Add(transactionReceive, dbTransaction);

        TransactionEntity transactionServiceFee = new()
        {
            Type = TransactionType.Fee,
            Amount = TransactionFee,
            AccountId = senderId,
        };

        await _transactionRepository.Add(transactionServiceFee, dbTransaction);

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

    private async Task SendTransaction(Guid accountId, decimal amount)
    {

    }

    private async Task RecieveTransaction(Guid accountId, decimal amount)
    {

    }

    private async Task ServiceFeeTransaction(Transaction transaction)
    {

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
