## The Light BDD test framework scenarios Azure DevOps import tool

See latest release details on [What Is New]()  wiki page!

### Project description
ScenarioSync for Azure DevOps integrates the BDD process with Azure DevOps by connecting and synchronizing the BDD scenarios with Test Cases 
and link them to related User Stories or Tasks in Azure DevOps. This way of the development reduce time spend to maintain actuality of scenarios in code and Azure DevOps.

### Example
```bash
dotnet scenariosync push \
  --projectUrl "https://dev.azure.com/organization-name/project-name" \
  --patToken "344urpefnuf4skfobpu3fejhlumm7mvo373pxqmwhbbdxabjq" \
  --testPlanId  5 \
  --reportFilePath  "FeaturesReport.xml" 
```