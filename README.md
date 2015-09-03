# MonoX-CalendarEvent-SearchProvider
MonoX Calendar event search provider sample

## Usage

- web.config registration
```xml
<SearchEngine>
    <providers>
        <add name="YourSearchProvider" type="YourNameSpace.YourSearchProvider, YourDllName" BoldSearchPhrases="true"/>
    </providers>
</SearchEngine>
```

- Search WebPart setup
```xml
<MonoX:Search ID="search1" runat="server" Title='Your title'>
    <SearchProviderItems>
        <Search:SearchProviderItem Name="YourSearchProvider" Template="Default"></Search:SearchProviderItem>
    </SearchProviderItems>
</MonoX:Search>
```

