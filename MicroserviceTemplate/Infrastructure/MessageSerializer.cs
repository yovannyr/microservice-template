
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Infrastructure
{
    public class MessageSerializer
    {
        public byte[] Serialize(string msg)
        {
            return Encoding.UTF8.GetBytes(msg);
        }


        public string Deserialize(byte[] msgBody)
        {
            return Encoding.UTF8.GetString(msgBody);
        }

        public byte[] Serialize<T>(T msg)
        {
            var msgStr = JsonConvert.SerializeObject(msg,new JsonSerializerSettings
                                                {
                                                    Formatting = Formatting.Indented,
                                                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                                                });

            return Encoding.UTF8.GetBytes(msgStr);
        }

        public T Deserialize<T>(byte[] msgBody)
        {
            var msgStr = Encoding.UTF8.GetString(msgBody);
          
            var msg = JsonConvert.DeserializeObject<T>(msgStr, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });


            return msg;
        }



    }
}
