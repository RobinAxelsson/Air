// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Air.Domain
{
    internal sealed record HttpResponseDto
    {
        public int StatusCode { get; init; }
        public string? ReasonPhrase { get; init; }
        public bool IsSuccessStatusCode { get; init; }
        public Dictionary<string, string> Headers { get; init; } = new();
        public string? ResponseBody { get; init; }
        public string? RequestUri { get; init; }
        public string? Method { get; init; }
    }
}
