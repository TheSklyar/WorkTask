using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Helpers.Settings
{
    internal class XmlSettings
    {
        public XmlSettings(string path)
        {
            _path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), path);
            _user = SystemInformation.UserName;
            CheckPath();
        }

        public string GetValue(string app, string param, string defvalue)
        {
            if (!File.Exists(_path))
            {
                return defvalue;
            }

            var source1 = XElement.Load(_path)
                .Elements("user")
                .Where(xElement => String.Equals(xElement.Attribute("name").Value,
                    _user,
                    StringComparison.CurrentCultureIgnoreCase));

            var xElements = source1 as IList<XElement> ?? source1.ToList();
            if (!xElements.Any())
            {
                return defvalue;
            }

            var source2 =
                xElements.Elements("app")
                    .Where(xElement => String.Equals(xElement.Attribute("name").Value,
                        app,
                        StringComparison.CurrentCultureIgnoreCase));

            var elements = source2 as IList<XElement> ?? source2.ToList();
            if (!elements.Any())
            {
                return defvalue;
            }

            var source3 =
                elements.Elements("param")
                    .Where(xElement => String.Equals(xElement.FirstAttribute.Value,
                        param,
                        StringComparison.CurrentCultureIgnoreCase))
                    .Select(xElement => xElement.LastAttribute.Value);

            var enumerable = source3 as string[] ?? source3.ToArray();

            return !enumerable.Any() ? defvalue : enumerable.First();
        }

        public void SetValue(string app, string param, string value)
        {
            if (!File.Exists(_path))
            {
                new XElement("configuration",
                    new XElement("user",
                        new XAttribute("name", _user),
                        new XElement("app",
                            new XAttribute("name", app),
                            (object)new XElement("param",
                                new XAttribute("name", param),
                                (object)new XAttribute("value", value))))).Save(_path);
            }
            else
            {
                var xelement = XElement.Load(_path);
                var source1 =
                    xelement.Elements("user")
                        .Where(
                            user =>
                                String.Equals(user.Attribute("name").Value,
                                    _user,
                                    StringComparison.CurrentCultureIgnoreCase));

                var xElements = source1 as XElement[] ?? source1.ToArray();
                if (!xElements.Any())
                {
                    xelement.Add(new XElement("user",
                        new XAttribute("name", _user),
                        (object)new XElement("app",
                            new XAttribute("name", app),
                            (object)new XElement("param",
                                new XAttribute("name", param),
                                new XAttribute("value", value)))));

                    xelement.Save(_path);
                }
                else
                {
                    var source2 =
                        xElements.Elements("app").Where(xElement =>
                            String.Equals(xElement.Attribute("name").Value,
                                app,
                                StringComparison.CurrentCultureIgnoreCase));

                    var enumerable = source2 as IList<XElement> ?? source2.ToList();
                    if (!enumerable.Any())
                    {
                        xElements.First()
                            .Add(new XElement("app",
                                new XAttribute("name", app),
                                (object)
                                    new XElement("param",
                                        new XAttribute("name", param),
                                        (object)new XAttribute("value", value))));

                        xelement.Save(_path);
                    }
                    else
                    {
                        var source3 =
                            enumerable
                                .Elements("param")
                                .Where(xElement => string.Equals(xElement.FirstAttribute.Value,
                                    param,
                                    StringComparison.CurrentCultureIgnoreCase))
                                .Select(xElement => xElement.LastAttribute);

                        var xAttributes = source3 as IList<XAttribute> ?? source3.ToList();
                        if (!xAttributes.Any())
                        {
                            enumerable.First().Add(new XElement("param",
                                new XAttribute("name", param),
                                (object)new XAttribute("value", value)));

                            xelement.Save(_path);
                        }
                        else
                        {
                            xAttributes.First().Value = value;
                            xelement.Save(_path);
                        }
                    }
                }
            }
        }

        private void CheckPath()
        {
            try
            {
                var directoryName = Path.GetDirectoryName(_path);
                if (directoryName != null && Directory.Exists(directoryName))
                {
                    return;
                }
                if (directoryName != null)
                {
                    Directory.CreateDirectory(directoryName);
                }
            }
            catch (Exception)
            {
                throw new ArgumentException(@"Ошибка проверки существования пути", _path);
            }
        }

        private readonly string _path;
        private readonly string _user;
    }
}
