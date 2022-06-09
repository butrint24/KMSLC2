using Kindergarten_Management_System.Contracts;
using Kindergarten_Management_System.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kindergarten_Management_System.Repository
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly ApplicationDbContext _db;

        public LeaveRequestRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool CreateT(LeaveRequest entity)
        {
            _db.LeaveRequests.Add(entity);
            return Save();
        }

        public bool DeleteT(LeaveRequest entity)
        {
            _db.LeaveRequests.Remove(entity);
            return Save();
        }

        public ICollection<LeaveRequest> FindAll()
        {
            var leaveHistories = _db.LeaveRequests
                .Include(q => q.RequestingEmployee)
                .Include(q => q.ApprovedBy)
                .Include(q => q.LeaveType)
                .ToList();
            return leaveHistories;
        }

        public LeaveRequest FindById(int id)
        {
            var leaveHistories = _db.LeaveRequests
                .Include(q => q.RequestingEmployee)
                .Include(q => q.ApprovedBy)
                .Include(q => q.LeaveType)
                .FirstOrDefault(q => q.Id == id);
            return leaveHistories;
        }

        public ICollection<LeaveRequest> GetLeaveRequestsByEmployee(string employeeid)
        {
            var leaveRequests = FindAll()
                .Where(q => q.RequestingEmployeeId == employeeid)
                .ToList();
            return leaveRequests;
        }

        public bool isExists(int id)
        {
            var exists = _db.LeaveRequests.Any(q => q.Id == id);
            return exists;
        }

        public bool Save()
        {
            var changes = _db.SaveChanges();
            return changes > 0;
        }

        public bool UpdateT(LeaveRequest entity)
        {
            _db.LeaveRequests.Update(entity);
            return Save();
        }
    }
}
