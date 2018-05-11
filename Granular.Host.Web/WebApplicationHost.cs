using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using Granular.Host.Render;
using static Retyped.dom;

namespace Granular.Host
{
    public class WebApplicationHost : IApplicationHost
    {
        public IPresentationSourceFactory PresentationSourceFactory { get; private set; }

        public ITaskScheduler TaskScheduler { get; private set; }

        public ITextMeasurementService TextMeasurementService { get; private set; }

        public WebApplicationHost()
        {
            RenderQueue renderQueue = new RenderQueue();
            HtmlValueConverter htmlValueConverter = new HtmlValueConverter();
            SvgValueConverter svgValueConverter = new SvgValueConverter();
            SvgDefinitionContainer svgDefinitionContainer = new SvgDefinitionContainer(renderQueue);
            ImageElementContainer imageElementContainer = new ImageElementContainer();
            EmbeddedResourceObjectFactory embeddedResourceObjectFactory = new EmbeddedResourceObjectFactory(htmlValueConverter);

            HtmlRenderElementFactory htmlRenderElementFactory = new HtmlRenderElementFactory(renderQueue, htmlValueConverter, imageElementContainer, embeddedResourceObjectFactory, svgValueConverter, svgDefinitionContainer);

            PresentationSourceFactory = new PresentationSourceFactory(htmlRenderElementFactory, htmlValueConverter, imageElementContainer, svgDefinitionContainer);
            TaskScheduler = new TaskScheduler();
            TextMeasurementService = new TextMeasurementService(htmlValueConverter);
        }

        public void Run(Action applicationEntryPoint)
        {
            window.onload += e =>
            {
                applicationEntryPoint();

                return e;
            };
        }
    }
}
