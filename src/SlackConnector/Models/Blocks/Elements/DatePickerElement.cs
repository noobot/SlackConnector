using Newtonsoft.Json;
using SlackConnector.Models.Blocks.Objects;
using SlackConnector.Serialising;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Models.Blocks.Elements
{
	public class DatePickerElement : InteractiveElement
	{
		public const string ElementName = "datepicker";
		public DatePickerElement(string actionId, string placeholder) : base(actionId, ElementName)
		{
			this.Placeholder = new TextObject(placeholder, TextObjectType.PlainText);
		}

		[JsonProperty(PropertyName = "placeholder")]
		public TextObject Placeholder { get; set; }

		[JsonProperty(
			PropertyName = "initial_date", 
			NullValueHandling = NullValueHandling.Ignore,
			ItemConverterType = typeof(DateFormatConverter),
			ItemConverterParameters = new[] { "yyyy-MM-dd" })]
		public DateTime InitialDate { get; set; }
	}
}
