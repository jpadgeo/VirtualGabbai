﻿using DataAccess.IntegrationTests.Helpers;
using DataCache.Models;
using GenFu;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IntegrationTests
{
    [TestClass()]
    public class UserRepositoryTests
    {
        VGTestContext _ctx = new VGTestContext();
        UserRepository repository;

        [TestInitialize()]
        public void Setup()
        {
            repository = new UserRepository(_ctx);
        }

        [TestCleanup()]
        public void Cleanup()
        {
            _ctx.Database.Delete();
        }

        [TestMethod]
        public void Exists_Item_Null_Item_Returns_False()
        {
            Assert.IsFalse(repository.Exists(null));
        }

        [TestMethod]
        public void Exists_Item_No_Match_Returns_False()
        {
            var item = A.New<User>();
            Assert.IsFalse(repository.Exists(item));
        }

        [TestMethod]
        public void Exists_Item_Match_Found_Returns_True()
        {
            var item = Helper.SetupData(_ctx);

            Assert.IsTrue(repository.Exists(item));
        }

        [TestMethod]
        public void Exists_ID_No_Match_Returns_False()
        {
            Assert.IsFalse(repository.Exists(1));
        }

        [TestMethod]
        public void Exists_ID_Match_Returns_True()
        {
            var item = Helper.SetupData(_ctx);

            Assert.IsTrue(repository.Exists(item.ID));
        }

        [TestMethod]
        public void Get_Returns_All_items()
        {
            var items = Helper.SetupData(_ctx, 2);

            CollectionAssert.AreEquivalent(items, repository.Get().ToList());
        }

        [TestMethod]
        public void GetByID_No_Data_Returns_Null()
        {
            Assert.IsNull(repository.GetByID(1));
        }

        [TestMethod]
        public void GetByID_No_Match_Returns_Null()
        {
            var items = Helper.SetupData(_ctx, 2);

            Assert.IsNull(repository.GetByID(items.Max(d => d.ID) + 1));
        }

        [TestMethod]
        public void GetByID_Match_Returns_Match()
        {
            var expected = Helper.SetupData(_ctx);

            Assert.AreEqual(expected, repository.GetByID(expected.ID));
        }

        [TestMethod]
        public void Add_ItemIsNull_NothingHappens()
        {
            var before = Helper.SetupData(_ctx, 5);

            repository.Add(null);

            var after = repository.Get().ToList();

            CollectionAssert.AreEquivalent(before, after);
        }

        [TestMethod]
        public void Add_ValidItem_Added()
        {
            Helper.SetupData(_ctx, 5);

            var before = repository.Get().ToList();

            var item = Helper.GenFuSetup(1, before.Select(u => u.PrivilegeGroup.GroupName))
                             .First();
            repository.Add(item);

            var after = repository.Get();

            Assert.IsFalse(before.Contains(item));
            Assert.IsTrue(after.Contains(item));
        }

        [TestMethod]
        public void Delete_ItemIsNull_NothingHappens()
        {
            var before = Helper.SetupData(_ctx, 5);

            repository.Delete(null);

            var after = repository.Get().ToList();

            CollectionAssert.AreEquivalent(before, after);
        }

        [TestMethod]
        public void Delete_ItemNotInDatabase_NothingHappens()
        {
            var before = Helper.SetupData(_ctx, 5);

            var item = Helper.GenFuSetup(1, before.Select(u => u.PrivilegeGroup.GroupName))
                             .First();
            item.ID = before.Max(p => p.ID) + 1;

            repository.Delete(item);

            var after = repository.Get().ToList();

            CollectionAssert.AreEquivalent(before, after);
        }

        [TestMethod]
        public void Delete_ItemIsValid_IsRemovedFromDatabase()
        {
            var before = Helper.SetupData(_ctx, 5);

            var item = before.Skip(1).First();
            repository.Delete(item);

            var after = repository.Get();

            Assert.IsTrue(before.Contains(item));
            Assert.IsFalse(after.Contains(item));
        }

        [TestMethod]
        public void Save_ItemIsNull_NothingHappens()
        {
            var before = Helper.SetupData(_ctx, 3);

            repository.Save(null);

            var after = repository.Get().ToList();

            CollectionAssert.AreEquivalent(before, after);
        }

        [TestMethod]
        public void Save_ItemIsNew_ItemIsAddedToDatabase()
        {
            var before = Helper.SetupData(_ctx, 3);

            var item = Helper.GenFuSetup(1, before.Select(u => u.PrivilegeGroup.GroupName))
                             .First();

            Assert.IsFalse(before.Contains(item));

            repository.Save(item);

            var after = repository.Get().ToList();

            Assert.IsTrue(after.Contains(item));
        }

        [TestMethod]
        public void Save_ExistingItemPrivilegeGroupChanged_ValuesAreUpdated()
        {
            var item = Helper.SetupData(_ctx);

            repository.Save(item);

            var after = repository.GetByID(item.ID);

            Assert.AreEqual(item, after);
        }

        class Helper
        {
            public static List<User> GenFuSetup(int count, IEnumerable<string> currentPGNames)
            {
                var generatedPrivileges = A.ListOf<Privilege>();
                var privileges = new List<Privilege>();
                foreach (var gP in generatedPrivileges)
                {
                    if (privileges.FirstOrDefault(pt => pt.Name.Equals(gP.Name, StringComparison.CurrentCultureIgnoreCase)) == null)
                    {
                        privileges.Add(gP);
                    }
                }

                while (privileges.Count < 25)
                {
                    generatedPrivileges = A.ListOf<Privilege>();
                    foreach (var gP in generatedPrivileges)
                    {
                        if (privileges.FirstOrDefault(pt => pt.Name.Equals(gP.Name, StringComparison.CurrentCultureIgnoreCase)) == null)
                        {
                            privileges.Add(gP);
                        }
                    }
                }

                var listOfPrivilegeLists = new List<List<Privilege>>();

                listOfPrivilegeLists.Add(privileges.Take(5).ToList());
                listOfPrivilegeLists.Add(privileges.Skip(5).Take(5).ToList());
                listOfPrivilegeLists.Add(privileges.Skip(10).Take(5).ToList());
                listOfPrivilegeLists.Add(privileges.Skip(15).Take(5).ToList());
                listOfPrivilegeLists.Add(privileges.Skip(20).Take(5).ToList());


                A.Configure<PrivilegesGroup>()
                    .Fill(pg => pg.Privileges)
                    .WithRandom(listOfPrivilegeLists);

                var users = A.ListOf<User>(count);

                foreach (var user in users)
                {
                    var pg = A.New<PrivilegesGroup>();

                    while (!currentPGNames.Any() || currentPGNames.Contains(pg.GroupName, StringComparer.CurrentCultureIgnoreCase))
                    {
                        pg = A.New<PrivilegesGroup>();
                    }

                    user.PrivilegeGroup = pg;
                }

                return users;
            }

            public static User SetupData(VGTestContext ctx)
            {
                var user = GenFuSetup(1, Enumerable.Empty<string>()).First();
                ctx.Users.Add(user);
                ctx.SaveChanges();

                return user;
            }

            public static List<User> SetupData(VGTestContext ctx, int count)
            {
                var users = GenFuSetup(count, Enumerable.Empty<string>());
                ctx.Users.AddRange(users);
                ctx.SaveChanges();

                return users;
            }
        }
    }
}