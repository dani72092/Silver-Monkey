﻿namespace Tapestry.Expressions
{
    public sealed class StringExpression : Expression<string>
    {
        public StringExpression(ref SourcePosition pos, string value)
            : base(ref pos) { Value = value; }
    }
}