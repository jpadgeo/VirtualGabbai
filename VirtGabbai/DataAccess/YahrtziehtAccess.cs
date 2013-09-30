﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataTypes;
using DataCache;

namespace DataAccess
{
    public class YahrtziehtAccess
    {

        #region Read Methods

        public Yahrtzieht GetSpecificYahrtzieht(long personId, DateTime date, string personName)
        {
            t_yahrtziehts requestedYahrtzieht = (from test in Cache.CacheData.t_yahrtziehts
                                                 where test.person_id == personId &&
                                                       test.date == date &&
                                                       test.deceaseds_name == personName
                                                 select test).First();

            return this.ConverSingleYahrtziehtToLocalType(requestedYahrtzieht);
        }

        public List<Yahrtzieht> GetYahrtziehtsByDate(long personId, DateTime date)
        {
            List<t_yahrtziehts> requestedYahrtzieht = (from test in Cache.CacheData.t_yahrtziehts
                                                       where test.date == date
                                                       select test).ToList<t_yahrtziehts>();

            return this.ConvertMultipleYahrtziehtToLocalType(requestedYahrtzieht);
        }

        public List<Yahrtzieht> GetAllYahrtziehts(long personId)
        {
            t_people test = (t_people)Cache.CacheData.t_people.Take(1);

            //List<t_yarthziehts> AllYarhtziets = (from CurrentYarhtziet  in Cache.CacheData.t_yarthziehts
            //                                     where CurrentYarhtziet.person_id == personId
            //                                     select CurrentYarhtziet).ToList<t_yarthziehts>();

            return this.ConvertMultipleYahrtziehtToLocalType(test.t_yahrtziehts.ToList<t_yahrtziehts>());
        }

        private t_yahrtziehts LookupSpecificYahrtzieht(long personId, DateTime date, string personName)
        {
            return (from test in Cache.CacheData.t_yahrtziehts
                    where test.person_id == personId &&
                        test.date == date &&
                        test.deceaseds_name == personName
                    select test).First();
        }

        private List<t_yahrtziehts> LookupYahrtziehtsByDate(long personId, DateTime date)
        {
            return (from test in Cache.CacheData.t_yahrtziehts
                    where test.date == date
                    select test).ToList<t_yahrtziehts>();
        }

        private List<t_yahrtziehts> LookupAllYahrtziehts(long personId)
        {
            return ((t_people)Cache.CacheData.t_people.Take(1)).t_yahrtziehts.ToList<t_yahrtziehts>();
        }

        private t_yahrtziehts LookupYahrtziehtById(long ID)
        {
            return (from test in Cache.CacheData.t_yahrtziehts
                    where test.C_id == ID
                    select test).First();
        }

        #endregion

        #region Write

        #region Create

        public void AddNewYahrtzieht(Yahrtzieht ya)
        {
            t_yahrtziehts yaToAdd = this.ConvertSingleYahrtziehtToDbType(ya);
            Cache.CacheData.t_yahrtziehts.AddObject(yaToAdd);
            Cache.CacheData.SaveChanges();
        }

        public void AddMultipleNewYahrtzieht(List<Yahrtzieht> myYaList)
        {
            foreach (var item in myYaList)
            {
                this.AddNewYahrtzieht(item);
            }
        }

        #endregion

        #region Update

        public void UpdateSingleYahrtzieht(Yahrtzieht ya)
        {
            /*
             * 1) LookUpYahrtziehtById
             * 2) convert the parameter passed in to the entity type
             * 3) make the reference from step 1 equal to the result from step 2
             * TRIAL
             * 4) attach
             * 5) SaveChanges
             * OR----
             * 4) SaveChanges
             * */
            t_yahrtziehts instanceUpdating = this.LookupYahrtziehtById(ya._Id);
            Cache.CacheData.t_yahrtziehts.Attach(instanceUpdating);
            Cache.CacheData.SaveChanges();
            //TODO lookup how to do updating when using entity framework
            //db.users.attach(updateduser)
            // db.savechanges()
            // db.Entry(original).currentvalues.setvalues(updatedUser) -after first loading origional
        }

        public void UpdateMultipleYahrtzieht(List<Yahrtzieht> myYaList)
        {
            foreach (var item in myYaList)
            {
                this.UpdateSingleYahrtzieht(item);
            }
        }

        #endregion

        #region Delete

        public void DeleteSingleYahrtzieht(Yahrtzieht ya)
        {
            t_yahrtziehts test = Cache.CacheData.t_yahrtziehts.First(person => person.C_id == ya._Id);
            Cache.CacheData.t_yahrtziehts.DeleteObject(test);
            Cache.CacheData.SaveChanges();
        }

        public void DeleteMultipleYahrtzieht(List<Yahrtzieht> myYaList)
        {
            foreach (var item in myYaList)
            {
                this.DeleteSingleYahrtzieht(item);
            }
        }

        #endregion

        #endregion

        #region Private Methods

        private List<t_yahrtziehts> ConvertMultipleYahrtziehtToDbType(List<Yahrtzieht> myYaList)
        {
            List<t_yahrtziehts> myList = new List<t_yahrtziehts>();

            foreach (var item in myYaList)
            {
                myList.Add(this.ConvertSingleYahrtziehtToDbType(item));
            }

            return myList;
        }

        private t_yahrtziehts ConvertSingleYahrtziehtToDbType(Yahrtzieht ya)
        {
            var test = t_yahrtziehts.Createt_yahrtziehts(ya._Id, ya.PersonId, ya.Date, ya.Name);
            test.relation = ya.Relation;
            return test;
        }

        private List<Yahrtzieht> ConvertMultipleYahrtziehtToLocalType(List<t_yahrtziehts> ya)
        {
            List<Yahrtzieht> myList = new List<Yahrtzieht>();

            foreach (var item in ya)
            {
                myList.Add(this.ConverSingleYahrtziehtToLocalType(item));
            }

            return myList;
        }

        private Yahrtzieht ConverSingleYahrtziehtToLocalType(t_yahrtziehts ya)
        {
            Yahrtzieht instance = new Yahrtzieht();
            instance._Id = ya.C_id;
            instance.Date = ya.date;
            instance.Relation = ya.relation;
            instance.Name = ya.deceaseds_name;
            instance.PersonId = ya.person_id;

            return instance;
        } 

        #endregion
    }
}
