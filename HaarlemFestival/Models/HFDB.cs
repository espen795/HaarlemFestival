using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class HFDB : DbContext
    {
        /* UPDATE DATABASE
         * enable-migrations
         * add-migration naam
          * update-database
         */
    
        public HFDB() : base("name=HFDB")
        {

        }
        DbModelBuilder modelBuilder = new DbModelBuilder();
        public DbSet<Account> Accounts { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Activity>().ToTable("Activities");
            modelBuilder.Entity<Jazz>().ToTable("Jazzs");
            modelBuilder.Entity<Dinner>().ToTable("Dinners");
            modelBuilder.Entity<Historic>().ToTable("Historics");
            modelBuilder.Entity<Talking>().ToTable("Talkings");
        }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<BesteldeActiviteit> BesteldeActiviteiten { get; set; }
        public DbSet<Cuisine> Cuisines { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<Dinner> Dinners { get; set; }
        public DbSet<Guide> Guides { get; set; }
        public DbSet<Historic> Historics { get; set; }
        public DbSet<InterviewQuestion> InterviewQuestions { get; set; }
        public DbSet<Jazz> Jazzs { get; set; }
        public DbSet<Klant> Klanten { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Reservering> Reserveringen { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Talk> Talks { get; set; }
        public DbSet<Talking> Talkings { get; set; }
    }
}
