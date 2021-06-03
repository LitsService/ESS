using ESS_Web_Application.Entity;
using ESS_Web_Application.Models;
using ESS_Web_Application.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESS_Web_Application.Services
{
    public class TestService:ITestService
    {
        DBContext dbcontext = new DBContext();
        //private readonly IRepository<Test> _testRepository;
        //public TestService(IRepository<Test> testRepository)
        //{
        //    this._testRepository = testRepository;
        //}

        public void CreateTest(Test test)
        {

            throw new NotImplementedException();
        }

        public Test GetGadget(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Test> GetTest()
        {
            throw new NotImplementedException();
        }
    }
}