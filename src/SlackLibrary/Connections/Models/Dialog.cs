using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackLibrary.Connections.Models
{
	public class Dialog
	{
		public Dialog()
		{
			this.Elements = new List<DialogElement>();
		}

		[JsonProperty("callback_id")]
		public string CallbackId { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("submit_label")]
		public string SubmitLabel { get; set; }

		[JsonProperty("state")]
		public string State { get; set; }

		[JsonProperty("elements")]
		public IList<DialogElement> Elements { get; set; }
	}

	public class DialogElement
	{
		[JsonProperty("label")]
		public string Label { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("optional")]
		public bool Optional { get; set; }

		[JsonProperty("value")]
		public string Value { get; set; }

		[JsonProperty("placeholder")]
		public string Placeholder { get; set; }

	}

	public class TextAreaDialogElement : TextDialogElement
	{
		public TextAreaDialogElement()
		{
			this.Type = "textarea";
		}
	}

	public class TextDialogElement : DialogElement
	{
		public TextDialogElement()
		{
			this.Type = "text";
		}

		[JsonProperty("hint")]
		public string Hint { get; set; }

		[JsonProperty("max_length", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public int MaxLength { get; set; }

		[JsonProperty("min_length", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public int MinLength { get; set; }

		[JsonProperty("subtype")]
		public string Subtype { get; set; }
	}

	public class SelectDialogElement : DialogElement
	{
		public SelectDialogElement()
		{
			this.Type = "select";
		}

		[JsonProperty("min_query_length", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public int MinQueryLength { get; set; }

		[JsonProperty("selected_options", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public IList<SelectOptionDialogItem> SelectedOptions { get; set; }

		[JsonProperty("options", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public IList<SelectOptionDialogItem> Options { get; set; }

		[JsonProperty("option_groups", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public IList<SelectOptionGroupDialogItem> OptionGroups { get; set; }
	}

	public class SelectOptionDialogItem
	{
		[JsonProperty("value")]
		public string Value { get; set; }

		[JsonProperty("label")]
		public string Label { get; set; }
	}

	public class SelectOptionGroupDialogItem
	{
		public SelectOptionGroupDialogItem()
		{
			this.Options = new List<SelectOptionDialogItem>();
		}

		[JsonProperty("options", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public IList<SelectOptionDialogItem> Options { get; set; }

		[JsonProperty("label")]
		public string Label { get; set; }
	}
}
