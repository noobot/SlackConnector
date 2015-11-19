namespace SlackConnector.Connections.Sockets.Messages.Inbound
{
    internal enum MessageSubType
    {
        Unknown = 0,
        bot_message,
        me_message,
        message_changed,
        message_deleted,
        channel_join,
        channel_leave,
        channel_topic,
        channel_purpose,
        channel_name,
        channel_archive,
        channel_unarchive,
        group_join,
        group_leave,
        group_topic,
        group_purpose,
        group_name,
        group_archive,
        group_unarchive,
        file_share,
        file_comment,
        file_mention,
        pinned_item,
        unpinned_item
    }
}