using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MonoSoftware.MonoX.SearchEngine.Providers;
using MonoSoftware.MonoX.SearchEngine;

namespace MonoSoftware.MonoX.SearchEngine.Providers
{
    public class CalendarEventSearchResultItem : SearchResultItemBase
    {
        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        public CalendarEventSearchResultItem()
            : base()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="provider">Search engine provider.</param>
        public CalendarEventSearchResultItem(ISearchEngineProvider provider)
            : base(provider)
        {
        }
        #endregion

        #region ISearchEngineResultItem Members
        /// <summary>
        /// Gets a provider name.
        /// </summary>
        [Obsolete("Please use the Provider property - direct search engine provider reference.")]
        public override string ProviderName
        {
            get { return "CalendarEventSearchProvider"; }
        }

        #endregion
    }
}