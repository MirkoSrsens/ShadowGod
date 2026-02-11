using Sirenix.OdinInspector;
using System;

namespace Razorhead.Core
{
    [IncludeMyAttributes]
    [HideLabel, Indent, InlineProperty]
    [Title("@$property.NiceName", horizontalLine: false, bold: false)]
    [PropertySpace(SpaceAfter = 6, SpaceBefore = -8)]
    public class ReInlineAttribute : Attribute
    {
    }
}
