using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Models.Blocks.Objects
{
	public class ConfirmObject
	{
		public ConfirmObject() : this(null, null, null, null)
		{
		}

		public ConfirmObject(string title, string text, string confirm, string deny)
		{
			this.Title = new TextObject(title, TextObjectType.PlainText);
			this.Text = new TextObject(text);
			if (!(confirm is null) && confirm.Length > 30)
				throw new ArgumentException("Confirm can't be greater than 30 char");
			this.Confirm = new TextObject(confirm);
			if (!(deny is null) && deny.Length > 30)
				throw new ArgumentException("Decny can't be greater than 30 char");
			this.Deny = new TextObject(deny);
		}

		public TextObject Title { get; set; }

		public TextObject Text { get; set; }

		public TextObject Confirm { get; set; }

		public TextObject Deny { get; set; }
	}
}
