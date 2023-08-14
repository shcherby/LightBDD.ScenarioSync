## The Light BDD test framework scenarios Azure DevOps import tool

See latest release details on [What Is New]()  wiki page!

## Project description
ScenarioSync for Azure DevOps integrates the BDD process with Azure DevOps by connecting and synchronizing the BDD scenarios with Test Cases 
and link them to related User Stories or Tasks in Azure DevOps. This way of the development reduce time spend to maintain actuality of scenarios in code and Azure DevOps.
ScenarioSync uses LightBDD FeaturesReport.xml file as a source and base on it creating Test suites and test cases in Azure DevOps, Test suites and Test cases that not in source removed from Azure DevOps.

### Light BDD to Azure DevOps Test Plan features mapping

| LightBDD                               | Azure DevOps Test Plan | Notes                                                                       |
|----------------------------------------|------------------------|-----------------------------------------------------------------------------|
| Feature                                | Test Suite             | Created as Test Suite                                                       |
| Feature Description                    | Test Suite Description | Saving to Test Suite Description                                            |
| Scenario                               | Test Case              | Created as Test Case                                                        |
| Scenario                               | Test Case Summary      | Saving as text rendered copy to Test Case description                       |
| Scenario Steps                         | Test Case Steps        | Created as Test Case Steps                                                  |
| Step File Attachment                   | Step Attachment        | Created Step Attachment                                                     |
| Step Table parameter                   | Step Parameters        | Not support Azure DevOps parameters, Saving as a text table in a step title |
| Step Tree parameter                    | X                      | Saving as a text in a step title                                            |
| Categories                             | Tags                   | Saving as Tags                                                              |
| Labels                                 | Tags                   | Saving as Tags                                                              |
| Comment                                | X                      | Saving to Test Case Description                                             |
| Step parameter expectation expressions | Step Expected Result   | Saving as Test case step expected result                                    |
| Step parameter expectation expressions | Step Expected Result   | Saving as Test case step expected result                                    |
| X                                      | Configurations         | Mapping not supported, can be applied to Static or Query based test suite   |
| Labels (Relations:)                    | Related Work           | Use Label attribute with text 'Relations:'                                  |
| Labels (Sync:)                         | Associated Automation  | Use Label attribute with text 'Sync:'                                       |

## ScenarioSync CLI 
### Usage
```bash
scenariosync <command> [options]
```

### Commands
| Command     | Notes                                                                                            |
|-------------|--------------------------------------------------------------------------------------------------|
| ```push```  | Create or update exist features from LightBDD FeaturesReport.xml to Azure DevOps Root Test Suite |
| ```clean``` | Delete all Test Suites and Test Cases from Root Azure DevOps Test Suite                          |

### Options
| Option                 | Notes                                                                                                                                                               |
|------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| ```--projectUrl```     | [required] <br/>Url to Azure DevOps project in a format "https://dev.azure.com/organization-name/project-name"                                                      |
| ```--patToken```       | [required] <br/>Azure DevOps user account "Personal access token"                                                                                                   |
| ```--testPlanId```     | [required] <br/>Azure DevOps Test Plan Id, defined in URL "https://dev.azure.com/organization-name/project-name/_testPlans/define?planId=1" as "planId" query param |
| ```--reportFilePath``` | [required] <br/>Path to "FeaturesReport.xml" LightBDD xml report                                                                                                    |
| ```--rootTestSuite```  | [optional] <br/>Root Test Suite name where Light BDD scenarios will be created. Default value: "LightBddSync"                                                       |

### Examples
#### Create/update scenarios to Azure Devops 'LightBddSync' Root Test Suite
```bash
dotnet scenariosync push --projectUrl "https://dev.azure.com/organization-name/project-name" --patToken "344urpefnuf4skfobpu3fejhlumm7mvo373pxqmwhbbdxabjq" --testPlanId 5 --reportFilePath "FeaturesReport.xml" 
```

#### Remove all scenarios the 'LightBddSync' Root TestSuite from Azure Devops
```bash
dotnet scenariosync clean --projectUrl "https://dev.azure.com/organization-name/project-name" --patToken "344urpefnuf4skfobpu3fejhlumm7mvo373pxqmwhbbdxabjq" --testPlanId 5 --reportFilePath "FeaturesReport.xml" 
```

## Getting started using ScenarioSync
ScenarioSync is a synchronization tool that can be invoked from the command line.
This guide shows you step-by-step how the synchronization tool can be configured.

### Preparation
For setting up ScenarioSync for Azure DevOps, you need a [LightBDD test framework](https://github.com/LightBDD/LightBDD) project and an Azure DevOps project with [Test Plan](https://learn.microsoft.com/en-us/azure/devops/test/overview?view=azure-devops). 

In our guide, we will use a Example.LightBDD.XUnit2 example that uses LightBDD framework with XUnit.
The sample project can be downloaded from [GitHub]().