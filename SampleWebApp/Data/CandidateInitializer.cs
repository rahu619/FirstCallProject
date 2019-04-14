using SampleWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleWebApp.Data
{
    public class CandidateInitializer : System.Data.Entity.DropCreateDatabaseAlways<CandidateContext>
    {
        protected override void Seed(CandidateContext context)
        {
            var candidates = new List<Candidate>
            {
                new Candidate{ FirstName="Paul",LastName="Grande",Email="pauli@gmail.com",Mobile="999-0878-78"},
                new Candidate{ FirstName="Pauli",LastName="Grande",Email="fauli@gmail.com",Mobile="999-0878-78"},
                new Candidate{ FirstName="Paulg",LastName="Grande",Email="gauli@gmail.com",Mobile="999-0878-78"}

            };


            context.SaveChanges();
        }


    }
}