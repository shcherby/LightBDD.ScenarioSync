using System.Text.Encodings.Web;
using LightBDD.ScenarioSync.Core.Entities;
using LightBDD.ScenarioSync.Target.Ado.Constants;
using LightBDD.ScenarioSync.Target.Ado.Models;
using Microsoft.TeamFoundation.TestManagement.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;

namespace LightBDD.ScenarioSync.Target.Ado.Helpers
{
    internal class TestItemJsonPatchDocumentBuilder
    {
        private const string NewRelationPosition = "-";
        private const string TestedByReverseRel = "Microsoft.VSTS.Common.TestedBy-Reverse";
        private const string AttachedFileRel = "AttachedFile";
        private readonly TestCaseStepsHelper _testCaseStepsHelper;
        private WorkItem _workItem;
        private IList<WorkItemRelation>? _relations;
        private IDictionary<string, object> _fields;
        private readonly JsonPatchDocument _patchDocument;

        public TestItemJsonPatchDocumentBuilder(TestCaseStepsHelper? testCaseStepsHelper = null)
        {
            _testCaseStepsHelper = testCaseStepsHelper;
            _relations = new List<WorkItemRelation>();
            _fields = new Dictionary<string, object>();
            _patchDocument = new JsonPatchDocument();
        }

        public TestItemJsonPatchDocumentBuilder Load(WorkItem workItem)
        {
            _workItem = workItem ?? new WorkItem();
            _relations = workItem.Relations ?? new List<WorkItemRelation>();
            _fields = workItem.Fields ?? new Dictionary<string, object>();
            GetRemoveRelationsOperations(_relations).ForEach(_patchDocument.Add);
            return this;
        }

        public TestItemJsonPatchDocumentBuilder SetTitle(string title)
        {
            if (TryGetJsonPatchOperation(
                    WorkItemFieldsConst.Title,
                    title,
                    out JsonPatchOperation patchOperation))
            {
                _patchDocument.Add(patchOperation);
            }

            return this;
        }

        public TestItemJsonPatchDocumentBuilder SetDescription(string description, bool useCodeTag = false)
        {
            string encodedDescription = HtmlEncoder.Default.Encode(description);
            encodedDescription = useCodeTag ? $"<pre><code>{encodedDescription}</code></pre>" : encodedDescription;
            if (TryGetJsonPatchOperation(
                    WorkItemFieldsConst.Description,
                    encodedDescription,
                    out JsonPatchOperation patchOperation))
            {
                _patchDocument.Add(patchOperation);
            }

            return this;
        }

        public TestItemJsonPatchDocumentBuilder SetAssignedTo(string assignedTo)
        {
            if (TryGetJsonPatchOperation(
                    "System.AssignedTo",
                    assignedTo,
                    out JsonPatchOperation patchOperation))
            {
                _patchDocument.Add(patchOperation);
            }

            return this;
        }

        public TestItemJsonPatchDocumentBuilder SetRelations(IReadOnlyCollection<RelationOperation> testCaseWorkItems)
        {
            foreach (RelationOperation workItem in testCaseWorkItems)
            {
                _patchDocument.Add(CreateRelationsPatchOperation(
                    NewRelationPosition,
                    new WorkItemRelation()
                    {
                        Rel = TestedByReverseRel,
                        Url = workItem.Url
                    },
                    Operation.Add));
            }

            return this;
        }

        public TestItemJsonPatchDocumentBuilder SetStepAttachments(IReadOnlyCollection<StepCreateAttachmentOperation> stepAttachments)
        {
            foreach (StepCreateAttachmentOperation stepAttachment in stepAttachments)
            {
                var attributes = new Dictionary<string, object>()
                {
                    { "comment", $"[TestStep={stepAttachment.StepNumber}]:" },
                };
                var atLink = new TestAttachmentLink()
                {
                    Rel = AttachedFileRel,
                    Attributes = attributes,
                    Url = stepAttachment.Url
                };

                _patchDocument.Add(CreateRelationsPatchOperation(NewRelationPosition, atLink, Operation.Add));
            }

            return this;
        }

