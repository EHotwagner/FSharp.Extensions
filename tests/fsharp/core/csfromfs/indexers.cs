//----------------------------------------------------------------------------
//
// Copyright (c) 2002-2011 Microsoft Corporation. 
//
// This source code is subject to terms and conditions of the Apache License, Version 2.0. A 
// copy of the license can be found in the License.html file at the root of this distribution. 
// By using this source code in any fashion, you are agreeing to be bound 
// by the terms of the Apache License, Version 2.0.
//
// You must not remove this notice, or any other, from this software.
//----------------------------------------------------------------------------

using System; using System.Diagnostics;


//****************************************************************************
// Indexers
//****************************************************************************

namespace CSharpIndexers
{
	public interface I
	{
		int this [int i] {
			get;
			set;
		}
	}

	public interface I2
	{
		int this [int i] {
			get;
			set;
		}
		int this [string i] {
			get;
			set;
		}
	}

	public class C : I
	{

		int I.this [int i] {
			get { return 100 + i; }
			set { return; }
		}

		
		public virtual int this [int i] {
			get { return 200 + i; } set { return; }
		}
	}

	public class D : C
	{
		public override int this [int i] {
                        get { return 300 + i; }
			set { return; }
		}
	}
	public class OverloadedIndexer : I2
	{

		int I2.this [int i] {
			get { return 100 + i; }
			set { return; }
		}

		int I2.this [string i] {
			get { return 100 + i.Length; }
			set { return; }
		}

		
		public virtual int this [int i] {
			get { return 200 + i; } set { return; }
		}
		public virtual int this [string i] {
			get { return 200 + i.Length; } set { return; }
		}
	}

}

