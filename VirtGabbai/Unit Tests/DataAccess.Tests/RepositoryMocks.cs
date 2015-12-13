﻿using DataCache.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Tests
{
    //TODO move this into a seperate project
    public static class RepositoryMocks
    {
        public static PhoneTypeRepository GetMockPhoneTypeRepository(List<PhoneType> data = null)
        {
            var mockContext = new Mock<ZeraLeviContext>();
            var mockSet = new Mock<DbSet<PhoneType>>().SetupData(data ?? new List<PhoneType>());
            mockContext.Setup(c => c.PhoneTypes).Returns(mockSet.Object);
            return new PhoneTypeRepository(mockContext.Object);
        }

        internal static PrivilegeGroupRepository GetMockPrivilegesGroupRepository(List<PrivilegesGroup> data = null)
        {
            var mockContext = new Mock<ZeraLeviContext>();
            var mockSet = new Mock<DbSet<PrivilegesGroup>>().SetupData(data ?? new List<PrivilegesGroup>());
            mockContext.Setup(c => c.PrivilegesGroups).Returns(mockSet.Object);
            return new PrivilegeGroupRepository(mockContext.Object);
        }

        public static PrivilegeRepository GetMockPrivilegeRepository(List<Privilege> data = null)
        {
            var mockContext = new Mock<ZeraLeviContext>();
            var mockSet = new Mock<DbSet<Privilege>>().SetupData(data ?? new List<Privilege>());
            mockContext.Setup(c => c.Privileges).Returns(mockSet.Object);
            return new PrivilegeRepository(mockContext.Object);
        }
    }
}
