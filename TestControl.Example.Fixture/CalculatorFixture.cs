using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestControl.Net;
using TestControl.Net.Interfaces;
using TestControl.Net.Locators;
using TestControl.Net.StdControls;

namespace TestControl.Example.Fixture
{
    /// <summary>
    /// Calc.Exe is from XP edition. 
    /// Fixture for automating calculator
    ///
    /// </summary>
    /// <automation-caption>Calculator</automation-caption>
    public class CalculatorFixture
    {

        private ButtonControl _aButtonControl = new ButtonControl();
        private TextBoxControl _aResultTextControl = new TextBoxControl();

        public CalculatorFixture()
        {
            //main app locator
            var locator = new ControlLocatorDefRepo("calc-app");
            locator.FindByName("Calculator", true);

            //locator for reading calculator result
            locator = new ControlLocatorDefRepo("result");
            locator.FindUsing("calc-app");
            locator.FindByAutomationId("403");
            _aResultTextControl.SystemUnderTestFromRepo("result");


            //locators for cacluator button 
            String[] CALCULATOR_BUTTON_NAMES = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "=", "+", "-", "/", "*" };
            foreach (var buttonName in CALCULATOR_BUTTON_NAMES)
            {
                var locatorName = buttonName;
                locator = new ControlLocatorDefRepo(locatorName);
                locator.FindUsing("calc-app");
                locator.FindByName(buttonName);
            }
        }

        /// <summary>
        ///  Enter a number into the calculator
        /// </summary>        
        /// <automation-name>0, 1, 2, 3,4 5,6,7,8,9</automation-name>
        /// <param name="aNumber"></param>
        public void EnterValue(String aNumber)
        {
            foreach (char ch in aNumber)
            {
                _aButtonControl.SystemUnderTestFromRepo(ch.ToString());
                _aButtonControl.Click();
                _aButtonControl.Wait(0, 50);
            }
        }

        /// <summary>
        /// select the operation to be performed on the calculator
        /// </summary>
        /// <remarks> </remarks>
        /// <automation-name>+,-,*,/</automation-name>
        /// <param name="opr"></param>
        public void Operation(String opr)
        {
            _aButtonControl.SystemUnderTestFromRepo(opr);
            _aButtonControl.Wait(0, 50);
            _aButtonControl.Click();
        }


        /// <summary>
        /// get the result value from the calculator        
        /// </summary>
        /// <automation-id>403</automation-id>
        public String Result
        {
            get
            {

                _aButtonControl.SystemUnderTestFromRepo("=");
                _aButtonControl.Wait(0, 20);
                _aButtonControl.Click();

                return _aResultTextControl.Text.Trim();
            }
        }
    }
}
