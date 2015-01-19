using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestControl.Net.Interfaces;

namespace TestControl.Help.Plugin
{
    public class HelpPlugin : ITestControlSelection
    {
        const string CODE_HELPER = "Code Help";

        private AutomationCodeHelper _codeHelper;

        
        public IList<string> CustomMenuItems
        {
            get { return new String[] { CODE_HELPER }; }
        }

        public string GetHandleInfo(IntPtr handle)
        {
            return "Status: Info";
        }

        HelperForm helpForm = new HelperForm();
        public void OnCustomMenuItemClick(string menuItem, ControlProperties controlProperties, UpdateHistory updateBack)
        {
            if (menuItem == CODE_HELPER)
            {
                
                helpForm.CodeHelper = GetCodeHelper();
                helpForm.ControlProperties = controlProperties;
                helpForm.Show();
  
            }
        }

        private AutomationCodeHelper GetCodeHelper()
        {
            if (_codeHelper == null)
            {
                _codeHelper = new AutomationCodeHelper();
                _codeHelper.LoadHelp();
            }
            return _codeHelper;
        }
    }
}
