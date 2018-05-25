/**
 * @version 0.4.0.0
 * @author Object.NET, Inc.
 * @copyright Copyright 2008-2018 Object.NET, Inc.
 * @compiler Bridge.NET 17.0.0
 */
Bridge.assembly("Granular.Common", function ($asm, globals) {
    "use strict";

    Bridge.define("Granular.Collections.CacheDictionary$2", function (TKey, TValue) { return {
        statics: {
            fields: {
                DefaultValue: null
            },
            ctors: {
                init: function () {
                    this.DefaultValue = Bridge.getDefaultValue(TValue);
                }
            },
            methods: {
                Create$1: function (tryResolveValue, comparer) {
                    if (comparer === void 0) { comparer = null; }
                    return new (Granular.Collections.CacheDictionary$2(TKey,TValue))(tryResolveValue, null, new (Granular.Collections.MinimalDictionary$2(TKey,TValue))(comparer), new (Granular.Collections.MinimalSet$1(TKey))(comparer));
                },
                Create: function (resolveValue, comparer) {
                    if (comparer === void 0) { comparer = null; }
                    return new (Granular.Collections.CacheDictionary$2(TKey,TValue))(null, resolveValue, new (Granular.Collections.MinimalDictionary$2(TKey,TValue))(comparer), new (Granular.Collections.MinimalSet$1(TKey))(comparer));
                },
                CreateUsingStringKeys$1: function (tryResolveValue, getStringKey) {
                    if (getStringKey === void 0) { getStringKey = null; }
                    return new (Granular.Collections.CacheDictionary$2(TKey,TValue))(tryResolveValue, null, new (Granular.Collections.ConvertedStringDictionary$2(TKey,TValue))(getStringKey), new (Granular.Collections.ConvertedStringSet$1(TKey))(getStringKey));
                },
                CreateUsingStringKeys: function (resolveValue, getStringKey) {
                    if (getStringKey === void 0) { getStringKey = null; }
                    return new (Granular.Collections.CacheDictionary$2(TKey,TValue))(null, resolveValue, new (Granular.Collections.ConvertedStringDictionary$2(TKey,TValue))(getStringKey), new (Granular.Collections.ConvertedStringSet$1(TKey))(getStringKey));
                }
            }
        },
        fields: {
            tryResolveValue: null,
            resolveValue: null,
            values: null,
            unsetValues: null
        },
        ctors: {
            ctor: function (tryResolveValue, resolveValue, valuesContainer, unsetValuesContainer) {
                this.$initialize();
                this.tryResolveValue = tryResolveValue;
                this.resolveValue = resolveValue;
                this.values = valuesContainer;
                this.unsetValues = unsetValuesContainer;
            }
        },
        methods: {
            GetValue: function (key) {
                var value = { };
                if (this.TryGetValue(key, value)) {
                    return value.v;
                }

                throw new Granular.Exception("Key \"{0}\" was not found", [key]);
            },
            TryGetValue: function (key, value) {
                var result = { };

                if (this.values.Granular$Collections$IMinimalDictionary$TryGetValue(key, result)) {
                    value.v = result.v;
                    return true;
                }

                if (!Bridge.staticEquals(this.resolveValue, null)) {
                    value.v = this.resolveValue(key);
                    this.values.Granular$Collections$IMinimalDictionary$Add(key, value.v);
                    return true;
                }

                if (this.unsetValues.Granular$Collections$IMinimalSet$Contains(key)) {
                    value.v = Granular.Collections.CacheDictionary$2(TKey,TValue).DefaultValue;
                    return false;
                }

                if (this.tryResolveValue(key, value)) {
                    this.values.Granular$Collections$IMinimalDictionary$Add(key, value.v);
                    return true;
                }

                this.unsetValues.Granular$Collections$IMinimalSet$Add(key);
                value.v = Granular.Collections.CacheDictionary$2(TKey,TValue).DefaultValue;
                return false;
            },
            Contains: function (key) {
                return this.values.Granular$Collections$IMinimalDictionary$ContainsKey(key) || this.unsetValues.Granular$Collections$IMinimalSet$Contains(key);
            },
            Remove: function (key) {
                this.values.Granular$Collections$IMinimalDictionary$Remove(key);
                this.unsetValues.Granular$Collections$IMinimalSet$Remove(key);
            },
            Clear: function () {
                this.values.Granular$Collections$IMinimalDictionary$Clear();
                this.unsetValues.Granular$Collections$IMinimalSet$Clear();
            }
        }
    }; });

    Bridge.define("Granular.Collections.IMinimalDictionary", {
        $kind: "interface"
    });

    Bridge.definei("Granular.Collections.IMinimalDictionary$2", function (TKey, TValue) { return {
        $kind: "interface"
    }; });

    Bridge.define("Granular.Collections.IMinimalSet", {
        $kind: "interface"
    });

    Bridge.definei("Granular.Collections.IMinimalSet$1", function (T) { return {
        $kind: "interface"
    }; });

    Bridge.define("Granular.Collections.INotifyCollectionChanged", {
        $kind: "interface"
    });

    Bridge.definei("Granular.Collections.IListDictionary$2", function (TKey, TValue) { return {
        inherits: [System.Collections.Generic.IEnumerable$1(System.Collections.Generic.KeyValuePair$2(TKey,TValue))],
        $kind: "interface"
    }; });

    Bridge.define("Granular.Collections.IListDictionaryExtensions", {
        statics: {
            methods: {
                GetKeys: function (TKey, TValue, listDictionary) {
                    return listDictionary["Granular$Collections$IListDictionary$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$Keys"];
                },
                GetValues: function (TKey, TValue, listDictionary) {
                    return listDictionary["Granular$Collections$IListDictionary$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$Values"];
                },
                Contains: function (TKey, TValue, listDictionary, key, value) {
                    return System.Linq.Enumerable.from(listDictionary["Granular$Collections$IListDictionary$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$GetValues"](key)).contains(value);
                }
            }
        }
    });

    Bridge.define("Granular.Collections.ListSortDirection", {
        $kind: "enum",
        statics: {
            fields: {
                Ascending: 0,
                Descending: 1
            }
        }
    });

    Bridge.define("Granular.Collections.MinimalDictionaryExtensions", {
        statics: {
            methods: {
                GetValue: function (TKey, TValue, minimalDictionary, key) {
                    var value = { };

                    if (minimalDictionary["Granular$Collections$IMinimalDictionary$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$TryGetValue"](key, value)) {
                        return value.v;
                    }

                    throw new Granular.Exception("The given key was not present in the dictionary.");
                },
                GetValue$1: function (minimalDictionary, key) {
                    var value = { };

                    if (minimalDictionary.Granular$Collections$IMinimalDictionary$TryGetValue(key, value)) {
                        return value.v;
                    }

                    throw new Granular.Exception("The given key was not present in the dictionary.");
                }
            }
        }
    });

    Bridge.define("Granular.Collections.NotifyCollectionChangedAction", {
        $kind: "enum",
        statics: {
            fields: {
                Add: 0,
                Remove: 1,
                Replace: 2,
                Move: 3,
                Reset: 4
            }
        }
    });

    Bridge.define("Granular.Collections.NotifyCollectionChangedEventArgs", {
        statics: {
            methods: {
                Add: function (item, index) {
                    return Granular.Collections.NotifyCollectionChangedEventArgs.AddRange(System.Array.init([item], System.Object), index);
                },
                AddRange: function (items, startingIndex) {
                    return new Granular.Collections.NotifyCollectionChangedEventArgs(Granular.Collections.NotifyCollectionChangedAction.Add, System.Array.init(0, null, System.Object), -1, items, startingIndex);
                },
                Remove: function (item, index) {
                    return Granular.Collections.NotifyCollectionChangedEventArgs.RemoveRange(System.Array.init([item], System.Object), index);
                },
                RemoveRange: function (items, startingIndex) {
                    return new Granular.Collections.NotifyCollectionChangedEventArgs(Granular.Collections.NotifyCollectionChangedAction.Remove, items, startingIndex, System.Array.init(0, null, System.Object), -1);
                },
                Replace: function (oldItem, newItem, index) {
                    return Granular.Collections.NotifyCollectionChangedEventArgs.ReplaceRange(System.Array.init([oldItem], System.Object), System.Array.init([newItem], System.Object), index);
                },
                ReplaceRange: function (oldItems, newItems, index) {
                    return new Granular.Collections.NotifyCollectionChangedEventArgs(Granular.Collections.NotifyCollectionChangedAction.Replace, oldItems, index, newItems, index);
                },
                Move: function (item, oldIndex, newIndex) {
                    return Granular.Collections.NotifyCollectionChangedEventArgs.MoveRange(System.Array.init([item], System.Object), oldIndex, newIndex);
                },
                MoveRange: function (items, oldStartingIndex, newStartingIndex) {
                    return new Granular.Collections.NotifyCollectionChangedEventArgs(Granular.Collections.NotifyCollectionChangedAction.Move, items, oldStartingIndex, items, newStartingIndex);
                },
                Reset: function (oldItems, newItems) {
                    return new Granular.Collections.NotifyCollectionChangedEventArgs(Granular.Collections.NotifyCollectionChangedAction.Reset, oldItems, 0, newItems, 0);
                }
            }
        },
        props: {
            Action: 0,
            NewItems: null,
            NewStartingIndex: 0,
            OldItems: null,
            OldStartingIndex: 0
        },
        ctors: {
            ctor: function (action, oldItems, oldStartingIndex, newItems, newStartingIndex) {
                this.$initialize();
                this.Action = action;
                this.OldItems = oldItems;
                this.OldStartingIndex = oldStartingIndex;
                this.NewItems = newItems;
                this.NewStartingIndex = newStartingIndex;
            }
        }
    });

    Bridge.define("Granular.Collections.NotifyCollectionChangedEventHandlerExtensions", {
        statics: {
            methods: {
                Raise: function (handler, sender, e) {
                    if (!Bridge.staticEquals(handler, null)) {
                        handler(sender, e);
                    }
                }
            }
        }
    });

    Bridge.define("Granular.Collections.PriorityQueue$1", function (TValue) { return {
        statics: {
            fields: {
                DefaultValue: null
            },
            ctors: {
                init: function () {
                    this.DefaultValue = Bridge.getDefaultValue(TValue);
                }
            }
        },
        fields: {
            queues: null,
            count: 0,
            topPriority: 0
        },
        props: {
            Count: {
                get: function () {
                    return this.count;
                }
            }
        },
        ctors: {
            ctor: function (maxPriotiry) {
                this.$initialize();
                this.queues = System.Array.init(maxPriotiry, null, System.Collections.Generic.Queue$1(TValue));
            }
        },
        methods: {
            Enqueue: function (priotiry, value) {
                var queue = this.queues[System.Array.index(priotiry, this.queues)];
                if (queue == null) {
                    queue = new (System.Collections.Generic.Queue$1(TValue)).ctor();
                    this.queues[System.Array.index(priotiry, this.queues)] = queue;
                }

                queue.Enqueue(value);
                this.count = (this.count + 1) | 0;

                if (this.topPriority < priotiry) {
                    this.topPriority = priotiry;
                }
            },
            Dequeue: function () {
                var value = { };
                if (this.TryDequeue(value)) {
                    return value.v;
                }

                throw new System.InvalidOperationException.$ctor1("Queue is empty");
            },
            TryDequeue: function (value) {
                if (this.count === 0) {
                    value.v = Granular.Collections.PriorityQueue$1(TValue).DefaultValue;
                    return false;
                }

                while (this.queues[System.Array.index(this.topPriority, this.queues)] == null || this.queues[System.Array.index(this.topPriority, this.queues)].Count === 0) {
                    this.topPriority = (this.topPriority - 1) | 0;
                }

                value.v = this.queues[System.Array.index(this.topPriority, this.queues)].Dequeue();
                this.count = (this.count - 1) | 0;

                return true;
            },
            Peek: function () {
                var value = { };
                if (this.TryPeek(value)) {
                    return value.v;
                }

                throw new System.InvalidOperationException.$ctor1("Queue is empty");
            },
            TryPeek: function (value) {
                if (this.count === 0) {
                    value.v = Granular.Collections.PriorityQueue$1(TValue).DefaultValue;
                    return false;
                }

                while (this.queues[System.Array.index(this.topPriority, this.queues)] == null || this.queues[System.Array.index(this.topPriority, this.queues)].Count === 0) {
                    this.topPriority = (this.topPriority - 1) | 0;
                }

                value.v = this.queues[System.Array.index(this.topPriority, this.queues)].Peek();
                return true;
            }
        }
    }; });

    Bridge.define("Granular.Collections.ReadOnlyStack$1", function (T) { return {
        fields: {
            enumerator: null
        },
        props: {
            IsEmpty: false
        },
        ctors: {
            ctor: function (source) {
                this.$initialize();
                this.enumerator = Bridge.getEnumerator(source, T);

                this.MoveNext();
            }
        },
        methods: {
            Pop: function () {
                if (this.IsEmpty) {
                    throw new Granular.Exception("Stack is empty");
                }

                var current = this.enumerator.System$Collections$IEnumerator$Current;

                this.MoveNext();

                return current;
            },
            Peek: function () {
                if (this.IsEmpty) {
                    throw new Granular.Exception("Stack is empty");
                }

                return this.enumerator.System$Collections$IEnumerator$Current;
            },
            MoveNext: function () {
                this.IsEmpty = !this.enumerator.System$Collections$IEnumerator$moveNext();
            }
        }
    }; });

    Bridge.init(function () {
        Bridge.Reflection.resolveTypeFullName = Bridge.Reflection.getTypeFullName;

        Bridge.Reflection.getTypeFullName = function (obj) {
            if (obj.$$fullname === undefined) {
                obj.$$fullname = Bridge.Reflection.resolveTypeFullName(obj);
            }

            return obj.$$fullname;
        };
    });
    Bridge.init(function () {
        Bridge.resolveTypeAlias = Bridge.getTypeAlias;

        Bridge.getTypeAlias = function (obj) {
            if (obj.$$alias === undefined) {
                obj.$$alias = Bridge.resolveTypeAlias(obj);
            }

            return obj.$$alias;
        };
    });

    Bridge.define("Granular.Common.Compatibility.Initialize");

    Bridge.define("Granular.Compatibility.AssemblyReferenceAttribute", {
        inherits: [System.Attribute],
        ctors: {
            ctor: function (reference) {
                this.$initialize();
                System.Attribute.ctor.call(this);
            }
        }
    });

    Bridge.define("Granular.Compatibility.Collection");

    Bridge.define("Granular.Compatibility.Comparer$1", function (T) { return {
        statics: {
            props: {
                Default: {
                    get: function () {
                        return new (Granular.Compatibility.Comparer$1.CompatibleComparer(T))(new (System.Collections.Generic.Comparer$1(T))(System.Collections.Generic.Comparer$1.$default.fn));
                    }
                }
            }
        }
    }; });

    Bridge.define("Granular.Compatibility.Comparer$1.CompatibleComparer", function (T) { return {
        inherits: [System.Collections.Generic.IComparer$1(T)],
        $kind: "nested class",
        fields: {
            comparer: null
        },
        alias: ["compare", ["System$Collections$Generic$IComparer$1$" + Bridge.getTypeAlias(T) + "$compare", "System$Collections$Generic$IComparer$1$compare"]],
        ctors: {
            ctor: function (comparer) {
                this.$initialize();
                this.comparer = comparer;
            }
        },
        methods: {
            compare: function (x, y) {
                return this.comparer.compare(x, y);
            }
        }
    }; });

    Bridge.define("Granular.Compatibility.Convert", {
        statics: {
            methods: {
                ChangeType: function (value, conversionType) {
                    if (Bridge.referenceEquals(conversionType, System.Int32)) {
                        return Bridge.box(System.Convert.toInt32(value), System.Int32);
                    }

                    if (Bridge.referenceEquals(conversionType, System.Double)) {
                        return Bridge.box(System.Convert.toDouble(value), System.Double, System.Double.format, System.Double.getHashCode);
                    }

                    throw new Granular.Exception("Can't convert value \"{0}\" from \"{1}\" to \"{2}\"", [value, Bridge.Reflection.getTypeName(Bridge.getType(value)), Bridge.Reflection.getTypeName(conversionType)]);
                }
            }
        }
    });

    Bridge.define("Granular.Compatibility.Dictionary");

    Bridge.define("Granular.Compatibility.EqualityComparer", {
        statics: {
            fields: {
                Default: null,
                Double: null
            },
            ctors: {
                init: function () {
                    this.Default = new (Granular.Compatibility.EqualityComparer$1(System.Object))(System.Collections.Generic.EqualityComparer$1(System.Object).def);
                    this.Double = new (Granular.Compatibility.EqualityComparer$1(System.Double))(System.Collections.Generic.EqualityComparer$1(System.Double).def);
                }
            }
        }
    });

    Bridge.define("Granular.Compatibility.EqualityComparer$1", function (T) { return {
        inherits: [System.Collections.Generic.IEqualityComparer$1(T)],
        statics: {
            fields: {
                Default: null
            },
            ctors: {
                init: function () {
                    this.Default = new (Granular.Compatibility.EqualityComparer$1(T))(System.Collections.Generic.EqualityComparer$1(T).def);
                }
            }
        },
        fields: {
            comparer: null
        },
        alias: [
            "equals2", ["System$Collections$Generic$IEqualityComparer$1$" + Bridge.getTypeAlias(T) + "$equals2", "System$Collections$Generic$IEqualityComparer$1$equals2"],
            "getHashCode2", ["System$Collections$Generic$IEqualityComparer$1$" + Bridge.getTypeAlias(T) + "$getHashCode2", "System$Collections$Generic$IEqualityComparer$1$getHashCode2"]
        ],
        ctors: {
            ctor: function (comparer) {
                this.$initialize();
                this.comparer = comparer;
            }
        },
        methods: {
            equals2: function (x, y) {
                return this.comparer[Bridge.geti(this.comparer, "System$Collections$Generic$IEqualityComparer$1$" + Bridge.getTypeAlias(T) + "$equals2", "System$Collections$Generic$IEqualityComparer$1$equals2")](x, y) || x !== x && y !== y;
            },
            getHashCode2: function (obj) {
                return this.comparer[Bridge.geti(this.comparer, "System$Collections$Generic$IEqualityComparer$1$" + Bridge.getTypeAlias(T) + "$getHashCode2", "System$Collections$Generic$IEqualityComparer$1$getHashCode2")](obj);
            }
        }
    }; });

    Bridge.define("Granular.Compatibility.Linq.Enumerable", {
        statics: {
            methods: {
                Aggregate: function (TSource, source, func) {
                    return System.Linq.Enumerable.from(source).aggregate(func);
                },
                All: function (TSource, source, predicate) {
                    var sourceArray = Granular.Compatibility.Linq.Enumerable.AsArray(TSource, source);

                    if (sourceArray == null) {
                        return System.Linq.Enumerable.from(source).all(predicate);
                    }

                    for (var i = 0; i < sourceArray.length; i = (i + 1) | 0) {
                        if (!predicate(sourceArray[System.Array.index(i, sourceArray)])) {
                            return false;
                        }
                    }

                    return true;
                },
                Any: function (TSource, source) {
                    var sourceArray = Granular.Compatibility.Linq.Enumerable.AsArray(TSource, source);

                    if (sourceArray == null) {
                        return System.Linq.Enumerable.from(source).any();
                    }

                    return sourceArray.length > 0;
                },
                Any$1: function (TSource, source, predicate) {
                    var sourceArray = Granular.Compatibility.Linq.Enumerable.AsArray(TSource, source);

                    if (sourceArray == null) {
                        return System.Linq.Enumerable.from(source).any(predicate);
                    }

                    for (var i = 0; i < sourceArray.length; i = (i + 1) | 0) {
                        if (predicate(sourceArray[System.Array.index(i, sourceArray)])) {
                            return true;
                        }
                    }

                    return false;
                },
                Cast: function (TResult, source) {
                    var sourceArray = Bridge.as(source, Array);

                    if (sourceArray == null) {
                        return System.Linq.Enumerable.from(source).select(function (x) { return Bridge.cast(x, TResult); });
                    }

                    var resultArray = System.Array.init(sourceArray.length, function (){
                        return Bridge.getDefaultValue(TResult);
                    }, TResult);

                    for (var i = 0; i < sourceArray.length; i = (i + 1) | 0) {
                        resultArray[System.Array.index(i, resultArray)] = Bridge.cast(Bridge.unbox(System.Array.get(sourceArray, i)), TResult);
                    }

                    return resultArray;
                },
                Concat: function (TSource, first, second) {
                    var firstArray = Granular.Compatibility.Linq.Enumerable.AsArray(TSource, first);
                    var secondArray = Granular.Compatibility.Linq.Enumerable.AsArray(TSource, second);

                    if (firstArray == null || secondArray == null) {
                        return System.Linq.Enumerable.from(first).concat(second);
                    }

                    var resultArray = System.Array.init(((firstArray.length + secondArray.length) | 0), function (){
                        return Bridge.getDefaultValue(TSource);
                    }, TSource);
                    var j = 0;

                    for (var i = 0; i < firstArray.length; i = (i + 1) | 0) {
                        resultArray[System.Array.index(j, resultArray)] = firstArray[System.Array.index(i, firstArray)];
                        j = (j + 1) | 0;
                    }

                    for (var i1 = 0; i1 < secondArray.length; i1 = (i1 + 1) | 0) {
                        resultArray[System.Array.index(j, resultArray)] = secondArray[System.Array.index(i1, secondArray)];
                        j = (j + 1) | 0;
                    }

                    return resultArray;
                },
                Contains: function (TSource, source, value) {
                    var sourceArray = Granular.Compatibility.Linq.Enumerable.AsArray(TSource, source);

                    if (sourceArray == null) {
                        return System.Linq.Enumerable.from(source).contains(value);
                    }

                    for (var i = 0; i < sourceArray.length; i = (i + 1) | 0) {
                        if (Bridge.equals(sourceArray[System.Array.index(i, sourceArray)], value)) {
                            return true;
                        }
                    }

                    return false;
                },
                Count: function (TSource, source) {
                    var sourceArray = Granular.Compatibility.Linq.Enumerable.AsArray(TSource, source);

                    if (sourceArray == null) {
                        return System.Linq.Enumerable.from(source).count();
                    }

                    return sourceArray.length;
                },
                Count$1: function (TSource, source, predicate) {
                    var sourceArray = Granular.Compatibility.Linq.Enumerable.AsArray(TSource, source);

                    if (sourceArray == null) {
                        return System.Linq.Enumerable.from(source).count(predicate);
                    }

                    var count = 0;

                    for (var i = 0; i < sourceArray.length; i = (i + 1) | 0) {
                        if (predicate(sourceArray[System.Array.index(i, sourceArray)])) {
                            count = (count + 1) | 0;
                        }
                    }

                    return count;
                },
                DefaultIfEmpty: function (TSource, source) {
                    var sourceArray = Granular.Compatibility.Linq.Enumerable.AsArray(TSource, source);

                    if (sourceArray == null) {
                        return System.Linq.Enumerable.from(source).defaultIfEmpty(Bridge.getDefaultValue(TSource));
                    }

                    return sourceArray.length > 0 ? sourceArray : System.Array.init([Bridge.getDefaultValue(TSource)], TSource);
                },
                DefaultIfEmpty$1: function (TSource, source, defaultValue) {
                    var sourceArray = Granular.Compatibility.Linq.Enumerable.AsArray(TSource, source);

                    if (sourceArray == null) {
                        return System.Linq.Enumerable.from(source).defaultIfEmpty(defaultValue);
                    }

                    return sourceArray.length > 0 ? sourceArray : System.Array.init([defaultValue], TSource);
                },
                Distinct: function (TSource, source) {
                    return System.Linq.Enumerable.from(source).distinct();
                },
                ElementAt: function (TSource, source, index) {
                    var sourceArray = Granular.Compatibility.Linq.Enumerable.AsArray(TSource, source);

                    if (sourceArray == null) {
                        return System.Linq.Enumerable.from(source).elementAt(index);
                    }

                    return sourceArray[System.Array.index(index, sourceArray)];
                },
                Empty: function (TResult) {
                    return System.Array.init(0, function (){
                        return Bridge.getDefaultValue(TResult);
                    }, TResult);
                },
                Except: function (TSource, first, second) {
                    return System.Linq.Enumerable.from(first).except(second);
                },
                First: function (TSource, source) {
                    var sourceArray = Granular.Compatibility.Linq.Enumerable.AsArray(TSource, source);

                    if (sourceArray == null) {
                        return System.Linq.Enumerable.from(source).first();
                    }

                    if (sourceArray.length === 0) {
                        throw new Granular.Exception("Sequence contains no elements");
                    }

                    return sourceArray[System.Array.index(0, sourceArray)];
                },
                First$1: function (TSource, source, predicate) {
                    var sourceArray = Granular.Compatibility.Linq.Enumerable.AsArray(TSource, source);

                    if (sourceArray == null) {
                        return System.Linq.Enumerable.from(source).first(predicate);
                    }

                    if (sourceArray.length === 0) {
                        throw new Granular.Exception("Sequence contains no elements");
                    }

                    for (var i = 0; i < sourceArray.length; i = (i + 1) | 0) {
                        if (predicate(sourceArray[System.Array.index(i, sourceArray)])) {
                            return sourceArray[System.Array.index(i, sourceArray)];
                        }
                    }

                    throw new Granular.Exception("Sequence contains no matching element");
                },
                FirstOrDefault: function (TSource, source) {
                    var sourceArray = Granular.Compatibility.Linq.Enumerable.AsArray(TSource, source);

                    if (sourceArray == null) {
                        return System.Linq.Enumerable.from(source).firstOrDefault(null, Bridge.getDefaultValue(TSource));
                    }

                    return sourceArray.length > 0 ? sourceArray[System.Array.index(0, sourceArray)] : Bridge.getDefaultValue(TSource);
                },
                FirstOrDefault$1: function (TSource, source, predicate) {
                    var sourceArray = Granular.Compatibility.Linq.Enumerable.AsArray(TSource, source);

                    if (sourceArray == null) {
                        return System.Linq.Enumerable.from(source).firstOrDefault(predicate, Bridge.getDefaultValue(TSource));
                    }

                    for (var i = 0; i < sourceArray.length; i = (i + 1) | 0) {
                        if (predicate(sourceArray[System.Array.index(i, sourceArray)])) {
                            return sourceArray[System.Array.index(i, sourceArray)];
                        }
                    }

                    return Bridge.getDefaultValue(TSource);
                },
                Intersect: function (TSource, first, second) {
                    return System.Linq.Enumerable.from(first).intersect(second);
                },
                Last: function (TSource, source) {
                    var sourceArray = Granular.Compatibility.Linq.Enumerable.AsArray(TSource, source);

                    if (sourceArray == null) {
                        return System.Linq.Enumerable.from(source).last();
                    }

                    if (sourceArray.length === 0) {
                        throw new Granular.Exception("Sequence contains no elements");
                    }

                    return sourceArray[System.Array.index(((sourceArray.length - 1) | 0), sourceArray)];
                },
                Last$1: function (TSource, source, predicate) {
                    var sourceArray = Granular.Compatibility.Linq.Enumerable.AsArray(TSource, source);

                    if (sourceArray == null) {
                        return System.Linq.Enumerable.from(source).last(predicate);
                    }

                    if (sourceArray.length === 0) {
                        throw new Granular.Exception("Sequence contains no elements");
                    }

                    for (var i = (sourceArray.length - 1) | 0; i >= 0; i = (i - 1) | 0) {
                        if (predicate(sourceArray[System.Array.index(i, sourceArray)])) {
                            return sourceArray[System.Array.index(i, sourceArray)];
                        }
                    }

                    throw new Granular.Exception("Sequence contains no matching element");
                },
                LastOrDefault: function (TSource, source) {
                    var sourceArray = Granular.Compatibility.Linq.Enumerable.AsArray(TSource, source);

                    if (sourceArray == null) {
                        return System.Linq.Enumerable.from(source).lastOrDefault(null, Bridge.getDefaultValue(TSource));
                    }

                    return sourceArray.length > 0 ? sourceArray[System.Array.index(((sourceArray.length - 1) | 0), sourceArray)] : Bridge.getDefaultValue(TSource);
                },
                LastOrDefault$1: function (TSource, source, predicate) {
                    var sourceArray = Granular.Compatibility.Linq.Enumerable.AsArray(TSource, source);

                    if (sourceArray == null) {
                        return System.Linq.Enumerable.from(source).lastOrDefault(predicate, Bridge.getDefaultValue(TSource));
                    }

                    for (var i = (sourceArray.length - 1) | 0; i >= 0; i = (i - 1) | 0) {
                        if (predicate(sourceArray[System.Array.index(i, sourceArray)])) {
                            return sourceArray[System.Array.index(i, sourceArray)];
                        }
                    }

                    return Bridge.getDefaultValue(TSource);
                },
                Max$1: function (source) {
                    return System.Linq.Enumerable.from(source).max();
                },
                Max: function (TSource, source) {
                    return System.Linq.Enumerable.from(source).max();
                },
                Min$1: function (source) {
                    return System.Linq.Enumerable.from(source).min();
                },
                Min: function (TSource, source) {
                    return System.Linq.Enumerable.from(source).min();
                },
                OfType: function (TResult, source) {
                    var sourceArray = Granular.Compatibility.Linq.Enumerable.AsArray$1(source);

                    if (sourceArray == null) {
                        return System.Linq.Enumerable.from(source).ofType(TResult);
                    }

                    var resultArray = System.Array.init(0, function (){
                        return Bridge.getDefaultValue(TResult);
                    }, TResult);

                    for (var i = 0; i < sourceArray.length; i = (i + 1) | 0) {
                        var item = Bridge.as(sourceArray[System.Array.index(i, sourceArray)], TResult);
                        if (item != null) {
                            resultArray.push(item);
                        }
                    }

                    return sourceArray.length === resultArray.length ? sourceArray : resultArray;
                },
                OrderBy: function (TSource, TKey, source, keySelector) {
                    return System.Linq.Enumerable.from(source).orderBy(keySelector);
                },
                OrderByDescending: function (TSource, TKey, source, keySelector) {
                    return System.Linq.Enumerable.from(source).orderByDescending(keySelector);
                },
                Reverse: function (TSource, source) {
                    return System.Linq.Enumerable.from(source).reverse();
                },
                Select: function (TSource, TResult, source, selector) {
                    var sourceArray = Granular.Compatibility.Linq.Enumerable.AsArray(TSource, source);

                    if (sourceArray == null) {
                        return System.Linq.Enumerable.from(source).select(selector);
                    }

                    var resultArray = System.Array.init(sourceArray.length, function (){
                        return Bridge.getDefaultValue(TResult);
                    }, TResult);

                    for (var i = 0; i < sourceArray.length; i = (i + 1) | 0) {
                        resultArray[System.Array.index(i, resultArray)] = selector(sourceArray[System.Array.index(i, sourceArray)]);
                    }

                    return resultArray;
                },
                SelectMany: function (TSource, TResult, source, selector) {
                    var $t;
                    var sourceArray = Granular.Compatibility.Linq.Enumerable.AsArray(TSource, source);

                    if (sourceArray == null) {
                        return System.Linq.Enumerable.from(source).selectMany(selector);
                    }

                    var resultArray = System.Array.init(0, function (){
                        return Bridge.getDefaultValue(TResult);
                    }, TResult);

                    for (var i = 0; i < sourceArray.length; i = (i + 1) | 0) {
                        $t = Bridge.getEnumerator(selector(sourceArray[System.Array.index(i, sourceArray)]), TResult);
                        try {
                            while ($t.moveNext()) {
                                var result = $t.Current;
                                resultArray.push(result);
                            }
                        } finally {
                            if (Bridge.is($t, System.IDisposable)) {
                                $t.System$IDisposable$Dispose();
                            }
                        }}

                    return resultArray;
                },
                SequenceEqual: function (TSource, first, second) {
                    return System.Linq.Enumerable.from(first).sequenceEqual(second);
                },
                Single: function (TSource, source) {
                    var sourceArray = Granular.Compatibility.Linq.Enumerable.AsArray(TSource, source);

                    if (sourceArray == null) {
                        return System.Linq.Enumerable.from(source).single();
                    }

                    if (sourceArray.length === 1) {
                        return sourceArray[System.Array.index(0, sourceArray)];
                    }

                    if (sourceArray.length === 0) {
                        throw new Granular.Exception("Sequence contains no elements");
                    }

                    throw new Granular.Exception("Sequence contains more than one element");
                },
                Skip: function (TSource, source, count) {
                    var sourceArray = Granular.Compatibility.Linq.Enumerable.AsArray(TSource, source);

                    if (sourceArray == null) {
                        return System.Linq.Enumerable.from(source).skip(count);
                    }

                    var length = (sourceArray.length - count) | 0;
                    var resultArray = System.Array.init(length, function (){
                        return Bridge.getDefaultValue(TSource);
                    }, TSource);

                    System.Array.copy(sourceArray, count, resultArray, 0, length);

                    return resultArray;
                },
                Sum$2: function (source) {
                    return System.Linq.Enumerable.from(source).sum();
                },
                Sum$3: function (TSource, source, selector) {
                    return System.Linq.Enumerable.from(source).sum(selector);
                },
                Sum: function (source) {
                    return System.Linq.Enumerable.from(source).sum();
                },
                Sum$1: function (TSource, source, selector) {
                    return System.Linq.Enumerable.from(source).sum(selector);
                },
                Take: function (TSource, source, count) {
                    var sourceArray = Granular.Compatibility.Linq.Enumerable.AsArray(TSource, source);

                    if (sourceArray == null) {
                        return System.Linq.Enumerable.from(source).take(count);
                    }

                    var resultArray = System.Array.init(count, function (){
                        return Bridge.getDefaultValue(TSource);
                    }, TSource);

                    System.Array.copy(sourceArray, 0, resultArray, 0, count);

                    return resultArray;
                },
                ToArray: function (TSource, source) {
                    var sourceArray = Granular.Compatibility.Linq.Enumerable.AsArray(TSource, source);

                    if (sourceArray == null) {
                        return System.Linq.Enumerable.from(source).ToArray();
                    }

                    var resultArray = System.Array.init(sourceArray.length, function (){
                        return Bridge.getDefaultValue(TSource);
                    }, TSource);

                    System.Array.copy(sourceArray, 0, resultArray, 0, sourceArray.length);

                    return resultArray;
                },
                ToList: function (TSource, source) {
                    return System.Linq.Enumerable.from(source).toList(TSource);
                },
                Union: function (TSource, first, second) {
                    return System.Linq.Enumerable.from(first).union(second);
                },
                Where: function (TSource, source, predicate) {
                    var sourceArray = Granular.Compatibility.Linq.Enumerable.AsArray(TSource, source);

                    if (sourceArray == null) {
                        return System.Linq.Enumerable.from(source).where(predicate);
                    }

                    var resultArray = System.Array.init(0, function (){
                        return Bridge.getDefaultValue(TSource);
                    }, TSource);

                    for (var i = 0; i < sourceArray.length; i = (i + 1) | 0) {
                        var item = sourceArray[System.Array.index(i, sourceArray)];
                        if (predicate(item)) {
                            resultArray.push(item);
                        }
                    }

                    return sourceArray.length === resultArray.length ? sourceArray : resultArray;
                },
                Zip: function (TFirst, TSecond, TResult, first, second, resultSelector) {
                    return System.Linq.Enumerable.from(first).zip(second, resultSelector);
                },
                AsArray$1: function (source) {
                    return (Bridge.as(source, Array) || Bridge.as(source.items, Array));
                },
                AsArray: function (TSource, source) {
                    return (Bridge.as(source, Array) || Bridge.as(source.items, Array));
                }
            }
        }
    });

    Bridge.define("Granular.Compatibility.String", {
        statics: {
            methods: {
                FromByteArray: function (data) {
                    return decodeURIComponent(escape(String.fromCharCode.apply(null, data)));
                },
                StartsWith: function (s, value) {
                    return s.length > 0 && s.charCodeAt(0) === value;
                },
                StartsWith$1: function (s, prefix) {
                    if (prefix.length > s.length) {
                        return false;
                    }

                    for (var i = 0; i < prefix.length; i = (i + 1) | 0) {
                        if (s.charCodeAt(i) !== prefix.charCodeAt(i)) {
                            return false;
                        }
                    }

                    return true;
                },
                EndsWith: function (s, value) {
                    return s.length > 0 && s.charCodeAt(((s.length - 1) | 0)) === value;
                },
                EndsWith$1: function (s, prefix) {
                    if (prefix.length > s.length) {
                        return false;
                    }

                    var startIndex = (s.length - prefix.length) | 0;
                    for (var i = 0; i < prefix.length; i = (i + 1) | 0) {
                        if (s.charCodeAt(((startIndex + i) | 0)) !== prefix.charCodeAt(i)) {
                            return false;
                        }
                    }

                    return true;
                }
            }
        }
    });

    Bridge.define("Granular.Compatibility.TimeSpan", {
        statics: {
            fields: {
                MinValue: null,
                MaxValue: null,
                TimeSpanFormatRegex: null,
                TimeSpanFormatSignGroupIndex: 0,
                TimeSpanFormatDaysGroupIndex: 0,
                TimeSpanFormatHoursGroupIndex: 0,
                TimeSpanFormatMinutesGroupIndex: 0,
                TimeSpanFormatSecondsGroupIndex: 0,
                TimeSpanFormatMillisecondsGroupIndex: 0,
                TimeSpanFormatDaysAlternativeGroupIndex: 0
            },
            ctors: {
                init: function () {
                    this.MinValue = new System.TimeSpan();
                    this.MaxValue = new System.TimeSpan();
                    this.MinValue = System.TimeSpan.fromDays(-10000);
                    this.MaxValue = System.TimeSpan.fromDays(10000);
                    this.TimeSpanFormatRegex = new RegExp("^(-?)(((([0-9]+)\\.)?([0-9]+):([0-9]+)(:([0-9]*)(\\.([0-9]+))?)?)|([0-9]+))$");
                    this.TimeSpanFormatSignGroupIndex = 1;
                    this.TimeSpanFormatDaysGroupIndex = 5;
                    this.TimeSpanFormatHoursGroupIndex = 6;
                    this.TimeSpanFormatMinutesGroupIndex = 7;
                    this.TimeSpanFormatSecondsGroupIndex = 9;
                    this.TimeSpanFormatMillisecondsGroupIndex = 11;
                    this.TimeSpanFormatDaysAlternativeGroupIndex = 12;
                }
            },
            methods: {
                TryParse: function (s, result) {
                    var days = { v : 0 };
                    var hours = 0;
                    var minutes = 0;
                    var seconds = 0;
                    var milliseconds = 0;

                    var match = Granular.Compatibility.TimeSpan.TimeSpanFormatRegex.exec(s);

                    if (match == null) {
                        result.v = System.TimeSpan.zero;
                        return false;
                    }

                    if (!System.Int32.tryParse(match[System.Array.index(Granular.Compatibility.TimeSpan.TimeSpanFormatDaysAlternativeGroupIndex, match)], days)) {
                        days.v = System.Int32.parse(Granular.Extensions.StringExtensions.DefaultIfNullOrEmpty(match[System.Array.index(Granular.Compatibility.TimeSpan.TimeSpanFormatDaysGroupIndex, match)], "0"));
                        hours = System.Int32.parse(Granular.Extensions.StringExtensions.DefaultIfNullOrEmpty(match[System.Array.index(Granular.Compatibility.TimeSpan.TimeSpanFormatHoursGroupIndex, match)], "0"));
                        minutes = System.Int32.parse(Granular.Extensions.StringExtensions.DefaultIfNullOrEmpty(match[System.Array.index(Granular.Compatibility.TimeSpan.TimeSpanFormatMinutesGroupIndex, match)], "0"));
                        seconds = System.Int32.parse(Granular.Extensions.StringExtensions.DefaultIfNullOrEmpty(match[System.Array.index(Granular.Compatibility.TimeSpan.TimeSpanFormatSecondsGroupIndex, match)], "0"));
                        milliseconds = System.Int32.parse(System.String.alignString(Granular.Extensions.StringExtensions.DefaultIfNullOrEmpty(match[System.Array.index(Granular.Compatibility.TimeSpan.TimeSpanFormatMillisecondsGroupIndex, match)], "000"), -3, 48));
                    }

                    if (hours >= 24 || minutes >= 60 || seconds >= 60 || milliseconds >= 1000) {
                        result.v = System.TimeSpan.zero;
                        return false;
                    }

                    if (Bridge.referenceEquals(match[System.Array.index(Granular.Compatibility.TimeSpan.TimeSpanFormatSignGroupIndex, match)], "-")) {
                        days.v = (-days.v) | 0;
                        hours = (-hours) | 0;
                        minutes = (-minutes) | 0;
                        seconds = (-seconds) | 0;
                        milliseconds = (-milliseconds) | 0;
                    }

                    result.v = new System.TimeSpan(days.v, hours, minutes, seconds, milliseconds);
                    return true;
                },
                Subtract: function (value1, value2) {
                    return System.DateTime.subdd(value1, value2);
                }
            }
        }
    });

    Bridge.define("Granular.Compatibility.Uri", {
        statics: {
            fields: {
                PackUriScheme: null
            },
            ctors: {
                init: function () {
                    this.PackUriScheme = "pack";
                }
            },
            methods: {
                RegisterPackUriParser: function () { },
                CreateRelativeOrAbsoluteUri: function (relativeUriString) {
                    return new System.Uri(relativeUriString);
                },
                CreateAbsoluteUri: function (absoluteUriString) {
                    var uri = { };

                    if (!Granular.Compatibility.Uri.TryCreateAbsoluteUri(absoluteUriString, uri)) {
                        throw new Granular.Exception("Can't create CreateAbsoluteUri from {0}", [absoluteUriString]);
                    }

                    return uri.v;
                },
                CreateAbsoluteUri$1: function (baseUri, relativeUriString) {
                    var uri = { };

                    if (!Granular.Compatibility.Uri.TryCreateAbsoluteUri$1(baseUri, relativeUriString, uri)) {
                        throw new Granular.Exception("Can't create CreateAbsoluteUri from {0} and {1}", [System.UriExtensions.GetAbsolutePath(baseUri), relativeUriString]);
                    }

                    return uri.v;
                },
                CreateRelativeUri: function (relativeUriString) {
                    return new System.Uri(relativeUriString);
                },
                TryCreateAbsoluteUri: function (absoluteUriString, uri) {
                    uri.v = new System.Uri(absoluteUriString);
                    var uriComponents = { };

                    if (!Granular.Compatibility.Uri.TryGetUriComponents(uri.v, uriComponents)) {
                        uri.v = null;
                        return false;
                    }

                    return true;
                },
                TryCreateAbsoluteUri$1: function (baseUri, relativeUriString, uri) {
                    var baseUriComponents = { };

                    if (!Granular.Compatibility.Uri.TryGetUriComponents(baseUri, baseUriComponents)) {
                        uri.v = null;
                        return false;
                    }

                    var uriComponents = baseUriComponents.v.Combine(relativeUriString);

                    uri.v = new System.Uri(uriComponents.AbsoluteUri);
                    uri.v.UriComponents = uriComponents;

                    return true;
                },
                GetUriComponents: function (uri) {
                    var uriComponents = { };
                    if (!Granular.Compatibility.Uri.TryGetUriComponents(uri, uriComponents)) {
                        throw new System.InvalidOperationException.$ctor1("This operation is not supported for a relative URI.");
                    }

                    return uriComponents.v;
                },
                TryGetUriComponents: function (uri, uriComponents) {
                    if (uri.hasOwnProperty("UriComponents")) {
                        uriComponents.v = Bridge.cast(uri.UriComponents, Granular.Compatibility.UriComponents);
                    } else {
                        if (!Granular.Compatibility.UriComponents.TryParse(uri.getAbsoluteUri(), uriComponents)) {
                            uriComponents.v = null;
                        }

                        uri.UriComponents = uriComponents.v;
                    }

                    return uriComponents.v != null;
                }
            }
        }
    });

    Bridge.define("Granular.Compatibility.UriComponents", {
        statics: {
            fields: {
                AbsoluteUriRegex: null,
                RelativeUriRegex: null,
                RootedPathRegex: null,
                HttpDefaultPort: 0,
                HttpsDefaultPort: 0
            },
            ctors: {
                init: function () {
                    this.AbsoluteUriRegex = new RegExp("^([^:]*):(\\/\\/(([^@]*)@)?([^:\\/]*)(:[^\\/]*)?)?(\\/?[^\\?#]*)(\\?[^#]*)?(#.*)?$");
                    this.RelativeUriRegex = new RegExp("^(\\/?[^\\?#]*)(\\?[^#]*)?(#.*)?$");
                    this.RootedPathRegex = new RegExp("^([a-zA-Z]:\\/)[^\\/]");
                    this.HttpDefaultPort = 80;
                    this.HttpsDefaultPort = 443;
                }
            },
            methods: {
                CombineFilePath: function (path, relativePath) {
                    if (Granular.Compatibility.UriComponents.RootedPathRegex.exec(relativePath) != null) {
                        return relativePath;
                    }

                    if (System.String.startsWith(relativePath, "/")) {
                        return (path.substr(0, ((System.String.indexOf(path, String.fromCharCode(58)) + 1) | 0)) || "") + (relativePath || "");
                    }

                    return (path.substr(0, ((path.lastIndexOf(String.fromCharCode(47)) + 1) | 0)) || "") + (relativePath || "");
                },
                CombineUncPath: function (absolutePath, relativePath) {
                    return System.String.startsWith(relativePath, "/") ? ((absolutePath || "") + (relativePath || "")) : ((absolutePath.substr(0, ((absolutePath.lastIndexOf(String.fromCharCode(47)) + 1) | 0)) || "") + (relativePath || ""));
                },
                TryParse: function (uriString, uriComponents) {
                    uriString = System.String.replaceAll(uriString, String.fromCharCode(92), String.fromCharCode(47));

                    if (Granular.Compatibility.UriComponents.RootedPathRegex.exec(uriString) != null) {
                        uriString = "file:///" + (uriString || "");
                    }

                    var absoluteUriMatch = Granular.Compatibility.UriComponents.AbsoluteUriRegex.exec(uriString);
                    if (absoluteUriMatch == null) {
                        uriComponents.v = null;
                        return false;
                    }

                    var scheme = Granular.Extensions.StringExtensions.DefaultIfNullOrEmpty(absoluteUriMatch[System.Array.index(1, absoluteUriMatch)]);
                    var userInfo = Granular.Extensions.StringExtensions.DefaultIfNullOrEmpty(absoluteUriMatch[System.Array.index(4, absoluteUriMatch)]);
                    var path = Granular.Extensions.StringExtensions.DefaultIfNullOrEmpty(absoluteUriMatch[System.Array.index(7, absoluteUriMatch)]);
                    var query = Granular.Extensions.StringExtensions.DefaultIfNullOrEmpty(absoluteUriMatch[System.Array.index(8, absoluteUriMatch)]);
                    var fragment = Granular.Extensions.StringExtensions.DefaultIfNullOrEmpty(absoluteUriMatch[System.Array.index(9, absoluteUriMatch)]);

                    var host;
                    var port = { };

                    var hostGroupValue = Granular.Extensions.StringExtensions.DefaultIfNullOrEmpty(absoluteUriMatch[System.Array.index(5, absoluteUriMatch)]);
                    var portGroupValue = Granular.Extensions.StringExtensions.DefaultIfNullOrEmpty(absoluteUriMatch[System.Array.index(6, absoluteUriMatch)]);

                    if (Granular.Extensions.StringExtensions.IsNullOrEmpty(portGroupValue)) {
                        host = hostGroupValue;
                        port.v = Granular.Compatibility.UriComponents.GetDefaultPort(scheme);
                    } else if (!System.Int32.tryParse(portGroupValue.substr(1), port)) {
                        host = (hostGroupValue || "") + (portGroupValue || "");
                        port.v = -1;
                    } else {
                        host = hostGroupValue;
                    }

                    uriComponents.v = new Granular.Compatibility.UriComponents(scheme, userInfo, host, port.v, path, query, fragment);
                    return true;
                },
                GetDefaultPort: function (scheme) {
                    if (Bridge.referenceEquals(scheme.toLowerCase(), "http")) {
                        return Granular.Compatibility.UriComponents.HttpDefaultPort;
                    }

                    if (Bridge.referenceEquals(scheme.toLowerCase(), "https")) {
                        return Granular.Compatibility.UriComponents.HttpsDefaultPort;
                    }

                    return -1;
                },
                GetPathSegments: function (path) {
                    var segments = System.String.split(path, [47].map(function (i) {{ return String.fromCharCode(i); }}));

                    for (var i = 0; i < ((segments.length - 1) | 0); i = (i + 1) | 0) {
                        segments[System.Array.index(i, segments)] = (segments[System.Array.index(i, segments)] || "") + "/";
                    }

                    return segments;
                },
                GetAbsoluteUri: function (scheme, userInfo, host, port, path, query, fragment) {
                    var stringBuilder = new System.Text.StringBuilder();

                    stringBuilder.append(scheme);
                    stringBuilder.append("://");

                    if (!Granular.Extensions.StringExtensions.IsNullOrEmpty(userInfo)) {
                        stringBuilder.append(userInfo);
                        stringBuilder.append("@");
                    }

                    stringBuilder.append(host);

                    if (port !== Granular.Compatibility.UriComponents.GetDefaultPort(scheme)) {
                        stringBuilder.append(":");
                        stringBuilder.append(port);
                    }

                    stringBuilder.append(path);
                    stringBuilder.append(query);
                    stringBuilder.append(fragment);

                    return stringBuilder.toString();
                }
            }
        },
        props: {
            Scheme: null,
            AbsoluteUri: null,
            AbsolutePath: null,
            LocalPath: null,
            Segments: null,
            UserInfo: null,
            Host: null,
            Port: 0,
            Query: null,
            Fragment: null,
            IsDefaultPort: false,
            IsFile: false,
            IsUnc: false,
            IsLoopback: false,
            PathAndQuery: null
        },
        ctors: {
            ctor: function (scheme, userInfo, host, port, path, query, fragment) {
                this.$initialize();
                var isFile = Bridge.referenceEquals(scheme.toLowerCase(), "file");
                var isUnc = isFile && !Granular.Extensions.StringExtensions.IsNullOrEmpty(host);
                var isLocalhost = Bridge.referenceEquals(host.toLowerCase(), "localhost");
                var absolutePath = isFile && !isUnc ? System.String.trimStart(path, [47]) : path;

                this.Scheme = scheme;
                this.AbsolutePath = absolutePath;
                this.UserInfo = userInfo;
                this.Host = host;
                this.Port = port;
                this.Query = query;
                this.Fragment = fragment;

                this.IsFile = isFile;
                this.IsUnc = isUnc;
                this.IsLoopback = isFile && !isUnc || isLocalhost;
                this.LocalPath = isUnc ? "\\\\" + (host || "") + (System.String.replaceAll(absolutePath, String.fromCharCode(47), String.fromCharCode(92)) || "") : isFile ? System.String.replaceAll(absolutePath, String.fromCharCode(47), String.fromCharCode(92)) : absolutePath;
                this.IsDefaultPort = this.Port === Granular.Compatibility.UriComponents.GetDefaultPort(scheme);
                this.PathAndQuery = (absolutePath || "") + (query || "");
                this.AbsoluteUri = Granular.Compatibility.UriComponents.GetAbsoluteUri(scheme, userInfo, host, port, path, query, fragment);
                this.Segments = Granular.Compatibility.UriComponents.GetPathSegments(path);
            }
        },
        methods: {
            Combine: function (relativeUriString) {
                relativeUriString = System.String.replaceAll(relativeUriString, String.fromCharCode(92), String.fromCharCode(47));

                if (this.IsUnc) {
                    return new Granular.Compatibility.UriComponents(this.Scheme, this.UserInfo, this.Host, this.Port, Granular.Compatibility.UriComponents.CombineUncPath(this.AbsolutePath, relativeUriString), this.Query, this.Fragment);
                }

                if (this.IsFile) {
                    return new Granular.Compatibility.UriComponents(this.Scheme, this.UserInfo, this.Host, this.Port, "/" + (Granular.Compatibility.UriComponents.CombineFilePath(this.AbsolutePath, relativeUriString) || ""), this.Query, this.Fragment);
                }

                var relativeUriMatch = Granular.Compatibility.UriComponents.RelativeUriRegex.exec(relativeUriString);

                var path = Granular.Extensions.StringExtensions.DefaultIfNullOrEmpty(relativeUriMatch[System.Array.index(1, relativeUriMatch)]);
                var query = Granular.Extensions.StringExtensions.DefaultIfNullOrEmpty(relativeUriMatch[System.Array.index(2, relativeUriMatch)]);
                var fragment = Granular.Extensions.StringExtensions.DefaultIfNullOrEmpty(relativeUriMatch[System.Array.index(3, relativeUriMatch)]);

                var combinedPath = System.String.startsWith(path, "/") ? path : ((this.AbsolutePath.substr(0, ((this.AbsolutePath.lastIndexOf(String.fromCharCode(47)) + 1) | 0)) || "") + (path || ""));

                return new Granular.Compatibility.UriComponents(this.Scheme, this.UserInfo, this.Host, this.Port, combinedPath, query, fragment);
            }
        }
    });

    Bridge.define("Granular.Diagnostics.HitCounter", {
        fields: {
            name: null,
            getAdditionalStatus: null,
            totalHitsCount: 0,
            lastHitsCount: 0,
            lastReportTime: null
        },
        ctors: {
            init: function () {
                this.lastReportTime = System.DateTime.getDefaultValue();
            },
            ctor: function (name, getAdditionalStatus) {
                if (getAdditionalStatus === void 0) { getAdditionalStatus = null; }

                this.$initialize();
                this.name = name;
                this.getAdditionalStatus = getAdditionalStatus;

                this.lastReportTime = System.DateTime.getNow();
                System.Console.WriteLine(System.String.format("{0} - HitCounter initialized", [name]));
            }
        },
        methods: {
            Hit: function () {
                this.totalHitsCount = (this.totalHitsCount + 1) | 0;

                var now = System.DateTime.getNow();
                var interval = Granular.Compatibility.TimeSpan.Subtract(now, this.lastReportTime);

                if (System.TimeSpan.gte(interval, System.TimeSpan.fromSeconds(1))) {
                    var rate = Bridge.Math.round((((this.totalHitsCount - this.lastHitsCount) | 0)) / interval.getTotalSeconds(), 1, 6);
                    var additionalStatus = !Bridge.staticEquals(this.getAdditionalStatus, null) ? this.getAdditionalStatus() : "";

                    if (Granular.Extensions.StringExtensions.IsNullOrEmpty(additionalStatus)) {
                        System.Console.WriteLine(System.String.format("{0} - Total: {1} hits, Rate: {2} hits/sec", this.name, Bridge.box(this.totalHitsCount, System.Int32), Bridge.box(rate, System.Double, System.Double.format, System.Double.getHashCode)));
                    } else {
                        System.Console.WriteLine(System.String.format("{0} - Total: {1} hits, Rate: {2} hits/sec - {3}", this.name, Bridge.box(this.totalHitsCount, System.Int32), Bridge.box(rate, System.Double, System.Double.format, System.Double.getHashCode), additionalStatus));
                    }

                    this.lastReportTime = now;
                    this.lastHitsCount = this.totalHitsCount;
                }
            }
        }
    });

    Bridge.define("Granular.Diagnostics.Profiler", {
        statics: {
            fields: {
                scopes: null
            },
            ctors: {
                init: function () {
                    this.scopes = new (System.Collections.Generic.Dictionary$2(System.String,Granular.Diagnostics.Profiler.ProfilingScope))();
                }
            },
            methods: {
                IncludeScope: function (scopeName) {
                    return Granular.Diagnostics.Profiler.GetInitializedScope(scopeName).Include();
                },
                ExcludeScope: function (scopeName) {
                    return Granular.Diagnostics.Profiler.GetInitializedScope(scopeName).Exclude();
                },
                GetInitializedScope: function (scopeName) {
                    var scope = { };
                    if (!Granular.Diagnostics.Profiler.scopes.tryGetValue(scopeName, scope)) {
                        scope.v = new Granular.Diagnostics.Profiler.ProfilingScope(scopeName);
                        Granular.Diagnostics.Profiler.scopes.add(scopeName, scope.v);
                    }

                    return scope.v;
                }
            }
        }
    });

    Bridge.define("Granular.Diagnostics.Profiler.ProfilingScope", {
        $kind: "nested class",
        fields: {
            totalInclusiveTime: null,
            totalExclusiveTime: null,
            exclusiveTime: null,
            includeLevel: 0,
            isIncluding: false,
            name: null
        },
        ctors: {
            init: function () {
                this.totalInclusiveTime = new System.TimeSpan();
                this.totalExclusiveTime = new System.TimeSpan();
                this.exclusiveTime = new System.TimeSpan();
            },
            ctor: function (name) {
                this.$initialize();
                this.name = name;

                this.totalInclusiveTime = System.TimeSpan.zero;
                this.totalExclusiveTime = System.TimeSpan.zero;
            }
        },
        methods: {
            Include: function () {
                if (this.includeLevel === 0) {
                    this.exclusiveTime = System.TimeSpan.zero;
                }

                if (this.isIncluding) {
                    return Granular.Disposable.Empty;
                }

                this.includeLevel = (this.includeLevel + 1) | 0;
                this.isIncluding = true;

                var includeStartTime = System.DateTime.getNow();

                return new Granular.Disposable(Bridge.fn.bind(this, function () {
                    this.includeLevel = (this.includeLevel - 1) | 0;
                    this.isIncluding = false;

                    var includeTime = System.DateTime.subdd(System.DateTime.getNow(), includeStartTime);
                    this.exclusiveTime = System.TimeSpan.add(this.exclusiveTime, includeTime);

                    if (this.includeLevel === 0) {
                        this.totalExclusiveTime = System.TimeSpan.add(this.totalExclusiveTime, this.exclusiveTime);
                        this.totalInclusiveTime = System.TimeSpan.add(this.totalInclusiveTime, includeTime);

                        System.Console.WriteLine(System.String.format("{0} - exclusive {1}ms (total {2}ms), inclusive {3}ms (total {4}ms)", this.name, Bridge.Int.clip64(this.exclusiveTime.getTotalMilliseconds()), Bridge.Int.clip64(this.totalExclusiveTime.getTotalMilliseconds()), Bridge.Int.clip64(includeTime.getTotalMilliseconds()), Bridge.Int.clip64(this.totalInclusiveTime.getTotalMilliseconds())));
                    }
                }));
            },
            Exclude: function () {
                if (!this.isIncluding) {
                    return Granular.Disposable.Empty;
                }

                this.isIncluding = false;

                var excludeStartTime = System.DateTime.getNow();

                return new Granular.Disposable(Bridge.fn.bind(this, function () {
                    this.isIncluding = true;

                    var excludeTime = System.DateTime.subdd(System.DateTime.getNow(), excludeStartTime);

                    this.exclusiveTime = System.TimeSpan.sub(this.exclusiveTime, excludeTime);
                }));
            }
        }
    });

    Bridge.define("Granular.Disposable", {
        inherits: [System.IDisposable],
        statics: {
            fields: {
                Empty: null
            },
            ctors: {
                init: function () {
                    this.Empty = new Granular.Disposable.EmptyDisposable();
                }
            },
            methods: {
                Combine: function (disposable1, disposable2) {
                    return new Granular.Disposable(function () {
                        disposable1.System$IDisposable$Dispose();
                        disposable2.System$IDisposable$Dispose();
                    });
                }
            }
        },
        fields: {
            dispose: null
        },
        alias: ["Dispose", "System$IDisposable$Dispose"],
        ctors: {
            ctor: function (dispose) {
                this.$initialize();
                this.dispose = dispose;
            }
        },
        methods: {
            Dispose: function () {
                this.dispose();
            }
        }
    });

    Bridge.define("Granular.Disposable.EmptyDisposable", {
        inherits: [System.IDisposable],
        $kind: "nested class",
        alias: ["Dispose", "System$IDisposable$Dispose"],
        methods: {
            Dispose: function () { }
        }
    });

    Bridge.define("Granular.Exception", {
        inherits: [System.Exception],
        ctors: {
            ctor: function (format, args) {
                if (args === void 0) { args = []; }

                this.$initialize();
                System.Exception.ctor.call(this, System.String.format.apply(System.String, [format].concat(args)));
            }
        },
        methods: {
            toString: function () {
                return this.Message;
            }
        }
    });

    Bridge.define("Granular.Extensions.AssemblyExtensions", {
        statics: {
            fields: {
                attributesCache: null
            },
            ctors: {
                init: function () {
                    this.attributesCache = new (System.Collections.Generic.Dictionary$2(System.String,System.Collections.Generic.IEnumerable$1(System.Object)))();
                }
            },
            methods: {
                GetCustomAttributesCached: function (T, assembly) {
                    var attributes = { };

                    if (Granular.Extensions.AssemblyExtensions.attributesCache.tryGetValue(assembly.name, attributes)) {
                        return System.Linq.Enumerable.from(attributes.v).ofType(T);
                    }

                    attributes.v = assembly.getCustomAttributes(false) || System.Array.init(0, null, System.Object);

                    Granular.Extensions.AssemblyExtensions.attributesCache.add(assembly.name, attributes.v);
                    return System.Linq.Enumerable.from(attributes.v).ofType(T);
                },
                FirstOrDefaultCustomAttributeCached: function (T, assembly) {
                    return System.Linq.Enumerable.from(Granular.Extensions.AssemblyExtensions.GetCustomAttributesCached(T, assembly)).firstOrDefault(null, Bridge.getDefaultValue(T));
                }
            }
        }
    });

    Bridge.define("Granular.Extensions.BinaryReaderExtensions", {
        statics: {
            methods: {
                SkipStrings: function (binaryReader, count) {
                    for (var i = 0; i < count; i = (i + 1) | 0) {
                        binaryReader.ReadString();
                    }
                }
            }
        }
    });

    Bridge.define("Granular.Extensions.CollectionExtensions", {
        statics: {
            methods: {
                AddRange$1: function (T, collection, items) {
                    var $t;
                    $t = Bridge.getEnumerator(items, T);
                    try {
                        while ($t.moveNext()) {
                            var item = $t.Current;
                            System.Array.add(collection, item, T);
                        }
                    } finally {
                        if (Bridge.is($t, System.IDisposable)) {
                            $t.System$IDisposable$Dispose();
                        }
                    }},
                AddRange: function (collection, items) {
                    var $t;
                    $t = Bridge.getEnumerator(items);
                    try {
                        while ($t.moveNext()) {
                            var item = $t.Current;
                            System.Array.add(collection, item);
                        }
                    } finally {
                        if (Bridge.is($t, System.IDisposable)) {
                            $t.System$IDisposable$Dispose();
                        }
                    }}
            }
        }
    });

    Bridge.define("Granular.Extensions.DoubleExtensions", {
        statics: {
            fields: {
                Epsilon: 0
            },
            ctors: {
                init: function () {
                    this.Epsilon = 1E-10;
                }
            },
            methods: {
                IsClose: function ($this, value) {
                    return ($this === value) || Granular.Extensions.DoubleExtensions.IsNaN($this) && Granular.Extensions.DoubleExtensions.IsNaN(value) || Math.abs($this - value) < Granular.Extensions.DoubleExtensions.Epsilon * (Math.abs($this) + Math.abs(value) + 1);
                },
                IsNaN: function ($this) {
                    return isNaN($this);
                },
                DefaultIfNaN: function ($this, defaultValue) {
                    return isNaN($this) ? defaultValue : $this;
                },
                Min: function ($this, value) {
                    return $this < value ? $this : value;
                },
                Max: function ($this, value) {
                    return $this > value ? $this : value;
                },
                Bounds: function ($this, minimum, maximum) {
                    if (minimum > maximum) {
                        throw new Granular.Exception("Invalid bounds (minimum: {0}, maximum: {1})", [Bridge.box(minimum, System.Double, System.Double.format, System.Double.getHashCode), Bridge.box(maximum, System.Double, System.Double.format, System.Double.getHashCode)]);
                    }

                    return Granular.Extensions.DoubleExtensions.Min(Granular.Extensions.DoubleExtensions.Max($this, minimum), maximum);
                },
                Abs: function ($this) {
                    return Math.abs($this);
                }
            }
        }
    });

    Bridge.define("Granular.Extensions.EnumerableExtensions", {
        statics: {
            methods: {
                ConcatSingle: function (TSource, source, value) {
                    return System.Linq.Enumerable.from(source).concat(System.Array.init([value], TSource));
                },
                TrySelect: function (TSource, TResult, source, selector) {
                    return new (Bridge.GeneratorEnumerable$1(TResult))(Bridge.fn.bind(this, function (TSource, TResult, source, selector) {
                        var $step = 0,
                            $jumpFromFinally,
                            $returnValue,
                            $t,
                            item,
                            result,
                            $async_e;

                        var $enumerator = new (Bridge.GeneratorEnumerator$1(TResult))(Bridge.fn.bind(this, function () {
                            try {
                                for (;;) {
                                    switch ($step) {
                                        case 0: {
                                            $t = Bridge.getEnumerator(source, TSource);
                                                $step = 1;
                                                continue;
                                        }
                                        case 1: {
                                            if ($t.moveNext()) {
                                                    item = $t.Current;
                                                    $step = 2;
                                                    continue;
                                                }
                                            $step = 6;
                                            continue;
                                        }
                                        case 2: {
                                            result = { };
                                                if (selector(item, result)) {
                                                    $step = 3;
                                                    continue;
                                                } 
                                                $step = 5;
                                                continue;
                                        }
                                        case 3: {
                                            $enumerator.current = result.v;
                                                $step = 4;
                                                return true;
                                        }
                                        case 4: {
                                            $step = 5;
                                            continue;
                                        }
                                        case 5: {
                                            $step = 1;
                                            continue;
                                        }
                                        case 6: {

                                        }
                                        default: {
                                            return false;
                                        }
                                    }
                                }
                            } catch($async_e1) {
                                $async_e = System.Exception.create($async_e1);
                                throw $async_e;
                            }
                        }));
                        return $enumerator;
                    }, arguments));
                }
            }
        }
    });

    Bridge.define("Granular.Extensions.EventHandlerExtensions", {
        statics: {
            methods: {
                Raise$2: function (handler, sender) {
                    if (!Bridge.staticEquals(handler, null)) {
                        handler(sender, { });
                    }
                },
                Raise$3: function (handler, sender, e) {
                    if (!Bridge.staticEquals(handler, null)) {
                        handler(sender, e);
                    }
                },
                Raise$4: function (T, handler, sender, e) {
                    if (!Bridge.staticEquals(handler, null)) {
                        handler(sender, e);
                    }
                },
                Raise$1: function (handler, sender, propertyName) {
                    Granular.Extensions.EventHandlerExtensions.Raise(handler, sender, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
                },
                Raise: function (handler, sender, e) {
                    if (!Bridge.staticEquals(handler, null)) {
                        handler(sender, e);
                    }
                }
            }
        }
    });

    Bridge.define("Granular.Extensions.IntExtensions", {
        statics: {
            methods: {
                Min: function ($this, value) {
                    return $this < value ? $this : value;
                },
                Max: function ($this, value) {
                    return $this > value ? $this : value;
                },
                Bounds: function ($this, minimum, maximum) {
                    if (minimum > maximum) {
                        throw new Granular.Exception("Invalid bounds (minimum: {0}, maximum: {1})", [Bridge.box(minimum, System.Int32), Bridge.box(maximum, System.Int32)]);
                    }

                    return Granular.Extensions.IntExtensions.Min(Granular.Extensions.IntExtensions.Max($this, minimum), maximum);
                }
            }
        }
    });

    Bridge.define("Granular.Extensions.ListExtensions", {
        statics: {
            methods: {
                InsertRange: function (T, list, index, values) {
                    var $t;
                    $t = Bridge.getEnumerator(values, T);
                    try {
                        while ($t.moveNext()) {
                            var value = $t.Current;
                            System.Array.insert(list, index, value, T);
                            index = (index + 1) | 0;
                        }
                    } finally {
                        if (Bridge.is($t, System.IDisposable)) {
                            $t.System$IDisposable$Dispose();
                        }
                    }},
                RemoveRange: function (T, list, index, count) {
                    for (var i = 0; i < count; i = (i + 1) | 0) {
                        System.Array.removeAt(list, index, T);
                    }
                }
            }
        }
    });

    Bridge.define("Granular.Extensions.StringExtensions", {
        statics: {
            methods: {
                IsNullOrEmpty: function ($this) {
                    return System.String.isNullOrEmpty($this);
                },
                IsNullOrWhiteSpace: function ($this) {
                    return System.String.isNullOrWhiteSpace($this);
                },
                DefaultIfNullOrEmpty: function ($this, defaultValue) {
                    var $t;
                    if (defaultValue === void 0) { defaultValue = null; }
                    return System.String.isNullOrEmpty($this) ? (($t = defaultValue, $t != null ? $t : "")) : $this;
                },
                GetCharacterIndexFromLineIndex: function ($this, lineIndex) {
                    var linesStartIndex = System.Linq.Enumerable.from(Bridge.cast(System.Array.init([0], System.Int32), System.Collections.Generic.IEnumerable$1(System.Int32))).concat(System.Linq.Enumerable.from(Granular.Extensions.StringExtensions.IndexOfAll($this, "\n")).select($asm.$.Granular.Extensions.StringExtensions.f1)).ToArray(System.Int32);

                    return lineIndex >= 0 && lineIndex < linesStartIndex.length ? linesStartIndex[System.Array.index(lineIndex, linesStartIndex)] : -1;
                },
                GetLineIndexFromCharacterIndex: function ($this, charIndex) {
                    var linesIndex = Granular.Extensions.StringExtensions.IndexOfAll($this, "\n");
                    var linesStartIndex = System.Linq.Enumerable.from(Bridge.cast(System.Array.init([0], System.Int32), System.Collections.Generic.IEnumerable$1(System.Int32))).concat(System.Linq.Enumerable.from(linesIndex).select($asm.$.Granular.Extensions.StringExtensions.f1)).ToArray(System.Int32);
                    var linesEndIndex = System.Linq.Enumerable.from(Bridge.cast(linesIndex, System.Collections.Generic.IEnumerable$1(System.Int32))).concat(System.Array.init([$this.length], System.Int32)).ToArray(System.Int32);

                    for (var i = 0; i < linesStartIndex.length; i = (i + 1) | 0) {
                        if (linesStartIndex[System.Array.index(i, linesStartIndex)] <= charIndex && charIndex <= linesEndIndex[System.Array.index(i, linesEndIndex)]) {
                            return i;
                        }
                    }

                    return -1;
                },
                GetLineLength: function ($this, lineIndex) {
                    var lines = Granular.Extensions.StringExtensions.GetLines($this);
                    return lineIndex >= 0 && lineIndex < lines.length ? lines[System.Array.index(lineIndex, lines)].length : -1;
                },
                GetLineText: function ($this, lineIndex) {
                    var lines = Granular.Extensions.StringExtensions.GetLines($this);
                    return lineIndex >= 0 && lineIndex < lines.length ? lines[System.Array.index(lineIndex, lines)] : "";
                },
                GetLines: function ($this) {
                    var linesIndex = Granular.Extensions.StringExtensions.IndexOfAll($this, "\n");

                    var linesStartIndex = System.Linq.Enumerable.from(Bridge.cast(System.Array.init([0], System.Int32), System.Collections.Generic.IEnumerable$1(System.Int32))).concat(System.Linq.Enumerable.from(linesIndex).select($asm.$.Granular.Extensions.StringExtensions.f1));
                    var linesEndIndex = System.Linq.Enumerable.from(Bridge.cast(linesIndex, System.Collections.Generic.IEnumerable$1(System.Int32))).concat(System.Array.init([$this.length], System.Int32)).select(function (x) {{ return Bridge.cast(x, System.Int32); }});

                    return System.Linq.Enumerable.from(linesStartIndex).zip(linesEndIndex, function (lineStartIndex, lineEndIndex) {
                            return System.String.trimEnd($this.substr(lineStartIndex, ((lineEndIndex - lineStartIndex) | 0)), [13]);
                        }).ToArray(System.String);
                },
                IndexOfAll: function ($this, allOf) {
                    var list = new (System.Collections.Generic.List$1(System.Int32)).ctor();

                    for (var i = 0; i < (($this.length - allOf.length) | 0); i = (i + 1) | 0) {
                        var match = true;

                        for (var j = 0; j < allOf.length; j = (j + 1) | 0) {
                            if ($this.charCodeAt(((i + j) | 0)) !== allOf.charCodeAt(j)) {
                                match = false;
                                break;
                            }
                        }

                        if (match) {
                            list.add(i);
                        }
                    }

                    return list.ToArray();
                }
            }
        }
    });

    Bridge.ns("Granular.Extensions.StringExtensions", $asm.$);

    Bridge.apply($asm.$.Granular.Extensions.StringExtensions, {
        f1: function (index) {
            return ((index + 1) | 0);
        }
    });

    Bridge.define("Granular.Extensions.TimeSpanExtensions", {
        statics: {
            methods: {
                Scale: function (timeSpan, factor) {
                    return System.TimeSpan.fromTicks(Bridge.Int.clip64(System.Int64.toNumber(timeSpan.getTicks()) * factor));
                },
                Divide: function ($this, timeSpan) {
                    return $this.getTicks() / System.Int64.toNumber(timeSpan.getTicks());
                },
                Min: function ($this, timeSpan) {
                    return System.TimeSpan.lt($this, timeSpan) ? $this : timeSpan;
                },
                Max: function ($this, timeSpan) {
                    return System.TimeSpan.gt($this, timeSpan) ? $this : timeSpan;
                }
            }
        }
    });

    Bridge.define("Granular.Extensions.TypeExtensions", {
        statics: {
            methods: {
                GetInstanceProperty: function (type, propertyName) {
                    return Bridge.Reflection.getMembers(type, 16, 84 | 256, propertyName);
                },
                GetDefaultIndexProperty: function (type) {
                    return System.Linq.Enumerable.from(Bridge.Reflection.getMembers(type, 16, 28)).firstOrDefault($asm.$.Granular.Extensions.TypeExtensions.f1, null);
                },
                GetInterfaceType: function (type, interfaceGenericType) {
                    return System.Linq.Enumerable.from(Bridge.Reflection.getInterfaces(type)).firstOrDefault(function (interfaceType) {
                            return Bridge.referenceEquals(interfaceType, interfaceGenericType) || Bridge.Reflection.isGenericType(interfaceType) && Bridge.referenceEquals(interfaceGenericType, Bridge.Reflection.getGenericTypeDefinition(interfaceType));
                        }, null);
                }
            }
        }
    });

    Bridge.ns("Granular.Extensions.TypeExtensions", $asm.$);

    Bridge.apply($asm.$.Granular.Extensions.TypeExtensions, {
        f1: function (property) {
            return System.Linq.Enumerable.from((property.ipi || [])).any();
        }
    });

    Bridge.define("Granular.ReentrancyLock", {
        statics: {
            methods: {
                op_Implicit: function (ScopeEntrancyLock) {
                    return ScopeEntrancyLock.IsEntered;
                }
            }
        },
        props: {
            IsEntered: false
        },
        ctors: {
            ctor: function () {
                this.$initialize();
                this.IsEntered = false;
            }
        },
        methods: {
            Enter: function () {
                if (this.IsEntered) {
                    throw new Granular.Exception("Can't enter the scope more than once");
                }

                this.IsEntered = true;
                return new Granular.Disposable(Bridge.fn.bind(this, $asm.$.Granular.ReentrancyLock.f1));
            }
        }
    });

    Bridge.ns("Granular.ReentrancyLock", $asm.$);

    Bridge.apply($asm.$.Granular.ReentrancyLock, {
        f1: function () {
            this.IsEntered = false;
        }
    });

    Bridge.define("System.AssemblyExtensions", {
        statics: {
            methods: {
                GetName: function (assembly) {
                    return new System.AssemblyName(assembly.name);
                },
                GetManifestResourceStream: function (assembly, name) {
                    return new System.IO.MemoryStream.$ctor1(assembly.getManifestResourceData(name));
                }
            }
        }
    });

    Bridge.define("System.AssemblyName", {
        props: {
            Name: null
        },
        ctors: {
            ctor: function (name) {
                this.$initialize();
                this.Name = name;
            }
        }
    });

    Bridge.define("System.Collections.Generic.DictionaryExtensions", {
        statics: {
            methods: {
                Clear: function (TKey, TValue, dictionary) {
                    Bridge.cast(dictionary, System.Collections.Generic.Dictionary$2(TKey,TValue)).clear();
                }
            }
        }
    });

    Bridge.define("System.Collections.Generic.SortedList$2", function (TKey, TValue) { return {
        inherits: [System.Collections.Generic.IDictionary$2(TKey,TValue)],
        statics: {
            fields: {
                DefaultValue: null
            },
            ctors: {
                init: function () {
                    this.DefaultValue = Bridge.getDefaultValue(TValue);
                }
            }
        },
        fields: {
            keys: null,
            readOnlyKeys: null,
            values: null,
            readOnlyValues: null,
            comparer: null
        },
        props: {
            System$Collections$Generic$IDictionary$2$Keys: {
                get: function () {
                    return this.readOnlyKeys;
                }
            },
            System$Collections$Generic$IDictionary$2$Values: {
                get: function () {
                    return this.readOnlyValues;
                }
            },
            Count: {
                get: function () {
                    return this.keys.Count;
                }
            },
            IsReadOnly: {
                get: function () {
                    return false;
                }
            }
        },
        alias: [
            "System$Collections$Generic$IDictionary$2$Keys", "System$Collections$Generic$IDictionary$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$Keys",
            "System$Collections$Generic$IDictionary$2$Values", "System$Collections$Generic$IDictionary$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$Values",
            "getItem", "System$Collections$Generic$IDictionary$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$getItem",
            "setItem", "System$Collections$Generic$IDictionary$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$setItem",
            "Count", "System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$Count",
            "IsReadOnly", "System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$IsReadOnly",
            "containsKey", "System$Collections$Generic$IDictionary$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$containsKey",
            "add$1", "System$Collections$Generic$IDictionary$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$add",
            "remove$1", "System$Collections$Generic$IDictionary$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$remove",
            "tryGetValue", "System$Collections$Generic$IDictionary$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$tryGetValue",
            "clear", "System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$clear",
            "GetEnumerator", ["System$Collections$Generic$IEnumerable$1$System$Collections$Generic$KeyValuePair$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$GetEnumerator", "System$Collections$Generic$IEnumerable$1$GetEnumerator"],
            "remove", "System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$remove",
            "add", "System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$add",
            "copyTo", "System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$copyTo",
            "contains", "System$Collections$Generic$ICollection$1$System$Collections$Generic$KeyValuePair$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$contains"
        ],
        ctors: {
            ctor: function (comparer) {
                this.$initialize();
                this.comparer = comparer;

                this.keys = new (System.Collections.Generic.List$1(TKey)).ctor();
                this.readOnlyKeys = new (System.Collections.ObjectModel.ReadOnlyCollection$1(TKey))(this.keys);

                this.values = new (System.Collections.Generic.List$1(TValue)).ctor();
                this.readOnlyValues = new (System.Collections.ObjectModel.ReadOnlyCollection$1(TValue))(this.values);
            }
        },
        methods: {
            getItem: function (key) {
                var index = { };
                if (!this.FindItem$1(key, index)) {
                    throw new Granular.Exception("Item with key \"{0}\" does not exist", [key]);
                }

                return this.values.getItem(index.v);
            },
            setItem: function (key, value) {
                var index = { };
                if (!this.FindItem$1(key, index)) {
                    this.keys.insert(index.v, key);
                    this.values.insert(index.v, value);
                } else {
                    this.keys.setItem(index.v, key);
                    this.values.setItem(index.v, value);
                }
            },
            containsKey: function (key) {
                var index = { };
                return this.FindItem$1(key, index);
            },
            add$1: function (key, value) {
                var index = { };
                if (this.FindItem$1(key, index)) {
                    throw new Granular.Exception("Item with key \"{0}\" already exists", [key]);
                }

                this.keys.insert(index.v, key);
                this.values.insert(index.v, value);
            },
            add: function (item) {
                this.add$1(item.key, item.value);
            },
            remove$1: function (key) {
                var index = { };
                if (!this.FindItem$1(key, index)) {
                    return false;
                }

                this.keys.removeAt(index.v);
                this.values.removeAt(index.v);
                return true;
            },
            remove: function (item) {
                throw new System.NotImplementedException.ctor();
            },
            RemoveAt: function (index) {
                if (index >= this.Count) {
                    return false;
                }

                this.keys.removeAt(index);
                this.values.removeAt(index);
                return true;
            },
            tryGetValue: function (key, value) {
                var index = { };
                if (!this.FindItem$1(key, index)) {
                    value.v = System.Collections.Generic.SortedList$2(TKey,TValue).DefaultValue;
                    return false;
                }

                value.v = this.values.getItem(index.v);
                return true;
            },
            clear: function () {
                this.keys.clear();
                this.values.clear();
            },
            FindItem$1: function (key, closestIndex) {
                return this.FindItem(key, 0, ((this.keys.Count - 1) | 0), closestIndex);
            },
            FindItem: function (key, firstIndex, lastIndex, closestIndex) {
                if (firstIndex > lastIndex) {
                    closestIndex.v = firstIndex;
                    return false;
                }

                var middleIndex = (Bridge.Int.div((((firstIndex + lastIndex) | 0)), 2)) | 0;
                var compareResult = this.comparer[Bridge.geti(this.comparer, "System$Collections$Generic$IComparer$1$" + Bridge.getTypeAlias(TKey) + "$compare", "System$Collections$Generic$IComparer$1$compare")](key, this.keys.getItem(middleIndex));

                if (compareResult < 0) {
                    return this.FindItem(key, firstIndex, ((middleIndex - 1) | 0), closestIndex);
                }

                if (compareResult > 0) {
                    return this.FindItem(key, ((middleIndex + 1) | 0), lastIndex, closestIndex);
                }

                closestIndex.v = middleIndex;
                return true;
            },
            GetEnumerator: function () {
                return System.Linq.Enumerable.from(this.keys).zip(this.values, function (key, value) {
                        return new (System.Collections.Generic.KeyValuePair$2(TKey,TValue)).$ctor1(key, value);
                    }).GetEnumerator();
            },
            System$Collections$IEnumerable$GetEnumerator: function () {
                return this.GetEnumerator();
            },
            copyTo: function (array, arrayIndex) { },
            contains: function (item) {
                var value = { };
                return this.tryGetValue(item.key, value) && Bridge.equals(item.value, value.v);
            }
        }
    }; });

    Bridge.define("System.Linq.EnumerableExtensions", {
        statics: {
            methods: {
                CopyTo: function (T, source, array, arrayIndex) {
                    var $t;
                    var i = arrayIndex;
                    $t = Bridge.getEnumerator(source, T);
                    try {
                        while ($t.moveNext()) {
                            var item = $t.Current;
                            array[System.Array.index(i, array)] = item;
                            i = (i + 1) | 0;
                        }
                    } finally {
                        if (Bridge.is($t, System.IDisposable)) {
                            $t.System$IDisposable$Dispose();
                        }
                    }}
            }
        }
    });

    Bridge.define("System.Reflection.EventInfoExtensions", {
        statics: {
            methods: {
                GetEventHandlerType: function (eventInfo) {
                    return Function;
                }
            }
        }
    });

    Bridge.define("System.Reflection.PropertyInfoExtensions", {
        statics: {
            methods: {
                GetGetMethod: function (propertyInfo) {
                    return propertyInfo.g;
                },
                GetSetMethod: function (propertyInfo) {
                    return propertyInfo.s;
                },
                IsDelegate: function (propertyInfo) {
                    return !System.String.endsWith(propertyInfo.n, "Type") && Bridge.Reflection.isAssignableFrom(Function, propertyInfo.rt);
                }
            }
        }
    });

    Bridge.define("System.Resources.ResourceSet", {
        inherits: [System.IDisposable],
        statics: {
            fields: {
                Int32Size: 0
            },
            ctors: {
                init: function () {
                    this.Int32Size = 4;
                }
            },
            methods: {
                Read7BitEncodedInt: function (binaryReader) {
                    var value = 0;

                    for (var i = 0; i < 5; i = (i + 1) | 0) {
                        var b = binaryReader.ReadByte();

                        value = value | ((b & 127) << (Bridge.Int.mul(i, 7)));

                        if ((b & 128) === 0) {
                            break;
                        } else if (i === 4) {
                            throw new System.Exception("Can't read int at current position");
                        }
                    }

                    return value;
                }
            }
        },
        fields: {
            memoryStream: null,
            binaryReader: null,
            resourceEntries: null
        },
        alias: ["Dispose", "System$IDisposable$Dispose"],
        ctors: {
            ctor: function (stream) {
                var $t;
                this.$initialize();
                this.memoryStream = new System.IO.MemoryStream.ctor();
                stream.CopyTo(this.memoryStream);

                this.binaryReader = new System.IO.BinaryReader.ctor(this.memoryStream);

                this.memoryStream.Seek(System.Int64(0), 0);

                var header = (this.binaryReader.ReadInt32()) >>> 0;
                if (header !== 3203386062) {
                    throw new Granular.Exception(System.String.format("Invalid ResourceSet header ({0})", [Bridge.box(header, System.UInt32)]));
                }

                var version = this.binaryReader.ReadInt32();
                if (version !== 1) {
                    throw new Granular.Exception(System.String.format("Unsupported ResourceSet version ({0})", [Bridge.box(version, System.Int32)]));
                }

                var bytesToSkip = this.binaryReader.ReadInt32();
                this.memoryStream.Seek(System.Int64(bytesToSkip), 1);

                var version2 = this.binaryReader.ReadInt32();

                var resourcesCount = this.binaryReader.ReadInt32();
                var typesCount = this.binaryReader.ReadInt32();

                Granular.Extensions.BinaryReaderExtensions.SkipStrings(this.binaryReader, typesCount);

                var paddingCount = (7 - System.Int64.clip32((this.memoryStream.Position.add(System.Int64(7)))) % 8) | 0;
                this.memoryStream.Seek(System.Int64(paddingCount), 1);

                this.memoryStream.Seek(System.Int64(Bridge.Int.mul(resourcesCount, System.Resources.ResourceSet.Int32Size)), 1);

                this.memoryStream.Seek(System.Int64(Bridge.Int.mul(resourcesCount, System.Resources.ResourceSet.Int32Size)), 1);

                var dataOrigin = this.binaryReader.ReadInt32();

                this.resourceEntries = new (System.Collections.Generic.List$1(System.Resources.ResourceSet.ResourceEntry)).ctor();

                for (var i = 0; i < resourcesCount; i = (i + 1) | 0) {
                    var name = System.Text.Encoding.Unicode.GetString(System.Text.Encoding.UTF8.GetBytes(($t = this.binaryReader.ReadString(), System.String.toCharArray($t, 0, $t.length))));
                    var offset = this.binaryReader.ReadInt32();

                    this.resourceEntries.add(new System.Resources.ResourceSet.ResourceEntry(name, ((dataOrigin + offset) | 0)));
                }
            }
        },
        methods: {
            Dispose: function () {
                this.binaryReader.Dispose();
                this.memoryStream.Dispose();
            },
            GetObject: function (name) {
                for (var i = 0; i < this.resourceEntries.Count; i = (i + 1) | 0) {
                    var entry = this.resourceEntries.getItem(i);
                    if (!Bridge.referenceEquals(entry.Name, name)) {
                        continue;
                    }

                    this.memoryStream.Seek(System.Int64(entry.Offset), 0);

                    var resourceTypeCode = System.Resources.ResourceSet.Read7BitEncodedInt(this.binaryReader);
                    if (resourceTypeCode !== 33) {
                        throw new Granular.Exception(System.String.format("Unsupported ResourceTypeCode ({0})", [Bridge.box(resourceTypeCode, System.Int32)]));
                    }

                    var length = this.binaryReader.ReadInt32();
                    var buffer = System.Array.init(length, 0, System.Byte);
                    this.memoryStream.Read(buffer, 0, length);

                    return new System.IO.MemoryStream.$ctor1(buffer);
                }

                return null;
            }
        }
    });

    Bridge.define("System.Resources.ResourceSet.ResourceEntry", {
        $kind: "nested class",
        props: {
            Name: null,
            Offset: 0
        },
        ctors: {
            ctor: function (name, offset) {
                this.$initialize();
                this.Name = name;
                this.Offset = offset;
            }
        }
    });

    Bridge.define("System.STAThreadAttribute", {
        inherits: [System.Attribute]
    });

    Bridge.define("System.TypeExtensions", {
        statics: {
            methods: {
                GetIsValueType: function (type) {
                    return !Bridge.Reflection.isClass(type) && !Bridge.Reflection.isInterface(type);
                },
                GetTypeHandle: function (type) {
                    return type;
                }
            }
        }
    });

    Bridge.define("System.UriExtensions", {
        statics: {
            methods: {
                ResolveAbsoluteUri: function (uri, baseUri) {
                    if (System.UriExtensions.GetIsAbsoluteUri(uri)) {
                        return uri;
                    }

                    if (System.Uri.notEquals(baseUri, null) && System.UriExtensions.GetIsAbsoluteUri(baseUri)) {
                        return Granular.Compatibility.Uri.CreateAbsoluteUri$1(baseUri, System.UriExtensions.GetOriginalString(uri));
                    }

                    if (System.Uri.equals(baseUri, null)) {
                        throw new Granular.Exception("Can't resolve absolute uri for \"{0}\", base uri is null", [System.UriExtensions.GetOriginalString(uri)]);
                    }

                    throw new Granular.Exception("Can't resolve absolute uri for \"{0}\", base uri \"{1}\" is not an absolute Uri", [System.UriExtensions.GetOriginalString(uri), System.UriExtensions.GetOriginalString(baseUri)]);
                },
                GetOriginalString: function (uri) {
                    return uri.getAbsoluteUri();
                },
                GetIsAbsoluteUri: function (uri) {
                    var uriComponents = { };
                    return Granular.Compatibility.Uri.TryGetUriComponents(uri, uriComponents);
                },
                GetScheme: function (uri) {
                    return Granular.Compatibility.Uri.GetUriComponents(uri).Scheme;
                },
                GetAbsoluteUri: function (uri) {
                    return Granular.Compatibility.Uri.GetUriComponents(uri).AbsoluteUri;
                },
                GetAbsolutePath: function (uri) {
                    return Granular.Compatibility.Uri.GetUriComponents(uri).AbsolutePath;
                },
                GetLocalPath: function (uri) {
                    return Granular.Compatibility.Uri.GetUriComponents(uri).LocalPath;
                },
                GetSegments: function (uri) {
                    return Granular.Compatibility.Uri.GetUriComponents(uri).Segments;
                },
                GetUserInfo: function (uri) {
                    return Granular.Compatibility.Uri.GetUriComponents(uri).UserInfo;
                },
                GetHost: function (uri) {
                    return Granular.Compatibility.Uri.GetUriComponents(uri).Host;
                },
                GetPort: function (uri) {
                    return Granular.Compatibility.Uri.GetUriComponents(uri).Port;
                },
                GetQuery: function (uri) {
                    return Granular.Compatibility.Uri.GetUriComponents(uri).Query;
                },
                GetFragment: function (uri) {
                    return Granular.Compatibility.Uri.GetUriComponents(uri).Fragment;
                },
                GetIsDefaultPort: function (uri) {
                    return Granular.Compatibility.Uri.GetUriComponents(uri).IsDefaultPort;
                },
                GetIsFile: function (uri) {
                    return Granular.Compatibility.Uri.GetUriComponents(uri).IsFile;
                },
                GetIsUnc: function (uri) {
                    return Granular.Compatibility.Uri.GetUriComponents(uri).IsUnc;
                },
                GetIsLoopback: function (uri) {
                    return Granular.Compatibility.Uri.GetUriComponents(uri).IsLoopback;
                },
                GetPathAndQuery: function (uri) {
                    return Granular.Compatibility.Uri.GetUriComponents(uri).PathAndQuery;
                }
            }
        }
    });

    Bridge.define("System.Windows.Input.ICommand", {
        $kind: "interface"
    });

    Bridge.define("System.Windows.Markup.ITokenDefinition", {
        $kind: "interface"
    });

    Bridge.define("System.Windows.Markup.Lexer", {
        statics: {
            fields: {
                WhiteSpaceRegex: null
            },
            ctors: {
                init: function () {
                    this.WhiteSpaceRegex = new RegExp("^[ \t]+");
                }
            }
        },
        fields: {
            tokensDefinition: null
        },
        ctors: {
            ctor: function (tokensDefinition) {
                if (tokensDefinition === void 0) { tokensDefinition = []; }

                this.$initialize();
                this.tokensDefinition = tokensDefinition;
            }
        },
        methods: {
            GetTokens: function (stream) {
                return new (Bridge.GeneratorEnumerable$1(System.Windows.Markup.Token))(Bridge.fn.bind(this, function (stream) {
                    var $step = 0,
                        $jumpFromFinally,
                        $returnValue,
                        start,
                        matches,
                        selectedToken,
                        $t,
                        tokenDefinition,
                        matchedToken,
                        $async_e;

                    var $enumerator = new (Bridge.GeneratorEnumerator$1(System.Windows.Markup.Token))(Bridge.fn.bind(this, function () {
                        try {
                            for (;;) {
                                switch ($step) {
                                    case 0: {
                                        start = 0;
                                        $step = 1;
                                        continue;
                                    }
                                    case 1: {
                                        if ( start < stream.length ) {
                                                $step = 2;
                                                continue;
                                            } 
                                            $step = 4;
                                            continue;
                                    }
                                    case 2: {
                                        matches = System.Windows.Markup.Lexer.WhiteSpaceRegex.exec(stream.substr(start));

                                            if (matches != null) {
                                                start = (start + matches[System.Array.index(0, matches)].length) | 0;
                                            }

                                            selectedToken = null;

                                            $t = Bridge.getEnumerator(this.tokensDefinition);
                                            try {
                                                while ($t.moveNext()) {
                                                    tokenDefinition = $t.Current;
                                                    matchedToken = tokenDefinition.System$Windows$Markup$ITokenDefinition$Match(stream, start);
                                                    if (matchedToken != null && (selectedToken == null || matchedToken.Value.length > selectedToken.Value.length)) {
                                                        selectedToken = matchedToken;
                                                    }
                                                }
                                            } finally {
                                                if (Bridge.is($t, System.IDisposable)) {
                                                    $t.System$IDisposable$Dispose();
                                                }
                                            }
                                            if (selectedToken == null) {
                                                throw new Granular.Exception("Can't parse \"{0}\" at index {1} ('{2}' is unexpected)", [stream, Bridge.box(start, System.Int32), String.fromCharCode(stream.charCodeAt(start))]);
                                            }

                                            start = (start + selectedToken.Value.length) | 0;

                                            $enumerator.current = selectedToken;
                                            $step = 3;
                                            return true;
                                    }
                                    case 3: {
                                        
                                            $step = 1;
                                            continue;
                                    }
                                    case 4: {

                                    }
                                    default: {
                                        return false;
                                    }
                                }
                            }
                        } catch($async_e1) {
                            $async_e = System.Exception.create($async_e1);
                            throw $async_e;
                        }
                    }));
                    return $enumerator;
                }, arguments));
            }
        }
    });

    Bridge.define("System.Windows.Markup.MarkupCompatibility", {
        statics: {
            fields: {
                NamespaceName: null,
                IgnorableDirective: null
            },
            ctors: {
                init: function () {
                    this.NamespaceName = "http://schemas.openxmlformats.org/markup-compatibility/2006";
                    this.IgnorableDirective = new System.Windows.Markup.XamlName("Ignorable", System.Windows.Markup.MarkupCompatibility.NamespaceName);
                }
            },
            methods: {
                IsDirective: function (namespaceName, localName) {
                    return Bridge.referenceEquals(namespaceName, System.Windows.Markup.MarkupCompatibility.IgnorableDirective.NamespaceName) && Bridge.referenceEquals(localName, System.Windows.Markup.MarkupCompatibility.IgnorableDirective.LocalName);
                }
            }
        }
    });

    Bridge.define("System.Windows.Markup.MarkupExtensionParser", {
        statics: {
            fields: {
                lexer: null
            },
            ctors: {
                init: function () {
                    this.lexer = new System.Windows.Markup.Lexer([new System.Windows.Markup.RegexTokenDefinition(Bridge.box(System.Windows.Markup.MarkupExtensionParser.TokenType.Terminal, System.Windows.Markup.MarkupExtensionParser.TokenType, System.Enum.toStringFn(System.Windows.Markup.MarkupExtensionParser.TokenType)), new RegExp("^[{}=,]")), new System.Windows.Markup.RegexTokenDefinition(Bridge.box(System.Windows.Markup.MarkupExtensionParser.TokenType.Boolean, System.Windows.Markup.MarkupExtensionParser.TokenType, System.Enum.toStringFn(System.Windows.Markup.MarkupExtensionParser.TokenType)), new RegExp("^(true|True|false|False)")), new System.Windows.Markup.RegexTokenDefinition(Bridge.box(System.Windows.Markup.MarkupExtensionParser.TokenType.String, System.Windows.Markup.MarkupExtensionParser.TokenType, System.Enum.toStringFn(System.Windows.Markup.MarkupExtensionParser.TokenType)), new RegExp("^'([^']|'')*'")), new System.Windows.Markup.RegexTokenDefinition(Bridge.box(System.Windows.Markup.MarkupExtensionParser.TokenType.Integer, System.Windows.Markup.MarkupExtensionParser.TokenType, System.Enum.toStringFn(System.Windows.Markup.MarkupExtensionParser.TokenType)), new RegExp("^[0-9]+")), new System.Windows.Markup.RegexTokenDefinition(Bridge.box(System.Windows.Markup.MarkupExtensionParser.TokenType.Decimal, System.Windows.Markup.MarkupExtensionParser.TokenType, System.Enum.toStringFn(System.Windows.Markup.MarkupExtensionParser.TokenType)), new RegExp("^[0-9]*\\.[0-9]+")), new System.Windows.Markup.RegexTokenDefinition(Bridge.box(System.Windows.Markup.MarkupExtensionParser.TokenType.Identifier, System.Windows.Markup.MarkupExtensionParser.TokenType, System.Enum.toStringFn(System.Windows.Markup.MarkupExtensionParser.TokenType)), new RegExp("^[A-Za-z0-9_:\\(\\)\\.]*"))]);
                }
            },
            methods: {
                BooleanParse: function (value) {
                    value = value.toLowerCase();
                    if (System.String.compare(value, "true") === 0 || System.String.compare(value, "false") === 0) {
                        return System.String.compare(value, "true") === 0;
                    }

                    throw new Granular.Exception("Can't parse boolean value \"{0}\"", [value]);
                },
                Parse: function (text, namespaces, sourceUri) {
                    if (sourceUri === void 0) { sourceUri = null; }
                    if (System.Windows.Markup.MarkupExtensionParser.IsEscaped(text)) {
                        return System.Windows.Markup.MarkupExtensionParser.GetEscapedText(text);
                    }

                    if (System.Windows.Markup.MarkupExtensionParser.IsMarkupExtension(text)) {
                        return new System.Windows.Markup.MarkupExtensionParser(text, namespaces, sourceUri).Parse();
                    }

                    return text;
                },
                IsMarkupExtension: function (text) {
                    text = text.trim();
                    return Granular.Compatibility.String.StartsWith(text, 123) && Granular.Compatibility.String.EndsWith(text, 125);
                },
                IsEscaped: function (text) {
                    return Granular.Compatibility.String.StartsWith$1(text, "{}");
                },
                GetEscapedText: function (text) {
                    return text.substr(2);
                }
            }
        },
        fields: {
            text: null,
            namespaces: null,
            sourceUri: null,
            tokens: null
        },
        ctors: {
            ctor: function (text, namespaces, sourceUri) {
                this.$initialize();
                this.text = text;
                this.namespaces = namespaces;
                this.sourceUri = sourceUri;
            }
        },
        methods: {
            Parse: function () {
                this.tokens = new (Granular.Collections.ReadOnlyStack$1(System.Windows.Markup.Token))(System.Windows.Markup.MarkupExtensionParser.lexer.GetTokens(this.text));

                var root = this.MatchElement();

                if (!this.tokens.IsEmpty) {
                    throw new Granular.Exception("Can't parse \"{0}\", end of stream is expected at index {1}", [this.text, Bridge.box(this.tokens.Peek().Start, System.Int32)]);
                }

                return root;
            },
            MatchElement: function () {
                this.VerifyTokensExists();

                this.MatchTerminal("{");
                var typeFullName = this.MatchIdentifier();
                var membersList = this.MatchMembersList();
                this.MatchTerminal("}");

                return new System.Windows.Markup.XamlElement(new System.Windows.Markup.XamlName(this.GetTypeName(typeFullName), this.GetTypeNamespace(typeFullName)), this.namespaces, this.sourceUri, membersList, void 0, void 0);
            },
            MatchMembersList: function () {
                this.VerifyTokensExists();

                var list = new (System.Collections.Generic.List$1(System.Windows.Markup.XamlMember)).ctor();

                if (!Bridge.referenceEquals(this.tokens.Peek().Value, "}")) {
                    list.add(this.MatchMember());
                    list.AddRange(this.MatchMembersListEnd());
                }

                return list;
            },
            MatchMembersListEnd: function () {
                this.VerifyTokensExists();

                var list = new (System.Collections.Generic.List$1(System.Windows.Markup.XamlMember)).ctor();

                if (!Bridge.referenceEquals(this.tokens.Peek().Value, "}")) {
                    this.MatchTerminal(",");

                    list.add(this.MatchMember());
                    list.AddRange(this.MatchMembersListEnd());
                }

                return list;
            },
            MatchMember: function () {
                this.VerifyTokensExists();

                var name = "";
                var value;

                if (System.Nullable.getValue(Bridge.cast(Bridge.unbox(this.tokens.Peek().Id), System.Int32)) === System.Windows.Markup.MarkupExtensionParser.TokenType.Identifier) {
                    var identifier = this.MatchIdentifier();
                    value = this.MatchNamedValue();

                    if (value == null) {
                        value = identifier;
                    } else {
                        name = identifier;
                    }
                } else {
                    value = this.MatchValue();
                }

                return new System.Windows.Markup.XamlMember.$ctor1(new System.Windows.Markup.XamlName(name), this.namespaces, this.sourceUri, value);
            },
            MatchNamedValue: function () {
                this.VerifyTokensExists();

                var token = this.tokens.Peek();
                if (System.Nullable.getValue(Bridge.cast(Bridge.unbox(token.Id), System.Int32)) !== System.Windows.Markup.MarkupExtensionParser.TokenType.Terminal || !Bridge.referenceEquals(token.Value, "=")) {
                    return null;
                }

                this.MatchTerminal("=");

                return this.MatchValue();
            },
            MatchValue: function () {
                this.VerifyTokensExists();

                if (System.Nullable.getValue(Bridge.cast(Bridge.unbox(this.tokens.Peek().Id), System.Int32)) === System.Windows.Markup.MarkupExtensionParser.TokenType.Terminal) {
                    return this.MatchElement();
                }

                var token = this.tokens.Pop();
                var constValue;

                switch (System.Nullable.getValue(Bridge.cast(Bridge.unbox(token.Id), System.Int32))) {
                    case System.Windows.Markup.MarkupExtensionParser.TokenType.Identifier: 
                        constValue = token.Value;
                        break;
                    case System.Windows.Markup.MarkupExtensionParser.TokenType.String: 
                        constValue = System.String.replaceAll(token.Value.substr(1, ((token.Value.length - 2) | 0)), "''", "'");
                        break;
                    case System.Windows.Markup.MarkupExtensionParser.TokenType.Boolean: 
                        constValue = Bridge.box(System.Windows.Markup.MarkupExtensionParser.BooleanParse(token.Value), System.Boolean, System.Boolean.toString);
                        break;
                    case System.Windows.Markup.MarkupExtensionParser.TokenType.Integer: 
                        constValue = Bridge.box(System.Int32.parse(token.Value), System.Int32);
                        break;
                    case System.Windows.Markup.MarkupExtensionParser.TokenType.Decimal: 
                        constValue = Bridge.box(System.Double.parse(token.Value), System.Double, System.Double.format, System.Double.getHashCode);
                        break;
                    default: 
                        throw new Granular.Exception("Can't parse \"{0}\", value is expected, \"{1}\" was found at index {2}", [this.text, token.Value, Bridge.box(token.Start, System.Int32)]);
                }

                return constValue;
            },
            MatchIdentifier: function () {
                this.VerifyTokensExists();

                var token = this.tokens.Pop();

                if (System.Nullable.getValue(Bridge.cast(Bridge.unbox(token.Id), System.Int32)) !== System.Windows.Markup.MarkupExtensionParser.TokenType.Identifier) {
                    throw new Granular.Exception("Can't parse \"{0}\", identifier is expected, \"{1}\" was found at index {2}", [this.text, token.Value, Bridge.box(token.Start, System.Int32)]);
                }

                return token.Value;
            },
            MatchTerminal: function (terminal) {
                this.VerifyTokensExists();

                var token = this.tokens.Pop();

                if (System.Nullable.getValue(Bridge.cast(Bridge.unbox(token.Id), System.Int32)) !== System.Windows.Markup.MarkupExtensionParser.TokenType.Terminal || !Bridge.referenceEquals(token.Value, terminal)) {
                    throw new Granular.Exception("Can't parse \"{0}\", \"{1}\" is expected, \"{2}\" was found at index {3}", [this.text, terminal, token.Value, Bridge.box(token.Start, System.Int32)]);
                }

                return token;
            },
            VerifyTokensExists: function () {
                if (this.tokens.IsEmpty) {
                    throw new Granular.Exception("Can't parse \"{0}\", stream was terminated unexpectedly", [this.text]);
                }
            },
            GetTypeName: function (typeFullName) {
                var namespaceSeparatorIndex = System.String.indexOf(typeFullName, ":");
                return namespaceSeparatorIndex !== -1 ? typeFullName.substr(((namespaceSeparatorIndex + 1) | 0)) : typeFullName;
            },
            GetTypeNamespace: function (typeFullName) {
                var namespaceSeparatorIndex = System.String.indexOf(typeFullName, ":");
                return this.namespaces.GetNamespace(namespaceSeparatorIndex !== -1 ? typeFullName.substr(0, namespaceSeparatorIndex) : "");
            }
        }
    });

    Bridge.define("System.Windows.Markup.MarkupExtensionParser.TokenType", {
        $kind: "nested enum",
        statics: {
            fields: {
                Terminal: 0,
                Identifier: 1,
                String: 2,
                Boolean: 3,
                Integer: 4,
                Decimal: 5
            }
        }
    });

    Bridge.define("System.Windows.Markup.NamespaceDeclaration", {
        props: {
            Prefix: null,
            Namespace: null
        },
        ctors: {
            ctor: function ($namespace) {
                System.Windows.Markup.NamespaceDeclaration.$ctor1.call(this, "", $namespace);
            },
            $ctor1: function (prefix, $namespace) {
                this.$initialize();
                this.Prefix = prefix;
                this.Namespace = $namespace;
            }
        },
        methods: {
            equals: function (obj) {
                var other = Bridge.as(obj, System.Windows.Markup.NamespaceDeclaration);

                return Bridge.referenceEquals(this, other) || !Bridge.referenceEquals(other, null) && Bridge.referenceEquals(this.Prefix, other.Prefix) && Bridge.referenceEquals(this.Namespace, other.Namespace);
            },
            getHashCode: function () {
                return Bridge.getHashCode(this.Prefix) ^ Bridge.getHashCode(this.Prefix);
            }
        }
    });

    Bridge.define("System.Windows.Markup.Token", {
        props: {
            Id: null,
            Value: null,
            Start: 0
        },
        ctors: {
            ctor: function (id, value, start) {
                this.$initialize();
                this.Id = id;
                this.Value = value;
                this.Start = start;
            }
        },
        methods: {
            toString: function () {
                return System.String.format("{0} \"{1}\" ({2})", this.Id, this.Value, Bridge.box(this.Start, System.Int32));
            }
        }
    });

    Bridge.define("System.Windows.Markup.XamlNode", {
        props: {
            Name: null,
            Namespaces: null,
            SourceUri: null
        },
        ctors: {
            ctor: function (name, namespaces, sourceUri) {
                this.$initialize();
                this.Name = name;
                this.Namespaces = namespaces;
                this.SourceUri = sourceUri;
            }
        },
        methods: {
            toString: function () {
                return this.Name.toString();
            }
        }
    });

    Bridge.define("System.Windows.Markup.XamlLanguage", {
        statics: {
            fields: {
                NamespaceName: null,
                ClassDirective: null,
                NameDirective: null,
                KeyDirective: null,
                SharedDirective: null,
                NullTypeName: null,
                TypeTypeName: null
            },
            ctors: {
                init: function () {
                    this.NamespaceName = "http://schemas.microsoft.com/winfx/2006/xaml";
                    this.ClassDirective = new System.Windows.Markup.XamlName("Class", System.Windows.Markup.XamlLanguage.NamespaceName);
                    this.NameDirective = new System.Windows.Markup.XamlName("Name", System.Windows.Markup.XamlLanguage.NamespaceName);
                    this.KeyDirective = new System.Windows.Markup.XamlName("Key", System.Windows.Markup.XamlLanguage.NamespaceName);
                    this.SharedDirective = new System.Windows.Markup.XamlName("Shared", System.Windows.Markup.XamlLanguage.NamespaceName);
                    this.NullTypeName = new System.Windows.Markup.XamlName("Null", System.Windows.Markup.XamlLanguage.NamespaceName);
                    this.TypeTypeName = new System.Windows.Markup.XamlName("Type", System.Windows.Markup.XamlLanguage.NamespaceName);
                }
            },
            methods: {
                IsDirective: function (namespaceName, localName) {
                    return Bridge.referenceEquals(namespaceName, System.Windows.Markup.XamlLanguage.NamespaceName) && (Bridge.referenceEquals(localName, System.Windows.Markup.XamlLanguage.ClassDirective.LocalName) || Bridge.referenceEquals(localName, System.Windows.Markup.XamlLanguage.NameDirective.LocalName) || Bridge.referenceEquals(localName, System.Windows.Markup.XamlLanguage.KeyDirective.LocalName) || Bridge.referenceEquals(localName, System.Windows.Markup.XamlLanguage.SharedDirective.LocalName));
                },
                IsXamlType: function (namespaceName, localName) {
                    return Bridge.referenceEquals(namespaceName, System.Windows.Markup.XamlLanguage.NamespaceName) && (Bridge.referenceEquals(localName, System.Windows.Markup.XamlLanguage.NullTypeName.LocalName) || Bridge.referenceEquals(localName, System.Windows.Markup.XamlLanguage.TypeTypeName.LocalName));
                }
            }
        }
    });

    Bridge.define("System.Windows.Markup.XamlMemberExtensions", {
        statics: {
            methods: {
                GetSingleValue: function (member) {
                    if (!Granular.Compatibility.Linq.Enumerable.Any(System.Object, member.Values)) {
                        throw new Granular.Exception("Member \"{0}\" doesn't have values", [member.Name]);
                    }

                    if (Granular.Compatibility.Linq.Enumerable.Count(System.Object, member.Values) > 1) {
                        throw new Granular.Exception("Member \"{0}\" cannot have multiple values", [member.Name]);
                    }

                    return Granular.Compatibility.Linq.Enumerable.First(System.Object, member.Values);
                }
            }
        }
    });

    Bridge.define("System.Windows.Markup.XamlName", {
        statics: {
            fields: {
                Empty: null
            },
            ctors: {
                init: function () {
                    this.Empty = new System.Windows.Markup.XamlName("");
                }
            },
            methods: {
                FromPrefixedName: function (prefixedName, namespaces) {
                    var typeName = prefixedName;
                    var typeNamespacePrefix = "";

                    var namespaceSeparatorIndex = System.String.indexOf(prefixedName, String.fromCharCode(58));
                    if (namespaceSeparatorIndex !== -1) {
                        typeNamespacePrefix = prefixedName.substr(0, namespaceSeparatorIndex);
                        typeName = prefixedName.substr(((namespaceSeparatorIndex + 1) | 0));
                    }

                    return namespaces.ContainsPrefix(typeNamespacePrefix) ? new System.Windows.Markup.XamlName(typeName, namespaces.GetNamespace(typeNamespacePrefix)) : System.Windows.Markup.XamlName.Empty;
                },
                op_Equality: function (name1, name2) {
                    return Bridge.equals(name1, name2);
                },
                op_Inequality: function (name1, name2) {
                    return !(System.Windows.Markup.XamlName.op_Equality(name1, name2));
                }
            }
        },
        props: {
            LocalName: null,
            NamespaceName: null,
            FullName: null,
            MemberName: null,
            HasContainingTypeName: false,
            ContainingTypeName: null,
            IsEmpty: {
                get: function () {
                    return Granular.Extensions.StringExtensions.IsNullOrEmpty(this.LocalName);
                }
            }
        },
        ctors: {
            ctor: function (localName, namespaceName) {
                if (namespaceName === void 0) { namespaceName = null; }
                var $t;

                this.$initialize();
                this.LocalName = localName;
                this.NamespaceName = ($t = namespaceName, $t != null ? $t : "");

                this.FullName = namespaceName == null ? localName : (namespaceName || "") + ":" + (localName || "");

                var typeSeparatorIndex = System.String.indexOf(localName, String.fromCharCode(46));

                if (typeSeparatorIndex !== -1) {
                    this.MemberName = localName.substr(((typeSeparatorIndex + 1) | 0));
                    this.ContainingTypeName = localName.substr(0, typeSeparatorIndex);

                    this.HasContainingTypeName = true;
                } else {
                    this.MemberName = localName;
                }
            }
        },
        methods: {
            toString: function () {
                return this.FullName;
            },
            getHashCode: function () {
                return Bridge.getHashCode(this.LocalName) ^ Bridge.getHashCode(this.NamespaceName);
            },
            equals: function (obj) {
                var other = Bridge.as(obj, System.Windows.Markup.XamlName);

                return Bridge.referenceEquals(this, other) || !Bridge.referenceEquals(other, null) && Bridge.referenceEquals(this.LocalName, other.LocalName) && Bridge.referenceEquals(this.NamespaceName, other.NamespaceName);
            }
        }
    });

    Bridge.define("System.Windows.Markup.XamlNamespaces", {
        statics: {
            fields: {
                Empty: null
            },
            ctors: {
                init: function () {
                    this.Empty = new System.Windows.Markup.XamlNamespaces.ctor(System.Array.init(0, null, System.Windows.Markup.NamespaceDeclaration));
                }
            }
        },
        fields: {
            items: null
        },
        ctors: {
            $ctor1: function ($namespace) {
                System.Windows.Markup.XamlNamespaces.ctor.call(this, System.Array.init([new System.Windows.Markup.NamespaceDeclaration.ctor($namespace)], System.Windows.Markup.NamespaceDeclaration));
            },
            $ctor2: function (prefix, $namespace) {
                System.Windows.Markup.XamlNamespaces.ctor.call(this, System.Array.init([new System.Windows.Markup.NamespaceDeclaration.$ctor1(prefix, $namespace)], System.Windows.Markup.NamespaceDeclaration));
            },
            ctor: function (items) {
                this.$initialize();
                this.items = items;
            }
        },
        methods: {
            toString: function () {
                var count = Granular.Compatibility.Linq.Enumerable.Count(Bridge.global.System.Windows.Markup.NamespaceDeclaration, this.items);
                return count === 0 ? "XamlNamespaces.Empty" : System.String.format("XamlNamespaces[{0}]", [Bridge.box(count, System.Int32)]);
            },
            ContainsPrefix: function (prefix) {
                return Granular.Compatibility.Linq.Enumerable.Any$1(Bridge.global.System.Windows.Markup.NamespaceDeclaration, this.items, function (item) {
                    return Bridge.referenceEquals(item.Prefix, prefix);
                });
            },
            ContainsNamespace: function ($namespace) {
                return Granular.Compatibility.Linq.Enumerable.Any$1(Bridge.global.System.Windows.Markup.NamespaceDeclaration, this.items, function (item) {
                    return Bridge.referenceEquals(item.Namespace, $namespace);
                });
            },
            GetNamespace: function (prefix) {
                return this.GetNamespaceDeclaration(prefix).Namespace;
            },
            GetNamespaceDeclaration: function (prefix) {
                var namespaceDeclaration = Granular.Compatibility.Linq.Enumerable.FirstOrDefault$1(Bridge.global.System.Windows.Markup.NamespaceDeclaration, this.items, function (item) {
                    return Bridge.referenceEquals(item.Prefix, prefix);
                });

                if (namespaceDeclaration == null) {
                    throw new Granular.Exception("Namespaces doesn't contain a namespace with prefix \"{0}\"", [prefix]);
                }

                return namespaceDeclaration;
            },
            Merge: function (namespaceDeclarations) {
                return Granular.Compatibility.Linq.Enumerable.Any(Bridge.global.System.Windows.Markup.NamespaceDeclaration, namespaceDeclarations) ? new System.Windows.Markup.XamlNamespaces.ctor(Granular.Compatibility.Linq.Enumerable.ToArray(Bridge.global.System.Windows.Markup.NamespaceDeclaration, Granular.Compatibility.Linq.Enumerable.Distinct(Bridge.global.System.Windows.Markup.NamespaceDeclaration, Granular.Compatibility.Linq.Enumerable.Concat(Bridge.global.System.Windows.Markup.NamespaceDeclaration, this.items, namespaceDeclarations)))) : this;
            }
        }
    });

    Bridge.define("System.Windows.Markup.XamlNamespacesExtensions", {
        statics: {
            methods: {
                ContainsDefaultNamespace: function ($this) {
                    return $this.ContainsPrefix("");
                },
                GetDefaultNamespace: function ($this) {
                    return $this.GetNamespace("");
                }
            }
        }
    });

    Bridge.define("System.Windows.Markup.XamlParser", {
        statics: {
            methods: {
                Parse: function (content, sourceUri) {
                    if (sourceUri === void 0) { sourceUri = null; }
                    return System.Windows.Markup.XamlParser.Parse$1(System.Xml.Linq.XDocument.Parse(content).Root, sourceUri);
                },
                Parse$1: function (element, sourceUri) {
                    if (sourceUri === void 0) { sourceUri = null; }
                    var namespaces = new System.Windows.Markup.XamlNamespaces.ctor(System.Windows.Markup.XamlParser.GetNamespaces(element));
                    var ignorableNamespaces = new System.Windows.Markup.XamlNamespaces.ctor(System.Windows.Markup.XamlParser.GetIgnorableNamespaces(element, namespaces));

                    return System.Windows.Markup.XamlParser.CreateXamlElement(element, namespaces, ignorableNamespaces, sourceUri);
                },
                CreateXamlElement: function (element, namespaces, ignorableNamespaces, sourceUri) {
                    return new System.Windows.Markup.XamlElement(new System.Windows.Markup.XamlName(element.Name.LocalName, element.Name.NamespaceName), namespaces, sourceUri, System.Windows.Markup.XamlParser.CreateXamlMembers(element, namespaces, ignorableNamespaces, sourceUri), System.Windows.Markup.XamlParser.CreateValues(element, namespaces, ignorableNamespaces, sourceUri), System.Windows.Markup.XamlParser.CreateDirectives(element, namespaces, ignorableNamespaces, sourceUri));
                },
                CreateXamlMembers: function (element, namespaces, ignorableNamespaces, sourceUri) {
                    var $t;
                    var members = new (System.Collections.Generic.List$1(System.Windows.Markup.XamlMember)).$ctor1(Granular.Compatibility.Linq.Enumerable.Select(Bridge.global.System.Xml.Linq.XAttribute, Bridge.global.System.Windows.Markup.XamlMember, Granular.Compatibility.Linq.Enumerable.Where(Bridge.global.System.Xml.Linq.XAttribute, element.Attributes(), function (attribute) {
                        return !System.Windows.Markup.XamlParser.IsDirective(attribute.Name) && !attribute.IsNamespaceDeclaration && !System.Windows.Markup.XamlParser.IsIgnorableAttribute(attribute) && !System.Windows.Markup.XamlParser.IsIgnorable(attribute.Name, ignorableNamespaces);
                    }), function (attribute) {
                        return System.Windows.Markup.XamlParser.CreateXamlMember(attribute, namespaces, ignorableNamespaces, sourceUri);
                    }));

                    $t = Bridge.getEnumerator(element.Elements());
                    try {
                        while ($t.moveNext()) {
                            var memberElement = $t.Current;
                            if (!System.Windows.Markup.XamlParser.IsMemberName(memberElement.Name)) {
                                continue;
                            }

                            var memberNamespaces = namespaces.Merge(System.Windows.Markup.XamlParser.GetNamespaces(memberElement));
                            var memberIgnorableNamespaces = ignorableNamespaces.Merge(System.Windows.Markup.XamlParser.GetIgnorableNamespaces(memberElement, memberNamespaces));

                            if (System.Windows.Markup.XamlParser.IsIgnorable(memberElement.Name, memberIgnorableNamespaces)) {
                                continue;
                            }

                            members.add(System.Windows.Markup.XamlParser.CreateXamlMember$1(memberElement, memberNamespaces, memberIgnorableNamespaces, sourceUri));
                        }
                    } finally {
                        if (Bridge.is($t, System.IDisposable)) {
                            $t.System$IDisposable$Dispose();
                        }
                    }
                    return members;
                },
                CreateXamlMember: function (attribute, namespaces, ignorableNamespaces, sourceUri) {
                    var name = new System.Windows.Markup.XamlName(attribute.Name.LocalName, Granular.Extensions.StringExtensions.IsNullOrEmpty(attribute.Name.NamespaceName) ? System.Windows.Markup.XamlNamespacesExtensions.GetDefaultNamespace(namespaces) : attribute.Name.NamespaceName);
                    var value = System.Windows.Markup.MarkupExtensionParser.Parse(attribute.Value, namespaces, sourceUri);

                    return new System.Windows.Markup.XamlMember.$ctor1(name, namespaces, sourceUri, value);
                },
                CreateXamlMember$1: function (element, namespaces, ignorableNamespaces, sourceUri) {
                    var name = new System.Windows.Markup.XamlName(element.Name.LocalName, Granular.Extensions.StringExtensions.IsNullOrEmpty(element.Name.NamespaceName) ? System.Windows.Markup.XamlNamespacesExtensions.GetDefaultNamespace(namespaces) : element.Name.NamespaceName);

                    if (Granular.Compatibility.Linq.Enumerable.Any$1(Bridge.global.System.Xml.Linq.XAttribute, element.Attributes(), function (attribute) {
                        return !System.Windows.Markup.XamlParser.IsIgnorable(attribute.Name, ignorableNamespaces);
                    })) {
                        throw new Granular.Exception("Member \"{0}\" cannot contain attributes", [element.Name]);
                    }

                    if (Granular.Compatibility.Linq.Enumerable.Any$1(Bridge.global.System.Xml.Linq.XElement, element.Elements(), function (child) {
                        return !System.Windows.Markup.XamlParser.IsIgnorable(child.Name, ignorableNamespaces) && System.Windows.Markup.XamlParser.IsMemberName(child.Name);
                    })) {
                        throw new Granular.Exception("Member \"{0}\" cannot contain member elements", [element.Name]);
                    }

                    return new System.Windows.Markup.XamlMember.ctor(name, namespaces, sourceUri, System.Windows.Markup.XamlParser.CreateValues(element, namespaces, ignorableNamespaces, sourceUri));
                },
                CreateDirectives: function (element, namespaces, ignorableNamespaces, sourceUri) {
                    var attributeDirectives = Granular.Compatibility.Linq.Enumerable.Select(Bridge.global.System.Xml.Linq.XAttribute, Bridge.global.System.Windows.Markup.XamlMember, Granular.Compatibility.Linq.Enumerable.Where(Bridge.global.System.Xml.Linq.XAttribute, element.Attributes(), $asm.$.System.Windows.Markup.XamlParser.f1), function (attribute) {
                        return System.Windows.Markup.XamlParser.CreateXamlMember(attribute, namespaces, ignorableNamespaces, sourceUri);
                    });
                    var elementDirectives = Granular.Compatibility.Linq.Enumerable.Select(Bridge.global.System.Xml.Linq.XElement, Bridge.global.System.Windows.Markup.XamlMember, Granular.Compatibility.Linq.Enumerable.Where(Bridge.global.System.Xml.Linq.XElement, element.Elements(), $asm.$.System.Windows.Markup.XamlParser.f2), function (child) {
                        return System.Windows.Markup.XamlParser.CreateXamlMember$1(child, namespaces, ignorableNamespaces, sourceUri);
                    });

                    return Granular.Compatibility.Linq.Enumerable.ToArray(Bridge.global.System.Windows.Markup.XamlMember, Granular.Compatibility.Linq.Enumerable.Concat(Bridge.global.System.Windows.Markup.XamlMember, attributeDirectives, elementDirectives));
                },
                CreateValues: function (element, namespaces, ignorableNamespaces, sourceUri) {
                    var $t;
                    var values = new (System.Collections.Generic.List$1(System.Object)).ctor();

                    $t = Bridge.getEnumerator(element.Nodes());
                    try {
                        while ($t.moveNext()) {
                            var value = $t.Current;
                            var valueText = Bridge.as(value, System.Xml.Linq.XText);
                            if (valueText != null) {
                                values.add(valueText.Value.trim());
                            }

                            var valueElement = Bridge.as(value, System.Xml.Linq.XElement);
                            if (valueElement != null) {
                                if (!System.Windows.Markup.XamlParser.IsValueName(valueElement.Name)) {
                                    continue;
                                }

                                var valueNamespaces = namespaces.Merge(System.Windows.Markup.XamlParser.GetNamespaces(valueElement));
                                var valueIgnorableNamespaces = ignorableNamespaces.Merge(System.Windows.Markup.XamlParser.GetIgnorableNamespaces(valueElement, valueNamespaces));

                                if (System.Windows.Markup.XamlParser.IsIgnorable(valueElement.Name, valueIgnorableNamespaces)) {
                                    continue;
                                }

                                values.add(System.Windows.Markup.XamlParser.CreateXamlElement(valueElement, valueNamespaces, valueIgnorableNamespaces, sourceUri));
                            }
                        }
                    } finally {
                        if (Bridge.is($t, System.IDisposable)) {
                            $t.System$IDisposable$Dispose();
                        }
                    }
                    return values;
                },
                IsMemberName: function (name) {
                    return System.String.contains(name.LocalName,".") && !System.Windows.Markup.XamlParser.IsDirective(name);
                },
                IsValueName: function (name) {
                    return !System.String.contains(name.LocalName,".") && !System.Windows.Markup.XamlParser.IsDirective(name);
                },
                IsDirective: function (name) {
                    return System.Windows.Markup.XamlLanguage.IsDirective(name.NamespaceName, name.LocalName);
                },
                IsIgnorable: function (name, ignorableNamespaces) {
                    return ignorableNamespaces.ContainsNamespace(name.NamespaceName);
                },
                IsIgnorableAttribute: function (attribute) {
                    return Bridge.referenceEquals(attribute.Name.NamespaceName, System.Windows.Markup.MarkupCompatibility.IgnorableDirective.NamespaceName) && Bridge.referenceEquals(attribute.Name.LocalName, System.Windows.Markup.MarkupCompatibility.IgnorableDirective.LocalName);
                },
                GetNamespaces: function (element) {
                    return Granular.Compatibility.Linq.Enumerable.ToArray(Bridge.global.System.Windows.Markup.NamespaceDeclaration, Granular.Compatibility.Linq.Enumerable.Select(Bridge.global.System.Xml.Linq.XAttribute, Bridge.global.System.Windows.Markup.NamespaceDeclaration, Granular.Compatibility.Linq.Enumerable.Where(Bridge.global.System.Xml.Linq.XAttribute, element.Attributes(), $asm.$.System.Windows.Markup.XamlParser.f3), $asm.$.System.Windows.Markup.XamlParser.f4));
                },
                GetIgnorableNamespaces: function (element, namespaces) {
                    return Granular.Compatibility.Linq.Enumerable.ToArray(Bridge.global.System.Windows.Markup.NamespaceDeclaration, Granular.Compatibility.Linq.Enumerable.Select(System.String, Bridge.global.System.Windows.Markup.NamespaceDeclaration, Granular.Compatibility.Linq.Enumerable.SelectMany(Bridge.global.System.Xml.Linq.XAttribute, System.String, Granular.Compatibility.Linq.Enumerable.Where(Bridge.global.System.Xml.Linq.XAttribute, element.Attributes(), System.Windows.Markup.XamlParser.IsIgnorableAttribute), $asm.$.System.Windows.Markup.XamlParser.f5), function (prefix) {
                        return namespaces.GetNamespaceDeclaration(prefix);
                    }));
                },
                GetNamespaceDeclarationPrefix: function (attribute) {
                    return Granular.Extensions.StringExtensions.IsNullOrEmpty(attribute.Name.NamespaceName) ? "" : attribute.Name.LocalName;
                }
            }
        }
    });

    Bridge.ns("System.Windows.Markup.XamlParser", $asm.$);

    Bridge.apply($asm.$.System.Windows.Markup.XamlParser, {
        f1: function (attribute) {
            return System.Windows.Markup.XamlParser.IsDirective(attribute.Name);
        },
        f2: function (child) {
            return System.Windows.Markup.XamlParser.IsDirective(child.Name);
        },
        f3: function (attribute) {
            return attribute.IsNamespaceDeclaration;
        },
        f4: function (attribute) {
            return new System.Windows.Markup.NamespaceDeclaration.$ctor1(System.Windows.Markup.XamlParser.GetNamespaceDeclarationPrefix(attribute), attribute.Value);
        },
        f5: function (attribute) {
            return System.String.split(attribute.Value, [32].map(function (i) {{ return String.fromCharCode(i); }}));
        }
    });

    Bridge.define("System.Xml.Linq.NodeExtensions", {
        statics: {
            methods: {
                GetLocalName: function (node) {
                    return node.nodeName.substr(((System.String.indexOf(node.nodeName, String.fromCharCode(58)) + 1) | 0));
                }
            }
        }
    });

    Bridge.define("System.Xml.Linq.XAttribute", {
        fields: {
            node: null
        },
        props: {
            Name: null,
            Value: null,
            IsNamespaceDeclaration: false
        },
        ctors: {
            ctor: function (node) {
                this.$initialize();
                this.node = node;

                var nodeName = node.nodeName;

                if (Bridge.referenceEquals(nodeName, "xmlns")) {
                    this.Name = System.Xml.Linq.XName.Get("", node.namespaceURI);
                    this.IsNamespaceDeclaration = true;
                } else {
                    this.Name = System.Xml.Linq.XName.Get(System.Xml.Linq.NodeExtensions.GetLocalName(node), node.namespaceURI);
                    this.IsNamespaceDeclaration = System.String.startsWith(nodeName, "xmlns:");
                }

                this.Value = node.nodeValue;
            }
        }
    });

    Bridge.define("System.Xml.Linq.XNode");

    Bridge.define("System.Xml.Linq.XName", {
        statics: {
            methods: {
                Get: function (localName, namespaceName) {
                    return new System.Xml.Linq.XName(localName, namespaceName);
                }
            }
        },
        props: {
            LocalName: null,
            NamespaceName: null
        },
        ctors: {
            ctor: function (localName, namespaceName) {
                this.$initialize();
                this.LocalName = localName;
                this.NamespaceName = namespaceName;
            }
        },
        methods: {
            toString: function () {
                return System.String.isNullOrEmpty(this.NamespaceName) ? this.LocalName : System.String.format("{{{0}}}{1}", this.NamespaceName, this.LocalName);
            }
        }
    });

    Bridge.define("Granular.Collections.ConvertedStringDictionary$2", function (TKey, TValue) { return {
        inherits: [Granular.Collections.IMinimalDictionary$2(TKey,TValue),Granular.Collections.IMinimalDictionary],
        statics: {
            fields: {
                DefaultValue: null
            },
            ctors: {
                init: function () {
                    this.DefaultValue = Bridge.getDefaultValue(TValue);
                }
            }
        },
        fields: {
            keys: null,
            values: null,
            getStringKey: null
        },
        alias: [
            "Add", "Granular$Collections$IMinimalDictionary$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$Add",
            "ContainsKey", "Granular$Collections$IMinimalDictionary$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$ContainsKey",
            "Remove", "Granular$Collections$IMinimalDictionary$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$Remove",
            "TryGetValue", "Granular$Collections$IMinimalDictionary$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$TryGetValue",
            "Clear", "Granular$Collections$IMinimalDictionary$Clear",
            "Clear", "Granular$Collections$IMinimalDictionary$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$Clear",
            "GetKeys", "Granular$Collections$IMinimalDictionary$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$GetKeys",
            "GetValues", "Granular$Collections$IMinimalDictionary$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$GetValues"
        ],
        ctors: {
            ctor: function (getStringKey) {
                if (getStringKey === void 0) { getStringKey = null; }

                this.$initialize();
                this.keys = new Granular.Compatibility.StringDictionary();
                this.values = new Granular.Compatibility.StringDictionary();
                this.getStringKey = getStringKey || ($asm.$.Granular.Collections.ConvertedStringDictionary$2.f1);
            }
        },
        methods: {
            Add: function (key, value) {
                var stringKey = this.getStringKey(key);

                this.keys.Granular$Collections$IMinimalDictionary$2$System$String$System$Object$Add(stringKey, key);
                this.values.Granular$Collections$IMinimalDictionary$2$System$String$System$Object$Add(stringKey, value);
            },
            Granular$Collections$IMinimalDictionary$Add: function (key, value) {
                var stringKey = this.getStringKey(key);

                this.keys.Granular$Collections$IMinimalDictionary$2$System$String$System$Object$Add(stringKey, key);
                this.values.Granular$Collections$IMinimalDictionary$2$System$String$System$Object$Add(stringKey, value);
            },
            ContainsKey: function (key) {
                return this.keys.Granular$Collections$IMinimalDictionary$2$System$String$System$Object$ContainsKey(this.getStringKey(key));
            },
            Granular$Collections$IMinimalDictionary$ContainsKey: function (key) {
                return this.keys.Granular$Collections$IMinimalDictionary$2$System$String$System$Object$ContainsKey(this.getStringKey(key));
            },
            Remove: function (key) {
                var stringKey = this.getStringKey(key);

                return this.keys.Granular$Collections$IMinimalDictionary$2$System$String$System$Object$Remove(stringKey) && this.values.Granular$Collections$IMinimalDictionary$2$System$String$System$Object$Remove(stringKey);
            },
            Granular$Collections$IMinimalDictionary$Remove: function (key) {
                var stringKey = this.getStringKey(key);

                return this.keys.Granular$Collections$IMinimalDictionary$2$System$String$System$Object$Remove(stringKey) && this.values.Granular$Collections$IMinimalDictionary$2$System$String$System$Object$Remove(stringKey);
            },
            TryGetValue: function (key, value) {
                var result = { };
                if (this.values.Granular$Collections$IMinimalDictionary$2$System$String$System$Object$TryGetValue(this.getStringKey(key), result)) {
                    value.v = result.v;
                    return true;
                }

                value.v = Granular.Collections.ConvertedStringDictionary$2(TKey,TValue).DefaultValue;
                return false;
            },
            Granular$Collections$IMinimalDictionary$TryGetValue: function (key, value) {
                return this.values.Granular$Collections$IMinimalDictionary$2$System$String$System$Object$TryGetValue(this.getStringKey(key), value);
            },
            Clear: function () {
                this.keys.Granular$Collections$IMinimalDictionary$2$System$String$System$Object$Clear();
                this.values.Granular$Collections$IMinimalDictionary$2$System$String$System$Object$Clear();
            },
            GetKeys: function () {
                return Granular.Compatibility.Linq.Enumerable.Cast(TKey, this.keys.Granular$Collections$IMinimalDictionary$2$System$String$System$Object$GetValues());
            },
            Granular$Collections$IMinimalDictionary$GetKeys: function () {
                return this.keys.Granular$Collections$IMinimalDictionary$2$System$String$System$Object$GetValues();
            },
            GetValues: function () {
                return Granular.Compatibility.Linq.Enumerable.Cast(TValue, this.values.Granular$Collections$IMinimalDictionary$2$System$String$System$Object$GetValues());
            },
            Granular$Collections$IMinimalDictionary$GetValues: function () {
                return this.values.Granular$Collections$IMinimalDictionary$2$System$String$System$Object$GetValues();
            },
            GetKeyValuePairs: function () {
                var $t;
                var stringKeys = Granular.Compatibility.Linq.Enumerable.ToArray(System.String, this.keys.Granular$Collections$IMinimalDictionary$2$System$String$System$Object$GetKeys());

                var pairs = System.Array.init(stringKeys.length, function (){
                    return new (System.Collections.Generic.KeyValuePair$2(TKey,TValue))();
                }, System.Collections.Generic.KeyValuePair$2(TKey,TValue));

                var i = 0;
                $t = Bridge.getEnumerator(stringKeys);
                try {
                    while ($t.moveNext()) {
                        var stringKey = $t.Current;
                        pairs[System.Array.index(i, pairs)] = new (System.Collections.Generic.KeyValuePair$2(TKey,TValue)).$ctor1(Granular.Collections.MinimalDictionaryExtensions.GetValue(System.String, System.Object, this.keys, stringKey), Granular.Collections.MinimalDictionaryExtensions.GetValue(System.String, System.Object, this.values, stringKey));
                        i = (i + 1) | 0;
                    }
                } finally {
                    if (Bridge.is($t, System.IDisposable)) {
                        $t.System$IDisposable$Dispose();
                    }
                }
                return pairs;
            }
        }
    }; });

    Bridge.ns("Granular.Collections.ConvertedStringDictionary$2", $asm.$);

    Bridge.apply($asm.$.Granular.Collections.ConvertedStringDictionary$2, {
        f1: function (key) {
            return Bridge.toString(key);
        }
    });

    Bridge.define("Granular.Collections.ConvertedStringSet$1", function (T) { return {
        inherits: [Granular.Collections.IMinimalSet$1(T),Granular.Collections.IMinimalSet],
        fields: {
            items: null,
            getStringItem: null
        },
        alias: [
            "Add", "Granular$Collections$IMinimalSet$1$" + Bridge.getTypeAlias(T) + "$Add",
            "Contains", "Granular$Collections$IMinimalSet$1$" + Bridge.getTypeAlias(T) + "$Contains",
            "Remove", "Granular$Collections$IMinimalSet$1$" + Bridge.getTypeAlias(T) + "$Remove",
            "Clear", "Granular$Collections$IMinimalSet$Clear",
            "Clear", "Granular$Collections$IMinimalSet$1$" + Bridge.getTypeAlias(T) + "$Clear",
            "GetValues", "Granular$Collections$IMinimalSet$1$" + Bridge.getTypeAlias(T) + "$GetValues"
        ],
        ctors: {
            ctor: function (getStringItem) {
                if (getStringItem === void 0) { getStringItem = null; }

                this.$initialize();
                this.items = new Granular.Compatibility.StringDictionary();
                this.getStringItem = getStringItem || ($asm.$.Granular.Collections.ConvertedStringSet$1.f1);
            }
        },
        methods: {
            Add: function (item) {
                var stringItem = this.getStringItem(item);

                if (this.items.Granular$Collections$IMinimalDictionary$2$System$String$System$Object$ContainsKey(stringItem)) {
                    return false;
                }

                this.items.Granular$Collections$IMinimalDictionary$2$System$String$System$Object$Add(stringItem, item);
                return true;
            },
            Granular$Collections$IMinimalSet$Add: function (item) {
                var stringItem = this.getStringItem(item);

                if (this.items.Granular$Collections$IMinimalDictionary$2$System$String$System$Object$ContainsKey(stringItem)) {
                    return false;
                }

                this.items.Granular$Collections$IMinimalDictionary$2$System$String$System$Object$Add(stringItem, item);
                return true;
            },
            Contains: function (item) {
                return this.items.Granular$Collections$IMinimalDictionary$2$System$String$System$Object$ContainsKey(this.getStringItem(item));
            },
            Granular$Collections$IMinimalSet$Contains: function (item) {
                return this.items.Granular$Collections$IMinimalDictionary$2$System$String$System$Object$ContainsKey(this.getStringItem(item));
            },
            Remove: function (item) {
                var stringItem = this.getStringItem(item);

                if (!this.items.Granular$Collections$IMinimalDictionary$2$System$String$System$Object$ContainsKey(stringItem)) {
                    return false;
                }

                this.items.Granular$Collections$IMinimalDictionary$2$System$String$System$Object$Remove(stringItem);
                return true;
            },
            Granular$Collections$IMinimalSet$Remove: function (item) {
                var stringItem = this.getStringItem(item);

                if (!this.items.Granular$Collections$IMinimalDictionary$2$System$String$System$Object$ContainsKey(stringItem)) {
                    return false;
                }

                this.items.Granular$Collections$IMinimalDictionary$2$System$String$System$Object$Remove(stringItem);
                return true;
            },
            Clear: function () {
                this.items.Granular$Collections$IMinimalDictionary$2$System$String$System$Object$Clear();
            },
            GetValues: function () {
                return System.Linq.Enumerable.from(this.items.Granular$Collections$IMinimalDictionary$2$System$String$System$Object$GetValues()).select(function (x) { return Bridge.cast(x, T); });
            },
            Granular$Collections$IMinimalSet$GetValues: function () {
                return this.items.Granular$Collections$IMinimalDictionary$2$System$String$System$Object$GetValues();
            }
        }
    }; });

    Bridge.ns("Granular.Collections.ConvertedStringSet$1", $asm.$);

    Bridge.apply($asm.$.Granular.Collections.ConvertedStringSet$1, {
        f1: function (item) {
            return Bridge.toString(item);
        }
    });

    Bridge.definei("Granular.Collections.IObservableCollection$1", function (T) { return {
        inherits: [System.Collections.Generic.IEnumerable$1(T),Granular.Collections.INotifyCollectionChanged],
        $kind: "interface"
    }; });

    Bridge.define("Granular.Collections.ListDictionary$2", function (TKey, TValue) { return {
        inherits: [Granular.Collections.IListDictionary$2(TKey,TValue)],
        fields: {
            dictionary: null
        },
        props: {
            Granular$Collections$IListDictionary$2$Keys: {
                get: function () {
                    return this.dictionary.getKeys();
                }
            },
            Granular$Collections$IListDictionary$2$Values: {
                get: function () {
                    return System.Linq.Enumerable.from(this.dictionary).selectMany($asm.$.Granular.Collections.ListDictionary$2.f1);
                }
            }
        },
        alias: [
            "Granular$Collections$IListDictionary$2$Keys", "Granular$Collections$IListDictionary$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$Keys",
            "Granular$Collections$IListDictionary$2$Values", "Granular$Collections$IListDictionary$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$Values",
            "Add", "Granular$Collections$IListDictionary$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$Add",
            "Remove", "Granular$Collections$IListDictionary$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$Remove",
            "GetValues", "Granular$Collections$IListDictionary$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$GetValues",
            "GetEnumerator", ["System$Collections$Generic$IEnumerable$1$System$Collections$Generic$KeyValuePair$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$GetEnumerator", "System$Collections$Generic$IEnumerable$1$GetEnumerator"]
        ],
        ctors: {
            ctor: function () {
                this.$initialize();
                this.dictionary = new (System.Collections.Generic.Dictionary$2(TKey,System.Collections.Generic.List$1(TValue)))();
            }
        },
        methods: {
            Add: function (key, value) {
                var list = { };

                if (!this.dictionary.tryGetValue(key, list)) {
                    list.v = new (System.Collections.Generic.List$1(TValue)).ctor();
                    this.dictionary.add(key, list.v);
                }

                list.v.add(value);
            },
            Remove: function (key, value) {
                var list = { };

                if (!this.dictionary.tryGetValue(key, list)) {
                    return false;
                }

                if (!list.v.remove(value)) {
                    return false;
                }

                if (list.v.Count === 0) {
                    this.dictionary.remove(key);
                }

                return true;
            },
            GetValues: function (key) {
                return this.dictionary.containsKey(key) ? Bridge.cast(this.dictionary.get(key), System.Collections.Generic.IEnumerable$1(TValue)) : System.Array.init(0, function (){
                    return Bridge.getDefaultValue(TValue);
                }, TValue);
            },
            GetEnumerator: function () {
                return System.Linq.Enumerable.from(this.dictionary).selectMany(function (pair) {
                        return System.Linq.Enumerable.from(pair.value).select(function (value) {
                                return new (System.Collections.Generic.KeyValuePair$2(TKey,TValue)).$ctor1(pair.key, value);
                            });
                    }).GetEnumerator();
            },
            System$Collections$IEnumerable$GetEnumerator: function () {
                return this.GetEnumerator();
            }
        }
    }; });

    Bridge.ns("Granular.Collections.ListDictionary$2", $asm.$);

    Bridge.apply($asm.$.Granular.Collections.ListDictionary$2, {
        f1: function (pair) {
            return pair.value;
        }
    });

    Bridge.define("Granular.Collections.MinimalDictionary$2", function (TKey, TValue) { return {
        inherits: [Granular.Collections.IMinimalDictionary$2(TKey,TValue),Granular.Collections.IMinimalDictionary],
        statics: {
            fields: {
                DefaultValue: null
            },
            ctors: {
                init: function () {
                    this.DefaultValue = Bridge.getDefaultValue(TValue);
                }
            }
        },
        fields: {
            dictionary: null
        },
        alias: [
            "Add", "Granular$Collections$IMinimalDictionary$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$Add",
            "ContainsKey", "Granular$Collections$IMinimalDictionary$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$ContainsKey",
            "Remove", "Granular$Collections$IMinimalDictionary$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$Remove",
            "TryGetValue", "Granular$Collections$IMinimalDictionary$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$TryGetValue",
            "Clear", "Granular$Collections$IMinimalDictionary$Clear",
            "Clear", "Granular$Collections$IMinimalDictionary$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$Clear",
            "GetKeys", "Granular$Collections$IMinimalDictionary$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$GetKeys",
            "GetValues", "Granular$Collections$IMinimalDictionary$2$" + Bridge.getTypeAlias(TKey) + "$" + Bridge.getTypeAlias(TValue) + "$GetValues"
        ],
        ctors: {
            ctor: function (comparer) {
                if (comparer === void 0) { comparer = null; }

                this.$initialize();
                this.dictionary = new (System.Collections.Generic.Dictionary$2(System.Object, System.Object))(null, Bridge.cast(comparer, System.Collections.Generic.IEqualityComparer$1(System.Object)));
            }
        },
        methods: {
            Add: function (key, value) {
                this.dictionary.add(key, value);
            },
            Granular$Collections$IMinimalDictionary$Add: function (key, value) {
                this.dictionary.add(key, value);
            },
            ContainsKey: function (key) {
                return this.dictionary.containsKey(key);
            },
            Granular$Collections$IMinimalDictionary$ContainsKey: function (key) {
                return this.dictionary.containsKey(key);
            },
            Remove: function (key) {
                return this.dictionary.remove(key);
            },
            Granular$Collections$IMinimalDictionary$Remove: function (key) {
                return this.dictionary.remove(key);
            },
            TryGetValue: function (key, value) {
                var result = { };
                if (this.dictionary.tryGetValue(key, result)) {
                    value.v = result.v;
                    return true;
                }

                value.v = Granular.Collections.MinimalDictionary$2(TKey,TValue).DefaultValue;
                return false;
            },
            Granular$Collections$IMinimalDictionary$TryGetValue: function (key, value) {
                return this.dictionary.tryGetValue(key, value);
            },
            Clear: function () {
                this.dictionary.clear();
            },
            GetKeys: function () {
                return System.Linq.Enumerable.from(Bridge.cast(this.dictionary, System.Collections.Generic.IDictionary$2(System.Object,System.Object)).System$Collections$Generic$IDictionary$2$System$Object$System$Object$Keys).select(function (x) { return Bridge.cast(x, TKey); });
            },
            Granular$Collections$IMinimalDictionary$GetKeys: function () {
                return Bridge.cast(this.dictionary, System.Collections.Generic.IDictionary$2(System.Object,System.Object)).System$Collections$Generic$IDictionary$2$System$Object$System$Object$Keys;
            },
            GetValues: function () {
                return System.Linq.Enumerable.from(Bridge.cast(this.dictionary, System.Collections.Generic.IDictionary$2(System.Object,System.Object)).System$Collections$Generic$IDictionary$2$System$Object$System$Object$Values).select(function (x) { return Bridge.cast(x, TValue); });
            },
            Granular$Collections$IMinimalDictionary$GetValues: function () {
                return Bridge.cast(this.dictionary, System.Collections.Generic.IDictionary$2(System.Object,System.Object)).System$Collections$Generic$IDictionary$2$System$Object$System$Object$Values;
            }
        }
    }; });

    Bridge.define("Granular.Collections.MinimalSet$1", function (TValue) { return {
        inherits: [Granular.Collections.IMinimalSet$1(TValue),Granular.Collections.IMinimalSet],
        fields: {
            set: null
        },
        alias: [
            "Add", "Granular$Collections$IMinimalSet$1$" + Bridge.getTypeAlias(TValue) + "$Add",
            "Contains", "Granular$Collections$IMinimalSet$1$" + Bridge.getTypeAlias(TValue) + "$Contains",
            "Remove", "Granular$Collections$IMinimalSet$1$" + Bridge.getTypeAlias(TValue) + "$Remove",
            "Clear", "Granular$Collections$IMinimalSet$Clear",
            "Clear", "Granular$Collections$IMinimalSet$1$" + Bridge.getTypeAlias(TValue) + "$Clear",
            "GetValues", "Granular$Collections$IMinimalSet$1$" + Bridge.getTypeAlias(TValue) + "$GetValues"
        ],
        ctors: {
            ctor: function (comparer) {
                if (comparer === void 0) { comparer = null; }

                this.$initialize();
                this.set = new (System.Collections.Generic.HashSet$1(System.Object)).$ctor3(Bridge.cast(comparer, System.Collections.Generic.IEqualityComparer$1(System.Object)));
            }
        },
        methods: {
            Add: function (item) {
                if (this.set.contains(item)) {
                    return false;
                }

                this.set.add(item);
                return true;
            },
            Granular$Collections$IMinimalSet$Add: function (item) {
                if (this.set.contains(item)) {
                    return false;
                }

                this.set.add(item);
                return true;
            },
            Contains: function (item) {
                return this.set.contains(item);
            },
            Granular$Collections$IMinimalSet$Contains: function (item) {
                return this.set.contains(item);
            },
            Remove: function (item) {
                return this.set.remove(item);
            },
            Granular$Collections$IMinimalSet$Remove: function (item) {
                return this.set.remove(item);
            },
            Clear: function () {
                this.set.clear();
            },
            GetValues: function () {
                return System.Linq.Enumerable.from(this.set).select(function (x) { return Bridge.cast(x, TValue); });
            },
            Granular$Collections$IMinimalSet$GetValues: function () {
                return this.set;
            }
        }
    }; });

    Bridge.define("Granular.Compatibility.StringDictionary", {
        inherits: [Granular.Collections.IMinimalDictionary$2(System.String,System.Object)],
        statics: {
            ctors: {
                ctor: function () {
                    if (!Object.values) {
                        Object.values = function (obj) {
                            return Object.keys(obj).map(function (key) { return obj[key]; });
                        }
                    }
                }
            }
        },
        fields: {
            dictionary: null
        },
        alias: [
            "Add", "Granular$Collections$IMinimalDictionary$2$System$String$System$Object$Add",
            "ContainsKey", "Granular$Collections$IMinimalDictionary$2$System$String$System$Object$ContainsKey",
            "Remove", "Granular$Collections$IMinimalDictionary$2$System$String$System$Object$Remove",
            "TryGetValue", "Granular$Collections$IMinimalDictionary$2$System$String$System$Object$TryGetValue",
            "Clear", "Granular$Collections$IMinimalDictionary$2$System$String$System$Object$Clear",
            "GetKeys", "Granular$Collections$IMinimalDictionary$2$System$String$System$Object$GetKeys",
            "GetValues", "Granular$Collections$IMinimalDictionary$2$System$String$System$Object$GetValues"
        ],
        ctors: {
            ctor: function () {
                this.$initialize();
                this.dictionary = { };
            }
        },
        methods: {
            Add: function (key, value) {
                if (!(this.dictionary[key] === undefined)) {
                    throw new Granular.Exception("An item with the same key has already been added.");
                }

                this.dictionary[key] = value;
            },
            ContainsKey: function (key) {
                return !(this.dictionary[key] === undefined);
            },
            Remove: function (key) {
                if ((this.dictionary[key] === undefined)) {
                    return false;
                }

                delete this.dictionary[key];
                return true;
            },
            TryGetValue: function (key, value) {
                value.v = this.dictionary[key];

                if ((value.v === undefined)) {
                    value.v = null;
                    return false;
                }

                return true;
            },
            Clear: function () {
                this.dictionary = { };
            },
            GetKeys: function () {
                return Object.keys(this.dictionary);
            },
            GetValues: function () {
                return Object.values(this.dictionary);
            }
        }
    });

    Bridge.define("System.Windows.Markup.RegexTokenDefinition", {
        inherits: [System.Windows.Markup.ITokenDefinition],
        fields: {
            id: null,
            regex: null
        },
        alias: ["Match", "System$Windows$Markup$ITokenDefinition$Match"],
        ctors: {
            ctor: function (id, regex) {
                this.$initialize();
                this.id = id;
                this.regex = regex;
            }
        },
        methods: {
            Match: function (stream, start) {
                var matches = this.regex.exec(stream.substr(start));

                if (matches != null) {
                    return new System.Windows.Markup.Token(this.id, matches[System.Array.index(0, matches)], start);
                }

                return null;
            }
        }
    });

    Bridge.define("System.Windows.Markup.XamlElement", {
        inherits: [System.Windows.Markup.XamlNode],
        statics: {
            fields: {
                EmptyMembers: null,
                EmptyValues: null,
                EmptyDirectives: null
            },
            ctors: {
                init: function () {
                    this.EmptyMembers = System.Array.init(0, null, System.Windows.Markup.XamlMember);
                    this.EmptyValues = System.Array.init(0, null, System.Object);
                    this.EmptyDirectives = System.Array.init(0, null, System.Windows.Markup.XamlMember);
                }
            }
        },
        props: {
            Members: null,
            Values: null,
            Directives: null
        },
        ctors: {
            ctor: function (name, namespaces, sourceUri, members, values, directives) {
                if (members === void 0) { members = null; }
                if (values === void 0) { values = null; }
                if (directives === void 0) { directives = null; }

                this.$initialize();
                System.Windows.Markup.XamlNode.ctor.call(this, name, namespaces, sourceUri);
                this.Members = members || System.Windows.Markup.XamlElement.EmptyMembers;
                this.Values = values || System.Windows.Markup.XamlElement.EmptyValues;
                this.Directives = directives || System.Windows.Markup.XamlElement.EmptyDirectives;
            }
        }
    });

    Bridge.define("System.Windows.Markup.XamlMember", {
        inherits: [System.Windows.Markup.XamlNode],
        statics: {
            fields: {
                EmptyValues: null
            },
            ctors: {
                init: function () {
                    this.EmptyValues = System.Array.init(0, null, System.Object);
                }
            }
        },
        props: {
            Values: null
        },
        ctors: {
            $ctor1: function (name, namespaces, sourceUri, value) {
                System.Windows.Markup.XamlMember.ctor.call(this, name, namespaces, sourceUri, System.Array.init([value], System.Object));
            },
            ctor: function (name, namespaces, sourceUri, values) {
                this.$initialize();
                System.Windows.Markup.XamlNode.ctor.call(this, name, namespaces, sourceUri);
                this.Values = values || System.Windows.Markup.XamlMember.EmptyValues;
            }
        },
        methods: {
            toString: function () {
                return Granular.Compatibility.Linq.Enumerable.Count(System.Object, this.Values) === 1 ? System.String.format("{0}={1}", System.Windows.Markup.XamlNode.prototype.toString.call(this), Bridge.toString(Granular.Compatibility.Linq.Enumerable.First(System.Object, this.Values))) : System.Windows.Markup.XamlNode.prototype.toString.call(this);
            }
        }
    });

    Bridge.define("System.Xml.Linq.XContainer", {
        inherits: [System.Xml.Linq.XNode],
        fields: {
            nodes: null,
            elements: null
        },
        ctors: {
            ctor: function (node) {
                this.$initialize();
                System.Xml.Linq.XNode.ctor.call(this);
                this.nodes = System.Array.init(0, null, System.Xml.Linq.XNode);
                this.elements = System.Array.init(0, null, System.Xml.Linq.XElement);

                for (var i = 0; i < node.childNodes.length; i = (i + 1) | 0) {
                    var childNode = node.childNodes[i];

                    if (childNode.nodeType === 1) {
                        var childElement = new System.Xml.Linq.XElement(Bridge.cast(childNode, Element));
                        this.elements.push(childElement);
                        this.nodes.push(childElement);
                    }

                    if (childNode.nodeType === 3 && !Granular.Extensions.StringExtensions.IsNullOrWhiteSpace(childNode.nodeValue)) {
                        var childText = new System.Xml.Linq.XText(childNode);
                        this.nodes.push(childText);
                    }
                }
            }
        },
        methods: {
            Nodes: function () {
                return this.nodes;
            },
            Elements: function () {
                return this.elements;
            }
        }
    });

    Bridge.define("System.Xml.Linq.XText", {
        inherits: [System.Xml.Linq.XNode],
        fields: {
            node: null
        },
        props: {
            Value: {
                get: function () {
                    return this.node.nodeValue;
                }
            }
        },
        ctors: {
            ctor: function (node) {
                this.$initialize();
                System.Xml.Linq.XNode.ctor.call(this);
                this.node = node;
            }
        }
    });

    Bridge.define("Granular.Collections.ICollectionView", {
        inherits: [Granular.Collections.IObservableCollection$1(System.Object)],
        $kind: "interface"
    });

    Bridge.define("Granular.Collections.ObservableCollection$1", function (T) { return {
        inherits: [Granular.Collections.IObservableCollection$1(T),System.Collections.Generic.IList$1(T),System.ComponentModel.INotifyPropertyChanged],
        statics: {
            fields: {
                CountPropertyChangedEventArgs: null
            },
            ctors: {
                init: function () {
                    this.CountPropertyChangedEventArgs = new System.ComponentModel.PropertyChangedEventArgs("Count");
                }
            }
        },
        fields: {
            items: null
        },
        events: {
            CollectionChanged: null,
            PropertyChanged: null
        },
        props: {
            Count: {
                get: function () {
                    return this.items.Count;
                }
            },
            IsReadOnly: {
                get: function () {
                    return false;
                }
            }
        },
        alias: [
            "addCollectionChanged", "Granular$Collections$INotifyCollectionChanged$addCollectionChanged",
            "removeCollectionChanged", "Granular$Collections$INotifyCollectionChanged$removeCollectionChanged",
            "addPropertyChanged", "System$ComponentModel$INotifyPropertyChanged$addPropertyChanged",
            "removePropertyChanged", "System$ComponentModel$INotifyPropertyChanged$removePropertyChanged",
            "Count", "System$Collections$Generic$ICollection$1$" + Bridge.getTypeAlias(T) + "$Count",
            "getItem", "System$Collections$Generic$IList$1$" + Bridge.getTypeAlias(T) + "$getItem",
            "setItem", "System$Collections$Generic$IList$1$" + Bridge.getTypeAlias(T) + "$setItem",
            "IsReadOnly", "System$Collections$Generic$ICollection$1$" + Bridge.getTypeAlias(T) + "$IsReadOnly",
            "add", "System$Collections$Generic$ICollection$1$" + Bridge.getTypeAlias(T) + "$add",
            "clear", "System$Collections$Generic$ICollection$1$" + Bridge.getTypeAlias(T) + "$clear",
            "contains", "System$Collections$Generic$ICollection$1$" + Bridge.getTypeAlias(T) + "$contains",
            "indexOf", "System$Collections$Generic$IList$1$" + Bridge.getTypeAlias(T) + "$indexOf",
            "insert", "System$Collections$Generic$IList$1$" + Bridge.getTypeAlias(T) + "$insert",
            "remove", "System$Collections$Generic$ICollection$1$" + Bridge.getTypeAlias(T) + "$remove",
            "removeAt", "System$Collections$Generic$IList$1$" + Bridge.getTypeAlias(T) + "$removeAt",
            "copyTo", "System$Collections$Generic$ICollection$1$" + Bridge.getTypeAlias(T) + "$copyTo",
            "GetEnumerator", ["System$Collections$Generic$IEnumerable$1$" + Bridge.getTypeAlias(T) + "$GetEnumerator", "System$Collections$Generic$IEnumerable$1$GetEnumerator"]
        ],
        ctors: {
            ctor: function () {
                Granular.Collections.ObservableCollection$1(T).$ctor2.call(this, new (System.Collections.Generic.List$1(T)).ctor());
            },
            $ctor1: function (collection) {
                Granular.Collections.ObservableCollection$1(T).$ctor2.call(this, new (System.Collections.Generic.List$1(T)).$ctor1(collection));
            },
            $ctor3: function (capacity) {
                Granular.Collections.ObservableCollection$1(T).$ctor2.call(this, new (System.Collections.Generic.List$1(T)).$ctor2(capacity));
            },
            $ctor2: function (items) {
                this.$initialize();
                this.items = items;
            }
        },
        methods: {
            getItem: function (index) {
                return this.items.getItem(index);
            },
            setItem: function (index, value) {
                if (Granular.Compatibility.EqualityComparer$1(T).Default.equals2(this.items.getItem(index), value)) {
                    return;
                }

                var oldItem = this.items.getItem(index);
                this.items.setItem(index, value);
                Granular.Collections.NotifyCollectionChangedEventHandlerExtensions.Raise(this.CollectionChanged, this, Granular.Collections.NotifyCollectionChangedEventArgs.Replace(oldItem, this.items.getItem(index), index));
            },
            add: function (item) {
                this.items.add(item);
                Granular.Collections.NotifyCollectionChangedEventHandlerExtensions.Raise(this.CollectionChanged, this, Granular.Collections.NotifyCollectionChangedEventArgs.Add(item, ((this.Count - 1) | 0)));
                Granular.Extensions.EventHandlerExtensions.Raise(this.PropertyChanged, this, Granular.Collections.ObservableCollection$1(T).CountPropertyChangedEventArgs);
            },
            clear: function () {
                var e = Granular.Collections.NotifyCollectionChangedEventArgs.RemoveRange(System.Linq.Enumerable.from(this.items).select(function (x) { return Bridge.cast(x, System.Object); }).ToArray(System.Object), 0);
                this.items.clear();
                Granular.Collections.NotifyCollectionChangedEventHandlerExtensions.Raise(this.CollectionChanged, this, e);
                Granular.Extensions.EventHandlerExtensions.Raise(this.PropertyChanged, this, Granular.Collections.ObservableCollection$1(T).CountPropertyChangedEventArgs);
            },
            contains: function (item) {
                return this.items.contains(item);
            },
            indexOf: function (item) {
                return this.items.indexOf(item);
            },
            insert: function (index, item) {
                this.items.insert(index, item);
                Granular.Collections.NotifyCollectionChangedEventHandlerExtensions.Raise(this.CollectionChanged, this, Granular.Collections.NotifyCollectionChangedEventArgs.Add(item, index));
                Granular.Extensions.EventHandlerExtensions.Raise(this.PropertyChanged, this, Granular.Collections.ObservableCollection$1(T).CountPropertyChangedEventArgs);
            },
            remove: function (item) {
                var index = this.items.indexOf(item);

                if (index === -1) {
                    return false;
                }

                this.removeAt(index);
                return true;
            },
            removeAt: function (index) {
                var e = Granular.Collections.NotifyCollectionChangedEventArgs.Remove(this.items.getItem(index), index);
                this.items.removeAt(index);
                Granular.Collections.NotifyCollectionChangedEventHandlerExtensions.Raise(this.CollectionChanged, this, e);
                Granular.Extensions.EventHandlerExtensions.Raise(this.PropertyChanged, this, Granular.Collections.ObservableCollection$1(T).CountPropertyChangedEventArgs);
            },
            copyTo: function (array, arrayIndex) {
                this.items.copyTo(array, arrayIndex);
            },
            GetEnumerator: function () {
                return this.items.GetEnumerator().$clone();
            },
            System$Collections$IEnumerable$GetEnumerator: function () {
                return this.GetEnumerator();
            }
        }
    }; });

    Bridge.define("System.Xml.Linq.XDocument", {
        inherits: [System.Xml.Linq.XContainer],
        statics: {
            methods: {
                Parse: function (text) {
                    return new System.Xml.Linq.XDocument(new DOMParser().parseFromString(text, "application/xml"));
                }
            }
        },
        fields: {
            node: null
        },
        props: {
            Root: null
        },
        ctors: {
            ctor: function (node) {
                this.$initialize();
                System.Xml.Linq.XContainer.ctor.call(this, node);
                this.node = node;
                this.Root = new System.Xml.Linq.XElement(Bridge.cast(node.firstChild, Element));
            }
        }
    });

    Bridge.define("System.Xml.Linq.XElement", {
        inherits: [System.Xml.Linq.XContainer],
        fields: {
            element: null,
            attributes: null
        },
        props: {
            Name: null
        },
        ctors: {
            ctor: function (element) {
                this.$initialize();
                System.Xml.Linq.XContainer.ctor.call(this, element);
                this.element = element;
                this.Name = System.Xml.Linq.XName.Get(System.Xml.Linq.NodeExtensions.GetLocalName(element), element.namespaceURI);

                this.attributes = System.Array.init(element.attributes.length, null, System.Xml.Linq.XAttribute);
                for (var i = 0; i < this.attributes.length; i = (i + 1) | 0) {
                    this.attributes[System.Array.index(i, this.attributes)] = new System.Xml.Linq.XAttribute(element.attributes[i]);
                }
            }
        },
        methods: {
            Attributes: function () {
                return this.attributes;
            }
        }
    });
});
