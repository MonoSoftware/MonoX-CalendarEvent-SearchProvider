using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MonoSoftware.MonoX.SearchEngine;
using MonoSoftware.Web;
using MonoSoftware.MonoX.Utilities;
using MonoSoftware.MonoX.DAL.EntityClasses;
using MonoSoftware.MonoX;
using MonoSoftware.MonoX.Resources;
using MonoSoftware.MonoX.Repositories;
using MonoSoftware.MonoX.DAL.HelperClasses;
using MonoSoftware.MonoX.BusinessLayer;

namespace MonoSoftware.MonoX.SearchEngine.Providers
{
    public class CalendarEventSearchProvider : MonoSoftware.MonoX.SearchEngine.Providers.SearchProviderBase, ISearchEngineProvider
    {
        #region Fields
        /// <summary>
        /// Calendar event preview page path parameter.
        /// </summary>
        public static readonly string CalendarEventPreviewPagePathParam = "CalendarEventPreviewPagePath";
        #endregion

        #region Properties
        /// <summary>
        /// Gets the provider's localized title.
        /// </summary>
        public string ProviderLocalizedTitle
        {
            get { return Resources.Search.CalendarEventSearchProviderTitle; }
        }
        /// <summary>
        /// Gets the provider name.
        /// </summary>
        public string ProviderName
        {
            get { return "CalendarEventSearchProvider"; }
        }

        /// <summary>
        /// Gets or sets the record count.
        /// </summary>
        public int RecordCount { get; set; }

        /// <summary>
        /// Maximum number of results to return.
        /// </summary>
        public static readonly string MaxNoOfResults = "200";


        #endregion

        /// <summary>
        /// Gets the event viewer URL.
        /// </summary>
        /// <param name="eventEntity">Calendar event entity.</param>
        /// <returns>Event viewer URL.</returns>
        protected virtual string GetEventViewUrl(CalendarEventEntity eventEntity)
        {
            string eventViewerURL = Paths.MonoX.Pages.SocialNetworking.EventCalendar_aspx;
            if (ProviderAttributes[CalendarEventPreviewPagePathParam] != null)
                eventViewerURL = ProviderAttributes[CalendarEventPreviewPagePathParam];

            return UrlFormatter.ResolveServerUrl(eventViewerURL).Append(UrlParams.EntityId, eventEntity.Id);
        }

        /// <summary>
        /// Gets the formatted event description.
        /// </summary>
        /// <param name="eventEntity">Event entity.</param>
        /// <returns>Formatted event description.</returns>
        protected virtual string GetFormattedEventDescription(CalendarEventEntity eventEntity)
        {
            string place = !String.IsNullOrWhiteSpace(eventEntity.Place) ? String.Format(EventModuleResources.Event_Description_Place, eventEntity.Place) : String.Empty;
            return String.Format(EventModuleResources.Event_Description, eventEntity.Title, place, eventEntity.StartTime.ToLongDateString(), eventEntity.EndTime.ToLongDateString(), String.Format("{0}<br/>", Environment.NewLine), eventEntity.Description);
        }

        /// <summary>
        /// Initialize result item.
        /// </summary>
        /// <param name="eventEntity">Event entity.</param>
        /// <returns>Result item.</returns>
        protected virtual CalendarEventSearchResultItem InitializeItem(CalendarEventEntity eventEntity)
        {
            CalendarEventSearchResultItem item = new CalendarEventSearchResultItem(this);

            string eventUrl = GetEventViewUrl(eventEntity);
            item.Title = String.Format("<a href='{0}' title='{1}'>{1}</a>", ClearSearchQueryParams(eventUrl), eventEntity.Title);
            item.Url = String.Empty;
            string description = GetFormattedEventDescription(eventEntity);

            if (ProviderAttributes[BoldSearchPhrasesParam] != null)
                if (bool.Parse(ProviderAttributes[BoldSearchPhrasesParam]))
                    SearchEngineCore.BoldSearchPhrases(SearchPhrase, ref description);

            item.Description = description;
            item.Related = String.Empty;

            return item;
        }

        /// <summary>
        /// Main search method.
        /// </summary>
        /// <returns>Array of search results.</returns>
        public List<ISearchEngineResultItem> Search()
        {
            int maxNoOfResults = Int32.Parse(MaxNoOfResults);
            if (ProviderAttributes[MaxNoOfResults] != null)
                Int32.TryParse(ProviderAttributes[MaxNoOfResults], out maxNoOfResults);

            CalendarEventArgs args = new CalendarEventArgs();
            args.SearchPhrase = SearchPhrase;
            args.MaximumNumberOfItems = maxNoOfResults;

            EntityCollection<CalendarEventEntity> collection = DependencyInjectionFactory.Resolve<ICalendarEventBLL>().
                GetCalendarEvents(args);
            List<ISearchEngineResultItem> results = new List<ISearchEngineResultItem>();

            foreach (CalendarEventEntity CalEvent in collection)
            {
                CalendarEventSearchResultItem item = InitializeItem(CalEvent);
                results.Add(item);
            }

            return results;
        }

        /// <summary>
        /// Gets a search result template.
        /// </summary>
        /// <returns>ISearchEngineResultTemplate for current provider.</returns>
        public int GetRecordCount()
        {
            int maxNoOfResults = MaxNumberOfRecords;
            if (ProviderAttributes[MaxNoOfResults] != null)
                Int32.TryParse(ProviderAttributes[MaxNoOfResults], out maxNoOfResults);

            CalendarEventArgs args = new CalendarEventArgs();
            args.SearchPhrase = SearchPhrase;
            args.MaximumNumberOfItems = maxNoOfResults;
            
            EntityCollection<CalendarEventEntity> collection = 
                DependencyInjectionFactory.Resolve<ICalendarEventBLL>().GetCalendarEvents(args);

            RecordCount = args.RecordCount > maxNoOfResults ? maxNoOfResults : args.RecordCount;
            return RecordCount;
        }
    }
}