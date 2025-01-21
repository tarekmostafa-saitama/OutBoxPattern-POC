﻿namespace OutBoxPattern_POC.Entities;

public sealed class OutboxMessage
{
	public int Id { get; set; }
	public string Type { get; set; }
	public string Content { get; set; }
	public DateTime OccurredOnUtc { get; set; }
	public DateTime? ProcessedOnUtc { get; set; }
	public string Error { get; set; }
}

