// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

namespace Manufactures
{
    public class ExtensionMetadata : IExtensionMetadata
    {
        public IEnumerable<StyleSheet> StyleSheets => new StyleSheet[] { };
        public IEnumerable<Script> Scripts => new Script[] { };
        public IEnumerable<MenuItem> MenuItems
        {
            get
            {
                return new MenuItem[]
                {
                    new MenuItem("/manufacture", "Manufactures", 100)
                };
            }
        }
    }
}