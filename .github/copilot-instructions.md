## TinyShop

<!-- Add Instructions here -->

### Backend
- Products project is the backend API.
- Built with .Net Minimal APIs.
- Uses Entity Framework Core with an in-memory database.
- Should follow OpenApi best preactices.

### Frontend
- Store project is a .Net10 Blazor Server application.
- Uses default Bootstrap styling.
- UI should have a modern look and feel.
- CSS should be in .razor.css files.

## Misc
- To display an image you will always need to use `ImagePrefix` in the web front end which comes from the Configuration["ImagePrefix"] which isc @inject IConfiguration Configuration 
- Ignore all *.md files code in the **lab** folder when making code suggestions.
