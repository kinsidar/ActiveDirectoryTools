﻿#region NameSpaces
using System;
using System.DirectoryServices;
#endregion

namespace ADT04RootDSE
{
    class Program
    {
        static string dnsHostName = "";
        static string theDnsNameRootDSE = "";

        // Ref: http://stackoverflow.com/questions/4015407/determine-current-domain-controller-programmatically
        public static string RetrieveRootDseDefaultNamingContext()
        {
            String RootDsePath = "LDAP://RootDSE";
            const string DefaultNamingContextPropertyName = "defaultNamingContext";

            DirectoryEntry rootDse = new DirectoryEntry(RootDsePath)
            {
                AuthenticationType = AuthenticationTypes.Secure
            };

            dnsHostName = (string)rootDse.Properties["dnsHostName"].Value;
            //Console.WriteLine("dnsHostName = " + dnsHostName);

            dnsHostName = dnsHostName + "|";
            object combinedPropertyValue = dnsHostName + rootDse.Properties[DefaultNamingContextPropertyName].Value;

            return combinedPropertyValue != null ? combinedPropertyValue.ToString() : null;
        }

        #region Main
        static void Main(string[] args)
        {
            theDnsNameRootDSE = RetrieveRootDseDefaultNamingContext();
            Console.WriteLine("The dnsHostName|RootDSE is " + theDnsNameRootDSE);

            // Ref: http://www.codeproject.com/Articles/667301/How-to-query-Active-Directory-without-hard-coding
            using (DirectoryEntry rootDSE = new DirectoryEntry("LDAP://RootDSE"))
            {
                Console.WriteLine("\r\nAttributes with a single value:");
                foreach (string propertyName in rootDSE.Properties.PropertyNames)
                {
                    if (rootDSE.Properties[propertyName].Count == 1)
                    {
                        Console.WriteLine("{0,30} = {1}", propertyName,
                            rootDSE.Properties[propertyName].Value);
                        continue;
                    }
                }
                Console.WriteLine("\r\nAttributes with multiple values:");
                foreach (string propertyName in rootDSE.Properties.PropertyNames)
                {
                    if (rootDSE.Properties[propertyName].Count > 1)
                    {
                        Console.WriteLine("    {0}:", propertyName);
                        foreach (object obj in (object[])(rootDSE.Properties[propertyName].Value))
                        {
                            Console.WriteLine("        {0}", obj.ToString());
                        }
                    }
                }
            }
            Console.WriteLine();
            Console.WriteLine("******************Exit the Program*******************");
            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
        }
        #endregion
    }
}
