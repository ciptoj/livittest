using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AvalonUnitTesting;
using SimpleOAuth.UI;
using System.Windows;
namespace SimpleOAuth.Test
{
    [TestClass]
    public class BindingTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            AvalonTestRunner.RunInSTA(
           delegate
           {
               //MainWindow window = new MainWindow();
               //Assert.AreEqual(480, ((System.Windows.Window)window).Height);
               AvalonTestRunner.RunDataBindingTests(new MainWindow());               
           });
        }
    }
}
