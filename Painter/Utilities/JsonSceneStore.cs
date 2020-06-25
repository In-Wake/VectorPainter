using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Painter.Utilities
{
    public class JsonSceneStore
    {
        const string ID_KEY = "KEY:ID";

        const string BLOK_SEPARATOR = "@@<-END->@@";

        readonly JsonSerializerSettings settings;
        readonly JsonSerializer serializer;

        public JsonSceneStore()
        {
            settings = new JsonSerializerSettings { Formatting = Formatting.Indented, Converters = new List<JsonConverter> { new BrushJsonConverter() } };
            serializer = JsonSerializer.Create(settings);
        }

        public string Extension { get => "json"; }

        public async ValueTask WriteAsync(ShapeStoreState shapeState, StreamWriter target)
        {
            JObject rawObject = new JObject
            {
                [ID_KEY] = shapeState.Id
            };

            foreach ((string propName, object value) in shapeState.Props)
            {
                rawObject[propName] = JToken.FromObject(value, serializer);
            }

            serializer.Serialize(target, rawObject);

            target.WriteLine();
            target.WriteLine(BLOK_SEPARATOR);

            await target.FlushAsync();
        }

        public async ValueTask WriteHeaderAsync<T>(T header, StreamWriter target)
        {
            await Task.Run(() => { serializer.Serialize(target, header); target.WriteLine(); target.WriteLine(BLOK_SEPARATOR); });
            await target.FlushAsync();
        }

        public async ValueTask<T> ReadHeaderAsync<T>(TextReader source)
        {
            return await Task.Run(() =>
            {

                var json = ReadBlock(source);

                return JsonConvert.DeserializeObject<T>(json, settings);
            });
        }

        public async ValueTask<ShapeStoreState> ReadAsync(TextReader source, Dictionary<int, Dictionary<string, Type>> shapesInfo)
        {
            return await Task.Run(() =>
            {
                var json = ReadBlock(source);

                //достигнуто дно ридера
                if (string.IsNullOrWhiteSpace(json))
                {
                    return null;
                }

                JObject rawObject = JObject.Parse(json);

                var result = new ShapeStoreState
                {
                    Id = rawObject[ID_KEY].ToObject<int>(),
                };

                if (!shapesInfo.ContainsKey(result.Id))
                {
                    throw new ArgumentException($"Unknown shape id = {result.Id}");
                }

                var propsInfo = shapesInfo[result.Id];


                foreach ((string propName, Type propType) in propsInfo)
                {
                    var token = rawObject[propName];

                    if (token != null)
                    {
                        result.Props[propName] = token.ToObject(propType, serializer);
                    }
                }

                return result;
            });


        }

        static string ReadBlock(TextReader source) {
            System.Text.StringBuilder json = new System.Text.StringBuilder();
            string buffer;
            while ((buffer = source.ReadLine()) != null && buffer != BLOK_SEPARATOR)
            {
                json.AppendLine(buffer);
            }

            return json.ToString();
        }
    }



}
