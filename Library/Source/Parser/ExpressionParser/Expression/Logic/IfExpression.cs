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

using System;

namespace Vici.Core.Parser
{
    public class IfExpression : Expression
    {
        public Expression Condition { get; set; }
        public Expression TrueExpression { get; set; }
        public Expression FalseExpression { get; set; }

        public IfExpression(TokenPosition position, Expression condition) : base(position)
        {
            Condition = condition;
        }

        public override ValueExpression Evaluate(IParserContext context)
        {
            bool result = context.ToBoolean(Condition.Evaluate(context).Value);

            ValueExpression expression = null;

            if (result)
                expression = TrueExpression.Evaluate(context);
            else if (FalseExpression != null)
                expression = FalseExpression.Evaluate(context);

            if (expression is ReturnValueExpression)
                return expression;

            return Exp.NullValue(TokenPosition);
        }

#if DEBUG
        public override string ToString()
        {
            return "if(" + Condition + ") " + TrueExpression + " else " + FalseExpression + ")";
        }
#endif
    }
}