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

module Test

#nowarn "44"

type PublicUnionType1 = X of string | XX of string * string
type PublicUnionType2 = X2 | XX2 of string
type 'a PublicUnionType3 = X3 | XX3 of 'a
type PublicRecordType1 = { r1a : int }
type 'a PublicRecordType2 = { r2b : 'a; r2a : int }


type internal InternalUnionType1 = InternalX of string | InternalXX of string * string
type internal InternalUnionType2 = InternalX2 | InternalXX2 of string
type internal 'a InternalUnionType3 = InternalX3 | InternalXX3 of 'a
type internal InternalRecordType1 = { internal_r1a : int }
type internal 'a InternalRecordType2 = { internal_r2b : 'a; internal_r2a : int }
