using System.Web.UI;
using System.Web.UI.WebControls;

namespace Widgets
{
    [ToolboxData("<{0}:BlockWebControl runat=server></{0}:BlockWebControl>")]
    public class BlockWebControl : CompositeControl
    {
        private readonly string _title;
        private int _initialControlSize;  

        public BlockWebControl(string title)
        {
            _initialControlSize = Controls.Count;
            _title = title;
        }
        protected override void RenderContents(HtmlTextWriter output)
        {
            if(Controls.Count == _initialControlSize)
                return;
            output.Write($@"
<div class=""question-block"">
    <h3 class=""question-block-title"">{_title}</h3>
    <div class=""question-block-questions"">");
            foreach (var ct in Controls)
            {
                ((Control)ct).RenderControl(output);
            }
            output.Write(@"
    </div>
</div>");
        }
    }
}
