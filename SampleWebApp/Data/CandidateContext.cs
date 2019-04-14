using SampleWebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SampleWebApp.Data
{
    public class CandidateContext : DbContext
    {
        public CandidateContext() : base("CandidateContext")
        {

        }

        public DbSet<Candidate> Candidates { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<CandidateContext>(null);
            base.OnModelCreating(modelBuilder);
        }

    }
}