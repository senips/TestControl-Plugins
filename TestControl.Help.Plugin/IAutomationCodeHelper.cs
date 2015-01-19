using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestControl.Net.Interfaces;

namespace TestControl.Help.Plugin
{
    public interface IAutomationCodeHelper
    {
        void LoadHelp();
        IList<BaseHelperItem> HelperItems { get; }
        IList<BaseHelperItem> ClassHelperItems { get; }
        IList<BaseHelperItem> GetClassMemberHelperItems(BaseHelperItem classItem);
        BaseHelperItem FindClass(ControlProperties controlProperties);
        BaseHelperItem FindMember(ControlProperties controlProperties);

    }
}
