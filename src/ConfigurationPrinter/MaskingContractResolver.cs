using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Nomailme.ConfigurationPrinter
{
    public class MaskingContractResolver : DefaultContractResolver
    {
        private readonly List<string> maskedPropertiesList;

        public MaskingContractResolver(List<string> maskedPropertiesList)
        {
            if (maskedPropertiesList == null)
            {
                throw new ArgumentNullException("maskedPropertiesList");
            }

            this.maskedPropertiesList = maskedPropertiesList;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> props = base.CreateProperties(type, memberSerialization);

            // Find all string properties that have a [JsonEncrypt] attribute applied
            // and attach an MaskedStringValueProvider instance to them
            foreach (JsonProperty prop in props.Where(p => maskedPropertiesList.Contains(p.PropertyName)))
            {
                PropertyInfo pi = type.GetProperty(prop.UnderlyingName);
                prop.ValueProvider = new MaskedStringValueProvider();
            }

            return props;
        }
    }

    internal class MaskedStringValueProvider : IValueProvider
    {
        private const string stringMask = "*****";

        public MaskedStringValueProvider()
        {
        }

        // GetValue is called by Json.Net during serialization.
        // The target parameter has the object from which to read the unencrypted string;
        // the return value is an encrypted string that gets written to the JSON
        public object GetValue(object target)
        {
            return stringMask;
        }

        // SetValue gets called by Json.Net during deserialization.
        // The value parameter has the encrypted value read from the JSON;
        // target is the object on which to set the decrypted value.
        public void SetValue(object target, object value)
        {
            throw new InvalidOperationException();
        }
    }
}


