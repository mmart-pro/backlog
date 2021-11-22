namespace backlog.api.DTO;

public record TodoCreateRequest(int ProjectId, int PriorityId, string Title, string Content);