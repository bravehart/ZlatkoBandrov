﻿using System;
using System.Linq;
using ZlatkoBandrov.DataAccess.Repositories.Generic;

namespace ZlatkoBandrov.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private static readonly FourthEntities FourthDbContext = new FourthEntities();

        #region Repositories

        private GenericRepository<Employee> _employeeRepository;
        public GenericRepository<Employee> EmployeeRepository
        {
            get { return this._employeeRepository ?? (this._employeeRepository = new GenericRepository<Employee>(FourthDbContext)); }
        }

        private GenericRepository<Role> _roleRepository;
        public GenericRepository<Role> RoleRepository
        {
            get { return this._roleRepository ?? (this._roleRepository = new GenericRepository<Role>(FourthDbContext)); }
        }

        private GenericRepository<Team> _teamRepository;
        public GenericRepository<Team> TeamRepository
        {
            get { return this._teamRepository ?? (this._teamRepository = new GenericRepository<Team>(FourthDbContext)); }
        }

        private GenericRepository<EmployeeArrival> _employeeArrivalRepository;
        public GenericRepository<EmployeeArrival> EmployeeArrivalRepository
        {
            get { return this._employeeArrivalRepository ?? (this._employeeArrivalRepository = new GenericRepository<EmployeeArrival>(FourthDbContext)); }
        }

        #endregion

        public void Rollback()
        {
            try
            {
                FourthDbContext.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
            }
            catch (Exception ex)
            {
                Dispose(true);
                throw ex;
            }
        }

        public void SaveChanges()
        {
            try
            {
                FourthDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Dispose(true);
                throw ex;
            }
        }

        #region Dispose Pattern

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    FourthDbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
