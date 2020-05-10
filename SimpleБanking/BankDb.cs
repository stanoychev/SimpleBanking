using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace SimpleBanking
{
    public interface IBankDb
    {
        IDbSet<Transaction> Transactions { get; set; }
        IDbSet<Account> Accounts { get; set; }
        IDbSet<Customer> Customers { get; set; }
        int SaveChanges();
    }

    public class BankDb : DbContext, IBankDb
    {
        public BankDb() : base("BankDb")
        {

        }

        public IDbSet<Transaction> Transactions { get; set; }
        public IDbSet<Account> Accounts { get; set; }
        public IDbSet<Customer> Customers { get; set; }
    }

    public class Transaction
    {
        public int TransactionId { get; set; }
        public double Amount { get; set; }
        public DateTime? TimeOfExecution { get; set; }
        public virtual Customer From { get; set; }
        public virtual Customer To { get; set; }
    }

    public class Account
    {
        public int AccountId { get; set; }
        public double Balance { get; set; }
        public virtual ICollection<Transaction> Transactions => new HashSet<Transaction>();

        public virtual Customer Customer { get; set; }
    }

    public class Customer
    {
        [ForeignKey("Account")]
        public int CustomerId { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [MaxLength(64)]
        [Column(TypeName = "Binary")]
        public byte[] User { get; set; }

        [Required]
        [MaxLength(64)]
        [Column(TypeName = "Binary")]
        public byte[] Pin { get; set; }

        public Account Account { get; set; }
    }
}