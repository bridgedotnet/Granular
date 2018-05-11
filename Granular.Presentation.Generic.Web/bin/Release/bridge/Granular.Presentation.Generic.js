/**
 * @version 0.4.0.0
 * @author Object.NET, Inc.
 * @copyright Copyright 2008-2018 Object.NET, Inc.
 * @compiler Bridge.NET 17.0.0
 */
Bridge.assembly("Granular.Presentation.Generic", function ($asm, globals) {
    "use strict";

    Bridge.define("Granular.Presentation.Generic.BulletChrome", {
        inherits: [System.Windows.Controls.Control],
        statics: {
            fields: {
                RenderEnabledProperty: null,
                RenderMouseOverProperty: null,
                RenderPressedProperty: null,
                RenderCheckedProperty: null,
                RenderRoundProperty: null,
                BulletTemplateProperty: null,
                IndeterminateBulletTemplateProperty: null,
                IsThreeStateProperty: null
            },
            ctors: {
                init: function () {
                    this.RenderEnabledProperty = System.Windows.DependencyProperty.RegisterAttached("RenderEnabled", System.Boolean, Granular.Presentation.Generic.BulletChrome, new System.Windows.FrameworkPropertyMetadata.$ctor8(System.Windows.FrameworkPropertyMetadataOptions.AffectsVisualState));
                    this.RenderMouseOverProperty = System.Windows.DependencyProperty.RegisterAttached("RenderMouseOver", System.Boolean, Granular.Presentation.Generic.BulletChrome, new System.Windows.FrameworkPropertyMetadata.$ctor8(System.Windows.FrameworkPropertyMetadataOptions.AffectsVisualState));
                    this.RenderPressedProperty = System.Windows.DependencyProperty.RegisterAttached("RenderPressed", System.Boolean, Granular.Presentation.Generic.BulletChrome, new System.Windows.FrameworkPropertyMetadata.$ctor8(System.Windows.FrameworkPropertyMetadataOptions.AffectsVisualState));
                    this.RenderCheckedProperty = System.Windows.DependencyProperty.RegisterAttached("RenderChecked", System.Nullable$1(System.Boolean), Granular.Presentation.Generic.BulletChrome, new System.Windows.FrameworkPropertyMetadata.$ctor8(System.Windows.FrameworkPropertyMetadataOptions.AffectsVisualState));
                    this.RenderRoundProperty = System.Windows.DependencyProperty.RegisterAttached("RenderRound", System.Boolean, Granular.Presentation.Generic.BulletChrome, new System.Windows.FrameworkPropertyMetadata.$ctor1(Bridge.box(true, System.Boolean, System.Boolean.toString)));
                    this.BulletTemplateProperty = System.Windows.DependencyProperty.RegisterAttached("BulletTemplate", System.Windows.Controls.ControlTemplate, Granular.Presentation.Generic.BulletChrome, new System.Windows.FrameworkPropertyMetadata.ctor());
                    this.IndeterminateBulletTemplateProperty = System.Windows.DependencyProperty.RegisterAttached("IndeterminateBulletTemplate", System.Windows.Controls.ControlTemplate, Granular.Presentation.Generic.BulletChrome, new System.Windows.FrameworkPropertyMetadata.ctor());
                    this.IsThreeStateProperty = System.Windows.DependencyProperty.RegisterAttached("IsThreeState", System.Boolean, Granular.Presentation.Generic.BulletChrome, new System.Windows.FrameworkPropertyMetadata.ctor());
                },
                ctor: function () {
                    System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(Granular.Presentation.Generic.BulletChrome, new System.Windows.FrameworkPropertyMetadata.$ctor1(new System.Windows.StyleKey(Granular.Presentation.Generic.BulletChrome)));
                }
            },
            methods: {
                GetPreviewToggledState: function (currentState, isThreeState) {
                    var toggledState = System.Windows.Controls.Primitives.ToggleButton.GetToggledState(currentState, isThreeState);
                    return System.Nullable.eq(toggledState, false) ? currentState : toggledState;
                },
                GetCheckStateName: function (isChecked) {
                    if (System.Nullable.hasValue(isChecked)) {
                        return System.Nullable.getValue(isChecked) ? System.Windows.Controls.VisualStates.CheckedState : System.Windows.Controls.VisualStates.UncheckedState;
                    }

                    return System.Windows.Controls.VisualStates.IndeterminateState;
                }
            }
        },
        props: {
            RenderEnabled: {
                get: function () {
                    return System.Nullable.getValue(Bridge.cast(Bridge.unbox(this.GetValue(Granular.Presentation.Generic.BulletChrome.RenderEnabledProperty)), System.Boolean));
                },
                set: function (value) {
                    this.SetValue(Granular.Presentation.Generic.BulletChrome.RenderEnabledProperty, Bridge.box(value, System.Boolean, System.Boolean.toString));
                }
            },
            RenderMouseOver: {
                get: function () {
                    return System.Nullable.getValue(Bridge.cast(Bridge.unbox(this.GetValue(Granular.Presentation.Generic.BulletChrome.RenderMouseOverProperty)), System.Boolean));
                },
                set: function (value) {
                    this.SetValue(Granular.Presentation.Generic.BulletChrome.RenderMouseOverProperty, Bridge.box(value, System.Boolean, System.Boolean.toString));
                }
            },
            RenderPressed: {
                get: function () {
                    return System.Nullable.getValue(Bridge.cast(Bridge.unbox(this.GetValue(Granular.Presentation.Generic.BulletChrome.RenderPressedProperty)), System.Boolean));
                },
                set: function (value) {
                    this.SetValue(Granular.Presentation.Generic.BulletChrome.RenderPressedProperty, Bridge.box(value, System.Boolean, System.Boolean.toString));
                }
            },
            RenderChecked: {
                get: function () {
                    return Bridge.cast(Bridge.unbox(this.GetValue(Granular.Presentation.Generic.BulletChrome.RenderCheckedProperty)), System.Boolean, true);
                },
                set: function (value) {
                    this.SetValue(Granular.Presentation.Generic.BulletChrome.RenderCheckedProperty, Bridge.box(value, System.Boolean, System.Nullable.toStringFn(System.Boolean.toString), System.Nullable.getHashCode));
                }
            },
            RenderRound: {
                get: function () {
                    return System.Nullable.getValue(Bridge.cast(Bridge.unbox(this.GetValue(Granular.Presentation.Generic.BulletChrome.RenderRoundProperty)), System.Boolean));
                },
                set: function (value) {
                    this.SetValue(Granular.Presentation.Generic.BulletChrome.RenderRoundProperty, Bridge.box(value, System.Boolean, System.Boolean.toString));
                }
            },
            BulletTemplate: {
                get: function () {
                    return Bridge.cast(this.GetValue(Granular.Presentation.Generic.BulletChrome.BulletTemplateProperty), System.Windows.Controls.ControlTemplate);
                },
                set: function (value) {
                    this.SetValue(Granular.Presentation.Generic.BulletChrome.BulletTemplateProperty, value);
                }
            },
            IndeterminateBulletTemplate: {
                get: function () {
                    return Bridge.cast(this.GetValue(Granular.Presentation.Generic.BulletChrome.IndeterminateBulletTemplateProperty), System.Windows.Controls.ControlTemplate);
                },
                set: function (value) {
                    this.SetValue(Granular.Presentation.Generic.BulletChrome.IndeterminateBulletTemplateProperty, value);
                }
            },
            IsThreeState: {
                get: function () {
                    return System.Nullable.getValue(Bridge.cast(Bridge.unbox(this.GetValue(Granular.Presentation.Generic.BulletChrome.IsThreeStateProperty)), System.Boolean));
                },
                set: function (value) {
                    this.SetValue(Granular.Presentation.Generic.BulletChrome.IsThreeStateProperty, Bridge.box(value, System.Boolean, System.Boolean.toString));
                }
            }
        },
        methods: {
            UpdateVisualState: function (useTransitions) {
                System.Windows.VisualStateManager.GoToState(this, this.GetCommonStateName(), useTransitions);
                System.Windows.VisualStateManager.GoToState(this, this.GetCheckStateName(), useTransitions);
            },
            GetCommonStateName: function () {
                if (!this.RenderEnabled) {
                    return System.Windows.Controls.VisualStates.DisabledState;
                }

                if (this.RenderPressed) {
                    return System.Windows.Controls.VisualStates.PressedState;
                }

                if (this.RenderMouseOver) {
                    return System.Windows.Controls.VisualStates.MouseOverState;
                }

                return System.Windows.Controls.VisualStates.NormalState;
            },
            GetCheckStateName: function () {
                return Granular.Presentation.Generic.BulletChrome.GetCheckStateName(this.RenderPressed ? Granular.Presentation.Generic.BulletChrome.GetPreviewToggledState(this.RenderChecked, this.IsThreeState) : this.RenderChecked);
            }
        }
    });

    Bridge.define("Granular.Presentation.Generic.ButtonChrome", {
        inherits: [System.Windows.Controls.Control],
        statics: {
            fields: {
                RenderEnabledProperty: null,
                RenderMouseOverProperty: null,
                RenderPressedProperty: null,
                RenderFocusedProperty: null,
                RenderCornersProperty: null
            },
            ctors: {
                init: function () {
                    this.RenderEnabledProperty = System.Windows.DependencyProperty.RegisterAttached("RenderEnabled", System.Boolean, Granular.Presentation.Generic.ButtonChrome, new System.Windows.FrameworkPropertyMetadata.$ctor8(System.Windows.FrameworkPropertyMetadataOptions.AffectsVisualState));
                    this.RenderMouseOverProperty = System.Windows.DependencyProperty.RegisterAttached("RenderMouseOver", System.Boolean, Granular.Presentation.Generic.ButtonChrome, new System.Windows.FrameworkPropertyMetadata.$ctor8(System.Windows.FrameworkPropertyMetadataOptions.AffectsVisualState));
                    this.RenderPressedProperty = System.Windows.DependencyProperty.RegisterAttached("RenderPressed", System.Boolean, Granular.Presentation.Generic.ButtonChrome, new System.Windows.FrameworkPropertyMetadata.$ctor8(System.Windows.FrameworkPropertyMetadataOptions.AffectsVisualState));
                    this.RenderFocusedProperty = System.Windows.DependencyProperty.RegisterAttached("RenderFocused", System.Boolean, Granular.Presentation.Generic.ButtonChrome, new System.Windows.FrameworkPropertyMetadata.ctor());
                    this.RenderCornersProperty = System.Windows.DependencyProperty.RegisterAttached("RenderCorners", System.Boolean, Granular.Presentation.Generic.ButtonChrome, new System.Windows.FrameworkPropertyMetadata.$ctor1(Bridge.box(true, System.Boolean, System.Boolean.toString)));
                },
                ctor: function () {
                    System.Windows.UIElement.FocusableProperty.OverrideMetadata(Granular.Presentation.Generic.ButtonChrome, new System.Windows.FrameworkPropertyMetadata.$ctor1(Bridge.box(false, System.Boolean, System.Boolean.toString)));
                    System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(Granular.Presentation.Generic.ButtonChrome, new System.Windows.FrameworkPropertyMetadata.$ctor1(new System.Windows.StyleKey(Granular.Presentation.Generic.ButtonChrome)));
                }
            }
        },
        props: {
            RenderEnabled: {
                get: function () {
                    return System.Nullable.getValue(Bridge.cast(Bridge.unbox(this.GetValue(Granular.Presentation.Generic.ButtonChrome.RenderEnabledProperty)), System.Boolean));
                },
                set: function (value) {
                    this.SetValue(Granular.Presentation.Generic.ButtonChrome.RenderEnabledProperty, Bridge.box(value, System.Boolean, System.Boolean.toString));
                }
            },
            RenderMouseOver: {
                get: function () {
                    return System.Nullable.getValue(Bridge.cast(Bridge.unbox(this.GetValue(Granular.Presentation.Generic.ButtonChrome.RenderMouseOverProperty)), System.Boolean));
                },
                set: function (value) {
                    this.SetValue(Granular.Presentation.Generic.ButtonChrome.RenderMouseOverProperty, Bridge.box(value, System.Boolean, System.Boolean.toString));
                }
            },
            RenderPressed: {
                get: function () {
                    return System.Nullable.getValue(Bridge.cast(Bridge.unbox(this.GetValue(Granular.Presentation.Generic.ButtonChrome.RenderPressedProperty)), System.Boolean));
                },
                set: function (value) {
                    this.SetValue(Granular.Presentation.Generic.ButtonChrome.RenderPressedProperty, Bridge.box(value, System.Boolean, System.Boolean.toString));
                }
            },
            RenderFocused: {
                get: function () {
                    return System.Nullable.getValue(Bridge.cast(Bridge.unbox(this.GetValue(Granular.Presentation.Generic.ButtonChrome.RenderFocusedProperty)), System.Boolean));
                },
                set: function (value) {
                    this.SetValue(Granular.Presentation.Generic.ButtonChrome.RenderFocusedProperty, Bridge.box(value, System.Boolean, System.Boolean.toString));
                }
            },
            RenderCorners: {
                get: function () {
                    return System.Nullable.getValue(Bridge.cast(Bridge.unbox(this.GetValue(Granular.Presentation.Generic.ButtonChrome.RenderCornersProperty)), System.Boolean));
                },
                set: function (value) {
                    this.SetValue(Granular.Presentation.Generic.ButtonChrome.RenderCornersProperty, Bridge.box(value, System.Boolean, System.Boolean.toString));
                }
            }
        },
        methods: {
            UpdateVisualState: function (useTransitions) {
                System.Windows.VisualStateManager.GoToState(this, this.GetStateName(), useTransitions);
            },
            GetStateName: function () {
                if (!this.RenderEnabled) {
                    return System.Windows.Controls.VisualStates.DisabledState;
                }

                if (this.RenderPressed) {
                    return System.Windows.Controls.VisualStates.PressedState;
                }

                if (this.RenderMouseOver) {
                    return System.Windows.Controls.VisualStates.MouseOverState;
                }

                return System.Windows.Controls.VisualStates.NormalState;
            }
        }
    });
});
