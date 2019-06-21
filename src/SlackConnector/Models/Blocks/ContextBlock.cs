using SlackConnector.Models.Blocks.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Models.Blocks
{
	public class ContextBlock : BlockBase
	{
		public ContextBlock() : base("context")
		{
			this.Elements = new List<ElementBase>();
		}

		public IList<ElementBase> Elements { get; set; }
	}
}
