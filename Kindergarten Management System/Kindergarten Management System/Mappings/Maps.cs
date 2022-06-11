using AutoMapper;
using Kindergarten_Management_System.Data;
using Kindergarten_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kindergarten_Management_System.Mappings
{
    public class Maps : Profile
    {
        public Maps()
        {
            CreateMap<LeaveType, LeaveTypeVM>().ReverseMap();

            CreateMap<LeaveAllocation, LeaveAllocationVM>().ReverseMap();

            CreateMap<LeaveRequest, LeaveRequestVM>().ReverseMap();

            CreateMap<AppUser, AppUserVM>().ReverseMap();

        }
    }
}