        public TestItemJsonPatchDocumentBuilder SetTags(IEnumerable<string> tags)
        {
            string tagsString = string.Join(";", tags);

            if (TryGetJsonPatchOperation(
                    WorkItemFieldsConst.Tags,
                    tagsString,
                    out JsonPatchOperation patchOperation))
            {
                _patchDocument.Add(patchOperation);
            }

            return this;
        }

        public TestItemJsonPatchDocumentBuilder SetAutomationMetadata(AutomatedTestMetadata automationMetadata)
        {
            if (TryGetJsonPatchOperation(
                    "Microsoft.VSTS.TCM.AutomatedTestName",
                    automationMetadata.MethodName,
                    out JsonPatchOperation automatedTestNamePatch))
            {
                _patchDocument.Add(automatedTestNamePatch);
            }

            if (TryGetJsonPatchOperation(
                    "Microsoft.VSTS.TCM.AutomatedTestStorage",
                    automationMetadata.StorageName,
                    out JsonPatchOperation automatedTestStorage))
            {
                _patchDocument.Add(automatedTestStorage);
            }

            if (TryGetJsonPatchOperation(
                    "Microsoft.VSTS.TCM.AutomatedTestId",
                    Guid.NewGuid().ToString(),
                    out JsonPatchOperation automatedTestId))
            {
                _patchDocument.Add(automatedTestId);
            }

            if (TryGetJsonPatchOperation(
                    "Microsoft.VSTS.TCM.AutomationStatus",
                    "Automated",
                    out JsonPatchOperation automationStatus))
            {
                _patchDocument.Add(automationStatus);
            }

            return this;
        }

        public TestItemJsonPatchDocumentBuilder SetSteps(IReadOnlyCollection<TestStep> steps)
        {
            if (_testCaseStepsHelper == null)
            {
                throw new ArgumentNullException(nameof(_testCaseStepsHelper), $"{nameof(_testCaseStepsHelper)} Init in constructor");
            }

            ITestBase testBase = new TestBaseHelper().Create();

            LoadSteps(testBase);

            _testCaseStepsHelper.SetTestSteps(testBase, steps);
            
            testBase.SaveActions(_patchDocument);
            return this;
        }

        private void LoadSteps(ITestBase testBase)
        {
            if (_fields.TryGetValue("Microsoft.VSTS.TCM.Steps", out object? stepsObj))
            {
                if (stepsObj is string stepsXml)
                {
                    testBase.LoadActions(stepsXml, new List<TestAttachmentLink>());
                }
            }
        }

        public JsonPatchDocument Build()
        {
            return _patchDocument;
        }

        private bool TryGetJsonPatchOperation(string fieldName, string fieldValue, out JsonPatchOperation patchOperation)
        {
            patchOperation = null;
            if (string.IsNullOrEmpty(fieldValue))
            {
                return false;
            }

            bool isUpdatingOperation = _fields.TryGetValue(fieldName, out object? existingFieldValue);
            Operation operation = isUpdatingOperation ? Operation.Replace : Operation.Add;
            patchOperation = new JsonPatchOperation()
            {
                Operation = operation,
                Path = $"/fields/{fieldName}",
                Value = fieldValue
            };

            if (!isUpdatingOperation)
            {
                return true;
            }

            if (existingFieldValue is string existingValue && existingValue != fieldValue)
            {
                return true;
            }

            return false;
        }

        private static IList<JsonPatchOperation> GetRemoveRelationsOperations(IEnumerable<WorkItemRelation>? relations)
        {
            relations ??= new List<WorkItemRelation>();
            var tcmlinks = new List<JsonPatchOperation>();
            var relationsList = relations.ToList();
            for (int position = 0; position < relationsList.Count; position++)
            {
                if (relationsList[position].Rel == AttachedFileRel)
                {
                    tcmlinks.Add(CreateRelationsPatchOperation(position.ToString(), null, Operation.Remove));
                }

                if (relationsList[position].Rel == TestedByReverseRel)
                {
                    tcmlinks.Add(CreateRelationsPatchOperation(position.ToString(), null, Operation.Remove));
                }
            }

            return tcmlinks;
        }

        private static JsonPatchOperation CreateRelationsPatchOperation(string position, object value, Operation operation)
            => new()
            {
                Operation = operation,
                Path = $"/relations/{position}",
                Value = value
            };
    }
}