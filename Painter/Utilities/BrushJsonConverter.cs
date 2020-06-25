using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Windows.Media;

namespace Painter.Utilities
{
    public class BrushJsonConverter : JsonConverter<SolidColorBrush>
    {
        public override SolidColorBrush ReadJson(JsonReader reader, Type objectType, [AllowNull] SolidColorBrush existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader is JTokenReader asReader)
            {
                var definition = new { Color = "", Opacity = 1.0 };

                var storeBrush = JsonConvert.DeserializeAnonymousType((string)asReader.CurrentToken.ToString(), definition);

                //плохая идея работать с Brush а не с промежуточным объектом - нужна синхронизация с потоком ui тк SolidColorBrush это DependencyObject
                return System.Windows.Application.Current.Dispatcher.Invoke(()=> new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(storeBrush.Color), Opacity = storeBrush.Opacity });
            }

            throw new NotImplementedException($"{nameof(BrushJsonConverter)} support only {nameof(JTokenReader)}");
        }

        public override void WriteJson(JsonWriter writer, [AllowNull] SolidColorBrush value, JsonSerializer serializer)
        {
            var storeBrush = new { Color = value.Color.ToString(), Opacity = value.Opacity };

            JObject o = JObject.FromObject(storeBrush);

            o.WriteTo(writer);
        }
    }
}
