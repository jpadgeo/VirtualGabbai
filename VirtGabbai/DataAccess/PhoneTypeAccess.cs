﻿using System;
using System.Collections.Generic;
using System.Linq;
using DataCache;
using DataTypes;

namespace DataAccess
{
    public class PhoneTypeAccess
    {
        #region Read Methods

        #region Local type return

        public PhoneType GetPhoneTypeByTypeName(string typeName)
        {
            return null;//this.ConverSingleYahrtziehtToLocalType(this.LookupSpecificYahrtzieht(personId, yahr_date, personName));
        }

        public PhoneType GetPhoneTypeById(int id)
        {
            return null;//this.ConverSingleYahrtziehtToLocalType(this.LookupSpecificYahrtzieht(personId, yahr_date, personName));
        }

        public List<PhoneType> GetAllPhoneTypes()
        {
            return null;
            //return this.ConvertMultipleYahrtziehtsToLocalType(this.LookupAllYahrtziehts(personId));
        }

        #endregion

        #region Db type return

        private t_phone_types LookupPhoneTypeByTypeName(string typeName)
        {
            return null;
            //return (from CurrYahr in Cache.CacheData.t_yahrtziehts
            //        where CurrYahr.person_id == personId &&
            //              CurrYahr.date == yahr_date &&
            //              CurrYahr.deceaseds_name == personName
            //        select CurrYahr).First();
        }

        private List<t_phone_types> LookupAllPhoneTypes()
        {
            return null;
            //return (from CurrPerson in Cache.CacheData.t_people
            //        where CurrPerson.C_id == personId
            //        select CurrPerson).First().t_yahrtziehts.ToList<t_yahrtziehts>();
        }

        private t_phone_types LookupPhoneTypById(int ID)
        {
            return null;
            //return (from CurrYahr in Cache.CacheData.t_yahrtziehts
            //        where CurrYahr.C_id == ID
            //        select CurrYahr).First();
        }

        #endregion

        #endregion

        #region Write

        #region Create

        public void AddNewPhoneType(PhoneType newPhoneType)
        {
            t_phone_types phonrTypeToAdd = this.ConvertSinglePhoneTypeToDbType(newPhoneType);
            Cache.CacheData.t_phone_types.AddObject(phonrTypeToAdd);
            Cache.CacheData.SaveChanges();
        }

        public void AddMultipleNewPhoneTypes(List<PhoneType> newPhoneTypeList)
        {
            foreach (PhoneType newPhoneType in newPhoneTypeList)
            {
                this.AddNewPhoneType(newPhoneType);
            }
        }

        #endregion

        #region Update

        public void UpdateSinglePhoneType(PhoneType updatedPhoneType)
        {
            t_phone_types phoneTypeUpdating = this.LookupPhoneTypById(updatedPhoneType._Id);
            phoneTypeUpdating = this.ConvertSinglePhoneTypeToDbType(updatedPhoneType);
            Cache.CacheData.t_phone_types.ApplyCurrentValues(phoneTypeUpdating);
            Cache.CacheData.SaveChanges();
        }

        public void UpdateMultiplePhoneTypes(List<PhoneType> updatedPhoneTypeList)
        {
            foreach (PhoneType updatedPhoneType in updatedPhoneTypeList)
            {
                this.UpdateSinglePhoneType(updatedPhoneType);
            }
        }

        #endregion

        #region Delete

        public void DeleteSinglePhoneType(PhoneType deletedPhoneType)
        {
            t_phone_types phoneTypeDeleting = 
                Cache.CacheData.t_phone_types.First(phoneType => phoneType.C_id == deletedPhoneType._Id);
            Cache.CacheData.t_phone_types.DeleteObject(phoneTypeDeleting);
            Cache.CacheData.SaveChanges();
        }

        public void DeleteMultiplePhoneTypes(List<PhoneType> deletedPhoneTypeList)
        {
            foreach (PhoneType deletedPhoneType in deletedPhoneTypeList)
            {
                this.DeleteSinglePhoneType(deletedPhoneType);
            }
        }

        #endregion

        #endregion

        #region Private Methods

        private List<t_phone_types> ConvertMultiplePhoneTypesToDbType(List<PhoneType> localTypePhoneTypeList)
        {
            //List<t_yahrtziehts> dbTypeYahrList = new List<t_yahrtziehts>();

            //foreach (Yahrtzieht CurrYahr in localTypeYahrList)
            //{
            //    dbTypeYahrList.Add(this.ConvertSingleYahrtziehtToDbType(CurrYahr));
            //}

            //return dbTypeYahrList;
            return null;
        }

        private t_phone_types ConvertSinglePhoneTypeToDbType(PhoneType localTypePhoneType)
        {
            return null;
            //var dbTypeYahr = t_yahrtziehts.Createt_yahrtziehts(localTypeYahr._Id, localTypeYahr.PersonId,
            //                                             localTypeYahr.Date, localTypeYahr.Name);
            //dbTypeYahr.relation = localTypeYahr.Relation;
            //return dbTypeYahr;
        }

        private List<PhoneType> ConvertMultiplePhoneTypesToLocalType(List<t_phone_types> dbTypePhoneTypeList)
        {
            return null;
            //List<Yahrtzieht> localTypeYahrList = new List<Yahrtzieht>();

            //foreach (t_yahrtziehts Curryahr in dbTypeYahrList)
            //{
            //    localTypeYahrList.Add(this.ConverSingleYahrtziehtToLocalType(Curryahr));
            //}

            //return localTypeYahrList;
        }

        private PhoneType ConverSinglePhoneTypeToLocalType(t_phone_types dbTypePhoneType)
        {
            return null;
            //Yahrtzieht localTypeYahr = new Yahrtzieht(dbTypeYahr.C_id, dbTypeYahr.date, dbTypeYahr.deceaseds_name, dbTypeYahr.relation, dbTypeYahr.person_id);
            //return localTypeYahr;
        }

        #endregion
    }
}
