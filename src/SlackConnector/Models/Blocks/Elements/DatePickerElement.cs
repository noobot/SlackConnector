﻿using Newtonsoft.Json;
using SlackConnector.Models.Blocks.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Models.Blocks.Elements
{
	public class DatePickerElement : InteractiveElement
	{
		public DatePickerElement(string actionId, string placeholder) : base(actionId, "datepicker")
		{
			this.Placeholder = new TextObject(placeholder, TextObjectType.PlainText);
		}

		public TextObject Placeholder { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public ConfirmObject Confirm { get; set; }

		[JsonProperty(PropertyName = "initial_date", NullValueHandling = NullValueHandling.Ignore)]
		public string InitialDate { get; set; }
	}
}
