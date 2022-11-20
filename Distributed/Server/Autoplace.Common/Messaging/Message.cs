using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoplace.Common.Messaging
{
    public class Message
    {
        public string serializedData;

        public Message(object data)
            => Data = data;

        public Message(object data, bool published)
        {
            Data = data;
            Published = published;
        }

        private Message()
        {
        }

        public int Id { get; private set; }

        public Type Type { get; private set; }

        public bool Published { get; private set; }

        public void MarkAsPublished() => Published = true;

        [NotMapped]
        public object Data
        {
            get => JsonConvert.DeserializeObject(serializedData, Type,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            set
            {
                Type = value.GetType();

                serializedData = JsonConvert.SerializeObject(value,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
        }
    }
}
