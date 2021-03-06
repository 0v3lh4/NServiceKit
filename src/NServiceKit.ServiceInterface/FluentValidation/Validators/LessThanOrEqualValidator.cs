#region License
// Copyright (c) Jeremy Skinner (http://www.jeremyskinner.co.uk)
// 
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
// http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.
// 
// The latest version of this file can be found at http://www.codeplex.com/FluentValidation
#endregion

namespace NServiceKit.FluentValidation.Validators
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using Attributes;
    using Internal;
    using Resources;

    /// <summary>The less than or equal validator.</summary>
    public class LessThanOrEqualValidator : AbstractComparisonValidator
    {
        /// <summary>Initializes a new instance of the NServiceKit.FluentValidation.Validators.LessThanOrEqualValidator class.</summary>
        ///
        /// <param name="value">The value.</param>
        public LessThanOrEqualValidator(IComparable value)
            : base(value, () => Messages.lessthanorequal_error, ValidationErrors.LessThanOrEqual)
        {
        }

        /// <summary>Initializes a new instance of the NServiceKit.FluentValidation.Validators.LessThanOrEqualValidator class.</summary>
        ///
        /// <param name="valueToCompareFunc">The value to compare function.</param>
        /// <param name="member">            The member.</param>
        public LessThanOrEqualValidator(Func<object, object> valueToCompareFunc, MemberInfo member)
            : base(valueToCompareFunc, member, () => Messages.lessthanorequal_error, ValidationErrors.LessThanOrEqual)
        {
        }

        /// <summary>Query if 'value' is valid.</summary>
        ///
        /// <param name="value">         The value.</param>
        /// <param name="valueToCompare">The value to compare.</param>
        ///
        /// <returns>true if valid, false if not.</returns>
        public override bool IsValid(IComparable value, IComparable valueToCompare)
        {
            return value.CompareTo(valueToCompare) <= 0;
        }

        /// <summary>Gets the comparison.</summary>
        ///
        /// <value>The comparison.</value>
        public override Comparison Comparison
        {
            get { return Comparison.LessThanOrEqual; }
        }
    }
}