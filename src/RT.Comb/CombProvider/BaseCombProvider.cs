﻿using System;
/*
	Copyright 2015-2025 Richard S. Tallent, II

	Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files
	(the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge,
	publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to
	do so, subject to the following conditions:

	The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
	MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
	LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
	CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

namespace RT.Comb {

	// Not worth its own file. Allows overriding how the random portion of Guids are created, in case
	// someone wants to use a different algorithm than Guid.NewGuid() or wants to decorate it before
	// the timestamp embedding (such as overriding another portion of the random bytes for some another
	// purpose).
	public delegate Guid GuidProvider();

	// This base class handles common methods for both the SQL Server and PostgreSql implementations.
	// Note that either implementation can be paired with either CombDateTimeStrategy.
	public abstract class BaseCombProvider : ICombProvider {

		protected ICombDateTimeStrategy DateTimeStrategy;
		private readonly TimestampProvider? _timestampProvider;
		private readonly GuidProvider? _guidProvider;

		public BaseCombProvider(ICombDateTimeStrategy dateTimeStrategy, TimestampProvider? customTimestampProvider = null, GuidProvider? customGuidProvider = null) {
			if (dateTimeStrategy.NumDateBytes != 4 && dateTimeStrategy.NumDateBytes != 6) {
				throw new NotSupportedException("ICombDateTimeStrategy is limited to either 4 or 6 bytes.");
			}
			DateTimeStrategy = dateTimeStrategy;
			_timestampProvider = customTimestampProvider;
			_guidProvider = customGuidProvider;
		}

		public abstract DateTime GetTimestamp(Guid comb);

		public Guid Create() => Create(_guidProvider?.Invoke() ?? Guid.NewGuid(), _timestampProvider?.Invoke() ?? DateTime.UtcNow);

		public Guid Create(Guid value) => Create(value, _timestampProvider?.Invoke() ?? DateTime.UtcNow);

		public Guid Create(DateTime timestamp) => Create(_guidProvider?.Invoke() ?? Guid.NewGuid(), timestamp);

		public abstract Guid Create(Guid value, DateTime timestamp);

	}

}