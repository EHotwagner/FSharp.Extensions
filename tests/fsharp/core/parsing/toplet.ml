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
let rec loopF n x = function 0 -> x | i -> loopF n (x+1) (i-1) in
let rec loopE n x = function 0 -> x | i -> loopE n (loopF n x n) (i-1) in
let rec loopD n x = function 0 -> x | i -> loopD n (loopE n x n) (i-1) in
let rec loopC n x = function 0 -> x | i -> loopC n (loopD n x n) (i-1) in
let rec loopB n x = function 0 -> x | i -> loopB n (loopC n x n) (i-1) in
let rec loopA n x = function 0 -> x | i -> loopA n (loopB n x n) (i-1) in  
let n =
  try 
    System.Int32.Parse(System.Environment.GetCommandLineArgs().[1])
  with 
    _ -> 1
  in
Printf.printf "%d\n" (loopA n 0 n)

