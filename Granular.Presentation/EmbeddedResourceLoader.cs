using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Markup;
using System.Resources;
using Granular.Extensions;
using Granular.Collections;
using System.IO;

namespace System.Windows
{
    public static class EmbeddedResourceLoader
    {
        private class UriEqualityComparer : IEqualityComparer<Uri>
        {
            public static readonly UriEqualityComparer Default = new UriEqualityComparer();

            private UriEqualityComparer()
            {
                //
            }

            public bool Equals(Uri x, Uri y)
            {
                return Object.Equals(x.GetAbsoluteUri(), y.GetAbsoluteUri());
            }

            public int GetHashCode(Uri obj)
            {
                return obj.GetAbsoluteUri().GetHashCode();
            }
        }

        private static readonly Granular.Compatibility.Regex ResourceUriRegex = new Granular.Compatibility.Regex("/([^;]*);component/(.*)");
        private const int ResourceUriAssemblyNameGroupIndex = 1;
        private const int ResourceUriPathGroupIndex = 2;

        private static readonly CacheDictionary<Assembly, ResourceSet> resourceSetCache = CacheDictionary<Assembly, ResourceSet>.CreateUsingStringKeys(TryResolveResourceSet, assembly => assembly.FullName);
        private static readonly CacheDictionary<Uri, byte[]> resourceDataCache = CacheDictionary<Uri, byte[]>.CreateUsingStringKeys(ResolveResourceData, uri => uri.GetAbsoluteUri());
        private static readonly CacheDictionary<Uri, object> resourceElementCache = CacheDictionary<Uri, object>.CreateUsingStringKeys(ResolveResourceElement, uri => uri.GetAbsoluteUri());

        public static byte[] LoadResourceData(Uri resourceUri)
        {
            VerifyResourceUri(resourceUri);

            return resourceDataCache.GetValue(resourceUri);
        }

        private static byte[] ResolveResourceData(Uri resourceUri)
        {
            string assemblyName;
            string resourcePath;
            if (!TryParseAbsolutePath(resourceUri.GetAbsolutePath(), out assemblyName, out resourcePath))
            {
                throw new Granular.Exception("Resource \"{0}\" absolute path is invalid", resourceUri.GetAbsoluteUri());
            }

            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.GetName().Name == assemblyName).FirstOrDefault();

            if (assembly == null)
            {
                assembly = Assembly.Load(assemblyName);
            }

            byte[] resourceData;
            if (!TryResolveEmbeddedResourceData(assembly, assemblyName, resourcePath, out resourceData) &&
                !TryResolveResourceData(assembly, resourcePath, out resourceData))
            {
                throw new Granular.Exception("Resource \"{0}\" was not found", resourceUri.GetAbsoluteUri());
            }

            return resourceData;
        }

        private static bool TryResolveEmbeddedResourceData(Assembly assembly, string assemblyName, string resourcePath, out byte[] resourceData)
        {
            string embeddedResourceName = String.Format("{0}.{1}", assemblyName, resourcePath.TrimStart('/').Replace('/', '.'));

            resourceData = assembly?.GetManifestResourceData(embeddedResourceName);
            return resourceData != null;
        }

        private static bool TryResolveResourceData(Assembly assembly, string resourcePath, out byte[] resourceData)
        {
            resourceData = null;

            ResourceSet resourceSet;
            if (!resourceSetCache.TryGetValue(assembly, out resourceSet))
            {
                return false;
            }

            string resourceName = resourcePath.TrimStart('/').ToLower();
            using (Stream stream = (Stream)resourceSet.GetObject(resourceName))
            {
                if (stream == null)
                {
                    return false;
                }

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    resourceData = memoryStream.ToArray();
                    return true;
                }
            }
        }

        public static object LoadResourceElement(Uri resourceUri)
        {
            VerifyResourceUri(resourceUri);
            var o = resourceElementCache.GetValue(resourceUri);
            return o;
        }

        private static object ResolveResourceElement(Uri resourceUri)
        {
            string resourceString = Granular.Compatibility.String.FromByteArray(resourceDataCache.GetValue(resourceUri));

            return XamlLoader.Load(XamlParser.Parse(resourceString, resourceUri));
        }

        private static bool TryResolveResourceSet(Assembly assembly, out ResourceSet resourceSet)
        {
            Stream resourceStream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.g.resources");

            if (resourceStream == null)
            {
                resourceSet = null;
                return false;
            }

            resourceSet = new ResourceSet(resourceStream);
            return true;
        }

        private static bool TryParseAbsolutePath(string absolutePath, out string assemblyName, out string resourcePath)
        {
            string[] matches = ResourceUriRegex.Match(absolutePath);

            if (matches == null)
            {
                assemblyName = null;
                resourcePath = null;
                return false;
            }

            assemblyName = matches[ResourceUriAssemblyNameGroupIndex];
            resourcePath = matches[ResourceUriPathGroupIndex];
            return true;
        }

        private static void VerifyResourceUri(Uri resourceUri)
        {
            if (!resourceUri.GetIsAbsoluteUri())
            {
                throw new Granular.Exception("Resource uri \"{0}\" must be an absolute uri", resourceUri.GetOriginalString());
            }

            if (resourceUri.GetScheme() != "pack")
            {
                throw new Granular.Exception("Resource uri \"{0}\" must be a pack uri", resourceUri.GetAbsoluteUri());
            }
        }
    }
}
