using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using TestControl.Net.Interfaces;

namespace TestControl.Help.Plugin
{
    public class AutomationCodeHelper : IAutomationCodeHelper
    {
        private readonly IList<BaseHelperItem> _codeHelperItems = new List<BaseHelperItem>();
        private bool _loaded;

        private static XDocument GetHelpDoc(string helpXml)
        {
            if (File.Exists(helpXml))
            {
                return XDocument.Load(helpXml);
            }
            return null;
        }

        public BaseHelperItem FindClass(ControlProperties controlProperties)
        {
            return FindClassInternal(ClassHelperItems, controlProperties);
        }

        public BaseHelperItem FindMember(ControlProperties controlProperties)
        {
            var helpeItems = _codeHelperItems.Where(x => ((x.MemberType == "M") || (x.MemberType == "P"))).ToList();
            return FindClassInternal(helpeItems, controlProperties);
        }

        public BaseHelperItem FindClassInternal(IList<BaseHelperItem> items, ControlProperties controlProperties)
        {
            var classitem = items.FirstOrDefault(x => (!String.IsNullOrEmpty(controlProperties.AutomationId)) && (x.AutomationId == controlProperties.AutomationId));
            if (classitem == null)
            {
                classitem = items.FirstOrDefault(x => (!String.IsNullOrEmpty(controlProperties.Caption)) && (x.AutomationCaption == controlProperties.Caption));
            }
            if (classitem == null)
            {
                classitem = items.FirstOrDefault(x => (!String.IsNullOrEmpty(controlProperties.Name)) && (x.AutomationName == controlProperties.Name));
                if (classitem == null)
                {
                    classitem = items.FirstOrDefault(x => (!String.IsNullOrEmpty(controlProperties.Name)) && (x.AutomationName.Split(',').Contains(controlProperties.Name)));
                }
            }
            if (classitem == null)
            {
                classitem = items.FirstOrDefault(x => (!String.IsNullOrEmpty(controlProperties.ClassName)) && (x.WindowClassName == controlProperties.ClassName));
            }
            return classitem;
        }

        public void LoadHelp()
        {
            if (_loaded)
            {
                return;
            }
            _loaded = true;
            foreach (KeyValueConfigurationElement helpFileItem in GetConfig().AppSettings.Settings)
            {
                var helpDoc = GetHelpDoc(helpFileItem.Value);
                BaseHelperItem parentItem = null;
                foreach (var item in helpDoc.Descendants("member"))
                {
                    var helperItem = new BaseHelperItem();
                    helperItem.MemberName = item.Attribute("name").Value;
                    helperItem.Summary = GetElementValue(item.Element("summary"));
                    helperItem.AutomationCaption = GetElementValue(item.Element("automation-caption"));
                    helperItem.AutomationId = GetElementValue(item.Element("automation-id"));
                    helperItem.WindowClassName = GetElementValue(item.Element("automation-class"));
                    helperItem.AutomationName = GetElementValue(item.Element("automation-name"));
                    if (helperItem.MemberType == "T")
                    {
                        parentItem = helperItem;
                    }
                    else
                    {
                        helperItem.Parent = parentItem;
                    }
                    _codeHelperItems.Add(helperItem);
                }
            }
        }

        private String GetElementValue(XElement element)
        {
            return (element == null) ? string.Empty : element.Value;
        }

        private Configuration GetConfig()
        {
            Configuration appConfig;
            try
            {
                appConfig = ConfigurationManager.OpenExeConfiguration(ConfigFile);
            }
            catch
            {
                appConfig = null;
            }
            return appConfig;
        }


        private string ConfigFile
        {
            get { return "TestControl.Help.Plugin.dll"; }
        }


        public IList<BaseHelperItem> ClassHelperItems
        {
            get
            {
                return _codeHelperItems.Where(x => x.MemberType == "T").ToList();
            }
        }

        public IList<BaseHelperItem> HelperItems
        {
            get
            {
                return _codeHelperItems;
            }
        }


        public IList<BaseHelperItem> GetClassMemberHelperItems(BaseHelperItem classItem)
        {
            return _codeHelperItems.Where(x => ((x.MemberType == "M") || (x.MemberType == "P")) && (x.MemberName.StartsWith(classItem.MemberName))).ToList();
        }
    }
}
