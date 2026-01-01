// Copyright (c) 2026 Kévin Alexandre Boissonneault

// Use, modification, and distribution is subject to the Boost Software
// License, Version 1.0. (See accompanying file LICENSE or copy at
// http://www.boost.org/LICENSE_1_0.txt)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMBF2.Blueprint
{
    internal class PatchUtils
    {
        public static bool StartPatch(string patchName)
        {
            Main.Log.Log($"Patching '{patchName}'");
            return true;
        }
    }
}
