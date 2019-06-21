using SlackConnector.Models.Blocks.Elements;
using SlackConnector.Models.Blocks.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Models.Blocks
{
	public class ActionsBlock : BlockBase
	{
		public ActionsBlock() : base("actions")
		{
			this.Elements = new List<InteractiveElement>();
		}

		public IList<InteractiveElement> Elements { get; set; }
	}
}
