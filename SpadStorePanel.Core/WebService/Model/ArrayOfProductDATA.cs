using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SpadStorePanel.Core.WebService.Model
{
	[XmlRoot(ElementName = "ArrayOfProductDATA", Namespace = "http://tempuri.org/")]
	public class ArrayOfProductDATA
	{
		[XmlElement(ElementName = "ProductDATA", Namespace = "http://tempuri.org/")]
		public List<ProductDATA> ProductDATA { get; set; }
		[XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string Xsi { get; set; }
		[XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string Xsd { get; set; }
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
	}
}
