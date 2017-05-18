using Microsoft.AspNetCore.DataProtection.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SessionTest
{
    public class CustomXmlRepository : IXmlRepository
    {
        private readonly string keyContent =
                                        @"<?xml version='1.0' encoding='utf-8'?>
                                        <key id = '55ae42f5-63fb-4ea0-946a-451c5cff4dd3' version='1'>
                                          <creationDate>2017-05-17T08:30:20.5565108Z</creationDate>
                                          <activationDate>2017-05-17T08:30:20.5055409Z</activationDate>
                                          <expirationDate>2017-08-15T08:30:20.5055409Z</expirationDate>
                                          <descriptor deserializerType = 'Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel.AuthenticatedEncryptorDescriptorDeserializer, Microsoft.AspNetCore.DataProtection, Version=1.1.2.0, Culture=neutral, PublicKeyToken=adb9793829ddae60' >
                                            <descriptor>
                                              <encryption algorithm='AES_256_CBC' />
                                              <validation algorithm = 'HMACSHA256' />
                                              <masterKey p4:requiresEncryption='true' xmlns:p4='http://schemas.asp.net/2015/03/dataProtection'>
                                                <!-- Warning: the key below is in an unencrypted form. -->
                                                <value>rUxa8Mpyld7AQh7ugkFMYVq+IUPH42E6nQrRTqI1Q2G8SsNZy8rQkq+4/saWlOwgzFK3S81QH2fJcHupyyMF6Q==</value>
                                              </masterKey>
                                            </descriptor>
                                          </descriptor>
                                        </key>";

        public virtual IReadOnlyCollection<XElement> GetAllElements()
        {
            return GetAllElementsCore().ToList().AsReadOnly();
        }

        private IEnumerable<XElement> GetAllElementsCore()
        {
            yield return XElement.Parse(keyContent);
        }
        public virtual void StoreElement(XElement element, string friendlyName)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }
            StoreElementCore(element, friendlyName);
        }

        private void StoreElementCore(XElement element, string filename)
        {
        }
    }
}
