using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using GreenSQL.Domain.Model;

namespace GreenSQL.Infrastructure.Configuration
{
    public sealed class ImageSettingsSection : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            var images = new List<Image>();
            
            string id = string.Empty;
            string firstName = string.Empty;
            string lastName = string.Empty;
            foreach (XmlNode childNode in section.ChildNodes)
            {
                if (null != childNode.Attributes["id"]) {
                    id = childNode.Attributes["id"].Value;
                    images.Add(new Image(id) {
                        Name = childNode.Attributes["name"]?.Value,
                        Edition = childNode.Attributes["edition"]?.Value,
                        Version = childNode.Attributes["version"]?.Value,
                        Source = childNode.Attributes["source"]?.Value,
                    });
                }
            }
            return images;
        }
    }
}
