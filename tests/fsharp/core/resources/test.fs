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


open System.Resources

type Resources = A

let foreachE (e : System.Collections.IEnumerator) (f : 'a -> unit) = 
  while (e.MoveNext()) do
    f (unbox e.Current);
  done


let main() = 
  let ass = (typeof<Resources>).Assembly in 
  Printf.printf "ass = %s\n" (ass.ToString());
  let args = System.Environment.GetCommandLineArgs() in
  let rname = if Array.length args > 1 then args.[1] else "Resources" in 
  let resourceMan = new System.Resources.ResourceManager(rname, (typeof<Resources>).Assembly) in 
  let resourceCulture = (null : System.Globalization.CultureInfo) in 
  let image1 : System.Drawing.Bitmap = resourceMan.GetObject("Image1", resourceCulture) :?> System.Drawing.Bitmap in 
  let chimes : System.IO.UnmanagedMemoryStream = resourceMan.GetStream("chimes", resourceCulture) in 
  let icon1 : System.Drawing.Icon = resourceMan.GetObject("Icon1", resourceCulture) :?> System.Drawing.Icon in
  Printf.printf "chimes = %s\n" (chimes.ToString());
  Printf.printf "icon1 = %s\n" (icon1.ToString());
  Printf.printf "image1 = %s\n" (image1.ToString())



do main()

