using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SpadStorePanel.Core.WebService.Model
{
	[XmlRoot(ElementName = "ProductDATA", Namespace = "http://tempuri.org/")]
	public class ProductDATA
	{
		[XmlElement(ElementName = "BaseUOMName", Namespace = "http://tempuri.org/")]
		public string BaseUOMName { get; set; }
		[XmlElement(ElementName = "ProductId", Namespace = "http://tempuri.org/")]
		public string ProductId { get; set; }
		[XmlElement(ElementName = "AnbarId", Namespace = "http://tempuri.org/")]
		public string AnbarId { get; set; }
		[XmlElement(ElementName = "ProductCode", Namespace = "http://tempuri.org/")]
		public string ProductCode { get; set; }
		[XmlElement(ElementName = "ProductName", Namespace = "http://tempuri.org/")]
		public string ProductName { get; set; }
		[XmlElement(ElementName = "PercentDiscount", Namespace = "http://tempuri.org/")]
		public string PercentDiscount { get; set; }
		[XmlElement(ElementName = "ProductFanniCode", Namespace = "http://tempuri.org/")]
		public string ProductFanniCode { get; set; }
		[XmlElement(ElementName = "LastSalePrice", Namespace = "http://tempuri.org/")]
		public string LastSalePrice { get; set; }
		[XmlElement(ElementName = "AvePurchasePrice", Namespace = "http://tempuri.org/")]
		public string AvePurchasePrice { get; set; }
		[XmlElement(ElementName = "PurchasePrice", Namespace = "http://tempuri.org/")]
		public string PurchasePrice { get; set; }
		[XmlElement(ElementName = "SalePrice", Namespace = "http://tempuri.org/")]
		public string SalePrice { get; set; }
		[XmlElement(ElementName = "IsActive", Namespace = "http://tempuri.org/")]
		public string IsActive { get; set; }
		[XmlElement(ElementName = "Model", Namespace = "http://tempuri.org/")]
		public string Model { get; set; }
		[XmlElement(ElementName = "UOMId", Namespace = "http://tempuri.org/")]
		public string UOMId { get; set; }
		[XmlElement(ElementName = "P_Country", Namespace = "http://tempuri.org/")]
		public string P_Country { get; set; }
		[XmlElement(ElementName = "Description", Namespace = "http://tempuri.org/")]
		public string Description { get; set; }
		[XmlElement(ElementName = "Include_MaliatAvarez", Namespace = "http://tempuri.org/")]
		public string Include_MaliatAvarez { get; set; }
		[XmlElement(ElementName = "Maliat", Namespace = "http://tempuri.org/")]
		public string Maliat { get; set; }
		[XmlElement(ElementName = "Avarez", Namespace = "http://tempuri.org/")]
		public string Avarez { get; set; }
		[XmlElement(ElementName = "Exist", Namespace = "http://tempuri.org/")]
		public string Exist { get; set; }

		public string Rate { get; set; }

		public string ShortDescription { get; set; }
	}
}
