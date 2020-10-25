﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Galaxy_Buds_Client.message;
using Galaxy_Buds_Client.model;

namespace Galaxy_Buds_Client.parser
{
    class GenericStringParser : BaseMessageParser
    {
        public override SPPMessage.MessageIds HandledType => SPPMessage.MessageIds.UNKNOWN_214;

        public String Content { set; get; }

        public override void ParseMessage(SPPMessage msg)
        {
            if (msg.Id != HandledType)
                return;

            Content = System.Text.Encoding.ASCII.GetString(msg.Payload);
        }

        public override Dictionary<String, String> ToStringMap()
        {
            Dictionary<String, String> map = new Dictionary<string, string>();
            PropertyInfo[] properties = this.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.Name == "HandledType" || property.Name == "ActiveModel")
                    continue;

                map.Add(property.Name, property.GetValue(this).ToString());
            }

            return map;
        }
    }
}