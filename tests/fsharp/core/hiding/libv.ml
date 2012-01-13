//----------------------------------------------------------------------------
//
// Copyright (c) 2002-2010 Microsoft Corporation. 
//
// This source code is subject to terms and conditions of the Apache License, Version 2.0. A 
// copy of the license can be found in the License.html file at the root of this distribution. 
// By using this source code in any fashion, you are agreeing to be bound 
// by the terms of the Apache License, Version 2.0.
//
// You must not remove this notice, or any other, from this software.
//----------------------------------------------------------------------------
#light




let internal topv = 1
type MyRecord = 
    { x1 : int;
      x2 : int } 
    member obj.X1 = obj.x1
    member obj.X2 = obj.x2
    member obj.TopV = topv
    member obj.TwiceX = obj.x1 + obj.x1
    static member Create(n) = { x1 = n; x2 = n }
    
