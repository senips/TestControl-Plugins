using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestControl.Help.Plugin
{
    public class BaseHelperItem : IEquatable<BaseHelperItem>, IComparable<BaseHelperItem>
    {

      
        private string _memberName;
        private string _className;
        private string _packageName;
        private string _methodName;


        public String MemberType { get; set; }

        public BaseHelperItem Parent { get; set; }

        public String MemberName
        {
            get
            {
                return _memberName;
            }
            set
            {
                var split = value.Split(':');
                MemberType = split[0];
                _memberName = split[1];

                if (MemberType == "T")
                {
                    var lastIndex = _memberName.LastIndexOf('.');
                    _packageName = _memberName.Substring(0, lastIndex);
                    _className = _memberName.Substring(lastIndex + 1, _memberName.Length - lastIndex - 1);
                }
                else if (MemberType == "M")
                {
                    var splits = _memberName.Split('(');
                    var msplits = splits[0].Split('.');
                    _packageName = "";
                    for (int i = 0; i < msplits.Length - 2; i++)
                    {
                        _packageName = _packageName + "." + msplits[i];
                    }
                    _packageName = msplits[msplits.Length - 2];
                    _className = msplits[msplits.Length - 2];
                    _methodName = msplits[msplits.Length - 1] + "(" + splits[1];
                }
                else if (MemberType == "P")
                {
                    var msplits = _memberName.Split('.');
                    _packageName = "";
                    for (int i = 0; i < msplits.Length - 2; i++)
                    {
                        _packageName = _packageName + "." + msplits[i];
                    }
                    _packageName = msplits[msplits.Length - 2];
                    _className = msplits[msplits.Length - 2];
                    _methodName = msplits[msplits.Length - 1];
                }
            }
        }
        public String Summary { get; set; }
        public String AutomationCaption { get; set; }
        public String AutomationId { get; set; }
        public String AutomationName { get; set; }
        public String WindowClassName { get; set; }



        public String PackageName
        {
            get
            {
                return _packageName;
            }
        }



        public String ClassName
        {
            get
            {
                return _className;
            }
        }

        public String MethodName
        {
            get
            {
                return _methodName;
            }
        }


        public String DisplayName
        {
            get
            {
                string dispName = string.Empty;
                if (MemberType == "T")
                {
                    return ClassName;
                }
                return MethodName;
            }
        }

        public int CompareTo(BaseHelperItem other)
        {
            if (other == null)
                return 1;

            else
                return this.ToString().CompareTo(other.ToString());
        }

        public bool Equals(BaseHelperItem other)
        {
            if (other == null) return false;
            return (this.ToString().Equals(other.ToString()));
        }

        public override string ToString()
        {
            return DisplayName;
        }

    }
}
