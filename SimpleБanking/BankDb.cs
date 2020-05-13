using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.IO;

namespace SimpleBanking
{
    public interface IBankDb
    {
        IDbSet<Transaction> Transactions { get; set; }
        IDbSet<Customer> Customers { get; set; }
        int SaveChanges();
    }

    public class BankDb : DbContext, IBankDb
    {
        public BankDb() => new BankDb(Directory.GetCurrentDirectory());

        public BankDb(string dbPath) : base($"Server=.\\SQLEXPRESS;AttachDbFilename={dbPath}BankDb.mdf;Initial Catalog=BankDb;Integrated Security=True") { }

        public IDbSet<Transaction> Transactions { get; set; }
        public IDbSet<Customer> Customers { get; set; }
    }

    public class Transaction
    {
        public int TransactionId { get; set; }
        public double Amount { get; set; }
        public DateTime? Date { get; set; }

        public virtual Customer From { get; set; }
        public virtual Customer To { get; set; }
    }

    public class Customer
    {
        public int CustomerId { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [MaxLength(100)]
        public string User { get; set; }

        [Required]
        public string Pin { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();
    }
}