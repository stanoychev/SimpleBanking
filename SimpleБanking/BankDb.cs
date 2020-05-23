using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace SimpleBanking
{
    public interface IBankDb : IDisposable
    {
        IDbSet<Transaction> Transactions { get; set; }
        IDbSet<Customer> Customers { get; set; }
        int SaveChanges();
    }

    public class BankDb : DbContext, IBankDb
    {
        public IDbSet<Transaction> Transactions { get; set; }
        public IDbSet<Customer> Customers { get; set; }
    }

    public class Transaction
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public DateTime? Date { get; set; }

        public virtual Customer Sender { get; set; }
        public virtual Customer Receiver { get; set; }
    }

    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [MaxLength(100)]
        public string User { get; set; }

        [Required]
        public string Pin { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string Cookie { get; set; }
        public DateTime? ExpiresOn { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();
    }
}