using ESS_Web_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESS_Web_Application.Services
{
    public interface ITestService
    {
        IEnumerable<Test> GetTest();
        Test GetGadget(int id);
        void CreateTest(Test test);
    }
}