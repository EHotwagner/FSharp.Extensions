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

using System;
using Microsoft.FSharp;
using Microsoft.FSharp.Core;
using Microsoft.FSharp.Control;
using Microsoft.FSharp.Collections;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

class Maine {
  static int Test2() {
    Test.EventTest2.SomeComponent ie = Test.EventTest2.mk();
    ie.SomeEvent += onEvent;
    FSharpHandler<string> seh = new FSharpHandler<string>(onEvent);
    ie.SomeEvent += seh;
    Test.EventTest2.fire(ie);
    ie.SomeEvent -= seh;

    ie.Paint += onPaint;
    FSharpHandler<EventArgs> peh = new FSharpHandler<EventArgs>(onPaint);
    ie.Paint += peh;
    Test.EventTest2.fire(ie);
    ie.Paint -= peh;

    Test.EventTest2.fire(ie);
    return 0;
  }
  static int Main() {
      int res = Test2();
      Console.WriteLine("failures = {0}", Test.failures);
      return res;

  }
  static void onEvent(object x,string s) {
    Console.WriteLine("onEvent, s = {0}",s);
  }
  static void onPaint(object sender, EventArgs ea) {
    Console.WriteLine("onPaint, ea = {0}",ea);
   }

}
