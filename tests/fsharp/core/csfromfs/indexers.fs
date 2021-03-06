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
#indent "off"

module Indexers

let failures = ref false
let report_failure () = 
  stderr.WriteLine "NO"; failures := true


let testIndexers = 
  begin 
    let c = new CSharpIndexers.C() in 
    if c.Item(3) <> 203 then report_failure();
    if (upcast c : CSharpIndexers.I).Item(3) <> 103 then report_failure(); 
    let d = new CSharpIndexers.D() in 
    if d.Item(3) <> 303 then report_failure();
    if (upcast d : CSharpIndexers.C).Item(3) <> 303 then report_failure();
  end;
  begin 
    let d = new CSharpIndexers.OverloadedIndexer() in 
    if d.Item(3) <> 203 then report_failure();
    if d.Item("2") <> 201 then report_failure();
    if (upcast d : CSharpIndexers.I2).Item(3) <> 103 then report_failure();
    if (upcast d : CSharpIndexers.I2).Item("4") <> 101 then report_failure();
   
  end


let _ = 
  if !failures then (stdout.WriteLine "Test Failed"; exit 1) 
  else (stdout.WriteLine "Test Passed"; 
        System.IO.File.WriteAllText("test.ok","ok"); 
        exit 0)
