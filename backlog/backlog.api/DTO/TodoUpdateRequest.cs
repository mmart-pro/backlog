namespace backlog.api.DTO;

public record TodoUpdateRequest(int TodoId, int PriorityId, string Title, string Content);