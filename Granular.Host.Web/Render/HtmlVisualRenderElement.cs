using Granular.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using static Retyped.dom;
using Granular.Extensions;

namespace Granular.Host.Render
{
    public class HtmlVisualRenderElement : HtmlContainerRenderElement, IVisualRenderElement
    {
        private static readonly CacheDictionary<Type, string> ElementTagNameCache = CacheDictionary<Type, string>.CreateUsingStringKeys(ResolveElementTagName, type => type.FullName);

        private Brush background;
        public Brush Background
        {
            get { return background; }
            set
            {
                if (background == value)
                {
                    return;
                }

                if (IsLoaded && background != null)
                {
                    background.Changed -= OnBackgroundChanged;
                }

                background = value;

                renderQueue.InvokeAsync(() =>
                {
                    HtmlElement.SetHtmlBackground(background, new Rect(Bounds.Size), factory, converter);
                    HtmlElement.SetHtmlIsHitTestVisible(IsHitTestVisible && background != null);
                });

                if (IsLoaded && background != null)
                {
                    background.Changed += OnBackgroundChanged;
                }
            }
        }

        private Rect bounds;
        public Rect Bounds
        {
            get { return bounds; }
            set
            {
                if (bounds == value)
                {
                    return;
                }

                bounds = value;
                renderQueue.InvokeAsync(() =>
                {
                    HtmlElement.SetHtmlBounds(bounds, converter);
                    HtmlElement.SetHtmlBackground(background, new Rect(Bounds.Size), factory, converter);
                });
            }
        }

        private HtmlGeometryRenderResource clipRenderResource;
        private Geometry clip;
        public Geometry Clip
        {
            get { return clip; }
            set
            {
                if (clip == value)
                {
                    return;
                }

                if (clipRenderResource != null && IsLoaded)
                {
                    clipRenderResource.Unload();
                }

                clip = value;
                clipRenderResource = (HtmlGeometryRenderResource)(clip?.GetRenderResource(factory));

                if (clipRenderResource != null && IsLoaded)
                {
                    clipRenderResource.Load();
                }

                renderQueue.InvokeAsync(() => HtmlElement.SetSvgClip(clipRenderResource));
            }
        }

        private bool clipToBounds;
        public bool ClipToBounds
        {
            get { return clipToBounds; }
            set
            {
                if (clipToBounds == value)
                {
                    return;
                }

                clipToBounds = value;
                renderQueue.InvokeAsync(() => HtmlElement.SetHtmlClipToBounds(clipToBounds));
            }
        }

        private bool isHitTestVisible;
        public bool IsHitTestVisible
        {
            get { return isHitTestVisible; }
            set
            {
                if (isHitTestVisible == value)
                {
                    return;
                }

                isHitTestVisible = value;
                renderQueue.InvokeAsync(() => HtmlElement.SetHtmlIsHitTestVisible(isHitTestVisible && Background != null));
            }
        }

        private bool isVisible;
        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                if (isVisible == value)
                {
                    return;
                }

                isVisible = value;
                renderQueue.InvokeAsync(() => HtmlElement.SetHtmlIsVisible(isVisible));
            }
        }

        private double opacity;
        public double Opacity
        {
            get { return opacity; }
            set
            {
                if (opacity == value)
                {
                    return;
                }

                opacity = value;
                renderQueue.InvokeAsync(() => HtmlElement.SetHtmlOpacity(opacity, converter));
            }
        }

        private Matrix transform;
        public Matrix Transform
        {
            get { return transform; }
            set
            {
                if (transform == value)
                {
                    return;
                }

                transform = value;
                renderQueue.InvokeAsync(() => HtmlElement.SetHtmlTransform(transform, converter));
            }
        }

        private IRenderElementFactory factory;
        private RenderQueue renderQueue;
        private HtmlValueConverter converter;

        public HtmlVisualRenderElement(object owner, IRenderElementFactory factory, RenderQueue renderQueue, HtmlValueConverter converter) :
            base(CreateHtmlElement(owner), renderQueue)
        {
            this.factory = factory;
            this.renderQueue = renderQueue;
            this.converter = converter;

            bounds = Rect.Zero;
            isVisible = true;
            opacity = 1;
            transform = Matrix.Identity;

            HtmlElement.SetHtmlBounds(Bounds, converter);
            HtmlElement.SetHtmlClipToBounds(ClipToBounds);
            HtmlElement.SetHtmlIsHitTestVisible(IsHitTestVisible && Background != null);
            HtmlElement.SetHtmlIsVisible(IsVisible);
            HtmlElement.SetHtmlOpacity(Opacity, converter);
            HtmlElement.SetHtmlTransform(Transform, converter);
        }

        private void OnBackgroundChanged(object sender, EventArgs e)
        {
            renderQueue.InvokeAsync(() => HtmlElement.SetHtmlBackground(background, new Rect(Bounds.Size), factory, converter));
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            if (Background != null)
            {
                Background.Changed += OnBackgroundChanged;
            }

            if (clipRenderResource != null)
            {
                clipRenderResource.Load();
            }

            renderQueue.InvokeAsync(() => HtmlElement.SetHtmlBackground(Background, new Rect(Bounds.Size), factory, converter));
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            if (Background != null)
            {
                Background.Changed -= OnBackgroundChanged;
            }

            if (clipRenderResource != null)
            {
                clipRenderResource.Unload();
            }
        }

        private static string GetElementTagName(object target)
        {
            return ElementTagNameCache.GetValue(target.GetType());
        }

        private static string ResolveElementTagName(Type type)
        {
            string typeName = type.Name.Replace('$', '_');
            return HtmlDefinition.Tags.Contains(typeName.ToLower()) ? String.Format("{0}_", typeName) : typeName;
        }

        private static string GetElementId(object target)
        {
            RuntimeNamePropertyAttribute nameAttribute = target.GetType().GetCustomAttributes(typeof(RuntimeNamePropertyAttribute), true).FirstOrDefault() as RuntimeNamePropertyAttribute;
            return nameAttribute != null ? (string)target.GetType().GetProperty(nameAttribute.Name).GetValue(target) : String.Empty;
        }

        private static HTMLElement CreateHtmlElement(object owner)
        {
            HTMLElement htmlElement = document.createElement(GetElementTagName(owner));

            string htmlElementId = GetElementId(owner);

            if (!htmlElementId.IsNullOrEmpty())
            {
                htmlElement.id = htmlElementId;
            }

            return htmlElement;
        }
    }
}
