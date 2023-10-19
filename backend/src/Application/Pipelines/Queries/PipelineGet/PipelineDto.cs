﻿using KwikDeploy.Application.Common.Mappings;
using KwikDeploy.Domain.Entities;

namespace KwikDeploy.Application.Pipelines.Queries.PipelineGet;

public class PipelineDto : IMapFrom<Pipeline>
{
    public int Id { get; init; }

    public int ProjectId { get; init; }

    public string Name { get; init; } = null!;
}
