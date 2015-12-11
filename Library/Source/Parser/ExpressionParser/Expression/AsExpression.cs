#region License
//=============================================================================
// Vici Core - Productivity Library for .NET 3.5 
//
// Copyright (c) 2008-2012 Philippe Leybaert
//
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights 
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
// copies of the Software, and to permit persons to whom the Software is 
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in 
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//=============================================================================
#endregion

using System;
using System.Collections.Generic;

namespace Vici.Core.Parser
{
    public class AsExpression : BinaryExpression
    {
        public Expression ObjectExpression { get { return base.Left; } }
        public Expression TypeExpression { get { return base.Right; } }

        public AsExpression(TokenPosition position, Expression objectExpression, Expression typeExpression) : base(position,objectExpression,typeExpression)
        {
        }

        public override ValueExpression Evaluate(IParserContext context)
        {
            ClassName className = TypeExpression.Evaluate(context).Value as ClassName;

            if (className == null)
                throw new IllegalOperandsException("as operator requires type. "  + TypeExpression + " is not a type",this);

            Type checkType = className.Type;
            ValueExpression objectValue = ObjectExpression.Evaluate(context);
            Type objectType = objectValue.Type;

            if (objectValue.Value == null)
                return Exp.Value(TokenPosition, null, checkType);

            objectType = objectType.Inspector().RealType;

            if (!objectType.Inspector().IsValueType)
                return Exp.Value(TokenPosition, objectValue.Value, checkType);

            if ((Nullable.GetUnderlyingType(checkType) ?? checkType) == objectType)
                return Exp.Value(TokenPosition, objectValue.Value, checkType);

            return Exp.Value(TokenPosition, null, checkType);
        }
    }
}
