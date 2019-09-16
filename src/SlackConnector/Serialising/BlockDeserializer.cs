using Newtonsoft.Json.Linq;
using SlackConnector.Models.Blocks;
using SlackConnector.Models.Blocks.Elements;
using SlackConnector.Models.Blocks.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Serialising
{
	public class BlockDeserializer
	{
		private ElementBase DeserializeElement(JToken json)
		{
			switch (json["type"].Value<string>())
			{
				case ButtonElement.ElementName:
					return json.ToObject<ButtonElement>();
				case ChannelsSelectElement.ElementName:
					return json.ToObject<ChannelsSelectElement>();
				case ConversationsSelectElement.ElementName:
					return json.ToObject<ConversationsSelectElement>();
				case DatePickerElement.ElementName:
					return json.ToObject<DatePickerElement>();
				case ExternalSelectElement.ElementName:
					return json.ToObject<ExternalSelectElement>();
				case ImageElement.ElementName:
					return json.ToObject<ImageElement>();
				case OverflowElement.ElementName:
					return json.ToObject<OverflowElement>();
				case StaticSelectElement.ElementName:
					return json.ToObject<StaticSelectElement>();
				case UsersSelectElement.ElementName:
					return json.ToObject<UsersSelectElement>();
			}
			return null;
		}

		private IContextElement DeserializeContextElement(JToken json)
		{
			switch (json["type"].Value<string>())
			{
				case ImageElement.ElementName:
					return json.ToObject<ImageElement>();
				default:
					return json.ToObject<TextObject>();
			}
		}

		private ActionsBlock DeserializeActionsBlock(JToken json)
		{
			var elementsCopy = json["elements"];
			json["elements"] = null;
			var block = json.ToObject<ActionsBlock>();

			var elements = elementsCopy.AsJEnumerable();
			var outputElements = new List<InteractiveElement>();
			foreach (var element in elements)
			{
				var outputElement = this.DeserializeElement(element);
				if (outputElement is InteractiveElement ie)
					outputElements.Add(ie);
			}
			block.Elements = outputElements;

			return block;
		}

		private ContextBlock DeserializeContextBlock(JToken json)
		{
			var block = new ContextBlock();
			if (json["block_id"] != null)
				block.BlockId = json["block_id"].Value<string>();

			var elements = json["elements"].AsJEnumerable();
			var outputElements = new List<IContextElement>();
			foreach (var element in elements)
			{
				var outputElement = this.DeserializeContextElement(element);
				outputElements.Add(outputElement);
			}
			block.Elements = outputElements;

			return block;
		}

		private SectionBlock DeserializeSectionBlock(JToken json)
		{
			var accessory = json["accessory"];
			json["accessory"] = null;
			var block = json.ToObject<SectionBlock>();

			if (accessory != null)
				block.Accessory = this.DeserializeElement(accessory);

			return block;
		}

		public BlockBase DeserializeBlock(JToken json)
		{
			switch (json["type"].Value<string>())
			{
				case ActionsBlock.BlockName:
					return this.DeserializeActionsBlock(json);
				case ContextBlock.BlockName:
					return this.DeserializeContextBlock(json);
				case DividerBlock.BlockName:
					return json.ToObject<DividerBlock>();
				case ImageBlock.BlockName:
					return json.ToObject<ImageBlock>();
				case SectionBlock.BlockName:
					return this.DeserializeSectionBlock(json);
			}
			return null;
		}

		public IEnumerable<BlockBase> Deserialize(string json)
		{
			var blocks = JArray.Parse(json);
			var outputBlocks = new List<BlockBase>();

			foreach (var block in blocks)
			{
				var outputBlock = this.DeserializeBlock(block);
				outputBlocks.Add(outputBlock);
			}
			return outputBlocks;
		}
	}
}
