﻿using System;
using System.Collections.Generic;
using System.Linq;
using DataCache;
using DataTypes;

namespace DataAccess
{
    public static class PhoneNumberAccess
    {
        #region Read Methods

        #region Local type return

        public static List<PhoneNumber> GetPhoneNumberByType(PhoneType searchedType)
        {
            return ConvertMultipleDbPhoneNumbersToLocalType(LookupPhoneNumberByType(searchedType));
        }

        public static PhoneNumber GetPhoneNumberById(int id)
        {
            return ConvertSingleDbPhoneNumberToLocalType(LookupPhoneNumberById(id));
        }

        public static List<PhoneNumber> GetAllPhoneNumbers(int personId)
        {
            return ConvertMultipleDbPhoneNumbersToLocalType(LookupAllPhoneNumbers(personId));
        }

        public static PhoneNumber GetSpecificPhoneNumber(string phoneNumber, PhoneType numberType)
        {
            return ConvertSingleDbPhoneNumberToLocalType(LookupSpecificPhoneNumber(phoneNumber, numberType._Id));
        }

        #endregion

        #region Db type return

        private static List<t_phone_numbers> LookupPhoneNumberByType(PhoneType searchedType)
        {
            return (from CurrPhoneNumber in Cache.CacheData.t_phone_numbers
                    where CurrPhoneNumber.number_type == searchedType._Id
                    select CurrPhoneNumber).ToList<t_phone_numbers>();
        }

        private static t_phone_numbers LookupSpecificPhoneNumber(string phoneNumber, int numberType)
        {
            return (from CurrPhoneNumber in Cache.CacheData.t_phone_numbers
                    where CurrPhoneNumber.number == phoneNumber &&
                          CurrPhoneNumber.number_type == numberType
                    select CurrPhoneNumber).First();
        }

        private static List<t_phone_numbers> LookupAllPhoneNumbers(int personId)
        {
            return (from CurrPhoneType in Cache.CacheData.t_phone_numbers
                    where CurrPhoneType.person_id == personId
                    select CurrPhoneType).ToList<t_phone_numbers>();
        }

        private static t_phone_numbers LookupPhoneNumberById(int id)
        {
            return (from CurrPhoneType in Cache.CacheData.t_phone_numbers
                    where CurrPhoneType.C_id == id
                    select CurrPhoneType).First();
        }

        #endregion

        #endregion

        #region Write

        #region Create

        public static void AddNewPhoneNumber(PhoneNumber newPhoneNumber, int personId)
        {
            t_phone_numbers phonrNumberToAdd = ConvertSingleLocalPhoneNumberToDbType(newPhoneNumber, personId);
            Cache.CacheData.t_phone_numbers.AddObject(phonrNumberToAdd);
            Cache.CacheData.SaveChanges();
        }

        public static void AddMultipleNewPhoneTypes(List<PhoneNumber> newPhoneNumberList, int personId)
        {
            foreach (PhoneNumber newPhoneNumber in newPhoneNumberList)
            {
                AddNewPhoneNumber(newPhoneNumber, personId);
            }
        }

        #endregion

        #region Update

        public static void UpdateSinglePhoneNumber(PhoneNumber updatedPhoneNumber, int personId)
        {
            t_phone_numbers phoneTypeUpdating = LookupPhoneNumberById(updatedPhoneNumber._Id);
            phoneTypeUpdating = ConvertSingleLocalPhoneNumberToDbType(updatedPhoneNumber, personId);
            Cache.CacheData.t_phone_numbers.ApplyCurrentValues(phoneTypeUpdating);
            Cache.CacheData.SaveChanges();
        }

        public static void UpdateMultiplePhoneNumbers(List<PhoneNumber> updatedPhoneNumberList, int personId)
        {
            foreach (PhoneNumber updatedPhoneNumber in updatedPhoneNumberList)
            {
                UpdateSinglePhoneNumber(updatedPhoneNumber, personId);
            }
        }

        #endregion

        #region Delete

        public static void DeleteSinglePhoneNumber(PhoneNumber deletedPhoneNumber)
        {
            t_phone_numbers phoneTypeDeleting = 
                Cache.CacheData.t_phone_numbers.First(number => number.C_id == deletedPhoneNumber._Id);
            Cache.CacheData.t_phone_numbers.DeleteObject(phoneTypeDeleting);
            Cache.CacheData.SaveChanges();
        }

        public static void DeleteMultiplePhoneNumbers(List<PhoneNumber> deletedPhoneNumberList)
        {
            foreach (PhoneNumber deletedPhoneNumber in deletedPhoneNumberList)
            {
                DeleteSinglePhoneNumber(deletedPhoneNumber);
            }
        }

        #endregion

        #endregion

        #region Private Methods

        private static List<t_phone_numbers> ConvertMultipleLocalPhoneNumbersToDbType(List<PhoneNumber> localTypePhoneNumberList, int personId)
        {
            List<t_phone_numbers> dbTypePhoneNumberList = new List<t_phone_numbers>();

            foreach (PhoneNumber CurrPhoneNumber in localTypePhoneNumberList)
            {
                dbTypePhoneNumberList.Add(ConvertSingleLocalPhoneNumberToDbType(CurrPhoneNumber, personId));
            }

            return dbTypePhoneNumberList;
        }

        private static t_phone_numbers ConvertSingleLocalPhoneNumberToDbType(PhoneNumber localTypePhoneNumber, int personId)
        {
            return t_phone_numbers.Createt_phone_numbers(personId, localTypePhoneNumber.Number, 
                                            localTypePhoneNumber.NumberType._Id, localTypePhoneNumber._Id);
        }

        private static List<PhoneNumber> ConvertMultipleDbPhoneNumbersToLocalType(List<t_phone_numbers> dbTypePhoneNumberList)
        {
            List<PhoneNumber> localTypePhoneTypeList = new List<PhoneNumber>();

            foreach (t_phone_numbers CurrPhoneNumber in dbTypePhoneNumberList)
            {
                localTypePhoneTypeList.Add(ConvertSingleDbPhoneNumberToLocalType(CurrPhoneNumber));
            }

            return localTypePhoneTypeList;
        }

        private static PhoneNumber ConvertSingleDbPhoneNumberToLocalType(t_phone_numbers dbTypePhoneNumber)
        {
            PhoneType numberType = PhoneTypeAccess.GetPhoneTypeById(dbTypePhoneNumber.number_type);
            return new PhoneNumber(dbTypePhoneNumber.C_id, dbTypePhoneNumber.number, numberType);
        }

        #endregion
    }
}