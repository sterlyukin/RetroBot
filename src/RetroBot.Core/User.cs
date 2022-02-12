﻿namespace RetroBot.Core;

public sealed class User : IWithIssuedId
{
    public long Id { get; set; }
    public UserState State { get; set; }
}