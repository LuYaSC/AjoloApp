﻿using AjoloApp.Core.Business;
using AjoloApp.Model.Collaborator;

namespace AjoloApp.RuleEngine.CollaboratorService
{
    public interface ICollaboratorService
    {
        Result<List<CollaboratorsResult>> GetCollaborators();

        Result<CollaboratorsResult> GetCollaboratorById(CollaboratorByIdDto dto);

        Result<string> CreateCollaborator(CreateCollaboratorDto dto);

        Result<string> UpdateCollaborator(UpdateCollaboratorDto dto);

        Result<string> ActivateOrDeactivate(DeleteDto dto);

        Result<ReportResult> GeneratePdf(string title);
    }
}
