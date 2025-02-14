﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ICSharpCode.SharpZipLib.Core;
using Microsoft.Extensions.Logging;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;
using Newtonsoft.Json;
using Noggog;

namespace SynthusMaximus.Data.Converters
{
    public abstract class GenericFormLinkConverter<T> : JsonConverter<IFormLink<T>>, IInjectedConverter, IFormLinkJsonConverter
        where T : class, IMajorRecordCommonGetter
    {
        private Dictionary<string, FormLink<T>> _links;

        protected GenericFormLinkConverter(IEnumerable<T> records, ILogger logger)
        {
            _links = records
                .Where(r => r.EditorID != null)
                .GroupBy(r => r.EditorID!)
                .ToDictionary(t => t.Key, t =>
                {
                    if (t.CountGreaterThan(1))
                    {
                        logger.LogWarning("Editor Id: {EditorID} key is ambiguous between {Options}", t.Key, string.Join(", ", t.Select(f => f.FormKey)));
                    }
                    return new FormLink<T>(t.First());
                });
        }

        public override void WriteJson(JsonWriter writer, IFormLink<T>? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override IFormLink<T> ReadJson(JsonReader reader, Type objectType, IFormLink<T>? existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var name = (string?)reader.Value;
            if (name == null)
                return new FormLink<T>();

            if (_links.TryGetValue(name, out var r))
                return r;

            throw new InvalidDataException($"Cannot find {typeof(T).Name} with Editor ID {name}");
        }
    }
}