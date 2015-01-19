using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TestControl.Net.Interfaces;

namespace TestControl.Help.Plugin
{
    public partial class HelperForm : Form
    {
       
        public HelperForm()
        {
            InitializeComponent();                     
        }


        private IAutomationCodeHelper _codeHelper;
        public IAutomationCodeHelper CodeHelper
        {
            get
            {
                return _codeHelper;
            }
            set
            {
                _codeHelper = value;
                classList.DataSource = _codeHelper.ClassHelperItems;
            }
        }

        private ControlProperties _controlProperties;
        public ControlProperties ControlProperties
        {
            get
            {
                return _controlProperties;
            }
            set
            {               
                _controlProperties = value;
                var classItem = _codeHelper.FindClass(value);
                if (classItem != null)
                {
                    classList.SelectedItem = classItem;
                    methodList.DataSource = _codeHelper.GetClassMemberHelperItems(classItem);


                    //methodList.SelectedItem 
                }
                else
                {                 
                    var memberitem = _codeHelper.FindMember(value);
                    if (memberitem != null)
                    {
                         classList.SelectedItem = memberitem.Parent;
                         methodList.DataSource = _codeHelper.GetClassMemberHelperItems(memberitem.Parent);
                        methodList.SelectedItem = memberitem;
                    }
                 
                }
                //var helpText =_codeHelper.GetAutomationHelp(value.Caption);
                //codeSnippet.AppendText(helpText);
            }
        }

        private void classList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (classList.SelectedItem != null)
            {
                methodList.DataSource = _codeHelper.GetClassMemberHelperItems( ((BaseHelperItem)classList.SelectedItem) );
            }
        }

        private void HelperForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

      

    }
}
