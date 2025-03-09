// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Air.Domain
{
    public abstract class EntityBase
    {
        public Guid AirId { get; } = Guid.CreateVersion7(); //is sortable on creation and uses Datetime.UtcNow in the encoding, new for .NET 9
    }
}
