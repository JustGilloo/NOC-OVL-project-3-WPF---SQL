﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Groepsproject_Blokken
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BenKrabbeDBEntities : DbContext
    {
        public BenKrabbeDBEntities()
            : base("name=BenKrabbeDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<GameLogSP> GamesLogSP { get; set; }
        public virtual DbSet<GameLogVS> GamesLogVS { get; set; }
        public virtual DbSet<Manager> Managers { get; set; }
        public virtual DbSet<Player> Players { get; set; }
    }
}