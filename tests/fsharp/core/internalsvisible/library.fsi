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

module Library

module M :
  val internal internalF          : int -> int
  val internal signatureInternalF : int -> int    
  val publicF                     : int -> int

module internal P :
    type internal InternalClass =
        new : int -> InternalClass
        member X : int

    val internal InternalObject : InternalClass
