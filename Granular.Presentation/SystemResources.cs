using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Markup;
using Granular.Collections;
using Granular.Extensions;

namespace System.Windows
{
    public enum ResourceDictionaryLocation
    {
        None,
        SourceAssembly,
        ExternalAssembly,
    }

    public class ThemeInfoAttribute : Attribute
    {
        public ResourceDictionaryLocation GenericDictionaryLocation { get; private set; }

        public ThemeInfoAttribute(ResourceDictionaryLocation themeDictionaryLocation, ResourceDictionaryLocation genericDictionaryLocation) :
            this(genericDictionaryLocation)
        {
            //
        }

        public ThemeInfoAttribute(ResourceDictionaryLocation genericDictionaryLocation)
        {
            this.GenericDictionaryLocation = genericDictionaryLocation;
        }
    }

    public class SystemResources : IResourceContainer
    {
        private const string ThemeName = "Generic";
        private const string ThemeNameAndColor = "Generic";

        public bool IsEmpty { get { return false; } }

        event EventHandler<ResourcesChangedEventArgs> IResourceContainer.ResourcesChanged { add { } remove { } }

        private CacheDictionary<Assembly, ResourceDictionary> themeResourcesCache;

        public SystemResources()
        {
            themeResourcesCache = CacheDictionary<Assembly, ResourceDictionary>.CreateUsingStringKeys(ResolveAssemblyThemeResources, assembly => assembly.FullName);
        }

        private static bool ResolveAssemblyThemeResources(Assembly assembly, out ResourceDictionary value)
        {
            ThemeInfoAttribute themeInfoAttribute = assembly.FirstOrDefaultCustomAttributeCached<ThemeInfoAttribute>();

            if (themeInfoAttribute == null || themeInfoAttribute.GenericDictionaryLocation == ResourceDictionaryLocation.None)
            {
                value = null;
                return false;
            }

            string themeResourcesAssemblyName = themeInfoAttribute.GenericDictionaryLocation == ResourceDictionaryLocation.SourceAssembly ? assembly.GetName().Name : String.Format("{0}.{1}", assembly.GetName().Name, ThemeName);

            var e = String.Format("pack://application:,,,/{0};component/Themes/{1}.xaml", themeResourcesAssemblyName,
                ThemeNameAndColor);

            value = (ResourceDictionary)EmbeddedResourceLoader.LoadResourceElement(Granular.Compatibility.Uri.CreateAbsoluteUri(e));
            return true;
        }

        public bool TryGetResource(object resourceKey, out object value)
        {
            value = null;

            Assembly assembly = (resourceKey as IResourceKey)?.Assembly;
            if (assembly == null)
            {
                return false;
            }

            ResourceDictionary themeResources;
            return themeResourcesCache.TryGetValue(assembly, out themeResources) && themeResources.TryGetValue(resourceKey, out value);
        }
    }
}
