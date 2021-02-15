# DeepIndex

DeepIndex is a search engine school project, a learning experience rather than
a practical solution.

## Architecture

The underlying architecture follows the Clean (Onion) Architecture, centered 
around the the Core namespace.

This project uses Microsoft's Dependency Injection framework to maintain low 
coupling. The project relies in extension methods within each project to 
further decouple the projects from each other, by placing the responsibility of
managing internal requirements/interface implementations on the module rather 
than the project's entry point.

## Configuration

The Hosting framework will by default load configurations from various sources 
in the following order:

* Host
  * Environment variables prefixed with DOTNET_.
  * Command-line arguments.
* App
  * appsettings.json.
  * appsettings.{Environment}.json.
  * Secret Manager when the app runs in the Development environment.
  * Environment variables.
  * Command-line arguments.

The configuration JSON files are located in the project's root folder and 
should be linked into the projects that need them (remember to mark them as
`copy to output directory`.)
