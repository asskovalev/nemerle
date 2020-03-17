using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Drawing;
using Microsoft.VisualStudio.Language.NavigateTo.Interfaces;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using System.Collections.Generic;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Imaging.Interop;
using Microsoft.VisualStudio.Text;
using System.IO;
using Nemerle.Completion2;
using Nemerle.VisualStudio;

namespace Nemerle.VsExtension.NavigateTo
{
    class NavigateToItemDisplay : INavigateToItemDisplay3
    {
        readonly IGlyphService _glyphService;
        readonly NavigateToItem _item;
        readonly IServiceProvider _serviceProvider;

        public NavigateToItemDisplay(IServiceProvider serviceProvider, IGlyphService glyphService, NavigateToItem item)
        {
            _serviceProvider = serviceProvider;
            _glyphService = glyphService;
            _item = item;
        }

        public string AdditionalInformation
        {
            get
            {
                var info = (SymbolInfo)_item.Tag;
                return info.FullName;
            }
        }

        DescriptionRun[] MakeNameDescriptionRuns()
        {
            return Array.Empty<DescriptionRun>();
        }

        public Icon Glyph => null;
        public string Name => _item.Name;

        public int GetProvisionalViewingStatus()
        {
            return (int)__VSPROVISIONALVIEWINGSTATUS.PVS_Enabled;
        }

        // INavigateToItemDisplay2

        public void NavigateTo()
        {
            var loc = ((SymbolInfo)_item.Tag).Location;
            Utils.NavigateTo(_serviceProvider, loc.File, loc.Span);
        }

        public void PreviewItem()
        {
            var loc = ((SymbolInfo)_item.Tag).Location;
            Utils.NavigateTo(_serviceProvider, loc.File, loc.Span);
        }

        // INavigateToItemDisplay3

        public ImageMoniker GlyphMoniker => ImageLibrary.InvalidImageMoniker;

        public string Description => throw new NotImplementedException();

        public ReadOnlyCollection<DescriptionItem> DescriptionItems => new ReadOnlyCollection<DescriptionItem>(Array.Empty<DescriptionItem>());

        public IReadOnlyList<Span> GetAdditionalInformationMatchRuns(string searchValue)
        {
            return new List<Span>();
        }

        public IReadOnlyList<Span> GetNameMatchRuns(string searchValue)
        {
            var info = (SymbolInfo)_item.Tag;
            return info.MatchRuns.Select(x => new Span(x.StartPos, x.Length)).ToArray();
        }
    }
}
